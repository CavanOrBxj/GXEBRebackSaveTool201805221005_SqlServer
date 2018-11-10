using System;
using System.Collections.Generic;

namespace GXEBRebackSaveTool.Utils
{
   public class DataDetail
    {
        /// <summary>
        /// 音频数据
        /// </summary>
        public byte[] AudioData { get; set; }

        public int PhysicalAddressLength { get; set; }

        public string PhysicalAddress { get; set; }

        public string FileName { get; set; }

        public string PackageNum { get; set; }

        public int DataLength { get; set; }
    }


   public class FileAll
    {

        public List<DataDetail> DataList { get; set; }
     
        public DateTime ReceiveTime { get; set; }
    }

}
