using System.ComponentModel;

namespace GXEBRebackSaveTool.Enums
{
    enum RecordingCategory : byte
    {
        /// <summary>
        /// 本地话筒
        /// </summary>
        [Description("本地话筒")]
        LocalMike = 1,
        /// <summary>
        /// 电话插播
        /// </summary>
        [Description("电话插播")]
        PhoneInsBroadcast = 2,
        /// <summary>
        /// 短信插播
        /// </summary>
        [Description("短信插播")]
        MsgInsBroadcast = 3,
        /// <summary>
        /// 其它音源
        /// </summary>
        [Description("其它音源")]
        OtherSource = 4,
        /// <summary>
        /// 上级广播
        /// </summary>
        [Description("上级广播")]
        SuperBroadcast = 5,


        /// <summary>
        /// 上级广播
        /// </summary>
        [Description("预留1")]
        obligate1 = 6,

        /// <summary>
        /// 上级广播
        /// </summary>
        [Description("预留2")]
        obligate2 = 7,


        /// <summary>
        /// 上级广播
        /// </summary>
        [Description("预留3")]
        obligate3 = 8,

        /// <summary>
        /// 上级广播
        /// </summary>
        [Description("预留4")]
        obligate4 = 9,

        /// <summary>
        /// 上级广播
        /// </summary>
        [Description("预留5")]
        obligate5 = 10,
    }
}
