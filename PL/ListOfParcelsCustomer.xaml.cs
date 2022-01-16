//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
//using BO;

//namespace PL
//{
//    /// <summary>
//    /// Interaction logic for ListOfParcelsCustomer.xaml
//    /// </summary>
//    public partial class ListOfParcelsCustomer : Page
//    {
//        BlApi.IBL bl;
//        CustomerWindow customerWindow;
//        int index;
//        int myId;
//        Customer customer;
//        public ListOfParcelsCustomer(BlApi.IBL blObject,CustomerWindow mycustomerWindow,int myindex,int Id)
//        {
//            InitializeComponent();
//            myId = Id;
//            bl = blObject;
//            customerWindow = mycustomerWindow;
//            index = myindex;
//            customer = bl.GetCustomer(Id);
//            DataContext = customer;

//        }
//        private void ParcelFromCustomer_Click(object sender, MouseButtonEventArgs e)
//        {
//            Parcel parcel = bl.GetParcel(customer.ParcelFromCustomer[index].Id);
//            new ParcelWindow(bl, this, parcel.Id, index);
            
//        }

//        private void ParcelToCustomer_Click(object sender, MouseButtonEventArgs e)
//        {
//            Parcel parcel = bl.GetParcel(customer.ParcelToCustomer[index].Id);
//            new ParcelWindow(bl, this, parcel.Id, index);
//        }

       
//    }
//}
