using MagneziaComplex.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MagneziaComplex
{
    /// <summary>
    /// Логика взаимодействия для AddClientSubscriptionWindow.xaml
    /// </summary>
    public partial class AddClientSubscriptionWindow : Window
    {
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        public static extern void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        VisualObjectActions vActions = new VisualObjectActions();
        
        public AddClientSubscriptionWindow()
        {
            InitializeComponent();
            var subs = AppData.Context.Subscription.ToList();
            lvSubs.ItemsSource = subs;
          
        }

        private void AppWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HwndSource hwndSource;
            hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            IntPtr hwnd = hwndSource.Handle;

            ReleaseCapture();
            SendMessage(hwnd, 0x112, 0xf012, 0);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            AppData.clientNewSub = null;
            this.Close();
        }

        private void btnCancel_MouseEnter(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void btnCancel_MouseLeave(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderReset(sender);
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {         
            if (AppData.clientNewSub != null)
            {
           
                this.Close();
            }
            else
            {
                MessageWindow msg = new MessageWindow("Выберите абонемент");
                msg.ShowDialog();
                return;
            }
        }

        private void btnSelect_MouseEnter(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void btnSelect_MouseLeave(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderReset(sender);
        }

        private void lvSubs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvSubs.SelectedItem is EF.Subscription)
            {
                var sub = lvSubs.SelectedItem as EF.Subscription;
                AppData.clientNewSub = sub;
                
            }
        }
    }
}
