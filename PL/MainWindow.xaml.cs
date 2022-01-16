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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Media;
using BlApi;


namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BlApi.IBL BlObject;
        #region constractor
        public MainWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            BlObject = bl;
        }
        #endregion constractor
    
        #region Drone list
        private void ShowDroneListButtom(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(BlObject).ShowDialog();
            //this.Close();
            //SystemSounds.Asterisk.Play();
        }
        #endregion Drone list
        #region base station list
        private void BaseStation_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationListWindow(BlObject).ShowDialog();
            //this.Close();
        }
        #endregion base station list
        #region Customer list
        private void Customer_Click(object sender, RoutedEventArgs e)
        {
            new CustomerListWindow(BlObject).ShowDialog();
            //this.Close();
        }
        #endregion Customer list
        #region parcel list
        private void Parcel_Click(object sender, RoutedEventArgs e)
        {
            new ParcelListWindow(BlObject).ShowDialog();
            //this.Close();
        }
        #endregion parcel list
        #region rate us
        private void Star_Click(object sender, RoutedEventArgs e)
        {
            new Star1().ShowDialog();
          
        }
        #endregion rate us
        #region close
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion close


        #region logout click
        private void LogoutButtem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult boxresult = MessageBox.Show("Are you sure you want to log out?", "info!", MessageBoxButton.YesNo, MessageBoxImage.Information);
            switch (boxresult)
            {

                case MessageBoxResult.Yes:                  
                    new Login1().Show();
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }
        #endregion logout click
        #region info click
        private void  info_Click(object sender, RoutedEventArgs e)
        {
            new info().ShowDialog();

        }
        #endregion info click
    }




}
