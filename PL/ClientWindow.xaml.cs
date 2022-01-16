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
        #region constarctor
        /// <summary>
        ///constractor for th eclient window
        /// </summary>
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
        #endregion constarctor

        #region rate us
        /// <summary>
        ///rate us window
        /// </summary>
        private void Star_Click(object sender, RoutedEventArgs e)
        {
            new Star1().Show();

        }
        #endregion constarctor

        #region close click
        /// <summary>
        ///close window
        /// </summary>
        private void Close_Click(object sender, RoutedEventArgs e)
        {

        
            Application.Current.Shutdown();
        }



        /// <summary>
        ///to log out from the account
        /// </summary
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
        #endregion close click

        #region info click
        /// <summary>
        ///information about us window
        /// </summary>
        private void info_Click(object sender, RoutedEventArgs e)
        {
            new info().Show();

        }
        #endregion info click

        #region add parcel
        /// <summary>
        ///add parcel window window
        /// </summary>
        private void AddParcel_Click(object sender, RoutedEventArgs e)
        {
            ParcelListWindow parcelList=null;
            new ParcelWindow(bl, customer, parcelList, this).Show();
            ParcelFromCustomer.ItemsSource = bl.GetCustomer(customer.Id).ParcelFromCustomer;
            ParcelFromCustomer.Items.Refresh();



        }
        #endregion add parcel

        #region pickup parcel
        /// <summary>
        ///to pick up a parcel from the client  window
        /// </summary>
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

        }
        #endregion pickup parcel

        #region reset click

        /// <summary>
        ///reset click for combo box
        /// </summary>
        private void restComboBoxPickup_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxPickUpList.SelectedItem = null;
        }
        /// <summary>
        ///reset click for combo box
        /// </summary
        private void resetComboBoxDelivery_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxDeliverdList.SelectedItem = null;
        }
        #endregion endreset click

        #region deliver parcel
        /// <summary>
        ///to deliver a parcel from the client  window
        /// </summary>
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
        #endregion deliver parcel

         #region delete parcel
        /// <summary>
        ///delet click parcel
        /// </summary>
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
#endregion delete parcel

        #region parcel info
        /// <summary>
        ///info about parcel from client window
        /// </summary>
        private void ParcelFromCustomer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelAtCustomer parcel = (ParcelAtCustomer)ParcelFromCustomer.SelectedItem;
            parcelIndex = ParcelFromCustomer.SelectedIndex;
            myParcel = parcel;
                   
        }
        #endregion parcel info



    }
}
