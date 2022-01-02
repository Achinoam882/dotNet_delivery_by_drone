using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerListWindow.xaml
    /// </summary>
    public partial class CustomerListWindow : Window
    {
        BlApi.IBL bl;
        //bool close = true;
        public ObservableCollection<BO.CustomerToList> customerToList;

        public CustomerListWindow(BlApi.IBL blObject)
        {
            InitializeComponent();
            bl = blObject;
            customerToList = new ObservableCollection<CustomerToList>();
            List<BO.CustomerToList> customerBL = bl.GetCustomerList().ToList();
            foreach (var x in customerBL)
            {
                customerToList.Add(x);
            }
            //customerToList.CollectionChanged += CustomerToList_CollectionChanged;
            CustomerListView.ItemsSource = customerToList;
            

        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();
        }
        private void CustomerAct_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerToList customer = (CustomerToList)CustomerListView.SelectedItem;
            int customerIndex = CustomerListView.SelectedIndex;
           // this.IsEnabled = false;
            if (customer != null)
                new CustomerWindow(bl, this, customer.Id, customerIndex).Show();
         }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(bl, this).Show();
            //this.IsEnabled = false;
        }
    }
}

