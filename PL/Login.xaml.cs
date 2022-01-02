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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for Login1.xaml
    /// </summary>
    public partial class Login1 : Window
    {
        public Login1()
        {
            InitializeComponent();
        }

        

        private void signIn_checked(object sender, RoutedEventArgs e)
        {
//ButtonSignin.Visibility = Visibility.Visible;
        }

        

        //private void Name_KeyDown(object sender, KeyEventArgs e)
        //{
        //    NameTextBox.Clear();
        //}
        private void PassWord_KeyDown(object sender, KeyEventArgs e)
        {
            PassWordTextBox.Clear();
            PassWordTextBox.Foreground = Brushes.Black;
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {

            Close();
        }
    }
}
