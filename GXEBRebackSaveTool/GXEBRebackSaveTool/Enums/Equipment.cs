using System.ComponentModel;

namespace GXEBRebackSaveTool.Enums
{
    /// <summary>
    /// 具体设备参数
    /// </summary>
    public enum Equipment
    {
        /// <summary>
        /// 开机（专用）
        /// </summary>
        [Description("开机（专用）")]
        Open = 0x0060,
        /// <summary>
        /// 关机（专用）
        /// </summary>
        [Description("关机（专用）")]
        Close = 0x0061,
        /// <summary>
        /// 位操作(开\合\置位)
        /// </summary>
        [Description(@"位操作(开\合\置位)")]
        BitOperationOpen = 0x0062,
        /// <summary>
        /// 位操作(关\分\清位)
        /// </summary>
        [Description(@"位操作(关\分\清位)")]
        BitOperationClose = 0x0063,
        /// <summary>
        /// 位操作(非\反转)
        /// </summary>
        [Description(@"位操作(非\反转)")]
        BitOperationUn = 0x0064,
        /// <summary>
        /// 增量（加）
        /// </summary>
        [Description("增量（加）Byte")]
        ByteOpAddition = 0x0065,
        /// <summary>
        /// 减量（减）
        /// </summary>
        [Description("减量（减）Byte")]
        ByteOpSubtraction = 0x0066,
        /// <summary>
        /// 增量（加）
        /// </summary>
        [Description("增量（加）UShort")]
        UShortOpAddition = 0x0067,
        /// <summary>
        /// 减量（减）
        /// </summary>
        [Description("减量（减）UShort")]
        UShortOpSubtraction = 0x0068,
        /// <summary>
        /// 增量（加）
        /// </summary>
        [Description("增量（加）Single")]
        SingleOpAddition = 0x0069,
        /// <summary>
        /// 减量（减）
        /// </summary>
        [Description("减量（减）Single")]
        SingleOpSubtraction = 0x006A,


