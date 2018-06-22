using HPSocketCS;
using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace GXEBRebackSaveTool.Utils
{
    /// <summary>
    /// 支持tcp、udp
    /// </summary>
    class NetServer : IDisposable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(NetServer));

        public const string PATTERNIP = @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";
        public const string PATTERNPORT = @"^([0-9]|[1-9]\d|[1-9]\d{2}|[1-9]\d{3}|[1-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5])$";

        private TcpServer tcpServer = new TcpServer();
        private UdpServer udpServer = new UdpServer();

        //tcp客户端信息
        private Extra<ClientInfo> tcpExtra = new Extra<ClientInfo>();
        //udp客户端信息
        private Extra<ClientInfo> udpExtra = new Extra<ClientInfo>();

        public event EventHandler<SocketDataEventArgs> UDPReceiveData;
        public event EventHandler<SocketDataEventArgs> TCPReceiveData;

        public NetServer(ushort tcpPort, ushort udpPort, string ip)
        {
            tcpServer.IpAddress = ip;// "127.0.0.1";
            tcpServer.Port = tcpPort;
            tcpServer.OnAccept += TcpServer_OnAccept;
            tcpServer.OnReceive += TcpServer_OnReceive;
            tcpServer.OnClose += TcpServer_OnClose;
            tcpServer.OnShutdown += TcpServer_OnShutdown;

            udpServer.IpAddress = ip;//"127.0.0.1";
            udpServer.Port = udpPort;
            udpServer.OnAccept += UdpServer_OnAccept;
            udpServer.OnReceive += UdpServer_OnReceive;
            udpServer.OnClose += UdpServer_OnClose;
            udpServer.OnShutdown += UdpServer_OnShutdown;
        }

        public void Start()
        {
            bool isTcpStart = tcpServer.Start();
            bool isUdpStart = udpServer.Start();
        }

        public void Stop()
        {
            bool isTcpStop = tcpServer.Stop();
            bool isUdpStop = udpServer.Stop();
        }

        private HandleResult UdpServer_OnClose(IntPtr connId, SocketOperation enOperation, int errorCode)
        {
            udpExtra.Remove(connId);
            return HandleResult.Ok;
        }

        private HandleResult UdpServer_OnShutdown()
        {
            log.Info("UDP服务器关闭");
            return HandleResult.Ok;
        }

        private HandleResult UdpServer_OnReceive(IntPtr connId, byte[] bytes)
        {
            string ip = string.Empty;
            ushort port = 0;
            udpServer.GetRemoteAddress(connId, ref ip, ref port);
            System.Net.IPEndPoint point = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ip), port);
            if (UDPReceiveData != null) UDPReceiveData(this, new SocketDataEventArgs(point, bytes, "", connId));
            return HandleResult.Ok;
        }

        private HandleResult UdpServer_OnAccept(IntPtr connId, IntPtr pClient)
        {
            string ip = string.Empty;
            ushort port = 0;
            udpServer.GetRemoteAddress(connId, ref ip, ref port);
            // 设置附加数据
            ClientInfo clientInfo = new ClientInfo();
            clientInfo.ConnId = connId;
            clientInfo.IpAddress = ip;
            clientInfo.Port = port;
            udpExtra.Set(connId, clientInfo);
            return HandleResult.Ok;
        }

        private HandleResult TcpServer_OnShutdown()
        {
            log.Info("TCP服务器关闭");
            return HandleResult.Ok;
        }

        private HandleResult TcpServer_OnClose(IntPtr connId, SocketOperation enOperation, int errorCode)
        {
            tcpExtra.Remove(connId);
            log.Info("客户端连接关闭，状态：" + enOperation + "  错误码：" + errorCode);
            return HandleResult.Ok;
        }

        private HandleResult TcpServer_OnReceive(IntPtr connId, byte[] bytes)
        {
            string ip = string.Empty;
            ushort port = 0;
            tcpServer.GetRemoteAddress(connId, ref ip, ref port);
            System.Net.IPEndPoint point = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ip), port);
            if (TCPReceiveData != null) TCPReceiveData(this, new SocketDataEventArgs(point, bytes, "", connId));
            return HandleResult.Ok;
        }

        private HandleResult TcpServer_OnAccept(IntPtr connId, IntPtr pClient)
        {
            string ip = string.Empty;
            ushort port = 0;
            tcpServer.GetRemoteAddress(connId, ref ip, ref port);
            // 设置附加数据
            ClientInfo clientInfo = new ClientInfo();
            clientInfo.ConnId = connId;
            clientInfo.IpAddress = ip;
            clientInfo.Port = port;
            tcpExtra.Set(connId, clientInfo);

            log.Info("新客户端接入，host：" + ip + "(" + port + ")");
            return HandleResult.Ok;
        }

        public bool Send(IntPtr connId, byte[] bytes, int size)
        {
            bool send = false;
            if(tcpExtra.Get(connId) != default(ClientInfo))
            {
                send = tcpServer.Send(connId, bytes, size);
            }
            else if(udpExtra.Get(connId) != default(ClientInfo))
            {
                send = udpServer.Send(connId, bytes, size);
            }
            return send;
        }

        public bool Send(IntPtr connId, byte[] bytes, int offset, int size)
        {
            bool send = false;
            if (tcpExtra.Get(connId) != default(ClientInfo))
            {
                send = tcpServer.Send(connId, bytes, offset, size);
            }
            else if (udpExtra.Get(connId) != default(ClientInfo))
            {
                send = udpServer.Send(connId, bytes, offset, size);
            }
            return send;
        }

        public void Dispose()
        {
            if (tcpServer != null)
            {
                tcpServer.Destroy();
                tcpServer = null;
            }
            if (udpServer != null)
            {
                udpServer.Destroy();
                udpServer = null;
            }
        }
    }



    [StructLayout(LayoutKind.Sequential)]
    public class ClientInfo
    {
        public IntPtr ConnId { get; set; }
        public string IpAddress { get; set; }
        public ushort Port { get; set; }
        public string PhysicalCode { get; set; }
    }
}
