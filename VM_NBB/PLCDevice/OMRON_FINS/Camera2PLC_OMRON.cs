using PLC;
using PLCInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationTookit

//namespace WindowsFormsApp22
{

    public class Camera2PLC_OMRON
    {
        #region field
        private const int TIME_COUNT = 2;
        private object objLock = new object();
        private object objLock_base = new object();

        public List<PLCAddress> adrsRead = new List<PLCAddress>();
        public List<PLCAddress> adrsWrite = new List<PLCAddress>();
        public List<PLCAddress> adrsAll = new List<PLCAddress>();

        private PLCAddress adrsHeart = null;
        private bool isConnected = false;
        private bool isHeartBeatStarted = false;
        private bool isDataMontiorStarted = false;
        private bool flgHeartBeat;
        private bool flgHeartBeatResult;
        private bool flgReadResult;
        private bool flgReady = false;
        private int flgReset = 0;
        private int flgFirst = 0;

        private string localIP;
        private string targetIP;
        private int localPort = 8000;
        private int targetPort;
        private int readTimeOut = 15;
        private int writeTimeOut = 5;

      //  private PLCTCPClient clientHeartBeat = null;
        private PlcCommunicationByTCP clientDataMontior = null;
        public PlcCommunicationByTCP clientCommon = null;
     //   private PLCUDP udpHeartBeat = null;
        private PlcCommunicationByTCP udpDataMontior = null;
       private PlcCommunicationByTCP udpCommon = null;

      //  private Task tskHeartBeat;
        private Task tskDataMontior;
        private Task tskDataMontior_Camera1;
        //private Task tskDataMontior_Camera2; 

       // private CancellationTokenSource ctsHeartBeat;
        private CancellationTokenSource ctsDataMontior;
        private CancellationTokenSource ctsDataMontior_Camera1;
        //private CancellationTokenSource ctsDataMontior_Camera2;

        private byte[] btHandShake;
        private byte[] btHandShakeRep;
        private byte[] btNodeHeart;
        private byte[] btNodeData;
        private byte[] btNodeCommon;

        private DateTime dtHeartStart;
        private DateTime dtHeartEnd;
        //private bool isHeartSend = false;


        #endregion

        #region ctor
        public Camera2PLC_OMRON()
        {
            //clientHeartBeat = new PLCTCPClient(readTimeOut, writeTimeOut);
            //clientDataMontior = new PLCTCPClient(readTimeOut, writeTimeOut);
            //clientCommon = new PLCTCPClient(readTimeOut, writeTimeOut);
            //udpHeartBeat = new PLCUDP(readTimeOut, writeTimeOut);
            //udpDataMontior = new PLCUDP(readTimeOut, writeTimeOut);
            //udpCommon = new PLCUDP(readTimeOut, writeTimeOut);

            clientCommon=  new PlcCommunicationByTCP(readTimeOut, writeTimeOut);

            dtHeartStart = DateTime.Now;
            dtHeartEnd = DateTime.Now;
            GetHandShakeCmd();
        }


        public Camera2PLC_OMRON(PlcCommunicationByTCP Client)
        {
            clientCommon = Client;
            dtHeartStart = DateTime.Now;
            dtHeartEnd = DateTime.Now;
            GetHandShakeCmd();
        }

        ~Camera2PLC_OMRON()
        {
            Dispose();
        }

        #endregion

        #region property
        public bool IsUdp { get; set; } = false;
        public bool IsNeedHandShake { get; set; }

        public string LocalIP
        {
            get
            {
                if (string.IsNullOrWhiteSpace(localIP))
                {
                   // localIP = PLCTCPClient.GetLocalIP();

                    localIP= PlcCommunicationByTCP.GetLocalIP();
                }
                return localIP;
            }
            set { targetIP = value; }
        }
        public int LocalPort
        {
            get { return localPort; }
            set { localPort = value; }
        }
        public string TargetIP
        {
            get { return targetIP; }
            set { targetIP = value; }
        }
        public int TargetPort
        {
            get { return targetPort; }
            set { targetPort = value; }
        }
        public bool IsConnected
        {
            get { return isConnected; }
        }
        public List<PLCAddress> Addresses
        {
            get
            {
                if (adrsAll.Count == 0)
                {
                    if (null != adrsHeart)
                        adrsAll.Add(adrsHeart);
                    adrsAll.AddRange(adrsRead);
                    adrsAll.AddRange(adrsWrite);
                }
                return adrsAll;
            }
        }
        #endregion

     
        public event Action<object, CommunicationMessage> OnReceivedMessage;
      

        #region private method
        private void HeartBeatStart()
        {
            //if (!isHeartBeatStarted)
            //{   
            //    ctsHeartBeat = new CancellationTokenSource();
            //    tskHeartBeat = new Task(() =>
            //    {
            //        while (!ctsHeartBeat.Token.IsCancellationRequested)
            //        {
            //            WriteHeartBeat();
            //            Thread.Sleep(PLCDefine.HEART_BEAT_INTERVAL);
            //        }
            //        System.Diagnostics.Debugger.Log(0, null, "plc::tskHeartBeat is ending");
            //    }, ctsHeartBeat.Token);
            //    isHeartBeatStarted = true;
            //    tskHeartBeat.Start();
            //}
        }

        private void HeartBeatStop()
        {
            //if (isHeartBeatStarted)
            //{
            //    if (ctsHeartBeat != null)
            //    {
            //        ctsHeartBeat.Cancel();
            //    }
            //    isHeartBeatStarted = false;
            //}
        }


        private void DataMontiorStart()
        {
            if (!isDataMontiorStarted)
            {
                ctsDataMontior = new CancellationTokenSource();

                tskDataMontior_Camera1 = new Task(() =>
                {                 
                    while (!ctsDataMontior.Token.IsCancellationRequested)
                    {                     
                        MoniterPlcAddress_Camera1();
                        Thread.Sleep(1);
                    }

                }, ctsDataMontior.Token);
      
                isDataMontiorStarted = true;
                tskDataMontior_Camera1.Start();
              
            }
        }

        private void DataMontiorStop()
        {
            if (isDataMontiorStarted)
            {
                if (ctsDataMontior != null)
                {
                    ctsDataMontior.Cancel();
                }
                isDataMontiorStarted = false;
            }
        }

        public void CreateAddresses()
        {
            //adrsRead.Add(new PLCAddress(PLCDefine.R_3000, PLCDefine.R_3000, 1, ValueType.UINT16, CommandType.Read, true));
            //adrsRead[0].Description = "触发信号camea1";

            //adrsWrite.Add(new PLCAddress(PLCDefine.W_3001, PLCDefine.W_3001, 1, ValueType.UINT16, CommandType.Write));
            //adrsWrite.Add(new PLCAddress(PLCDefine.W_3002, PLCDefine.W_3002, 2, ValueType.INT32, CommandType.Write));
            //adrsWrite.Add(new PLCAddress(PLCDefine.W_3004, PLCDefine.W_3004, 34, ValueType.INT32, CommandType.Write));

            //adrsWrite.Add(new PLCAddress(PLCDefine.W_3036, PLCDefine.W_3036, 34, ValueType.INT32, CommandType.Write));

            //adrsWrite[0].Description = "相机写入相机状态地址,如Ready信号";
            //adrsWrite[1].Description = "相机发送算法处理结果地址：NG/OK状态";
            //adrsWrite[2].Description = "数据";

        }

        public void CreateAddresses(List<PLCAddress> AdrsRead, List<PLCAddress> AdrsWrite)
        {
            this.adrsRead = AdrsRead;
            this.adrsWrite = AdrsWrite;
        }

            private void WriteHeartBeat()
        {
            if (flgHeartBeat)
            {
                flgHeartBeatResult = Write(adrsHeart, "1", TCPType.Common);
                flgHeartBeat = false;
            }
            else
            {
                flgHeartBeatResult = Write(adrsHeart, "0", TCPType.Common);
                flgHeartBeat = true;
            }
            if (!flgHeartBeatResult)
            {
                isConnected = false;
                flgReady = false;
            }

            if (flgReady && isConnected)
            {
                //如果当前处于连接状态，且没有与PLC其他交互，可以告知PLC当前的VM是准备好的
                Write(PLCDefine.W_3001, PLCDefine.W_Ready, TCPType.Common);
                // LogHelper.objLog.Debug(string.Format("idle  send ready to plc"));
            }
        }

        public bool Load(List<PLCAddress> adrs)
        {
            if (null == adrs || adrs.Count == 0)
            {
                return false;
            }
            foreach (var adr in adrs)
            {
                if (adr.IsHeart)
                {
                    if (null == adrsHeart)
                        adrsHeart = adr;
                }
                else if (adr.CommandType == CommandType.Read)
                {
                    adrsRead.Add(adr);
                }
                else if (adr.CommandType == CommandType.Write)
                {
                    adrsWrite.Add(adr);
                }
            }
            return true;
        }

        /// <summary>
        /// 与PLC或VM断线时置0,在自动连接时发送Ready信号
        /// </summary>
        public void ResetFlagReady()
        {
            //
            if (flgReady)
            {
                //在VM掉线的时候要把ready信号清空
                ResetReadySignal();
            }
            flgReady = false;
        }

        /// <summary>
        /// 初始化PLC的信号
        /// </summary>
        public void InitSignal()
        {
            if (!flgReady)
            {
                Thread.Sleep(1000);
                if (0 == flgFirst)
                {
                    Thread.Sleep(1000);
                }
                flgFirst = 1;
              //  flgReady = Write(PLCDefine.W_3001, PLCDefine.W_Ready, TCPType.Common);
                flgReady = Write(adrsWrite[0], PLCDefine.W_Ready, TCPType.Common);

            }
        }

        //VM掉线后要把ready信号置空
        public void ResetReadySignal()
        {
            if (IsConnected)
            {
               // Write(PLCDefine.W_3001, "0", TCPType.Common);
                Write(adrsWrite[0], "0", TCPType.Common);
              
            }
        }

        private void MoniterPlcAddress_Camera1()
        {
            if (!isDataMontiorStarted)
                return;
            try
            {
                string value;
                if (Read(adrsRead[0].Name, out value))
                {
                    OnReceivedMessage?.Invoke(this, new CommunicationMessage(value, adrsRead[0].Name));
                }

            }
           
            catch (System.IO.IOException ioEx)
            {
                isConnected = false;
               
            }
            catch (System.ObjectDisposedException disposeEx)
            {
                isConnected = false;

               
            }
            catch (Exception e)
            {
            }
            finally
            {
            }

        }

 
        private void MoniterPlcAddress()
        {
            {
                if (!isDataMontiorStarted)
                    return;
                try
                {
                    string value;
                    foreach (var item in adrsRead)
                    {
                        if (!item.IsPolled)
                        {
                            continue;
                        }
                        flgReadResult = Read(item.Name, out value/*, TCPType.DataMontior*/);
                        if (flgReadResult)
                        {
                            //invoke the event
                            if (OnReceivedMessage != null)
                            {
                                //if ( "1" == value )
                                //{
                                //    UILogManager.Instance.AddLog(UILogLevel.Infor, string.Format("invoke, {0}, {1}", item.Name, value));
                                //}
                                OnReceivedMessage(this, new CommunicationMessage(value, item.Name));
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                }
                finally
                {
                }
            }
        }

        private bool Receive(out string rcvMsg, TCPType clientType)
        {
            switch (clientType)
            {
                //case TCPType.HeartBeat:
                //    if (IsUdp)
                //        return udpHeartBeat.Receive(out rcvMsg);
                //    else
                //        return clientHeartBeat.Receive(out rcvMsg);
                case TCPType.DataMontior:
                    if (IsUdp)
                        return udpDataMontior.Receive(out rcvMsg);
                    else
                        return clientDataMontior.Receive(out rcvMsg);
                case TCPType.Common:
                    if (IsUdp)
                        return udpCommon.Receive(out rcvMsg);
                    else
                        return clientCommon.Receive(out rcvMsg);
                default:
                    if (IsUdp)
                        return udpCommon.Receive(out rcvMsg);
                    else
                        return clientCommon.Receive(out rcvMsg);
            }
        }

        private bool Receive(out byte[] rcvMsg, TCPType clientType)
        {
            if (IsUdp)
                return udpCommon.Receive(out rcvMsg);
            else
                return clientCommon.Receive(out rcvMsg);

        }

        private bool Send(string cmdMsg, TCPType clientType)
        {

            if (IsUdp)
                return udpCommon.Send(cmdMsg);
            else
                return clientCommon.Send(cmdMsg);
          
        }

        private bool Send(byte[] cmdMsg, TCPType clientType)
        {
            if (IsUdp)
                return udpCommon.Send(cmdMsg, cmdMsg.Length);
            else
                return clientCommon.Send(cmdMsg, cmdMsg.Length);                     
        }

        private void GetHandShakeCmd()
        {
            //handshake
            btHandShake = new byte[20];
            //header
            btHandShake[0] = PLCDefine.BT_F;
            btHandShake[1] = PLCDefine.BT_I;
            btHandShake[2] = PLCDefine.BT_N;
            btHandShake[3] = PLCDefine.BT_S;
            //len
            btHandShake[4] = 0;
            btHandShake[5] = 0;
            btHandShake[6] = 0;
            btHandShake[7] = 0x0c;
            //cmd
            btHandShake[8] = 0;
            btHandShake[9] = 0;
            btHandShake[10] = 0;
            btHandShake[11] = 0/*0*/;
            //err 
            btHandShake[12] = 0;
            btHandShake[13] = 0;
            btHandShake[14] = 0;
            btHandShake[15] = 0;
            //client node
            btHandShake[16] = 0;
            btHandShake[17] = 0;
            btHandShake[18] = 0;
            btHandShake[19] = 0; //0;//ask for client and server node number, the client node will allocated automatically

            return;
        }

        private byte[] GetReadCommandUdp(PLCAddress adrs, TCPType clientType)
        {
            byte[] btCmd = new byte[18];
            //command frame header
            btCmd[0] = PLCDefine.BT_ICF_C; //ICF
            btCmd[1] = 0x00;//RSV
            btCmd[2] = 0x02;//GCT, less than 8 network layers
            btCmd[3] = 0x00;//DNA, local network
            btCmd[4] = Convert.ToByte(targetIP.Substring(targetIP.LastIndexOf('.') + 1)); //Server[3];//DA1 PLC IP 地址最后一位
            btCmd[5] = 0x00;//DA2, CPU unit
            btCmd[6] = 0x00;//SNA, local network
            btCmd[7] = Convert.ToByte(LocalIP.Substring(LocalIP.LastIndexOf('.') + 1)); //Client[3];//SA1  PC 的 IP 地址最后一位
            btCmd[8] = 0x00;//SA2, CPU unit
            btCmd[9] = 0x00;//Convert.ToByte( 21 );//SID   

            btCmd[10] = 0x01;//read cmd
            btCmd[11] = 0x01;
            btCmd[12] = 0x82; //DM
            //地址
            byte[] btAdrs = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt16(adrs.CellAddress)));
            btCmd[13] = btAdrs[0];
            btCmd[14] = btAdrs[1];
            btCmd[15] = 0; //要读的位
            //寄存器个数
            byte[] btCellCnt = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt16(adrs.CellCount)));
            btCmd[16] = btCellCnt[0];
            btCmd[17] = btCellCnt[1];

            return btCmd;
        }

        private byte[] GetReadCommandTcp(PLCAddress adrs, TCPType clientType)
        {
            byte[] btCmd = new byte[34];
            //TCP FINS header
            btCmd[0] = 0x46;//F
            btCmd[1] = 0x49;//I
            btCmd[2] = 0x4e;//N
            btCmd[3] = 0x53;//S
            btCmd[4] = 0;//cmd length
            btCmd[5] = 0;
            btCmd[6] = 0;
            btCmd[7] = 0x1A;
            btCmd[8] = 0; //frame command
            btCmd[9] = 0;
            btCmd[10] = 0;
            btCmd[11] = 0x02;
            btCmd[12] = 0; //err
            btCmd[13] = 0;
            btCmd[14] = 0;
            btCmd[15] = 0;
            //command frame header
            btCmd[16] = PLCDefine.BT_ICF_C; //ICF
            btCmd[17] = 0x00;//RSV
            btCmd[18] = 0x02;//GCT, less than 8 network layers
            btCmd[19] = 0x00;//DNA, local network
            btCmd[20] = Convert.ToByte(targetIP.Substring(targetIP.LastIndexOf('.') + 1)); //Server[3];//DA1 PLC IP 地址最后一位
            btCmd[21] = 0x00;//DA2, CPU unit
            btCmd[22] = 0x00;//SNA, local network
            if (clientType == TCPType.HeartBeat)
            {
                btCmd[23] = btNodeHeart[0]; //Client[3];//SA1  PC 的 IP 地址最后一位
            }
            else if (clientType == TCPType.DataMontior)
            {
                btCmd[23] = btNodeData[0]; //Client[3];//SA1  PC 的 IP 地址最后一位
            }
            else
            {
                btCmd[23] = btNodeCommon[0]; //Client[3];//SA1  PC 的 IP 地址最后一位
            }
            btCmd[24] = 0x00;//SA2, CPU unit
            btCmd[25] = 0x00;//Convert.ToByte( 21 );//SID   

            btCmd[26] = 0x01;//read cmd
            btCmd[27] = 0x01;

            btCmd[28] = 0x82; //DM
            //地址
            byte[] btAdrs = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt16(adrs.CellAddress)));
            btCmd[29] = btAdrs[0];
            btCmd[30] = btAdrs[1];
            btCmd[31] = 0; //要读的位
            //寄存器个数
            byte[] btCellCnt = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt16(adrs.CellCount)));
            btCmd[32] = btCellCnt[0];
            btCmd[33] = btCellCnt[1];

            return btCmd;
        }

        private byte[] GetReadCommand(PLCAddress adrs, TCPType clientType)
        {
            if (IsUdp)
            {
                return GetReadCommandUdp(adrs, clientType);
            }
            else
            {
                return GetReadCommandTcp(adrs, clientType);
            }
        }

        private byte[] GetWriteCommandUdp(PLCAddress adrs, string[] contents, TCPType clientType)
        {
            int lenContent = 0;
            if (adrs.ValueType == ValueType.INT16 || adrs.ValueType == ValueType.UINT16)
            {
                if (contents.Length > Convert.ToInt32(adrs.CellCount))
                {
                    return null;
                }
                lenContent = 2 * Convert.ToInt32(adrs.CellCount);
            }
            else if (adrs.ValueType == ValueType.INT32 || adrs.ValueType == ValueType.UINT32)
            {
                if (contents.Length * 2 > Convert.ToInt32(adrs.CellCount))
                {
                    return null;
                }
                lenContent = 2 * Convert.ToInt32(adrs.CellCount);
            }

            byte[] btCmd = new byte[18 + lenContent];
            //command frame header
            btCmd[0] = PLCDefine.BT_ICF_C; //ICF
            btCmd[1] = 0x00;//RSV
            btCmd[2] = 0x02;//GCT, less than 8 network layers
            btCmd[3] = 0x00;//DNA, local network
            btCmd[4] = Convert.ToByte(targetIP.Substring(targetIP.LastIndexOf('.') + 1)); //Server[3];//DA1 PLC IP 地址最后一位
            btCmd[5] = 0x00;//DA2, CPU unit
            btCmd[6] = 0x00;//SNA, local network
            btCmd[7] = Convert.ToByte(LocalIP.Substring(LocalIP.LastIndexOf('.') + 1)); //Client[3];//SA1  PC 的 IP 地址最后一位
            btCmd[8] = 0x00;//SA2, CPU unit
            btCmd[9] = 0x00;//Convert.ToByte( 21 );//SID   

            btCmd[10] = 0x01;//write cmd
            btCmd[11] = 0x02;

            btCmd[12] = 0x82; //DM
            //地址
            byte[] btAdrs = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt16(adrs.CellAddress)));
            btCmd[13] = btAdrs[0];
            btCmd[14] = btAdrs[1];
            btCmd[15] = 0; //要读的位
            //寄存器个数
            byte[] btCellCnt = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt16(adrs.CellCount)));
            btCmd[16] = btCellCnt[0];
            btCmd[17] = btCellCnt[1];

            int offset = 18;
            if (adrs.ValueType == ValueType.INT16 || adrs.ValueType == ValueType.UINT16)
            {
                for (int i = 0; i < contents.Length; i++)
                {
                    byte[] btVal = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt16(contents[i])));
                    btCmd[offset++] = btVal[0];
                    btCmd[offset++] = btVal[1];
                }
                while (offset < 18 + lenContent)
                {
                    btCmd[offset++] = 0;
                }
            }
            else if (adrs.ValueType == ValueType.INT32 || adrs.ValueType == ValueType.UINT32)
            {
                for (int i = 0; i < contents.Length; i++)
                {
                    byte[] btVal = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt32(contents[i])));
                    btCmd[offset++] = btVal[2];
                    btCmd[offset++] = btVal[3];
                    btCmd[offset++] = btVal[0];
                    btCmd[offset++] = btVal[1];
                }
                while (offset < 18 + lenContent)
                {
                    btCmd[offset++] = 0;
                }
            }
            return btCmd;
        }

        private byte[] GetWriteCommandTcp(PLCAddress adrs, string[] contents, TCPType clientType)
        {
            int lenContent = 0;
            if (adrs.ValueType == ValueType.INT16 || adrs.ValueType == ValueType.UINT16)
            {
                if (contents.Length > Convert.ToInt32(adrs.CellCount))
                {
                    return null;
                }
                lenContent = 2 * Convert.ToInt32(adrs.CellCount);
            }
            else if (adrs.ValueType == ValueType.INT32 || adrs.ValueType == ValueType.UINT32)
            {
                if (contents.Length * 2 > Convert.ToInt32(adrs.CellCount))
                {
                    return null;
                }
                lenContent = 2 * Convert.ToInt32(adrs.CellCount);
            }

            byte[] btCmd = new byte[34 + lenContent];
            //TCP FINS header
            btCmd[0] = 0x46;//F
            btCmd[1] = 0x49;//I
            btCmd[2] = 0x4e;//N
            btCmd[3] = 0x53;//S
            //cmd length
            byte[] btLen = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt32(26 + lenContent)));
            btCmd[4] = btLen[0];
            btCmd[5] = btLen[1];
            btCmd[6] = btLen[2];
            btCmd[7] = btLen[3];
            //frame command
            btCmd[8] = 0;
            btCmd[9] = 0;
            btCmd[10] = 0;
            btCmd[11] = 0x02;
            btCmd[12] = 0; //err
            btCmd[13] = 0;
            btCmd[14] = 0;
            btCmd[15] = 0;
            //command frame header
            btCmd[16] = PLCDefine.BT_ICF_C; //ICF
            btCmd[17] = 0x00;//RSV
            btCmd[18] = 0x02;//GCT, less than 8 network layers
            btCmd[19] = 0x00;//DNA, local network
            btCmd[20] = Convert.ToByte(targetIP.Substring(targetIP.LastIndexOf('.') + 1)); //Server[3];//DA1 PLC IP 地址最后一位
            btCmd[21] = 0x00;//DA2, CPU unit
            btCmd[22] = 0x00;//SNA, local network
            if (clientType == TCPType.HeartBeat)
            {
                btCmd[23] = btNodeHeart[0]; //Client[3];//SA1  PC 的 IP 地址最后一位
            }
            else if (clientType == TCPType.DataMontior)
            {
                btCmd[23] = btNodeData[0]; //Client[3];//SA1  PC 的 IP 地址最后一位
            }
            else
            {
                btCmd[23] = btNodeCommon[0]; //Client[3];//SA1  PC 的 IP 地址最后一位
            }
            btCmd[24] = 0x00;//SA2, CPU unit
            btCmd[25] = 0x00;//Convert.ToByte( 21 );//SID   

            btCmd[26] = 0x01;//write cmd
            btCmd[27] = 0x02;

            btCmd[28] = 0x82; //DM
            //地址
            byte[] btAdrs = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt16(adrs.CellAddress)));
            btCmd[29] = btAdrs[0];
            btCmd[30] = btAdrs[1];
            btCmd[31] = 0; //要读的位
            //寄存器个数
            byte[] btCellCnt = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt16(adrs.CellCount)));
            btCmd[32] = btCellCnt[0];
            btCmd[33] = btCellCnt[1];

            int offset = 34;
            if (adrs.ValueType == ValueType.INT16 || adrs.ValueType == ValueType.UINT16)
            {
                for (int i = 0; i < contents.Length; i++)
                {
                    byte[] btVal = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt16(contents[i])));
                    btCmd[offset++] = btVal[0];
                    btCmd[offset++] = btVal[1];
                }
                while (offset < 34 + lenContent)
                {
                    btCmd[offset++] = 0;
                }
            }
            else if (adrs.ValueType == ValueType.INT32 || adrs.ValueType == ValueType.UINT32)
            {
                for (int i = 0; i < contents.Length; i++)
                {
                    byte[] btVal = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt32(contents[i])));
                    btCmd[offset++] = btVal[2];
                    btCmd[offset++] = btVal[3];
                    btCmd[offset++] = btVal[0];
                    btCmd[offset++] = btVal[1];
                }
                while (offset < 34 + lenContent)
                {
                    btCmd[offset++] = 0;
                }
            }
            return btCmd;
        }

        private byte[] GetWriteCommand(PLCAddress adrs, string[] contents, TCPType clientType)
        {
            if (IsUdp)
            {
                return GetWriteCommandUdp(adrs, contents, clientType);
            }
            else
            {
                return GetWriteCommandTcp(adrs, contents, clientType);
            }
        }
        private bool ParseWriteRep(byte[] rep)
        {
            if (IsUdp)
            {
                return ParseWriteRepUdp(rep);
            }
            else
            {
                return ParseWriteRepTcp(rep);
            }
        }
        private bool ParseWriteRepUdp(byte[] rep)
        {
            if (null == rep)
            {
                return false;
            }
            if (rep.Length < 14)
            {
                return false;
            }
            if (rep[12] == 0 /*&& rep[13] == 0*/ ) //40
            {
                return true;
            }
            return false;
        }
        private bool ParseWriteRepTcp(byte[] rep)
        {
            if (null == rep)
            {
                return false;
            }
            if (rep.Length < 30)
            {
                return false;
            }
            if (rep[11] == 3 && rep[15] != 0)
            {
                return false;
            }
            if (rep[28] == 0 /*&& rep[29] == 0*/ )
            {
                return true;
            }
            return false;
        }

        private bool HandShake(byte[] cmd, out byte[] nodes, TCPType clientType)
        {
            nodes = null;
            if (Send(cmd, clientType))
            {
                byte[] rep = null;
                //  System.Diagnostics.Debugger.Log(0, null, string.Format("plc::握手请求发送成功,[{0}]\r\n", BitConverter.ToString(cmd, 0)));
                for (int i = 0; i < 50; i++)
                {
                    if (!Receive(out rep, clientType))
                    {
                        //Thread.Sleep(1);
                        continue;
                    }
                    else
                    {
                        if (null == rep)
                        {
                            //Thread.Sleep(1);
                            continue;
                        }
                        if (rep.Length == 24 && rep[12] == 0 && rep[13] == 0 && rep[14] == 0 && rep[15] == 0)
                        {
                            nodes = new byte[2];
                            nodes[0] = rep[19];
                            nodes[1] = rep[23];
                            //  System.Diagnostics.Debugger.Log(0, null, string.Format("plc::握手应答成功,[{0}]\r\n", BitConverter.ToString(rep, 0)));
                            return true;
                        }
                        else
                        {
                            LogHelper.objLog.Info(string.Format("plc::Failed to answer the handshake"));
                         //   System.Diagnostics.Debugger.Log(0, null, string.Format("plc::握手应答失败,[{0}]\r\n", BitConverter.ToString(rep, 0)));
                            return false;
                        }
                    }
                }
                if (null == rep)
                {
                    LogHelper.objLog.Info(string.Format("plc::No handshake response received"));
                   // System.Diagnostics.Debugger.Log(0, null, string.Format("plc::未收到握手应答\r\n"));
                    return false;
                }
            }
            else
            {
                LogHelper.objLog.Info(string.Format("plc::Failed to send handshake request"));
              //  System.Diagnostics.Debugger.Log(0, null, string.Format("plc::握手请求发送失败,[{0}]\r\n", BitConverter.ToString(cmd, 0)));
                return false;
            }
            return false;
        }

        private bool ConnectByTcp()
        {
            try
            {
                if (isConnected)
                    return true;

                clientCommon.Connect(targetIP, targetPort);
                if (!clientCommon.IsConnected)
                {
                    isConnected = false;
                    return false;
                }
               // System.Diagnostics.Debugger.Log(0, null, string.Format("plc::clientCommon创建成功\r\n"));
                if (!HandShake(btHandShake, out btNodeCommon, TCPType.Common))
                {
                    isConnected = false;
                  //  clientHeartBeat.Disconnect();
                    clientDataMontior.Disconnect();
                    clientCommon.Disconnect();
                    return false;
                }

                isConnected = true;
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.objLog.Error(ex.Message);
                isConnected = false;
                return false;
            }
        }

        private bool ConnectByUdp()
        {
            try
            {
                if (isConnected)
                    return true;

                //udpHeartBeat.Connect(targetIP, targetPort);
                //if (!udpHeartBeat.IsConnected)
                //{
                //    isConnected = false;
                //    return false;
                //}
                //System.Diagnostics.Debugger.Log(0, null, string.Format("plc::udpHeartBeat创建成功\r\n"));
                //if (IsNeedHandShake)
                //{
                //    if (!HandShake(btHandShake, out btNodeHeart, TCPType.HeartBeat))
                //    {
                //        isConnected = false;
                //        udpHeartBeat.Disconnect();
                //        return false;
                //    }
                //}

                //udpDataMontior.LocalPort = localPort + 1; ;
                //udpDataMontior.Connect(targetIP, targetPort);
                //if (!udpDataMontior.IsConnected)
                //{
                //    isConnected = false;
                //    return false;
                //}
                //System.Diagnostics.Debugger.Log(0, null, string.Format("plc::udpDataMontior创建成功\r\n"));
                //if (IsNeedHandShake)
                //{
                //    if (!HandShake(btHandShake, out btNodeData, TCPType.DataMontior))
                //    {
                //        isConnected = false;
                //        udpHeartBeat.Disconnect();
                //        udpDataMontior.Disconnect();
                //        return false;
                //    }
                //}

               // udpCommon.LocalPort = localPort;// +2; ;
                udpCommon.Connect(targetIP, targetPort);
                if (!udpCommon.IsConnected)
                {
                    isConnected = false;
                    return false;
                }
          //      System.Diagnostics.Debugger.Log(0, null, string.Format("plc::udpCommon创建成功\r\n"));
                //if (IsNeedHandShake)
                //{
                //    if (!HandShake(btHandShake, out btNodeCommon, TCPType.Common))
                //    {
                //        isConnected = false;
                //        udpHeartBeat.Disconnect();
                //        udpDataMontior.Disconnect();
                //        udpCommon.Disconnect();
                //        return false;
                //    }
                //}

                isConnected = true;
                return true;
            }
            catch (Exception ex)
            {
                isConnected = false;
                return false;
            }
        }
        #endregion

        public override string ToString()
        {
            return base.ToString();
        }

        public bool Read(PLCAddress address, out string value, TCPType clientType)
        {
            lock (objLock)
            {
                value = string.Empty;
                try
                {
                   
                    if (!isConnected)
                    {
                        //isConnected =clientCommon.Connect(targetIP, targetPort);
                        return false;
                    }
                    if (address == null)
                    {
                        return false;
                    }
                    if (address.CellCount < 1)
                    {
                        return false;
                    }
                    byte[] btCmd = GetReadCommand(address, clientType);
                    if (null == btCmd)
                        return false;
                    //   System.Diagnostics.Debugger.Log(0, null, string.Format("plc::ReadCommand,[{0}]\r\n", BitConverter.ToString(btCmd, 0)));

                    if (!Send(btCmd, clientType))
                    {

                        isConnected = clientCommon.Connect(targetIP, targetPort);

                        return false;
                    }
                    int count = TIME_COUNT;
                    byte[] rcvMsg = null; ;
                    for (int i = 0; i < count; i++)
                    {
                        if (!Receive(out rcvMsg, clientType))
                        {

                            continue;
                        }
                        if (null != rcvMsg)
                        {
                            break;
                        }

                    }
                    if (null == rcvMsg)
                    {


                        return false;
                    }
                    else
                    {

                        string[] vals;
                        if (address.ParseReadRep(rcvMsg, out vals, IsUdp)/*address.ParseMsgRead(rcvMsg, out value)*/)
                        {
                            value = vals[0];
                            return true;
                        }
                        else
                        {

                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.objLog.Error(ex.Message);
                    return false;
                }
            }
        }

        public bool Read(string addressName, out string value, TCPType clientType = TCPType.Common)
        {
            
                value = string.Empty;
            try
            {
                PLCAddress address = this.adrsRead.Where(a => a.Name == addressName).FirstOrDefault();
                if (address == null)
                {
                    return false;
                }
                return Read(address, out value, clientType);
            }
            catch (Exception ex)
            {
                LogHelper.objLog.Error(ex.Message);
                return false;
            }
        }

        public bool Write(string addressName, string content, TCPType clientType = TCPType.Common)
        {


            try
            {
                PLCAddress address = this.adrsWrite.Where(a => a.Name == addressName).FirstOrDefault();
                if (address == null)
                {
                    return false;
                }
                return Write(address, content, clientType);
            }

            catch (System.IO.IOException ioEx)
            {
                LogHelper.objLog.Error(ioEx.Message);
                isConnected = false;
              
                return false;
            }
            catch (System.ObjectDisposedException disposeEx)
            {
                LogHelper.objLog.Error(disposeEx.Message);
                isConnected = false;
             
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.objLog.Error(ex.Message);
                return false;
            }


        }

        public bool WriteArray(string addressName, string[] content, TCPType clientType = TCPType.Common)
        {

            try
            {
                PLCAddress address = this.adrsWrite.Where(a => a.Name == addressName).FirstOrDefault();
                if (address == null)
                {
                    return false;
                }
                return WriteArray(address, content, clientType);
            }

            catch (System.IO.IOException ioEx)
            {
                LogHelper.objLog.Error(ioEx.Message);
                isConnected = false;

                return false;
            }
            catch (System.ObjectDisposedException disposeEx)
            {
                LogHelper.objLog.Error(disposeEx.Message);
                isConnected = false;

                return false;
            }
            catch (Exception ex)
            {
                LogHelper.objLog.Error(ex.Message);
                return false;
            }
        }

        public bool Write(PLCAddress address, string content, TCPType clientType)
        {
            lock (objLock)
            {
                try
                {
                    if (!isConnected)
                    {
                        return false;
                    }
                    if (address == null)
                    {
                        return false;
                    }
                    if (address.CellCount < 1)
                    {
                        return false;
                    }
                    string[] contents = new string[1];
                    contents[0] = content;
                    byte[] btCmd = GetWriteCommand(address, contents, clientType);
                    if (null == btCmd)
                    {
                        return false;
                    }


                    if (!Send(btCmd, clientType))
                    {
                        return false;
                    }
                    int count = TIME_COUNT;
                    byte[] rcvMsg = null;
                    for (int i = 0; i < count; i++)
                    {
                        if (!Receive(out rcvMsg, clientType))
                        {

                            continue;
                        }
                        if (null != rcvMsg)
                        {
                            break;
                        }

                    }
                    if (null == rcvMsg)
                    {
                        LogHelper.objLog.Error("plc write timeout");
                        //  System.Diagnostics.Debugger.Log(0, null, string.Format("plc::write timeout\r\n"));

                        return false;
                    }
                    else
                    {

                        if (ParseWriteRep(rcvMsg)/*address.ParseMsgWrite(rcvMsg)*/)
                        {
                            return true;
                        }
                        else
                        {
                            //UILogManager.Instance.AddLog(UILogLevel.Error, string.Format("write cmd:{0},rep:{1}", cmdMsg, rcvMsg));
                            return false;
                        }
                    }
                }

                catch (System.IO.IOException ioEx)
                {
                    isConnected = false;

                    return false;
                }
                catch (System.ObjectDisposedException disposeEx)
                {
                    isConnected = false;

                    return false;
                }
                catch (Exception ex)
                {
                    LogHelper.objLog.Error(ex.Message);
                    return false;
                }

            }
        }

        //从指定的软件元开始连续发送数据
        public bool WriteArray(PLCAddress address, string[] arrayConten, TCPType clientType)
        {
            lock (objLock)
            {
                try
                {
                    if (!isConnected)
                    {
                        return false;
                    }
                    if (address == null)
                    {
                        return false;
                    }
                    if (address.CellCount < 1)
                    {
                        return false;
                    }
                    byte[] btCmd = GetWriteCommand(address, arrayConten, clientType);
                    if (null == btCmd)
                    {
                        return false;
                    }
                    System.Diagnostics.Debugger.Log(0, null, string.Format("plc::WriteCommand,[{0}]\r\n", BitConverter.ToString(btCmd, 0)));

                    if (!Send(btCmd, clientType))
                    {
                        return false;
                    }
                    int count = TIME_COUNT;
                    byte[] rcvMsg = null;
                    for (int i = 0; i < count; i++)
                    {
                        if (!Receive(out rcvMsg, clientType))
                        {
                            if (IsUdp)
                            {
                                Thread.Sleep(1);
                            }
                            //Thread.Sleep(1);
                            continue;
                        }
                        if (null != rcvMsg)
                        {
                            break;
                        }
                        else
                        {
                            if (IsUdp)
                            {
                                Thread.Sleep(1);
                            }
                        }
                    }
                    if (null == rcvMsg)
                    {
                        //  System.Diagnostics.Debugger.Log(0, null, string.Format("plc::write timeout\r\n"));
                        LogHelper.objLog.Error("plc::write timeout");
                        //UILogManager.Instance.AddLog(UILogLevel.Error, string.Format("write cmd:{0},timeout", cmdMsg));
                        return false;
                    }
                    else
                    {
                        // System.Diagnostics.Debugger.Log(0, null, string.Format("plc::WriteRep,[{0}]\r\n", BitConverter.ToString(rcvMsg, 0)));
                       // LogHelper.objLog.Error("plc::WriteRep");

                        if (ParseWriteRep(rcvMsg)/*address.ParseMsgWrite(rcvMsg)*/)
                        {
                            return true;
                        }
                        else
                        {
                            //UILogManager.Instance.AddLog(UILogLevel.Error, string.Format("write cmd:{0},rep:{1}", cmdMsg, rcvMsg));
                            return false;
                        }
                    }
                }

                catch (System.IO.IOException ioEx)
                {
                    isConnected = false;

                    return false;
                }
                catch (System.ObjectDisposedException disposeEx)
                {
                    isConnected = false;

                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }


            }
        }

        /// <summary>
        /// 调用之前必须得先调用SetCommunicationParams设置IP和端口
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            if (false)
            {
                return ConnectByUdp();
            }
            else
            {
                return ConnectByTcp();
            }
        }

        public bool Reconnect()
        {
            try
            {
                return Connect();
            }
            catch (Exception)
            {
                //ConnectionStatus = false;
                return false;
            }
        }

        public bool Disconnect()
        {
            try
            {
                if (isConnected)
                {
               //     clientHeartBeat.Disconnect();
                  //  clientDataMontior.Disconnect();
                    clientCommon.Disconnect();

                 //   udpDataMontior.Disconnect();
                 //   udpHeartBeat.Disconnect();
                 //   udpCommon.Disconnect();
                    isConnected = false;
                }
            }
            catch (Exception)
            {
                isConnected = false;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 断开连接、关闭心跳、停止监控PLC地址
        /// </summary>
        public void Dispose()
        {
            Disconnect();
            //HeartBeatStop();
            DataMontiorStop();
        }

        public void Init()
        {
            DataMontiorStart();
         
        }

    }
}
