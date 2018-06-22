namespace GXEBRebackSaveTool.Models
{
    /// <summary>
    /// 修改回传方式配置指令
    /// </summary>
    class RebackConfig
    {
        /// <summary>
        /// 回传通道
        /// </summary>
        public byte RebackType { get; set; }
        /// <summary>
        /// 回传地址参数长度
        /// </summary>
        public byte RebackAddressLength { get; set; }
        /// <summary>
        /// RebackType=1时有效（长度11）
        /// </summary>
        public byte[] RebackTelNumber { get; set; }
        /// <summary>
        /// RebackType=2时有效（长度4）
        /// </summary>
        public byte[] RebackIPAddress { get; set; }
        /// <summary>
        /// RebackType=2时有效（长度4）
        /// </summary>
        public byte[] RebackIPPort { get; set; }
        /// <summary>
        /// RebackType=3时有效
        /// </summary>
        public byte[] RebackDomainName { get; set; }
        /// <summary>
        /// RebackType=3时有效
        /// </summary>
        public byte[] RebackDomainPort { get; set; }
        /// <summary>
        /// 要设置的终端编号的地址类型 1: 表示逻辑地址 2：表示物理地址
        /// </summary>
        public byte AddressType { get; set; }
        /// <summary>
        /// 需要配置的区域和终端编码的个数
        /// </summary>
        public byte TerminalNumber { get; set; }
        /// <summary>
        /// 特指每个区域和终端编码占用的长度，若采用本标准的资源逻辑编码作为逻辑地址格式，则其值=9
        /// </summary>
        public byte AddressLength { get; set; }
        /// <summary>
        /// 应急广播终端逻辑编码
        /// </summary>
        public byte[] TerminalAddress { get; set; }
    }
}
