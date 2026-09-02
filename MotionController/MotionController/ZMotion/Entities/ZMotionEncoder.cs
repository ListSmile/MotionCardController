using MotionController.Core.Interface;
using MotionController.ZMotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionController.MotionController.ZMotion.Entities
{
    public class ZMotionEncoder : IEncoder
    {
        private IMotionController _controller;
        private int _channel;
        private string _name;
        public ZMotionEncoder(IMotionController controller, int channel, string name) 
        {
            _controller = controller;
            _channel = channel;
            _name = name;
        }
        public int Channel => _channel;

        public double Position => GetPosition();

        private double GetPosition() 
        {
            float position = 0;
            Zmcaux.ZAux_Direct_GetEncoder(_controller.CardHandle, _channel, ref position);
            return position;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        [Obsolete("暂未实现")]
        public void ResetPosition(double position = 0)
        {
            
        }
    }
}
