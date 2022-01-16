using BO;
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
using System.Media;
using System.ComponentModel;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        DroneWindow MyDroneWindow;
        Parcel parcel;
        int index;
        BlApi.IBL bl;
        private ParcelListWindow parcelListWindow;
        #region constractor
        // <summary>
        /// Constractor to add parcel window.
        /// </summary>
        public ParcelWindow(BlApi.IBL blObject, ParcelListWindow parcelListWindoww)
        {
            InitializeComponent();
            AddParcelGrid.Visibility = Visibility.Visible;
            bl = blObject;
            parcelListWindow = parcelListWindoww;
            WeightComboBox.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            PriortryComboBox.ItemsSource = Enum.GetValues(typeof(Priorities));
        }
        #endregion constractor
        #region add clicks
        // <summary>
        /// Add a new parcel to the list 
        /// </summary>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (SenderNameTextBox.Text.Length != 0 && TargetNameTextBox.Text.Length != 0 && WeightComboBox.SelectedItem != null && PriortryComboBox.SelectedItem != null)
            {
                CustomerParcel newSenderParcel = new CustomerParcel { Id = int.Parse(SenderIdTextBox.Text), Name = SenderNameTextBox.Text };
                CustomerParcel newTargetParcel = new CustomerParcel { Id = int.Parse(TargetIdTextBox.Text), Name = TargetNameTextBox.Text };


                Parcel newParcel = new Parcel
                {
                    Sender = newSenderParcel,
                    Receiving = newTargetParcel,
                    Weight = (WeightCategories)WeightComboBox.SelectedItem,
                    Priority = (Priorities)PriortryComboBox.SelectedItem,

                };

                try
                {
                    bl.SetParcel(newParcel);
                    
               
                    if (parcelListWindow != null)
                    {
                        ParcelToList parcelList = bl.GetParcelList().ToList().Find(i => i.Id == newParcel.Id);
                        parcelListWindow.parcelToList.Add(parcelList);
                        parcelListWindow.ParcelListView.ItemsSource = bl.GetParcelList();

                        //parcelListWindow.ParcelListView.Items.Refresh(); 

                    }
                    MessageBox.Show("Your Parcel was added successfully", "success!");
                    if (clientWindow!=null)
                    {
                        clientWindow.ParcelFromCustomer.ItemsSource= bl.GetCustomer(customer.Id).ParcelFromCustomer;
                    }
                    Close();





                }
                catch (AddingProblemException ex)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    //if (IdTextBox.Focus())
                    //{
                    //    IdTextBox.BorderBrush = Brushes.Red; //bonus 
                    //}
                }
            }
            else
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Please make sure all fields are filled", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        // <summary>
        /// Cancel addition parcel
        /// </summary>
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult boxresult = MessageBox.Show("Are you sure you want to cancel this addition?", "info!", MessageBoxButton.YesNo, MessageBoxImage.Information);
            switch (boxresult)
            {

                case MessageBoxResult.Yes:
                   // close = false;
                    Close();
                    parcelListWindow.IsEnabled = true;
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }
        #endregion add clicks
        #region constractor for update
        // <summary>
        /// Constractor for update parcel
        /// </summary>
        public ParcelWindow(BlApi.IBL blObject, ParcelListWindow MyparcelListWindow, int id, int myIndex)
        {
            InitializeComponent();
            UpdateGrid.Visibility = Visibility.Visible;
            bl = blObject;
            parcelListWindow = MyparcelListWindow;
            index = myIndex;
            parcel = bl.GetParcel(id);
            DataContext = parcel;
            if (parcel.DroneAtParcel==null)
            {
                DroneDetails.Visibility = Visibility.Hidden;
            }

        }
        #endregion constractor for update

        #region constractor fromm drone window
        // <summary>
        /// Constractor for update parcel from drone window.
        /// </summary>
        public ParcelWindow(BlApi.IBL blObject, DroneWindow droneWindow,  int id)
        {
            InitializeComponent();
            UpdateGrid.Visibility = Visibility.Visible;
            bl = blObject;
            MyDroneWindow = droneWindow;
            parcel = bl.GetParcel(id);
            DataContext = parcel;

        }
        #endregion constractor form drone window

        CustomerWindow ListOfParcel;
        #region constractor from customer window
        // <summary>
        /// constractor to update parcel from customer window
        /// </summary>
        public ParcelWindow(BlApi.IBL blObject, CustomerWindow myList ,int id,int Index )
        {
            InitializeComponent();
            UpdateGrid.Visibility = Visibility.Visible;
            bl = blObject;
            parcel = bl.GetParcel(id);
            index = Index;
            DataContext = parcel;
            ListOfParcel = myList;
        }
        #endregion constractor form customer window

        #region delete click
        // <summary>
        /// Delete an exists  parcel
        /// </summary>
        private void deleteParcel_Click(object sender, RoutedEventArgs e)
        {
            if(parcel.Scheduled!=null)
            {
                //deleteParcel.IsEnabled = false;
                MessageBox.Show("Parcel that has been assigned cant't be deleted!", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {

                MessageBoxResult boxresult = MessageBox.Show("Are you sure you want to delete  this parcel?", "info!", MessageBoxButton.YesNo, MessageBoxImage.Information);
                switch (boxresult)
                {

                    case MessageBoxResult.Yes:

                        bl.DeleteParcel(parcel.Id);
                        if (parcelListWindow != null)
                        {
                            parcelListWindow.parcelToList.RemoveAt(index);
                            parcelListWindow.IsEnabled = true;
                        }
                        if(ListOfParcel!=null)
                        {
                          
                             ListOfParcel.ParcelFromCustomer.ItemsSource = bl.GetCustomer(parcel.Sender.Id).ParcelFromCustomer; 
                             
                            ListOfParcel.listOfCustomerRecieve.ItemsSource = bl.GetCustomer(parcel.Receiving.Id).ParcelToCustomer;
                        }
                        deleteParcel.IsEnabled = false;
                        MessageBox.Show("Your Parcel was deleted  successfully", "success!");
                        Close();
                       
                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        break;
                }


            }
        }
        #endregion delete click
        #region sender target details
        // <summary>
        ///Open  customer sender details
        /// </summary>
        private void SenderDetails_Click(object sender, MouseButtonEventArgs e)
        {
          Customer customer=  bl.GetCustomer(parcel.Sender.Id);
           new CustomerWindow(bl,  customer.Id, index).ShowDialog(); 
           
        }
        // <summary>
        ///Open  customer target details
        /// </summary>
        private void ReceiverDetails_Click(object sender, MouseButtonEventArgs e)
        {
            Customer customer = bl.GetCustomer(parcel.Receiving.Id);
            new CustomerWindow(bl, customer.Id, index).ShowDialog();
        }
        #endregion sender target details
        #region drone assig to parcel
        // <summary>
        ///Open  drone window that is assigned to the parcel
        /// </summary>
        private void DroneParcel_Click(object sender, MouseButtonEventArgs e)
        {
            if(parcel.Scheduled!=null)
            {
                
                    Drone drone = bl.GetDrone(parcel.DroneAtParcel.Id);
                    new DroneWindow(bl, drone.Id, index).ShowDialog();
                

            }
        }
        #endregion drone assig to parcel
        #region close clicks
        // <summary>
        ///shuting down the program
        /// </summary>
        private void Close_Click(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();
        }
        // <summary>
        ///going back
        /// </summary>
        private void BackWindow_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        // <summary>
        ///going back to home screen
        /// </summary>
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            //close = false;
            if (parcelListWindow != null)
            {
                parcelListWindow.Close();
            }

            Close();

        }
        // <summary>
        ///going back
        /// </summary>
        private void back_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion close clicks
        ClientWindow clientWindow;
        Customer customer;
        #region constractor for client window
        // <summary>
        ///Open  parcel window from client window
        /// </summary>
        public ParcelWindow(BlApi.IBL blObject,Customer mycustomer,ParcelListWindow myparcel, ClientWindow myclientWindow)
        {
            InitializeComponent();
            AddParcelGrid.Visibility = Visibility.Visible;
            bl = blObject;
            clientWindow = myclientWindow;
            customer = mycustomer;
            parcelListWindow = myparcel;
            //DataContext = customer;
            WeightComboBox.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            PriortryComboBox.ItemsSource = Enum.GetValues(typeof(Priorities));
        }
        #endregion constractor for client window
      
    }
}

