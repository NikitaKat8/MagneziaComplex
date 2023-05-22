using MagneziaComplex.Classes;
using MagneziaComplex.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MagneziaComplex.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEmployeePage.xaml
    /// </summary>
    /// \
    /// 

  
    public partial class AddEmployeePage : Page
    {
        Employee editEmpl = null;
        bool editMode = false;
        VisualObjectActions vActions = new VisualObjectActions();
        public AddEmployeePage(bool isEdit, Employee employee)
        {
            InitializeComponent();
            editMode = isEdit;
      
            dpBirthDay.DisplayDateEnd = DateTime.Now;
            cmbGender.ItemsSource = AppData.Context.Gender.ToList();
            cmbGender.DisplayMemberPath = "Name";
            cmbClub.ItemsSource = AppData.Context.Club.ToList();
            cmbClub.DisplayMemberPath = "Title";
            cmbRole.ItemsSource = AppData.Context.Role.ToList();
            cmbRole.DisplayMemberPath = "Name";

            tbPass.IsEnabled = false;
            tbLogin.IsEnabled = false;

            if(isEdit)
            {
                editEmpl = employee;
                if(editEmpl.idRole == 4)
                {
                    tbPass.IsEnabled = true;
                    tbLogin.IsEnabled = true;

                    tbLogin.Text = editEmpl.Login;
                    tbPass.Text = editEmpl.Password;

                    tbFirstName.Text = editEmpl.FirstName;
                    tbLastName.Text = editEmpl.LastName;
                    tbPatronymic.Text = editEmpl.Patronymic;
                    tbPhone.Text = editEmpl.PhoneNumber;
                    tbPassSerial.Text = editEmpl.PassSerial;
                    tbPassNum.Text = editEmpl.PassNumber;
                    dpBirthDay.SelectedDate = editEmpl.Birthday;
                    cmbGender.SelectedIndex = editEmpl.idGender - 1;
                    cmbClub.SelectedIndex = editEmpl.idClub - 1;
                    //cmbClub.SelectedIndex = AppData.Context.SubscriptionClient.Where(x=> x.idClient == editClient.idClient).OrderByDescending(v=> v.idSubscriptionClient).Select(z=> z.idClub).FirstOrDefault() - 1;
                    cmbRole.SelectedIndex = editEmpl.idRole - 1;
                }
                else
                {
                    tbFirstName.Text = editEmpl.FirstName;
                    tbLastName.Text = editEmpl.LastName;
                    tbPatronymic.Text = editEmpl.Patronymic;
                    tbPhone.Text = editEmpl.PhoneNumber;
                    tbPassSerial.Text = editEmpl.PassSerial;
                    tbPassNum.Text = editEmpl.PassNumber;
                    dpBirthDay.SelectedDate = editEmpl.Birthday;
                    cmbGender.SelectedIndex = editEmpl.idGender - 1;
                    cmbClub.SelectedIndex = editEmpl.idClub - 1;
                    //cmbClub.SelectedIndex = AppData.Context.SubscriptionClient.Where(x=> x.idClient == editClient.idClient).OrderByDescending(v=> v.idSubscriptionClient).Select(z=> z.idClub).FirstOrDefault() - 1;
                    cmbRole.SelectedIndex = editEmpl.idRole - 1;
                }

                btnAdd.Content = "Изменить";
            }

        }

        private void btnAdd_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void btnAdd_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            vActions.ButtonBorderReset(sender);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(cmbRole.SelectedIndex == 3)
            {
                if (string.IsNullOrWhiteSpace(tbFirstName.Text) || string.IsNullOrWhiteSpace(tbLastName.Text) || string.IsNullOrWhiteSpace(tbPhone.Text)
               || string.IsNullOrWhiteSpace(tbLogin.Text) || string.IsNullOrWhiteSpace(tbPassSerial.Text) || string.IsNullOrWhiteSpace(tbPassNum.Text) || string.IsNullOrWhiteSpace(tbPass.Text))
                {
                    MessageWindow msg = new MessageWindow("Некоторые поля не заполнены");
                    msg.ShowDialog();
                    return;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(tbFirstName.Text) || string.IsNullOrWhiteSpace(tbLastName.Text) || string.IsNullOrWhiteSpace(tbPhone.Text)
              || string.IsNullOrWhiteSpace(tbPassSerial.Text) || string.IsNullOrWhiteSpace(tbPassNum.Text))
                {
                    MessageWindow msg = new MessageWindow("Некоторые поля не заполнены");
                    msg.ShowDialog();
                    return;
                }
            }

           
            if (string.IsNullOrWhiteSpace(tbPatronymic.Text))
            {
                tbPatronymic.Text = null;
            }
            if (tbPhone.Text.Length < 11 || tbPassSerial.Text.Length < 4 || tbPassNum.Text.Length < 6)
            {
                MessageWindow msg = new MessageWindow("Некоторые поля c фиксированной длинной не заполнены до конца");
                msg.ShowDialog();
                return;
            }
            if (cmbGender.SelectedIndex == -1)
            {
                MessageWindow msg = new MessageWindow("Выберите пол");
                msg.ShowDialog();
                return;
            }
            if (dpBirthDay.SelectedDate == null)
            {
                MessageWindow msg = new MessageWindow("Выберите дату рождения");
                msg.ShowDialog();
                return;
            }
            if (cmbClub.SelectedIndex == -1)
            {
                MessageWindow msg = new MessageWindow("Выберите клуб");
                msg.ShowDialog();
                return;
            }
           
            if(editMode == false)
            {
                var pass_exist_check = AppData.Context.Employee.Where(x => x.PassSerial == tbPassSerial.Text && x.PassNumber == tbPassNum.Text).ToList().FirstOrDefault();
                if (pass_exist_check != null)
                {
                    MessageWindow msg = new MessageWindow("Такой пользователь уже есть");
                    msg.ShowDialog();
                    return;
                }
            }
            

            if(editMode)
            {
                if(editEmpl.idRole == 4)
                {
                    editEmpl.FirstName = tbFirstName.Text;
                    editEmpl.LastName = tbLastName.Text;
                    editEmpl.Patronymic = tbPatronymic.Text;
                    editEmpl.PassSerial = tbPassSerial.Text;
                    editEmpl.PassNumber = tbPassNum.Text;
                    editEmpl.Birthday = dpBirthDay.DisplayDate;
                    editEmpl.PhoneNumber = tbPhone.Text;
                    editEmpl.idGender = cmbGender.SelectedIndex + 1;
                    editEmpl.idRole = cmbRole.SelectedIndex + 1;
                    editEmpl.idClub = cmbClub.SelectedIndex + 1;
                    editEmpl.Password = tbPass.Text;
                    editEmpl.Login = tbLogin.Text;
                }
                else
                {
                    editEmpl.FirstName = tbFirstName.Text;
                    editEmpl.LastName = tbLastName.Text;
                    editEmpl.Patronymic = tbPatronymic.Text;
                    editEmpl.PassSerial = tbPassSerial.Text;
                    editEmpl.PassNumber = tbPassNum.Text;
                    editEmpl.Birthday = dpBirthDay.DisplayDate;
                    editEmpl.PhoneNumber = tbPhone.Text;
                    editEmpl.idGender = cmbGender.SelectedIndex + 1;
                    editEmpl.idRole = cmbRole.SelectedIndex + 1;
                    editEmpl.idClub = cmbClub.SelectedIndex + 1;
                }
                AppData.Context.SaveChanges();

                MessageWindow msg = new MessageWindow("Сотрудник изменён");
                msg.ShowDialog();
                FrameNav.current_frame.Navigate(new Pages.EmployeePage());
                
            }
            else
            {
                if (cmbRole.SelectedIndex == 3)
                {
                    AppData.Context.Employee.Add(new EF.Employee
                    {
                        FirstName = tbFirstName.Text,
                        LastName = tbLastName.Text,
                        Patronymic = tbPatronymic.Text,
                        PhoneNumber = tbPhone.Text,
                        idRole = cmbRole.SelectedIndex + 1,
                        idClub = cmbClub.SelectedIndex + 1,
                        PassSerial = tbPassSerial.Text,
                        PassNumber = tbPassNum.Text,
                        Birthday = dpBirthDay.DisplayDate,
                        Login = tbLogin.Text,
                        Password = tbPass.Text,
                        idGender = cmbGender.SelectedIndex + 1,

                    });

                    AppData.Context.SaveChanges();
                }
                else
                {
                    AppData.Context.Employee.Add(new EF.Employee
                    {
                        FirstName = tbFirstName.Text,
                        LastName = tbLastName.Text,
                        Patronymic = tbPatronymic.Text,
                        PhoneNumber = tbPhone.Text,
                        idRole = cmbRole.SelectedIndex + 1,
                        idClub = cmbClub.SelectedIndex + 1,
                        PassSerial = tbPassSerial.Text,
                        PassNumber = tbPassNum.Text,
                        Birthday = dpBirthDay.DisplayDate,

                        idGender = cmbGender.SelectedIndex + 1,

                    });

                    AppData.Context.SaveChanges();
                }
                MessageWindow cmp = new MessageWindow("Сотрудник добавлен!");
                cmp.ShowDialog();
            }
                         
            FrameNav.current_frame.Navigate(new Pages.EmployeePage());



        }

        #region PreviewTextInput

        

        private void tbLastName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Handled != "йцукенгшщзхъфывапролджэячсмитьбюЙЦУК-ЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ".IndexOf(e.Text) < 0)
            {
                e.Handled = true;
            }
        }

        private void tbFirstName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Handled != "йцукенгшщзхъфывапролджэячсмитьбюЙЦУК-ЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ".IndexOf(e.Text) < 0)
            {
                e.Handled = true;
            }
        }

        private void tbPatronymic_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Handled != "йцукенгшщзхъфывапролджэячсмитьбюЙЦУК-ЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ".IndexOf(e.Text) < 0)
            {
                e.Handled = true;
            }
        }

        private void tbPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Handled != "1234567890".IndexOf(e.Text) < 0)
            {
                e.Handled = true;
            }
        }

        private void tbPassSerial_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Handled != "1234567890".IndexOf(e.Text) < 0)
            {
                e.Handled = true;
            }
        }

        private void tbPassNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Handled != "1234567890".IndexOf(e.Text) < 0)
            {
                e.Handled = true;
            }
        }

        private void tbLogin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Handled != "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890".IndexOf(e.Text) < 0)
            {
                e.Handled = true;
            }
        }
        #endregion

        private void cmbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbRole.SelectedIndex == 3)
            {
                tbLogin.IsEnabled = true;
                tbPass.IsEnabled = true;
            }
            else
            {
                tbLogin.IsEnabled = false;
                tbPass.IsEnabled = false;
                tbLogin.Text = "";
                tbPass.Text = "";
            }
        }
    }
}
