using MotionController.Core.Entities;
using MotionController.Core.Enums;
using MotionController.Core.Interface;
using MotionController.ZMotion;

namespace MotionController.MotionController.ZMotion.Entities
{
    public class ZMotionAxis : IAxis
    {
        private int _axisindex;
        private string _axisname;
        private IntPtr _cardHandle;
        public ZMotionAxis(IMotionController motionController,int axisindex, string axisname)
        {
            _axisindex = axisindex;
            _axisname = axisname;
            _cardHandle = motionController.CardHandle;
        }
        public IAxisParam AxisParam => throw new NotImplementedException();

        public string Name => _axisname;

        public double Position => GetPosition();
        private double GetPosition()
        {
            //调用ZMotion的API获取轴的位置
            float position = 0;
            int ret = Zmcaux.ZAux_Direct_GetDpos(_cardHandle, _axisindex, ref position);
            if (ret != 0)
            {
                return 0; // 获取失败，返回0
            }
            return position;
        }

        public bool Enable => GetEnable();
        private bool GetEnable()
        {
            //调用ZMotion的API获取轴的使能状态
            int enable = 0;
            int ret = Zmcaux.ZAux_Direct_GetAxisEnable(_cardHandle, _axisindex, ref enable);
            if (ret != 0)
            {
                return false; // 获取失败，返回false
            }
            return enable != 0;
        }
        public AxisStatus Status => GetStatus();
        private AxisStatus GetStatus()
        {
            //调用ZMotion的API获取轴的状态
            int status = 0;
            int ret = Zmcaux.ZAux_Direct_GetAxisStatus(_cardHandle, _axisindex, ref status);
            if (ret != 0)
            {
                return AxisStatus.Unknown; // 获取失败，返回未知状态
            }
            return ParseStatusWord(status);
        }
        private AxisStatus ParseStatusWord(int word)
        {
            // 优先级：最高优先级的异常状态最先判断
            // 急停 > 报警 > 限位 > 回零 > 点动 > 运动 > 使能 > 未知

            // 急停（假设 Bit5 为急停输入）
            if ((word & 1 << 22) != 0)
                return AxisStatus.EmergencyStop;

            // 驱动器报警（Bit4）
            if ((word & 1 << 3) != 0)
                return AxisStatus.Alarm;

            // 硬件正限位（Bit2）
            if ((word & 1 << 4) != 0)
                return AxisStatus.PositiveLimit;

            // 硬件负限位（Bit3）
            if ((word & 1 << 5) != 0)
                return AxisStatus.NegativeLimit;

            // 回零中（Bit6）
            if ((word & 1 << 6) != 0)
                return AxisStatus.Homing;

            // 未使能
            return AxisStatus.Disabled;
        }
        public int AxisIndex => _axisindex;
        [Obsolete("当前指令调用无效")]
        public void Alarm_Reset()
        {

        }
        /// <summary>
        /// 轴绝对运动
        /// </summary>
        /// <param name="options"></param>
        /// <exception cref="Exception"></exception>
        public void Move_Abs(MoveOptions options)
        {
            int ret = Zmcaux.ZAux_Direct_Single_MoveAbs(_cardHandle, _axisindex, options.destpos);
            if (ret != 0)
            {
                throw new Exception($"Move_Abs failed with error code: {ret}");
            }
        }
        /// <summary>
        /// 轴连续运动
        /// </summary>
        /// <param name="options"></param>
        /// <exception cref="Exception"></exception>
        public void Move_Continuous(MoveOptions options)
        {
            int ret = Zmcaux.ZAux_Direct_Single_Vmove(_cardHandle, _axisindex, options.Direction);
            if (ret != 0)
            {
                throw new Exception($"Move_Abs failed with error code: {ret}");
            }
        }
        /// <summary>
        /// 轴相对运动
        /// </summary>
        /// <param name="options"></param>
        /// <exception cref="Exception"></exception>
        public void Move_Rel(MoveOptions options)
        {
            int ret = Zmcaux.ZAux_Direct_Single_Move(_cardHandle, _axisindex, options.destpos);
            if (ret != 0)
            {
                throw new Exception($"Move_Rel failed with error code: {ret}");
            }
        }
        /// <summary>
        /// 轴停止
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Stop()
        {
            int ret = Zmcaux.ZAux_Direct_Single_Cancel(_cardHandle, _axisindex, 2);
            if (ret != 0)
            {
                throw new Exception($"Stop failed with error code: {ret}");
            }
        }

        public void SetAxisName(string name)
        {
            _axisname = name;
        }
    }
}
