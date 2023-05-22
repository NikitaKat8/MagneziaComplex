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
    /// Логика взаимодействия для EditSubscriptionWindow.xaml
    /// </summary>
    public partial class EditSubscriptionWindow : Window
    {
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        public static extern void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        VisualObjectActions vActions = new VisualObjectActions();

        EF.Subscription editSub = null;
        bool editMode = false;
        public EditSubscriptionWindow(EF.Subscription sub, bool isEdit)
        {
            InitializeComponent();
            if(isEdit)
            {
                editSub = sub;
                editMode= true;
                tbName.Text = sub.Name;
                tbDescription.Text = sub.Description;
                tbPrice.Text = sub.Price.ToString();
                tbDiscount.Text = sub.Discount.ToString();
                tbMonthQty.Text = sub.CountMonth.ToString();
            }
            else
            {
                btnEdit.Content = "Добавить";
            }
            

        }

        private void AppWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HwndSource hwndSource;
            hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            IntPtr hwnd = hwndSource.Handle;

            ReleaseCapture();
            SendMessage(hwnd, 0x112, 0xf012, 0);
        }

        private void btnCancel_MouseEnter(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void btnCancel_MouseLeave(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderReset(sender);
        }

        private void btnEdit_MouseEnter(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void btnEdit_MouseLeave(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderReset(sender);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(tbName.Text) || string.IsNullOrWhiteSpace(tbPrice.Text) || string.IsNullOrWhiteSpace(tbDescription.Text) || string.IsNullOrWhiteSpace(tbMonthQty.Text) 
                || string.IsNullOrWhiteSpace(tbDiscount.Text))
            {
                MessageWindow msg = new MessageWindow("Некоторые поля не заполнены");
                msg.ShowDialog();
                return;
            }
            if(Convert.ToDouble(tbDiscount.Text) > 1)
            {
                MessageWindow msg = new MessageWindow("Некорректный ввод скидки");
                msg.ShowDialog();
                return;
                
            }
            try
            {
                if (editMode)
                {
                    editSub.Name = tbName.Text;
                    editSub.Description = tbDescription.Text;
                    editSub.Price = Convert.ToDecimal(tbPrice.Text);
                    editSub.CountMonth = Convert.ToInt32(tbMonthQty.Text);
                    editSub.Discount = Convert.ToDecimal(tbDiscount.Text);

                    AppData.Context.SaveChanges();

                    MessageWindow msg = new MessageWindow("Абонемент изменён");
                    msg.ShowDialog();
                }
                else
                {
                    AppData.Context.Subscription.Add(new EF.Subscription
                    {
                        Name = tbName.Text,
                        Description = tbDescription.Text,
                        Price = Convert.ToDecimal(tbPrice.Text),
                        CountMonth = Convert.ToInt32(tbMonthQty.Text),
                        Discount = Convert.ToDecimal(tbDiscount.Text)
                    });
                    AppData.Context.SaveChanges();

                    MessageWindow msg = new MessageWindow("Абонемент добавлен");
                    msg.ShowDialog();
                }
               
                this.Close();

            }
            catch(Exception ex) 
            {
                MessageWindow msg = new MessageWindow(ex.Message);
                msg.ShowDialog();
                return;
            }

           
        }

        private void tbPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Handled != "1234567890".IndexOf(e.Text) < 0)
            {
                e.Handled = true;
            }
        }

        private void tbMonthQty_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Handled != "1234567890".IndexOf(e.Text) < 0)
            {
                e.Handled = true;
            }
        }

        private void tbDiscount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Handled != "1234567890.".IndexOf(e.Text) < 0)
            {
                e.Handled = true;
            }
           
        }

       
    }
}