        /// <summary>
        /// 调频频点列表
        /// </summary>
        [Description("调频频点列表")]
        FMFreList = 0x0301,
        /// <summary>
        /// 调频信号强度
        /// </summary>
        [Description("调频信号强度")]
        FMSignalStrength = 0x0302,
        /// <summary>
        /// 播放类型
        /// </summary>
        [Description("播放类型")]
        PlayType = 0x0303,
        /// <summary>
        /// 软硬件版本号
        /// </summary>
        [Description("软硬件版本号")]
        Versions = 0x0304,
        /// <summary>
        /// 数字电视射频模式
        /// </summary>
        [Description("数字电视射频模式")]
        DigitalTVRadioFrequencyMode = 0x0305,
        /// <summary>
        /// 数字电视射频频率
        /// </summary>
        [Description("数字电视射频频率")]
        DigitalTVRadioFrequencyFre = 0x0306,
        /// <summary>
        /// QAM 方式
        /// </summary>
        [Description("QAM方式")]
        QAMType = 0x0307,
        /// <summary>
        /// 广播音量大小
        /// </summary>
        [Description("广播音量大小")]
        BroadcastVolume = 0x0308,
        /// <summary>
        /// 广播超时时间
        /// </summary>
        [Description("广播超时时间")]
        BroadcastTimeout = 0x0309,
        /// <summary>
        /// 当前模式下信号质量
        /// </summary>
        [Description("当前模式下信号质量")]
        CurrentModeSignalQuality = 0x0310,
        /// <summary>
        /// 当前模式下信号强度
        /// </summary>
        [Description("当前模式下信号强度")]
        CurrentModeSignalStrength = 0x0311,
        /// <summary>
        /// 远端控制中心IP地址
        /// </summary>
        [Description("远端控制中心IP地址")]
        RemoteControlCenterIPAddress = 0x0312,
        /// <summary>
        /// 远端控制中心服务端口
        /// </summary>
        [Description("远端控制中心服务端口")]
        RemoteControlCenterPort = 0x0313,
        /// <summary>
        /// 音频服务器IP地址
        /// </summary>
        [Description("音频服务器IP地址")]
        AudioServerIPAddress = 0x0314,
        /// <summary>
        /// 音频服务端口
        /// </summary>
        [Description("音频服务端口")]
        AudioServerPort = 0x0315,
        /// <summary>
        /// 喊话方式
        /// </summary>
        [Description("喊话方式")]
        CallWay = 0x0316,
        /// <summary>
        /// 文件名
        /// </summary>
        [Description("文件名")]
        FileName = 0x0317,
        /// <summary>
        /// 时长
        /// </summary>
        [Description("时长")]
        RecordingDuration = 0x0318,
        /// <summary>
        /// 文件包数
        /// </summary>
        [Description("文件包数")]
        PacksTotalNumber = 0x0319,
        /// <summary>
        /// 回传文件类型
        /// </summary>
        [Description("回传文件类型")]
        RebackFileType = 0x0320,
        /// <summary>
        /// 起始包号
        /// </summary>
        [Description("起始包号")]
        PackStartIndex = 0x0321,
        /// <summary>
        /// 连续包个数
        /// </summary>
        [Description("连续包个数")]
        LastPacksNuber = 0x0322,
        /// <summary>
        /// 逻辑地址
        /// </summary>
        [Description("逻辑地址")]
        LogicalAddress = 0x0323,
        /// <summary>
        /// 物理地址
        /// </summary>
        [Description("物理地址")]
        PhysicalAddress = 0x0324,
        /// <summary>
        /// 广播状态
        /// </summary>
        [Description("广播状态")]
        BroadcastState = 0x0325,
        /// <summary>
        /// 终端类型
        /// </summary>
        [Description("终端类型")]
        TerminalType = 0x0326,
        /// <summary>
        /// 经度
        /// </summary>
        [Description("经度")]
        Longitude = 0x0327,
        /// <summary>
        /// 纬度
        /// </summary>
        [Description("纬度")]
        Latitude = 0x0328,
        /// <summary>
        /// 回传模式
        /// </summary>
        [Description("回传模式")]
        RebackMode = 0x0329,
        /// <summary>
        /// 网络模式
        /// </summary>
        [Description("网络模式")]
        NetworkMode = 0x0330,
        /// <summary>
        /// 220V 电压
        /// </summary>
        [Description("220V 电压")]
        Voltage220 = 0x0331,
        /// <summary>
        /// 24V 电压
        /// </summary>
        [Description("24V 电压")]
        Voltage24 = 0x0332,
        /// <summary>
        /// 12V 电压
        /// </summary>
        [Description("12V 电压")]
        Voltage12 = 0x0333,
        /// <summary>
        /// 功放电流
        /// </summary>
        [Description("功放电流")]
        AmplifierElectricCurrent = 0x0334,
        /// <summary>
        /// 本机IP地址
        /// </summary>
        [Description("本机IP地址")]
        LocalHost = 0x0335,
        /// <summary>
        /// 子网掩码
        /// </summary>
        [Description("子网掩码")]
        SubnetMask = 0x0336,
        /// <summary>
        /// 默认网关
        /// </summary>
        [Description("默认网关")]
        DefaultGateway = 0x0337,

        /// <summary>
        /// 音频回传
        /// </summary>
        [Description("音频回传")]
        AudioReback = 0x0338,
        /// <summary>
        /// 音频回传协议
        /// </summary>
        [Description("音频回传协议")]
        AudioRebackProtocol = 0x0339,
        /// <summary>
        /// 录音类别
        /// </summary>
        [Description("录音类别")]
        RecordingCategory = 0x0340,
        /// <summary>
        /// 起始包序号     异常
        /// </summary>
        [Description("起始包序号")]
        StartPackageNumber = 0x03999,

        /// <summary>
        /// 终端和服务器握手
        /// </summary>
        [Description("终端和服务器握手")]
        terminal2serverhandshake = 0x0341,

        /// <summary>
        /// 升级
        /// </summary>
        [Description("在线升级")]
        OnlineUpdate  = 0x0342,

        /// <summary>
        /// 文本转语音
        /// </summary>
        [Description("文本转语音")]
        TTS = 0x0344,
    }
}
