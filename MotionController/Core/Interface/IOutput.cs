using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionController.Core.Interface
{
    public interface IOutput
    {
        string Name { get; }
        int IndexPort { get; }
        bool Value { get; set; }
        void SetValue(bool value);
        void Revert();
        void SetPWM(double dutyCycle, double frequency);
        void ON();
        void OFF();
    }
}
