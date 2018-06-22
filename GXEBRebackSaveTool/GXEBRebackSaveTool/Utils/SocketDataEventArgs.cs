using System;
using System.Net;

namespace GXEBRebackSaveTool.Utils
{
    /// <summary>
    /// socket通信使用的事件数据
    /// </summary>
    public class SocketDataEventArgs : EventArgs
    {
        private IntPtr connId;
        /// <summary>
        /// 连接的客户端句柄
        /// </summary>
        public IntPtr ConnId { get { return connId; } }

        private EndPoint endPoint;
        /// <summary>
        /// 远程的Host信息
        /// </summary>
        public EndPoint EndPoint { get { return endPoint; } }

        private byte[] data;
        /// <summary>
        /// 接收到的数据
        /// </summary>
        public byte[] Data { get { return data; } }

        private string message;
        /// <summary>
        /// 事件附带的信息（可为空）
        /// </summary>
        public string Message { get { return message; } }

        /// <summary>
        /// 初始化SocketDataEventArgs的新实例
        /// </summary>
        /// <param name="endPoint">远程的Host信息</param>
        /// <param name="data">接收到的数据</param>
        /// <param name="message">事件附带的信息（可为空）</param>
        public SocketDataEventArgs(EndPoint endPoint, byte[] data, string message, IntPtr connId)
        {
            this.endPoint = endPoint;
            this.data = data;
            this.message = message;
            this.connId = connId;
        }
    }
}
