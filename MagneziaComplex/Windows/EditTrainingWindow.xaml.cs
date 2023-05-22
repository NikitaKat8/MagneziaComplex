using MagneziaComplex.Classes;
using MagneziaComplex.EF;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    /// Логика взаимодействия для EditTrainingWindow.xaml
    /// </summary>
    public partial class EditTrainingWindow : Window
    {
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        public static extern void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        VisualObjectActions vActions = new VisualObjectActions();

        Training training = null;
        bool editMode;
        public EditTrainingWindow(Training tr, bool isEdit)
        {
            InitializeComponent();
            training = tr;
            editMode = isEdit;
            if(isEdit)
            {
                tbName.Text = tr.Title;
                tbDescription.Text = tr.Description;
                tbPrice.Text = tr.Price.ToString();
                tbDuration.Text = tr.DurationMinutes.ToString();

                if (tr.isActive)
                {
                    rbActual.IsChecked = true;
                    rbNotActual.IsChecked = false;
                }
                else
                {
                    rbNotActual.IsChecked = true;
                    rbActual.IsChecked = false;
                }
            }
            else
            {
                btnEdit.Content = "Добавить";
            }


        }

        private void rbActual_Checked(object sender, RoutedEventArgs e)
        {
            rbNotActual.IsChecked = false;
        }

        private void rbNotActual_Checked(object sender, RoutedEventArgs e)
        {
            rbActual.IsChecked = false;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbName.Text) || string.IsNullOrWhiteSpace(tbPrice.Text) || string.IsNullOrWhiteSpace(tbDescription.Text) || string.IsNullOrWhiteSpace(tbDuration.Text))
              
            {
                MessageWindow msg = new MessageWindow("Некоторые поля не заполнены");
                msg.ShowDialog();
                return;
            }
           
            if(rbActual.IsChecked == false && rbNotActual.IsChecked == false)
            {
                MessageWindow msg = new MessageWindow("Выберите актуальность");
                msg.ShowDialog();
                return;
            }

            bool actual;
            if (rbActual.IsChecked == true)
            {
                actual = true;
            }
            else
            {
                actual = false;
            }


            try
            {
                if (editMode)
                {
                    training.Title = tbName.Text;
                    training.Description = tbDescription.Text;
                    training.Price = Convert.ToDecimal(tbPrice.Text);
                    training.DurationMinutes = Convert.ToInt32(tbDuration.Text);                
                    training.isActive = actual;

                    AppData.Context.SaveChanges();

                    MessageWindow msg = new MessageWindow("Тренировка изменена");
                    msg.ShowDialog();
                    
                }
                else
                {
                    AppData.Context.Training.Add(new EF.Training
                    {
                        Title = tbName.Text,
                        Description = tbDescription.Text,
                        Price = Convert.ToDecimal(tbPrice.Text),
                        DurationMinutes = Convert.ToInt32(tbDuration.Text),
                        isActive = actual
                    });
                    AppData.Context.SaveChanges();

                    MessageWindow msg = new MessageWindow("Тренировка добавлена");
                    msg.ShowDialog();
                    
                }

                this.Close();

            }
            catch (Exception ex)
            {
                MessageWindow msg = new MessageWindow(ex.Message);
                msg.ShowDialog();
                return;
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

        private void tbPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Handled != "1234567890".IndexOf(e.Text) < 0)
            {
                e.Handled = true;
            }
        }

        private void tbDuration_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Handled != "1234567890".IndexOf(e.Text) < 0)
            {
                e.Handled = true;
            }
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
    }
}
