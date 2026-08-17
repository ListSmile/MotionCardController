using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionController.Core.Interface
{
    public interface IEncoder
    {
        int Channel { get; }
        double Position { get; }
        void ResetPosition(double position = 0);
    }
}
