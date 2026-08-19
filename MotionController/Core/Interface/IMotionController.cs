using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionController.Core.Interface
{
    /// <summary>
    /// 控制卡接口，定义了控制卡的基本操作和属性。
    /// </summary>
    public interface IMotionController
    {
        /// <summary>
        /// 是否链接
        /// </summary>
        bool Connected { get;}
        /// <summary>
        /// 控制卡名称
        /// </summary>
        string CardName {  get;}
        /// <summary>
        /// 控制卡品牌
        /// </summary>
        string CardBand {  get;}
        /// <summary>
        /// 卡的句柄    
        /// </summary>
        IntPtr CardHandle { get; }
        /// <summary>
        /// 链接控制卡
        /// </summary>
        void Connect();
        bool Init();
        Task<bool> InitAsync();
        Task ConnectAsync();
        void Disconnect();
        Task DisconnectAsync();
        IAxis GetAxis(int axisIndex);
        IInput GetInput(int inputIndex);
        IOutput GetOutput(int outputIndex);
        IEncoder GetEncoder(int encoderIndex);
        IADConverter GetADConverter(int adConverterIndex);
        IDAConverter GetDAConverter(int daConverterIndex);
        IPSOManager GetPSOManager(int psoManagerIndex);
    }
}
