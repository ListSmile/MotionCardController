using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionController.Core.Entities
{
    /// <summary>
    /// 轴运动参数（不可变、线程安全）
    /// </summary>
    /// <param name="Position">目标位置（工程单位：mm 或 deg）</param>
    /// <param name="Velocity">速度（单位/s）</param>
    /// <param name="Acceleration">加速度（单位/s²），默认值通常由驱动器内部设定</param>
    /// <param name="Deceleration">减速度（单位/s²），默认为加速度的 1.2 倍防止急停冲击</param>
    /// <param name="Jerk">加加速度（单位/s³），用于 S 型曲线规划，默认 0 表示梯形曲线</param>
    public record MoveOptions(double Position, double Velocity,
        double Acceleration = 0,        // 0 通常代表使用驱动器默认值
        double? Deceleration = null,    // null 代表与 Acceleration 一致
        double Jerk = 0)
    {
        // 内部计算实际使用的减速度
        public double EffectiveDeceleration => Deceleration ?? Acceleration * 1.2;
    };
}
