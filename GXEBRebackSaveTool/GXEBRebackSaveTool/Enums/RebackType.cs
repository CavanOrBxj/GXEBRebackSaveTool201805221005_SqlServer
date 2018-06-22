using System.ComponentModel;

namespace GXEBRebackSaveTool.Enums
{
    enum RebackType
    {
        /// <summary>
        /// 短信，地址为11个数字电话号码
        /// </summary>
        [Description("短信，地址为11个数字电话号码")]
        Message = 1,
        /// <summary>
        /// IP通道，使用UDP协议，固定服务器ip地址回传
        /// </summary>
        [Description("IP通道，使用UDP协议，固定服务器ip地址回传")]
        IPUDPFixedServer = 2,
        /// <summary>
        /// IP通道，使用UDP协议，域名服务器地址回传
        /// </summary>
        [Description("IP通道，使用UDP协议，域名服务器地址回传")]
        IPUDPDomainServer = 3,
        /// <summary>
        /// IP通道，使用TCP协议，固定服务器ip地址回传
        /// </summary>
        [Description("IP通道，使用TCP协议，固定服务器ip地址回传")]
        IPTCPFixedServer = 4,
        /// <summary>
        /// IP通道，使用TCP协议，域名服务器地址回传
        /// </summary>
        [Description("IP通道，使用TCP协议，域名服务器地址回传")]
        IPTCPDomainServer = 5,
        /// <summary>
        /// GPRS通道，使用UDP协议，固定服务器ip地址回传
        /// </summary>
        [Description("GPRS通道，使用UDP协议，固定服务器ip地址回传")]
        GPRSUDPFixedServer = 6,
        /// <summary>
        /// GPRS通道，使用UDP协议，域名服务器地址回传
        /// </summary>
        [Description("GPRS通道，使用UDP协议，域名服务器地址回传")]
        GPRSUDPDomainServer = 7,
        /// <summary>
        /// GPRS通道，使用TCP协议，固定服务器ip地址回传
        /// </summary>
        [Description("GPRS通道，使用TCP协议，固定服务器ip地址回传")]
        GPRSTCPFixedServer = 8,
        /// <summary>
        /// GPRS通道，使用TCP协议，域名服务器地址回传
        /// </summary>
        [Description("GPRS通道，使用TCP协议，域名服务器地址回传")]
        GPRSTCPDomainServer = 9,
    }
}
