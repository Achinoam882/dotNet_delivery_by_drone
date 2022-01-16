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
using BO;
using BlApi; 

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
        BlApi.IBL bl = BlApi.BlFactory.GetBl();
        int id;
        private void signIn_checked(object sender, RoutedEventArgs e)
        {
            
                
        }
        private void PassWord_KeyDown(object sender, KeyEventArgs e)
        {
            PassWordTextBox.Clear();
            PassWordTextBox.Foreground = Brushes.Black;
        }
        /// <summary>
        /// event click 
        /// Long in btn,Checking whether the user exists in another system will be notified to the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
           

            try
            {
                if (bl.GetUser(NameTextBox.Text).HashedPassword != Tools.hashPassword(bl.GetUser(NameTextBox.Text).Salt + PassWordTextBox.Password))
                {
                    throw new ArgumentException();
                }

                else if (bl.GetUser(NameTextBox.Text).AllowingAccess != true)
                {
                    id = bl.GetUser(NameTextBox.Text).Id;
                    Progress.Visibility = Visibility.Visible;
                    await Task.Delay(2500);
                    new ClientWindow(bl, id).Show();
                }
                else if (bl.GetUser(NameTextBox.Text).AllowingAccess == true)
                {
                    Progress.Visibility = Visibility.Visible;
                    await Task.Delay(2500);
                    new MainWindow(bl).Show();
                }


            }           
            catch (ArgumentException )
            {
               MessageBox.Show("Wrong Password,Please Try Again", "ERROR", MessageBoxButton.OKCancel);
            }
            catch (UpdateProblemException ex)
            {
               MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (GetDetailsProblemException ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            this.Close();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonSignup_Click(object sender, RoutedEventArgs e)
        {
            new SignUpWindow(bl).Show();
            this.Close();
        }

     
    }
}
