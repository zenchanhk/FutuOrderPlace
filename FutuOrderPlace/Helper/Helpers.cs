using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace FutuOrderPlace
{
    public sealed class TextContainSearch : DependencyObject
    {
        public static void SetText(DependencyObject element, string text)
        {
            var controlSearch = element as Control;
            if (controlSearch != null)
                controlSearch.KeyUp += (sender, e) =>
                {
                    if (sender is ComboBox)
                    {
                        var control = sender as ComboBox;
                        control.IsDropDownOpen = true;
                        var oldText = control.Text;
                        foreach (var itemFromSource in control.ItemsSource)
                        {
                            if (itemFromSource != null)
                            {
                                Object simpleType = itemFromSource.GetType().GetProperty(text).GetValue(itemFromSource, null);
                                String propertOfList = simpleType as string;
                                if (!string.IsNullOrEmpty(propertOfList) && propertOfList.Contains(control.Text))
                                {
                                    control.SelectedItem = itemFromSource;
                                    control.Items.MoveCurrentTo(itemFromSource);
                                    break;
                                }
                            }
                        }
                        control.Text = oldText;
                        TextBox txt = control.Template.FindName("PART_EditableTextBox", control) as TextBox;
                        if (txt != null)
                        {
                            txt.Select(txt.Text.Length, 0);
                        }
                    }
                };
        }

        public static void SetSource(DependencyObject element, string text)
        {
            var controlSearch = element as Control;
            if (controlSearch != null)
                controlSearch.KeyUp += (sender, e) =>
                {
                    if (sender is ComboBox)
                    {
                        var control = sender as ComboBox;
                        control.IsDropDownOpen = true;
                        var oldText = control.Text;
                        foreach (var itemFromSource in control.ItemsSource)
                        {
                            if (itemFromSource != null)
                            {
                                Object simpleType = itemFromSource.GetType().GetProperty(text).GetValue(itemFromSource, null);
                                String propertOfList = simpleType as string;
                                if (!string.IsNullOrEmpty(propertOfList) && propertOfList.Contains(control.Text))
                                {
                                    control.SelectedItem = itemFromSource;
                                    control.Items.MoveCurrentTo(itemFromSource);
                                    break;
                                }
                            }
                        }
                        control.Text = oldText;
                        TextBox txt = control.Template.FindName("PART_EditableTextBox", control) as TextBox;
                        if (txt != null)
                        {
                            txt.Select(txt.Text.Length, 0);
                        }
                    }
                };
        }
    }
    public class Helper
    {
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }

    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void _UpdateField<T>(ref T field, T newValue,
            Action<T> onChangedCallback = null,
            [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return;
            }

            T oldValue = field;

            field = newValue;
            onChangedCallback?.Invoke(oldValue);
            _RaisePropertyChanged(propertyName);
        }

        protected void _RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class GlobalExceptionHandler
    {
        private static string working_dir = System.IO.Directory.GetCurrentDirectory();
        private static string log_dir = working_dir + Path.DirectorySeparatorChar + "omlog" + Path.DirectorySeparatorChar;
        public static void HandleException(object sender, Exception ex, EventArgs args = null, string message = null, bool slient = false)
        {
            if (!slient)
                MessageBox.Show((message == null ? ex.Message : message)
                    + "\nSource: " + ex.Source
                    + "\nSender: " + (sender != null ? sender.ToString() : "")
                    + "\nPlease see the log for details (located at "
                    + working_dir + ")",
                    "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            //YTrace.Trace(message, YTrace.TraceLevel.Information);
            StringBuilder sb = new StringBuilder();

            if (args != null)
            {
                if (args.GetType() == typeof(UnobservedTaskExceptionEventArgs))
                {
                    sb.AppendLine("UnobservedTaskException: " + ex.GetType().Name);
                    //((UnobservedTaskExceptionEventArgs)args).SetObserved();
                }
                if (args.GetType() == typeof(DispatcherUnhandledExceptionEventArgs))
                {
                    sb.AppendLine("DispatcherUnhandledException: " + ex.GetType().Name);
                    //((DispatcherUnhandledExceptionEventArgs)args).Handled = true;
                }
                if (args.GetType() == typeof(UnhandledExceptionEventArgs))
                {
                    sb.AppendLine("Uncaptured exception for current domain: " + ex.GetType().Name);
                }
            }
            else
            {
                sb.AppendLine("No exception event args");
            }


            sb.AppendLine("Sender:" + sender.ToString());
            if (ex != null)
            {
                sb.AppendLine("Exception Message: " + ex.Message);
                if (ex.Source != null) sb.AppendLine("Source: " + ex.Source);
                if (ex.StackTrace != null) sb.AppendLine("StackTrace: " + ex.StackTrace);
                if (ex.InnerException != null)
                {
                    sb.AppendLine("InnerException Message: " + ex.InnerException.Message);
                    if (ex.InnerException.Source != null) sb.AppendLine("InnerException Source: " + ex.InnerException.Source);
                    if (ex.InnerException.StackTrace != null) sb.AppendLine("InnerException StackTrace: " + ex.InnerException.StackTrace);
                }
                // for task AggregateException
                if (ex.GetType() == typeof(AggregateException))
                {
                    foreach (var inner in ((AggregateException)ex).InnerExceptions)
                    {
                        sb.AppendLine("InnerException Message: " + inner.Message);
                        if (inner.Source != null) sb.AppendLine("InnerException Source: " + inner.Source);
                        if (inner.StackTrace != null) sb.AppendLine("InnerException StackTrace: " + inner.StackTrace);
                    }
                }
            }
            Log(sb.ToString());

        }

        public static void LogMessage(string ticker, string message)
        {
            // place your logging code here
            try
            {
                string file_path = ticker + ".log";
                StreamWriter sw = new StreamWriter(file_path, true);
                sw.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ":" + message);
                sw.Close();
            }
            catch (Exception ex)
            {
                
            }
        }
        public static void Log(string lines)
        {
            try
            {
                //string file_path = "\"" + string.Format("{0}error{1:yyyyMMdd}.log\"", log_dir, DateTime.Now);
                string file_path = string.Format("error{0:yyyyMMdd}.log", DateTime.Now);
                StreamWriter sw = new StreamWriter(file_path, true);
                sw.WriteLine(DateTime.Now.ToShortTimeString() + ":" + lines);
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to log message." + "\nException:" + ex.Message, "Log Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    public static class DependencyObjectExtensions
    {
        public static T GetVisualParent<T>(this DependencyObject child) where T : Visual
        {
            while ((child != null) && !(child is T))
            {
                child = VisualTreeHelper.GetParent(child);
            }
            return child as T;
        }

        public static List<Control> AllChildren(DependencyObject parent)
        {
            var _List = new List<Control>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var _Child = VisualTreeHelper.GetChild(parent, i);
                if (_Child is Control)
                {
                    _List.Add(_Child as Control);
                }
                _List.AddRange(AllChildren(_Child));
            }
            return _List;
        }
        public static T FindControl<T>(DependencyObject parentContainer, string controlName)
        {
            var childControls = AllChildren(parentContainer);
            var control = childControls.OfType<Control>().Where(x => x.Name.Equals(controlName)).Cast<T>().First();
            return control;
        }
    }

    public static class ExtensionMethods
    {
        public static int Remove<T>(
            this ObservableCollection<T> coll, Func<T, bool> condition)
        {
            var itemsToRemove = coll.Where(condition).ToList();

            foreach (var itemToRemove in itemsToRemove)
            {
                coll.Remove(itemToRemove);
            }

            return itemsToRemove.Count;
        }
    }

    public static class KeyEventUtility
    {
        // ReSharper disable InconsistentNaming
        public enum MapType : uint
        {
            MAPVK_VK_TO_VSC = 0x0,
            MAPVK_VSC_TO_VK = 0x1,
            MAPVK_VK_TO_CHAR = 0x2,
            MAPVK_VSC_TO_VK_EX = 0x3,
        }
        // ReSharper restore InconsistentNaming

        [DllImport("user32.dll")]
        public static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs( UnmanagedType.LPWStr, SizeParamIndex = 4 )]
        StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

        public static char GetCharFromKey(Key key)
        {
            char ch = ' ';

            int virtualKey = KeyInterop.VirtualKeyFromKey(key);
            var keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            uint scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
            var stringBuilder = new StringBuilder(2);

            int result = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
            switch (result)
            {
                case -1:
                    break;
                case 0:
                    break;
                case 1:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
                default:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
            }
            return ch;
        }
    }
}
