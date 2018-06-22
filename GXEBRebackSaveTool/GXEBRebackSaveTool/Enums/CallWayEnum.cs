using System.ComponentModel;

namespace GXEBRebackSaveTool.Enums
{
    enum CallWayEnum
    {
        [Description("线路接入")]
        Line = 1,
        [Description("麦克风")]
        Microphone = 2,
        [Description("电话呼入")]
        Telephone = 3,
        [Description("短信接入")]
        SMSIn = 4,
    }
}
