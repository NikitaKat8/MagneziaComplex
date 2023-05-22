using MagneziaComplex.Classes;
using MagneziaComplex.EF;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MagneziaComplex.Pages
{
    /// <summary>
    /// Логика взаимодействия для TrainingRegistration.xaml
    /// </summary>
    public partial class TrainingRegistration : Page
    {

        VisualObjectActions vActions = new VisualObjectActions();

        Employee thisEmpl = new Employee();
        Training thisTraining = new Training();
        Client thisClient = new Client();
        public TrainingRegistration()
        {
            InitializeComponent();
            cmbEmpl.IsEnabled = false;
            cmbTraining.IsEnabled = false;
            txtPrice.Text = "----";

            cmbClub.ItemsSource = AppData.Context.Club.ToList();
            cmbClub.DisplayMemberPath = "Title";

           

            cmbClient.ItemsSource = AppData.Context.Client.ToList();
            cmbClient.DisplayMemberPath = "Fio";

            cmbTraining.ItemsSource = AppData.Context.Training.Where(x=> x.isActive == true).ToList();
            cmbTraining.DisplayMemberPath = "Title";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(thisEmpl == null || thisClient == null || thisTraining == null || cmbClub.SelectedIndex == -1)
            {
                MessageWindow msg = new MessageWindow("Некторые поля не выбраны");
                msg.ShowDialog();
                return;
            }


            var favouriteExist = AppData.Context.ClientFavouriteTrainers.Where(x => x.idClient == thisClient.idClient && x.idEmployee == thisEmpl.idEmployee).FirstOrDefault();

            if(favouriteExist != null)
            {
                favouriteExist.CountTrainings++;
                favouriteExist.TotalPrice += thisTraining.Price;
                favouriteExist.DateSale = DateTime.Now;
                AppData.Context.SaveChanges();
            }
            else
            {
                AppData.Context.ClientFavouriteTrainers.Add(new ClientFavouriteTrainers
                {
                    idClient= thisClient.idClient,
                    idEmployee= thisEmpl.idEmployee,
                    CountTrainings = 1,
                    DateSale = DateTime.Now,
                    TotalPrice= thisTraining.Price
                });
                AppData.Context.SaveChanges();
            }

            AppData.Context.ClientTraining.Add(new ClientTraining
            {
                idClient= thisClient.idClient,
                idTraining= thisTraining.idTraining,
                DateSale= DateTime.Now,
                TotalPrice= thisTraining.Price
            });
            AppData.Context.SaveChanges();

            MessageWindow msg2 = new MessageWindow("Запись на тренировку добавлена");
            msg2.ShowDialog();

            FrameNav.current_frame.Navigate(new Pages.ActionPage());


        }

        private void cmbClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            cmbClient.IsDropDownOpen = true;
        }

        private void cmbClient_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbClient.Text != null)
                {
                    string[] words = cmbClient.Text.Split(new char[] { ' ' });
                    string lastName = words[0];
                    string firstName = words[1];
                    string patr = words[2];


                    thisClient = AppData.Context.Client.Where(x => x.LastName == lastName && x.FirstName == firstName && x.Patronymic == patr).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

            }
        }

       

      
        private void cmbClient_GotFocus(object sender, RoutedEventArgs e)
        {
            cmbClient.IsDropDownOpen = true;
        }

      

       

        private void cmbEmpl_TextChanged(object sender, TextChangedEventArgs e)
        {
            cmbEmpl.IsDropDownOpen = true;
        }

        private void cmbEmpl_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbEmpl.Text != null)
                {
                    string[] words = cmbEmpl.Text.Split(new char[] { ' ' });
                    string lastName = words[0];
                    string firstName = words[1];
                    string patr = words[2];


                    thisEmpl = AppData.Context.Employee.Where(x => x.LastName == lastName && x.FirstName == firstName && x.Patronymic == patr && x.idRole != 4).FirstOrDefault();

                }
                cmbTraining.IsEnabled = true;
            }
            catch (Exception ex)
            {

            }
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

        private void cmbTraining_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if(cmbTraining.SelectedIndex != -1)
            {
                thisTraining = null;
                var train = cmbTraining.SelectedItem as Training;
                thisTraining = AppData.Context.Training.Where(x => x.idTraining == train.idTraining).FirstOrDefault();
                txtPrice.Text = thisTraining.Price.ToString();
               
            }
          
          
        }

     

        private void btnAdd_MouseEnter(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void btnAdd_MouseLeave(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderReset(sender);
        }

        private void cmbClub_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if(cmbClub.SelectedIndex != -1)
            {
                cmbEmpl.IsEnabled= true;
                cmbEmpl.ItemsSource = AppData.Context.Employee.Where(x => x.idRole != 4 && x.idClub == cmbClub.SelectedIndex + 1).ToList();
                cmbEmpl.DisplayMemberPath = "Fio";
            }
        }

        private void cmbTraining_GotFocus(object sender, RoutedEventArgs e)
        {
            cmbTraining.IsDropDownOpen= true;
        }
    }
}
