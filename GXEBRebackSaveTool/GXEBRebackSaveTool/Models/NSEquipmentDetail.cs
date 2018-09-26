namespace GXEBRebackSaveTool.Models
{
    /// <summary>
    /// 具体设备参数缓存类
    /// </summary>
    public class NSEquipmentDetail
    {
        /// <summary>
        /// 数据包头
        /// </summary>
        public byte[] HeaderData { get; set; }


        /// <summary>
        /// 资源码
        /// </summary>
        private string _ResourceCode = string.Empty;
        public string ResourceCode { get { return _ResourceCode; } set { _ResourceCode = value; } }

        /// <summary>
        /// 广播音量大小
        /// </summary>
        private string _Volume = string.Empty;
        public string TerminalVolume { get { return _Volume; } set { _Volume = value; } }


        /// <summary>
        /// 终端地址信息
        /// </summary>
        private string _TerminalAddressInfo = string.Empty;
        public string TerminalAddressInfo { get { return _TerminalAddressInfo; } set { _TerminalAddressInfo = value; } }

       

        /// <summary>
        /// 回传信息
        /// </summary>
        private string _RebackInfo=string.Empty;
        public string RebackInfo { get { return _RebackInfo; } set { _RebackInfo = value; } }

        
        /// <summary>
        /// 物理码
        /// </summary>
        private string _srv_physical_code = string.Empty;
        public string srv_physical_code { get { return _srv_physical_code; } set { _srv_physical_code = value; } }

        /// <summary>
        /// 终端工作状态
        /// </summary>
        private string _WorkStatus = string.Empty;
        public string WorkStatus { get { return _WorkStatus; } set { _WorkStatus = value; } }

        /// <summary>
        /// 故障码
        /// </summary>
        private string _FaultCode = string.Empty;
        public string FaultCode { get { return _FaultCode; } set { _FaultCode = value; } }


        /// <summary>
        /// 设备类型码
        /// </summary>
        private string _DeviceTypeCode = string.Empty;
        public string DeviceTypeCode { get { return _DeviceTypeCode; } set { _DeviceTypeCode = value; } }

        /// <summary>
        /// 硬件版本
        /// </summary>
        private string _HardwareVersionNum = string.Empty;
        public string HardwareVersionNum { get { return _HardwareVersionNum; } set { _HardwareVersionNum = value; } }

        /// <summary>
        /// 软件版本
        /// </summary>
        private string _SoftwareVersionNum = string.Empty;
        public string SoftwareVersionNum { get { return _SoftwareVersionNum; } set { _SoftwareVersionNum = value; } }

        /// <summary>
        /// 调频信号状态
        /// </summary>
        private string _FMStatus = string.Empty;
        public string FMStatus { get { return _FMStatus; } set { _FMStatus = value; } }

        /// <summary>
        /// 有线信号状态
        /// </summary>
        private string _DVBCStatus = string.Empty;
        public string DVBCStatus { get { return _DVBCStatus; } set { _DVBCStatus = value; } }


        /// <summary>
        /// 地面无线信号状态
        /// </summary>
        private string _DTMBStatus = string.Empty;
        public string DTMBStatus { get { return _DTMBStatus; } set { _DTMBStatus = value; } }


        /// <summary>
        /// 有线频率信息
        /// </summary>
        private string _DVBC_FreqInfo = string.Empty;
        public string DVBC_FreqInfo { get { return _DVBC_FreqInfo; } set { _DVBC_FreqInfo = value; } }


        /// <summary>
        /// 地面无线频率信息
        /// </summary>
        private string _DTMB_FreqInfo = string.Empty;
        public string DTMB_FreqInfo { get { return _DTMB_FreqInfo; } set { _DTMB_FreqInfo = value; } }


        /// <summary>
        /// FM频点扫描列表
        /// </summary>
        private string _FM_FreqScanList = string.Empty;
        public string FM_FreqScanList { get { return _FM_FreqScanList; } set { _FM_FreqScanList = value; } }


        /// <summary>
        /// FM当前频点信息
        /// </summary>
        private string _FM_CurrentFreqInfo = string.Empty;
        public string FM_CurrentFreqInfo { get { return _FM_CurrentFreqInfo; } set { _FM_CurrentFreqInfo = value; } }



        /// <summary>
        ///  FM维持指令模式信息
        /// </summary>
        private string _FM_KeepOrderInfo = string.Empty;
        public string FM_KeepOrderInfo { get { return _FM_KeepOrderInfo; } set { _FM_KeepOrderInfo = value; } }


        /// <summary>
        /// 心跳中的物理码
        /// </summary>
        private string _Heartbeat = string.Empty;
        public string Heartbeat { get { return _Heartbeat; } set { _Heartbeat = value; } }



    }
}
