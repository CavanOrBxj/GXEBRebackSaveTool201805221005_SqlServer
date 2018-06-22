using System;
using System.ComponentModel;

namespace GXEBRebackSaveTool.Utils
{
    class EnumHelper
    {
        public static string GetEnumDescription(Enum en)
        {
            var fieldInfo = en.GetType().GetField(en.ToString());
            if (fieldInfo != null)
            {
                object[] objs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs.Length > 0)
                {
                    foreach (var obj in objs)
                    {
                        if (obj is DescriptionAttribute)
                        {
                            DescriptionAttribute attr = obj as DescriptionAttribute;
                            return attr.Description;
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
