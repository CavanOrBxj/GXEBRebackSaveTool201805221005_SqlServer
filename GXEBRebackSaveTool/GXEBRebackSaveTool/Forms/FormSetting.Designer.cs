namespace GXEBRebackSaveTool.Forms
{
    partial class FormSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSet = new System.Windows.Forms.Button();
            this.gbSetting = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDb = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDbPass = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDbuser = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.groupHost = new System.Windows.Forms.GroupBox();
            this.cbBoxIP = new System.Windows.Forms.ComboBox();
            this.textUdpPort = new System.Windows.Forms.TextBox();
            this.lblUdpPort = new System.Windows.Forms.Label();
            this.textIP = new System.Windows.Forms.TextBox();
            this.textTcpPort = new System.Windows.Forms.TextBox();
            this.lblTcpPort = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblTip = new System.Windows.Forms.Label();
            this.btnDBTest = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textMQPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTopiName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtMQServer = new System.Windows.Forms.TextBox();
            this.gbSetting.SuspendLayout();
            this.groupHost.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSet
            // 
            this.btnSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSet.Location = new System.Drawing.Point(270, 500);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(97, 31);
            this.btnSet.TabIndex = 14;
            this.btnSet.Text = "设 置";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // gbSetting
            // 
            this.gbSetting.Controls.Add(this.label7);
            this.gbSetting.Controls.Add(this.txtDb);
            this.gbSetting.Controls.Add(this.label6);
            this.gbSetting.Controls.Add(this.txtDbPass);
            this.gbSetting.Controls.Add(this.label5);
            this.gbSetting.Controls.Add(this.txtDbuser);
            this.gbSetting.Controls.Add(this.label3);
            this.gbSetting.Controls.Add(this.txtServer);
            this.gbSetting.Location = new System.Drawing.Point(22, 162);
            this.gbSetting.Name = "gbSetting";
            this.gbSetting.Size = new System.Drawing.Size(345, 174);
            this.gbSetting.TabIndex = 16;
            this.gbSetting.TabStop = false;
            this.gbSetting.Text = "数据库连接设置";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(63, 69);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 22;
            this.label7.Text = "数据库";
            // 
            // txtDb
            // 
            this.txtDb.Location = new System.Drawing.Point(124, 66);
            this.txtDb.Name = "txtDb";
            this.txtDb.Size = new System.Drawing.Size(179, 21);
            this.txtDb.TabIndex = 21;
            this.txtDb.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(51, 139);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 20;
            this.label6.Text = "登录密码";
            // 
            // txtDbPass
            // 
            this.txtDbPass.Location = new System.Drawing.Point(124, 136);
            this.txtDbPass.Name = "txtDbPass";
            this.txtDbPass.Size = new System.Drawing.Size(179, 21);
            this.txtDbPass.TabIndex = 19;
            this.txtDbPass.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(63, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 18;
            this.label5.Text = "用户名";
            // 
            // txtDbuser
            // 
            this.txtDbuser.Location = new System.Drawing.Point(124, 101);
            this.txtDbuser.Name = "txtDbuser";
            this.txtDbuser.Size = new System.Drawing.Size(179, 21);
            this.txtDbuser.TabIndex = 17;
            this.txtDbuser.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(63, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 16;
            this.label3.Text = "服务器";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(124, 31);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(179, 21);
            this.txtServer.TabIndex = 15;
            this.txtServer.TabStop = false;
            // 
            // groupHost
            // 
            this.groupHost.Controls.Add(this.cbBoxIP);
            this.groupHost.Controls.Add(this.textUdpPort);
            this.groupHost.Controls.Add(this.lblUdpPort);
            this.groupHost.Controls.Add(this.textIP);
            this.groupHost.Controls.Add(this.textTcpPort);
            this.groupHost.Controls.Add(this.lblTcpPort);
            this.groupHost.Controls.Add(this.lblIP);
            this.groupHost.Location = new System.Drawing.Point(22, 10);
            this.groupHost.Name = "groupHost";
            this.groupHost.Size = new System.Drawing.Size(345, 152);
            this.groupHost.TabIndex = 17;
            this.groupHost.TabStop = false;
            this.groupHost.Text = "Host设置";
            // 
            // cbBoxIP
            // 
            this.cbBoxIP.FormattingEnabled = true;
            this.cbBoxIP.Location = new System.Drawing.Point(122, 30);
            this.cbBoxIP.Name = "cbBoxIP";
            this.cbBoxIP.Size = new System.Drawing.Size(181, 20);
            this.cbBoxIP.TabIndex = 23;
            // 
            // textUdpPort
            // 
            this.textUdpPort.Location = new System.Drawing.Point(122, 70);
            this.textUdpPort.Name = "textUdpPort";
            this.textUdpPort.Size = new System.Drawing.Size(181, 21);
            this.textUdpPort.TabIndex = 22;
            this.textUdpPort.Text = "7001";
            // 
            // lblUdpPort
            // 
            this.lblUdpPort.AutoSize = true;
            this.lblUdpPort.Location = new System.Drawing.Point(45, 73);
            this.lblUdpPort.Name = "lblUdpPort";
            this.lblUdpPort.Size = new System.Drawing.Size(59, 12);
            this.lblUdpPort.TabIndex = 21;
            this.lblUdpPort.Text = "UDP端口号";
            // 
            // textIP
            // 
            this.textIP.Location = new System.Drawing.Point(122, 43);
            this.textIP.Name = "textIP";
            this.textIP.ReadOnly = true;
            this.textIP.Size = new System.Drawing.Size(181, 21);
            this.textIP.TabIndex = 19;
            this.textIP.Text = "127.0.0.1";
            this.textIP.Visible = false;
            // 
            // textTcpPort
            // 
            this.textTcpPort.Location = new System.Drawing.Point(122, 111);
            this.textTcpPort.Name = "textTcpPort";
            this.textTcpPort.Size = new System.Drawing.Size(181, 21);
            this.textTcpPort.TabIndex = 18;
            this.textTcpPort.Text = "7002";
            // 
            // lblTcpPort
            // 
            this.lblTcpPort.AutoSize = true;
            this.lblTcpPort.Location = new System.Drawing.Point(45, 114);
            this.lblTcpPort.Name = "lblTcpPort";
            this.lblTcpPort.Size = new System.Drawing.Size(59, 12);
            this.lblTcpPort.TabIndex = 17;
            this.lblTcpPort.Text = "TCP端口号";
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(63, 33);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(41, 12);
            this.lblIP.TabIndex = 16;
            this.lblIP.Text = "服务IP";
            // 
            // lblTip
            // 
            this.lblTip.AutoSize = true;
            this.lblTip.ForeColor = System.Drawing.Color.Red;
            this.lblTip.Location = new System.Drawing.Point(126, 475);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(137, 12);
            this.lblTip.TabIndex = 18;
            this.lblTip.Text = "服务启动后参数设置无效";
            // 
            // btnDBTest
            // 
            this.btnDBTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDBTest.Location = new System.Drawing.Point(22, 500);
            this.btnDBTest.Name = "btnDBTest";
            this.btnDBTest.Size = new System.Drawing.Size(97, 31);
            this.btnDBTest.TabIndex = 19;
            this.btnDBTest.Text = "测试连接";
            this.btnDBTest.UseVisualStyleBackColor = true;
            this.btnDBTest.Click += new System.EventHandler(this.btnDBTest_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textMQPort);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtTopiName);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtMQServer);
            this.groupBox1.Location = new System.Drawing.Point(22, 336);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(345, 151);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MQ设置";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 22;
            this.label1.Text = "MQ端口号";
            // 
            // textMQPort
            // 
            this.textMQPort.Location = new System.Drawing.Point(124, 66);
            this.textMQPort.Name = "textMQPort";
            this.textMQPort.Size = new System.Drawing.Size(179, 21);
            this.textMQPort.TabIndex = 21;
            this.textMQPort.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 18;
            this.label4.Text = "TopicName";
            // 
            // txtTopiName
            // 
            this.txtTopiName.Location = new System.Drawing.Point(124, 101);
            this.txtTopiName.Name = "txtTopiName";
            this.txtTopiName.Size = new System.Drawing.Size(179, 21);
            this.txtTopiName.TabIndex = 17;
            this.txtTopiName.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(63, 34);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 16;
            this.label8.Text = "MQIP";
            // 
            // txtMQServer
            // 
            this.txtMQServer.Location = new System.Drawing.Point(124, 31);
            this.txtMQServer.Name = "txtMQServer";
            this.txtMQServer.Size = new System.Drawing.Size(179, 21);
            this.txtMQServer.TabIndex = 15;
            this.txtMQServer.TabStop = false;
            // 
            // FormSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 543);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnDBTest);
            this.Controls.Add(this.lblTip);
            this.Controls.Add(this.groupHost);
            this.Controls.Add(this.gbSetting);
            this.Controls.Add(this.btnSet);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSetting";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "服务设置";
            this.gbSetting.ResumeLayout(false);
            this.gbSetting.PerformLayout();
            this.groupHost.ResumeLayout(false);
            this.groupHost.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.GroupBox gbSetting;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtDb;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDbPass;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDbuser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.GroupBox groupHost;
        private System.Windows.Forms.TextBox textIP;
        private System.Windows.Forms.TextBox textTcpPort;
        private System.Windows.Forms.Label lblTcpPort;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.TextBox textUdpPort;
        private System.Windows.Forms.Label lblUdpPort;
        private System.Windows.Forms.Label lblTip;
        private System.Windows.Forms.Button btnDBTest;
        private System.Windows.Forms.ComboBox cbBoxIP;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textMQPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTopiName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtMQServer;
    }
}