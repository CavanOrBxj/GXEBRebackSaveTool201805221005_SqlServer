using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXEBRebackSaveTool
{
    class HexHelper
    {

        public static byte[] HexStringToByteArray(string s)
        {

            s = "FEFEFE4C" + s + "0016";
            s = s.Replace("   ", " ");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);

            int length = buffer.Length;
            UInt32 sum = 0;
            for (int i = 3; i < length-2; i++)
            {
                sum += buffer[i];
            }
            buffer[length - 2] = (byte)(sum & 255);
              
            return buffer;
        }


        public static string ByteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2").PadLeft(2,'0');
                }
            }
            return returnStr;
        }


        //CRC校验
       // public static string crc16(byte[] crc_data, int data_len)

        public static string crc16(string  data)
        {

            byte[] crc_data = new byte[data.Length / 2];
            for (int j = 0; j < data.Length; j += 2)
            {
                crc_data[j / 2] = Convert.ToByte(data.Substring(j, 2), 16);
            }
            int data_len = crc_data.Length;
            byte crc16lo = Convert.ToByte("ff", 16);
            byte crc16hi = Convert.ToByte("ff", 16);
            byte glo = Convert.ToByte("01", 16);
            byte ghi = Convert.ToByte("a0", 16);
            for (int i = 0; i <= data_len - 1; i++)
            {
                crc16lo = Convert.ToByte(crc16lo ^ crc_data[i]);
                for (int flag = 0; flag <= 7; flag++)
                {
                    byte savelo = crc16lo;
                    byte savehi = crc16hi;
                    crc16hi = Convert.ToByte(crc16hi >> 1);
                    crc16lo = Convert.ToByte(crc16lo >> 1);
                    if ((savehi & Convert.ToByte("01", 16)) == Convert.ToByte("01", 16))
                    {
                        crc16lo = Convert.ToByte(crc16lo | Convert.ToByte("80", 16));
                    }
                    if ((savelo & Convert.ToByte("01", 16)) == Convert.ToByte("01", 16))
                    {
                        crc16hi = Convert.ToByte(crc16hi ^ ghi);
                        crc16lo = Convert.ToByte(crc16lo ^ glo);
                    }
                }
            }
            return crc16lo.ToString("X2") + crc16hi.ToString("X2");
        }

        public static byte[] crc16(byte[] data, int startIndex)
        {
            byte[] crc_data = new byte[data.Length - startIndex - 2];
            for (int j = 0; j < crc_data.Length; j++)
            {
                crc_data[j] = data[startIndex +j];
            }
            int data_len = crc_data.Length;
            byte crc16lo = Convert.ToByte("ff", 16);
            byte crc16hi = Convert.ToByte("ff", 16);
            byte glo = Convert.ToByte("01", 16);
            byte ghi = Convert.ToByte("a0", 16);
            for (int i = 0; i <= data_len - 1; i++)
            {
                crc16lo = Convert.ToByte(crc16lo ^ crc_data[i]);
                for (int flag = 0; flag <= 7; flag++)
                {
                    byte savelo = crc16lo;
                    byte savehi = crc16hi;
                    crc16hi = Convert.ToByte(crc16hi >> 1);
                    crc16lo = Convert.ToByte(crc16lo >> 1);
                    if ((savehi & Convert.ToByte("01", 16)) == Convert.ToByte("01", 16))
                    {
                        crc16lo = Convert.ToByte(crc16lo | Convert.ToByte("80", 16));
                    }
                    if ((savelo & Convert.ToByte("01", 16)) == Convert.ToByte("01", 16))
                    {
                        crc16hi = Convert.ToByte(crc16hi ^ ghi);
                        crc16lo = Convert.ToByte(crc16lo ^ glo);
                    }
                }
            }
            data[data.Length - 2] = crc16lo;
            data[data.Length - 1] = crc16hi;
            return data;
        }
    
    }
}
