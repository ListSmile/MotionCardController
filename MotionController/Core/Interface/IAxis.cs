using MotionController.Core.Entities;
using MotionController.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionController.Core.Interface
{
    public interface IAxis
    {
        IAxisParam AxisParam { get; }
        string Name { get; }
        double Position { get; }
        bool Enable { get; }
        AxisStatus Status { get; }
        void Move_Abs(MoveOptions options);
        void Move_Rel(MoveOptions options);
        void Move_Continuous(MoveOptions options);
        void Stop();
        void Alarm_Reset();
    }
}
