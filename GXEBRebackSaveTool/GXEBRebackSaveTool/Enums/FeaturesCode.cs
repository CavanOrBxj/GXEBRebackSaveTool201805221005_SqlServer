using System.ComponentModel;

namespace GXEBRebackSaveTool.Enums
{
    /// <summary>
    /// 功能码
    /// </summary>
    public enum FeaturesCode : byte
    {
        /// <summary>
        /// 读命令
        /// </summary>
        [Description("读命令")]
        Read = 0x03,
        /// <summary>
        /// 写命令
        /// </summary>
        [Description("写命令")]
        Write = 0x10,
        /// <summary>
        /// 写命令错误响应帧体
        /// </summary>
        [Description("写命令错误响应帧体")]
        WriteError = 0x90,
        /// <summary>
        /// 主动上报
        /// </summary>
        [Description("主动上报")]
        AutoReport = 0x65,
        /// <summary>
        /// 警报主动上报
        /// </summary>
        [Description("警报主动上报")]
        AlertAutoReport = 0x66,
        /// <summary>
        /// 心跳帧
        /// </summary>
        [Description("心跳帧")]
        AutoHeartBeat = 0x56,
        /// <summary>
        /// 搜索命令
        /// </summary>
        [Description("搜索命令")]
        Search = 0x5A,

        /// <summary>
        /// 无命令
        /// </summary>
        [Description("无命令")]
        None = 0x00,
    }
}
