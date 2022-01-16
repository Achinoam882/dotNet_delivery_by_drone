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
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        ParcelAtCustomer myParcel;
        Customer customer;
        BlApi.IBL bl;
        int id,  parcelIndex;

        public ClientWindow(BlApi.IBL myBl, int myId)
        {
            InitializeComponent();
            bl = myBl;
            
            id = myId;
            customer = bl.GetCustomer(id);
            DataContext = customer;
            CheckBoxPickUpList.ItemsSource = bl.GetParcelList(x => x.Sender == customer.Name && x.Status == ParcelStatus.Scheduled);
            CheckBoxDeliverdList.ItemsSource = bl.GetParcelList(x => x.Sender == customer.Name && x.Status == ParcelStatus.PickUp);
            ParcelFromCustomer.ItemsSource = bl.GetCustomer(customer.Id).ParcelFromCustomer;
            listOfCustomerRecieve.ItemsSource= bl.GetCustomer(customer.Id).ParcelToCustomer;
            CheckBoxPickUpList.DisplayMemberPath = "Id";
            CheckBoxDeliverdList.DisplayMemberPath = "Id";
        }

        private void Star_Click(object sender, RoutedEventArgs e)
        {
            new Star1().Show();

        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {

        
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

        private void info_Click(object sender, RoutedEventArgs e)
        {
            new info().Show();

        }

        private void AddParcel_Click(object sender, RoutedEventArgs e)
        {
            ParcelListWindow parcelList=null;
            new ParcelWindow(bl, customer, parcelList, this).Show();
            ParcelFromCustomer.ItemsSource = bl.GetCustomer(customer.Id).ParcelFromCustomer;
            ParcelFromCustomer.Items.Refresh();



        }

        private void SendToPickUp_Checked(object sender, RoutedEventArgs e)
        {
            Parcel parcel;
            int droneId;
             int id = ((ParcelToList)CheckBoxPickUpList.SelectedItem).Id;
            try
            {
                parcel= bl.GetParcel(id);
                droneId = parcel.DroneAtParcel.Id;
                bl.PickUpParcelByDrone(droneId);
                MessageBoxResult result = MessageBox.Show("Your Parcel was successfully Picked up", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        CheckBoxPickUp.IsChecked = false;
                        CheckBoxPickUpList.ItemsSource =bl .GetParcelList(x => x.Sender == customer.Name && x.Status == ParcelStatus.Scheduled);
                        CheckBoxDeliverdList.ItemsSource = bl.GetParcelList(x => x.Sender == customer.Name && x.Status == ParcelStatus.PickUp);
                        CheckBoxPickUpList.DisplayMemberPath = "Id";
                        customer = bl.GetCustomer(customer.Id);
                         DataContext = customer;
                        ParcelFromCustomer.ItemsSource = bl.GetCustomer(customer.Id).ParcelFromCustomer;
                        
                        //ParcelFromCustomer.Items.Refresh();

                        break;
                    default:
                        break;
                }
            }
             
            catch (UpdateProblemException ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
          
        


            //else
            //{
            //    MessageBox.Show("No Parcel was selected", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
            //     CheckBoxPickUp.IsChecked = false;
            //}

        }

        private void restComboBoxPickup_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxPickUpList.SelectedItem = null;
        }

        private void SendToDeliverd_Checked(object sender, RoutedEventArgs e)
        {
            int id = ((ParcelToList)CheckBoxDeliverdList.SelectedItem).Id;
            try
            {
                bl.DroneDeliverParcel(bl.GetParcel(id).DroneAtParcel.Id);
                MessageBoxResult result = MessageBox.Show("Your Parcel was successfully Delivered", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        SendToDeliverd.IsChecked = false;

                        CheckBoxDeliverdList.ItemsSource = bl.GetParcelList(x => x.Sender == customer.Name && x.Status == ParcelStatus.PickUp);
                        CheckBoxPickUpList.DisplayMemberPath = "Id";
                        customer = bl.GetCustomer(customer.Id);
                       
                       DataContext = customer;
                       ParcelFromCustomer.ItemsSource = bl.GetCustomer(customer.Id).ParcelFromCustomer;
                       // ParcelFromCustomer.Items.Refresh();
                        break;
                    default:
                        break;
                }
            }

            catch (UpdateProblemException ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }
        private void resetComboBoxDelivery_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxDeliverdList.SelectedItem = null;
        }
        private void deleteButtom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (myParcel.Status == ParcelStatus.Requested)
                {
                    MessageBoxResult boxresult = MessageBox.Show("Are you sure you want to delete  this parcel?", "info!", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    switch (boxresult)
                    {
                        case MessageBoxResult.Yes:

                            bl.DeleteParcel(myParcel.Id);
                            ParcelFromCustomer.ItemsSource = bl.GetCustomer(customer.Id).ParcelFromCustomer;
                            MessageBox.Show("Your Parcel was deleted  successfully", "success!");
                            break;
                        case MessageBoxResult.No:
                            break;
                        default:
                            break;
                    }
                }

                else
                {
                    MessageBox.Show("Parcel that has been assigned cant't be deleted!", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (UpdateProblemException ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void ParcelFromCustomer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelAtCustomer parcel = (ParcelAtCustomer)ParcelFromCustomer.SelectedItem;
            parcelIndex = ParcelFromCustomer.SelectedIndex;
            myParcel = parcel;
                   
        }

     
    }
}
