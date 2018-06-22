using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXEBRebackSaveTool.Utils
{
    public static class EnumerableHelper
    {
        public static string ToArrayString<T>(this IEnumerable<T> a)
        {
            if (a == null || a.Count() == 0) return string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < a.Count(); i++)
            {
                sb.Append(a.ElementAt(i));
            }
            return sb.ToString();
        }

        public static string ToArrayString<T>(this IEnumerable<T> a, string separator)
        {
            if (a == null || a.Count() == 0) return string.Empty;
            string[] str = new string[a.Count()];
            for (int i = 0; i < a.Count(); i++)
            {
                str[i] = a.ElementAt(i).ToString();
            }
            return string.Join(separator, str);
        }

        /// <summary>
        /// 将数组转换成指定进制数的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="separator"></param>
        /// <param name="toBase">要转换的进制数（只能为2,8,10,16）</param>
        /// <returns></returns>
        public static string ToNumberArrayString(this byte[] a, string separator, int toBase)
        {
            if (a == null || a.Length == 0) return string.Empty;
            if (toBase != 2 && toBase != 8 && toBase != 10 && toBase != 16)
            {
                toBase = 10;
            }
            string[] str = new string[a.Count()];
            if (toBase == 16)
            {
                for (int i = 0; i < a.Count(); i++)
                {
                    str[i] = Convert.ToString(a[i], toBase).PadLeft(2, '0').ToUpper();
                }
            }
            else
            {
                for (int i = 0; i < a.Count(); i++)
                {
                    str[i] = Convert.ToString(a[i], toBase);
                }
            }
            return string.Join(separator, str);
        }

        public static bool ArrayEquals(Array a, Array b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            else if (a == null)
            {
                return false;
            }
            else if (b == null)
            {
                return false;
            }
            if (a.Length != b.Length)
            {
                return false;
            }
            for (int i = 0; i < a.Length; i++)
            {
                if (!a.GetValue(i).Equals(b.GetValue(i)))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool EqualsArray(this Array a, Array b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            else if (a == null)
            {
                return false;
            }
            else if (b == null)
            {
                return false;
            }
            if (a.Length != b.Length)
            {
                return false;
            }
            for (int i = 0; i < a.Length; i++)
            {
                if (!a.GetValue(i).Equals(b.GetValue(i)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
