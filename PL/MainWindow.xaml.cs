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
        public MainWindow()
        {
            InitializeComponent();
        }
        BlApi.IBL BlObject = BlApi.BlFactory.GetBl();
        private void ShowDroneListButtom(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(BlObject).Show();
            //this.Close();
            //SystemSounds.Asterisk.Play();
        }

        private void BaseStation_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationListWindow(BlObject).Show();
            //this.Close();
        }

        private void Customer_Click(object sender, RoutedEventArgs e)
        {
            new CustomerListWindow(BlObject).Show();
            //this.Close();
        }

        private void Parcel_Click(object sender, RoutedEventArgs e)
        {
            new ParcelListWindow(BlObject).Show();
            //this.Close();
        }

        private void Star_Click(object sender, RoutedEventArgs e)
        {
            new Star1().Show();
          
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {

            IEnumerable<BO.DroneToList> DroneToList = from item in BlObject.GetDroneList()
                                                      where item.Status == BO.DroneStatuses.InMaintenance
                                                      select item;
            if (DroneToList.Any())
            {
                foreach (var item in DroneToList)//when we close the app we need to put all of the drones that are inmintinan to free
                                                 //becuse we dont have such area to store the in charge drones 
                {
                    BlObject.ReleaseFromCharging(item.Id);
                    
                }
            }
            Application.Current.Shutdown();
        }

  
        

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

        private void  info_Click(object sender, RoutedEventArgs e)
        {
            new info().Show();

        }
    }
}
