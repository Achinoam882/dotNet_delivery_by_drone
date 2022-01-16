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
using System.ComponentModel;
using System.Media;
using BO;
namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        Customer customer;
        int index;
        BlApi.IBL bl;
        private CustomerListWindow customerListWindow;
        #region constractor to add
        /// <summary>
        /// constractor to add customer
        /// </summary>
        public CustomerWindow(BlApi.IBL blObject, CustomerListWindow customerList)
        {
            InitializeComponent();
            AddCustomerGrid.Visibility = Visibility.Visible;
            bl = blObject;
            customerListWindow = customerList;
        }
        #endregion constractor to add

        #region close clicks
        /// <summary>
        ///back to previous window.
        /// </summary>
        private void BackWindow_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        ///close all the windows.
        /// </summary>
        private void Close_Click(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();
        }
        /// <summary>
        ///back to home window.
        /// </summary>
        private void HOME_CLICK(object sender, RoutedEventArgs e)
        {
            CustomerListWindow customerlist;
            customerlist = customerListWindow;
            customerListWindow.Close();
            Close();


        }
        /// <summary>
        ///back to previous window.
        /// </summary>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void listOfCustomerRecieve_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        #endregion close clicks

        #region add clicks
        /// <summary>
        ///Add a new customer .
        /// </summary>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (IdTextBox.Text.Length != 0 && NameTextBox.Text.Length != 0 && latitudeTextBox.Text.Length != 0 && longitudeTextBox.Text.Length != 0 && PhoneNumberTextbox.Text.Length != 0)
            {
                Location newLocation = new Location { Latitude = double.Parse(latitudeTextBox.Text), Longitude = double.Parse(longitudeTextBox.Text) };

                Customer newCustomer = new Customer
                {
                    Id = int.Parse(IdTextBox.Text),
                    Name = NameTextBox.Text,
                    PhoneNumber = PhoneNumberTextbox.Text,
                    CustomerLocation = newLocation
                };
                try
                {
                    bl.SetCustomer(newCustomer);
                    MessageBox.Show("Your customer was added successfully", "success!");
                    CustomerToList customerList = bl.GetCustomerList().ToList().Find(i => i.Id == newCustomer.Id);
                    customerListWindow.customerToList.Add(customerList);
                    Close();
                    customerListWindow.IsEnabled = true;
                }
                catch (AddingProblemException ex)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    if (IdTextBox.Focus())
                    {
                        IdTextBox.BorderBrush = Brushes.Red; //bonus 
                    }

                }
            }
            else
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Please make sure all fields are filled", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        ///cancel addition customer.
        /// </summary>
        private void cancel_click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult boxresult = MessageBox.Show("Are you sure you want to cancel this addition?", "info!", MessageBoxButton.YesNo, MessageBoxImage.Information);
            switch (boxresult)
            {
                case MessageBoxResult.Yes:
                    Close();
                    customerListWindow.IsEnabled = true;
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }
        #endregion add clicks

        #region constarctor info window
        /// <summary>
        ///constractor to update customer.
        /// </summary>
        public CustomerWindow(BlApi.IBL blObject, CustomerListWindow customerList, int id, int indexId)
        {
            InitializeComponent();
            UpdateGrid.Visibility = Visibility.Visible;
            bl = blObject;
            customerListWindow = customerList;
            index = indexId;
            customer = bl.GetCustomer(id);
            DataContext = customer;
         
        }
        #endregion constarctor info window

        #region update click
        /// <summary>
        ///update the name and phone number customer.
        /// </summary>
        private void UpdateCustomer_Click(object sender, RoutedEventArgs e)
            {
            bl.UpdateCustomer(customer.Id, NameTextBox1.Text, PhoneTextBox.Text);
            MessageBox.Show("The Customer Was Updated successfully", "success!");
            customer = bl.GetCustomer(customer.Id);
            DataContext = customer;
            if(customerListWindow!=null)
            {
                int index = customerListWindow.CustomerListView.SelectedIndex;
                CustomerToList customerToList = customerListWindow.customerToList[index];
                customerToList.Name = NameTextBox1.Text;
                customerToList.PhoneNumber = PhoneTextBox.Text;
                customerListWindow.customerToList[index] = customerToList;
                customerListWindow.CustomerListView.Items.Refresh();

            }

            NameTextBox1.IsReadOnly = true;
            PhoneTextBox.IsReadOnly = true;
            }
        #endregion update click

        #region click info parcel
        /// <summary>
        ///open a new parcel window that the customer sent
        /// </summary>
        private void ParcelFromCustomer_Click(object sender, MouseButtonEventArgs e)
        {
            ParcelAtCustomer parcelAtCustomer = (ParcelAtCustomer)ParcelFromCustomer.SelectedItem;
            int droneIndex = ParcelFromCustomer.SelectedIndex;
            if (parcelAtCustomer != null)
                new ParcelWindow(bl, this, parcelAtCustomer.Id, index).Show();
        }

        /// <summary>
        ///open a new parcel window that the customer receive
        /// </summary>
        private void ParcelToCustomer_Click(object sender, MouseButtonEventArgs e)
        {
            ParcelAtCustomer parcelAtCustomer = (ParcelAtCustomer)listOfCustomerRecieve.SelectedItem;
            int droneIndex = listOfCustomerRecieve.SelectedIndex;
            if (parcelAtCustomer != null)
                new ParcelWindow(bl,this, parcelAtCustomer.Id, index).Show();
        }
        #endregion click info parcel

        #region constractor for update
        /// <summary>
        ///constractor to update customer from parcel window
        /// </summary>
        public CustomerWindow(BlApi.IBL blObject, int id, int indexId)
        {
            InitializeComponent();
            UpdateGrid.Visibility = Visibility.Visible;
            bl = blObject;
            index = indexId;
            customer = bl.GetCustomer(id);
            DataContext = customer;
            ParcelFromCustomer.ItemsSource = bl.GetCustomer(customer.Id).ParcelFromCustomer;
            listOfCustomerRecieve.ItemsSource = bl.GetCustomer(customer.Id).ParcelToCustomer;
        }
        #endregion constractor for update

        #region checks up for adding customer
        /// <summary>
        ///integrity check to add customer
        /// </summary>
        private void TextBoxId_KeyDown(object sender, KeyEventArgs e)
        {
           // IdTextBox.BorderBrush = Brushes.Gray;
            
            if (e.Key < Key.D0 || e.Key > Key.D9)
            {
                if (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) 
                {
                    e.Handled = true;
                }
                else
                {
                    e.Handled = false;
                }
            }
            if (IdTextBox.Text.Length > 8)
            {
                e.Handled = true;
            }
        }
        /// <summary>
        ///integrity check to add customer
        /// </summary>
        private void TextBoxPN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key < Key.D0 || e.Key > Key.D9)
            {
                if (e.Key < Key.NumPad0 || e.Key > Key.NumPad9)
                {
                    e.Handled = true;
                }
                else
                {
                    e.Handled = false;
                }
            }
            if (IdTextBox.Text.Length > 9)
            {
                e.Handled = true;
            }
        }
        #endregion checks up for adding customer


        #region list of parcel to and from customer
        /// <summary>
        ///Show list of parcel that sent
        /// </summary>
        private void ListParcelToCustomer_Click(object sender, RoutedEventArgs e)
        {
            ToCustomer.Visibility = Visibility.Visible;
        }
        /// <summary>
        ///Show list of parcel that receive
        /// </summary>
        private void ListParcelFromCustomer_Click(object sender, RoutedEventArgs e)
        {
            FromCustomer.Visibility = Visibility.Visible;

        }
        #endregion list of parcel to and from customer
       
    }
    
}


