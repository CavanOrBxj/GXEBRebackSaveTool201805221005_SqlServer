namespace GXEBRebackSaveTool
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.textTcpPort = new System.Windows.Forms.TextBox();
            this.btnUpdateLocalHost = new System.Windows.Forms.Button();
            this.lblTcpPort = new System.Windows.Forms.Label();
            this.richTextRebackMsg = new System.Windows.Forms.RichTextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.textUdpPort = new System.Windows.Forms.TextBox();
            this.lblUdpPort = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textTcpPort
            // 
            this.textTcpPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textTcpPort.Location = new System.Drawing.Point(215, 511);
            this.textTcpPort.Name = "textTcpPort";
            this.textTcpPort.ReadOnly = true;
            this.textTcpPort.Size = new System.Drawing.Size(45, 21);
            this.textTcpPort.TabIndex = 43;
            this.textTcpPort.Text = "0";
            // 
            // btnUpdateLocalHost
            // 
            this.btnUpdateLocalHost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUpdateLocalHost.Location = new System.Drawing.Point(347, 506);
            this.btnUpdateLocalHost.Name = "btnUpdateLocalHost";
            this.btnUpdateLocalHost.Size = new System.Drawing.Size(107, 37);
            this.btnUpdateLocalHost.TabIndex = 39;
            this.btnUpdateLocalHost.Text = "服务设置";
            this.btnUpdateLocalHost.UseVisualStyleBackColor = true;
            this.btnUpdateLocalHost.Click += new System.EventHandler(this.btnUpdateLocalHost_Click);
            // 
            // lblTcpPort
            // 
            this.lblTcpPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTcpPort.AutoSize = true;
            this.lblTcpPort.Location = new System.Drawing.Point(161, 515);
            this.lblTcpPort.Name = "lblTcpPort";
            this.lblTcpPort.Size = new System.Drawing.Size(47, 12);
            this.lblTcpPort.TabIndex = 38;
            this.lblTcpPort.Text = "tcp端口";
            // 
            // richTextRebackMsg
            // 
            this.richTextRebackMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextRebackMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextRebackMsg.Location = new System.Drawing.Point(12, 12);
            this.richTextRebackMsg.Name = "richTextRebackMsg";
            this.richTextRebackMsg.Size = new System.Drawing.Size(759, 484);
            this.richTextRebackMsg.TabIndex = 33;
            this.richTextRebackMsg.Text = "";
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStart.Location = new System.Drawing.Point(549, 506);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(107, 37);
            this.btnStart.TabIndex = 44;
            this.btnStart.Text = "启动服务";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // textUdpPort
            // 
            this.textUdpPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textUdpPort.Location = new System.Drawing.Point(110, 512);
            this.textUdpPort.Name = "textUdpPort";
            this.textUdpPort.ReadOnly = true;
            this.textUdpPort.Size = new System.Drawing.Size(45, 21);
            this.textUdpPort.TabIndex = 46;
            this.textUdpPort.Text = "0";
            // 
            // lblUdpPort
            // 
            this.lblUdpPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblUdpPort.AutoSize = true;
            this.lblUdpPort.Location = new System.Drawing.Point(20, 515);
            this.lblUdpPort.Name = "lblUdpPort";
            this.lblUdpPort.Size = new System.Drawing.Size(83, 12);
            this.lblUdpPort.TabIndex = 45;
            this.lblUdpPort.Text = "绑定的udp端口";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(484, 266);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(107, 37);
            this.button1.TabIndex = 47;
            this.button1.Text = "MQ发送测试";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Location = new System.Drawing.Point(599, 334);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(107, 37);
            this.button2.TabIndex = 48;
            this.button2.Text = "升级_测试指令";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Location = new System.Drawing.Point(443, 433);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(167, 37);
            this.button3.TabIndex = 49;
            this.button3.Text = "文本转语音_测试指令";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button4.Location = new System.Drawing.Point(549, 390);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(145, 37);
            this.button4.TabIndex = 50;
            this.button4.Text = "音频调取_测试指令";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(297, 542);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(569, 12);
            this.label1.TabIndex = 51;
            this.label1.Text = "注：测试指令为写死在程序中的指令，点击即可发送；实际场景为融合平台发送，然后回传服务转发至终端";
            this.label1.Visible = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 551);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textUdpPort);
            this.Controls.Add(this.lblUdpPort);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.textTcpPort);
            this.Controls.Add(this.btnUpdateLocalHost);
            this.Controls.Add(this.lblTcpPort);
            this.Controls.Add(this.richTextRebackMsg);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "应急广播回传消息服务";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textTcpPort;
        private System.Windows.Forms.Button btnUpdateLocalHost;
        private System.Windows.Forms.Label lblTcpPort;
        private System.Windows.Forms.RichTextBox richTextRebackMsg;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox textUdpPort;
        private System.Windows.Forms.Label lblUdpPort;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label1;
    }
}

