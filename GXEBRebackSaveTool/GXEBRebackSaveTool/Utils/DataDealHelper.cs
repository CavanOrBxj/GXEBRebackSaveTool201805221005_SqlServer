﻿using GXEBRebackSaveTool.Enums;
using GXEBRebackSaveTool.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace GXEBRebackSaveTool.Utils
{
    /// <summary>
    /// 数据缓存处理
    /// </summary>
    class DataDealHelper : IDisposable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(DataDealHelper));
        /// <summary>
        /// 数据缓存大小
        /// </summary>
        private const int MAXSTATUSCACHE = 100;

        private ConcurrentDictionary<string, IntPtr> clientsConn;
        private ConcurrentDictionary<string, byte[]> clientsHeadData;
        private ConcurrentQueue<EquipmentSource> beforeAnalysisQueue;
        private ConcurrentQueue<byte[]> beforeAnalysisQueue_record;//录音文件上传接收队列  20181108新增


        private ConcurrentQueue<EquipmentDetail> afterAnalysisQueue;

        private ConcurrentQueue<NSEquipmentDetail> afterAnalysisQueue_NS;
        private DataTable dtStatus;
        private DataTable dtStatusNew;

        private DataTable dtsrvStatus_national;

        private DBHelper db;
        private bool isRun = true;

        /// <summary>
        /// 获取当前连接的终端列表（Key为物理码，Value为连接的句柄）
        /// </summary>
        public ConcurrentDictionary<string, IntPtr> Clients { get { return clientsConn; } }

        public ConcurrentDictionary<string, byte[]> ClientHD { get { return clientsHeadData; } }


        private AutoResetEvent autoEvent = new AutoResetEvent(false);


        public delegate void MyDelegate(object data);

        public static event MyDelegate MyEvent; //注意须关键字 static  

        public DataDealHelper(DBHelper db = null)
        {
            try
            {
                clientsConn = new ConcurrentDictionary<string, IntPtr>();
                clientsHeadData = new ConcurrentDictionary<string, byte[]>();
                beforeAnalysisQueue = new ConcurrentQueue<EquipmentSource>();

                beforeAnalysisQueue_record = new ConcurrentQueue<byte[]>();

                afterAnalysisQueue = new ConcurrentQueue<EquipmentDetail>();
                afterAnalysisQueue_NS = new ConcurrentQueue<NSEquipmentDetail>();
                this.db = db;

                dtStatus = new DataTable("Srv_Status");
                dtStatusNew = new DataTable("Srv_StatusGxNew");
                dtsrvStatus_national = new DataTable("srvStatus_national");

                #region 初始化要存储的数据列
                dtStatus.Columns.AddRange(new DataColumn[] {
                new DataColumn("devid", typeof(int)), new DataColumn("srv_physical_code", typeof(string)),
                new DataColumn("powersupplystatus", typeof(string)), new DataColumn("powervoltage", typeof(string)),
                new DataColumn("powercullent", typeof(string)), new DataColumn("powerconsumption", typeof(string)),
                new DataColumn("audiolevel", typeof(string)), new DataColumn("pavoltage", typeof(string)),
                new DataColumn("controlfrequency", typeof(string)), new DataColumn("cflevel", typeof(string)),
                new DataColumn("audiofrequency", typeof(string)), new DataColumn("aflevel", typeof(string)),
                new DataColumn("detectfrequency", typeof(string)), new DataColumn("dflevel", typeof(string)),
                new DataColumn("devlogicid", typeof(string)), new DataColumn("devphyid", typeof(string)),
                new DataColumn("monitorstatus", typeof(string)), new DataColumn("monitor_up_timer", typeof(string)),
                new DataColumn("devrtc", typeof(string)), new DataColumn("srv_time", typeof(string)),});
                dtStatusNew.Columns.AddRange(new DataColumn[] {
                new DataColumn("devid", typeof(int)), new DataColumn("srv_physical_code", typeof(string)),
                new DataColumn("broadcaststate", typeof(string)), new DataColumn("voltage220", typeof(string)),
                new DataColumn("fm_frelist1", typeof(string)), new DataColumn("fm_signalstrength1", typeof(string)),
                new DataColumn("fm_frelist2", typeof(string)), new DataColumn("fm_signalstrength2", typeof(string)),
                new DataColumn("logicaladdress", typeof(string)), new DataColumn("physicaladdress", typeof(string)),
                new DataColumn("srv_time", typeof(string)),
                new DataColumn("playtype", typeof(string)), new DataColumn("versions", typeof(string)),
                new DataColumn("digitaltv_radiofrequencymode", typeof(string)), new DataColumn("digitaltv_radiofrequencyfre", typeof(string)),
                new DataColumn("broadcast_volume", typeof(string)), new DataColumn("currentmode_signalquality", typeof(string)),
                new DataColumn("currentmode_signalstrength", typeof(string)), new DataColumn("remotecontrolcenter_ip", typeof(string)),
                new DataColumn("remotecontrolcenter_port", typeof(string)), new DataColumn("audioserver_ip", typeof(string)),
                new DataColumn("audioserver_port", typeof(string)), new DataColumn("callway", typeof(string)),
                new DataColumn("filename", typeof(string)), new DataColumn("recording_duration", typeof(string)),
                new DataColumn("packs_totalnumber", typeof(string)), new DataColumn("rebackfiletype", typeof(string)),
                new DataColumn("packstartindex", typeof(string)), new DataColumn("lastpacksnumber", typeof(string)),
                new DataColumn("terminaltype", typeof(string)),
                new DataColumn("longitude", typeof(string)), new DataColumn("latitude", typeof(string)),
                new DataColumn("rebackmode", typeof(string)), new DataColumn("networkmode", typeof(string)),
                new DataColumn("voltage24", typeof(string)), new DataColumn("voltage12", typeof(string)),
                new DataColumn("amplifierelectric_current", typeof(string)), new DataColumn("localhost", typeof(string)),
                new DataColumn("subnetmask", typeof(string)), new DataColumn("defaultgateway", typeof(string)),
                new DataColumn("manufacturer_information", typeof(string)), });

                dtsrvStatus_national.Columns.AddRange(new DataColumn[] {
                new DataColumn("ResourceCode", typeof(string)), new DataColumn("TerminalVolume", typeof(string)),
                new DataColumn("TerminalAddressInfo", typeof(string)), 
                new DataColumn("RebackInfo", typeof(string)), new DataColumn("srv_physical_code", typeof(string)),
                new DataColumn("WorkStatus", typeof(string)), new DataColumn("FaultCode", typeof(string)),
                new DataColumn("DeviceTypeCode", typeof(string)), new DataColumn("HardwareVersionNum", typeof(string)),
                new DataColumn("SoftwareVersionNum", typeof(string)),
                new DataColumn("FMStatus", typeof(string)), new DataColumn("DVBCStatus", typeof(string)),
                new DataColumn("DTMBStatus", typeof(string)), new DataColumn("DVBC_FreqInfo", typeof(string)),
                new DataColumn("DTMB_FreqInfo", typeof(string)), new DataColumn("FM_FreqScanList", typeof(string)),
                new DataColumn("FM_CurrentFreqInfo", typeof(string)), 
                new DataColumn("FM_KeepOrderInfo", typeof(string)), });
                #endregion
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
           
        }

        /// <summary>
        /// 数据解析
        /// </summary>
        public void DealStatus()
        {
            while (isRun)
            {
                try
                {
                    if (!beforeAnalysisQueue.IsEmpty)
                    {
                        EquipmentSource data;
                        beforeAnalysisQueue.TryDequeue(out data); //拿出数据
                        string dadada = "";
                        for (int i = 0; i < data.RawData.Length; i++)
                        {
                            dadada += " " + data.RawData[i].ToString("X2"); 
                        }

                        if (SingletonInfo.GetInstance().ProtocolCode == "1")
                        {
                            //处理国标情况  20180913新增
                            if (data != null && data.RawData != null && data.RawData.Length > 0)
                            {
                                var dataNew = HandlerQueue_NationalStandard(data); //解析数据
                                if (dataNew != null)
                                {
                                    afterAnalysisQueue_NS.Enqueue(dataNew);  //放入已解析队列
                                                                          //  clientsConn.AddOrUpdate(dataNew.PhysicalAddressFormat, data.ConnId, (key, value) => { return value = data.ConnId; });
                                    autoEvent.Set(); //通知saveThread
                                }
                            }
                        }
                        else
                        {
                            //处理广西情况 
                            if (data != null && data.RawData != null && data.RawData.Length > 0)
                            {
                                var dataNew = HandlerQueue(data); //解析数据
                                if (dataNew != null)
                                {
                                    afterAnalysisQueue.Enqueue(dataNew);  //放入已解析队列
                                                                          //  clientsConn.AddOrUpdate(dataNew.PhysicalAddressFormat, data.ConnId, (key, value) => { return value = data.ConnId; });
                                    autoEvent.Set(); //通知saveThread
                                }
                            }
                        }
                    }

                    if (!beforeAnalysisQueue_record.IsEmpty)
                    {
                        byte[] dataByte;
                        beforeAnalysisQueue_record.TryDequeue(out dataByte); //拿出数据
                        if (dataByte != null && dataByte.Length > 0)
                        {
                            //解析数据
                            List<DataDetail> dataNew = HandlerQueue_Record(dataByte); //解析数据
                            if (dataNew != null)
                            {
                                string filename = dataNew[0].FileName;
                                string filepath = Directory.GetCurrentDirectory();
                                if (Directory.EnumerateFiles(filepath, filename + ".*", SearchOption.AllDirectories).Any())
                                {
                                    break;
                                }
                                if (SingletonInfo.GetInstance().FileDic.ContainsKey(filename))
                                {
                                    //文件已经在传了
                                    FileAll file = SingletonInfo.GetInstance().FileDic[filename];
                                    file.ReceiveTime = DateTime.Now.AddSeconds(10);
                                    foreach (DataDetail item in dataNew)
                                    {
                                        file.DataList.Add(item);
                                    }
                                }
                                else
                                {
                                    //文件刚开始传
                                    FileAll file = new FileAll();
                                    file.ReceiveTime = DateTime.Now;
                                    file.DataList = new List<DataDetail>();
                                    foreach (DataDetail item in dataNew)
                                    {
                                        file.DataList.Add(item);
                                    }
                                    SingletonInfo.GetInstance().FileDic.Add(filename, file);
                                }
                            }
                        }
                    }
                    Thread.Sleep(50);
                }
                catch (Exception ex)
                {
                    log.Error("数据解析异常-" + beforeAnalysisQueue.Count, ex);
                    continue;
                }
            }
        }

        /// <summary>
        /// 数据存储到内存
        /// </summary>
        public void SaveStatus()
        {
            EquipmentDetail detail;
            NSEquipmentDetail detail_NS;
            while (this.isRun)
            {
                try
                {
                    if (SingletonInfo.GetInstance().ProtocolCode == "2")
                    {
                        #region 处理图南协议的
                        if (!this.afterAnalysisQueue.IsEmpty)
                        {
                            this.afterAnalysisQueue.TryDequeue(out detail);
                            if (detail != null && !IsHeartBeat(detail))
                            {
                                if (detail.FileName != null && detail.FileName.Length > 0 && Enum.IsDefined(typeof(RecordingCategory), detail.RecordingCategory))
                                {
                                    ThreadPool.QueueUserWorkItem(delegate
                                    {
                                        if (this.db != null)
                                        {
                                            this.db.InsertOrUpdateAudioRecorde(detail);
                                        }
                                    });
                                }
                                else
                                {
                                    string[] fmFreList = detail.FMFreListFormat.Split(new string[1]
                                {
                                ","
                                }, StringSplitOptions.RemoveEmptyEntries);
                                    string[] fmSigStrength = detail.FMSignalStrengthFormat.Split(new string[1]
                                {
                                ","
                                }, StringSplitOptions.RemoveEmptyEntries);
                                    lock (this.dtStatus.Rows.SyncRoot)
                                    {
                                        DataRow[] rows2 = this.dtStatus.Select("srv_physical_code='" + detail.PhysicalAddressFormat + "'");
                                        if (rows2 != null && rows2.Length > 0)
                                        {
                                            DataRow[] array = rows2;
                                            foreach (DataRow item2 in array)
                                            {
                                                for (int j = this.dtStatus.Rows.Count - 1; j >= 0; j--)
                                                {
                                                    if (this.dtStatus.Rows[j]["srv_physical_code"] == item2["srv_physical_code"])
                                                    {
                                                        this.dtStatus.Rows[j].Delete();
                                                    }
                                                }
                                                this.dtStatus.AcceptChanges();
                                            }
                                        }
                                        this.dtStatus.Rows.Add(0, detail.PhysicalAddressFormat, (detail.BroadcastStateFormat == "待机") ? "关机" : "开机", detail.Voltage220Format, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, (fmFreList.Length > 0) ? fmFreList[0] : "0", (fmSigStrength.Length > 0) ? fmSigStrength[0] : "0", (fmFreList.Length > 1) ? fmFreList[1] : "0", (fmSigStrength.Length > 1) ? fmSigStrength[1] : "0", DBNull.Value, DBNull.Value, detail.LogicalAddressFormat, detail.PhysicalAddressFormat, DBNull.Value, DBNull.Value, DBNull.Value, detail.SrvTime);
                                    }
                                    lock (this.dtStatusNew.Rows.SyncRoot)
                                    {
                                        DataRow[] rows = this.dtStatusNew.Select("srv_physical_code='" + detail.PhysicalAddressFormat + "'");
                                        if (rows != null && rows.Length > 0)
                                        {
                                            DataRow[] array2 = rows;
                                            foreach (DataRow item in array2)
                                            {
                                                for (int i = this.dtStatusNew.Rows.Count - 1; i >= 0; i--)
                                                {
                                                    if (this.dtStatusNew.Rows[i]["srv_physical_code"] == item["srv_physical_code"])
                                                    {
                                                        this.dtStatusNew.Rows[i].Delete();
                                                    }
                                                }
                                                this.dtStatusNew.AcceptChanges();
                                            }
                                        }
                                        this.dtStatusNew.Rows.Add(0, detail.PhysicalAddressFormat, detail.BroadcastStateFormat, detail.Voltage220Format, (fmFreList.Length > 0) ? fmFreList[0] : "0", (fmSigStrength.Length > 0) ? fmSigStrength[0] : "0", (fmFreList.Length > 1) ? fmFreList[1] : "0", (fmSigStrength.Length > 1) ? fmSigStrength[1] : "0", detail.LogicalAddressFormat, detail.PhysicalAddressFormat, detail.SrvTime, detail.PlayTypeFormat, detail.VersionsFormat, detail.DigitalTVRadioFrequencyModeFormat, detail.DigitalTVRadioFrequencyFreFormat, detail.BroadcastVolumeFormat, detail.CurrentModeSignalQualityFormat, detail.CurrentModeSignalStrengthFormat, detail.RemoteControlCenterIPAddressFormat, detail.RemoteControlCenterPortFormat, detail.AudioServerIPAddressFormat, detail.AudioServerPortFormat, detail.CallWayFormat, detail.FileNameFormat, detail.RecordingDurationFormat, detail.PacksTotalNumberFormat, detail.RebackFileTypeFormat, detail.PackStartIndexFormat, detail.LastPacksNuberFormat, detail.TerminalTypeFormat, detail.LongitudeFormat, detail.LatitudeFormat, detail.RebackModeFormat, detail.NetworkModeFormat, detail.Voltage24Format, detail.Voltage12Format, detail.AmplifierElectricCurrentFormat, detail.LocalHostFormat, detail.SubnetMaskFormat, detail.DefaultGatewayFormat, detail.FactoryName);
                                        int count = this.dtStatusNew.Rows.Count;
                                        this.SaveEqStatus();
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.autoEvent.WaitOne();
                        }
                        #endregion
                    }
                    else
                    {
                        //处理国标协议
                        if (!this.afterAnalysisQueue_NS.IsEmpty)
                        {
                            this.afterAnalysisQueue_NS.TryDequeue(out detail_NS);
                            if (detail_NS != null)
                            {

                                if (string.IsNullOrEmpty(detail_NS.ResourceCode))
                                {
                                    db.UpdateSrvEquipmentStatusBatch_Nation(detail_NS);
                                }
                                else
                                {
                                    lock (this.dtsrvStatus_national.Rows.SyncRoot)
                                    {
                                        DataRow[] rows2 = this.dtsrvStatus_national.Select("srv_physical_code='" + detail_NS.srv_physical_code + "'");
                                        if (rows2 != null && rows2.Length > 0)
                                        {
                                            DataRow[] array = rows2;
                                            foreach (DataRow item2 in array)
                                            {
                                                for (int j = this.dtsrvStatus_national.Rows.Count - 1; j >= 0; j--)
                                                {
                                                    if (this.dtsrvStatus_national.Rows[j]["srv_physical_code"] == item2["srv_physical_code"])
                                                    {
                                                        this.dtsrvStatus_national.Rows[j].Delete();
                                                    }
                                                }
                                                this.dtsrvStatus_national.AcceptChanges();
                                            }
                                        }
                                        this.dtsrvStatus_national.Rows.Add(detail_NS.ResourceCode, detail_NS.TerminalVolume, detail_NS.TerminalAddressInfo, detail_NS.RebackInfo, detail_NS.srv_physical_code, detail_NS.WorkStatus, detail_NS.FaultCode, detail_NS.DeviceTypeCode, detail_NS.HardwareVersionNum, detail_NS.SoftwareVersionNum, detail_NS.FMStatus, detail_NS.DVBCStatus, detail_NS.DTMBStatus, detail_NS.DVBC_FreqInfo, detail_NS.DTMB_FreqInfo, detail_NS.FM_FreqScanList, detail_NS.FM_CurrentFreqInfo, detail_NS.FM_KeepOrderInfo);
                                        this.SaveEqStatus();
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.autoEvent.WaitOne();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // DataDealHelper.log.Error("数据缓存异常-" + this.afterAnalysisQueue.Count, ex);
                }
            }
        }

        /// <summary>
        /// 判断是否是心跳包  心跳包不入数据库   新增于20180302
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public bool IsHeartBeat(EquipmentDetail detail)
        {
            bool flag = false;
            if (detail.AmplifierElectricCurrentFormat==""&& detail.AudioReback==null&&detail.AudioRebackFormat==""&&detail.AudioRebackProtocolFormat==""&&detail.AudioServerIPAddress==null&&detail.AudioServerIPAddressFormat==""&&detail.AudioServerPortFormat==""&&detail.BroadcastStateFormat==""&&detail.BroadcastTimeoutFormat==""&&detail.BroadcastVolumeFormat==""&&detail.CallWayFormat==""&&detail.CurrentModeSignalQualityFormat==""&&detail.CurrentModeSignalStrengthFormat==""&&detail.DefaultGateway==null&&detail.DefaultGatewayFormat==""&&detail.DigitalTVRadioFrequencyFreFormat==""&&detail.DigitalTVRadioFrequencyModeFormat==""&&detail.FileName==null&&detail.FileNameFormat==""&&detail.FileNameFormat==""&&detail.FMFreList==null&&detail.FMFreListFormat==""&&detail.FMSignalStrength==null&&detail.FMSignalStrengthFormat==""&&detail.IPAddressTmp==null&&detail.LastPacksNuberFormat==""&&detail.LatitudeFormat==""&&detail.LocalHost==null&&detail.LocalHostFormat==""&&detail.LogicalAddressFormat==""&&detail.LongitudeFormat==""&&detail.NetworkModeFormat==""&&detail.PackStartIndexFormat==""&&detail.PacksTotalNumberFormat==""&&detail.PlayTypeFormat==""&&detail.QAMTypeFormat==""&&detail.RebackFileTypeFormat==""&&detail.RebackModeFormat==""&&detail.RecordingCategoryFormat==""&&detail.RecordingDurationFormat==""&&detail.RemoteControlCenterIPAddress==null&&detail.RemoteControlCenterIPAddressFormat==""&&detail.RemoteControlCenterPortFormat==""&&detail.RemotePortTmpFormat==""&&detail.StartPackageNumberFormat==""&&detail.SubnetMask==null&&detail.SubnetMaskFormat==""&&detail.Versions==null&&detail.VersionsFormat==""&&detail.Voltage12Format==""&&detail.Voltage220Format==""&&detail.Voltage24Format=="")
            {

                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// 输出数据到数据库
        /// </summary>
        public void SaveEqStatus()
        {
            DataTable dt = null;
            DataTable dtNew = null;
            DataTable dtNa = null;

            if (SingletonInfo.GetInstance().ProtocolCode == "1")
            {
                //国标情况
                lock (dtsrvStatus_national.Rows.SyncRoot) //多线程环境下锁住DataTable中的Rows集合同时也锁住了Columns集合
                {
                    if (dtsrvStatus_national.Rows.Count > 0)
                    {
                        dtNa = dtsrvStatus_national.Copy();
                        dtsrvStatus_national.Rows.Clear();
                    }
                }

                if (db != null && dtNa != null )
                {
                    db.BulkEquipmentDetailNation(dtNa);
                }
            }
            else
            {
                //图南情况
                lock (dtStatus.Rows.SyncRoot)
                {
                    if (dtStatus.Rows.Count > 0)
                    {
                        dt = dtStatus.Copy();
                        dtStatus.Rows.Clear();
                    }
                }
                lock (dtStatusNew.Rows.SyncRoot) //多线程环境下锁住DataTable中的Rows集合同时也锁住了Columns集合
                {
                    if (dtStatusNew.Rows.Count > 0)
                    {
                        dtNew = dtStatusNew.Copy();
                        dtStatusNew.Rows.Clear();
                    }
                }
                if (db != null && dt != null && dtNew != null)
                {
                    var status = db.UpdateSrvEquipmentStatusBatch(dt);
                    db.BulkEquipmentDetail(dt);
                    db.BulkNewEquipmentDetail(dtNew);
                  //  log.Info("数据库存储结束-" + status);
                }
            }
        }

        /// <summary>
        /// 将数据加入缓存
        /// </summary>
        public void Enqueue(byte[] data, IntPtr connId,string iptmp,int porttmp)
        {
            if (data != null && data.Length > 0)
            {
                beforeAnalysisQueue.Enqueue(new EquipmentSource(data, connId, iptmp, porttmp));
            }
        }

        public void EnqueueRecord(byte[] data)
        {
            if (data != null && data.Length > 0)
            {
                beforeAnalysisQueue_record.Enqueue(data);
            }
        }

        private NSEquipmentDetail HandlerQueue_NationalStandard(EquipmentSource datare)
        {
            byte[] data = datare.RawData;
            if (data.Length < 15) return null;

            NSEquipmentDetail equipmentDetail = new NSEquipmentDetail();
            try
            {
                log.Info("开始解析数据-" + data.Length);
              
                string outMsg; //返回的错误信息，正确解析则返回空
                var detail = EquipmentHelper.HandleData_NS(data, out outMsg);

                if (detail != null && string.IsNullOrWhiteSpace(outMsg))
                {
                    #region 解析EquipmentDetail
                    foreach (var key in detail.Keys)
                    {
                        var proInfo = typeof(NSEquipmentDetail).GetProperty(key.ToString());
                        if (null != proInfo)
                        {
                            var eqValue = EquipmentHelper.GetNSEquipmentValue(key, detail[key], equipmentDetail);
                            proInfo.SetValue(equipmentDetail, eqValue, null);
                        }
                    }
                    #endregion
                }
                else
                {
                    equipmentDetail = null;
                }
            }
            catch (Exception ex)
            {
                log.Error("设备详细信息解析异常", ex);
                equipmentDetail = null;
            }
            return equipmentDetail;
        }

        /// <summary>
        /// 解析终端设备工作状态
        /// </summary>
        /// <param name="data"></param>
        /// <returns>设备状态</returns>
        private EquipmentDetail HandlerQueue(EquipmentSource datare)
        {
            byte[] data = datare.RawData;
            if (data.Length < 13) return null;

            EquipmentDetail equipmentDetail = new EquipmentDetail();
            try
            {
                equipmentDetail.IPAddressTmpFormat = datare.RemoteIPTmp;
                equipmentDetail.RemotePortTmp = (ushort)datare.RemotePortTmp;
                log.Info("开始解析数据-" + data.Length);
                equipmentDetail.SrvTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                FeaturesCode fc;
                string outMsg; //返回的错误信息，正确解析则返回空
                var detail = EquipmentHelper.HandleData(data, out fc, out outMsg);

                if (detail != null && string.IsNullOrWhiteSpace(outMsg) && detail.ContainsKey(Equipment.PhysicalAddress))
                {

                    var msgType = Convert.ToChar(data[0]);
                    if (msgType == '&')
                    {
                        //帧头占12字节
                        equipmentDetail.HeaderData = data.Take(12).ToArray();
                    }
                    else if (msgType == '%')
                    {
                        //帧头占18字节
                        equipmentDetail.HeaderData = data.Take(18).ToArray();
                    }
                    byte[] address = detail[Equipment.PhysicalAddress];//每个包必须有物理码

                    string pp = "";
                    for (int i = 0; i < address.Length; i++)
                    {
                        pp += address[i].ToString("X2");// bytes[i].ToString("X2").PadLeft(2, '0');
                    }


                    clientsConn.AddOrUpdate(pp, datare.ConnId, (key, value) => { return value = datare.ConnId; });

                    clientsHeadData.AddOrUpdate(pp, equipmentDetail.HeaderData, (key, value) => { return value = equipmentDetail.HeaderData; });

                    #region 特殊处理 20181105  厂家优先判定，便于后面数据特殊处理
                    var factoryName = (FactoryName)ConvertHelper.Byte2int(equipmentDetail.HeaderData.Skip(10).Take(2).ToArray());
                    equipmentDetail.FactoryName = EnumHelper.GetEnumDescription(factoryName);
                    #endregion

                    #region 解析EquipmentDetail
                    foreach (var key in detail.Keys)
                    {
                        if (key.ToString()== "RemoteControlCenterIPAddress")
                        {
                            int ppr = 0;
                            int ffp = 1;
                            int fs = ppr + ffp;
                            Console.Write("");
                        }
                        var proInfo = typeof(EquipmentDetail).GetProperty(key.ToString());
                        if (null != proInfo)
                        {
                            var eqValue = EquipmentHelper.GetEquipmentValue(key, detail[key], equipmentDetail);
                            proInfo.SetValue(equipmentDetail, eqValue, null);
                            typeof(EquipmentDetail).GetProperty(key.ToString() + "Format").SetValue(equipmentDetail,
                                EquipmentHelper.GetEquipmentValueString(key, eqValue), null);
                        }
                    }
                    #endregion

                    var dataHeader = EquipmentHelper.HandleHeader(EquipmentHelper.GetFrameHeader(data));
                    //if (dataHeader != null)
                    //{
                    //    equipmentDetail.FactoryName = dataHeader[FrameHeaderEnum.FactoryNumber];
                    //}
                    equipmentDetail.FactoryName = "";
                }
                else
                {
                    equipmentDetail = null;
                }
            }
            catch (Exception ex)
            {
                log.Error("设备详细信息解析异常", ex);
                equipmentDetail = null;
            }
            return equipmentDetail;
        }


        /// <summary>
        /// 解析终端设备工作状态
        /// </summary>
        /// <param name="data"></param>
        /// <returns>设备状态</returns>
        private List<DataDetail> HandlerQueue_Record(byte[] datare)
        {
            byte[] data = datare;
            if (data.Length < 13) return null;

            List<DataDetail> DDlist = new List<DataDetail>();
            try
            {
                string pp = "";
                for (int i = 0; i < data.Length; i++)
                {
                    pp += " " + data[i].ToString("X2");
                }


                string pp1 = "";
                for (int i = 0; i < data.Length; i++)
                {
                    pp1 += data[i].ToString("X2");
                }



                var msgType = Convert.ToChar(data[0]);
                if (msgType == '&')
                {
                    //帧头占12字节
                }
                else if (msgType == '%')
                {
                    List<byte[]> bodyList = GetBodtList(data);


                    foreach (byte[] singledataBody in bodyList)
                    {
                        //帧头占18字节
                        byte[] dataBody = singledataBody;


                        //判断CRC是否对应
                        var array1 = CalmCRC.GetCRC16(dataBody.Take(dataBody.Length - 2).ToArray(), true);
                        var array2 = dataBody.Skip(dataBody.Length - 2).ToArray();
                        if (EqualsArray(array1, array2))
                        {
                            string allengthstr = Convert.ToString((int)dataBody[2], 16).PadLeft(2, '0') + Convert.ToString((int)dataBody[1], 16).PadLeft(2, '0');
                            int AlldataLength = Convert.ToInt32(allengthstr, 16);
                            int l = Convert.ToInt32(Convert.ToString((int)dataBody[5], 16).PadLeft(2, '0'), 16);//第一个数据段数据部分的长度

                            List<byte> Section1List = new List<byte>();
                            for (int i = 0; i < l; i++)
                            {
                                Section1List.Add(dataBody[6 + i]);
                            }

                            DataDetail data1 = Deal(Section1List);
                            DDlist.Add(data1);

                            while (AlldataLength > l + 3)
                            {
                                int SectionNdataLength = Convert.ToInt32(Convert.ToString((int)dataBody[l + 6 + 2], 16).PadLeft(2, '0'), 16);
                                List<byte> SectionList = new List<byte>();
                                for (int i = 0; i < SectionNdataLength; i++)
                                {
                                    SectionList.Add(dataBody[l + 6 + 3 + i]);
                                }

                                DataDetail datatmp = Deal(SectionList);
                                DDlist.Add(datatmp);
                                l = l + 3 + Convert.ToInt32(Convert.ToString((int)dataBody[l + 6 + 2], 16).PadLeft(2, '0'), 16);
                            }
                        }
                    }

                }
                return DDlist;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataDetail Deal(List<byte> dtlist)
        {
            byte[] bdata = dtlist.ToArray();
            DataDetail datadetail = new DataDetail();
            datadetail.PhysicalAddressLength = Convert.ToInt32(Convert.ToString((int)bdata[0], 16).PadLeft(2, '0'), 16);


            List<byte> PhysicalAddressList = new List<byte>();
            for (int i = 0; i < datadetail.PhysicalAddressLength; i++)//物理码地址 5
            {
                PhysicalAddressList.Add(bdata[1 + i]);
            }
            byte[] PhysicalAddressArray = PhysicalAddressList.ToArray();
            string phyaddr = "";
            for (int i = 0; i < PhysicalAddressArray.Length; i++)
            {
                phyaddr += PhysicalAddressArray[i].ToString("x2");
            }
            datadetail.PhysicalAddress = phyaddr;



            List<byte> FileNameList = new List<byte>();
            for (int i = 0; i < 33; i++)
            {
                FileNameList.Add(bdata[1 + datadetail.PhysicalAddressLength + i]);
            }
            byte[] FileNameArray = FileNameList.ToArray();
            datadetail.FileName = System.Text.Encoding.ASCII.GetString(FileNameArray);



            List<byte> PackNumList = new List<byte>();
            for (int i = 0; i < 4; i++)
            {
                PackNumList.Add(bdata[33 + 1 + datadetail.PhysicalAddressLength + i]);
            }
            PackNumList.Reverse();
            byte[] PackNumArray = PackNumList.ToArray();



            // datadetail.PackageNum = ToHexString(PackNumArray);
            datadetail.PackageNum = ConvertHelper.GetDataLenth(PackNumArray).ToString();
            log.Error("收到包序号：" + datadetail.PackageNum);
            datadetail.DataLength = Convert.ToInt32(bdata[1 + datadetail.PhysicalAddressLength + 33 + 5].ToString("x2") + bdata[1 + datadetail.PhysicalAddressLength + 33 + 4].ToString("x2"), 16);

            List<byte> AudioDataList = new List<byte>();
            for (int i = 0; i < datadetail.DataLength; i++)
            {
                AudioDataList.Add(bdata[33 + 1 + datadetail.PhysicalAddressLength + 5 + 1 + i]);
            }
            byte[] AudioDataArray = AudioDataList.ToArray();
            datadetail.AudioData = AudioDataArray;
            return datadetail;

        }

        public string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            string hexString = string.Empty;

            if (bytes != null)
            {

                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {

                    strB.Append(bytes[i].ToString("X2"));

                }

                hexString = strB.ToString();

            }
            return hexString;

        }

        public static bool EqualsArray(Array a, Array b)
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

        private List<byte[]> GetBodtList(byte[] data)
        {
            List<byte[]> bodyList = new List<byte[]>();
            if (data.Length == 0) return null;


            int l = data.Length;

            while (l > 0)
            {
                byte[] nimei = data.Skip(data.Length - l).ToArray();
                byte[] datatmp = nimei.Skip(18).ToArray();
                if (datatmp.Length > 9)//一个完整帧长度
                {
                    int singledataLength = Convert.ToInt32(datatmp[2].ToString("x2") + datatmp[1].ToString("x2"), 16);
                    List<byte> singledata = new List<byte>();

                    for (int i = 0; i < singledataLength + 5; i++)
                    {
                        singledata.Add(datatmp[i]);
                    }

                    bodyList.Add(singledata.ToArray());
                    l = l - singledata.Count - 18;
                }
            }
            return bodyList;

        }

        /// <summary>
        /// 保存录音文件
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>是否已成功保存</returns>
        private bool SaveRecordingFile(byte[] bytes)
        {
            try
            {
                bytes.Take(7).Skip(1).ToArray();
                byte[] fileBytes = bytes.Take(21).Skip(7).ToArray();
                bytes.Take(25).Skip(21).ToArray();
                byte[] data = bytes.Skip(27).ToArray();
                string fileName = BitConverter.ToString(fileBytes);
                int packIndex = 0;
                byte[] dataNew2 = null;
                if (File.Exists(fileName))
                {
                    dataNew2 = new byte[packIndex + data.Length];
                    byte[] oldData = File.ReadAllBytes(fileName);
                    Array.ConstrainedCopy(oldData, 0, dataNew2, 0, packIndex);
                    Array.ConstrainedCopy(data, 0, dataNew2, packIndex, data.Length);
                }
                else
                {
                    dataNew2 = data;
                }
                File.WriteAllBytes(fileName, dataNew2);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            isRun = false;
            autoEvent.Set();
            clientsConn.Clear();
        }



        public void Serialize(string Msg)
        {
            if (Msg.Contains("UPGRADE"))
            {
                UpdateCode tmp = new UpdateCode();
                string[] msgItem = Msg.Split('|');
                if (msgItem.Length > 0)
                {
                    foreach (var item in msgItem)
                    {
                        switch (item.Split('~')[0])
                        {
                            case "PACKETTYPE":
                                tmp.PACKETTYPE = item.Split('~')[1];
                                break;
                            case "NEWVERSION":
                                tmp.NEWVERSION = item.Split('~')[1];
                                break;
                            case "OLDVERSION":
                                tmp.OLDVERSION = item.Split('~')[1];
                                break;
                            case "UPGRADEMODE":
                                tmp.UPGRADEMODE = item.Split('~')[1];
                                break;
                            case "HTTPURL":
                                tmp.HTTPURL = item.Split('~')[1];
                                break;
                            case "LIST":
                                string[] str = item.Split('~')[1].Split('&');
                                tmp.LIST = new List<string>();
                                foreach (var code in str)
                                {
                                    tmp.LIST.Add(code);
                                }
                                break;
                        }
                    }
                    SendbackDetail sendbackDetail = new SendbackDetail();
                    sendbackDetail.tag = Equipment.OnlineUpdate;
                    sendbackDetail.Extras = tmp;
                    DataDealHelper.MyEvent(sendbackDetail);
                }
            }
            else if (Msg.Contains("TRANSFER"))
            {
                TransferCode tmp = new TransferCode();
                string[] msgItem = Msg.Split('|');
                if (msgItem.Length > 0)
                {
                    foreach (var item in msgItem)
                    {
                        switch (item.Split('~')[0])
                        {
                            case "PACKETTYPE":
                                tmp.PACKETTYPE = item.Split('~')[1];
                                break;
                            case "FILENAME":
                                tmp.FILENAME = item.Split('~')[1];
                                break;
                            case "SRVPHYSICALCODE":
                                tmp.SRVPHYSICALCODE = item.Split('~')[1];
                                break;
                            case "PACKSTARTINDEX":
                                byte[] startIndex = new byte[4];
                                if (item.Split('~')[1].Length > 0)
                                {
                                    for (int i = 0; i < item.Split('~')[1].Length / 2; i++)
                                    {
                                        startIndex[i] = (byte)Convert.ToInt32(item.Split('~')[1].Substring(2 * i, 2), 16);
                                    }
                                }
                                tmp.PACKSTARTINDEX = startIndex;

                                break;
                            case "AUDIOREBACKSERVERIP":
                                tmp.AUDIOREBACKSERVERIP = item.Split('~')[1];
                                break;
                            case "AUDIOREBACKPORT":
                                tmp.AUDIOREBACKPORT = Convert.ToInt32(item.Split('~')[1]);
                                break;
                            case "PACKS_TOTALNUMBER":
                              
                                SingletonInfo.GetInstance().packnum = Convert.ToInt32(item.Split('~')[1]);
                                break;
                            default:
                                break;
                        }
                    }

                    if (SingletonInfo.GetInstance().udpport != tmp.AUDIOREBACKPORT.ToString())
                    {
                        //图南设备  状态回传  和音频回传不同一个端口
                        if (!Chenckudpport(tmp.AUDIOREBACKPORT.ToString()))
                        {
                            //需要创建新的接收数据线程
                            tmp.Audio_reback_mode = 1;//1 UDP;2 TCP;3串口;其它值预留  现暂时默认设置为udp  20180129
                            SendbackDetail sendbackDetail = new SendbackDetail();
                            sendbackDetail.tag = Equipment.FileName;
                            sendbackDetail.Extras = tmp;
                            DataDealHelper.MyEvent(sendbackDetail);
                        }
                    }
                    else
                    {
                        //德芯的设备
                        SingletonInfo.GetInstance().packnum -= 1;
                    }
                    //if (tmp.AUDIOREBACKSERVERIP != null)
                    //{
                    //    string name = Dns.GetHostName();
                    //    IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
                    //    foreach (var item in ipadrlist)
                    //    {
                    //        if (item.ToString() == tmp.AUDIOREBACKSERVERIP)
                    //        {
                    //            //OpenReceiveTool(tmp.AUDIOREBACKSERVERIP, Convert.ToInt32(tmp.AUDIOREBACKPORT),SingletonInfo.GetInstance().FTPServer,SingletonInfo.GetInstance().FTPPort,SingletonInfo.GetInstance().FTPUserName,SingletonInfo.GetInstance().FTPPwd,SingletonInfo.GetInstance().ftppath);

                    //            OpenReceiveTool(tmp.AUDIOREBACKSERVERIP, Convert.ToInt32(tmp.AUDIOREBACKPORT), SingletonInfo.GetInstance().FTPServer, SingletonInfo.GetInstance().FTPPort, SingletonInfo.GetInstance().FTPUserName, SingletonInfo.GetInstance().FTPPwd, "\\");
                    //        }
                    //    }
                    //}
                    //由TS指令服务实现
                    //tmp.Audio_reback_mode = 1;//1 UDP;2 TCP;3串口;其它值预留  现暂时默认设置为udp  20180129
                    //SendbackDetail sendbackDetail = new SendbackDetail();
                    //sendbackDetail.tag = Equipment.FileName;
                    //sendbackDetail.Extras = tmp;
                    //DataDealHelper.MyEvent(sendbackDetail);
                }
            }
            else if (Msg.Contains("TTS"))
            {
                TTSC tmp = new TTSC();
                string[] msgItem = Msg.Split('|');
                if (msgItem.Length > 0)
                {
                    foreach (var item in msgItem)
                    {
                        switch (item.Split('~')[0])
                        {
                            case "PACKETTYPE":
                                tmp.PACKETTYPE = item.Split('~')[1];
                                break;
                            case "TsCmd_Params":
                                tmp.TsCmd_Params = item.Split('~')[1];
                                break;
                            case "TsCmd_Mode":
                                tmp.TsCmd_Mode = item.Split('~')[1];
                                break;
                            case "TsCmd_ValueID":
                                tmp.DeviceIdList = new List<int>();
                                if (tmp.TsCmd_Mode == "区域")
                                {
                                    string strAreaId = item.Split('~')[1];
                                    string[] ids = strAreaId.Split(',');
                                    List<int> tt = new List<int>();
                                    for (int i = 0; i < ids.Length; i++)
                                    {
                                        tt.Add(Convert.ToInt32(ids[i]));
                                    }

                                    tmp.DeviceIdList = db.AeraCode2DeviceID(tt);
                                }
                                else
                                {
                                    string strDeviceID = item.Split('~')[1];
                                    string[] ids = strDeviceID.Split(',');

                                    foreach (var id in ids)
                                    {
                                        tmp.DeviceIdList.Add(Convert.ToInt32(id));
                                    }
                                }

                                break;
                            case "TsCmd_PlayCount":
                                tmp.TsCmd_PlayCount = Convert.ToInt32(item.Split('~')[1]);
                                break;
                        }
                    }
                    tmp.FileID = (SingletonInfo.GetInstance().FileID + 1).ToString("X").PadLeft(4, '0');
                    SingletonInfo.GetInstance().FileID += 1;
                    SendbackDetail sendbackDetail = new SendbackDetail();
                    sendbackDetail.tag = Equipment.TTS;
                    sendbackDetail.Extras = tmp;

                    log.Error("收到文本转语音MQ指令");
                    DataDealHelper.MyEvent(sendbackDetail);
                }
            }
        }


        /// <summary>
        /// 判断列表中是否有端口    有的话 返回true 没有返回false
        /// </summary>
        /// <param name="udpport"></param>
        /// <returns></returns>
        private bool Chenckudpport(string udpport)
        {
            bool flag = false;
            string findport = SingletonInfo.GetInstance().PortList.Find(x => x == udpport);
            if (findport == null)
            {
                SingletonInfo.GetInstance().PortList.Add(udpport);
            }
            else
            {
                flag = true;
            }
            return flag;
        }

        #region 调用第三方接收工具 目前不用该方法
        private void OpenReceiveTool(string ip, int port, string FTPServer, string FTPPort, string FTPUserName, string FTPPwd,string ftppath)
        {
            List<int> pid = new List<int>();
            Process[] PL = Process.GetProcessesByName("ReceviceFileTool");
            foreach (Process item in PL)
            {
                pid.Add(item.Id);
            }
            foreach (int idtmp in pid)
            {
                Process deleteP = Process.GetProcessById(idtmp);
                deleteP.Kill();
            }

            Process process = new Process();
            process.StartInfo.FileName = Directory.GetCurrentDirectory() + "\\AudioReceiveTool\\ReceviceFileTool";
            process.StartInfo.Arguments = ip + " " + port.ToString() + " " + FTPServer + " " + FTPPort + " " + FTPUserName + " " + FTPPwd + " " + ftppath;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory() + "\\AudioReceiveTool";

            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            log.Error("打开接收工具");
        }
        #endregion

        public List<string> DecId2Physical(List<int> DeviceId)
        {
            return db.PhysicalSearch(DeviceId);
        }
    }
}
