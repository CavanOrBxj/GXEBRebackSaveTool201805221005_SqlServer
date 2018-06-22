using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace GXEBRebackSaveTool
{
    /// <summary>
    /// UDP操作类。
    /// </summary>
    public class UdpSender
    {
        /// <summary>
        /// UDP消息接收者。
        /// </summary>
        protected UdpClient udpReceiver = null;

        /// <summary>
        /// 初始化Udp类的新实例。
        /// </summary>
        public UdpSender()
        {
            udpReceiver = new UdpClient();
        }

        /// <summary>
        /// 初始化Udp类的新实例，并监听UDP:Port端口。
        /// </summary>
        /// <param name="port">指定本地端口。</param>
        public UdpSender(int port)
        {
            udpReceiver = new UdpClient(port);
        }

        /// <summary>
        /// 向指定目标地址发送数据，并返回应答数据。
        /// </summary>
        /// <param name="ip">目标IP地址。</param>
        /// <param name="port">目标端口。</param>
        /// <param name="senddata">待发送数据。</param>
        /// <returns>应答数据。</returns>
        public byte[] SendData(string ip, int port, byte[] datas)
        {
            return SendData(ip, port, datas, -1);
        }

        /// <summary>
        /// 向指定目标地址发送数据，并返回应答数据。
        /// </summary>
        /// <param name="ip">目标IP地址。</param>
        /// <param name="port">目标端口。</param>
        /// <param name="senddata">待发送数据。</param>
        /// <param name="nTimeOut">超时时间(单位：豪秒)，-1指不超时。</param>
        /// <returns>应答数据。</returns>
        public byte[] SendData(string ip, int port, byte[] datas, int nTimeOut)
        {
            IPEndPoint observer = new IPEndPoint(System.Net.IPAddress.Parse(ip), port);

            udpReceiver.Send(datas, datas.Length, observer);

            IPEndPoint _remodeIpEndPoint = null;
            byte[] receiveBytes = null;      
            do
            {
                if (udpReceiver.Available > 0) break;     
                System.Threading.Thread.Sleep(10);
                nTimeOut -= 10;
            }while (nTimeOut > 0);
            if ( nTimeOut <= 0 ) return null;
            receiveBytes = udpReceiver.Receive(ref _remodeIpEndPoint);    
            return receiveBytes;
        }

        /// <summary>
        /// 向指定目标地址发送数据，并返回应答数据。
        /// </summary>
        /// <param name="ip">目标IP地址。</param>
        /// <param name="port">目标端口。</param>
        /// <param name="senddata">待发送数据。</param>
        /// <param name="remoteEP">应答数据来源地址。</param>
        /// <returns>应答数据</returns>
        public byte[] SendData(string ip, int port, byte[] datas, ref IPEndPoint remoteEP)
        {
            return SendData(ip, port, datas, ref remoteEP, -1);
        }

        /// <summary>
        /// 向指定目标地址发送数据，并返回应答数据。
        /// </summary>
        /// <param name="ip">目标IP地址。</param>
        /// <param name="port">目标端口。</param>
        /// <param name="senddata">待发送数据。</param>
        /// <param name="remoteEP">应答数据来源地址。</param>
        /// <param name="nTimeOut">超时时间(单位：豪秒)，-1指不超时。</param>
        /// <returns>应答数据</returns>
        public byte[] SendData(string ip, int port, byte[] datas, ref IPEndPoint remoteEP, int nTimeOut)
        {    
            IPEndPoint observer = new IPEndPoint(System.Net.IPAddress.Parse(ip), port);

            udpReceiver.Send(datas, datas.Length, observer);

            IPEndPoint _remodeIpEndPoint = null;
            byte[] receiveBytes = null;

            do
            {
                if (udpReceiver.Available > 0) break;

                System.Threading.Thread.Sleep(10);
                nTimeOut -= 10;
            } while (nTimeOut > 0);
            if ( nTimeOut <= 0 ) return null;
            receiveBytes = udpReceiver.Receive(ref _remodeIpEndPoint);
            remoteEP = _remodeIpEndPoint;

            return receiveBytes;
        }

        public int SendDatawithoutreturn(string ip, int port, byte[] datas)
        {
            IPEndPoint observer = new IPEndPoint(System.Net.IPAddress.Parse(ip), port);
          return   udpReceiver.Send(datas, datas.Length, observer);
        }

        public void CloseConnection()
        {
            udpReceiver.Close(); 
        }
    }
}
