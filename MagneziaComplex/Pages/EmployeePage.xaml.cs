using MagneziaComplex.Classes;
using MagneziaComplex.EF;
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

namespace MagneziaComplex.Pages
{
    /// <summary>
    /// Логика взаимодействия для EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage : Page
    {
        List<Club> clubList = new List<Club>();
        List<Role> roleList = new List<Role>();
        ListFilter ListFilter= new ListFilter();
        VisualObjectActions vActions = new VisualObjectActions();
        public EmployeePage()
        {
            InitializeComponent();
            lvEmpl.ItemsSource = AppData.Context.Employee.ToList();

            clubList = AppData.Context.Club.ToList();
            clubList.Insert(0, new Club { Title = "Все" });

            roleList = AppData.Context.Role.ToList();
            roleList.Insert(0, new Role { Name = "Все" });

            cmbClub.ItemsSource = clubList.ToList();
            cmbClub.DisplayMemberPath = "Title";
            cmbClub.SelectedIndex = 0;

            cmbRole.ItemsSource = roleList.ToList();
            cmbRole.DisplayMemberPath = "Name";
            cmbRole.SelectedIndex = 0;
        }
        private ListSortDirection _sortDirection;
        private GridViewColumnHeader _sortColumn;
        private void lvEmpl_Click(object sender, RoutedEventArgs e)
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

        private void cmbClub_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if(cmbClub.SelectedIndex == 0)
            //{
            //    lvEmpl.ItemsSource = AppData.Context.Employee.ToList();
            //}
            //else
            //{
            //    var selectedClub = cmbClub.SelectedItem as Club;
            //    lvEmpl.ItemsSource = AppData.Context.Employee.Where(x => x.idClub == selectedClub.idClub).ToList();
            //}
            lvEmpl.ItemsSource = ListFilter.myEmployeeFilter(cmbClub.SelectedIndex, cmbRole.SelectedIndex, lvEmpl);
        }

        private void cmbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbRole.SelectedIndex == 0)
            //{
            //    lvEmpl.ItemsSource = AppData.Context.Employee.ToList();
            //}
            //else
            //{
            //    var selectedRole = cmbRole.SelectedItem as Role;
            //    lvEmpl.ItemsSource = AppData.Context.Employee.Where(x => x.idRole == selectedRole.idRole).ToList();
            //}
            lvEmpl.ItemsSource = ListFilter.myEmployeeFilter(cmbClub.SelectedIndex, cmbRole.SelectedIndex, lvEmpl);
        }

        private void btnEdit_MouseEnter(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void btnEdit_MouseLeave(object sender, MouseEventArgs e)
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

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if(lvEmpl.SelectedItem != null)
            {
                if (lvEmpl.SelectedItem is EF.Employee)
                {
                    var empl = lvEmpl.SelectedItem as EF.Employee;
                    FrameNav.current_frame.Navigate(new Pages.AddEmployeePage(true, empl));
                }
            }
            else
            {
                MessageWindow msg2 = new MessageWindow("Выберите сотрудника");
                msg2.ShowDialog();
                return;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            FrameNav.current_frame.Navigate(new Pages.AddEmployeePage(false, null));
        }

        private void tbSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbSearch.Text == string.Empty)
            {
                tbSearch.Text = "";
                tbSearch.Text = "Поиск";
            }
          
        }

        private void tbSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            tbSearch.Text = string.Empty;
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(tbSearch.Text != "" && tbSearch.Text != "Поиск")
            {
                var res = ListFilter.myEmployeeFilter(cmbClub.SelectedIndex, cmbRole.SelectedIndex, lvEmpl);
                lvEmpl.ItemsSource = res.Where(x => x.FirstName.Contains(tbSearch.Text) || x.LastName.Contains(tbSearch.Text) || x.Patronymic.Contains(tbSearch.Text)).ToList();
            }
            if(tbSearch.Text == "")
            {
                lvEmpl.ItemsSource = ListFilter.myEmployeeFilter(cmbClub.SelectedIndex, cmbRole.SelectedIndex, lvEmpl);
            }
        }
    }
}
