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
        BlApi.IBL bl;
        public bool close = true;
        private ParcelListWindow parcelListWindow;
        public ParcelWindow(BlApi.IBL blObject, ParcelListWindow parcelListWindoww)
        {
            InitializeComponent();
            AddParcelGrid.Visibility = Visibility.Visible;
            bl = blObject;
            parcelListWindow = parcelListWindoww;
            WeightComboBox.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            PriortryComboBox.ItemsSource = Enum.GetValues(typeof(Priorities));
        }

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
                    MessageBox.Show("Your Parcel was added successfully", "success!");
                    ParcelToList parcelList = bl.GetParcelList().ToList().Find(i => i.Id == newParcel.Id);
                    parcelListWindow.parcelToList.Add(parcelList);
                    close = false;
                    Close();
                    parcelListWindow.IsEnabled = true;


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


        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult boxresult = MessageBox.Show("Are you sure you want to cancel this addition?", "info!", MessageBoxButton.YesNo, MessageBoxImage.Information);
            switch (boxresult)
            {

                case MessageBoxResult.Yes:
                    close = false;
                    Close();
                    parcelListWindow.IsEnabled = true;
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }
        Parcel parcel;
        int index;
        public ParcelWindow(BlApi.IBL blObject, ParcelListWindow MyparcelListWindow, int id, int myIndex)
        {
            InitializeComponent();
            UpdateGrid.Visibility = Visibility.Visible;
            bl = blObject;
            parcelListWindow = MyparcelListWindow;
            index = myIndex;
            parcel = bl.GetParcel(id);
            DataContext = parcel;
            SenderTextBox.Text = parcel.Sender.ToString();
            RecieverTextBox.Text = parcel.Receiving.ToString();
            if(parcel.Scheduled!=null)
            DroneParcelTextBox.Text = parcel.DroneAtParcel.ToString();


        }
        DroneWindow MyDroneWindow;
        private void SenderDetails_Click(object sender, MouseButtonEventArgs e)
        {
            //CustomerParcel customerParcel = (CustomerParcel)SenderTextBox.Text;
            
            //int droneIndex = DroneChargingView.SelectedIndex;
            //this.IsEnabled = false;
            //if (drone != null)
            //    new DroneWindow(bl, this, drone.Id, droneIndex).Show();
        }
        public ParcelWindow(BlApi.IBL blObject, DroneWindow droneWindow,  int id)
        {
            InitializeComponent();
            UpdateGrid.Visibility = Visibility.Visible;
            bl = blObject;
            MyDroneWindow = droneWindow;
            parcel = bl.GetParcel(id);
            DataContext = parcel;

        }

        private void Delivery_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PickUp_Click(object sender, RoutedEventArgs e)
     {
        ////    DroneParcel droneParcel = DroneParcel.Parse(PickUpTextBox1.Text);
        ////    DroneParcel droneParcel =(DroneParcel) this.PickUpTextBox1.Text;
        ////    bl.PickUpParcelByDrone()



        }

        private void deleteParcel_Click(object sender, RoutedEventArgs e)
        {
            if(parcel.Scheduled!=null)
            {
                //deleteParcel.IsEnabled = false;
                MessageBox.Show("Parcel that has been assigned cant't be deleted?", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {

                MessageBoxResult boxresult = MessageBox.Show("Are you sure you want to delete  this parcel", "info!", MessageBoxButton.YesNo, MessageBoxImage.Information);
                switch (boxresult)
                {

                    case MessageBoxResult.Yes:

                        bl.DeleteParcel(parcel.Id);
                        parcelListWindow.parcelToList.RemoveAt(index);
                        parcelListWindow.ParcelListView.Items.Refresh();
                        parcelListWindow.IsEnabled = true;
                        deleteParcel.IsEnabled = false;
                        close = false;
                        Close();
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

        
           
          
    }
}
