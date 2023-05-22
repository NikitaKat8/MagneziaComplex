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
using MagneziaComplex.Classes;
using MagneziaComplex.Pages;

namespace MagneziaComplex.Pages
{
    /// <summary>
    /// Логика взаимодействия для ActionPage.xaml
    /// </summary>
    public partial class ActionPage : Page
    {
        VisualObjectActions vActions = new VisualObjectActions();
        public ActionPage()
        {
            InitializeComponent();
            
        }
        #region MouseEvents

       
        private void btnReg_MouseEnter(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void btnReg_MouseLeave(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderReset(sender);
        }

        private void btnChangeSubs_MouseEnter(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void btnChangeSubs_MouseLeave(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderReset(sender);
        }

        private void btnSubs_MouseEnter(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void btnSubs_MouseLeave(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderReset(sender);
        }

        private void btnTraining_MouseEnter(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void btnTraining_MouseLeave(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderReset(sender);
        }

        private void btnGym_MouseEnter(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void btnGym_MouseLeave(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderReset(sender);
        }


        #endregion

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            FrameNav.current_frame.Navigate(new Pages.AddClientPage(false, null));
        }

        private void btnSubs_Click(object sender, RoutedEventArgs e)
        {
            FrameNav.current_frame.Navigate(new Pages.SubscriptionsPage());
        }

        private void btnGym_Click(object sender, RoutedEventArgs e)
        {
            FrameNav.current_frame.Navigate(new Pages.GymPage());
        }

        private void btnChangeSubs_Click(object sender, RoutedEventArgs e)
        {
            FrameNav.current_frame.Navigate(new Pages.ChangeSubPage());
        }

        private void btnTraining_Click(object sender, RoutedEventArgs e)
        {
            FrameNav.current_frame.Navigate(new Pages.TrainingRegistration());
        }
    }
}
