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
    }
}
