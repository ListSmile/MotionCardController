using MotionController.Core.Interface;
using MotionController.ZMotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Task ConnectAsync()
        {
            //这里进行异步连接操作，返回一个Task对象
            return Task.CompletedTask;
        }

        public void Disconnect()
        {
            _connected = false; 
        }

        public Task DisconnectAsync()
        {
            _connected = false;
            return Task.CompletedTask;
        }

        public IADConverter GetADConverter(int adConverterIndex)
        {
            throw new NotImplementedException();
        }

        public IAxis GetAxis(int axisIndex)
        {
            return new ZMotionAxis(this,axisIndex,$"默认轴{axisIndex}");
        }

        public IDAConverter GetDAConverter(int daConverterIndex)
        {
            throw new NotImplementedException();
        }

        public IEncoder GetEncoder(int encoderIndex)
        {
            throw new NotImplementedException();
        }

        public IInput GetInput(int inputIndex)
        {
            return new ZMotionInput(this,$"输入{inputIndex}",inputIndex);
        }

        public IOutput GetOutput(int outputIndex)
        {
            return new ZMotionOutput(this,$"输出{outputIndex}", outputIndex);
        }

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

        public Task< bool> InitAsync()
        {
            return Task.Run(Init);
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
    }
}
