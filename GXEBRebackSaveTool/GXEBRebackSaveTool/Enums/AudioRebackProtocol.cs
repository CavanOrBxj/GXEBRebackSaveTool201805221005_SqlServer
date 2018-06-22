using System.ComponentModel;

namespace GXEBRebackSaveTool.Enums
{
    enum AudioRebackProtocol
    {
        [Description("UDP")]
        UDP = 1,
        [Description("TCP")]
        TCP = 2,
        [Description("串口")]
        SerialPort = 3,
    }
}
