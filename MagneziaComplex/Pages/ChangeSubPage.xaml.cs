using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using MagneziaComplex.Classes;
using MagneziaComplex.EF;

namespace MagneziaComplex.Pages
{
    /// <summary>
    /// Логика взаимодействия для ChangeSubPage.xaml
    /// </summary>
    public partial class ChangeSubPage : Page
    {
        VisualObjectActions vActions = new VisualObjectActions();

        public ChangeSubPage()
        {
            InitializeComponent();

            cmbSub.SelectedIndex = 0;
            cmbClub.ItemsSource = AppData.Context.Club.ToList();
            cmbClub.DisplayMemberPath = "Title";
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
            if (cmbClub.SelectedIndex == -1)
            {
                MessageWindow msg = new MessageWindow("Выберите клуб");
                msg.ShowDialog();
                return;
            }

            if (lvClients.SelectedItem != null)
            {
                var subCl = lvClients.SelectedItem as SubscriptionClient;
                var client = AppData.Context.Client.Where(x => x.idClient == subCl.idClient).FirstOrDefault();

                AddClientSubscriptionWindow ase = new AddClientSubscriptionWindow();
                ase.ShowDialog();
                if (AppData.clientNewSub != null)
                {
                    DateTime subDateEnd = DateTime.Now;
                    var clientSub = AppData.clientNewSub;

                    var oldSub = AppData.Context.SubscriptionClient.Where(x => x.idClient == client.idClient && x.idSubscription == subCl.idSubscription).FirstOrDefault();
                    oldSub.IsActive = false;
                    AppData.Context.SaveChanges();

                    var subClient = AppData.Context.SubscriptionClient.Add(new SubscriptionClient
                    {
                        idClient = client.idClient,
                        idSubscription = AppData.clientNewSub.idSubscription,
                        DateStart = DateTime.Now,
                        DateEnd = subDateEnd.AddMonths(clientSub.CountMonth),
                        IsActive = true,
                        idClub = cmbClub.SelectedIndex + 1,
                        TotalPrice = clientSub.Price - (clientSub.Price * clientSub.Discount)
                    });
                    AppData.Context.SaveChanges();
                    MessageWindow msg2 = new MessageWindow("Абонемент обновлён");
                    msg2.ShowDialog();
                    lvClients.ItemsSource = lvClients.ItemsSource = AppData.Context.SubscriptionClient.Where(x => x.IsActive == true).ToList();
                    cmbSub.SelectedIndex = 0;
                    cmbClub.SelectedIndex = 0;
                }
            }
            else
            {
                MessageWindow msg3 = new MessageWindow("Выберите клиента");
                msg3.ShowDialog();
                return;
            }
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
                lvClients.ItemsSource = AppData.Context.SubscriptionClient.Where(x => x.IsActive == true).ToList();
            }
            if (cmbSub.SelectedIndex == 1)
            {

                //var noActive = AppData.Context.SubscriptionClient.Where(x => x.IsActive == false).OrderByDescending(x => x.DateStart).GroupBy(x=> x.DateStart).ToList();

                List<SubscriptionClient> yesActive = AppData.Context.SubscriptionClient.Where(x => x.IsActive == true).ToList();
                List<SubscriptionClient> noActive = AppData.Context.SubscriptionClient.Where(x => x.IsActive == false).ToList();

                foreach (var item in yesActive.ToList())
                {
                    foreach (var item2 in noActive.ToList())
                    {
                        if (item.idClient == item2.idClient)
                        {
                            noActive.Remove(item2);
                        }
                    }

                }
                lvClients.ItemsSource = noActive.ToList();

                //List<SubscriptionClient> noActiveList = new List<SubscriptionClient>();
                //foreach (var item in AppData.Context.SubscriptionClient.ToList())
                //{
                //    var maxDate = AppData.Context.SubscriptionClient.Where(x => x.idSubscriptionClient == item.idSubscriptionClient).OrderByDescending(x => x.DateStart).Take(1).FirstOrDefault();
                //    if (item.IsActive == false && item.DateStart == maxDate.DateStart)
                //    {
                //        if (noActiveList.Count > 0)
                //        {
                //            foreach (var item2 in noActiveList.ToList())
                //            {
                //                if (item2.idClient == item.idClient)
                //                {
                //                    if (item.DateStart > item2.DateStart)
                //                    {
                //                        noActiveList.Add(item);
                //                        noActiveList.Remove(item2);
                //                    }
                //                }
                //                else
                //                {
                //                    noActiveList.Add(item);
                //                }

                //            }
                //        }
                //        else
                //        {
                //            noActiveList.Add(item);
                //        }



                //    }
                //}
                //lvClients.ItemsSource = noActiveList.ToList();
                //lvClients.ItemsSource = AppData.Context.SubscriptionClient.Where(x => x.IsActive == false).OrderByDescending(x=>x.DateStart).Take(1).ToList();
            }
            if (cmbSub.SelectedIndex == 2)
            {
                lvClients.ItemsSource = AppData.Context.SubscriptionClient.ToList();
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

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(tbSearch.Text == "")
            {
                if (cmbSub.SelectedIndex == 0)
                {
                    lvClients.ItemsSource = AppData.Context.SubscriptionClient.Where(x => x.IsActive == true).ToList();
                }
                if (cmbSub.SelectedIndex == 1)
                {

                    //var noActive = AppData.Context.SubscriptionClient.Where(x => x.IsActive == false).OrderByDescending(x => x.DateStart).GroupBy(x=> x.DateStart).ToList();

                    List<SubscriptionClient> yesActive = AppData.Context.SubscriptionClient.Where(x => x.IsActive == true).ToList();
                    List<SubscriptionClient> noActive = AppData.Context.SubscriptionClient.Where(x => x.IsActive == false).ToList();

                    foreach (var item in yesActive.ToList())
                    {
                        foreach (var item2 in noActive.ToList())
                        {
                            if (item.idClient == item2.idClient)
                            {
                                noActive.Remove(item2);
                            }
                        }

                    }
                    lvClients.ItemsSource = noActive.ToList();

                 
                }
                if (cmbSub.SelectedIndex == 2)
                {
                    lvClients.ItemsSource = AppData.Context.SubscriptionClient.ToList();
                }
            }

            if (tbSearch.Text != string.Empty && tbSearch.Text != "Поиск")
            {
                if(cmbSub.SelectedIndex == 0)
                {
                   lvClients.ItemsSource = AppData.Context.SubscriptionClient.Where(x => (x.Client.FirstName.Contains(tbSearch.Text) || x.Client.LastName.Contains(tbSearch.Text) || x.Client.Patronymic.Contains(tbSearch.Text)) && x.IsActive == true).ToList();
                }
                if(cmbSub.SelectedIndex == 1)
                {
                    List<SubscriptionClient> yesActive = AppData.Context.SubscriptionClient.Where(x => x.IsActive == true).ToList();
                    List<SubscriptionClient> noActive = AppData.Context.SubscriptionClient.Where(x => x.IsActive == false).ToList();

                    foreach (var item in yesActive.ToList())
                    {
                        foreach (var item2 in noActive.ToList())
                        {
                            if (item.idClient == item2.idClient)
                            {
                                noActive.Remove(item2);
                            }
                        }

                    }
                    lvClients.ItemsSource = noActive.Where(x => x.Client.FirstName.Contains(tbSearch.Text) || x.Client.LastName.Contains(tbSearch.Text) || x.Client.Patronymic.Contains(tbSearch.Text)).ToList();
                }
                if (cmbSub.SelectedIndex == 2)
                {
                    lvClients.ItemsSource = AppData.Context.SubscriptionClient.Where(x => x.Client.FirstName.Contains(tbSearch.Text) || x.Client.LastName.Contains(tbSearch.Text) || x.Client.Patronymic.Contains(tbSearch.Text)).ToList();
                }
            }
        }
    }
}
