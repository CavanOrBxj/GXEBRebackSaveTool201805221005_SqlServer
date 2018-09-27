using Apache.NMS;
using GXEBRebackSaveTool.Enums;
using GXEBRebackSaveTool.Models;
using GXEBRebackSaveTool.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace GXEBRebackSaveTool
{
    public partial class FormMain : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(FormMain));

        private NetServer netServer;
        private IniFiles ini;
        private DataDealHelper dataHelper;

        private Thread dealThread;  //数据解析线程
        private Thread saveThread;  //数据持久化线程
        private System.Timers.Timer dbTimer;  //定时存储到数据库

        private bool isServerRun;
        private bool saveDataToLog;

     

        private IMessageConsumer m_consumer; //消费者

        private IMessageProducer m_producer; //生产者
        private bool isConn = false; //是否已与MQ服务器正常连接
        private int timercounter;
        /// <summary>
        /// 连接的数据库
        /// </summary>
        public static DBHelper DataBase { get; set; }

        /// <summary>
        /// MQ连接
        /// </summary>
        public static MQ m_mq;

        public FormMain()
        {
            InitializeComponent();
            Load += FormMain_Load;
            FormClosing += FormMain_FormClosing;
            Text = Text + "-" + Application.ProductVersion;
            
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (netServer != null)
                {
                    netServer.TCPReceiveData -= Server_TCPReceiveData;
                    netServer.UDPReceiveData -= Server_UDPReceiveData;
                    netServer.Dispose();
                    netServer = null;
                }
                if (dataHelper != null)
                {
                    dataHelper.Dispose();
                    dataHelper = null;
                }
                if (dbTimer != null)
                {
                    dbTimer.Stop();
                    dbTimer.Dispose();
                }

                #region 关闭工具
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
                #endregion

                //  m_mq.Close();//关闭MQ连接  注  进程中关闭的时候会有异常
                System.Environment.Exit(0);
            }
            catch (Exception)
            {

                this.Close();
            }
       
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            FormMain.m_mq = new MQ();
            saveDataToLog = string.Equals(bool.TrueString, System.Configuration.ConfigurationManager.AppSettings["SaveDataToLog"], StringComparison.CurrentCultureIgnoreCase);

            //初始化数据解析类
            bool iniCheck = CheckIniConfig();
            DataBase = new DBHelper();
            dataHelper = new DataDealHelper(DataBase);

                textTcpPort.Text = ini.ReadValue("LocalHost", "TCPLocalPort");
                textUdpPort.Text = ini.ReadValue("LocalHost", "UDPLocalPort");

                SingletonInfo.GetInstance().ProtocolCode = ini.ReadValue("ProtocolType", "type");


                //初始化处理线程
                dealThread = new Thread(dataHelper.DealStatus);
                saveThread = new Thread(dataHelper.SaveStatus);

            //初始化存储数据库的计时器（5秒更新一次数据库）
            dbTimer = new System.Timers.Timer(10000);
            dbTimer.AutoReset = true;
            dbTimer.Elapsed += DbTimer_Elapsed;

            EquipmentHelper.MyEvent += new EquipmentHelper.MyDelegate(SendDataResponse);
            DataDealHelper.MyEvent += new DataDealHelper.MyDelegate(SendDataResponse);

            this.timercounter = 0;


            SingletonInfo.GetInstance().FTPServer = ini.ReadValue("Reback", "FTPServer");
            SingletonInfo.GetInstance().FTPPort = ini.ReadValue("Reback", "FTPPort");
            SingletonInfo.GetInstance().FTPUserName = ini.ReadValue("Reback", "FTPUserName");
            SingletonInfo.GetInstance().FTPPwd = ini.ReadValue("Reback", "FTPPwd");
         //   string tmpftppath = ini.ReadValue("Reback", "ftppath").Split(':')[1];
          //  SingletonInfo.GetInstance().ftppath =tmpftppath.Remove(0,1); 


            btnStart_Click(null,null);
            log.Info("回传服务启动：启动时间->" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        void EquipmentHelper_MyEvent(object data)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// 初始化MQ信息
        /// </summary>
        private void ConnectMQServer()
        {
            try
            {
                FormMain.m_mq.uri = "tcp://" + this.ini.ReadValue("MQ", "MQIP") + ":" + this.ini.ReadValue("MQ", "MQPORT");
                FormMain.m_mq.Start();
                this.isConn = true;
                this.m_consumer = FormMain.m_mq.CreateConsumer(false, this.ini.ReadValue("MQ", "TopicName"));
                this.m_consumer.Listener += this.consumer_listener;
                FormMain.m_mq.CreateProducer(false, this.ini.ReadValue("MQ", "TopicName"));
            }
            catch (Exception)
            {

            
            }
        }
        private void consumer_listener(IMessage message)
        {
            try
            {
                ITextMessage msg = (ITextMessage)message;
                string strMsg = msg.Text;

                if (strMsg.Contains("TsCmd_Type~TTS"))
                {
                    log.Info("文本转语音MQ指令：" + strMsg);  
                }
                dataHelper.Serialize(strMsg);
            }
            catch (Exception)
            {
                this.m_consumer.Close();
            }
        }

        private void DbTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
          //  dataHelper.SaveEqStatus();
        }

        /// <summary>
        /// 检查配置文件能否正常打开
        /// </summary>
        private bool CheckIniConfig()
        {
            try
            {
                string iniPath = Path.Combine(Application.StartupPath, "gxreback.ini");
                this.ini = new IniFiles(iniPath);
            }
            catch (Exception iniEx)
            {
                FormMain.log.Error("INI配置文件异常", iniEx);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 服务设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateLocalHost_Click(object sender, EventArgs e)
        {
            try
            {
                //修改配置
                new Forms.FormSetting(isServerRun).ShowDialog(this);
                textTcpPort.Text = ini.ReadValue("LocalHost", "TCPLocalPort");
                textUdpPort.Text = ini.ReadValue("LocalHost", "UDPLocalPort");
            }
            catch (Exception)
            {

                
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnStart.Text == "停止服务")
                {
                    if (netServer != null)
                    {
                        netServer.Stop();
                        netServer.TCPReceiveData -= Server_TCPReceiveData;
                        netServer.UDPReceiveData -= Server_UDPReceiveData;
                        FormMain.m_mq.Close();
                        this.m_consumer.Listener -= this.consumer_listener;
                        this.m_consumer.Close();
                        this.m_consumer = null;
                        this.timer1.Enabled = false;
                    }
                    isServerRun = false;
                    btnStart.Text = "启动服务";
                    return;
                }
                this.timer1.Enabled = true;
                this.ConnectMQServer();
                string portStr = ini.ReadValue("LocalHost", "UDPLocalPort");
                if (!Regex.IsMatch(portStr, NetServer.PATTERNPORT))
                {
                    MessageBox.Show("端口号设置错误", "提示");
                    return;
                }
                ushort udpPort = ushort.Parse(portStr);
                string tcpPortStr = ini.ReadValue("LocalHost", "TCPLocalPort");
                if (!Regex.IsMatch(tcpPortStr, NetServer.PATTERNPORT))
                {
                    MessageBox.Show("端口号设置错误", "提示");
                    return;
                }
                ushort tcpPort = ushort.Parse(tcpPortStr);

                InitDatabase();
                if (!DataBase.OpenTest())
                {
                    MessageBox.Show("数据库连接失败，请检查数据库配置", "提示");
                    return;
                }

                string ip = ini.ReadValue("LocalHost", "LoaclIP");
                if (ip == "127.0.0.1")
                {
                    var result = MessageBox.Show(this, "当前绑定IP为127.0.0.1，若要与外界通信需修改为本地公网IP，是否继续绑定127.0.0.1，选择否则返回设置？",
                        "提示", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }

                //启动数据处理线程和计时器
                if (!dealThread.IsAlive)
                {
                    dealThread.Start();
                }
                if (!saveThread.IsAlive)
                {
                    saveThread.Start();
                }
                if (!dbTimer.Enabled)
                {
                    dbTimer.Start();
                }

                #region 同时启动tcp和udp
                
                netServer = new NetServer(tcpPort, udpPort, ip);
                netServer.TCPReceiveData += Server_TCPReceiveData;
                netServer.UDPReceiveData += Server_UDPReceiveData;
                netServer.Start();
                #endregion

                isServerRun = true;
                btnStart.Text = "停止服务";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                //启动异常则关闭服务
                if (netServer != null)
                {
                    netServer.Stop();
                    netServer.TCPReceiveData -= Server_TCPReceiveData;
                    netServer.UDPReceiveData -= Server_UDPReceiveData;
                }
                isServerRun = false;
                btnStart.Text = "启动服务";
                return;
            }
        }

        private void Server_UDPReceiveData(object sender, SocketDataEventArgs e)
        {
            try
            {
                if (e.Data != null && e.Data.Length > 0)
                {
                    dataHelper.Enqueue(e.Data, e.ConnId, e.EndPoint.ToString().Split(':')[0], Convert.ToInt32(e.EndPoint.ToString().Split(':')[1]));
                    Invoke(new MethodInvoker(() =>
                    {
                        richTextRebackMsg.AppendText("接收时间:" + DateTime.Now + "\n");
                        richTextRebackMsg.AppendText("UDP数据:" + e.Data.ToNumberArrayString(" ", 16) + "\n");
                    }));
                    if (saveDataToLog)
                    {
                        log.Info(e.Data.ToNumberArrayString(" ", 16));
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

        private void Server_TCPReceiveData(object sender, SocketDataEventArgs e)
        {
            try
            {
                if (e.Data != null && e.Data.Length > 0)
                {
                    dataHelper.Enqueue(e.Data, e.ConnId, e.EndPoint.ToString().Split(':')[0], Convert.ToInt32(e.EndPoint.ToString().Split(':')[1]));
                    Invoke(new MethodInvoker(() =>
                    {
                        richTextRebackMsg.AppendText("接收时间:" + DateTime.Now + "\n");
                        richTextRebackMsg.AppendText("TCP数据:" + e.Data.ToNumberArrayString(" ", 16) + "\n");
                    }));
                    if (saveDataToLog)
                    {
                        log.Info(e.Data.ToNumberArrayString(" ", 16));
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

        /// <summary>
        /// 终端和服务器握手  应答发送
        /// </summary>
        /// <param name="data"></param>
        private void SendDataResponse(object data)
        {

            SendbackDetail SendObject = (SendbackDetail)data;
            switch ((Equipment)SendObject.tag)
            {
                case Equipment.terminal2serverhandshake:
                    {
                        EquipmentDetail ob = SendObject.Rebackdata;
                        byte[] SendCommandShakehandsList = new byte[21] { 38, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                        for (int m = 0; m < 9; m++)
                        {
                            SendCommandShakehandsList[1 + m] = ob.HeaderData[1 + m];
                        }
                        SendCommandShakehandsList[10] = ob.HeaderData[16];
                        string datatmp2 = "10040041030102";//20180125应朱红彪需求 数据长度的大小端转换 高低位换位置  以后对外对接可能存在问题
                        string crcdata = "10040041030102";
                        datatmp2 += HexHelper.crc16(crcdata);//算CRC
                        byte[] byteArray = new byte[datatmp2.Length / 2];
                        for (int l = 0; l < byteArray.Length; l++)
                        {
                            SendCommandShakehandsList[12 + l] = (byte)Convert.ToInt32(datatmp2.Substring(l * 2, 2).ToString(), 16);
                        }
                        string physicalAddress = ob.PhysicalAddressFormat;
                        IntPtr connId = default(IntPtr);

                        if (physicalAddress != null)
                        {
                            if (this.dataHelper.Clients.TryGetValue(physicalAddress, out connId))
                            {
                                byte[] bytes2 = SendCommandShakehandsList;
                                this.netServer.Send(connId, bytes2, bytes2.Length);

                                //日志打印  20180305
                                log.Info("握手应答指令已发送");  
                            }
                        }
                        break;
                    }

                case Equipment.OnlineUpdate://在线升级指令下发
                    UpdateCode uc = (UpdateCode)SendObject.Extras;
                    if (uc.LIST.Count > 0)
                    {
                        foreach (var item in uc.LIST)
                        {
                            if (Encoding.Default.GetBytes(uc.HTTPURL).Length < 128)//url长度小于128字节
                            {
                                byte[] HeaderData = new byte[] { };

                                if (!this.dataHelper.ClientHD.TryGetValue(item, out HeaderData))
                                {
                                    break;
                                }
                                log.Info("升级数据头拿到");  
                                string codestr = uc.UPGRADEMODE + "," + item + "," + uc.OLDVERSION + "," + uc.NEWVERSION + "," + uc.HTTPURL;

                                log.Info("升级信息-"+codestr);  
                                byte[] DataList = Encoding.Default.GetBytes(codestr);//字符串转ASCII
                                List<byte> SendCommandListUpdate = new List<byte>();
                                SendCommandListUpdate.Add(38);

                                for (int k = 0; k < 9; k++)
                                {
                                    SendCommandListUpdate.Add(HeaderData[1 + k]);//9位资源码
                                }

                                SendCommandListUpdate.Add(HeaderData[16]);//协议版本号

                                //补全整个发送帧
                                for (int i = 0; i < 6 + 2 + DataList.Length + 1; i++)
                                {
                                    SendCommandListUpdate.Add(0);
                                }

                                byte[] sendcommand = SendCommandListUpdate.ToArray();

                                string datalenth = (DataList.Length + 3).ToString("x").PadLeft(4, '0');

                                sendcommand[12] = (byte)Convert.ToInt32("10", 16);
                                sendcommand[13] = (byte)Convert.ToInt32(datalenth.Substring(2, 2), 16);
                                sendcommand[14] = (byte)Convert.ToInt32(datalenth.Substring(0, 2), 16);
                                sendcommand[15] = (byte)Convert.ToInt32("42", 16);
                                sendcommand[16] = 3;
                                sendcommand[17] = (byte)DataList.Length;

                                for (int l = 0; l < DataList.Length; l++)
                                {
                                    sendcommand[18 + l] = DataList[l];
                                }
                                sendcommand = HexHelper.crc16(sendcommand, 12);
                                IntPtr connIdFile = default(IntPtr);
                                if (this.dataHelper.Clients.TryGetValue(item, out connIdFile))
                                {
                                    byte[] bytes = sendcommand;


                                    var str = DateTime.Now.ToString();
                                    var encode = Encoding.UTF8;
                                    StringBuilder ret = new StringBuilder();
                                    foreach (byte b in bytes)
                                    {
                                        //{0:X2} 大写
                                        ret.AppendFormat("{0:x2}", b);
                                    }
                                    string hex = ret.ToString();



                                    this.netServer.Send(connIdFile, bytes, bytes.Length);

                                    //日志打印  20180305
                                    log.Info("在线升级指令已发送：" + ret);  
                                }

                            }
                        }
                    }

                    break;
                case Equipment.FileName://文件调取

                  //由TS指令服务去实现了  但是接收还是它

                    //TransferCode select = (TransferCode)SendObject.Extras;
                    //byte[] HeadDataFile = new byte[] { };

                    //if (!this.dataHelper.ClientHD.TryGetValue(select.SRVPHYSICALCODE, out HeadDataFile))
                    //{
                    //    break;
                    //}
                    //List<byte> SendCommandFilesList2 = new List<byte>();
                    //SendCommandFilesList2.Add(38);

                    //for (int k = 0; k < 9; k++)
                    //{
                    //    SendCommandFilesList2.Add(HeadDataFile[1 + k]);//9位资源码
                    //}
                    //SendCommandFilesList2.Add(HeadDataFile[16]);//协议版本号

                    //SendCommandFilesList2.Add(0);//预留字段
                    ////补全整个发送帧
                    //for (int i = 0; i < 55; i++)
                    //{
                    //    SendCommandFilesList2.Add(0);
                    //}


                    //byte[] sendcommandFile = SendCommandFilesList2.ToArray();
                    //sendcommandFile[12] = (byte)Convert.ToInt32("10", 16);

                    //string datalenthFile = (50).ToString("x").PadLeft(4, '0');
                    //sendcommandFile[12] = (byte)Convert.ToInt32("03", 16);
                    //sendcommandFile[13] = (byte)Convert.ToInt32(datalenthFile.Substring(2, 2), 16);
                    //sendcommandFile[14] = (byte)Convert.ToInt32(datalenthFile.Substring(0, 2), 16);
                    //sendcommandFile[15] = (byte)Convert.ToInt32("17", 16);
                    //sendcommandFile[16] = (byte)Convert.ToInt32("03", 16);
                    //sendcommandFile[17] = (byte)24;
                    //string pp2 = select.FILENAME.Substring(0, 15);
                    //byte[] array2 = Encoding.ASCII.GetBytes(pp2);
                    //for (int i = 0; i < array2.Length; i++)
                    //{//默认长度为15
                    //    sendcommandFile[18 + i] = array2[i];
                    //}
                    //string pp = select.FILENAME.Substring(15, 18);
                    //byte[] array = new byte[pp.Length / 2];
                    //for (int j = 0; j < pp.Length / 2; j++)
                    //{
                    //    //默认长度为9
                    //    array[j] = Convert.ToByte(pp.Substring(2 * j, 2).ToString(), 16);
                    //}

                    //for (int i = 0; i < 9; i++)
                    //{
                    //    sendcommandFile[33 + i] = array[i];
                    //}

                    //sendcommandFile[42] = (byte)Convert.ToInt32("21", 16);
                    //sendcommandFile[43] = (byte)Convert.ToInt32("03", 16);
                    //sendcommandFile[44] = (byte)4;
                    //for (int i = 0; i < 4; i++)
                    //{
                    //    sendcommandFile[45 + i] = select.PACKSTARTINDEX[i];
                    //}

                    //sendcommandFile[49] = (byte)Convert.ToInt32("39", 16);
                    //sendcommandFile[50] = (byte)Convert.ToInt32("03", 16);
                    //sendcommandFile[51] = (byte)1;
                    //sendcommandFile[52] = (byte)select.Audio_reback_mode;

                    //sendcommandFile[53] = (byte)Convert.ToInt32("14", 16);
                    //sendcommandFile[54] = (byte)Convert.ToInt32("03", 16);
                    //sendcommandFile[55] = (byte)4;

                    //for (int i = 0; i < 4; i++)
                    //{
                    //    sendcommandFile[56 + i] = (byte)(Convert.ToInt32(select.AUDIOREBACKSERVERIP.Split('.')[i]));
                    //}

                    //sendcommandFile[60] = (byte)Convert.ToInt32("15", 16);
                    //sendcommandFile[61] = (byte)Convert.ToInt32("03", 16);
                    //sendcommandFile[62] = (byte)2;
                    //string porttmp = select.AUDIOREBACKPORT.ToString("x").PadLeft(4, '0');
                    //sendcommandFile[63] = (byte)Convert.ToInt32(porttmp.Substring(2, 2), 16);
                    //sendcommandFile[64] = (byte)Convert.ToInt32(porttmp.Substring(0, 2), 16);

                    //sendcommandFile = HexHelper.crc16(sendcommandFile, 12);




                    //IntPtr connIdFiletransfer = default(IntPtr);
                    //if (this.dataHelper.Clients.TryGetValue(select.SRVPHYSICALCODE, out connIdFiletransfer))
                    //{
                    //    byte[] bytes = sendcommandFile;

                    //    string dadada = "";
                    //    for (int i = 0; i < bytes.Length; i++)
                    //    {
                    //        dadada += bytes[i].ToString("X2");
                    //    }

                    //    this.netServer.Send(connIdFiletransfer, bytes, bytes.Length);
                    //    //日志打印  20180305
                    //    log.Info("录音文件调取指令已发送");  
                    //}

                    break;
                case Equipment.TTS://文本转语音
                    log.Info("文本转语音指令进行组装发送阶段");  
                    TTSC ttsc = (TTSC)SendObject.Extras;

                    string tmp1 = "";
                    foreach (int item1 in ttsc.DeviceIdList)
                    {
                        tmp1 += " " + item1.ToString();
                    }
                    log.Info("文本转语音指令设备ID"+tmp1); 

                    List<string> PhysicalList = dataHelper.DecId2Physical(ttsc.DeviceIdList);

                    string tmp = "";

                    foreach (string item in PhysicalList)
                    {
                        tmp += " " + item;
                    }
                    log.Info("文本转语音指令物理码"+tmp); 
                    byte[] TextList = Encoding.Default.GetBytes(ttsc.TsCmd_Params);//字符串转ASCII

                    foreach (var phycode in PhysicalList)
                    {
                        if (TextList.Length <= (255 - 6) * 5)//一个UDP包能发完  终端接收缓冲区为1024字节 实际最多一帧4段发完
                        {
                            List<byte> SendCommandListTTS = new List<byte>();
                            SendCommandListTTS.Add(38);
                            byte[] HeaderData = new byte[] { };

                            if (dataHelper.ClientHD == null)
                            {
                                log.Info("文本转语音指令数据头为空");  
                                //终端未上报  建立连接
                                return;
                            }
                            if (!this.dataHelper.ClientHD.TryGetValue(phycode, out HeaderData))
                            {
                                //终端未上报  建立连接
                                log.Info("文本转语音指令的物理码搜不到数据头，当前物理码：" + phycode.ToString());  
                                continue;
                            }

                            for (int k = 0; k < 9; k++)
                            {
                                SendCommandListTTS.Add(HeaderData[1 + k]);
                            }
                            SendCommandListTTS.Add(HeaderData[16]);//协议版本号


                            int count = TextList.Length / 249 + 1;//段数  文本通过几段发送

                            switch (count)
                            {
                                case 1:
                                    //补充完整发送数据
                                    for (int i = 0; i < 6 + 2 + TextList.Length + 1 + 6; i++)
                                    {
                                        SendCommandListTTS.Add(0);
                                    }

                                    byte[] sendcommand = SendCommandListTTS.ToArray();

                                    string datalenth = (TextList.Length + 6 + 3).ToString("x").PadLeft(4, '0');

                                    sendcommand[12] = (byte)Convert.ToInt32("10", 16);
                                    sendcommand[13] = (byte)Convert.ToInt32(datalenth.Substring(2, 2), 16);
                                    sendcommand[14] = (byte)Convert.ToInt32(datalenth.Substring(0, 2), 16);
                                    sendcommand[15] = (byte)Convert.ToInt32("44", 16);
                                    sendcommand[16] = (byte)Convert.ToInt32("03", 16);
                                    sendcommand[17] = (byte)(TextList.Length + 6);

                                    sendcommand[18] = (byte)0;
                                    sendcommand[19] = (byte)ttsc.TsCmd_PlayCount;


                                    sendcommand[20] = (byte)Convert.ToInt32(ttsc.FileID.Substring(0, 2), 16);
                                    sendcommand[21] = (byte)Convert.ToInt32(ttsc.FileID.Substring(2, 2), 16);
                                    sendcommand[22] = (byte)1;//分段序号
                                    sendcommand[23] = (byte)1;//总段数


                                    for (int l = 0; l < TextList.Length; l++)
                                    {
                                        sendcommand[24 + l] = TextList[l];
                                    }
                                    sendcommand = HexHelper.crc16(sendcommand, 12);
                                    IntPtr connIdFile = default(IntPtr);
                                    if (this.dataHelper.Clients.TryGetValue(phycode, out connIdFile))
                                    {
                                        byte[] bytes = sendcommand;
                                        this.netServer.Send(connIdFile, bytes, bytes.Length);
                                        //日志打印  20180305
                                        log.Info("文本转语音指令已发送");  
                                    }
                                    break;
                                case 2:

                                    for (int i = 0; i < TextList.Length + 24; i++)
                                    {
                                        SendCommandListTTS.Add(0);
                                    }

                                    byte[] sendcommandSection2 = SendCommandListTTS.ToArray();

                                    string datalenthSection2 = (TextList.Length + 18).ToString("x").PadLeft(4, '0');

                                    sendcommandSection2[12] = (byte)Convert.ToInt32("10", 16);
                                    sendcommandSection2[13] = (byte)Convert.ToInt32(datalenthSection2.Substring(2, 2), 16);
                                    sendcommandSection2[14] = (byte)Convert.ToInt32(datalenthSection2.Substring(0, 2), 16);


                                    #region 数据段1
                                    sendcommandSection2[15] = (byte)Convert.ToInt32("44", 16);
                                    sendcommandSection2[16] = (byte)Convert.ToInt32("03", 16);
                                    sendcommandSection2[17] = (byte)255;
                                    sendcommandSection2[18] = (byte)0;
                                    sendcommandSection2[19] = (byte)ttsc.TsCmd_PlayCount;
                                    sendcommandSection2[20] = (byte)Convert.ToInt32(ttsc.FileID.Substring(0, 2), 16);
                                    sendcommandSection2[21] = (byte)Convert.ToInt32(ttsc.FileID.Substring(2, 2), 16);
                                    sendcommandSection2[22] = (byte)1;//分段序号
                                    sendcommandSection2[23] = (byte)2;//总段数
                                    for (int l = 0; l < 249; l++)
                                    {
                                        sendcommandSection2[24 + l] = TextList[l];
                                    }
                                    #endregion

                                    #region 数据段2
                                    sendcommandSection2[273] = (byte)Convert.ToInt32("44", 16);
                                    sendcommandSection2[274] = (byte)Convert.ToInt32("03", 16);
                                    sendcommandSection2[275] = (byte)(TextList.Length - 249 + 6);
                                    sendcommandSection2[276] = (byte)0;
                                    sendcommandSection2[277] = (byte)ttsc.TsCmd_PlayCount;
                                    sendcommandSection2[278] = (byte)Convert.ToInt32(ttsc.FileID.Substring(0, 2), 16);
                                    sendcommandSection2[279] = (byte)Convert.ToInt32(ttsc.FileID.Substring(2, 2), 16);
                                    sendcommandSection2[280] = (byte)2;//分段序号
                                    sendcommandSection2[281] = (byte)2;//总段数

                                    for (int l = 0; l < TextList.Length - 249; l++)
                                    {
                                        sendcommandSection2[282 + l] = TextList[249 + l];
                                    }
                                    #endregion

                                    sendcommandSection2 = HexHelper.crc16(sendcommandSection2, 12);
                                    IntPtr connIdFileTTS2 = default(IntPtr);
                                    if (this.dataHelper.Clients.TryGetValue(phycode, out connIdFileTTS2))
                                    {
                                        byte[] bytes = sendcommandSection2;
                                        this.netServer.Send(connIdFileTTS2, bytes, bytes.Length);
                                        //日志打印  20180305
                                        log.Info("文本转语音指令已发送");  
                                    }
                                    break;
                                case 3:

                                    for (int i = 0; i < TextList.Length + 33; i++)
                                    {
                                        SendCommandListTTS.Add(0);
                                    }

                                    byte[] sendcommandSection3 = SendCommandListTTS.ToArray();

                                    string datalenthSection3 = (TextList.Length + 27).ToString("x").PadLeft(4, '0');

                                    sendcommandSection3[12] = (byte)Convert.ToInt32("10", 16);
                                    sendcommandSection3[13] = (byte)Convert.ToInt32(datalenthSection3.Substring(2, 2), 16);
                                    sendcommandSection3[14] = (byte)Convert.ToInt32(datalenthSection3.Substring(0, 2), 16);

                                    #region 数据段1
                                    sendcommandSection3[15] = (byte)Convert.ToInt32("44", 16);
                                    sendcommandSection3[16] = (byte)Convert.ToInt32("03", 16);
                                    sendcommandSection3[17] = (byte)255;
                                    sendcommandSection3[18] = (byte)0;
                                    sendcommandSection3[19] = (byte)ttsc.TsCmd_PlayCount;
                                    sendcommandSection3[20] = (byte)Convert.ToInt32(ttsc.FileID.Substring(0, 2), 16);
                                    sendcommandSection3[21] = (byte)Convert.ToInt32(ttsc.FileID.Substring(2, 2), 16);
                                    sendcommandSection3[22] = (byte)1;//分段序号
                                    sendcommandSection3[23] = (byte)3;//总段数
                                    for (int l = 0; l < 249; l++)
                                    {
                                        sendcommandSection3[24 + l] = TextList[l];
                                    }
                                    #endregion

                                    #region 数据段2
                                    sendcommandSection3[273] = (byte)Convert.ToInt32("44", 16);
                                    sendcommandSection3[274] = (byte)Convert.ToInt32("03", 16);
                                    sendcommandSection3[275] = (byte)255;
                                    sendcommandSection3[276] = (byte)0;
                                    sendcommandSection3[277] = (byte)ttsc.TsCmd_PlayCount;
                                    sendcommandSection3[278] = (byte)Convert.ToInt32(ttsc.FileID.Substring(0, 2), 16);
                                    sendcommandSection3[279] = (byte)Convert.ToInt32(ttsc.FileID.Substring(2, 2), 16);
                                    sendcommandSection3[280] = (byte)2;//分段序号
                                    sendcommandSection3[281] = (byte)3;//总段数

                                    for (int l = 0; l < 249; l++)
                                    {
                                        sendcommandSection3[282 + l] = TextList[249 + l];
                                    }
                                    #endregion

                                    #region 数据段3
                                    sendcommandSection3[531] = (byte)Convert.ToInt32("44", 16);
                                    sendcommandSection3[532] = (byte)Convert.ToInt32("03", 16);
                                    sendcommandSection3[533] = (byte)(TextList.Length - 249 * 2 + 6);
                                    sendcommandSection3[534] = (byte)0;
                                    sendcommandSection3[535] = (byte)ttsc.TsCmd_PlayCount;
                                    sendcommandSection3[536] = (byte)Convert.ToInt32(ttsc.FileID.Substring(0, 2), 16);
                                    sendcommandSection3[537] = (byte)Convert.ToInt32(ttsc.FileID.Substring(2, 2), 16);
                                    sendcommandSection3[538] = (byte)3;//分段序号
                                    sendcommandSection3[539] = (byte)3;//总段数

                                    for (int l = 0; l < TextList.Length - 249 * 2; l++)
                                    {
                                        sendcommandSection3[540 + l] = TextList[498 + l];
                                    }
                                    #endregion

                                    sendcommandSection3 = HexHelper.crc16(sendcommandSection3, 12);
                                    IntPtr connIdFileTTS3 = default(IntPtr);
                                    if (this.dataHelper.Clients.TryGetValue(phycode, out connIdFileTTS3))
                                    {
                                        byte[] bytes = sendcommandSection3;
                                        this.netServer.Send(connIdFileTTS3, bytes, bytes.Length);
                                        //日志打印  20180305
                                        log.Info("文本转语音指令已发送");  
                                    }
                                    break;
                                case 4:

                                       for (int i = 0; i < TextList.Length + 42; i++)
                                    {
                                        SendCommandListTTS.Add(0);
                                    }

                                    byte[] sendcommandSection4 = SendCommandListTTS.ToArray();

                                    string datalenthSection4 = (TextList.Length + 36).ToString("x").PadLeft(4, '0');

                                    sendcommandSection4[12] = (byte)Convert.ToInt32("10", 16);
                                    sendcommandSection4[13] = (byte)Convert.ToInt32(datalenthSection4.Substring(2, 2), 16);
                                    sendcommandSection4[14] = (byte)Convert.ToInt32(datalenthSection4.Substring(0, 2), 16);

                                    #region 数据段1
                                    sendcommandSection4[15] = (byte)Convert.ToInt32("44", 16);
                                    sendcommandSection4[16] = (byte)Convert.ToInt32("03", 16);
                                    sendcommandSection4[17] = (byte)255;
                                    sendcommandSection4[18] = (byte)0;
                                    sendcommandSection4[19] = (byte)ttsc.TsCmd_PlayCount;
                                    sendcommandSection4[20] = (byte)Convert.ToInt32(ttsc.FileID.Substring(0, 2), 16);
                                    sendcommandSection4[21] = (byte)Convert.ToInt32(ttsc.FileID.Substring(2, 2), 16);
                                    sendcommandSection4[22] = (byte)1;//分段序号
                                    sendcommandSection4[23] = (byte)4;//总段数
                                    for (int l = 0; l < 249; l++)
                                    {
                                        sendcommandSection4[24 + l] = TextList[l];
                                    }
                                    #endregion

                                    #region 数据段2
                                    sendcommandSection4[273] = (byte)Convert.ToInt32("44", 16);
                                    sendcommandSection4[274] = (byte)Convert.ToInt32("03", 16);
                                    sendcommandSection4[275] = (byte)255;
                                    sendcommandSection4[276] = (byte)0;
                                    sendcommandSection4[277] = (byte)ttsc.TsCmd_PlayCount;
                                    sendcommandSection4[278] = (byte)Convert.ToInt32(ttsc.FileID.Substring(0, 2), 16);
                                    sendcommandSection4[279] = (byte)Convert.ToInt32(ttsc.FileID.Substring(2, 2), 16);
                                    sendcommandSection4[280] = (byte)2;//分段序号
                                    sendcommandSection4[281] = (byte)4;//总段数

                                    for (int l = 0; l < 249; l++)
                                    {
                                        sendcommandSection4[282 + l] = TextList[249 + l];
                                    }
                                    #endregion

                                    #region 数据段3
                                    sendcommandSection4[531] = (byte)Convert.ToInt32("44", 16);
                                    sendcommandSection4[532] = (byte)Convert.ToInt32("03", 16);
                                    sendcommandSection4[533] = (byte)255;
                                    sendcommandSection4[534] = (byte)0;
                                    sendcommandSection4[535] = (byte)ttsc.TsCmd_PlayCount;
                                    sendcommandSection4[536] = (byte)Convert.ToInt32(ttsc.FileID.Substring(0, 2), 16);
                                    sendcommandSection4[537] = (byte)Convert.ToInt32(ttsc.FileID.Substring(2, 2), 16);
                                    sendcommandSection4[538] = (byte)3;//分段序号
                                    sendcommandSection4[539] = (byte)4;//总段数

                                    for (int l = 0; l < 249; l++)
                                    {
                                        sendcommandSection4[540 + l] = TextList[498 + l];
                                    }
                                    #endregion

                                    #region 数据段4
                                    sendcommandSection4[789] = (byte)Convert.ToInt32("44", 16);
                                    sendcommandSection4[790] = (byte)Convert.ToInt32("03", 16);
                                    sendcommandSection4[791] = (byte)(TextList.Length - 249 * 3 + 6);
                                    sendcommandSection4[792] = (byte)0;
                                    sendcommandSection4[793] = (byte)ttsc.TsCmd_PlayCount;
                                    sendcommandSection4[794] = (byte)Convert.ToInt32(ttsc.FileID.Substring(0, 2), 16);
                                    sendcommandSection4[795] = (byte)Convert.ToInt32(ttsc.FileID.Substring(2, 2), 16);
                                    sendcommandSection4[796] = (byte)4;//分段序号
                                    sendcommandSection4[797] = (byte)4;//总段数

                                    for (int l = 0; l < TextList.Length - 249 * 3; l++)
                                    {
                                        sendcommandSection4[798 + l] = TextList[747 + l];
                                    }
                                    #endregion

                                    sendcommandSection4= HexHelper.crc16(sendcommandSection4, 12);
                                    IntPtr connIdFileTTS4 = default(IntPtr);
                                    if (this.dataHelper.Clients.TryGetValue(phycode, out connIdFileTTS4))
                                    {
                                        byte[] bytes = sendcommandSection4;
                                        this.netServer.Send(connIdFileTTS4, bytes, bytes.Length);
                                        //日志打印  20180305
                                        log.Info("文本转语音指令已发送");  
                                    }

                                    break;
                                case 5:
                                    break;

                            }
                        }
                        else
                        {
                            //需分包发送  目前暂无这么大的数据
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 初始化数据库信息
        /// </summary>
        public void InitDatabase()
        {
            if (DataBase == null)
            {
                DataBase = new DBHelper();
            }
            string serverName = ini.ReadValue("Database", "ServerName");
            string database = ini.ReadValue("Database", "DataBase");
            string logID = ini.ReadValue("Database", "LogID");
            string logPass = ini.ReadValue("Database", "LogPass");
            DataBase.SetConnectString(serverName, logID, logPass, database);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
               m_mq.CreateProducer(false, "transfer");


                m_mq.SendMQMessage("你好");
            }
            catch (Exception)
            {
                
              
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //清屏
                if (this.timercounter < 1800)
                {
                    this.timercounter++;
                }
                else
                {
                    this.timercounter = 0;
                    this.richTextRebackMsg.Text = "";
                }
            }
            catch (Exception)
            {
               
            }
        }


        /// <summary>
        /// 测试  手动发送升级
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string strMsg = "PACKETTYPE~UPGRADE|NEWVERSION~102|OLDVERSION~101|UPGRADEMODE~0|HTTPURL~http://192.168.4.87:80/upload/111.txt|LIST~0102030455";

                TextForm form = new TextForm(strMsg);
                form.ShowDialog();
                if (form.IsSend)
                {
                    strMsg = form.textBox1.Text;
                    dataHelper.Serialize(strMsg);
                }
            }
            catch (Exception)
            {
                
            }
        }

        /// <summary>
        /// 测试 手动发送文本转语音
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //string strMsg = "PACKETTYPE~TTS|TsCmd_Mode~终端|TsCmd_Type~TTS播放|TsCmd_UserID~3|TsCmd_ValueID~26622,26624|TsCmd_Params~久远的一篇名唤想中的理想大约是十二三岁时写的以前还有可惜散失了我还记得最初的一篇小说是一个无题的家庭伦理悲剧关于一个小康之家姓云娶了个媳妇名叫月娥小姑叫风娥哥哥出门经商去了于是风娥便乘机定下计策来谋害嫂嫂写到这里便搁下了没有续下去另起炉灶写一篇历史小说开头是话说隋末唐初的时候我喜欢那时候那仿佛是一个兴兴轰轰红色的时代我记得这|TsCmd_Date~2018-01-19 13:17:44|TsCmd_Status~0|USER_PRIORITY~0|USER_ORG_CODE~P10|TsCmd_PlayCount~2";


             //   string strMsg = "PACKETTYPE~TTS|TsCmd_Mode~终端|TsCmd_Type~TTS播放|TsCmd_UserID~3|TsCmd_ValueID~26622,26624|TsCmd_Params~久远的一篇名唤想中的理想大约是十二三岁时写的以前还有可惜散失了我还记得最初的一篇小说是一个无题的|TsCmd_Date~2018-01-19 13:17:44|TsCmd_Status~0|USER_PRIORITY~0|USER_ORG_CODE~P10|TsCmd_PlayCount~2";


                //string strMsg = "PACKETTYPE~TTS|TsCmd_Mode~区域|TsCmd_Type~TTS播放|TsCmd_UserID~1|TsCmd_ValueID~3|TsCmd_Params~12|TsCmd_Date~2018-03-07 14:36:38|TsCmd_Status~0|USER_PRIORITY~0|USER_ORG_CODE~P37|TsCmd_PlayCount~1";

                string strMsg = "PACKETTYPE~TTS|TsCmd_Mode~终端|TsCmd_Type~TTS播放|TsCmd_UserID~1|TsCmd_ValueID~1|TsCmd_Params~12|TsCmd_Date~2018-03-07 14:36:38|TsCmd_Status~0|USER_PRIORITY~0|USER_ORG_CODE~P37|TsCmd_PlayCount~1";

               // string strMsg = "PACKETTYPE~TTS|TsCmd_Mode~终端|TsCmd_Type~TTS播放|TsCmd_UserID~3|TsCmd_ValueID~26622,26624|TsCmd_Params~久远的一篇名唤想中的理想大约是十二三岁时写的以前还有可惜散失了我还记得最初的一篇小说是一个无题的家庭伦理悲剧关于一个小康之家姓云娶了个媳妇名叫月娥小姑叫风娥哥哥出门经商去了于是风娥便乘机定下计策来谋害嫂嫂写到这里便搁下了没有续下去另起炉灶写一篇历史小说开头是话说隋末唐初的时候我喜欢那时候那仿佛是一个兴兴轰轰红色的时代我记得这最初的一篇小说是一个无题的家庭伦理悲剧关于一个小康之家姓云娶了个媳妇名叫月娥小姑叫风娥哥哥出门经商去了于是风娥便乘机定下计策来谋害嫂嫂写到这里便搁下了没有续下去另起炉灶写一篇历史小说开头是话说隋末唐初的时候我喜欢那时候那仿佛是一个兴兴轰轰红色的时代我记得这最初的一篇小说是一个无题的家庭伦理悲剧关于一个小康之家姓云娶了个媳妇色的时代我记得这|TsCmd_Date~2018-01-19 13:17:44|TsCmd_Status~0|USER_PRIORITY~0|USER_ORG_CODE~P10|TsCmd_PlayCount~2";
              //  dataHelper.Serialize(strMsg);


                TextForm form = new TextForm(strMsg);
                form.ShowDialog();
                if (form.IsSend)
                {
                    strMsg = form.textBox1.Text;
                    dataHelper.Serialize(strMsg);
                }
            }
            catch (Exception)
            {
                
            }
        }

        /// <summary>
        /// 音频文件调取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
              //  string strMsg = "PACKETTYPE~TRANSFER|FILENAME~S20171204235236060545140210420202.mp3|SRVPHYSICALCODE~0017042752|PACKSTARTINDEX~00000001";
                string strMsg = "PACKETTYPE~TRANSFER|FILENAME~B20740131003632000000001122334500.wav|SRVPHYSICALCODE~0000000000|PACKSTARTINDEX~00000001|AUDIOREBACKSERVERIP~192.168.4.109|AUDIOREBACKPORT~8520";
             
                
              //  string strMsg = "PACKETTYPE~TRANSFER|FILENAME~B20000109235934000000000000000000.wav|SRVPHYSICALCODE~0102030455|PACKSTARTINDEX~00000001|AUDIOREBACKSERVERIP~192.168.4.109|AUDIOREBACKPORT~8520";  测试注释   20180301


                TextForm form = new TextForm(strMsg);
                form.ShowDialog();
                if (form.IsSend)
                {
                    strMsg = form.textBox1.Text;
                    dataHelper.Serialize(strMsg);
                }


               // dataHelper.Serialize(strMsg);
            }
            catch (Exception)
            {
                
            }
        }

    }
}
