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
    /// Логика взаимодействия для EditDiscountWindow.xaml
    /// </summary>
    public partial class EditDiscountWindow : Window
    {
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        public static extern void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        VisualObjectActions vActions = new VisualObjectActions();

        EF.Subscription editSub = null;
        public EditDiscountWindow(EF.Subscription sub)
        {
            InitializeComponent();
            editSub = sub;
            tbName.Text = editSub.Name;
            tbDiscount.Text = editSub.Discount.ToString();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbDiscount.Text))
            {
                MessageWindow msg = new MessageWindow("Поле скидки не заполнено");
                msg.ShowDialog();
                return;
            }
            if (Convert.ToDouble(tbDiscount.Text) > 1)
            {
                MessageWindow msg = new MessageWindow("Некорректный ввод скидки");
                msg.ShowDialog();
                return;

            }

            editSub.Discount = Convert.ToDecimal(tbDiscount.Text);
            AppData.Context.SaveChanges();
            this.Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HwndSource hwndSource;
            hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            IntPtr hwnd = hwndSource.Handle;

            ReleaseCapture();
            SendMessage(hwnd, 0x112, 0xf012, 0);
        }

        private void tbDiscount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Handled != "1234567890.".IndexOf(e.Text) < 0)
            {
                e.Handled = true;
            }
        }

        private void btnOk_MouseEnter(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void btnOk_MouseLeave(object sender, MouseEventArgs e)
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
    }
}
