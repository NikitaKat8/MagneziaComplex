using MagneziaComplex.Classes;
using MagneziaComplex.EF;
using MagneziaComplex.Windows;
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
    /// Логика взаимодействия для TrainingsPage.xaml
    /// </summary>
    public partial class TrainingsPage : Page
    {
        Training currentTrainig = null;
        VisualObjectActions vActions = new VisualObjectActions();
        public TrainingsPage()
        {
            InitializeComponent();
            rbActual.IsChecked = true;
            lvTrainings.ItemsSource = AppData.Context.Training.Where(x=>x.isActive == true).ToList();
        }

        private void rbActual_Checked(object sender, RoutedEventArgs e)
        {
            rbNotActual.IsChecked = false;
            lvTrainings.ItemsSource = AppData.Context.Training.Where(x => x.isActive == true).ToList();
        }

        private void rbNotActual_Checked(object sender, RoutedEventArgs e)
        {
            rbActual.IsChecked = false;
            lvTrainings.ItemsSource = AppData.Context.Training.Where(x => x.isActive == false).ToList();
        }

        private void btnEditActual_Click(object sender, RoutedEventArgs e)
        {
            if(currentTrainig != null)
            {
                if(currentTrainig.isActive == true)
                {
                    currentTrainig.isActive = false;
                    AppData.Context.SaveChanges();
                    lvTrainings.ItemsSource = AppData.Context.Training.Where(x => x.isActive == true).ToList();
                    currentTrainig = null;

                    MessageWindow msg = new MessageWindow("Актуальность изменена");
                    msg.ShowDialog();
                }
                else
                {
                    currentTrainig.isActive = true;
                    AppData.Context.SaveChanges();
                    lvTrainings.ItemsSource = AppData.Context.Training.Where(x => x.isActive == false).ToList();
                    currentTrainig = null;

                    MessageWindow msg = new MessageWindow("Актуальность изменена");
                    msg.ShowDialog();
                }
            }
            else
            {
                MessageWindow msg = new MessageWindow("Выберите тренировку");
                msg.ShowDialog();
                return;
            }
        }

        private void lvTrainings_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvTrainings.SelectedItem is EF.Training)
            {
                var tr = lvTrainings.SelectedItem as EF.Training;
                currentTrainig = tr;
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (currentTrainig == null)
            {
                MessageWindow msg = new MessageWindow("Выберите тренировку");
                msg.ShowDialog();
                return;
            }
            else
            {
                EditTrainingWindow esw = new EditTrainingWindow(currentTrainig, true);
                esw.ShowDialog();

                if(rbActual.IsChecked == true)
                {
                    lvTrainings.ItemsSource = AppData.Context.Training.Where(x=> x.isActive == true).ToList();
                }
                else
                {
                    lvTrainings.ItemsSource = AppData.Context.Training.Where(x => x.isActive == false).ToList();
                }
              

                currentTrainig = null;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
           
                EditTrainingWindow esw = new EditTrainingWindow(null, false);
                esw.ShowDialog();

                if (rbActual.IsChecked == true)
                {
                    lvTrainings.ItemsSource = AppData.Context.Training.Where(x => x.isActive == true).ToList();
                }
                else
                {
                    lvTrainings.ItemsSource = AppData.Context.Training.Where(x => x.isActive == false).ToList();
                }


                currentTrainig = null;
           
        }

        private void btnAdd_MouseEnter(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void btnAdd_MouseLeave(object sender, MouseEventArgs e)
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

        private void btnEditActual_MouseEnter(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void btnEditActual_MouseLeave(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderReset(sender);
        }
    }
}
