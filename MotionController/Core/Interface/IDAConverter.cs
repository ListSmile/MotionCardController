using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionController.Core.Interface
{
    public interface IDAConverter
    {
        int Channel { get; }
        void SetVoltage(double voltage);
        void SetCurrent(double current);
    }
}
