using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Net;
using System.Xml.Serialization;
using CommunicationTookit;
using ValueType = CommunicationTookit.ValueType;

namespace PLCInterface
{
    public class PLCAddress : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 通知属性变化
        /// </summary>
        /// <param name="name"></param>
        public virtual void OnPropertyChanged( string name )
        {
            if ( !isBinded )
            {
                return;
            }
            if ( PropertyChanged != null )
                PropertyChanged( this, new PropertyChangedEventArgs( name ) );
        }
        #endregion

        private bool isPolled;
        private bool isBinded = false;
        private string key;
        private string name;
        private string cellAddress;
        private int cellCount;
        private CellType cellType;
        private MessageType messageType;
        private CommandType commandType;
        private ValueType valueType;

        public PLCAddress( )
        {
            key = DateTime.Now.ToString( "yyyyMMdd_HHmmss" );
            cellType = CellType.DM;
            messageType = MessageType.ASCII;
        }

        public PLCAddress( string name, string address, int cellCount, ValueType valueType, CommandType cmdType, bool isPolled = false )
            : this( )
        {
            this.name = name;
            this.cellAddress = address;
            this.cellCount = cellCount;
            this.valueType = valueType;
            this.commandType = cmdType;
            this.isPolled = isPolled;
        }

        public string Key
        {
            get
            {
                return key;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if ( name != value )
                {
                    name = value;                 
                }
            }
        }

        public string CellAddress
        {
            get
            {
                return cellAddress;
            }
            set
            {
                if ( cellAddress != value )
                {
                    cellAddress = value;
                   
                }
            }
        }

        public int CellCount
        {
            get
            {
                return cellCount;
            }
            set
            {
                if ( cellCount != value )
                {
                    cellCount = value;
                   
                }
            }
        }

        public CellType CellType
        {
            get
            {
                return cellType;
            }
            set
            {
                if ( cellType != value )
                {
                    cellType = value;
                   
                }
            }
        }

        public MessageType MessageType
        {
            get
            {
                return messageType;
            }
            set
            {
                if ( messageType != value )
                {
                    messageType = value;
                   
                }
            }
        }

        public CommandType CommandType
        {
            get
            {
                return commandType;
            }
            set
            {
                if ( commandType != value )
                {
                    commandType = value;
                   
                }
            }
        }

        public ValueType ValueType
        {
            get
            {
                return valueType;
            }
            set
            {
                if ( valueType != value )
                {
                    valueType = value;
                   
                }
            }
        }
        
        public bool IsPolled
        {
            get
            {
                if ( this.commandType == CommandType.Write )
                {
                    return false;
                }
                else
                {
                    return isPolled;
                }
            }
            set { isPolled = value; }
        }

        public bool IsHeart { get; set; }
        [XmlAttribute]
        public string Description { get; set; }

        [XmlIgnore]
        public object FirstValue { get; set; }

        [XmlIgnore]
        public List<object> Values { get; set; }

        public override string ToString( )
        {
            return name + " " + cellAddress + " " + CellCount.ToString( ) + " " + valueType.ToString( );
        }

