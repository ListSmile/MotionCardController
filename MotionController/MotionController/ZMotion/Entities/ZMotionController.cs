using MotionController.Core.Interface;
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
    public class ZMotionController : IMotionController
    {
        private bool _connected;
        private string _cardname;
        private string _cardband;
        private IntPtr _cardHandle;
        public ZMotionController(string cardname, string cardband)
        {
            _cardname = cardname;
            _cardband = cardband;
        }

        public bool Connected => _connected;

        public string CardName => _cardname;

        public string CardBand => _cardband;
        public IntPtr CardHandle => _cardHandle;

        public void Connect()
        {
            _connected = true;
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
            return new ZMotionAxis(axisIndex,$"默认轴{axisIndex}", _cardHandle);
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
            return new ZMotionInput($"输入{inputIndex}",inputIndex,_cardHandle);
        }

        public IOutput GetOutput(int outputIndex)
        {
            return new ZMotionOutput($"输出{outputIndex}", outputIndex, _cardHandle);
        }

        public IPSOManager GetPSOManager(int psoManagerIndex)
        {
            throw new NotImplementedException();
        }
    }
}
