namespace MotionController.Core.Enums
{
    public enum AxisStatus
    {
        /// <summary>未知状态，可能是驱动器未初始化或其他异常情况</summary>
        Unknown = 0,
        /// <summary>已使能，空闲就绪（绿色常亮）</summary>
        Ready = 1,
        /// <summary>未使能（灰色/暗色）</summary>
        Disabled = 2,
        /// <summary>回零中（黄色闪烁）</summary>
        Homing = 3,
        /// <summary>运动中（蓝色/青色呼吸）</summary>
        Moving = 4,
        /// <summary>点动中（浅蓝）</summary>
        Jogging = 5,
        /// <summary>驱动器报警（红色常亮）</summary>
        Alarm = 6,
        /// <summary>急停触发（红色快闪）</summary>
        EmergencyStop = 7,
        /// <summary>正限位触发（橙色）</summary>
        PositiveLimit = 8,
        /// <summary>负限位触发（橙色）</summary>
        NegativeLimit = 9,
        /// <summary>正软限位触发（橙色）</summary>
        SoftPositiveLimit = 10,
        /// <summary>负软限位触发（橙色）</summary>
        SoftNegativeLimit = 11,
    }
}
