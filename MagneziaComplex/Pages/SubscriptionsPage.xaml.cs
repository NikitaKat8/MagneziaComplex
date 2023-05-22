using MagneziaComplex.Classes;
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
using MagneziaComplex;
namespace MagneziaComplex.Pages
{
    /// <summary>
    /// Логика взаимодействия для SubscriptionsPage.xaml
    /// </summary>
    public partial class SubscriptionsPage : Page
    {
        VisualObjectActions vActions = new VisualObjectActions();
        public SubscriptionsPage()
        {
            InitializeComponent();
            var subs = AppData.Context.Subscription.ToList();
            lvSubs.ItemsSource = subs;
        }
        EF.Subscription currentSub;
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if(currentSub == null)
            {
                MessageWindow msg = new MessageWindow("Выберите абонемент");
                msg.ShowDialog();
                return;
            }
            else
            {
                EditSubscriptionWindow esw = new EditSubscriptionWindow(currentSub, true);
                esw.ShowDialog();
                lvSubs.ItemsSource = AppData.Context.Subscription.ToList();

                currentSub = null;
            }
        }

        private void lvSubs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvSubs.SelectedItem is EF.Subscription)
            {
                var sub = lvSubs.SelectedItem as EF.Subscription;
                currentSub = sub;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            EditSubscriptionWindow esw = new EditSubscriptionWindow(null, false);
            esw.ShowDialog();
            lvSubs.ItemsSource = AppData.Context.Subscription.ToList();
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

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderReset(sender);
        }

        private void btnEditDiscount_Click(object sender, RoutedEventArgs e)
        {
            if (currentSub == null)
            {
                MessageWindow msg = new MessageWindow("Выберите абонемент");
                msg.ShowDialog();
                return;
            }
            else
            {
                EditDiscountWindow esw = new EditDiscountWindow(currentSub);
                esw.ShowDialog();
                lvSubs.ItemsSource = AppData.Context.Subscription.ToList();
            }
        }
    }
}
