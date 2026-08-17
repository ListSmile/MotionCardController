using MotionController.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionController.MotionController.ZMotion.Entities
{
    public class ZMotionController : IMotionController
    {
        private bool _connected;
        private string _cardname;
        private string _cardband;
        public ZMotionController(string cardname, string cardband)
        {
            _cardname = cardname;
            _cardband = cardband;
        }

        public bool Connected => _connected;

        public string CardName => _cardname;

        public string CardBand => _cardband;

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
            throw new NotImplementedException();
        }

        public IADConverter GetADConverter(int adConverterIndex)
        {
            throw new NotImplementedException();
        }

        public IAxis GetAxis(int axisIndex)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public IOutput GetOutput(int outputIndex)
        {
            throw new NotImplementedException();
        }

        public IPSOManager GetPSOManager(int psoManagerIndex)
        {
            throw new NotImplementedException();
        }
    }
}
