using System.ComponentModel;

namespace GXEBRebackSaveTool.Enums
{
    /// <summary>
    /// 具体设备参数
    /// </summary>
    public enum EquipmentNS
    {
       
        [Description("终端音量")]
        TerminalVolume = 0x01,
        
        [Description("本地地址")]
        TerminalAddressInfo = 0x02,
      
        [Description("回传地址")]
        RebackInfo = 0x03,
   
        [Description("终端资源编码")]
        ResourceCode = 0x04,
   
        [Description("物理地址编码")]
        srv_physical_code = 0x05,
 
        [Description("工作状态")]
        WorkStatus = 0x06,
     
        [Description("故障代码")]
        FaultCode = 0x07,
 
        [Description("设备类型")]
        DeviceTypeCode = 0x08,
      
        [Description("硬件版本号")]
        HardwareVersionNum = 0x09,


        [Description("软件版本号")]
        SoftwareVersionNum = 0x0A,

        [Description("调频信号状态")]
        FMStatus = 0x0B,

        [Description("有线信号状态")]
        DVBCStatus = 0x0C,

        [Description("地面无线信号状态")]
        DTMBStatus = 0x0D,

        [Description("有线频率")]
        DVBC_FreqInfo = 0x0E,

        [Description("地面无线频率")]
        DTMB_FreqInfo = 0x0F,

        [Description("FM频点扫描列表")]
        FM_FreqScanList = 0x10,

        [Description("FM当前频点")]
        FM_CurrentFreqInfo = 0x11,

        [Description("FM维持指令模式")]
        FM_KeepOrderInfo = 0x12,

        [Description("终端心跳")]
        Heartbeat = 0x55,
    }
}
