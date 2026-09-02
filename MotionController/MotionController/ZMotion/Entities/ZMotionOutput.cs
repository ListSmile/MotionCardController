using MotionController.Core.Interface;
using MotionController.ZMotion;

namespace MotionController.MotionController.ZMotion.Entities
{
    /// <summary>
    /// 正运动输出口类
    /// </summary>
    public class ZMotionOutput : IOutput
    {
        private string _name;
        private int _indexport;
        private IntPtr _handle;
        public ZMotionOutput(IMotionController motionController,string name, int indexport)
        {
            _name = name;
            _indexport = indexport;
            _handle = motionController.CardHandle;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name => _name;
        /// <summary>
        /// 输出口序号
        /// </summary>
        public int IndexPort => _indexport;
        /// <summary>
        /// 当前输出口的状态
        /// </summary>
        public bool Value { get => GetValue(); set => SetValue(value); }

        private bool GetValue()
        {
            uint val = 0;
            int ret = Zmcaux.ZAux_Direct_GetOp(_handle, _indexport, ref val);
            if (ret != 0)
            {
                return false;
            }
            return val > 0;
        }
        /// <summary>
        /// 关闭输出
        /// </summary>
        public void OFF()
        {
            SetValue(false);
        }
        /// <summary>
        /// 打开输出
        /// </summary>
        public void ON()
        {
            SetValue(true);
        }
        /// <summary>
        /// 值反转
        /// </summary>
        public void Revert()
        {
            Value = !Value;
        }
        /// <summary>
        /// 设置占空比和频率
        /// </summary>
        /// <param name="dutyCycle"></param>
        /// <param name="frequency"></param>
        /// <exception cref="Exception"></exception>
        public void SetPWM(double dutyCycle, double frequency)
        {
            int ret = Zmcaux.ZAux_Direct_SetPwmFreq(_handle, _indexport, (float)frequency);
            ret += Zmcaux.ZAux_Direct_SetPwmDuty(_handle, _indexport, (float)dutyCycle);
            if (ret != 0)
            {
                throw new Exception($"ZMotion_Output_SetPWMFail_Code:{ret}");
            }
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="Exception"></exception>
        public void SetValue(bool value)
        {
            int ret = Zmcaux.ZAux_Direct_SetOp(_handle, _indexport, value ? (uint)1 : 0);
            if (ret != 0)
            {
                throw new Exception($"ZMotion_Output_SetFail_Code:{ret}");
            }
        }

        public void SetName(string name)
        {
            _name = name;
        }
    }
}
