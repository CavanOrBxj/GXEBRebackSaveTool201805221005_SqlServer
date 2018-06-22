using System;

namespace GXEBRebackSaveTool.Models
{
    class EquipmentSource
    {
        /// <summary>
        /// 原始数据
        /// </summary>
        public byte[] RawData { get; set; }

        /// <summary>
        /// 对应的连接ID
        /// </summary>
        public IntPtr ConnId { get; set; }

        public string RemoteIPTmp { get; set; }

        public int RemotePortTmp { get; set; }



        public EquipmentSource(byte[] rawData, IntPtr connId,string iptmp,int porttmp)
        {
            RawData = rawData;
            ConnId = connId;
            RemoteIPTmp = iptmp;
            RemotePortTmp = porttmp;
        }
    }
}
