using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
//using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using FTWrapper;
using Futu.OpenApi.Pb;
using Newtonsoft.Json;
//using static Futu.OpenApi.Pb.QotCommon;
using static Futu.OpenApi.Pb.TrdCommon;

namespace FutuOrderPlace
{    

    public class Symbol : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Contract Contract { get; set; }
        public Symbol() { }

        private string _Code;
        public string Code
        {
            get { return _Code; }
            set
            {
                if (_Code != value)
                {
                    _Code = value;
                    OnPropertyChanged("Code");
                }
            }
        }

        public Security Security { get; set; }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string PinYin { set; get; }

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this,
                new PropertyChangedEventArgs(name));
        }

        public override string ToString()
        {
            return Name;
        }
    }
    public class Ticker
    {
        public Security Security { get; set; }
        public DateTime Time { get; set; }
        public double Volume { get; set; }
        public double Price { get; set; }
    }
    public class OHLCBar
    {
        public Security Security { get; set; }
        public DateTime Time { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }

    }

    public class OutStanding
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double Last { get; set; }
        public double Volume { get; set; }
        public DateTime PeekTime { get; set; }
        public DateTime BarTime { get; set; }
        public String Remark { get; set; }

    }
    public class Quote : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _Checked;
        public bool Checked
        {
            get { return _Checked; }
            set
            {
                if (_Checked != value)
                {
                    _Checked = value;
                    OnPropertyChanged("Checked");
                }
            }
        }
        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                if (_code != value)
                {
                    _code = value;
                    OnPropertyChanged("Code");
                }
            }
        }

        public Security Security { get; set; }

        private double _last;
        public double Last
        {
            get { return _last; }
            set
            {
                if (_last != value)
                {
                    _last = value;
                    OnPropertyChanged("Last");
                }
            }
        }

        private double _volume;
        public double Volume
        {
            get { return _volume; }
            set
            {
                if (_volume != value)
                {
                    _volume = value;
                    OnPropertyChanged("Volume");
                }
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private DateTime _time;
        public DateTime Time
        {
            get { return _time; }
            set
            {
                if (_time != value)
                {
                    _time = value;
                    OnPropertyChanged("Time");
                }
            }
        }

        private double _pDayHigh;
        public double DayHigh
        {
            get { return _pDayHigh; }
            set
            {
                if (_pDayHigh != value)
                {
                    _pDayHigh = value;
                    OnPropertyChanged("DayHigh");
                }
            }
        }

        private double _LastClose;
        public double LastClose
        {
            get { return _LastClose; }
            set
            {
                if (_LastClose != value)
                {
                    _LastClose = value;
                    OnPropertyChanged("LastClose");
                }
            }
        }

        private double _LastVolume;
        public double LastVolume
        {
            get { return _LastVolume; }
            set
            {
                if (_LastVolume != value)
                {
                    _LastVolume = value;
                    OnPropertyChanged("LastVolume");
                }
            }
        }

        private double _pLastDayHigh;
        public double LastDayHigh
        {
            get { return _pLastDayHigh; }
            set
            {
                if (_pLastDayHigh != value)
                {
                    _pLastDayHigh = value;
                    OnPropertyChanged("LastDayHigh");
                }
            }
        }


        private DateTime _pLastDate;
        public DateTime LastDate
        {
            get { return _pLastDate; }
            set
            {
                if (_pLastDate != value)
                {
                    _pLastDate = value;
                    OnPropertyChanged("LastDate");
                }
            }
        }

        private DateTime _pLastUpdTs;
        public DateTime LastUpdTs
        {
            get { return _pLastUpdTs; }
            set
            {
                if (_pLastUpdTs != value)
                {
                    _pLastUpdTs = value;
                    OnPropertyChanged("LastUpdTs");
                }
            }
        }

        private string _Fulfilled;
        public string Fulfilled
        {
            get { return _Fulfilled; }
            set
            {
                if (_Fulfilled != value)
                {
                    _Fulfilled = value;
                    OnPropertyChanged("Fulfilled");
                }
            }
        }

        private bool _pIsDirty = false;
        public bool IsDirty
        {
            get { return _pIsDirty; }
            set
            {
                if (_pIsDirty != value)
                {
                    _pIsDirty = value;
                    OnPropertyChanged("IsDirty");
                }
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }

    public class DataSource
    {
        public string ID { get; set; }
        public string Vendor { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }
    }
    public class ScheduledOrder : NotifyPropertyChangedBase
    {
        public int Id { get; set; }
        public uint serialNo;
        private DateTime _pScheduledTime = 
            DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd") + " 09:30", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        public DateTime ScheduledTime
        {
            get { return _pScheduledTime; }
            set { _UpdateField(ref _pScheduledTime, value); }
        }

        public System.Timers.Timer Timer { get; private set; } = new System.Timers.Timer(1000);
        public FTWrapper.Order Order { get; set; }
        public Contract Contract { get; set; }

        private double _pPrice;
        public double Price
        {
            get { return _pPrice; }
            set { _UpdateField(ref _pPrice, value); Order.Price = value; }
        }

        private double _pQuantity;
        public double Quantity
        {
            get { return _pQuantity; }
            set { _UpdateField(ref _pQuantity, value); Order.Quantity = value; }
        }

        private TrdSide _pTrdSide = TrdSide.TrdSide_Buy;
        public TrdSide TrdSide
        {
            get { return _pTrdSide; }
            set { _UpdateField(ref _pTrdSide, value); Order.TradeSide = value; }
        }

        private OrderType _pOrderType = OrderType.OrderType_Normal;
        public OrderType OrderType
        {
            get { return _pOrderType; }
            set { _UpdateField(ref _pOrderType, value); Order.OrderType = value; }
        }

        private DateTime _pTime = DateTime.ParseExact("1999-01-01 09:30", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        public DateTime Time
        {
            get { return _pTime; }
            set { _UpdateField(ref _pTime, value); }
        }

        // only activated order will be sent
        private bool _pIsActivated;
        public bool IsActivated
        {
            get { return _pIsActivated; }
            set { _UpdateField(ref _pIsActivated, value); }
        }
        // if order has been placed
        private bool _pIsSent;
        public bool IsSent
        {
            get { return _pIsSent; }
            set { _UpdateField(ref _pIsSent, value); }
        }

        private string _pMessage;
        public string Message
        {
            get { return _pMessage; }
            set { _UpdateField(ref _pMessage, value); }
        }

        private OrderStatus _pOrderStatus = OrderStatus.OrderStatus_Unknown;
        public OrderStatus OrderStatus
        {
            get { return _pOrderStatus; }
            set { _UpdateField(ref _pOrderStatus, value); }
        }

        private string _pCountdown;
        public string Countdown
        {
            get { return _pCountdown; }
            set { _UpdateField(ref _pCountdown, value); }
        }

    }
    public class Message
    {
        public DateTime Time { get; set; }
        public int Code { get; set; }
        public string Text { get; set; }
        public string Source { get; set; }
    }
    public class Direction
    {
        public TrdSide TrdSide { get; set; }
        public string Desc { get; set; }
    }
    public class TradeEnv
    {
        public TrdEnv TrdEnv { get; set; }
        public string Desc { get; set; }
    }
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static int _id = 0;
        private readonly object idLock = new object();

        public Commands Commands { get; } = new Commands();
        public UserPreference UserPreference { get; private set; }
        public ObservableCollection<Quote> List { get; set; }
        public ObservableCollection<OutStanding> FilteredResult { get; set; }
        //public HashSet<Symbol> AllSymbols { get; set; }
        public ObservableCollection<ScheduledOrder> OrderList { get; private set; } = new ObservableCollection<ScheduledOrder>();
        public List<Direction> Directions { get; } = new List<Direction>();
        public List<TradeEnv> TrdEnvs { get; } = new List<TradeEnv>();

        private HashSet<Symbol> _AllSymbols = new HashSet<Symbol>();
        public HashSet<Symbol> AllSymbols
        {
            get { return _AllSymbols; }
            set
            {
                if (_AllSymbols != value)
                {
                    _AllSymbols = value;
                    OnPropertyChanged("AllSymbols");
                }
            }
        }
        private List<Symbol> _SymbolSearchResult;
        public List<Symbol> SymbolSearchResult
        {
            get { return _SymbolSearchResult; }
            set
            {
                if (_SymbolSearchResult != value)
                {
                    _SymbolSearchResult = value;
                    OnPropertyChanged("SymbolSearchResult");
                }
            }
        }

        private string _pCodeFilter;
        public string CodeFilter
        {
            get { return _pCodeFilter; }
            set
            {
                if (_pCodeFilter != value)
                {
                    _pCodeFilter = value;
                    LookupSymbols(value);
                    OnPropertyChanged("CodeFilter");
                    OnPropertyChanged("SymbolSearchResult");
                }
            }
        }

        private DateTime _pNow = DateTime.Now;
        public DateTime Now
        {
            get { return _pNow; }
            set
            {
                if (_pNow != value)
                {
                    _pNow = value;
                    OnPropertyChanged("Now");
                }
            }
        }

        //check if watch list has a header 
        private bool _header;
        public bool header
        {
            get { return _header; }
            set
            {
                if (_header != value)
                {
                    _header = value;
                    OnPropertyChanged("header");
                }
            }
        }

        private Symbol _SelectedSymbol;
        public Symbol SelectedSymbol
        {
            get { return _SelectedSymbol; }
            set
            {
                if (_SelectedSymbol != value)
                {
                    _SelectedSymbol = value;
                    OnPropertyChanged("SelectedSymbol");
                }
            }
        }

        private int _pInterval = 5;
        public int Interval
        {
            get { return _pInterval; }
            set
            {
                if (_pInterval != value)
                {
                    _pInterval = value;
                    OnPropertyChanged("Interval");
                }
            }
        }
        private TrdEnv  _pTrdEnv = TrdEnv.TrdEnv_Real;
        public TrdEnv TrdEnv
        {
            get { return _pTrdEnv; }
            set
            {
                if (_pTrdEnv != value)
                {
                    _pTrdEnv = value;
                    OnPropertyChanged("TrdEnv");
                }
            }
        }

        private string _pStatusMessage = "Ready";
        public string StatusMessage
        {
            get { return _pStatusMessage; }
            set
            {
                if (_pStatusMessage != value)
                {
                    _pStatusMessage = value;
                    OnPropertyChanged("StatusMessage");
                }
            }
        }
        public ObservableCollection<Message> MessageList { get; private set; } = new ObservableCollection<Message>();
        private bool isScanning = false;
        public ObservableCollection<IController> Controllers { get; private set; } = new ObservableCollection<IController>();

        private readonly string[] markets = { "HK", "US" };//, "SH", "SZ", "HK_FUTURE"};
        private readonly string[] stock_types = { "STOCK", "ETF" };//, "IDX", "ETF", "WARRANT", "BOND", "DRVT"};

        private Dictionary<Security, ObservableCollection<OHLCBar>> KLines = new Dictionary<Security, ObservableCollection<OHLCBar>>();
        private string dtFormat = "yyyy-MM-dd HH:mm:ss";

        // singelton pattern
        public static MainViewModel Instance { get { return instance; } }
        private static readonly MainViewModel instance = new MainViewModel();
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static MainViewModel()
        {
        }
        private MainViewModel()
        {            
            List = new ObservableCollection<Quote>();
            FilteredResult = new ObservableCollection<OutStanding>();
            AllSymbols = new HashSet<Symbol>();
            Directions.Add(new Direction { TrdSide = TrdSide.TrdSide_Buy, Desc ="Buy" });
            Directions.Add(new Direction { TrdSide = TrdSide.TrdSide_Sell, Desc = "Sell" });
            TrdEnvs.Add(new TradeEnv { TrdEnv = TrdEnv.TrdEnv_Real, Desc = "Live" });
            TrdEnvs.Add(new TradeEnv { TrdEnv = TrdEnv.TrdEnv_Simulate, Desc = "Simulated" });
            List.CollectionChanged += List_CollectionChanged;
            //initialization
            //Init();
            ReadSettings();
        }
        private int GetScheduledOrderId()
        {
            lock (idLock)
            {
                return _id++;
            }
        }
        private void List_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                foreach (var item in e.NewItems)
                {
                    Security security = ((Quote)item).Security;
                    if (!KLines.ContainsKey(security))
                    {
                        ObservableCollection<OHLCBar> bars = new ObservableCollection<OHLCBar>();
                        bars.CollectionChanged += Bars_CollectionChanged;
                        KLines.Add(security, bars);
                    }
                }                
            }
        }

        private void Bars_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<OHLCBar> bars = (ObservableCollection<OHLCBar>)sender;
            if (bars.Count > 5)
                bars.RemoveAt(0);
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        
        public void LookupSymbols(string str)
        {
            var result = AllSymbols.Where(x => x.Code.ToUpper().Contains(str.ToUpper())
                                               || x.PinYin.Contains(str.ToUpper())
                                               || x.Name.ToUpper().Contains(str.ToUpper()));
            SymbolSearchResult = new List<Symbol>(result);
        }

        private List<Security> plates = new List<Security>
        { new Security { Market = Futu.OpenApi.Pb.QotCommon.QotMarket.QotMarket_HK_Security, Code = "BK1910" },
            new Security {Market = Futu.OpenApi.Pb.QotCommon.QotMarket.QotMarket_CNSH_Security, Code = "3000002"},
            new Security {Market = Futu.OpenApi.Pb.QotCommon.QotMarket.QotMarket_CNSZ_Security, Code = "3000004"}
        };
        private bool isRequestedSymbols = false;
        public async Task<bool> Request_all_symbols()
        {
            try
            {
                if (isRequestedSymbols) return true;
                                
                FTClient client = null;
                foreach (var ctrl in Controllers)
                {
                    if (ctrl.ConnectionStatus.IsQotConnected)
                        client = ((FTController)ctrl).Client;
                }
                if (client == null) return false;
                foreach (var plate in plates)
                {
                    List<Contract> contracts = await client.RequestSymbols(plate);
                    FillInSymbols(contracts);                    
                }
                isRequestedSymbols = true;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
                        
        }

        public async Task<bool> Request_all_symbols(FTClient client)
        {
            try
            {
                if (isRequestedSymbols) return true;
                if (client == null) return false;
                isRequestedSymbols = true;
                foreach (var plate in plates)
                {
                    List<Contract> contracts = await client.RequestSymbols(plate);
                    FillInSymbols(contracts);
                }                
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        private void FillInSymbols(List<Contract> contracts)
        {
            
            foreach (var contract in contracts)
            {
                AllSymbols.Add(new Symbol()
                {
                    Security = contract.Security,
                    Code = contract.Security.Code,
                    Name = contract.Name,
                    Contract = contract,
                    PinYin = PinYin.GetSpellCode(contract.Name)
                });
            }

        }

        private void QotCallback_UpdateKL(object sender, FTWrapper.Events.UpdateKLEventArgs e)
        {
            //Console.WriteLine("KL update {0}", e.SerialNo);
            if (e.Result.RetType == (int)Common.RetType.RetType_Succeed)
            {
                foreach (var item in e.Result.S2C.KlListList)
                {
                    DateTime d;
                    if (DateTime.TryParseExact(item.Time, dtFormat, CultureInfo.InvariantCulture,
                                     DateTimeStyles.None, out d))
                    {
                        var quote = List.FirstOrDefault(x => (int)x.Security.Market == e.Result.S2C.Security.Market
                            && x.Security.Code == e.Result.S2C.Security.Code);
                        if (quote != null)
                        {
                            ObservableCollection<OHLCBar> bars = KLines[quote.Security];
                            // put the latest date into queue
                            AddKLToKL(bars, item);

                            OHLCBar bar = bars.LastOrDefault();

                            quote.LastUpdTs = DateTime.Now;
                            quote.Time = d;
                            quote.Last = bar.Close;
                            quote.Volume = bar.Volume;
                            if (quote.DayHigh < bar.High)
                                quote.DayHigh = bar.High;

                            bool cb = false; //close break
                            bool vb = false; // volume break
                            bool co = false; // close > open

                            int barNum = 5;
                            if (bar != null && bars.Count == barNum)
                            {
                                if (quote.Last > GetHighest(bars.Select(x => x.High).ToList().GetRange(0, barNum - 1)))
                                    cb = true;
                                if (bar.Volume > GetHighest(bars.Select(x => x.Volume).ToList().GetRange(0, barNum - 1)))
                                    vb = true;
                                var last_bar = bars.LastOrDefault();
                                if (last_bar.Open < last_bar.Close)
                                    co = true;
                            }

                            bool cond1 = false;
                            bool cond2 = false;
                            bool cond3 = false;

                            if (cb && vb && co) cond1 = true;
                            if (quote.Last > quote.LastDayHigh && quote.LastDayHigh > 0) cond2 = true;
                            if (quote.Last > quote.DayHigh && quote.DayHigh > 0 &&
                                quote.Time >= quote.Time.Date.Add(new TimeSpan(9, 45, 0))) cond3 = true;
                            
                            if (cond1 || cond3)
                            {
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    OutStanding tmp = new OutStanding();
                                    tmp.Code = quote.Security.Code;
                                    tmp.Name = quote.Name;
                                    tmp.Last = quote.Last;
                                    tmp.Volume = quote.Volume;
                                    tmp.PeekTime = DateTime.Now;
                                    tmp.BarTime = quote.Time;

                                    if (cond1)
                                        tmp.Remark = "C&V break And C>O;";
                                    //if (cond2)
                                    //    tmp.Remark += "Break dayhigh;";
                                    if (cond3)
                                        tmp.Remark += "Break last dayhigh;";

                                    if (FilteredResult.Count > 0 &&
                                    tmp.Code == FilteredResult[0].Code &&
                                    tmp.Name == FilteredResult[0].Name)
                                    {
                                        FilteredResult.RemoveAt(0);
                                    }                                    
                                    FilteredResult.Insert(0, tmp);
                                });
                            }
                        }
                    }
                }
            }
        }

        private void QotCallback_Subscription(object sender, FTWrapper.Events.SubscriptionEventArgs e)
        {
            //int i = 0;
        }

        private double GetHighest(List<double> queue)
        {
            double result = 0;
            foreach (var item in queue)
            {
                if (item > result)
                    result = item;
            }
            return result;
        }

        private DateTime GetBarTime(DateTime time)
        {
            int day = 0;
            int hour = time.Hour;
            int min = time.Minute;
            if (Interval < 60)
            {
                min = min - min % Interval + (min % Interval > 0 ? Interval : 0);
            }
            else
                min = 0;
            if (min >= 60)
            {
                min = 0;
                hour++;
                if (hour > 23)
                {
                    hour = 0;
                    day++;
                }
            }
            return time.Date.AddDays(day).Add(new TimeSpan(hour, min, 0));
        }

        private void AddTickerToKL(ObservableCollection<OHLCBar> bars, double price, double volume, DateTime time)
        {   
            var bar = bars.FirstOrDefault(x => x.Time == GetBarTime(time));
            if (bar == null)
                bars.Add(new OHLCBar { Time = GetBarTime(time), Open = price, Close = price, High = price, Low = price, Volume = volume });
            else
            {
                if (bar.High < price)
                    bar.High = price;
                if (bar.Low > price)
                    bar.Low = price;
                bar.Close = price;
                bar.Volume += volume;
            }
        }

        private void AddKLToKL(ObservableCollection<OHLCBar> bars, QotCommon.KLine kLine)
        {
            DateTime time = DateTime.ParseExact(kLine.Time, dtFormat, CultureInfo.InvariantCulture);
            var bar = bars.FirstOrDefault(x => x.Time == GetBarTime(time));
            if (bar == null)
                bars.Add(new OHLCBar
                {
                    Time = GetBarTime(time),
                    High = kLine.HighPrice,
                    Low = kLine.LowPrice,
                    Close = kLine.ClosePrice,
                    Open = kLine.OpenPrice,
                    Volume = kLine.Volume
                });
            else
            {
                bar.High = kLine.HighPrice;
                bar.Low = kLine.LowPrice;
                bar.Close = kLine.ClosePrice;
                bar.Open = kLine.OpenPrice;
                bar.Volume = kLine.Volume;
            }
        }

        private void QotCallback_UpdateTicker(object sender, FTWrapper.Events.UpdateTickerEventArgs e)
        {
            if (e.Result.RetType == (int)Common.RetType.RetType_Succeed)
            {
                foreach (var item in e.Result.S2C.TickerListList)
                {
                    DateTime d;
                    if (DateTime.TryParseExact(item.Time, dtFormat, CultureInfo.InvariantCulture,
                                     DateTimeStyles.None, out d))
                    {
                        var quote = List.FirstOrDefault(x => (int)x.Security.Market == e.Result.S2C.Security.Market
                            && x.Security.Code == e.Result.S2C.Security.Code);
                        if (quote != null)
                        {
                            ObservableCollection<OHLCBar> bars = KLines[quote.Security];
                            // put the latest date into queue
                            AddTickerToKL(bars, item.Price, item.Volume,
                                DateTime.ParseExact(item.Time, dtFormat, CultureInfo.InvariantCulture));

                            OHLCBar bar = bars.LastOrDefault();

                            quote.Time = d;
                            quote.Last = item.Price;
                            quote.Volume = bar.Volume;
                            if (quote.DayHigh < item.Price)
                                quote.DayHigh = item.Price;

                            bool cb = false; //close break
                            bool vb = false; // volume break

                            
                            if (bar != null)
                            {
                                if (quote.Last > GetHighest(bars.Select(x => x.High).ToList()))
                                    cb = true;
                                if (bar.Volume >= GetHighest(bars.Select(x => x.Volume).ToList()))
                                    vb = true;
                            }                            

                            
                            
                        }
                    }
                }
            }
        }

        private void QotCallback_RequestHistoryKL(object sender, FTWrapper.Events.RequestHistoryKLEventArgs e)
        {
            var quote = List.FirstOrDefault(x => (int)x.Security.Market == e.Result.S2C.Security.Market
                            && x.Security.Code == e.Result.S2C.Security.Code);
            if (quote == null) return;
                        
            if (e.Result.RetType == (int)Common.RetType.RetType_Succeed)
            {
                foreach (var item in e.Result.S2C.KlListList.OrderByDescending(x => DateTime.ParseExact(x.Time, dtFormat, CultureInfo.InvariantCulture)))
                {
                    DateTime d;
                    if (DateTime.TryParseExact(item.Time, dtFormat, CultureInfo.InvariantCulture,
                                     DateTimeStyles.None, out d))
                    {
                        if (d.Date < DateTime.Now.Date)
                        {
                            quote.LastClose = item.ClosePrice;
                            quote.LastVolume = item.Volume;
                            quote.LastDate = d.Date;
                            quote.LastDayHigh = item.HighPrice;
                            break;
                        }
                        if (d.Date == DateTime.Now.Date)
                        {
                            quote.Last = item.ClosePrice;
                            quote.DayHigh = item.HighPrice;
                        }
                    }
                }
            }
        }

        private Dictionary<IController, HistoryKLQuota> HistoryQuota = new Dictionary<IController, HistoryKLQuota>();
        private async Task<bool> GetQuota()
        {
            HistoryKLQuota quota = null;
            foreach (var ctrl in Controllers)
            {
                if (ctrl.ConnectionStatus.IsQotConnected)
                {
                    quota = await ((FTController)ctrl).Client.RequestHistoryKLQuota<HistoryKLQuota>();
                    if (HistoryQuota.ContainsKey(ctrl))
                        HistoryQuota[ctrl] = quota;
                    else
                        HistoryQuota.Add(ctrl, quota);
                }
            }
            return true;
        }

        private async Task<bool> Scan(Quote item)
        {
            try
            {
                if (item.LastDate < DateTime.Now.AddDays(-10) || item.IsDirty)
                {

                    ObservableCollection<OHLCBar> bars = KLines[item.Security];
                    FTClient client = null;
                    // search in the quota
                    foreach (var quota in HistoryQuota)
                    {
                        var tmp = quota.Value.DetailItems.FirstOrDefault(x => x.Security == item.Security);
                        if (tmp != null)
                        {
                            client = ((FTController)quota.Key).Client;
                            break;
                        }
                    }
                    if (client == null)
                    {
                        foreach (var quota in HistoryQuota)
                        {
                            if (quota.Value.RemainQuota > 0)
                                client = ((FTController)quota.Key).Client;
                        }
                    }
                    if (client == null)
                    {
                        throw new OutOfQuotaException();
                    }
                    // request history data                        
                    client.RequestHistoryData(item.Security, DateTime.Now.AddDays(-5), DateTime.Now, QotCommon.KLType.KLType_Day);
                    // request latest KLines
                    List<QotCommon.KLine> kLines = await client.GetKL(item.Security, FTUtil.IntToKLType(Interval));
                    var quote = List.FirstOrDefault(x => item.Security == x.Security);
                    foreach (var line in kLines)
                    {
                        DateTime time = DateTime.ParseExact(line.Time, dtFormat, CultureInfo.InvariantCulture);
                        if (time > GetBarTime(DateTime.Now))
                            break;

                        if (time.Date == DateTime.Now.Date)
                        {
                            bars.Add(new OHLCBar
                            {
                                Security = item.Security,
                                Open = line.OpenPrice,
                                High = line.HighPrice,
                                Low = line.LowPrice,
                                Close = line.ClosePrice,
                                Volume = line.Volume,
                                Time = GetBarTime(time)
                            });
                            if (quote.DayHigh < line.HighPrice)
                                quote.DayHigh = line.HighPrice;
                        }

                    }
                    //client.RequestMarketData(item.Security, FTUtil.IntToSubType(Interval));
                    //client.RequestMarketData(item.Security, QotCommon.SubType.SubType_Ticker);
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async void Scan()
        {
            isScanning = true;
            await GetQuota();
            try
            {
                foreach (var item in List)
                {
                    await Scan(item);
                }
            }
            catch (Exception e)
            {
                GlobalExceptionHandler.HandleException(this, e, null);
            }
            
        }

        public void ReadSettings()
        {
            UserPreference = JsonConvert.DeserializeObject<UserPreference>(Properties.Settings.Default["preference"].ToString());
            List<IController> all_ctrls = new List<IController>();
            if (UserPreference != null)
            {
                Type t = typeof(MainViewModel);
                string ns = t.Namespace;
                foreach (string vendor in UserPreference.Vendors)
                {
                    AccountOption accOpt = (dynamic)UserPreference.GetType().GetProperty(vendor + "Account").GetValue(UserPreference);
                    foreach (var acc in accOpt.Accounts)
                    {
                        if (acc.IsActivate)
                        {
                            string clsName = ns + "." + vendor + "Controller";
                            Type type = Type.GetType(clsName);
                            IController ctrl = Activator.CreateInstance(type, this) as IController;
                            ctrl.ConnParam = acc;
                            // if some connection is connected, then remain unchanged
                            IController ic = Controllers.FirstOrDefault(x => x.DisplayName == ctrl.DisplayName);
                            if (ic != null)
                            {
                                ic.ConnParam = acc;
                                all_ctrls.Add(ic);
                            }
                            else
                            {
                                all_ctrls.Add(ctrl);
                            }
                        }
                    }
                    // add new controllers
                    foreach (var ctrl in all_ctrls)
                    {
                        IController ic = Controllers.FirstOrDefault(x => x.DisplayName == ctrl.DisplayName);
                        if (ic == null)
                        {
                            Controllers.Add(ctrl);
                            ctrl.Connected += Ctrl_Connected;
                            ctrl.Disconnected += Ctrl_Disconnected;
                        }
                    }


                }
            }

        }

        private void Ctrl_Disconnected(object sender, DisconnectedEventArgs e)
        {
            
        }

        private Timer GetServerTime = new Timer();
        private void Ctrl_Connected(object sender, ConnectedEventArgs e)
        {
            Request_all_symbols(e.Controller.Client);
            e.Controller.Client.QotCallback.RequestHistoryKL += QotCallback_RequestHistoryKL;
            e.Controller.Client.QotCallback.UpdateKL += QotCallback_UpdateKL;
            e.Controller.Client.TrdCallback.PlaceOrder += TrdCallback_PlaceOrder;
            e.Controller.Client.TrdCallback.UpdateOrder += TrdCallback_UpdateOrder;
            e.Controller.Client.TrdCallback.UpdateOrderFill += TrdCallback_UpdateOrderFill;

            // subscribe to order status updates
            e.Controller.Client.SubAccPush();
            // get global state
            e.Controller.Client.QotCallback.GlobalState += (sender1, e1) => QotCallback_GlobalState(sender1, e1, e.Controller);            
            GetServerTime.Interval = 1000;
            GetServerTime.Elapsed += GetServerTime_Elapsed;
            GetServerTime.Start();
        }

        private void TrdCallback_UpdateOrderFill(object sender, FTWrapper.Events.UpdateOrderFillEventArgs e)
        {
            if (e.Result.RetType == (int)Common.RetType.RetType_Succeed)
            {
                ulong id = e.Result.S2C.OrderFill.OrderID;
                ScheduledOrder order = OrderList.FirstOrDefault(x => x.Order.OrderId == id);
                if (order != null)
                {
                    order.OrderStatus = (OrderStatus)e.Result.S2C.OrderFill.Status;
                }
            }
            else
                AddMessage(new Message { Code = e.Result.ErrCode, Source = "UpdateOrderFill", Text = e.Result.RetMsg, Time = DateTime.Now });
        }

        private void TrdCallback_UpdateOrder(object sender, FTWrapper.Events.UpdateOrderEventArgs e)
        {
            if (e.Result.RetType == (int)Common.RetType.RetType_Succeed)
            {
                ulong id = e.Result.S2C.Order.OrderID;
                ScheduledOrder order = OrderList.FirstOrDefault(x => x.Order.OrderId == id);
                if (order != null)
                {
                    order.OrderStatus = (OrderStatus)e.Result.S2C.Order.OrderStatus;
                }
            }
            else
                AddMessage(new Message { Code = e.Result.ErrCode, Source = "UpdateOrder", Text = e.Result.RetMsg, Time = DateTime.Now });
        }

        private void TrdCallback_PlaceOrder(object sender, FTWrapper.Events.PlaceOrderEventArgs e)
        {
            if (e.Result.RetType == (int)Common.RetType.RetType_Succeed)
            {
                ulong id = e.Result.S2C.OrderID;
                ScheduledOrder order = OrderList.FirstOrDefault(x => x.serialNo == e.SerialNo);
                if (order != null)
                    order.Order.OrderId = id;
            }
            else
            {
                AddMessage(new Message { Code = e.Result.ErrCode, Source = "PlaceOrder", Text = e.Result.RetMsg, Time = DateTime.Now });
            }
        }

        private void GetServerTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var ctrl in Controllers)
            {
                if (ctrl.IsConnected)
                    ((FTController)ctrl).Client.GetGlobalState();
            }
            
        }

        private void QotCallback_GlobalState(object sender, FTWrapper.Events.GlobalStateEventArgs e, FTController controller)
        {
            if (e.Result.RetType == (int)Common.RetType.RetType_Succeed)
            {
                Now = Helper.UnixTimeStampToDateTime(e.Result.S2C.LocalTime);
                foreach (var order in OrderList)
                {
                    if (order.IsActivated)
                    {
                        TimeSpan ts = order.ScheduledTime - Now;
                        if (ts.TotalSeconds > 0)
                            order.Countdown = ((int)ts.TotalSeconds).ToString();
                        else
                        {
                            order.Countdown = "Expired";
                            if (!order.IsSent)
                            {
                                uint serialNo = controller.Client.PlaceOrder(order.Order);
                                order.serialNo = serialNo;
                                order.IsSent = true;
                            }                            
                        }                            

                    }
                    else
                        order.Countdown = "N/A";
                }
            }
        }
        private TrdSecMarket MarketConversion(QotCommon.QotMarket qotMarket)
        {
            TrdSecMarket trdSecMarket = TrdSecMarket.TrdSecMarket_Unknown;
            switch (qotMarket)
            {
                case QotCommon.QotMarket.QotMarket_CNSH_Security:
                    trdSecMarket = TrdSecMarket.TrdSecMarket_CN_SH;
                    break;
                case QotCommon.QotMarket.QotMarket_CNSZ_Security:
                    trdSecMarket = TrdSecMarket.TrdSecMarket_CN_SZ;
                    break;
                case QotCommon.QotMarket.QotMarket_HK_Future:
                case QotCommon.QotMarket.QotMarket_HK_Security:
                    trdSecMarket = TrdSecMarket.TrdSecMarket_HK;
                    break;
                case QotCommon.QotMarket.QotMarket_US_Security:
                    trdSecMarket = TrdSecMarket.TrdSecMarket_US;
                    break;
            }
            return trdSecMarket;
        }
        public void AddMessage(Message msg)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                MessageList.Insert(0, msg);
            });
        }
        public void AddOrderList(ScheduledOrder order, int index = 0)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {                
                OrderList.Insert(index, order);
            });
        }

        public void RemoveOrderList(ScheduledOrder order)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                OrderList.Remove(order);
            });
        }
        public void AddSymbol()
        {
            Quote quote = new Quote() { Code = SelectedSymbol.Code, Name = SelectedSymbol.Name, Security = SelectedSymbol.Security };
            
            ScheduledOrder scheduledOrder = new ScheduledOrder
            {
                Id = GetScheduledOrderId(),
                Order = new FTWrapper.Order
                {
                    Code = SelectedSymbol.Code,
                    SecMarket = MarketConversion(SelectedSymbol.Security.Market),
                    TradeEnv = this.TrdEnv
                },
                Contract = SelectedSymbol.Contract,
                Quantity = SelectedSymbol.Contract.LotSize                
            };
            scheduledOrder.Order.OrderType = scheduledOrder.OrderType;
            scheduledOrder.Order.TradeSide = scheduledOrder.TrdSide;
            AddOrderList(scheduledOrder);
        }
        
        public void ReadWatchListFromFile(string path)
        {
            var lines = File.ReadAllLines(path);
            char[] charToTrim = { ' ', '\t', '\r', '\n' };
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Trim(charToTrim) != String.Empty)
                {
                    string code = lines[i].Split(" \t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1];
                    string market = lines[i].Split(" \t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];
                    if (i == 0 && header)
                        continue;

                    int j;
                    Security security = new Security();
                    if (Int32.TryParse(market, out j))
                    {
                        security.Market = (QotCommon.QotMarket)j;
                        security.Code = code;
                        List.Add(new Quote() { Code = code, Security = security, Name = AllSymbols.Where(x => x.Code == code).Select(s => s.Name).DefaultIfEmpty("").First() });
                    }                    
                }
            }
        }

        public void ClearList()
        {
            List.Clear();
        }

        public void saveWList(string path)
        {
            using (StreamWriter writetext = new StreamWriter(path))
            {
                foreach (Quote q in List)
                {
                    writetext.WriteLine(string.Join("\t", new string[] { ((int)q.Security.Market).ToString(), q.Code }));
                }
            }
        }
        public void saveOutput(string path)
        {
            PropertyInfo[] properties = typeof(Quote).GetProperties();
            using (StreamWriter writetext = new StreamWriter(path))
            {
                StringBuilder sb = new StringBuilder();
                // write header
                foreach (PropertyInfo pi in properties)
                {
                    sb.Append(pi.Name).Append(",");
                }
                writetext.WriteLine(sb);
                sb.Clear();
                // write content
                foreach (var q in FilteredResult)
                {
                    foreach (PropertyInfo pi in properties)
                    {
                        sb.Append(pi.GetValue(q)).Append(",");
                    }
                    writetext.WriteLine(sb);
                    sb.Clear();
                }
            }
        }
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //當新增物件到集合時，做一些初始化
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Quote item in e.NewItems)
                {
                    item.PropertyChanged += (x, y) =>
                    {
                        //Do Something
                    };
                }
            }
        }

    }
}
