using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FutuOrderPlace
{
    public partial class AccountConfig : Window
    {
        public string AccName;
        public string Host;
        public ushort Port;
        public int ClientId;
        public bool IsActivate;
        public bool IsMulti;
        public string UnlockPassword;
        public string UnlockPwdMD5;
        public AccountConfig()
        {
            InitializeComponent();
            //this.Icon = ConvertBitmapToBitmapImage.Convert(icon.ToBitmap(16, System.Drawing.Color.DarkGray));
            //this.Icon = ImageHelper.ImageSourceForBitmap(icon.ToBitmap(16, System.Drawing.Color.DarkGray));   
            //this.Icon = Util.mdIcons.ToImageSource<MaterialIcons>(MaterialIcons.Cogs, new SolidColorBrush(Util.Color.Indigo), 18);
        }

        private void OKBtn_Click(object sender, RoutedEventArgs e)
        {
            AccName = txtName.Text;
            Host = txtHost.Text;
            Port = ushort.Parse(txtPort.Text);
            ClientId = int.Parse(txtClientID.Text);
            IsActivate = (bool)chkIsEnabled.IsChecked;
            IsMulti = (bool)chkIsMulti.IsChecked;
            UnlockPassword = txtPwd.Password.Trim();
            UnlockPwdMD5 = txtPwdMD5.Password.Trim();
            this.DialogResult = true;
            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtName.Text = AccName;
            txtHost.Text = Host;
            txtPort.Text = Port > 0 ? Port.ToString() : "";
            txtClientID.Text = ClientId.ToString();
            chkIsEnabled.IsChecked = IsActivate;
            chkIsMulti.IsChecked = IsMulti;
        }
    }
}
