using MagneziaComplex.Classes;
using MagneziaComplex.EF;
using MagneziaComplex.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
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
    /// Логика взаимодействия для TimetablePage.xaml
    /// </summary>
    public partial class TimetablePage : Page
    {
        List<Club> clubList = new List<Club>();
        VisualObjectActions vActions = new VisualObjectActions();
        public TimetablePage()
        {
            InitializeComponent();
            lvTimeTable.ItemsSource = AppData.Context.TimetableTraining.ToList();

            clubList = AppData.Context.Club.ToList();
            clubList.Insert(0, new Club { Title = "Все" });

            cmbClub.ItemsSource = clubList.ToList();
            cmbClub.DisplayMemberPath = "Title";
            cmbClub.SelectedIndex = 0;

        }
        private ListSortDirection _sortDirection;
        private GridViewColumnHeader _sortColumn;
        private void lvTimeTable_Click(object sender, RoutedEventArgs e)
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
            if(cmbClub.SelectedIndex == 0 && dpDate.SelectedDate ==  null)
            {
                lvTimeTable.ItemsSource = AppData.Context.TimetableTraining.ToList();
                return;
            }


            if (dpDate.SelectedDate != null && cmbClub.SelectedIndex != 0)
            {
                lvTimeTable.ItemsSource = AppData.Context.TimetableTraining.Where(x => x.idClub == cmbClub.SelectedIndex && x.DateStart.Year == dpDate.SelectedDate.Value.Year && x.DateStart.Month == dpDate.SelectedDate.Value.Month && x.DateStart.Day == dpDate.SelectedDate.Value.Day).ToList();
                return;
            }
            if (dpDate.SelectedDate != null && cmbClub.SelectedIndex == 0)
            {
                lvTimeTable.ItemsSource = AppData.Context.TimetableTraining.Where(x => x.DateStart.Year == dpDate.SelectedDate.Value.Year && x.DateStart.Month == dpDate.SelectedDate.Value.Month && x.DateStart.Day == dpDate.SelectedDate.Value.Day).ToList();
                return;
            }
            if (dpDate.SelectedDate == null && cmbClub.SelectedIndex != 0)
            {
                lvTimeTable.ItemsSource = AppData.Context.TimetableTraining.Where(x => x.idClub == cmbClub.SelectedIndex).ToList();
            }




            //if(dpDate.DisplayDate != null)
            //{
            //    if(cmbClub.SelectedIndex == 0)
            //    {
            //        lvTimeTable.ItemsSource = AppData.Context.TimetableTraining.Where(x => x.DateStart == dpDate.DisplayDate).ToList();
            //    }    
            //    else
            //    {
            //        lvTimeTable.ItemsSource = AppData.Context.TimetableTraining.Where(x => x.idClub == cmbClub.SelectedIndex && x.DateStart == dpDate.DisplayDate).ToList();
            //    }            
            //}
            //else
            //{
            //    if (cmbClub.SelectedIndex == 0)
            //    {
            //        lvTimeTable.ItemsSource = AppData.Context.TimetableTraining.ToList();
            //    }
            //    else
            //    {
            //        lvTimeTable.ItemsSource = AppData.Context.TimetableTraining.Where(x => x.idClub == cmbClub.SelectedIndex).ToList();
            //    }
            //}
        }

        private void dpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbClub.SelectedIndex != 0 && dpDate.SelectedDate == null)
            {
                lvTimeTable.ItemsSource = AppData.Context.TimetableTraining.Where(x => x.idClub == cmbClub.SelectedIndex).ToList();

            }
            if (cmbClub.SelectedIndex == 0 && dpDate.SelectedDate == null)
            {
                lvTimeTable.ItemsSource = AppData.Context.TimetableTraining.ToList();
            }
            if (dpDate.SelectedDate != null && cmbClub.SelectedIndex == 0)
            {
                lvTimeTable.ItemsSource = AppData.Context.TimetableTraining.Where(x => x.DateStart.Year == dpDate.SelectedDate.Value.Year && x.DateStart.Month == dpDate.SelectedDate.Value.Month && x.DateStart.Day == dpDate.SelectedDate.Value.Day).ToList();
            }
            if (dpDate.SelectedDate != null && cmbClub.SelectedIndex != 0)
            {
                lvTimeTable.ItemsSource = AppData.Context.TimetableTraining.Where(x => x.idClub == cmbClub.SelectedIndex && x.DateStart.Year == dpDate.SelectedDate.Value.Year && x.DateStart.Month == dpDate.SelectedDate.Value.Month && x.DateStart.Day == dpDate.SelectedDate.Value.Day).ToList();

            }


            //lvTimeTable.ItemsSource = AppData.Context.TimetableTraining.Where(x => x.DateStart.Year == dpDate.SelectedDate.Value.Year && x.DateStart.Month == dpDate.SelectedDate.Value.Month && x.DateStart.Day == dpDate.SelectedDate.Value.Day).ToList();
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
            AppData.isLogout = false;
            if(lvTimeTable.SelectedItem is EF.TimetableTraining)
            {
                LogoutWindow lw = new LogoutWindow("Удалить запись?");
                lw.ShowDialog();
                if(AppData.isLogout == true)
                {
                    var tb = lvTimeTable.SelectedItem as EF.TimetableTraining;
                    AppData.Context.TimetableTraining.Remove(tb);
                    AppData.Context.SaveChanges();
                    AppData.isLogout = false;
                    lvTimeTable.ItemsSource = AppData.Context.TimetableTraining.ToList();
                    cmbClub.SelectedIndex = 0;
                    dpDate.SelectedDate = null;

                }           
            }
            else
            {
                MessageWindow msg = new MessageWindow("Выберите запись");
                msg.ShowDialog();
                return;
            }
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Windows.AddTimetableWindow aw = new Windows.AddTimetableWindow();
            aw.ShowDialog();
            lvTimeTable.ItemsSource = AppData.Context.TimetableTraining.ToList();
            cmbClub.SelectedIndex = 0;
            dpDate.SelectedDate = null;
        }
    }
}
