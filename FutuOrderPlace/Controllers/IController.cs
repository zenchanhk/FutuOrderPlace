using Futu.OpenApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutuOrderPlace
{
    public class ConnectedEventArgs : EventArgs
    {
        public FTController Controller { get; private set; }
        public FTAPI_Conn Client { get; private set; }
        public long ErrCode { get; private set; }
        public string Message { get; private set; }
        public ConnectedEventArgs(FTController ctrl, FTAPI_Conn client, long errCode, string message)
        {
            Controller = ctrl;
            Client = client;
            ErrCode = errCode;
            Message = message;
        }
    }

    public class DisconnectedEventArgs : EventArgs
    {
        public FTController Controller { get; private set; }
        public FTAPI_Conn Client { get; private set; }
        public long ErrCode { get; private set; }
        public DisconnectedEventArgs(FTController ctrl, FTAPI_Conn client, long errCode)
        {
            Controller = ctrl;
            Client = client;
            ErrCode = errCode;
        }
    }

    public class ConnectionStatus : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


        private bool _pIsQotConnected;
        public bool IsQotConnected
        {
            get { return _pIsQotConnected; }
            set
            {
                if (_pIsQotConnected != value)
                {
                    _pIsQotConnected = value;
                    OnPropertyChanged("IsQotConnected");
                }
            }
        }

        private bool _pIsTrdConnected;
        public bool IsTrdConnected
        {
            get { return _pIsTrdConnected; }
            set
            {
                if (_pIsTrdConnected != value)
                {
                    _pIsTrdConnected = value;
                    OnPropertyChanged("IsTrdConnected");
                }
            }
        }

        private ulong _pQotConnectID;
        public ulong QotConnectID
        {
            get { return _pQotConnectID; }
            set
            {
                if (_pQotConnectID != value)
                {
                    _pQotConnectID = value;
                    OnPropertyChanged("QotConnectID");
                }
            }
        }

        private ulong _pTrdConnectID;
        public ulong TrdConnectID
        {
            get { return _pTrdConnectID; }
            set
            {
                if (_pTrdConnectID != value)
                {
                    _pTrdConnectID = value;
                    OnPropertyChanged("TrdConnectID");
                }
            }
        }

        private string _pQotError;
        public string QotError
        {
            get { return _pQotError; }
            set
            {
                if (_pQotError != value)
                {
                    _pQotError = value;
                    OnPropertyChanged("QotError");
                }
            }
        }

        private string _pTrdError;
        public string TrdError
        {
            get { return _pTrdError; }
            set
            {
                if (_pTrdError != value)
                {
                    _pTrdError = value;
                    OnPropertyChanged("TrdError");
                }
            }
        }

        private bool _pIsLocked = true;
        public bool IsLocked
        {
            get { return _pIsLocked; }
            set
            {
                if (_pIsLocked != value)
                {
                    _pIsLocked = value;
                    OnPropertyChanged("IsLocked");
                }
            }
        }

    }

    public interface IController
    {
        event EventHandler<ConnectedEventArgs> Connected;
        event EventHandler<DisconnectedEventArgs> Disconnected;
        // should be unique
        string DisplayName { get; }
        string Vendor { get; }  //short name
        string VendorFullName { get; }
        ConnectionParam ConnParam { get; set; }
        bool IsConnected { get; }
        ConnectionStatus ConnectionStatus { get; }
        void Connect();
        void Disconnect();
        
    }
}
