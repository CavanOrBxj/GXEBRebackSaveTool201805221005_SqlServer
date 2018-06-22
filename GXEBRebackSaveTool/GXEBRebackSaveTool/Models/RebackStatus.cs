namespace GXEBRebackSaveTool.Models
{
    public class RebackStatus
    {
        #region 状态参数

        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquType { get; set; }
        /// <summary>
        /// 软件版本号
        /// </summary>
        public string SoftVersion { get; set; }
        /// <summary>
        /// 硬件版本号
        /// </summary>
        public string HardVersion { get; set; }
        /// <summary>
        /// 设备物理编码
        /// </summary>
        public string PhysicCode { get; set; }
        /// <summary>
        /// 设备逻辑编码/资源编码
        /// </summary>
        public string LogicCode { get; set; }
        /// <summary>
        /// 设备电压
        /// </summary>
        public string Voltage { get; set; }
        /// <summary>
        /// 设备电流
        /// </summary>
        public string ElectricCurrent { get; set; }
        /// <summary>
        /// 广播类型
        /// </summary>
        public string BroadType { get; set; }
        /// <summary>
        /// 广播级别
        /// </summary>
        public string BroadGrade { get; set; }
        /// <summary>
        /// 设备当前广播通道
        /// </summary>
        public string CurrentBroadChannel { get; set; }
        /// <summary>
        /// DTMB广播当前锁定频率
        /// </summary>
        public string DtmbLockFreq { get; set; }
        /// <summary>
        /// 锁定的DTMB频率信号强度
        /// </summary>
        public string DtmbSigleStrength { get; set; }
        /// <summary>
        /// 锁定的DTMB频率信噪比
        /// </summary>
        public string DtmbSNR { get; set; }
        /// <summary>
        /// 锁定的DTMB频率信号质量
        /// </summary>
        public string DtmbSQ { get; set; }
        /// <summary>
        /// 锁定的DTMB频率误码率
        /// </summary>
        public string DtmbBER { get; set; }
        /// <summary>
        /// DVBC广播当前锁定频率
        /// </summary>
        public string DvbcLockFreq { get; set; }
        /// <summary>
        /// 锁定的DVBC频率信号强度
        /// </summary>
        public string DvbcSigleStrength { get; set; }
        /// <summary>
        /// 锁定的DVBC频率信噪比
        /// </summary>
        public string DvbcSNR { get; set; }
        /// <summary>
        /// 锁定的DVBC频率信号质量
        /// </summary>
        public string DvbcSQ { get; set; }
        /// <summary>
        /// 锁定的DVBC频率误码率（暂不展示）
        /// </summary>
        public string DvbcBER { get; set; }
        /// <summary>
        /// 调频广播当前锁定频率
        /// </summary>
        public string RdsLockFreq { get; set; }
        /// <summary>
        /// 锁定的调频频率信号强度
        /// </summary>
        public string RdsSigleStrength { get; set; }
        /// <summary>
        /// 当前在播的节目信息(音频PID)
        /// </summary>
        public string AudioPid { get; set; }
        /// <summary>
        /// 音量
        /// </summary>
        public string Volume { get; set; }
        /// <summary>
        /// SIM卡号码
        /// </summary>
        public string TelNum { get; set; }
        /// <summary>
        /// SIM卡对应的IMEI
        /// </summary>
        public string SimIMEI { get; set; }
        /// <summary>
        /// GPRS信号强度
        /// </summary>
        public string GprsStrength { get; set; }
        /// <summary>
        /// SIM所在的位置区码
        /// </summary>
        public string SimLAC { get; set; }
        /// <summary>
        /// SIM所在移动基站的ID
        /// </summary>
        public string SimCellID { get; set; }
        /// <summary>
        /// 硬件模块的tuner状态
        /// </summary>
        public string HardwareCheckTuner { get; set; }
        /// <summary>
        /// 硬件模块的demux状态
        /// </summary>
        public string HardwareCheckDemux { get; set; }
        /// <summary>
        /// 硬件模块的flash状态
        /// </summary>
        public string HardwareCheckFlash { get; set; }
        /// <summary>
        /// 硬件模块的RDS模块状态
        /// </summary>
        public string HardwareCheckRds { get; set; }

        #endregion

        #region 配置参数

        /// <summary>
        /// 终端默认的自动锁频的频率
        /// </summary>
        public string AutoSearchFreq { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public string Property { get; set; }
        /// <summary>
        /// 数字主音频PID
        /// </summary>
        public string AudioPidMain { get; set; }
        /// <summary>
        /// 数字指令PID
        /// </summary>
        public string CmdPid { get; set; }
        /// <summary>
        /// RDS广播频率
        /// </summary>
        public string RdsFreq { get; set; }
        /// <summary>
        /// 是否处于低功耗
        /// </summary>
        public string LowPowerParam { get; set; }
        /// <summary>
        /// 低功耗的休眠时间
        /// </summary>
        public string LowPowerDomant { get; set; }
        /// <summary>
        /// 唤醒后的运行时间
        /// </summary>
        public string LowPowerWake { get; set; }

        #endregion


    }
}
