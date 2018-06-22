using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXEBRebackSaveTool.Models
{
   public  class SendbackDetail
    {

        public object tag
        {
            get;
            set;
        }

        public object DataEntity
        {
            get;
            set;
        }

        public EquipmentDetail Rebackdata
        {
            get;
            set;
        }

        public object Extras
        {
            get;
            set;
        }
    }
}
