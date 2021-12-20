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

namespace PL
{
    /// <summary>
    /// Interaction logic for BaseStationWindow.xaml
    /// </summary>
    ///  BlApi.IBL bl;
   

    public partial class BaseStationWindow : Window
    {
        BlApi.IBL bl;
        public bool close = true;
        private BaseStationListWindow basestationList;
        public BaseStationWindow(BlApi.IBL blObject, BaseStationListWindow baseStationListWindow)
        {
            InitializeComponent();
            AddBaseStationGrid.Visibility = Visibility.Visible;
            bl = blObject;
            basestationList = baseStationListWindow;
           
        }

        private void cancel_click(object sender, RoutedEventArgs e)
        {
           MessageBoxResult boxresult = MessageBox.Show("Are you sure you want to cancel this addition?", "info!", MessageBoxButton.YesNo, MessageBoxImage.Information);
            switch (boxresult)
            {

                case MessageBoxResult.Yes:


                    close = false;
                    Close();
                 //  BaseStationListWindow.IsEnabled = true;

                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            //if (IdTextBox.Text.Length != 0 && NameTextBox.Text.Length != 0 && latitudeTextBox.Text.Length != 0 && longitudeTextBox.Text.Length != 0 && chargeslotstextbox.Text.Length != 0 )

            //{ }
        }
    }
}
