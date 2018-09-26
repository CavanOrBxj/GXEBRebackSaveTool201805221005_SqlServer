using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace GXEBRebackSaveTool.Models
{
    public class ComboBoxHelper
    {

        public static void InitOutSwitchType(DataGridViewComboBoxColumn box)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Display", typeof(string));
            dt.Columns.Add("Value", typeof(byte));
            DataRow dr = dt.NewRow();
            dr["Display"] = "关闭输出";
            dr["Value"] = 1;
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["Display"] = "开启输出";
            dr["Value"] = 2;
            dt.Rows.Add(dr);
            box.DisplayMember = "Display";
            box.ValueMember = "Value";
            box.DataSource = dt;
        }

        public static void InitProtocolType(ComboBox box)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Display", typeof(string));
            dt.Columns.Add("Value", typeof(byte));

            DataRow dr = dt.NewRow();
            dr["Display"] = "国标协议";
            dr["Value"] = 1;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Display"] = "图南协议";
            dr["Value"] = 2;
            dt.Rows.Add(dr);

            box.DisplayMember = "Display";
            box.ValueMember = "Value";
            box.DataSource = dt;
        }

        
      

       

    }
}
