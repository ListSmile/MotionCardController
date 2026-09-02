using MotionController.Core.Entities;
using MotionController.Core.Interface;
using MotionController.ZMotion;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace MotionController.MotionController.ZMotion.Entities
{
    /// <summary>
    /// 正运动控制卡
    /// </summary>
    public class ZMotionController_PCIE : IMotionController
    {
        private bool _connected;
        private string _cardname;
        private string _cardband = "正运动";
        private IntPtr _cardHandle;
        private string _filepath;
        /// <summary>
        /// 正运动的轴字典，key为轴索引（轴号），value为轴对象
        /// </summary>
        private ConcurrentDictionary<int,IAxis> MotionAxisDictionary { get; set; } = new ConcurrentDictionary<int, IAxis>();
        /// <summary>
        /// 正运动的输入字典，key为输入索引（输入号），value为输入对象
        /// </summary>
        private ConcurrentDictionary<int, IInput> MotionInputDictionary { get; set; } = new ConcurrentDictionary<int, IInput>();
        /// <summary>
        /// 正运动的输出字典，key为输出索引（输出号），value为输出对象
        /// </summary>
        private ConcurrentDictionary<int, IOutput> MotionOutputDictionary { get; set; } = new ConcurrentDictionary<int, IOutput>();
        /// <summary>
        /// 正运动的编码器字典，key为编码器索引（编码器号），value为编码器对象
        /// </summary>
        private ConcurrentDictionary<int, IEncoder> MotionEncoderDictionary { get; set; } = new ConcurrentDictionary<int, IEncoder>();
        /// <summary>
        /// 正运动的AD转换器字典，key为AD转换器索引（AD转换器号），value为AD转换器对象
        /// </summary>
        private ConcurrentDictionary<int, IADConverter> MotionADConverterDictionary { get; set; } = new ConcurrentDictionary<int, IADConverter>();
        /// <summary>
        /// 正运动的DA转换器字典，key为DA转换器索引（DA转换器号），value为DA转换器对象
        /// </summary>
        private ConcurrentDictionary<int, IDAConverter> MotionDAConverterDictionary { get; set; } = new ConcurrentDictionary<int, IDAConverter>();

        public ZMotionController_PCIE(string cardname,string zmccfgfilepath)
        {
            _cardname = cardname;
            _filepath = zmccfgfilepath;
        }

        public bool Connected => _connected;

        public string CardName => _cardname;

        public string CardBand => _cardband;
        public IntPtr CardHandle => _cardHandle;

        public void Connect()
        {
            int result = Zmcaux.ZAux_OpenPci(0,out _cardHandle);
            if (result == 0 && _cardHandle != (nint)0) 
            {
                _connected = true;
            }
            else
            {
                _connected = false;
            }
            
        }

        public async Task ConnectAsync()
        {
            await Task.Run(Connect);
        }

        public void Disconnect()
        {
            if (_cardHandle != 0)
            {
                Zmcaux.ZAux_Close(_cardHandle);
                _cardHandle = 0;
            }
            _connected = false; 
        }

        public async Task DisconnectAsync()
        {
            await Task.Run(Disconnect);
        }
        /// <summary>
        /// 获取AD转换器对象，如果不存在则创建一个新的AD转换器对象储存并返回
        /// </summary>
        /// <param name="adConverterIndex"></param>
        /// <returns></returns>
        public IADConverter GetADConverter(int adConverterIndex)
        {
            MotionADConverterDictionary.TryAdd(adConverterIndex, new ZMotionADConverter(this,adConverterIndex,$"默认AD转换器{adConverterIndex}"));
            MotionADConverterDictionary.TryGetValue(adConverterIndex, out IADConverter adConverter);
            return adConverter;
        }
        public IAxis GetAxis(int axisIndex)
        {
            MotionAxisDictionary.TryAdd(axisIndex, new ZMotionAxis(this,axisIndex,$"默认轴{axisIndex}"));
            MotionAxisDictionary.TryGetValue(axisIndex, out IAxis axis);
            return axis;
        }

        public IDAConverter GetDAConverter(int daConverterIndex)
        {
            MotionDAConverterDictionary.TryAdd(daConverterIndex, new ZMotionDAConverter(this,daConverterIndex,$"默认DA转换器{daConverterIndex}"));
            MotionDAConverterDictionary.TryGetValue(daConverterIndex, out IDAConverter daConverter);
            return daConverter;
        }

        public IEncoder GetEncoder(int encoderIndex)
        {
            MotionEncoderDictionary.TryAdd(encoderIndex, new ZMotionEncoder(this,encoderIndex,$"默认编码器{encoderIndex}"));
            MotionEncoderDictionary.TryGetValue(encoderIndex, out IEncoder encoder);
            return encoder;
        }

        public IInput GetInput(int inputIndex)
        {
            MotionInputDictionary.TryAdd(inputIndex, new ZMotionInput(this,$"输入{inputIndex}", inputIndex));
            MotionInputDictionary.TryGetValue(inputIndex, out IInput input);
            return input;
        }

        public IOutput GetOutput(int outputIndex)
        {
            MotionOutputDictionary.TryAdd(outputIndex, new ZMotionOutput(this,$"输出{outputIndex}", outputIndex));
            MotionOutputDictionary.TryGetValue(outputIndex, out IOutput output);
            return output;
        }
        [Obsolete("暂未实现，敬请期待")]
        public IPSOManager GetPSOManager(int psoManagerIndex)
        {
            throw new NotImplementedException();
        }

        public bool Init()
        {
            if (File.Exists(_filepath) == false)
            {
                return false;
            }
            if (_cardHandle == (IntPtr)0)
            {
                return false;
            }
            //将配置文件下载到控制器中，0：表示下载到控制器的ram，1表示下载到控制器的rom
            int result = Zmcaux.ZAux_ZarDown(_cardHandle, _filepath, 0);
            if (result != 0)
            {
                return false;
            }
            Thread.Sleep(6000);
            GetBusInitStatus();
            return true;
        }

        public async Task< bool> InitAsync()
        {
            return await Task.Run(Init);
        }

        /// <summary>
        /// 获取总线初始化状态，返回true表示总线初始化完成，返回false表示总线初始化未完成
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private bool GetBusInitStatus(int timeout = 30)
        {
            CancellationTokenSource cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));
            while (!cancellationToken.IsCancellationRequested)
            {
                int tempstatus = -1;
                int ret = Zmcaux.ZAux_Direct_GetVariableInt(_cardHandle, "EcatInitStatus", ref tempstatus);
                if (ret == 0 && tempstatus != -1)
                {
                    return tempstatus == 1;
                }
                Thread.Sleep(100);
            }
            return false;
        }

        public bool LoadConfig(string configFilePath)
        {
            FileStream fs = File.Create(configFilePath);
            Utf8JsonWriter utf8writer = new Utf8JsonWriter(fs, new JsonWriterOptions { Indented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            JsonSerializer.Serialize(utf8writer,new AxisParaDefine { AxisName = "搬运轴"});
            
            //1.检查文件是否存在
            if (File.Exists(configFilePath) == false)
            {
                return false;
            }
            //2.从文件读取
            try
            {
                var filecfg = JsonSerializer.Deserialize<AxisParaDefine>(configFilePath);
                if (filecfg != null)
                {
                   var currentaxis =  GetAxis(filecfg.AxisIndex);
                    currentaxis.SetAxisName(filecfg.AxisName);

                }
                 
            }
            catch (Exception ex)
            {

            }
            return false;
        }
    }
}
