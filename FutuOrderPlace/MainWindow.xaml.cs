using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace FutuOrderPlace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel vm = MainViewModel.Instance;
        public double ScalingFactor
        {
            get { return (double)GetValue(ScalingFactorProperty); }
            set { SetValue(ScalingFactorProperty, value); }
        }
        public static readonly DependencyProperty ScalingFactorProperty =
            DependencyProperty.Register("ScalingFactor", typeof(double), typeof(Window));

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this.vm;
            ScalingFactor = 1;
        }

        private DataGrid gridControl = null;
        private void MainWindow_Initialized(object sender, EventArgs e)
        {
            gridControl = (DataGrid)this.FindName("output");
            ObservableCollection<Quote> src = (ObservableCollection<Quote>)gridControl.ItemsSource;
        }

        private void OpenList(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                vm.ReadWatchListFromFile(openFileDialog.FileName);
        }

        private void SaveList(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                vm.saveWList(saveFileDialog.FileName);
        }

        private void SaveOutput(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                vm.saveOutput(saveFileDialog.FileName);
        }
        private void Setting(object sender, RoutedEventArgs e)
        {
            Setting setting = new Setting();
            bool? dr = setting.ShowDialog();
            if ((bool)dr)
                vm.ReadSettings();
        }
        private void Scan(object sender, RoutedEventArgs e)
        {
            vm.Scan();
        }
        private void RequestAllSymbols(object sender, RoutedEventArgs e)
        {
            vm.Request_all_symbols();
        }
        private void Clear(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure to clear all symbols?",
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                vm.ClearList();
            }
        }
        public void OnAddSymbol(object sender, RoutedEventArgs e)
        {
            vm.AddSymbol();
        }

        private void Mi_Connect_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            IController ctrl = mi.DataContext as IController;
            if (ctrl.IsConnected)
                ctrl.Disconnect();
            else
                ctrl.Connect();
        }

        public void OnDeleteSymbol(object sender, RoutedEventArgs e)
        {
            //vm.List.Remove(x => x.Checked);
            System.Windows.Controls.DataGrid gc = (DataGrid)this.FindName("watchListGrid");
            vm.List.Remove(x => gc.SelectedItems.Contains(x));
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs args)
        {
            base.OnPreviewMouseWheel(args);
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                ScalingFactor += ((args.Delta > 0) ? 0.1 : -0.1);
            }
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs args)
        {
            base.OnPreviewMouseDown(args);
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (args.MiddleButton == MouseButtonState.Pressed)
                {
                    //RestoreScalingFactor(uiScaleSlider, args);
                }
            }
        }

        void RestoreScalingFactor(object sender, MouseButtonEventArgs args)
        {
            ((Slider)sender).Value = 1.0;
        }

        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (vm.SelectedSymbol == null && vm.SymbolSearchResult != null && vm.SymbolSearchResult.Count > 0)
                vm.SelectedSymbol = vm.SymbolSearchResult[0];
        }


    }

    #region Ticker
    public class Clocking : INotifyPropertyChanged
    {
        public Clocking()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000; // 1 second updates
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        public string Now
        {
            get { return DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"); }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Now"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    #endregion
}
