using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXEBRebackSaveTool.Models
{
    public  class ConvertHelper
    {

        /// <summary>
        /// 把 2字节的byte[] 转int  高位在前 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public  static int Byte2int(byte[] input)
        {
            string strlen = "";
            foreach (byte item in input)
            {
                strlen += Convert.ToString(item, 16).PadLeft(2, '0');
            }
            int datalen = Convert.ToInt32(strlen, 16);

            return datalen;

        }

        /// <summary>
        /// byte转IP
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Bytes2IP(byte[] input)
        {
            string str = "";
            for (int i = 0; i < input.Length; i++)
            {
                str += "."+input[i].ToString();
            }
            str = str.Substring(1, str.Length - 1);
            return str;
        }

        /// <summary>
        /// BCD转string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string bcd2Str(byte[] bytes)
        {
            string str = "";
            foreach (byte item in bytes)
            {
                str += Convert.ToString(item, 16).PadLeft(2, '0');
            }
            return str;
        }

        /// <summary>
        /// byte[]转11位电话号码
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string Byte2PhoneNum(byte[] bytes)
        {
            string str = "";
            foreach (byte item in bytes)
            {
                str += item.ToString();
            }
            return str;
        }

        public static string Byte2wareVersion(byte[] bytes)
        {
            string str = "";
            foreach (byte item in bytes)
            {
                str += "."+item.ToString();
            }
            str = str.Substring(1, str.Length - 1);

          
            if (str.StartsWith("0"))
            {
                str = str.Substring(2, str.Length - 2);
            }
            return str;
        }


        /// <summary>
        /// 把4字节的byte[] 转int 高位在前
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int GetDataLenth(byte[] input)
        {

            Array.Reverse(input);
            return BitConverter.ToInt32(input, 0);
        }

    }
}
