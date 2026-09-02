using MotionController.Core.Interface;
using MotionController.ZMotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionController.MotionController.ZMotion.Entities
{
    public class ZMotionDAConverter : IDAConverter
    {
        private IMotionController _controller;
        private int _channel;
        private string _name;
        public int Channel => throw new NotImplementedException();

        public ZMotionDAConverter(IMotionController controller, int channel, string name) 
        {
            _controller = controller;
            _channel = channel;
            _name = name;
        }
        public void SetCurrent(double current)
        {
            Zmcaux.ZAux_Direct_SetDA(_controller.CardHandle, _channel, (float)current);
        }

        public void SetVoltage(double voltage)
        {
            Zmcaux.ZAux_Direct_SetDA(_controller.CardHandle, _channel, (float)voltage);
        }
    }
}
