using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionController.Core.Enums
{
    /// <summary>
    /// 串口校验位枚举，值与 System.IO.Ports.Parity 保持一致。
    /// </summary>
    public enum ParityEnum
    {
        /// <summary>
        /// 无校验
        /// </summary>
        None = 0,

        /// <summary>
        /// 奇校验
        /// </summary>
        Odd = 1,

        /// <summary>
        /// 偶校验
        /// </summary>
        Even = 2,

        /// <summary>
        /// 标记校验
        /// </summary>
        Mark = 3,

        /// <summary>
        /// 空格校验
        /// </summary>
        Space = 4,
    }
    public enum StopBitsEnum
    {
        /// <summary>
        /// 1 个停止位
        /// </summary>
        One = 1,
        /// <summary>
        /// 1.5 个停止位
        /// </summary>
        OnePointFive = 3,
        /// <summary>
        /// 2 个停止位
        /// </summary>
        Two = 2,
    }
}
