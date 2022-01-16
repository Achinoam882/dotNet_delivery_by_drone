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

        public ObservableCollection<BO.CustomerToList> customerToList;
        #region constractor
        /// <summary>
        ///constractor for customer list
        /// </summary
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
        #endregion constractor

        #region close clicks
        /// <summary>
        ///click shut down
        /// </summary
        private void Close_Click(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();
        }
        /// <summary>
        /// return home click 
        /// </summary
        private void Home_click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion close clicks

        #region infor of customer

        /// <summary>
        ///info about a customer from the view list
        /// </summary
        private void CustomerAct_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerToList customer = (CustomerToList)CustomerListView.SelectedItem;
            int customerIndex = CustomerListView.SelectedIndex;
           // this.IsEnabled = false;
            if (customer != null)
                new CustomerWindow(bl, this, customer.Id, customerIndex).ShowDialog();
         }
        #endregion infor of customer

        #region add customer
        /// <summary>
        ///add customer 
        /// </summary
        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(bl, this).ShowDialog();
            //this.IsEnabled = false;
        }
        #endregion add customer
        
    }
}

