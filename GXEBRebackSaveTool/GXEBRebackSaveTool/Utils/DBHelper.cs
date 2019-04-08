using GXEBRebackSaveTool.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
//using System.Data.SqlClient;

namespace GXEBRebackSaveTool.Utils
{
    /// <summary>
    /// 数据库处理
    /// </summary>
    public class DBHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(DBHelper));
        private SqlConnection conn;
        private const int TIMEOUT = 500;

        private object locker;

        public DBHelper()
        {
            conn = new SqlConnection();
            locker = new object();
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public void SetConnectString(string server, string user, string pass, string dataBase)
        {
            string sql = string.Format("server = {0};UID = {1};Password = {2};DataBase = {3};"
                  + "MultipleActiveResultSets=True", server, user, pass, dataBase, TIMEOUT);
            conn.ConnectionString = sql;
        }

        /// <summary>
        /// 检查是否能连接上数据库
        /// </summary>
        /// <returns></returns>
        public bool OpenTest()
        {
            try
            {
                conn.Open();
            }
            catch
            {
                return false;
            }
            conn.Close();
            return true;
        }

        /// <summary>
        /// 批量更新设备在线状态    需修改语法
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool UpdateSrvEquipmentStatusBatch(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return false;

            SqlDataAdapter sd = null;
            DataTable dataTable = new DataTable();
            int reslut = -1;
            lock (locker)
            {
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    // sd = new MySqlDataAdapter("SELECT SRV_ID,SRV_RMT_TIME,SRV_RMT_STATUS,SRV_PHYSICAL_CODE FROM Srv WITH(NOLOCK)", conn);
                    sd = new SqlDataAdapter("SELECT SRV_ID,SRV_RMT_TIME,SRV_RMT_STATUS,SRV_PHYSICAL_CODE FROM Srv WITH(NOLOCK)", conn);
                    SqlCommandBuilder scb = new SqlCommandBuilder(sd);
                    sd.UpdateCommand = scb.GetUpdateCommand();
                    sd.UpdateCommand = new SqlCommand("update Srv set SRV_RMT_TIME=@SRV_RMT_TIME, SRV_RMT_STATUS=@SRV_RMT_STATUS where SRV_PHYSICAL_CODE=@SRV_PHYSICAL_CODE", conn);
                    sd.UpdateCommand.Parameters.Add("@SRV_RMT_TIME", SqlDbType.DateTime, 255, "SRV_RMT_TIME");
                    sd.UpdateCommand.Parameters.Add("@SRV_RMT_STATUS", SqlDbType.VarChar, 255, "SRV_RMT_STATUS");
                    sd.UpdateCommand.Parameters.Add("@SRV_PHYSICAL_CODE", SqlDbType.VarChar, 255, "SRV_PHYSICAL_CODE");
                    sd.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;

                    sd.UpdateBatchSize = 0;
                    sd.Fill(dataTable);
                    


                    if (dataTable.Rows.Count > 0)//添加于20180116  SRV表中的数据来源？
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dataTable.Rows[i].BeginEdit();
                            var rows = dataTable.Select("SRV_PHYSICAL_CODE='" + dt.Rows[i]["srv_physical_code"] + "'");
                            if (rows != null && rows.Length > 0)
                            {
                                rows[0]["SRV_RMT_TIME"] = DateTime.Parse(dt.Rows[i]["srv_time"].ToString());
                                rows[0]["SRV_RMT_STATUS"] = "在线";
                            }
                            dataTable.Rows[i].EndEdit();
                        }
                        reslut = sd.Update(dataTable);
                    }

                }
                catch (Exception ex)
                {
                    log.Error("批量更新设备在线状态发生异常", ex);
                }
                finally
                {
                    dataTable.Clear();
                    sd.Dispose();
                    dataTable.Dispose();
                    conn.Close();
                }
            }
            
            return reslut != -1;
        }

        public bool UpdateSrvEquipmentStatusBatch_Nation(NSEquipmentDetail selectone)
        {
            SqlDataAdapter sd = null;
            DataTable dataTable = new DataTable();
            int reslut = -1;

            lock (locker)
            {
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    sd = new SqlDataAdapter("SELECT SRV_ID,SRV_RMT_TIME,SRV_RMT_STATUS,SRV_PHYSICAL_CODE FROM Srv WITH(NOLOCK)", conn);
                    SqlCommandBuilder scb = new SqlCommandBuilder(sd);
                    sd.UpdateCommand = scb.GetUpdateCommand();
                    sd.UpdateCommand = new SqlCommand("update Srv set SRV_RMT_TIME=@SRV_RMT_TIME, SRV_RMT_STATUS=@SRV_RMT_STATUS where SRV_PHYSICAL_CODE=@SRV_PHYSICAL_CODE", conn);
                    sd.UpdateCommand.Parameters.Add("@SRV_RMT_TIME", SqlDbType.DateTime, 255, "SRV_RMT_TIME");
                    sd.UpdateCommand.Parameters.Add("@SRV_RMT_STATUS", SqlDbType.VarChar, 255, "SRV_RMT_STATUS");
                    sd.UpdateCommand.Parameters.Add("@SRV_PHYSICAL_CODE", SqlDbType.VarChar, 255, "SRV_PHYSICAL_CODE");
                    sd.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;

                    sd.UpdateBatchSize = 0;
                    sd.Fill(dataTable);


                    if (dataTable.Rows.Count > 0)
                    {

                        dataTable.Rows[0].BeginEdit();
                        var rows = dataTable.Select("SRV_PHYSICAL_CODE='" + selectone.Heartbeat + "'");
                        if (rows != null && rows.Length > 0)
                        {
                            rows[0]["SRV_RMT_TIME"] = DateTime.Now;
                            rows[0]["SRV_RMT_STATUS"] = "在线";
                        }
                        dataTable.Rows[0].EndEdit();

                        reslut = sd.Update(dataTable);
                    }

                }
                catch (Exception ex)
                {
                    log.Error("批量更新设备在线状态发生异常", ex);
                }
                finally
                {
                    dataTable.Clear();
                    sd.Dispose();
                    dataTable.Dispose();
                    conn.Close();
                }
            }
          
            return reslut != -1;
        }

        public void BulkEquipmentDetail(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return;


            lock (locker)
            {

                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand("UporInsertSrv_Status", conn);  //带事物处理

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@PCODE", SqlDbType.Char);
                            cmd.Parameters.Add("@Powersupplystatus", SqlDbType.Char);
                            cmd.Parameters.Add("@Powervoltage", SqlDbType.Char);
                            cmd.Parameters.Add("@Controlfrequency", SqlDbType.Char);
                            cmd.Parameters.Add("@Cflevel", SqlDbType.Char);
                            cmd.Parameters.Add("@Audiofrequency", SqlDbType.Char);
                            cmd.Parameters.Add("@Aflevel", SqlDbType.Char);
                            cmd.Parameters.Add("@Devlogicid", SqlDbType.Char);
                            cmd.Parameters.Add("@Devphyid", SqlDbType.Char);
                            cmd.Parameters.Add("@Srv_time", SqlDbType.Char);


                            //开始给添加的各个参数进行赋值操作  
                            cmd.Parameters["@PCODE"].Value = row["srv_physical_code"].ToString();

                            cmd.Parameters["@Powersupplystatus"].Value = row["powersupplystatus"].ToString();
                            cmd.Parameters["@Powervoltage"].Value = row["powervoltage"].ToString();
                            cmd.Parameters["@Controlfrequency"].Value = row["controlfrequency"].ToString();
                            cmd.Parameters["@Cflevel"].Value = row["cflevel"].ToString();
                            cmd.Parameters["@Audiofrequency"].Value = row["audiofrequency"].ToString();
                            cmd.Parameters["@Aflevel"].Value = row["aflevel"].ToString();
                            cmd.Parameters["@Devlogicid"].Value = row["devlogicid"].ToString();
                            cmd.Parameters["@Devphyid"].Value = row["devphyid"].ToString();
                            cmd.Parameters["@Srv_time"].Value = row["srv_time"].ToString();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            log.Error("更新设备信息异常", ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("更新设备信息数据库异常", ex);
                }
                finally { conn.Close(); }
            }
        }

        public void BulkEquipmentDetailNation(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return;

            lock (locker)
            {
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand("UporInsertSrv_Status_Nation", conn);  //带事物处理
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@ResourceCode", SqlDbType.Char);
                            cmd.Parameters.Add("@TerminalVolume", SqlDbType.Char);
                            cmd.Parameters.Add("@TerminalAddressInfo", SqlDbType.Char);
                            cmd.Parameters.Add("@RebackInfo", SqlDbType.Char);
                            cmd.Parameters.Add("@srv_physical_code", SqlDbType.Char);
                            cmd.Parameters.Add("@WorkStatus", SqlDbType.Char);
                            cmd.Parameters.Add("@FaultCode", SqlDbType.Char);
                            cmd.Parameters.Add("@DeviceTypeCode", SqlDbType.Char);
                            cmd.Parameters.Add("@HardwareVersionNum", SqlDbType.Char);
                            cmd.Parameters.Add("@SoftwareVersionNum", SqlDbType.Char);

                            cmd.Parameters.Add("@FMStatus", SqlDbType.Char);
                            cmd.Parameters.Add("@DVBCStatus", SqlDbType.Char);
                            cmd.Parameters.Add("@DTMBStatus", SqlDbType.Char);
                            cmd.Parameters.Add("@DVBC_FreqInfo", SqlDbType.Char);
                            cmd.Parameters.Add("@DTMB_FreqInfo", SqlDbType.Char);
                            cmd.Parameters.Add("@FM_FreqScanList", SqlDbType.Char);
                            cmd.Parameters.Add("@FM_CurrentFreqInfo", SqlDbType.Char);
                            cmd.Parameters.Add("@FM_KeepOrderInfo", SqlDbType.Char);


                            //开始给添加的各个参数进行赋值操作  
                            cmd.Parameters["@ResourceCode"].Value = row["ResourceCode"].ToString();

                            cmd.Parameters["@TerminalVolume"].Value = row["TerminalVolume"].ToString();
                            cmd.Parameters["@TerminalAddressInfo"].Value = row["TerminalAddressInfo"].ToString();
                            cmd.Parameters["@RebackInfo"].Value = row["RebackInfo"].ToString();
                            cmd.Parameters["@srv_physical_code"].Value = row["srv_physical_code"].ToString();
                            cmd.Parameters["@WorkStatus"].Value = row["WorkStatus"].ToString();
                            cmd.Parameters["@FaultCode"].Value = row["FaultCode"].ToString();
                            cmd.Parameters["@DeviceTypeCode"].Value = row["DeviceTypeCode"].ToString();
                            cmd.Parameters["@HardwareVersionNum"].Value = row["HardwareVersionNum"].ToString();
                            cmd.Parameters["@SoftwareVersionNum"].Value = row["SoftwareVersionNum"].ToString();

                            cmd.Parameters["@FMStatus"].Value = row["FMStatus"].ToString();
                            cmd.Parameters["@DVBCStatus"].Value = row["DVBCStatus"].ToString();
                            cmd.Parameters["@DTMBStatus"].Value = row["DTMBStatus"].ToString();
                            cmd.Parameters["@DVBC_FreqInfo"].Value = row["DVBC_FreqInfo"].ToString();
                            cmd.Parameters["@DTMB_FreqInfo"].Value = row["DTMB_FreqInfo"].ToString();
                            cmd.Parameters["@FM_FreqScanList"].Value = row["FM_FreqScanList"].ToString();
                            cmd.Parameters["@FM_CurrentFreqInfo"].Value = row["FM_CurrentFreqInfo"].ToString();
                            cmd.Parameters["@FM_KeepOrderInfo"].Value = row["FM_KeepOrderInfo"].ToString();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            log.Error("更新设备信息异常", ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("更新设备信息数据库异常", ex);
                }
                finally { conn.Close(); }
            }
           
        }

        public void BulkNewEquipmentDetail(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return;


            lock (locker)
            {
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            // SqlCommand cmd = new SqlCommand("UpSrv_StatusGxNew", conn);
                            SqlCommand cmd = new SqlCommand("UporInsertSrv_StatusGxNew", conn);  //带事物处理

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@PCODE", SqlDbType.Char);
                            cmd.Parameters.Add("@Broadcaststate", SqlDbType.Char);
                            cmd.Parameters.Add("@Voltage220", SqlDbType.Char);
                            cmd.Parameters.Add("@Fm_frelist1", SqlDbType.Char);
                            cmd.Parameters.Add("@Fm_signalstrength1", SqlDbType.Char);
                            cmd.Parameters.Add("@Fm_frelist2", SqlDbType.Char);
                            cmd.Parameters.Add("@Fm_signalstrength2", SqlDbType.Char);
                            cmd.Parameters.Add("@Logicaladdress", SqlDbType.Char);
                            cmd.Parameters.Add("@Physicaladdress", SqlDbType.Char);
                            cmd.Parameters.Add("@Srv_time", SqlDbType.Char);


                            cmd.Parameters.Add("@Playtype", SqlDbType.Char);
                            cmd.Parameters.Add("@Versions", SqlDbType.Char);
                            cmd.Parameters.Add("@Digitaltv_radiofrequencymode", SqlDbType.Char);
                            cmd.Parameters.Add("@Digitaltv_radiofrequencyfre", SqlDbType.Char);
                            cmd.Parameters.Add("@Broadcast_volume", SqlDbType.Char);
                            cmd.Parameters.Add("@Currentmode_signalquality", SqlDbType.Char);
                            cmd.Parameters.Add("@Currentmode_signalstrength", SqlDbType.Char);
                            cmd.Parameters.Add("@Remotecontrolcenter_ip", SqlDbType.Char);
                            cmd.Parameters.Add("@Remotecontrolcenter_port", SqlDbType.Char);
                            cmd.Parameters.Add("@Audioserver_ip", SqlDbType.Char);


                            cmd.Parameters.Add("@Audioserver_port", SqlDbType.Char);
                            cmd.Parameters.Add("@Callway", SqlDbType.Char);
                            cmd.Parameters.Add("@Filename", SqlDbType.Char);
                            cmd.Parameters.Add("@Recording_duration", SqlDbType.Char);
                            cmd.Parameters.Add("@Packs_totalnumber", SqlDbType.Char);
                            cmd.Parameters.Add("@Rebackfiletype", SqlDbType.Char);
                            cmd.Parameters.Add("@Packstartindex", SqlDbType.Char);
                            cmd.Parameters.Add("@Lastpacksnumber", SqlDbType.Char);
                            cmd.Parameters.Add("@Terminaltype", SqlDbType.Char);
                            cmd.Parameters.Add("@Longitude", SqlDbType.Char);

                            cmd.Parameters.Add("@Latitude", SqlDbType.Char);
                            cmd.Parameters.Add("@Rebackmode", SqlDbType.Char);
                            cmd.Parameters.Add("@Networkmode", SqlDbType.Char);
                            cmd.Parameters.Add("@Voltage24", SqlDbType.Char);
                            cmd.Parameters.Add("@Voltage12", SqlDbType.Char);
                            cmd.Parameters.Add("@Amplifierelectric_current", SqlDbType.Char);
                            cmd.Parameters.Add("@Localhost", SqlDbType.Char);
                            cmd.Parameters.Add("@Subnetmask", SqlDbType.Char);
                            cmd.Parameters.Add("@Defaultgateway", SqlDbType.Char);
                            cmd.Parameters.Add("@Manufacturer_information", SqlDbType.Char);

                            //开始给添加的各个参数进行赋值操作  
                            cmd.Parameters["@PCODE"].Value = row["srv_physical_code"].ToString();

                            cmd.Parameters["@Broadcaststate"].Value = row["broadcaststate"].ToString();
                            cmd.Parameters["@Voltage220"].Value = row["voltage220"].ToString();
                            cmd.Parameters["@Fm_frelist1"].Value = row["fm_frelist1"].ToString();
                            cmd.Parameters["@Fm_signalstrength1"].Value = row["fm_signalstrength1"].ToString();
                            cmd.Parameters["@Fm_frelist2"].Value = row["fm_frelist2"].ToString();
                            cmd.Parameters["@Fm_signalstrength2"].Value = row["fm_signalstrength2"].ToString();
                            cmd.Parameters["@Logicaladdress"].Value = row["logicaladdress"].ToString();
                            cmd.Parameters["@Physicaladdress"].Value = row["physicaladdress"].ToString();
                            cmd.Parameters["@Srv_time"].Value = row["srv_time"].ToString();

                            cmd.Parameters["@Playtype"].Value = row["playtype"].ToString();
                            cmd.Parameters["@Versions"].Value = row["versions"].ToString();
                            cmd.Parameters["@Digitaltv_radiofrequencymode"].Value = row["digitaltv_radiofrequencymode"].ToString();
                            cmd.Parameters["@Digitaltv_radiofrequencyfre"].Value = row["digitaltv_radiofrequencyfre"].ToString();
                            cmd.Parameters["@Broadcast_volume"].Value = row["broadcast_volume"].ToString();
                            cmd.Parameters["@Currentmode_signalquality"].Value = row["currentmode_signalquality"].ToString();
                            cmd.Parameters["@Currentmode_signalstrength"].Value = row["currentmode_signalstrength"].ToString();
                            cmd.Parameters["@Remotecontrolcenter_ip"].Value = row["remotecontrolcenter_ip"].ToString();
                            cmd.Parameters["@Remotecontrolcenter_port"].Value = row["remotecontrolcenter_port"].ToString();
                            cmd.Parameters["@Audioserver_ip"].Value = row["audioserver_ip"].ToString();

                            cmd.Parameters["@Audioserver_port"].Value = row["audioserver_port"].ToString();
                            cmd.Parameters["@Callway"].Value = row["callway"].ToString();
                            cmd.Parameters["@Filename"].Value = row["filename"].ToString();
                            cmd.Parameters["@Recording_duration"].Value = row["recording_duration"].ToString();
                            cmd.Parameters["@Packs_totalnumber"].Value = row["packs_totalnumber"].ToString();
                            cmd.Parameters["@Rebackfiletype"].Value = row["rebackfiletype"].ToString();
                            cmd.Parameters["@Packstartindex"].Value = row["packstartindex"].ToString();
                            cmd.Parameters["@Lastpacksnumber"].Value = row["lastpacksnumber"].ToString();
                            cmd.Parameters["@Terminaltype"].Value = row["terminaltype"].ToString();
                            cmd.Parameters["@Longitude"].Value = row["longitude"].ToString();

                            cmd.Parameters["@Latitude"].Value = row["latitude"].ToString();
                            cmd.Parameters["@Rebackmode"].Value = row["rebackmode"].ToString();
                            cmd.Parameters["@Networkmode"].Value = row["networkmode"].ToString();
                            cmd.Parameters["@Voltage24"].Value = row["voltage24"].ToString();
                            cmd.Parameters["@Voltage12"].Value = row["voltage12"].ToString();
                            cmd.Parameters["@Amplifierelectric_current"].Value = row["amplifierelectric_current"].ToString();
                            cmd.Parameters["@Localhost"].Value = row["localhost"].ToString();
                            cmd.Parameters["@Subnetmask"].Value = row["subnetmask"].ToString();
                            cmd.Parameters["@Defaultgateway"].Value = row["defaultgateway"].ToString();
                            cmd.Parameters["@Manufacturer_information"].Value = row["manufacturer_information"].ToString();

                            cmd.ExecuteNonQuery();

                        }
                        catch (Exception ex)
                        {
                            log.Error("更新设备信息异常", ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("更新设备信息时数据库异常", ex);
                }
                finally { conn.Close(); }
            }
           
        }

        /// <summary>
        /// 更新或插入录音记录
        /// </summary>
        /// <param name="detail"></param>
        public void InsertOrUpdateAudioRecorde(EquipmentDetail detail)
        {
            if (detail == null) return;
            lock (locker)
            {
                string sql = "select filename from Srv_AudioRecorde where filename='" + detail.FileNameFormat + "'";
                try
                {
                    SqlCommand cmd = new SqlCommand(sql, this.conn);
                    if (this.conn.State != ConnectionState.Open)
                    {
                        this.conn.Open();
                    }
                    var rowCount = cmd.ExecuteScalar();
                    string sqlUpdate = "";
                    if (rowCount != null)
                    {
                        sqlUpdate = string.Format("update Srv_AudioRecorde set recordingcategory='{0}', rebackfiletype='{1}', " +
                            "recording_duration='{2}', packs_totalnumber='{3}', srv_time='{4}',ProgressBar={5} where filename='{6}'",
                            detail.RecordingCategoryFormat, detail.RebackFileTypeFormat, detail.RecordingDurationFormat,
                            detail.PacksTotalNumberFormat, detail.SrvTime, 0, detail.FileNameFormat);
                    }
                    else
                    {
                        sqlUpdate = string.Format("insert into Srv_AudioRecorde " +
                            "(recordingcategory,rebackfiletype,recording_duration,packs_totalnumber,srv_time,srv_physical_code,filename,ProgressBar) " +
                            "values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}',{7})",
                            detail.RecordingCategoryFormat, detail.RebackFileTypeFormat, detail.RecordingDurationFormat,
                            detail.PacksTotalNumberFormat, detail.SrvTime, detail.PhysicalAddressFormat, detail.FileNameFormat, 0);
                    }
                    cmd.CommandText = sqlUpdate;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Error("更新或插入录音记录异常", ex);
                }
                finally { conn.Close(); }
            }
           
        }


        public void UpdateAudioRecorde(string filename,int percent)
        {
            lock (locker)
            {
                string sql = "select filename from Srv_AudioRecorde where filename='" + filename + "'";
                try
                {
                    SqlCommand cmd = new SqlCommand(sql, this.conn);
                    if (this.conn.State != ConnectionState.Open)
                    {
                        this.conn.Open();
                    }
                    var rowCount = cmd.ExecuteScalar();
                    string sqlUpdate = "";
                    if (rowCount != null)
                    {
                        sqlUpdate = string.Format("update Srv_AudioRecorde set ProgressBar={0} where filename='{1}'", percent, filename);
                    }
                    cmd.CommandText = sqlUpdate;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Error("更新录音记录异常", ex);
                }
                finally { conn.Close(); }
            }
            
        }

        /// <summary>
        /// 查询sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool FromSql(string sql, out DataTable dt)
        {
            bool insertFlag = false;
            dt = new DataTable();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql;

            try
            {
                conn.Open();
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                insertFlag = dt.Rows.Count > 0;
            }
            catch (Exception e)
            {
                log.Error("更新状态信息异常", e);
            }
            finally
            {
                conn.Close();
            }
            return insertFlag;
        }

        public List<int> AeraCode2DeviceID(List<int> AeraID)
        {
            List<int> DeviceIdList = new List<int>();
            List<string> ORGCODEAList = new List<string>();
            DataTable dt = new DataTable();

            foreach (var aeraID in AeraID)
            {
                string sql = "select ORG_CODEA from organization where ORG_ID='" + aeraID + "';";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                string ORG_CODEA = "";

                try
                {
                    conn.Open();
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                    sqlAdapter.Fill(dt);
                    ORG_CODEA = dt.Rows[0]["ORG_CODEA"].ToString().Trim();
                    ORGCODEAList.Add(ORG_CODEA);
                    dt.Clear();
                }
                catch (Exception e)
                {
                    log.Error("查询区域码异常", e);
                }
                finally
                {
                    conn.Close();
                }
            }



            foreach (var orgID in ORGCODEAList)
            {
                string orgcodea=orgID.ToString();
                string sql = "select devid from srv_statusgxnew where orgCodea like'%" + orgcodea + "%';";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                string ORG_CODEA = "";

                try
                {
                    conn.Open();
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                    sqlAdapter.Fill(dt);
                    foreach (DataRow item in dt.Rows)
                    {
                        DeviceIdList.Add(Convert.ToInt32(item["devid"].ToString()));
                    }
                    dt.Clear();
                }
                catch (Exception e)
                {
                    log.Error("查询区域码异常", e);
                }
                finally
                {
                    conn.Close();
                }
            }
            return DeviceIdList;
        
        }

        public List<string> PhysicalSearch(List<int> DeviceID)
        {
            List<string> PhysicalList = new List<string>();
            DataTable dt = new DataTable();

            foreach (var devID in DeviceID)
            {
                string sql = "select srv_physical_code from Srv_StatusGxNew where devid='" + devID + "';";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                string physicalcode = "";

                try
                {
                    conn.Open();
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                    sqlAdapter.Fill(dt);
                    physicalcode = dt.Rows[0][0].ToString().Trim();

                    PhysicalList.Add(physicalcode);
                    dt.Clear();
                }
                catch (Exception e)
                {
                    log.Error("查询物理码异常", e);
                }
                finally
                {
                    conn.Close();
                }
            }

            return PhysicalList;

        }
       

    }
}
