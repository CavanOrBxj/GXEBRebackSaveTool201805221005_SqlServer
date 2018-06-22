using System;
using System.Collections.Generic;

namespace GXEBRebackSaveTool.Models
{
    /// <summary>
    /// 回传音频类  文件调取
    /// </summary>
    public class TransferCode
    {
        /// <summary>
        /// 包类型
        /// </summary>
        public string PACKETTYPE { get; set; }
       /// <summary>
        /// 起始包号
       /// </summary>
        public byte[] PACKSTARTINDEX { get; set; }

       /// <summary>
       /// 文件名称
       /// </summary>
       public string FILENAME { get; set; }

       /// <summary>
       /// 终端物理码
       /// </summary>
       public string SRVPHYSICALCODE { get; set; }

        /// <summary>
        /// 音频文件回传通道方式
        /// </summary>
        public int Audio_reback_mode { get; set; }

        /// <summary>
        /// 音频回传服务器地址
        /// </summary>
        public string AUDIOREBACKSERVERIP { get; set; }

        /// <summary>
        /// 音频回传端口，大端模式
        /// </summary>
        public int AUDIOREBACKPORT { get; set; }
    }

    /// <summary>
    /// 升级指令
    /// </summary>
   public class UpdateCode
   {

       /// <summary>
       /// 包类型
       /// </summary>
       public string PACKETTYPE { get; set; }
       /// <summary>
       /// 新版本
       /// </summary>
       public string NEWVERSION { get; set; }

       /// <summary>
       /// 老版本
       /// </summary>
       public string OLDVERSION { get; set; }

       /// <summary>
       /// 升级模式
       /// </summary>
       public string UPGRADEMODE { get; set; }

       /// <summary>
       /// 升级文件下载地址
       /// </summary>
       public string HTTPURL { get; set; }

       /// <summary>
       /// 终端物理码
       /// </summary>
       public List<string> LIST { get; set; }


   }


   /// <summary>
   ///文字转语音
   /// </summary>
   public class TTSC
   {

       /// <summary>
       /// 包类型
       /// </summary>
       public string PACKETTYPE { get; set; }
       /// <summary>
       /// 文件ID
       /// </summary>
       public string FileID { get; set; }

       /// <summary>
       /// 文本数据
       /// </summary>
       public string TsCmd_Params { get; set; }

       /// <summary>
       /// 播放次数
       /// </summary>
       public int TsCmd_PlayCount { get; set; }

       /// <summary>
       /// 设备ID 
       /// </summary>
       public List<int> DeviceIdList { get; set; }


       /// <summary>
       /// 模式   决定后面TsCmd_ValudeId代表是设备ID还是区域ID
       /// </summary>
       public string TsCmd_Mode { get; set; }
   }
}
