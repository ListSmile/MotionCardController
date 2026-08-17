using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MotionController.Core.Interface
{
    /// <summary>
    /// 轴的参数接口，定义了轴的基本参数属性
    /// </summary>
    public interface IAxisParam
    {
        /// <summary>
        /// 单圈指令数（脉冲，指令单位）
        /// </summary>
        uint CountsPerRevolution { get; set; }
        /// <summary>
        /// 单圈形成的实际位移量（单位：mm、度、弧度等）
        /// </summary>
        double FeedPerRevolution { get; set; }
    }
}
