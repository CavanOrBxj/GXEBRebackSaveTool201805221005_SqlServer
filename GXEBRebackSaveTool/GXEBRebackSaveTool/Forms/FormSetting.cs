using GXEBRebackSaveTool.Utils;
using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace GXEBRebackSaveTool.Forms
{
    public partial class FormSetting : Form
    {
        private IniFiles ini;
        private bool isServerRun;

        public FormSetting(bool isServerRun)
        {
            InitializeComponent();
            Load += FormSetting_Load;
            this.isServerRun = isServerRun;
            lblTip.Visible = isServerRun;
            btnSet.Enabled = !isServerRun;
        }

        private void FormSetting_Load(object sender, EventArgs e)
        {
            this.ini = new IniFiles(Path.Combine(Application.StartupPath, "gxreback.ini"));
            this.textTcpPort.Text = this.ini.ReadValue("LocalHost", "TCPLocalPort");
            this.textUdpPort.Text = this.ini.ReadValue("LocalHost", "UDPLocalPort");
            this.txtServer.Text = this.ini.ReadValue("Database", "ServerName");
            this.txtDb.Text = this.ini.ReadValue("Database", "DataBase");
            this.txtDbuser.Text = this.ini.ReadValue("Database", "LogID");
            this.txtDbPass.Text = this.ini.ReadValue("Database", "LogPass");
            this.txtMQServer.Text = this.ini.ReadValue("MQ", "MQIP");
            this.textMQPort.Text = this.ini.ReadValue("MQ", "MQPORT");
            this.txtTopiName.Text = this.ini.ReadValue("MQ", "TopicName");
            IPAddress[] ipArr = Dns.GetHostAddresses(Dns.GetHostName());
            IPAddress[] array = ipArr;
            foreach (IPAddress ip in array)
            {
                if (Regex.IsMatch(ip.ToString(), "^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$"))
                {
                    this.cbBoxIP.Items.Add(ip);
                }
            }
            this.cbBoxIP.Text = this.ini.ReadValue("LocalHost", "LoaclIP");
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(this.textTcpPort.Text.Trim(), "^([0-9]|[1-9]\\d|[1-9]\\d{2}|[1-9]\\d{3}|[1-5]\\d{4}|6[0-4]\\d{3}|65[0-4]\\d{2}|655[0-2]\\d|6553[0-5])$"))
            {
                MessageBox.Show("端口号应为0到65535的数字，请重新填写", "提示");
            }
            else if (!Regex.IsMatch(this.textUdpPort.Text.Trim(), "^([0-9]|[1-9]\\d|[1-9]\\d{2}|[1-9]\\d{3}|[1-5]\\d{4}|6[0-4]\\d{3}|65[0-4]\\d{2}|655[0-2]\\d|6553[0-5])$"))
            {
                MessageBox.Show("端口号应为0到65535的数字，请重新填写", "提示");
            }
            else if (this.textTcpPort.Text.Trim() == this.textUdpPort.Text.Trim())
            {
                MessageBox.Show("端口号不能重复，请重新填写", "提示");
            }
            else if (!Regex.IsMatch(this.textMQPort.Text.Trim(), "^([0-9]|[1-9]\\d|[1-9]\\d{2}|[1-9]\\d{3}|[1-5]\\d{4}|6[0-4]\\d{3}|65[0-4]\\d{2}|655[0-2]\\d|6553[0-5])$"))
            {
                MessageBox.Show("端口号应为0到65535的数字，请重新填写", "提示");
                this.textMQPort.Focus();
            }
            else if (!Regex.IsMatch(this.txtMQServer.Text, "^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$"))
            {
                MessageBox.Show("请输入合法IP", "提示");
                this.txtMQServer.Focus();
            }
            else if (!this.PingIp())
            {
                MessageBox.Show("MQ服务器连接失败，网络不通", "提示");
                this.txtMQServer.Focus();
            }
            else
            {
                if (FormMain.DataBase == null)
                {
                    FormMain.DataBase = new DBHelper();
                }
                FormMain.DataBase.SetConnectString(this.txtServer.Text.Trim(), this.txtDbuser.Text.Trim(), this.txtDbPass.Text.Trim(), this.txtDb.Text.Trim());
                this.ini.WriteValue("LocalHost", "LoaclIP", this.cbBoxIP.Text.Trim());
                this.ini.WriteValue("LocalHost", "TCPLocalPort", this.textTcpPort.Text.Trim());
                this.ini.WriteValue("LocalHost", "UDPLocalPort", this.textUdpPort.Text.Trim());
                this.ini.WriteValue("Database", "ServerName", this.txtServer.Text.Trim());
                this.ini.WriteValue("Database", "DataBase", this.txtDb.Text.Trim());
                this.ini.WriteValue("Database", "LogID", this.txtDbuser.Text.Trim());
                this.ini.WriteValue("Database", "LogPass", this.txtDbPass.Text.Trim());
                this.ini.WriteValue("MQ", "MQIP", this.txtMQServer.Text.Trim());
                this.ini.WriteValue("MQ", "MQPORT", this.textMQPort.Text.Trim());
                this.ini.WriteValue("MQ", "TopicName", this.txtTopiName.Text.Trim());
                base.DialogResult = DialogResult.OK;
            }
        }

        private void btnDBTest_Click(object sender, EventArgs e)
        {
            if (FormMain.DataBase == null)
            {
                FormMain.DataBase = new DBHelper();
            }
            if (!Regex.IsMatch(this.textMQPort.Text.Trim(), "^([0-9]|[1-9]\\d|[1-9]\\d{2}|[1-9]\\d{3}|[1-5]\\d{4}|6[0-4]\\d{3}|65[0-4]\\d{2}|655[0-2]\\d|6553[0-5])$"))
            {
                MessageBox.Show("端口号应为0到65535的数字，请重新填写", "提示");
                this.textMQPort.Focus();
            }
            else if (!Regex.IsMatch(this.txtMQServer.Text, "^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$"))
            {
                MessageBox.Show("请输入合法IP", "提示");
                this.txtMQServer.Focus();
            }
            else
            {
                FormMain.DataBase.SetConnectString(this.txtServer.Text.Trim(), this.txtDbuser.Text.Trim(), this.txtDbPass.Text.Trim(), this.txtDb.Text.Trim());
                this.btnDBTest.Text = "连接中...";
                Thread thread = new Thread((ThreadStart)delegate
                {
                    bool flag = FormMain.DataBase.OpenTest();
                    bool flag2 = this.PingIp();
                    if (!flag)
                    {
                        base.Invoke((Action)delegate
                        {
                            MessageBox.Show("数据库连接失败，请检查数据库配置", "提示");
                        });
                    }
                    if (!flag2)
                    {
                        base.Invoke((Action)delegate
                        {
                            MessageBox.Show("MQ服务器连接失败，网络不通", "提示");
                        });
                    }
                    if (flag && flag2)
                    {
                        base.Invoke((Action)delegate
                        {
                            MessageBox.Show("连接成功", "提示");
                        });
                    }
                    base.Invoke((Action)delegate
                    {
                        this.btnDBTest.Text = "连接测试";
                    });
                });
                thread.Start();
            }
        }

        private bool PingIp()
        {
            bool flag = false;
            Ping pingSender = new Ping();
            PingReply reply = pingSender.Send(this.txtMQServer.Text.Trim(), 120);
            if (reply.Status == IPStatus.Success)
            {
                flag = true;
            }
            return flag;
        }
    }
}
