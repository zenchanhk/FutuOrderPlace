using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using FontAwesome.Sharp;
using FTWrapper;
using FutuOrderPlace.UI;
using static Futu.OpenApi.Pb.TrdCommon;

namespace FutuOrderPlace
{
    public class Util
    {
        internal static readonly FontFamily mdIcons =
            Assembly.GetExecutingAssembly().GetFont("UI/fonts", "Material Design Icons");

        public class Color
        {
            public static System.Windows.Media.Color Black { get; private set; } = Util.ConvertStringToColor("#FF000000");
            public static System.Windows.Media.Color Red { get; private set; } = Util.ConvertStringToColor("#FFFF0000");
            public static System.Windows.Media.Color Green { get; private set; } = Util.ConvertStringToColor("#FF00FF00");
            public static System.Windows.Media.Color Yellow { get; private set; } = Util.ConvertStringToColor("#FFFFFF00");
            public static System.Windows.Media.Color Orange { get; private set; } = Util.ConvertStringToColor("#FFFF8C00");
            public static System.Windows.Media.Color Indigo { get; private set; } = Util.ConvertStringToColor("#FF4B0082");
            public static System.Windows.Media.Color Transparent { get; private set; } = Util.ConvertStringToColor("#00FFFFFF");
            public static System.Windows.Media.Color AliceBlue { get; private set; } = Util.ConvertStringToColor("#FF87CEFA");
            public static System.Windows.Media.Color Purple { get; private set; } = Util.ConvertStringToColor("#FF800080");
            public static System.Windows.Media.Color DimGray { get; private set; } = Util.ConvertStringToColor("#FF696969");
            public static System.Windows.Media.Color Gray { get; private set; } = Util.ConvertStringToColor("#FF808080");
            public static System.Windows.Media.Color Blue { get; private set; } = Util.ConvertStringToColor("##FF0000FF");
        }
        public static Image MaterialIconToImage(MaterialIcons icon, System.Windows.Media.Color? color = null, int size = 16)
        {
            Image i = new Image();
            SolidColorBrush brush = color != null ? new SolidColorBrush((System.Windows.Media.Color)color) :
                new SolidColorBrush(Util.Color.DimGray);
            i.Source = Util.mdIcons.ToImageSource<MaterialIcons>(icon, brush, size);
            return i;
        }
        public static System.Windows.Media.Color ConvertStringToColor(String hex)
        {
            //remove the # at the front
            hex = hex.Replace("#", "");

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;

            int start = 0;

            //handle ARGB strings (8 characters long)
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                start = 2;
            }

            //convert RGB characters to bytes
            r = byte.Parse(hex.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

            return System.Windows.Media.Color.FromArgb(a, r, g, b);
        }
    }

