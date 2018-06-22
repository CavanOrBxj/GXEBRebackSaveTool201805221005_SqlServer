namespace GXEBRebackSaveTool.Models
{
    /// <summary>
    /// 具体设备参数缓存类
    /// </summary>
    public class EquipmentDetail
    {
        /// <summary>
        /// 调频频点列表
        /// </summary>
        public byte[] FMFreList { get; set; }
        private string _FMFreListFormat = string.Empty;
        public string FMFreListFormat { get { return _FMFreListFormat; } set { _FMFreListFormat = value; } }
        /// <summary>
        /// 调频信号强度
        /// </summary>
        public byte[] FMSignalStrength { get; set; }
        private string _FMSignalStrengthFormat = string.Empty;
        public string FMSignalStrengthFormat { get { return _FMSignalStrengthFormat; } set { _FMSignalStrengthFormat = value; } }
        /// <summary>
        /// 播放类型
        /// </summary>
        public byte PlayType { get; set; }
        private string _PlayTypeFormat = string.Empty;
        public string PlayTypeFormat { get { return _PlayTypeFormat; } set { _PlayTypeFormat = value; } }
        /// <summary>
        /// 软硬件版本号
        /// </summary>
        public byte[] Versions { get; set; }
        private string _VersionsFormat = string.Empty;
        public string VersionsFormat { get { return _VersionsFormat; } set { _VersionsFormat = value; } }
        /// <summary>
        /// 数字电视射频模式
        /// </summary>
        public byte DigitalTVRadioFrequencyMode { get; set; }
        private string _DigitalTVRadioFrequencyModeFormat = string.Empty;
        public string DigitalTVRadioFrequencyModeFormat { get { return _DigitalTVRadioFrequencyModeFormat; } set { _DigitalTVRadioFrequencyModeFormat = value; } }
        /// <summary>
        /// 数字电视射频频率
        /// </summary>
        public uint DigitalTVRadioFrequencyFre { get; set; }
        private string _DigitalTVRadioFrequencyFreFormat = string.Empty;
        public string DigitalTVRadioFrequencyFreFormat { get { return _DigitalTVRadioFrequencyFreFormat; } set { _DigitalTVRadioFrequencyFreFormat = value; } }
        /// <summary>
        /// QAM 方式
        /// </summary>
        public byte QAMType { get; set; }
        private string _QAMTypeFormat = string.Empty;
        public string QAMTypeFormat { get { return _QAMTypeFormat; } set { _QAMTypeFormat = value; } }
        /// <summary>
        /// 广播音量大小
        /// </summary>
        public byte BroadcastVolume { get; set; }
        private string _BroadcastVolumeFormat = string.Empty;
        public string BroadcastVolumeFormat { get { return _BroadcastVolumeFormat; } set { _BroadcastVolumeFormat = value; } }
        /// <summary>
        /// 广播超时时间
        /// </summary>
        public byte BroadcastTimeout { get; set; }
        private string _BroadcastTimeoutFormat = string.Empty;
        public string BroadcastTimeoutFormat { get { return _BroadcastTimeoutFormat; } set { _BroadcastTimeoutFormat = value; } }
        /// <summary>
        /// 当前模式下信号质量
        /// </summary>
        public byte CurrentModeSignalQuality { get; set; }
        private string _CurrentModeSignalQualityFormat = string.Empty;
        public string CurrentModeSignalQualityFormat { get { return _CurrentModeSignalQualityFormat; } set { _CurrentModeSignalQualityFormat = value; } }
        /// <summary>
        /// 当前模式下信号强度
        /// </summary>
        public byte CurrentModeSignalStrength { get; set; }
        private string _CurrentModeSignalStrengthFormat = string.Empty;
        public string CurrentModeSignalStrengthFormat { get { return _CurrentModeSignalStrengthFormat; } set { _CurrentModeSignalStrengthFormat = value; } }
        /// <summary>
        /// 远端控制中心IP地址
        /// </summary>
        public byte[] RemoteControlCenterIPAddress { get; set; }
        private string _RemoteControlCenterIPAddressFormat = string.Empty;
        public string RemoteControlCenterIPAddressFormat { get { return _RemoteControlCenterIPAddressFormat; } set { _RemoteControlCenterIPAddressFormat = value; } }
        /// <summary>
        /// 远端控制中心服务端口
        /// </summary>
        public ushort RemoteControlCenterPort { get; set; }
        private string _RemoteControlCenterPortFormat = string.Empty;
        public string RemoteControlCenterPortFormat { get { return _RemoteControlCenterPortFormat; } set { _RemoteControlCenterPortFormat = value; } }
        /// <summary>
        /// 音频服务器IP地址
        /// </summary>
        public byte[] AudioServerIPAddress { get; set; }
        private string _AudioServerIPAddressFormat = string.Empty;
        public string AudioServerIPAddressFormat { get { return _AudioServerIPAddressFormat; } set { _AudioServerIPAddressFormat = value; } }
        /// <summary>
        /// 音频服务端口
        /// </summary>
        public ushort AudioServerPort { get; set; }
        private string _AudioServerPortFormat = string.Empty;
        public string AudioServerPortFormat { get { return _AudioServerPortFormat; } set { _AudioServerPortFormat = value; } }
        /// <summary>
        /// 喊话方式
        /// </summary>
        public byte CallWay { get; set; }
        private string _CallWayFormat = string.Empty;
        public string CallWayFormat { get { return _CallWayFormat; } set { _CallWayFormat = value; } }
        /// <summary>
        /// 文件名
        /// </summary>
        public byte[] FileName { get; set; }
        private string _FileNameFormat = string.Empty;
        public string FileNameFormat { get { return _FileNameFormat; } set { _FileNameFormat = value; } }
        /// <summary>
        /// 时长
        /// </summary>
        public uint RecordingDuration { get; set; }
        private string _RecordingDurationFormat = string.Empty;
        public string RecordingDurationFormat { get { return _RecordingDurationFormat; } set { _RecordingDurationFormat = value; } }
        /// <summary>
        /// 文件包数
        /// </summary>
        public uint PacksTotalNumber { get; set; }
        private string _PacksTotalNumberFormat = string.Empty;
        public string PacksTotalNumberFormat { get { return _PacksTotalNumberFormat; } set { _PacksTotalNumberFormat = value; } }
        /// <summary>
        /// 回传文件类型
        /// </summary>
        public byte RebackFileType { get; set; }
        private string _RebackFileTypeFormat = string.Empty;
        public string RebackFileTypeFormat { get { return _RebackFileTypeFormat; } set { _RebackFileTypeFormat = value; } }
        /// <summary>
        /// 起始包号
        /// </summary>
        public uint PackStartIndex { get; set; }
        private string _PackStartIndexFormat = string.Empty;
        public string PackStartIndexFormat { get { return _PackStartIndexFormat; } set { _PackStartIndexFormat = value; } }
        /// <summary>
        /// 连续包个数
        /// </summary>
        public uint LastPacksNuber { get; set; }
        private string _LastPacksNuberFormat = string.Empty;
        public string LastPacksNuberFormat { get { return _LastPacksNuberFormat; } set { _LastPacksNuberFormat = value; } }

        /// <summary>
        /// 广播状态
        /// </summary>
        public byte BroadcastState { get; set; }
        private string _BroadcastStateFormat = string.Empty;
        public string BroadcastStateFormat { get { return _BroadcastStateFormat; } set { _BroadcastStateFormat = value; } }
        /// <summary>
        /// 终端类型
        /// </summary>
        public byte TerminalType { get; set; }
        private string _TerminalTypeFormat = string.Empty;
        public string TerminalTypeFormat { get { return _TerminalTypeFormat; } set { _TerminalTypeFormat = value; } }
        /// <summary>
        /// 经度
        /// </summary>
        public float Longitude { get; set; }
        private string _LongitudeFormat = string.Empty;
        public string LongitudeFormat { get { return _LongitudeFormat; } set { _LongitudeFormat = value; } }
        /// <summary>
        /// 纬度
        /// </summary>
        public float Latitude { get; set; }
        private string _LatitudeFormat = string.Empty;
        public string LatitudeFormat { get { return _LatitudeFormat; } set { _LatitudeFormat = value; } }
        /// <summary>
        /// 回传模式
        /// </summary>
        public byte RebackMode { get; set; }
        private string _RebackModeFormat = string.Empty;
        public string RebackModeFormat { get { return _RebackModeFormat; } set { _RebackModeFormat = value; } }
        /// <summary>
        /// 网络模式
        /// </summary>
        public byte NetworkMode { get; set; }
        private string _NetworkModeFormat = string.Empty;
        public string NetworkModeFormat { get { return _NetworkModeFormat; } set { _NetworkModeFormat = value; } }
        /// <summary>
        /// 220V 电压(精确到小数点后2位)
        /// </summary>
        public float Voltage220 { get; set; }
        private string _Voltage220Format = string.Empty;
        public string Voltage220Format { get { return _Voltage220Format; } set { _Voltage220Format = value; } }
        /// <summary>
        /// 24V 电压(精确到小数点后2位)
        /// </summary>
        public float Voltage24 { get; set; }
        private string _Voltage24Format = string.Empty;
        public string Voltage24Format { get { return _Voltage24Format; } set { _Voltage24Format = value; } }
        /// <summary>
        /// 12V 电压(精确到小数点后2位)
        /// </summary>
        public float Voltage12 { get; set; }
        private string _Voltage12Format = string.Empty;
        public string Voltage12Format { get { return _Voltage12Format; } set { _Voltage12Format = value; } }
        /// <summary>
        /// 功放电流(精确到小数点后2位)
        /// </summary>
        public float AmplifierElectricCurrent { get; set; }
        private string _AmplifierElectricCurrentFormat = string.Empty;
        public string AmplifierElectricCurrentFormat { get { return _AmplifierElectricCurrentFormat; } set { _AmplifierElectricCurrentFormat = value; } }
        /// <summary>
        /// 设备本地IP地址
        /// </summary>
        public byte[] LocalHost { get; set; }
        private string _LocalHostFormat = string.Empty;
        public string LocalHostFormat { get { return _LocalHostFormat; } set { _LocalHostFormat = value; } }
        /// <summary>
        /// 子网掩码
        /// </summary>
        public byte[] SubnetMask { get; set; }
        private string _SubnetMaskFormat = string.Empty;
        public string SubnetMaskFormat { get { return _SubnetMaskFormat; } set { _SubnetMaskFormat = value; } }
        /// <summary>
        /// 默认网关
        /// </summary>
        public byte[] DefaultGateway { get; set; }
        private string _DefaultGatewayFormat = string.Empty;
        public string DefaultGatewayFormat { get { return _DefaultGatewayFormat; } set { _DefaultGatewayFormat = value; } }

        /// <summary>
        /// 音频回传
        /// </summary>
        public byte[] AudioReback { get; set; }
        private string _AudioRebackFormat = string.Empty;
        public string AudioRebackFormat { get { return _AudioRebackFormat; } set { _AudioRebackFormat = value; } }
        /// <summary>
        /// 音频回传协议
        /// </summary>
        public byte AudioRebackProtocol { get; set; }
        private string _AudioRebackProtocolFormat = string.Empty;
        public string AudioRebackProtocolFormat { get { return _AudioRebackProtocolFormat; } set { _AudioRebackProtocolFormat = value; } }
        /// <summary>
        /// 录音类别
        /// </summary>
        public byte RecordingCategory { get; set; }
        private string _RecordingCategoryFormat = string.Empty;
        public string RecordingCategoryFormat { get { return _RecordingCategoryFormat; } set { _RecordingCategoryFormat = value; } }
        /// <summary>
        /// 起始包序号
        /// </summary>
        public uint StartPackageNumber { get; set; }
        private string _StartPackageNumberFormat = string.Empty;
        public string StartPackageNumberFormat { get { return _StartPackageNumberFormat; } set { _StartPackageNumberFormat = value; } }


        /// <summary>
        /// 终端和服务器握手
        /// </summary>
        public byte terminal2serverhandshake { get; set; }
        private string _terminal2serverhandshake = string.Empty;
        public string terminal2serverhandshakeFormat { get { return _terminal2serverhandshake; } set { _terminal2serverhandshake = value; } }


        /// <summary>
        /// 逻辑地址
        /// </summary>
        public byte[] LogicalAddress { get; set; }
        private string _LogicalAddressFormat = string.Empty;
        public string LogicalAddressFormat { get { return _LogicalAddressFormat; } set { _LogicalAddressFormat = value; } }
        /// <summary>
        /// 物理地址（唯一标识）
        /// </summary>
        public byte[] PhysicalAddress { get; set; }
        public string PhysicalAddressFormat { get; set; }

        /// <summary>
        /// 回传时间
        /// </summary>
        public string SrvTime { get; set; }

        /// <summary>
        /// 厂家名称
        /// </summary>
        public string FactoryName { get; set; }


        /// <summary>
        /// 端口   用于终端和服务器握手
        /// </summary>
        public ushort RemotePortTmp { get; set; }
        private string _RemotePortTmpFormat = string.Empty;
        public string RemotePortTmpFormat { get { return _RemotePortTmpFormat; } set { _RemotePortTmpFormat = value; } }
        /// <summary>
        /// IP地址  终端和服务器握手
        /// </summary>
        public byte[] IPAddressTmp { get; set; }
        private string _IPAddressTmpFormat = string.Empty;
        public string IPAddressTmpFormat { get { return _IPAddressTmpFormat; } set { _IPAddressTmpFormat = value; } }




        /// <summary>
        /// 数据包头
        /// </summary>
        public byte[] HeaderData { get; set; }
    }
}