        public bool ParseReadRepUdp(byte[] rep, out string[] vals)
        {
            vals = null;
            if (null == rep)
            {
                return false;
            }
            if (rep.Length < 14)
            {
                return false;
            }           
            if (rep[12] == 0 /*&& rep[13] == 0*/ )
            {
                //读取数据
                int len = rep.Length - 14;
                if (len < 2)
                {
                    return false;
                }
                if (ValueType == ValueType.INT16)
                {
                    vals = new string[len / 2];
                    for (int i = 0; i < vals.Length; i++)
                    {
                        vals[i] = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(rep, 14 + 2 * i)).ToString();
                    }
                    return true;
                }
                else if (ValueType == ValueType.UINT16)
                {
                    vals = new string[len / 2];
                    for (int i = 0; i < vals.Length; i++)
                    {
                        vals[i] = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(rep, 14 + 2 * i)).ToString();
                    }
                    return true;
                }
                if (ValueType == ValueType.UINT32)
                {
                    vals = new string[len / 4];
                    for (int i = 0; i < vals.Length; i++)
                    {
                        vals[i] = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(rep, 14 + 4 * i)).ToString();
                    }
                    return true;
                }
                else if (ValueType == ValueType.INT32)
                {
                    vals = new string[len / 4];
                    for (int i = 0; i < vals.Length; i++)
                    {
                        vals[i] = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(rep, 14 + 4 * i)).ToString();
                    }
                    return true;
                }
            }
            return false;
        }

        public bool ParseReadRepTcp( byte[] rep, out string[] vals )
        {
            //byte[] btTemp = new byte[32];
            //for ( int i = 0; i < 32; i++ )
            //{
            //    btTemp[i] = Convert.ToByte(i);
            //}
            //string[] strs = new string[3];
            //for ( int i = 0; i < strs.Length; i++ )
            //{
            //    strs[i] = IPAddress.NetworkToHostOrder( BitConverter.ToInt16( btTemp, 0 + 2 * i ) ).ToString( );
            //}

            //string[] strs1 = new string[3];
            //for ( int i = 0; i < strs1.Length; i++ )
            //{
            //    strs1[i] = BitConverter.ToInt16( btTemp, 6 + 2 * i ).ToString( );
            //}

            //string[] strs2 = new string[3];
            //for ( int i = 0; i < strs2.Length; i++ )
            //{
            //    strs2[i] = IPAddress.NetworkToHostOrder( BitConverter.ToInt32( btTemp, 12 + 4 * i ) ).ToString( );
            //}

            //string[] strs3 = new string[2];
            //for ( int i = 0; i < strs3.Length; i++ )
            //{
            //    strs3[i] = BitConverter.ToInt32( btTemp, 24 + 4 * i ).ToString( );
            //}

            vals = null;
            if ( null == rep )
            {
                return false;
            }
            if ( rep.Length < 30 )
            {
                return false;
            }
            if ( rep[11] == 3 && rep[15] != 0 )
            {
                return false;
            }
            if ( rep[28] == 0 /*&& rep[29] == 0*/ )
            {
                //读取数据
                int len = IPAddress.NetworkToHostOrder( BitConverter.ToInt32( rep, 4 ) ) - 22;
                if ( len < 2 )
                {
                    return false;
                }
                if ( ValueType == ValueType.INT16 )
                {
                    vals = new string[len / 2];
                    for ( int i = 0; i < vals.Length; i++ )
                    {
                        vals[i] = IPAddress.NetworkToHostOrder(BitConverter.ToInt16( rep, 30 + 2 * i )).ToString( );
                    }
                    return true;
                }
                else if ( ValueType == ValueType.UINT16 )
                {
                    vals = new string[len / 2];
                    for ( int i = 0; i < vals.Length; i++ )
                    {
                        vals[i] = IPAddress.NetworkToHostOrder(BitConverter.ToInt16( rep, 30 + 2 * i )).ToString( );
                    }
                    return true;
                }
                if ( ValueType == ValueType.UINT32 )
                {
                    vals = new string[len / 4];
                    for ( int i = 0; i < vals.Length; i++ )
                    {
                        vals[i] = IPAddress.NetworkToHostOrder(BitConverter.ToInt32( rep, 30 + 4 * i )).ToString( );
                    }
                    return true;
                }
                else if ( ValueType == ValueType.INT32 )
                {
                    vals = new string[len / 4];
                    for ( int i = 0; i < vals.Length; i++ )
                    {
                        vals[i] = IPAddress.NetworkToHostOrder(BitConverter.ToInt32( rep, 30 + 4 * i )).ToString( );
                    }
                    return true;
                }                
            }
            return false;
        }

        public bool ParseReadRep( byte[] rep, out string[] vals, bool isUdp )
        {
            if (isUdp)
            {
                return ParseReadRepUdp(rep, out vals);
            }
            else
            {
                return ParseReadRepTcp(rep, out vals);
            }
        }

        public bool ParseMsgWrite( string msg )
        {
            return false;
        }

    }

}
