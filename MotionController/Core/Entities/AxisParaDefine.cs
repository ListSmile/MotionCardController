using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MotionController.Core.Entities
{
    [JsonSerializable(typeof(AxisParaDefine))]
    public class AxisParaDefine
    {
        public AxisParaDefine() 
        {
            
        }
        /// <summary>
        /// 轴名称
        /// </summary>
        public string AxisName { get; set; } = string.Empty;
        /// <summary>
        /// 轴号显示（-1代表，该轴没有被参数初始化）
        /// </summary>
        public int AxisIndex { get; set; }
        /// <summary>
        /// 回零模式
        /// </summary>
        public int HomeMode { get; set; }
        /// <summary>
        /// 原点输入索引(-1代表没有进行分配原点)
        /// </summary>
        public int OriginInputIndex { get; set; }=-1;
        /// <summary>
        /// 原点输入电平
        /// </summary>
        public InputVoltageLevel OriginInputVoltageLevel { get; set; } = InputVoltageLevel.低电平有效;
        /// <summary>
        /// 正极限输入索引
        /// </summary>
        public int PositiveLimitInputIndex { get; set; } = -1;
        /// <summary>
        /// 正极限输入电平
        /// </summary>
        public InputVoltageLevel PositiveLimitVoltageLevel { get; set; } = InputVoltageLevel.低电平有效;
        /// <summary>
        /// 负极限输入索引(-1为无效)
        /// </summary>
        public int NegativeLimitInputIndex { get; set; } = -1;
        /// <summary>
        /// 负极限输入电平
        /// </summary>
        public InputVoltageLevel NegativeLimitVoltageLevel { get; set; } = InputVoltageLevel.低电平有效;
        /// <summary>
        /// 单圈脉冲
        /// </summary>
        public float PPR { get; set; } = 10000;
        /// <summary>
        /// 单圈行程
        /// </summary>
        public float SPR { get; set; } = 10;
        /// <summary>
        /// 是否启用软限位
        /// </summary>
        public SwitchType SoftLimit { get; set; } = SwitchType.无效;
        /// <summary>
        /// 软正限位
        /// </summary>
        public float PositiveSoftLimit { get; set; } = -99999;
        /// <summary>
        /// 软负限位
        /// </summary>
        public float NegativeSoftLimit { get; set; } = 99999;
    }
    public enum InputVoltageLevel
    {
        高电平有效 = 1,
        低电平有效 = 0
    }
    public enum SwitchType
    {
        无效,
        有效,
    }
}
