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
using MagneziaComplex;
using MagneziaComplex.Classes;

namespace MagneziaComplex
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        public static extern void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        VisualObjectActions vActions = new VisualObjectActions();
        public LoginWindow()
        {
            InitializeComponent();
            
            
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HwndSource hwndSource;
            hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            IntPtr hwnd = hwndSource.Handle;

            ReleaseCapture();
            SendMessage(hwnd, 0x112, 0xf012, 0);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogin_MouseEnter(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void btnLogin_MouseLeave(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderReset(sender);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            vActions.ButtonBorderClick(sender);
            try
            {
                var accountCheck = AppData.Context.Employee.Where(x => x.Login == tbLog.Text && x.Password == tbPass.Password).FirstOrDefault();
                if (accountCheck != null)
                {
                    AppData.myAccount = accountCheck;
                    
                    MainWindow mw = new MainWindow();
                    mw.Show();
                    this.Close();
                }
                else
                {
                    MessageWindow msg = new MessageWindow("Неверные данные");
                    msg.ShowDialog();
                    return;
                }
            }
            catch(Exception ex) 
            {
                MessageWindow msg = new MessageWindow("Неверные данные");
                msg.ShowDialog();
                return;
            }
        }
    }
}
