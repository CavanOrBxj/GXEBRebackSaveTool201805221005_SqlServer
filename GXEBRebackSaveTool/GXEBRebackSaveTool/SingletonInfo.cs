using System.Threading;
using System.Collections.Generic;
using System.Data;
using System.Collections.Concurrent;
using GXEBRebackSaveTool.Models;

namespace GXEBRebackSaveTool
{
    public class SingletonInfo
    {
        private static SingletonInfo _singleton;

        public string  factorycode;
        public bool TerminalType;//true 表示为代号02/03  false 表示非代号02/03
        public string Version;//软件版本号

        public string remoteIP;//远程IP  
        public int remotePort;//远程

     //   public List<MonitorPlatformMessage> MonitorPlatformMessageList;
        public Dictionary<string, byte[]> HistoryHeaderData;//历史接收到的数据头

        public int FileID;//文件ID  平台发送文本到服务 服务生成一个文件ID（每次自增 最大65535）  

        public string FTPServer;
        public string FTPPort;
        public string FTPUserName;
        public string FTPPwd;
        public string ftppath;

        private SingletonInfo()                                                                 
        {
            factorycode = ""; //
            TerminalType = false;
            Version = "";
         //   MonitorPlatformMessageList = new List<MonitorPlatformMessage>();
            HistoryHeaderData = new Dictionary<string, byte[]>();
            FileID = 0;

            FTPServer = "";
            FTPPort = "";
            FTPUserName = "";
            FTPPwd = "";
            ftppath = "";
        }

   
        public static SingletonInfo GetInstance()
        {
            if (_singleton == null)
            {
                Interlocked.CompareExchange(ref _singleton, new SingletonInfo(), null);
            }
            return _singleton;
        }

   


    }
}