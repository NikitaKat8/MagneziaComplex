using MagneziaComplex.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using MagneziaComplex.EF;
using System.Xaml;

namespace MagneziaComplex.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        List<Client> clientList = new List<Client>();
        List<Client> previewClientList = new List<Client>();
        VisualObjectActions vActions = new VisualObjectActions();
        public ClientPage()
        {
            InitializeComponent();
            clientList = AppData.Context.Client.ToList();
            lvClients.ItemsSource = clientList;
            cmbSub.SelectedIndex = 0;

        }

        private ListSortDirection _sortDirection;
        private GridViewColumnHeader _sortColumn;
        private void lvClients_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = e.OriginalSource as GridViewColumnHeader;
            if (column == null || column.Column == null)
            {
                return;
            }

            if (_sortColumn == column)
            {
                // Toggle sorting direction 
                _sortDirection = _sortDirection == ListSortDirection.Ascending ?
                                                   ListSortDirection.Descending :
                                                   ListSortDirection.Ascending;
            }
            else
            {
                // Remove arrow from previously sorted header 
                if (_sortColumn != null && _sortColumn.Column != null)
                {
                    _sortColumn.Column.HeaderTemplate = null;
                    _sortColumn.Column.Width = _sortColumn.ActualWidth - 20;
                }

                _sortColumn = column;
                _sortDirection = ListSortDirection.Ascending;
                column.Column.Width = column.ActualWidth + 20;
            }

            if (_sortDirection == ListSortDirection.Ascending)
            {
                column.Column.HeaderTemplate = Resources["ArrowUp"] as DataTemplate;
            }
            else
            {
                column.Column.HeaderTemplate = Resources["ArrowDown"] as DataTemplate;
            }

            string header = string.Empty;

            // if binding is used and property name doesn't match header content 
            Binding b = _sortColumn.Column.DisplayMemberBinding as Binding;
            if (b != null)
            {
                header = b.Path.Path;
            }

            ICollectionView resultDataView = CollectionViewSource.GetDefaultView((sender as ListView).ItemsSource);
            resultDataView.SortDescriptions.Clear();
            resultDataView.SortDescriptions.Add(new SortDescription(header, _sortDirection));
        }

        private void cmbSub_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSub.SelectedIndex == 0)
            {
                lvClients.ItemsSource = AppData.Context.Client.ToList();
            }
            if (cmbSub.SelectedIndex == 1)
            {
                lvClients.ItemsSource = AppData.Context.Client.OrderByDescending(x => x.RegistrationDate).ToList();
            }
            if (cmbSub.SelectedIndex == 2)
            {
                lvClients.ItemsSource = AppData.Context.Client.OrderBy(x => x.RegistrationDate).ToList();
            }
            if (cmbSub.SelectedIndex == 3)
            {
                lvClients.ItemsSource = AppData.Context.Client.Where(x => x.SubscriptionClient.Select(z => z.DateStart.Month == DateTime.Now.Month).FirstOrDefault()).ToList();
            }
            if (cmbSub.SelectedIndex == 4)
            {
                lvClients.ItemsSource = AppData.Context.Client.Where(x => x.SubscriptionClient.Select(z => z.DateEnd.Month == DateTime.Now.Month).FirstOrDefault()).ToList();
            }
            if (cmbSub.SelectedIndex == 5)
            {
                lvClients.ItemsSource = AppData.Context.Client.Where(x => x.SubscriptionClient.Select(z => z.DateEnd.Month < DateTime.Now.Month).FirstOrDefault()).ToList();
            }
        }

        private void btnEdit_MouseEnter(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void btnEdit_MouseLeave(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderReset(sender);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lvClients.SelectedItem is EF.Client)
            {
                var client = lvClients.SelectedItem as EF.Client;
                FrameNav.current_frame.Navigate(new Pages.AddClientPage(true, client));
            }
            else
            {
                MessageWindow msg = new MessageWindow("Выберите клиента");
                msg.ShowDialog();
                return;
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(tbSearch.Text == "")
            {
                if (cmbSub.SelectedIndex == 0)
                {
                    lvClients.ItemsSource = AppData.Context.Client.ToList();
                }
                if (cmbSub.SelectedIndex == 1)
                {
                    lvClients.ItemsSource = AppData.Context.Client.OrderByDescending(x => x.RegistrationDate).ToList();
                }
                if (cmbSub.SelectedIndex == 2)
                {
                    lvClients.ItemsSource = AppData.Context.Client.OrderBy(x => x.RegistrationDate).ToList();
                }
                if (cmbSub.SelectedIndex == 3)
                {
                    lvClients.ItemsSource = AppData.Context.Client.Where(x => x.SubscriptionClient.Select(z => z.DateStart.Month == DateTime.Now.Month).FirstOrDefault()).ToList();
                }
                if (cmbSub.SelectedIndex == 4)
                {
                    lvClients.ItemsSource = AppData.Context.Client.Where(x => x.SubscriptionClient.Select(z => z.DateEnd.Month == DateTime.Now.Month).FirstOrDefault()).ToList();
                }
                if (cmbSub.SelectedIndex == 5)
                {
                    lvClients.ItemsSource = AppData.Context.Client.Where(x => x.SubscriptionClient.Select(z => z.DateEnd.Month < DateTime.Now.Month).FirstOrDefault()).ToList();
                }
            }

            if (tbSearch.Text != string.Empty && tbSearch.Text != "Поиск")
            {
                if (cmbSub.SelectedIndex == 0)
                {
                    lvClients.ItemsSource = AppData.Context.Client.Where(x => x.FirstName.Contains(tbSearch.Text) || x.LastName.Contains(tbSearch.Text) || x.Patronymic.Contains(tbSearch.Text)).ToList();
                }
                if (cmbSub.SelectedIndex == 1)
                {
                    lvClients.ItemsSource = AppData.Context.Client.Where(x => x.FirstName.Contains(tbSearch.Text) || x.LastName.Contains(tbSearch.Text) || x.Patronymic.Contains(tbSearch.Text)).OrderByDescending(x => x.RegistrationDate).ToList();
                }
                if (cmbSub.SelectedIndex == 2)
                {
                    lvClients.ItemsSource = AppData.Context.Client.Where(x => x.FirstName.Contains(tbSearch.Text) || x.LastName.Contains(tbSearch.Text) || x.Patronymic.Contains(tbSearch.Text)).OrderBy(x => x.RegistrationDate).ToList();
                }
                if (cmbSub.SelectedIndex == 3)
                {
                    lvClients.ItemsSource = AppData.Context.Client.Where(x => x.SubscriptionClient.Select(z => z.DateStart.Month == DateTime.Now.Month).FirstOrDefault() && (x.FirstName.Contains(tbSearch.Text) || x.LastName.Contains(tbSearch.Text) || x.Patronymic.Contains(tbSearch.Text))).ToList();
                }
                if (cmbSub.SelectedIndex == 4)
                {
                    lvClients.ItemsSource = AppData.Context.Client.Where(x => x.SubscriptionClient.Select(z => z.DateEnd.Month == DateTime.Now.Month).FirstOrDefault() && (x.FirstName.Contains(tbSearch.Text) || x.LastName.Contains(tbSearch.Text) || x.Patronymic.Contains(tbSearch.Text))).ToList();
                }
                if (cmbSub.SelectedIndex == 5)
                {
                    lvClients.ItemsSource = AppData.Context.Client.Where(x => x.SubscriptionClient.Select(z => z.DateEnd.Month < DateTime.Now.Month).FirstOrDefault() && (x.FirstName.Contains(tbSearch.Text) || x.LastName.Contains(tbSearch.Text) || x.Patronymic.Contains(tbSearch.Text))).ToList();
                }

            }


           
        }

        private void tbSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            tbSearch.Text = string.Empty;
        }

        private void tbSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbSearch.Text == string.Empty)
            {
                tbSearch.Text = "";
                tbSearch.Text = "Поиск";
            }
        }
    }
}
