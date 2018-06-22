using GXEBRebackSaveTool.Enums;
using GXEBRebackSaveTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXEBRebackSaveTool.Utils
{
    /// <summary>
    /// 具体设备参数解析类
    /// 
    /// </summary>
    class EquipmentHelper
    {


        public delegate void MyDelegate(object data);

        public  static event MyDelegate MyEvent; //注意须关键字 static  
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">整个帧数据</param>
        /// <param name="fc"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Dictionary<Equipment, byte[]> HandleData(byte[] data, out FeaturesCode fc, out string msg)
        {

            string pp = "";
            for (int i = 0; i < data.Length; i++)
            {
                pp += " " + data[i].ToString("X2");
            }

            string das = pp;
            msg = string.Empty;
            var msgType = Convert.ToChar(data[0]);
            if (msgType == '&')
            {
                //帧头占12字节
            }
            else if (msgType == '%')
            {
               // SingletonInfo.GetInstance().factorycode = data[10].ToString();   //增加于20171128 取厂家编号   测试注释 20171203
                //帧头占18字节
                byte[] dataBody = GetFrameBody(data);
                //判断CRC是否对应
                var array1 = CalmCRC.GetCRC16(dataBody.Take(dataBody.Length - 2).ToArray(), true);
                var array2 = dataBody.Skip(dataBody.Length - 2).ToArray();
                if (array1.EqualsArray(array2))
                {
                    if (dataBody[1] == (byte)FeaturesCode.Search)
                    {
                        fc = FeaturesCode.Search;
                    }
                    else if (Enum.IsDefined(typeof(FeaturesCode), dataBody[0]))
                    {
                        fc = (FeaturesCode)dataBody[0];
                        switch ((FeaturesCode)dataBody[0])
                        {
                            case FeaturesCode.AlertAutoReport:
                                break;
                            case FeaturesCode.AutoHeartBeat:
                                break;
                            case FeaturesCode.AutoReport:
                                return HandleAutoReport(dataBody);
                            case FeaturesCode.Read:
                                break;
                            case FeaturesCode.Search:
                                break;
                            case FeaturesCode.Write:
                                break;
                            case FeaturesCode.WriteError:
                                break;
                        }
                    }
                }
                msg = "CRC数据解析错误";
            }
            msg = "帧头数据解析错误";
            fc = FeaturesCode.None;
            return null;
        }

        /// <summary>
        /// 解析帧头信息
        /// </summary>
        /// <param name="data">帧头数据</param>
        /// <returns></returns>
        public static Dictionary<FrameHeaderEnum, string> HandleHeader(byte[] dataHeader)
        {
            if (dataHeader == null || dataHeader.Length < 1) return null;

            Dictionary<FrameHeaderEnum, string> data = new Dictionary<FrameHeaderEnum, string>();
            data.Add(FrameHeaderEnum.EBResourceCoding, dataHeader.Take(10).Skip(1).ToArrayString(""));
            var factoryName = (FactoryName)BitConverter.ToUInt16(dataHeader.Skip(10).Take(2).ToArray(), 0);
            data.Add(FrameHeaderEnum.FactoryNumber, EnumHelper.GetEnumDescription(factoryName));
            data.Add(FrameHeaderEnum.HardwareVersion, dataHeader.Skip(12).Take(2).ToArrayString("."));
            data.Add(FrameHeaderEnum.SoftwareVersion, dataHeader.Skip(14).Take(2).ToArrayString("."));
            data.Add(FrameHeaderEnum.ProtocolVersion, dataHeader.Skip(16).Take(1).ToString());

            return data;
        }

        /// <summary>
        /// 获取帧体
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] GetFrameBody(byte[] data)
        {
            if (data.Length == 0) return null;
            var msgType = Convert.ToChar(data[0]);
            if (msgType == '&')
            {
                //帧头占12字节
                return data.Skip(12).ToArray();
            }
            else if (msgType == '%')
            {
                //帧头占18字节
                return data.Skip(18).ToArray();
            }
            return null;
        }

        /// <summary>
        /// 获取帧头
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] GetFrameHeader(byte[] data)
        {
            if (data.Length == 0) return null;
            var msgType = Convert.ToChar(data[0]);
            if (msgType == '&')
            {
                //帧头占12字节
                return data.Take(12).ToArray();
            }
            else if (msgType == '%')
            {
                //帧头占18字节
                return data.Take(18).ToArray();
            }
            return null;
        }

        private static string GetEquipmentValueString(Equipment eq, byte[] eqData)
        {
           // return GetEquipmentValueString(eq, GetEquipmentValue(eq, eqData));

            return null;
        }

        public static string GetEquipmentValueString(Equipment eq, object value)
        {
            try
            {
                switch (eq)
                {
                    case Equipment.Open:
                    case Equipment.Close:
                    case Equipment.BitOperationOpen:
                    case Equipment.BitOperationClose:
                    case Equipment.BitOperationUn:
                    case Equipment.ByteOpAddition:
                    case Equipment.ByteOpSubtraction:
                    case Equipment.UShortOpAddition:
                    case Equipment.UShortOpSubtraction:
                    case Equipment.SingleOpAddition:
                    case Equipment.SingleOpSubtraction:
                        break;

                    case Equipment.FMFreList://byte
                        var freList = (byte[])value;
                        string[] sbFre = new string[freList[0]];
                        for (int i = 0; i < freList[0]; i++)
                        {
                            //sbFre[i] = ("频率" + freList[i * 3 + 1].ToString() + "：" + (ushort)(freList[i * 3 + 3] | freList[i * 3 + 2] << 8) * 10 + "KHz");
                           

                            //if (SingletonInfo.GetInstance().factorycode == "1" && SingletonInfo.GetInstance().TerminalType == true && SingletonInfo.GetInstance().Version == "2.0")
                            //{
                            //    //按条件判断解析方式  当厂家代码为01 终端表 02/03 版本2.0      采用以下解析方式      
                            //    sbFre[i] = ((freList[i * 3 + 3] << 8 | freList[i * 3 + 2]) * 10).ToString();//按照陈良及段强的建议修改  20171128
                            //}
                            //else
                            //{
                            //    sbFre[i] = ((freList[i * 3 + 3] | freList[i * 3 + 2] << 8) * 10).ToString();
                            //}              测试注释20171203

                           // sbFre[i] = ((freList[i * 3 + 3] | freList[i * 3 + 2] << 8) * 10).ToString();
                        sbFre[i] = ((freList[i * 3 + 3] << 8 | freList[i * 3 + 2]) * 10).ToString();//按照陈良及段强的建议修改  20171128
                        }
                        return string.Join(",", sbFre);
                    case Equipment.FMSignalStrength://byte
                        var signalStrengthList = (byte[])value;
                        string[] sbSignal = new string[signalStrengthList[0]];
                        for (int i = 0; i < signalStrengthList[0]; i++)
                        {
                            //sbSignal[i] = ("强度" + signalStrengthList[i * 2 + 1].ToString() + "：" + Convert.ToString(signalStrengthList[i * 2 + 2], 10) + "dB");
                            sbSignal[i] = Convert.ToString(signalStrengthList[i * 2 + 2], 10);
                        }
                        return string.Join(",", sbSignal);
                    case Equipment.PlayType://byte
                        var bitPlayType = Convert.ToString((byte)value, 2).PadLeft(8, '0');
                        var highPlayType = Convert.ToByte(bitPlayType.Substring(0, 4), 2);
                        var lowPlayType = Convert.ToByte(bitPlayType.Substring(4, 4), 2);
                        StringBuilder outStrPlayType = new StringBuilder();
                        switch (highPlayType)
                        {
                            case 1:
                                outStrPlayType.Append("省");
                                break;
                            case 2:
                                outStrPlayType.Append("市");
                                break;
                            case 3:
                                outStrPlayType.Append("县");
                                break;
                            case 4:
                                outStrPlayType.Append("乡");
                                break;
                            case 5:
                                outStrPlayType.Append("村");
                                break;
                        }
                        switch (lowPlayType)
                        {
                            case 1:
                                outStrPlayType.Append("一级（特别重大）");
                                break;
                            case 2:
                                outStrPlayType.Append("二级（重大）");
                                break;
                            case 3:
                                outStrPlayType.Append("三级（较大）");
                                break;
                            case 4:
                                outStrPlayType.Append("四级（一般）");
                                break;
                        }
                        return outStrPlayType.ToString();
                    case Equipment.Versions://byte
                        var versions = value as byte[];
                        return string.Format("{0}.{1}   {2}.{3}", Convert.ToString(versions[1], 16), Convert.ToString(versions[0], 16).PadLeft(2, '0'),
                          Convert.ToString(versions[3], 16), Convert.ToString(versions[2], 16).PadLeft(2, '0'));   //  测试注释  20171203



                        //return string.Format("{0}.{1}   {2}.{3}", Convert.ToString(versions[0], 16), Convert.ToString(versions[1], 16),
                        //  Convert.ToString(versions[2], 16), Convert.ToString(versions[3], 16));


                    case Equipment.DigitalTVRadioFrequencyMode://byte
                        return (byte)value == 0 ? "DTMB" : "DVB-C";
                    case Equipment.DigitalTVRadioFrequencyFre://Uint16

                        return value.ToString();
                    case Equipment.QAMType://byte
                        return EnumHelper.GetEnumDescription((QAMEnum)((byte)value));
                    case Equipment.BroadcastVolume://byte
                    case Equipment.BroadcastTimeout://byte  (秒)
                    case Equipment.CurrentModeSignalQuality://byte
                    case Equipment.CurrentModeSignalStrength://byte  (dB)
                        return value.ToString();
                    case Equipment.RemoteControlCenterIPAddress://IP
                        return ((byte[])value).ToArrayString(".");
                    case Equipment.RemoteControlCenterPort://Uint16
                        return value.ToString();
                    case Equipment.AudioServerIPAddress://IP
                        return ((byte[])value).ToArrayString(".");
                    case Equipment.AudioServerPort://Uint16
                        return value.ToString();
                    case Equipment.CallWay://byte
                        return EnumHelper.GetEnumDescription((CallWayEnum)((byte)value));
                    case Equipment.FileName://byte[]
                        return Encoding.ASCII.GetString((byte[])value);
                    case Equipment.RecordingDuration://Uint32
                    case Equipment.PacksTotalNumber://Uint32
                    case Equipment.RebackFileType://byte
                    case Equipment.PackStartIndex://Uint32
                    case Equipment.LastPacksNuber://Uint32
                        return value.ToString();
                    case Equipment.LogicalAddress://byte
                    case Equipment.PhysicalAddress://byte
                        return ((byte[])value).ToNumberArrayString("", 16);
                    case Equipment.BroadcastState://byte
                        switch((byte)value)
                        {
                            case 0x81:
                                return "播放上级日常广播";
                            case 0x82:
                                return "播放上级应急广播";
                            case 0x83:
                                return "本地插播";
                            case 0x84:
                                return "播放本地流程广播";
                            case 0x00:
                                return "待机";
                            default:
                                return "待机";
                        }
                    case Equipment.TerminalType://byte

                   string terminaltypestr= EnumHelper.GetEnumDescription((TerminalTypeEnum)((byte)value));
                   if (terminaltypestr=="3"||terminaltypestr=="2")
                        {
                            SingletonInfo.GetInstance().TerminalType = true;
                        }
                   return terminaltypestr;
                    case Equipment.Longitude://Float
                    case Equipment.Latitude://Float
                        return ((float)value).ToString("f7");
                    case Equipment.RebackMode://byte
                        return (byte)value == 1 ? "GPRS" : "IP 网络";
                    case Equipment.NetworkMode://byte
                        return (byte)value == 1 ? "局域网" : "跨路由";
                    case Equipment.Voltage220://Float  (V)
                    case Equipment.Voltage24://Float  (V)
                    case Equipment.Voltage12://Float  (V)
                    case Equipment.AmplifierElectricCurrent://Float  (A)
                        return ((float)value).ToString("f2");
                    case Equipment.LocalHost://IP
                    case Equipment.SubnetMask://IP
                    case Equipment.DefaultGateway://IP
                        return ((byte[])value).ToArrayString(".");

                    case Equipment.AudioReback://byte[]
                        return string.Empty;
                    case Equipment.AudioRebackProtocol://byte
                        return EnumHelper.GetEnumDescription((AudioRebackProtocol)((byte)value));
                    case Equipment.RecordingCategory://byte
                        return EnumHelper.GetEnumDescription((RecordingCategory)((byte)value));
                    case Equipment.StartPackageNumber://uint
                        return value.ToString();

                    case  Equipment.terminal2serverhandshake:

                        return value.ToString();
                }
                return value.ToString();
            }
            catch
            {
                return null;
            }
        }

        public static object GetEquipmentValue(Equipment eq, byte[] eqData,EquipmentDetail ob)
        {
            try
            {
                switch (eq)
                {
                    case Equipment.Open:
                    case Equipment.Close:
                    case Equipment.BitOperationOpen:
                    case Equipment.BitOperationClose:
                    case Equipment.BitOperationUn:
                    case Equipment.ByteOpAddition:
                    case Equipment.ByteOpSubtraction:
                    case Equipment.UShortOpAddition:
                    case Equipment.UShortOpSubtraction:
                    case Equipment.SingleOpAddition:
                    case Equipment.SingleOpSubtraction:
                        break;

                    case Equipment.FMFreList://byte

                        return eqData;


                    case Equipment.FMSignalStrength://byte
                        return eqData;
                    case Equipment.PlayType://byte
                        return eqData[0];
                    case Equipment.Versions://byte
                        return eqData;
                    case Equipment.DigitalTVRadioFrequencyMode://byte
                        return eqData[0];
                    case Equipment.DigitalTVRadioFrequencyFre://Uint16
                       // string DTVRFre ="" ;


                        //if (SingletonInfo.GetInstance().factorycode == "1" && SingletonInfo.GetInstance().TerminalType == true && SingletonInfo.GetInstance().Version == "2.0")
                        //{
                        //    DTVRFre = eqData[2].ToString() + eqData[1].ToString() + eqData[0].ToString();   //疑问  是否需要转16进制    20171128
                        //}
                        //else
                        //{
                        //    DTVRFre = eqData.ToNumberArrayString("", 16);
                        //}   测试注释  20171203
                       // var DTVRFre = eqData.ToNumberArrayString("", 16);


                       var DTVRFre =(Convert.ToInt32(eqData[2].ToString("X2") + eqData[1].ToString("X2"))/10).ToString() ; 
                        return uint.Parse(DTVRFre);
                    case Equipment.QAMType://byte
                    case Equipment.BroadcastVolume://byte
                    case Equipment.BroadcastTimeout://byte
                    case Equipment.CurrentModeSignalQuality://byte
                    case Equipment.CurrentModeSignalStrength://byte
                        return eqData[0];
                    case Equipment.RemoteControlCenterIPAddress://IP
                        return eqData;
                    case Equipment.RemoteControlCenterPort://Uint16
                        return BitConverter.ToUInt16(eqData, 0);
                    case Equipment.AudioServerIPAddress://IP
                        return eqData;
                    case Equipment.AudioServerPort://Uint16
                        return BitConverter.ToUInt16(eqData, 0);
                    case Equipment.CallWay://byte
                        return eqData[0];
                    case Equipment.FileName://byte[]
                        return eqData;
                    case Equipment.RecordingDuration://Uint32
                    case Equipment.PacksTotalNumber://Uint32
                        return BitConverter.ToUInt32(eqData, 0);
                    case Equipment.RebackFileType://byte
                        return eqData[0];
                    case Equipment.PackStartIndex://Uint32
                    case Equipment.LastPacksNuber://Uint32
                        return BitConverter.ToUInt32(eqData, 0);
                    case Equipment.LogicalAddress://byte
                    case Equipment.PhysicalAddress://byte
                //        Task.Factory.StartNew(delegate
                //{
                //    string phycode = eqData.ToNumberArrayString("", 16);
                //    if (SingletonInfo.GetInstance().MonitorPlatformMessageList.Any())//判断非空
                //    {
                //        MonitorPlatformMessage monitorPlatformMessage = SingletonInfo.GetInstance().MonitorPlatformMessageList.Find((MonitorPlatformMessage tmp) => tmp.SRVPHYSICALCODE == phycode);
                //        if (monitorPlatformMessage != null)
                //        {
                //            SendbackDetail sendbackDetail = new SendbackDetail();
                //            sendbackDetail.tag = Equipment.PhysicalAddress;
                //            sendbackDetail.DataEntity = monitorPlatformMessage;
                //            sendbackDetail.Rebackdata = ob;
                //            EquipmentHelper.MyEvent(sendbackDetail);
                //        }
                //    }
                //});
				return eqData;
                    case Equipment.BroadcastState://byte
                    case Equipment.TerminalType://byte
                        return eqData[0];
                    case Equipment.Longitude://Float
                    case Equipment.Latitude://Float
                        return BitConverter.ToSingle(eqData, 0);
                    case Equipment.RebackMode://byte
                    case Equipment.NetworkMode://byte
                        return eqData[0];
                    case Equipment.Voltage220://Float
                    case Equipment.Voltage24://Float
                    case Equipment.Voltage12://Float
                    case Equipment.AmplifierElectricCurrent://Float
                        return BitConverter.ToSingle(eqData, 0);
                    case Equipment.LocalHost://IP
                    case Equipment.SubnetMask://IP
                    case Equipment.DefaultGateway://IP
                        return eqData;

                    case Equipment.AudioReback://byte[]
                        return eqData;
                    case Equipment.AudioRebackProtocol://byte
                    case Equipment.RecordingCategory://byte
                        return eqData[0];
                    case Equipment.StartPackageNumber://uint
                        return BitConverter.ToUInt32(eqData, 0);

                    case Equipment.terminal2serverhandshake://uint

                       	Task.Factory.StartNew(delegate
				{
					SendbackDetail sendbackDetail2 = new SendbackDetail();
					sendbackDetail2.tag = Equipment.terminal2serverhandshake;
					sendbackDetail2.Rebackdata = ob;
					EquipmentHelper.MyEvent(sendbackDetail2);
				});
				return eqData[0];
                }
                return eqData;
            }
            catch
            {
                return null;
            }
        }

        public static string GetFramHeaderValueString(FrameHeaderEnum fh, byte[] eqData)
        {
            try
            {
                if (eqData == null || eqData.Length == 0) return string.Empty;
                switch (fh)
                {
                    case FrameHeaderEnum.EBResourceCoding:
                        return eqData.ToNumberArrayString("", 16);
                    case FrameHeaderEnum.FactoryNumber:
                        break;
                    case FrameHeaderEnum.HardwareVersion:
                        return string.Format("{0}.{1} ", Convert.ToString(eqData[0], 16), Convert.ToString(eqData[1], 16));
                    case FrameHeaderEnum.SoftwareVersion:
                        return string.Format("{0}.{1} ", Convert.ToString(eqData[0], 16), Convert.ToString(eqData[1], 16));
                    case FrameHeaderEnum.ProtocolVersion:
                        return eqData[0].ToString();
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// 解析自动上报帧体数据
        /// </summary>
        /// <param name="dataBody">帧体数据</param>
        /// <param name="dataStr"></param>
        private static Dictionary<Equipment, byte[]> HandleAutoReport(byte[] dataBody)
        {
            Dictionary<Equipment, byte[]> data = new Dictionary<Equipment, byte[]>();
            int index = 3;
            while (index < dataBody.Length - 2)
            {
                var eq = (Equipment)BitConverter.ToUInt16(dataBody, index);
                var dataOrder = dataBody.Skip(index + 3).Take(dataBody[index + 2]).ToArray();
                data.Add(eq, dataOrder);
                index += 3 + dataBody[index + 2];
            }
            return data;
        }

        /// <summary>
        /// 解析帧头信息（未启用）
        /// </summary>
        /// <param name="dataHeader"></param>
        /// <returns></returns>
        private static Dictionary<FrameHeaderEnum, byte[]> HandleFrameHeader(byte[] dataHeader)
        {
            if (dataHeader == null || dataHeader.Length == 0) return null;
            Dictionary<FrameHeaderEnum, byte[]> data = new Dictionary<FrameHeaderEnum, byte[]>();
            if (dataHeader.Length == 12 && dataHeader[0] == '&')
            {
                data.Add(FrameHeaderEnum.EBResourceCoding, dataHeader.Skip(1).Take(9).ToArray());
                data.Add(FrameHeaderEnum.ProtocolVersion, dataHeader.Skip(10).Take(1).ToArray());
            }
            else if (dataHeader.Length == 18 && dataHeader[0] == '%')
            {
                data.Add(FrameHeaderEnum.EBResourceCoding, dataHeader.Skip(1).Take(9).ToArray());
                data.Add(FrameHeaderEnum.FactoryNumber, dataHeader.Skip(10).Take(2).ToArray());
                data.Add(FrameHeaderEnum.HardwareVersion, dataHeader.Skip(12).Take(2).ToArray());
                data.Add(FrameHeaderEnum.SoftwareVersion, dataHeader.Skip(14).Take(2).ToArray());
                data.Add(FrameHeaderEnum.ProtocolVersion, dataHeader.Skip(16).Take(1).ToArray());
            }
            return data;
        }

    }
}
