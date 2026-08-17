using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionController.Core.Interface
{
    public interface IInput
    {
        string Name { get; }
        int IndexPort { get; }
        bool Value { get; }
    }
}
