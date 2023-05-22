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

namespace MagneziaComplex.Windows
{
    /// <summary>
    /// Логика взаимодействия для LogoutWindow.xaml
    /// </summary>
    public partial class LogoutWindow : Window
    {
        public LogoutWindow(string msg)
        {
            InitializeComponent();
            tbMessage.Text = msg;
        }

        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        public static extern void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HwndSource hwndSource;
            hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            IntPtr hwnd = hwndSource.Handle;

            ReleaseCapture();
            SendMessage(hwnd, 0x112, 0xf012, 0);
        }
        VisualObjectActions vActions = new VisualObjectActions();
        private void btnExit_MouseEnter(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void btnExit_MouseLeave(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderReset(sender);
        }

        private void btnCancel_MouseEnter(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void btnCancel_MouseLeave(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderReset(sender);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            AppData.isLogout = true;
            this.Close();
        }
    }
}
