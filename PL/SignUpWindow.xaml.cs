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

namespace PL
{
    /// <summary>
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        Random random = new Random();
        BlApi.IBL bl;
        public SignUpWindow(BlApi.IBL myBl)
        {
            InitializeComponent();
            bl = myBl;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int salt = random.Next();


                if (PasswordUser.Password.Any(char.IsDigit) && PasswordUser.Password.Any(char.IsLower) && PasswordUser.Password.Any(char.IsUpper))
                {
                    if (PasswordUser.Password.Length >= 6)
                    {
                        if (PasswordUser.Password == PasswordBoxConfirm.Password)
                        {
                            BO.User Newuser = new BO.User
                            {
                                Id = int.Parse(UserId.Text),
                                UserName = UserName.Text,
                                AllowingAccess = false,
                                DelUser = false,
                                password = PasswordUser.Password,
                                Salt = salt,
                                HashedPassword = Tools.hashPassword(salt + PasswordUser.Password)
                            };
                            bl.AddUser(Newuser);
                            MessageBox.Show("Registration passed successfully", "Registration", MessageBoxButton.OK, MessageBoxImage.Information);
                            new Login1().Show();
                            this.Close();
                            
                        }
                        else
                           MessageBox.Show("wrong Confirm password", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                      
                        

                    }
                    else
                        MessageBox.Show("Your password must Contains  At least 6 characters,For your safety :)", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);

                }
                else
                    MessageBox.Show("Your password must Contains lowercase and uppercase letters and numbers,For your safety :)", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);

            }
            catch (GetDetailsProblemException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            catch (UpdateProblemException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            catch (AddingProblemException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }
    }
}
