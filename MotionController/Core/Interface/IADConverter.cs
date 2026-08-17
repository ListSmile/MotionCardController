using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionController.Core.Interface
{
    public interface IADConverter
    {
        int Channel { get; }
        double ReadVoltage();
        double ReadCurrent();
    }
}
