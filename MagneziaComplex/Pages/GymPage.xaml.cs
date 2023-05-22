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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MagneziaComplex.Pages
{
    /// <summary>
    /// Логика взаимодействия для GymPage.xaml
    /// </summary>
    public partial class GymPage : Page
    {
        public GymPage()
        {
            InitializeComponent();
            lvGym.ItemsSource = AppData.Context.Club.ToList();
           
        }

      

    }
}
