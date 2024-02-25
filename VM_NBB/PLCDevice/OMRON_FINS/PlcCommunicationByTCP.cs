using PLC;
using PLCInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationTookit
{
  

    public class PlcCommunicationByTCP
    {
        private const int MSG_SIZE = 8192;
        private const int PORT_MIN = 0;
        private const int PORT_MAX = 65535;

        private bool isConnected = false;

        private string targetIP;
        private int targetPort;
        private int readTimeOut;
        private int writeTimeOut;

        public TcpClient client = null;
        private NetworkStream stream = null;
        private readonly byte[] btRcvMsg = new byte[MSG_SIZE];

        public bool IsConnected
        {
            get { return isConnected; }
        }

        #region ctor
        public PlcCommunicationByTCP(int recvTimeOut, int sendTimeOut)
        {
            readTimeOut = recvTimeOut;
            writeTimeOut = sendTimeOut;
        }

        ~PlcCommunicationByTCP()
        {
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
        }

        #endregion

        #region public

        public bool Connect(string ip, int port)
        {
            try
            {
                if (isConnected)
                    return true;
                if (string.IsNullOrWhiteSpace(ip) || port < PORT_MIN || port > PORT_MAX)
                {
                    return false;
                }
                targetIP = ip;
                targetPort = port;
                if (ConnectTcpServer(ip, port))
                {
                    stream = client.GetStream();
                    isConnected = true;
                }
                else
                {
                    isConnected = false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.objLog.Error(ex.Message);
                isConnected = false;
                return false;
            }
            return true;
        }

        public bool Reconnect()
        {
            try
            {
                return Connect(targetIP, targetPort);
            }
            catch (Exception ex)
            {
                LogHelper.objLog.Error(ex.Message);
                isConnected = false;
                return false;
            }
        }

        public bool Disconnect()
        {
            try
            {
                if (isConnected)
                {
                    if (stream != null)
                        stream.Close();
                    if (client != null)
                        client.Close();
                    isConnected = false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.objLog.Error(ex.Message);
                isConnected = false;
                return false;
            }
            return true;
        }

        public bool Receive(out string rcvMsg)
        {
            rcvMsg = string.Empty;
            try
            {
                if (!isConnected)
                {
                    return false;
                }
                if (true /*stream.CanRead && stream.DataAvailable*/ )
                {
                    int len = stream.Read(btRcvMsg, 0, MSG_SIZE);
                    if (0 == len)
                    {
                        return false;
                    }
                    rcvMsg = Encoding.Default.GetString(btRcvMsg, 0, len);
                    //byte[] recvData = new byte[len];
                    //Array.Copy( btRcvMsg, recvData, len );
                }
            }
            catch (Exception ex)
            {
                LogHelper.objLog.Error(ex.Message);
                return false;
            }
            return true;
        }

        public bool Receive(out byte[] rcvMsg)
        {

            rcvMsg = null;
            try
            {
                if (!isConnected)
                {
                    return false;
                }
                if (true /*stream.CanRead && stream.DataAvailable*/ )
                {
                    int len = stream.Read(btRcvMsg, 0, MSG_SIZE);
                    if (0 == len)
                    {
                        return false;
                    }
                    rcvMsg = new byte[len];
                    Array.Copy(btRcvMsg, rcvMsg, len);
                }
            }
            catch (Exception ex)
            {
                LogHelper.objLog.Error(ex.Message);
                return false;
            }
            return true;
        }

        public bool Send(string msg)
        {
            ClearReadCache();
            try
            {
                if (!isConnected)
                {
                    return false;
                }
                Byte[] data = Encoding.Default.GetBytes(msg);
                stream.Write(data, 0, data.Length);
                return true;
            }
            catch (System.IO.IOException ioEx)
            {
                LogHelper.objLog.Error(ioEx.Message);
                isConnected = false;
                Disconnect();
                return false;
            }
            catch (System.ObjectDisposedException disposeEx)
            {
                isConnected = false;
                Disconnect();
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.objLog.Error(ex.Message);
                return false;
            }
        }

        public bool Send(byte[] msg, int len)
        {
            ClearReadCache();
            try
            {
                if (!isConnected)
                {
                    return false;
                }
                stream.Write(msg, 0, len);
                return true;
            }
            catch (System.IO.IOException ioEx)
            {
                LogHelper.objLog.Error(ioEx.Message);
                isConnected = false;
                Disconnect();
                return false;
            }
            catch (System.ObjectDisposedException disposeEx)
            {
                LogHelper.objLog.Error(disposeEx.Message);
                isConnected = false;
                Disconnect();
                return false;
            }
            catch (Exception ex)
            {

                LogHelper.objLog.Error(ex.Message);
                return false;
            }
        }

        #endregion

        #region private

        private bool ConnectTcpServer(string ip, int port)
        {
            IPAddress ipaddress = IPAddress.Parse(ip);
            client = new TcpClient();
            client.SendTimeout = writeTimeOut;
            client.ReceiveTimeout = readTimeOut;
            client.Connect(ipaddress, port);
            return client.Connected;
        }

        #endregion

        public static string GetLocalIP()
        {
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                LogHelper.objLog.Error(ex.Message);
                return ex.Message;
            }
        }

        //清空读缓存
        public void ClearReadCache()
        {
            try
            {
                do
                {
                    if (isConnected && stream.CanRead && stream.DataAvailable)
                    {
                        byte[] rcvMsg = new byte[MSG_SIZE];
                        stream.Read(rcvMsg, 0, MSG_SIZE);
                    }
                    else
                    {
                        break;
                    }
                }
                while (true);

            }
            catch (Exception ex)
            {
                LogHelper.objLog.Error(ex.Message);
                return;
            }
            return;
        }
    }
}
