using MagneziaComplex.Classes;
using MagneziaComplex.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
using static System.Net.Mime.MediaTypeNames;

namespace MagneziaComplex.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddTimetableWindow.xaml
    /// </summary>
    public partial class AddTimetableWindow : Window
    {


        Employee thisEmpl = new Employee();
        Training thisTraining = new Training();

        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        public static extern void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        VisualObjectActions vActions = new VisualObjectActions();
        public AddTimetableWindow()
        {
            InitializeComponent();

            dpDateStart.DisplayDateStart = DateTime.Today;
            dpDateEnd.IsEnabled = false;
            dpDateEnd.SelectedDate = null;
            timeEnd.IsEnabled = false;

            cmbClub.ItemsSource = AppData.Context.Club.ToList();
            cmbClub.DisplayMemberPath = "Title";

            cmbEmpl.ItemsSource = AppData.Context.Employee.Where(x => x.idRole != 4).ToList();
            cmbEmpl.DisplayMemberPath = "Fio";

            cmbTraining.ItemsSource = AppData.Context.Training.ToList();
            cmbTraining.DisplayMemberPath = "Title";
        }

        private void cmbEmpl_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            cmbEmpl.IsDropDownOpen = true;
        }

        private void AppWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HwndSource hwndSource;
            hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            IntPtr hwnd = hwndSource.Handle;

            ReleaseCapture();
            SendMessage(hwnd, 0x112, 0xf012, 0);
        }

        private void cmbEmpl_GotFocus(object sender, RoutedEventArgs e)
        {
            cmbEmpl.IsDropDownOpen = true;
        }

        private void cmbTraining_TextChanged(object sender, TextChangedEventArgs e)
        {
            cmbTraining.ItemsSource = AppData.Context.Training.Where(x => x.Title.Contains(cmbTraining.Text)).ToList();
            cmbTraining.IsDropDownOpen = true;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
           
            if (timeStart.SelectedTime == null || timeEnd.SelectedTime == null)
            {
                MessageWindow msg = new MessageWindow("Выберите время");
                msg.ShowDialog();
                return;
            }


            var dataStarta = dpDateStart.SelectedDate.Value.ToShortDateString();
            var vremyaStart = timeStart.Text;

            var dataEnda = dpDateEnd.SelectedDate.Value.ToShortDateString();
            var vremyaEnda = timeEnd.Text;

            DateTime DateTimeStart = Convert.ToDateTime(dataStarta +" "+ vremyaStart);
            DateTime DateTimeEnda = Convert.ToDateTime(dataEnda + " " + vremyaEnda);

          

            if (DateTimeStart >= DateTimeEnda)
            {
                MessageWindow msg = new MessageWindow("Время начала не может быть позже времени окончания");
                msg.ShowDialog();
                return;
            }


            if(cmbClub.SelectedIndex == -1)
            {
                MessageWindow msg = new MessageWindow("Выберите клуб");
                msg.ShowDialog();
                return;
            }
            if (thisEmpl.idEmployee == 0)
            {              
                MessageWindow msg = new MessageWindow("Выберите сотрудника");
                msg.ShowDialog();
                return;
            }
          

            if (thisTraining.idTraining == 0)
            {
                MessageWindow msg = new MessageWindow("Выберите тренировку");
                msg.ShowDialog();
                return;
            }



            AppData.Context.TimetableTraining.Add(new TimetableTraining
            {
                idEmployee = thisEmpl.idEmployee,
                idTraining = thisTraining.idTraining,
                DateStart = DateTimeStart,
                DateEnd = DateTimeEnda,
                idClub = cmbClub.SelectedIndex + 1
            });

            AppData.Context.SaveChanges();
            MessageWindow msg228 = new MessageWindow("Запись в расписание добавлена");
            msg228.ShowDialog();
            thisEmpl = null;
            this.Close();

        }

        private void dpDateStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dpDateStart.SelectedDate != null)
            {
                timeEnd.IsEnabled = true;
                dpDateEnd.IsEnabled = true;
                dpDateEnd.DisplayDateStart = dpDateStart.SelectedDate;
            }
            if(dpDateStart.SelectedDate > dpDateEnd.SelectedDate)
            {
                dpDateEnd.SelectedDate = null;
                dpDateEnd.DisplayDateStart = dpDateStart.SelectedDate;
            }
        }

        private void cmbEmpl_SelectionChanged(object sender, RoutedEventArgs e)
        {
           try
            {
                if(cmbEmpl.Text != null)
                {
                    string[] words = cmbEmpl.Text.Split(new char[] { ' ' });
                    string lastName = words[0];
                    string firstName = words[1];
                    string patr = words[2];

                    
                    thisEmpl = AppData.Context.Employee.Where(x => x.LastName == lastName && x.FirstName == firstName && x.Patronymic == patr && x.idRole != 4).FirstOrDefault();
                    
                }
                
            }
            catch (Exception ex)
            {

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

        private void btnAdd_MouseEnter(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void btnAdd_MouseLeave(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderReset(sender);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cmbTraining_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                thisTraining = AppData.Context.Training.Where(x => x.Title == cmbTraining.Text).FirstOrDefault();           
            }
            catch (Exception ex)
            {

            }
        }
    }
}
