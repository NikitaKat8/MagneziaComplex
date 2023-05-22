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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MagneziaComplex.Classes;
using MagneziaComplex.EF;
using MagneziaComplex.Pages;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace MagneziaComplex.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddClientPage.xaml
    /// </summary>
    public partial class AddClientPage : Page
    {
        VisualObjectActions vActions = new VisualObjectActions();
        bool editMode = false;
        Client editClient;
        public AddClientPage(bool isEdit, Client client)
        {
            InitializeComponent();

            editMode = isEdit;
            editClient = client;

            dpBirthDay.DisplayDateEnd = DateTime.Now;
            cmbGender.ItemsSource = AppData.Context.Gender.ToList();
            cmbGender.DisplayMemberPath = "Name";
            cmbClub.ItemsSource = AppData.Context.Club.ToList();
            cmbClub.DisplayMemberPath = "Title";

            if (isEdit)
            {
                tbFirstName.Text = editClient.FirstName;
                tbLastName.Text = editClient.LastName;
                tbPatronymic.Text = editClient.Patronymic;
                tbPhone.Text = editClient.PhoneNumber;
                tbEmail.Text = editClient.Email;
                tbPassSerial.Text = editClient.PassSerial;
                tbPassNum.Text = editClient.PassNumber;
                tbGrowth.Text = editClient.Height.ToString();
                tbWeight.Text = editClient.Weight.ToString();
                dpBirthDay.SelectedDate = editClient.Birthday;
                cmbGender.SelectedIndex = editClient.idGender - 1;
                //cmbClub.SelectedIndex = AppData.Context.SubscriptionClient.Where(x=> x.idClient == editClient.idClient).OrderByDescending(v=> v.idSubscriptionClient).Select(z=> z.idClub).FirstOrDefault() - 1;
                cmbClub.IsEnabled = false;
                cmbClub.Opacity = 0;
                txtClub.Opacity = 0;

                btnReg.IsEnabled = false;
               
            }
            else
            {
                btnEdit.IsEnabled = false;
               
            }
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(tbFirstName.Text) || string.IsNullOrWhiteSpace(tbLastName.Text) || string.IsNullOrWhiteSpace(tbPhone.Text)
                || string.IsNullOrWhiteSpace(tbEmail.Text) || string.IsNullOrWhiteSpace(tbPassSerial.Text) || string.IsNullOrWhiteSpace(tbPassNum.Text) || string.IsNullOrWhiteSpace(tbGrowth.Text) || string.IsNullOrWhiteSpace(tbWeight.Text))
            {
                MessageWindow msg = new MessageWindow("Некоторые поля не заполнены");
                msg.ShowDialog();
                return;
            }
            if(string.IsNullOrWhiteSpace(tbPatronymic.Text))
            {
                tbPatronymic.Text = null;
            }
            if(tbPhone.Text.Length < 11 || tbPassSerial.Text.Length < 4 || tbPassNum.Text.Length < 6 || tbGrowth.Text.Length < 3 || tbWeight.Text.Length < 2)
            {
                MessageWindow msg = new MessageWindow("Некоторые поля c фиксированной длинной не заполнены до конца");
                msg.ShowDialog();
                return;
            }
            if(cmbGender.SelectedIndex == -1)
            {
                MessageWindow msg = new MessageWindow("Выберите пол");
                msg.ShowDialog();
                return;
            }
            if(dpBirthDay.SelectedDate == null)
            {
                MessageWindow msg = new MessageWindow("Выберите дату рождения");
                msg.ShowDialog();
                return;
            }
            if(cmbClub.SelectedIndex == -1)
            {
                MessageWindow msg = new MessageWindow("Выберите клуб");
                msg.ShowDialog();
                return;
            }
            string email = tbEmail.Text;
            string emailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            bool emailCheck = Regex.IsMatch(email, emailPattern);

            if (emailCheck == false)
            {
                MessageWindow msg = new MessageWindow("Неверный формат Email. \n Пример: user@mail.ru");
                msg.ShowDialog();
                return;
            }
            var email_exist_check = AppData.Context.Client.Where(x => x.Email == email).ToList().FirstOrDefault();
            var pass_exist_check = AppData.Context.Client.Where(x => x.PassSerial == tbPassSerial.Text && x.PassNumber == tbPassNum.Text).ToList().FirstOrDefault();
            if (email_exist_check != null || pass_exist_check != null)
            {
                MessageWindow msg = new MessageWindow("Такой пользователь уже есть");
                msg.ShowDialog();
                return;
            }



            AddClientSubscriptionWindow aw = new AddClientSubscriptionWindow();
            aw.ShowDialog();

            if(AppData.clientNewSub == null)
            {
                MessageWindow msg = new MessageWindow("Абонемент не выбран");
                msg.ShowDialog();
                return;
            }
            else
            {
               var newCLient = AppData.Context.Client.Add(new Client 
                {
                    FirstName = tbFirstName.Text,
                    LastName = tbLastName.Text,
                    Patronymic = tbPatronymic.Text,
                    PhoneNumber = tbPhone.Text,
                    Height = Convert.ToInt32(tbGrowth.Text),
                    Weight = Convert.ToInt32(tbWeight.Text),
                    PassSerial = tbPassSerial.Text,
                    PassNumber = tbPassNum.Text,
                    Birthday = dpBirthDay.DisplayDate,
                    Email = tbEmail.Text,
                    idGender = cmbGender.SelectedIndex + 1,
                    RegistrationDate = DateTime.Now
                });

                DateTime subDateEnd = DateTime.Now;
                var clientSub = AppData.clientNewSub;
                

                AppData.Context.SubscriptionClient.Add(new SubscriptionClient
                {
                    idClient = newCLient.idClient,
                    idSubscription = clientSub.idSubscription,
                    DateStart = DateTime.Now,
                    DateEnd = subDateEnd.AddMonths(clientSub.CountMonth),
                    IsActive = true,
                    idClub = cmbClub.SelectedIndex + 1,
                    TotalPrice = clientSub.Price - (clientSub.Price * clientSub.Discount)
                });

                AppData.Context.SaveChanges();
                MessageWindow cmp = new MessageWindow("Клиент добавлен!");
                cmp.ShowDialog();
                AppData.clientNewSub = null;

                FrameNav.current_frame.Navigate(new Pages.ActionPage());
            }


        }

        private void btnReg_MouseEnter(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderRecolor(sender);
        }

        private void btnReg_MouseLeave(object sender, MouseEventArgs e)
        {
            vActions.ButtonBorderReset(sender);
        }

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

        private void tbEmail_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Handled != "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890-_@.".IndexOf(e.Text) < 0)
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

        private void tbGrowth_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Handled != "1234567890".IndexOf(e.Text) < 0)
            {
                e.Handled = true;
            }
        }

        private void tbWeight_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Handled != "1234567890".IndexOf(e.Text) < 0)
            {
                e.Handled = true;
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
            if (string.IsNullOrWhiteSpace(tbFirstName.Text) || string.IsNullOrWhiteSpace(tbLastName.Text) || string.IsNullOrWhiteSpace(tbPhone.Text)
               || string.IsNullOrWhiteSpace(tbEmail.Text) || string.IsNullOrWhiteSpace(tbPassSerial.Text) || string.IsNullOrWhiteSpace(tbPassNum.Text) || string.IsNullOrWhiteSpace(tbGrowth.Text) || string.IsNullOrWhiteSpace(tbWeight.Text))
            {
                MessageWindow msg = new MessageWindow("Некоторые поля не заполнены");
                msg.ShowDialog();
                return;
            }
            if (string.IsNullOrWhiteSpace(tbPatronymic.Text))
            {
                tbPatronymic.Text = null;
            }
            if (tbPhone.Text.Length < 11 || tbPassSerial.Text.Length < 4 || tbPassNum.Text.Length < 6 || tbGrowth.Text.Length < 3 || tbWeight.Text.Length < 2)
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
            //if (cmbClub.SelectedIndex == -1)
            //{
            //    MessageWindow msg = new MessageWindow("Выберите клуб");
            //    msg.ShowDialog();
            //    return;
            //}
            string email = tbEmail.Text;
            string emailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            bool emailCheck = Regex.IsMatch(email, emailPattern);

            if (emailCheck == false)
            {
                MessageWindow msg = new MessageWindow("Неверный формат Email. \n Пример: user@mail.ru");
                msg.ShowDialog();
                return;
            }
            var email_exist_check = AppData.Context.Client.Where(x => x.Email == email && x.idClient != editClient.idClient).ToList().FirstOrDefault();
            var pass_exist_check = AppData.Context.Client.Where(x => x.PassSerial == tbPassSerial.Text && x.PassNumber == tbPassNum.Text && x.idClient != editClient.idClient).ToList().FirstOrDefault();
            if (email_exist_check != null || pass_exist_check != null)
            {
                MessageWindow msg = new MessageWindow("Такой пользователь уже есть");
                msg.ShowDialog();
                return;
            }

            editClient.FirstName = tbFirstName.Text;
            editClient.LastName = tbLastName.Text;
            editClient.Patronymic = tbPatronymic.Text;
            editClient.PhoneNumber = tbPhone.Text;
            editClient.Height = Convert.ToInt32(tbGrowth.Text);
            editClient.Weight = Convert.ToInt32(tbWeight.Text);
            editClient.PassSerial = tbPassSerial.Text;
            editClient.PassNumber = tbPassNum.Text;
            editClient.Birthday = dpBirthDay.DisplayDate;
            editClient.Email = tbEmail.Text;
            editClient.idGender = cmbGender.SelectedIndex + 1;
            
            //SubscriptionClient subClub = AppData.Context.SubscriptionClient.Where(x=> x.idClient == editClient.idClient).OrderByDescending(c=> c.idSubscriptionClient).FirstOrDefault();
            //subClub.idClub = cmbClub.SelectedIndex + 1;

            AppData.Context.SaveChanges();

            MessageWindow msg2 = new MessageWindow("Клиент успешно изменён");
            msg2.ShowDialog();

            FrameNav.current_frame.Navigate(new Pages.ClientPage());
        }
    }
}
