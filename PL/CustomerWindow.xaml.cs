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
        BlApi.IBL bl;
        public bool close = true;
        private CustomerListWindow customerListWindow;
        public CustomerWindow(BlApi.IBL blObject, CustomerListWindow customerList)
        {
            InitializeComponent();
            AddCustomerGrid.Visibility = Visibility.Visible;
            bl = blObject;
            customerListWindow = customerList;
        }
        private void BackWindow_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

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
                    close = false;
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
                //רענון
            }
            else
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Please make sure all fields are filled", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void cancel_click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult boxresult = MessageBox.Show("Are you sure you want to cancel this addition?", "info!", MessageBoxButton.YesNo, MessageBoxImage.Information);
            switch (boxresult)
            {
                case MessageBoxResult.Yes:
                    close = false;
                    Close();
                    customerListWindow.IsEnabled = true;
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }
        Customer customer;
        int index;
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
        

            private void UpdateCustomer_Click(object sender, RoutedEventArgs e)
            {
            bl.UpdateCustomer(customer.Id, NameTextBox1.Text, PhoneTextBox.Text);
            MessageBox.Show("The Customer Was Updated successfully", "success!");
            customer = bl.GetCustomer(customer.Id);
            DataContext = customer;
            int index = customerListWindow.CustomerListView.SelectedIndex;
            CustomerToList customerToList = customerListWindow.customerToList[index];
            customerToList.Name = NameTextBox1.Text;
            customerToList.PhoneNumber = PhoneTextBox.Text;
            customerListWindow.customerToList[index] = customerToList;
            NameTextBox1.IsReadOnly = true;
            PhoneTextBox.IsReadOnly = true;
            customerListWindow.CustomerListView.Items.Refresh();
            }

        //private void ParcelFromCustomer_Click(object sender, MouseButtonEventArgs e)
        //{
        //    Parcel parcel = bl.GetParcel(customer.ParcelFromCustomer[index].Id);
        //    new ParcelWindow(bl, this, parcel.Id, index);
        //}

        //private void ParcelToCustomer_Click(object sender, MouseButtonEventArgs e)
        //{
        //    Parcel parcel = bl.GetParcel(customer.ParcelToCustomer[index].Id);
        //    new ParcelWindow(bl, this, parcel.Id, index);
        //}
        public CustomerWindow(BlApi.IBL blObject, int id, int indexId)
        {
            InitializeComponent();
            UpdateGrid.Visibility = Visibility.Visible;
            this.NameTextBox1.IsReadOnly = true;
            this.PhoneTextBox.IsReadOnly = true;
            this.UpdateButton.IsEnabled = false;
            bl = blObject;
            //customerListWindow = customerList;
            index = indexId;
            customer = bl.GetCustomer(id);
            DataContext = customer;
        }
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

        private void ListParcelToCustomer_Click(object sender, RoutedEventArgs e)
        {
            ListOfParcelsCustomer listOfParcelsCustomer = new ListOfParcelsCustomer(bl, this, index, customer.Id);
            this.Content = listOfParcelsCustomer;
        }

        private void ListParcelFromCustomer_Click(object sender, RoutedEventArgs e)
        {
            ListOfParcelsCustomer listOfParcelsCustomer= new ListOfParcelsCustomer(bl, this, index, customer.Id);
            this.Content = listOfParcelsCustomer;
        }
    }
    
}


