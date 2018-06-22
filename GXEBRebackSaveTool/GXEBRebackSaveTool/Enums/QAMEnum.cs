using System.ComponentModel;

namespace GXEBRebackSaveTool.Enums
{
    enum QAMEnum
    {
        [Description("16QAM")]
        QAM16 = 0,
        [Description("32QAM")]
        QAM32 = 1,
        [Description("64QAM")]
        QAM64 = 2,
        [Description("128QAM")]
        QAM128 = 3,
        [Description("256QAM")]
        QAM256 = 4,
    }
}
