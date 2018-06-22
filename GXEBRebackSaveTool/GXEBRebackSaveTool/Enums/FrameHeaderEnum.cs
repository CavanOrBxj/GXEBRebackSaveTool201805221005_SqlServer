using System.ComponentModel;

namespace GXEBRebackSaveTool.Enums
{
    enum FrameHeaderEnum
    {
        /// <summary>
        /// 应急广播资源编码
        /// </summary>
        [Description("应急广播资源编码")]
        EBResourceCoding,
        /// <summary>
        /// 协议版本号
        /// </summary>
        [Description("协议版本号")]
        ProtocolVersion,
        /// <summary>
        /// 厂家编号
        /// </summary>
        [Description("厂家编号")]
        FactoryNumber,
        /// <summary>
        /// 硬件版本号
        /// </summary>
        [Description("硬件版本号")]
        HardwareVersion,
        /// <summary>
        /// 软件版本号
        /// </summary>
        [Description("软件版本号")]
        SoftwareVersion,
    }
}