    public class StatusToTooltipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ConnectionStatus ic = value as ConnectionStatus;
            string tooltip = "";
            if (ic != null)
            {
                tooltip = "Quote: " + (ic.IsQotConnected?"Connected[" + ic.QotConnectID + "]" : "Disconnected") + System.Environment.NewLine +
                    "Trade: " + (ic.IsTrdConnected ? "Connected[" + ic.TrdConnectID + "]" + (ic.IsLocked?"(Locked)":"(Unlocked)") : "Disconnected");
            }
            return tooltip;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IntToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color red = Color.FromArgb(255, 255, 76, 118);
            Color green = Color.FromArgb(255, 103, 223, 43);
            Color color;
            if (int.Parse(value.ToString()) == 0)
                color = red;
            else
                color = green;
            return new SolidColorBrush(color);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConvertItemToIndex : IValueConverter
    {
        #region IValueConverter Members
        //Convert the Item to an Index
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                //Get the CollectionView from the DataGrid that is using the converter
                DataGrid dg = (DataGrid)Application.Current.MainWindow.FindName("watchListGrid");
                CollectionView cv = (CollectionView)dg.Items;
                //Get the index of the item from the CollectionView
                int rowindex = cv.IndexOf(value) + 1;

                return rowindex.ToString();
            }
            catch (Exception e)
            {
                throw new NotImplementedException(e.Message);
            }
        }
        //One way binding, so ConvertBack is not implemented
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class StatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.Media.Color color = Util.ConvertStringToColor("#00FFFFFF");
            ConnectionStatus status = value as ConnectionStatus;
            if (status.IsQotConnected)
                color = Util.Color.Green;
            //else if (value.ToString().ToLower() == "connecting")
            //    color = Util.Color.Yellow;
            //else if (value.ToString().ToLower() == "error")
            //    color = Util.Color.Red;
            else if (!status.IsQotConnected)
                color = Util.Color.Orange;
            return new SolidColorBrush(color);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToIconColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.Media.Color color = Util.ConvertStringToColor("#00FFFFFF");
            ConnectionStatus status = value as ConnectionStatus;
            if (status.IsQotConnected)
                color = Util.Color.Orange;
            //else if (value.ToString().ToLower() == "connecting")
            //    color = Util.Color.Yellow;
            //else if (value.ToString().ToLower() == "error")
            //    color = Util.Color.Red;
            else if (!status.IsQotConnected)
                color = Util.Color.Green;
            return new SolidColorBrush(color);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ConnectionStatus status = value as ConnectionStatus;
            string icon = "PowerPlugOff";
            if (status.IsQotConnected)
                icon = "PowerPlugOff";
            //else if (value.ToString().ToLower() == "connecting")
            //    icon = "PowerPlug";
            else if (!status.IsQotConnected)
                icon = "PowerPlug";
            return icon;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class LockToCommandIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageSource img;
            bool isLocked = (bool)value;
            if (isLocked)
            {
                img = Util.mdIcons.ToImageSource<MaterialIcons>(MaterialIcons.LockOpen, new SolidColorBrush(Util.Color.Green));
            }
            else
            {
                img = Util.mdIcons.ToImageSource<MaterialIcons>(MaterialIcons.Lock, new SolidColorBrush(Util.Color.Red));
            }
            return img;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class LockToShowIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool status = (bool)value;
            string icon = "Lock";
            
                if (status)
                    icon = "Lock";
                else 
                    icon = "LockOpen";
            
            return icon;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolRevConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((bool)value);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class LockToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = (bool)value;
            return new SolidColorBrush(b ? Util.Color.Red : Util.Color.Blue);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToIconTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = "Conenct";
            ConnectionStatus status = (ConnectionStatus)value;
            if (status.IsQotConnected)
                text = "Disconnect";
            //else if (value.ToString().ToLower() == "connecting")
            //    text = "Stop connecting";
            else if (!status.IsQotConnected)
                text = "Conenct";
            return text;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class LockToIconTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = "Unlock";
            bool isLocked = (bool)value;
            if (isLocked)
                text = "Unlock";
            else
                text = "Lock";
            return text;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isEnable = true;
            ConnectionStatus status = value as ConnectionStatus;
            if (status.IsQotConnected)
                isEnable = true;
            else
                isEnable = false;
            return isEnable;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class StatusToVisbilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility vis = Visibility.Visible;
            ConnectionStatus status = value as ConnectionStatus;
            if (status.IsQotConnected)
                vis = Visibility.Visible;
            else
                vis = Visibility.Collapsed;
            return vis;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class StatusToReverseVisbilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility vis = Visibility.Visible;
            ConnectionStatus status = value as ConnectionStatus;
            if (status.IsQotConnected)
                vis = Visibility.Collapsed;
            else
                vis = Visibility.Visible;
            return vis;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class StatusToIconImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageSource img;
            ConnectionStatus status = value as ConnectionStatus;
            if (status.IsQotConnected)
            {
                img = Util.mdIcons.ToImageSource<MaterialIcons>(MaterialIcons.PowerPlugOff, new SolidColorBrush(Util.Color.Red));
            }
            else
            {
                img = Util.mdIcons.ToImageSource<MaterialIcons>(MaterialIcons.PowerPlug, new SolidColorBrush(Util.Color.Green));
            }
            return img;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NumToPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int per = (int)Math.Round(float.Parse(value.ToString()) * 100, 0);
            return "Zoom: " + per + "%";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = (bool)value;
            return new SolidColorBrush(b ? Util.Color.AliceBlue : Util.Color.Transparent);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class BoolToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Pin" : "PinOff";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class IsActivateToBtnContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Stop" : "Activate";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class IsSentToVisRevConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Collapsed : Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class IsSentToVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class OrderStatusToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = "";
            OrderStatus s = (OrderStatus)value;
            if (s != null)
            {
                switch (s)
                {
                    case OrderStatus.OrderStatus_Unsubmitted:
                        status = "Unsubmitted";
                        break;
                    case OrderStatus.OrderStatus_Unknown:
                        status = "Unknown";
                        break;
                    case OrderStatus.OrderStatus_WaitingSubmit:
                        status = "WaitingSubmit";
                        break;
                    case OrderStatus.OrderStatus_Submitting:
                        status = "Submitting";
                        break;
                    case OrderStatus.OrderStatus_SubmitFailed:
                        status = "SubmitFailed";
                        break;
                    case OrderStatus.OrderStatus_TimeOut:
                        status = "TimeOut";
                        break;
                    case OrderStatus.OrderStatus_Submitted:
                        status = "Submitted";
                        break;
                    case OrderStatus.OrderStatus_Filled_Part:
                        status = "Filled partly";
                        break;
                    case OrderStatus.OrderStatus_Filled_All:
                        status = "Filled";
                        break;
                    case OrderStatus.OrderStatus_Cancelling_Part:
                        status = "Cancelling partly";
                        break;
                    case OrderStatus.OrderStatus_Cancelling_All:
                        status = "Cancelling all";
                        break;
                    case OrderStatus.OrderStatus_Cancelled_Part:
                        status = "Cancelled partly";
                        break;
                    case OrderStatus.OrderStatus_Cancelled_All:
                        status = "Cancelled all";
                        break;
                    case OrderStatus.OrderStatus_Failed:
                        status = "Failed";
                        break;
                    case OrderStatus.OrderStatus_Disabled:
                        status = "Disabled";
                        break;
                    case OrderStatus.OrderStatus_Deleted:
                        status = "Deleted";
                        break;
                    case OrderStatus.OrderStatus_FillCancelled:
                        status = "FillCancelled";
                        break;
                    default:
                        break;
                }
            }
            return status;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class BoolToVisbilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class BoolToRevVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Collapsed : Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TimeToEnableConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
           DateTime schTime = (DateTime)value[0];
            bool isActivated = (bool)value[1];
            bool isEnabled = false;
            if (!isActivated && schTime < DateTime.Now)
                isEnabled = false;
            else
                isEnabled = true;
            return isEnabled;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
