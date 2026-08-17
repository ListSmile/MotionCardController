using MotionController.Core.Enums;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionController.Core.Entities
{
    public class ConnectConfig
    {
        public ConnectConfig()
        {
            Ip = "127.0.0.1";
            Port = 502;

            COM = "COM1";
            UseSerial = false;
            BaudRate = 9600;
            Parity = ParityEnum.None;
            DataBits = 8;
            StopBits = StopBitsEnum.One;
            ReadTimeout = 3000;
            WriteTimeout = 3000;
        }

        /// <summary>
        /// 目标主机的 IP 地址，默认为 "127.0.0.1"。
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 目标主机的端口号，默认为 502。
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 指示是否使用串口连接。为 true 时使用串口，为 false 时使用网络。
        /// </summary>
        public bool UseSerial { get; set; }

        /// <summary>
        /// 串口名称，例如 "COM1"。
        /// </summary>
        public string COM { get; set; }

        /// <summary>
        /// 串口波特率，整数值，单位为波特（baud）。
        /// </summary>
        public int BaudRate { get; set; }

        /// <summary>
        /// 串口校验位枚举值（ParityEnum）。
        /// </summary>
        public ParityEnum Parity { get; set; }

        /// <summary>
        /// 串口数据位（例如 7 或 8）。
        /// </summary>
        public int DataBits { get; set; }

        /// <summary>
        /// 串口停止位枚举值（StopBitsEnum）。
        /// </summary>
        public StopBitsEnum StopBits { get; set; }

        /// <summary>
        /// 读超时，单位为毫秒。-1 表示无限等待。
        /// </summary>
        public int ReadTimeout { get; set; }

        /// <summary>
        /// 写超时，单位为毫秒。-1 表示无限等待。
        /// </summary>
        public int WriteTimeout { get; set; }
    }
}
