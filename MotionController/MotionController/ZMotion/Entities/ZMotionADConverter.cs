using MotionController.Core.Interface;
using MotionController.ZMotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionController.MotionController.ZMotion.Entities
{
    public class ZMotionADConverter : IADConverter
    {
        private IMotionController _controller;
        private int _channel;
        private string _name;

        public ZMotionADConverter(IMotionController controller, int channel, string name)
        {
            _controller = controller;
            _channel = channel;
            _name = name;
        }

        public int Channel => _channel;

        public double ReadCurrent()
        {
            float value = 0;
            Zmcaux.ZAux_Direct_GetAD(_controller.CardHandle, _channel, ref value);
            return value;
        }
        [Obsolete("该方法未实现")]
        public double ReadVoltage()
        {
            return 0;
        }
    }
}
