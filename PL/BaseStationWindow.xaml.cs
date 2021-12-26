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
    /// Interaction logic for BaseStationWindow.xaml
    /// </summary>
    ///  BlApi.IBL bl;
   

    public partial class BaseStationWindow : Window
    {
        BlApi.IBL bl;
        public bool close = true;
        private BaseStationListWindow basestationListWindow;

        public BaseStationWindow(BlApi.IBL blObject, BaseStationListWindow baseStationListWindow)
        {
            InitializeComponent();
            AddBaseStationGrid.Visibility = Visibility.Visible;
            bl = blObject;
            basestationListWindow = baseStationListWindow;

           
        }

        private void cancel_click(object sender, RoutedEventArgs e)
        {
           MessageBoxResult boxresult = MessageBox.Show("Are you sure you want to cancel this addition?", "info!", MessageBoxButton.YesNo, MessageBoxImage.Information);
            switch (boxresult)
            {

                case MessageBoxResult.Yes:


                    close = false;
                    Close();
                  basestationListWindow.IsEnabled = true;

                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (IdTextBox.Text.Length != 0 && NameTextBox.Text.Length != 0 && latitudeTextBox.Text.Length != 0 && longitudeTextBox.Text.Length != 0 && chargeslotstextbox.Text.Length != 0)
            {
                Location newLocation = new Location { Latitude = double.Parse(latitudeTextBox.Text), Longitude = double.Parse(longitudeTextBox.Text )};

                BaseStation newBaseStation = new BaseStation
                {
                    Id = int.Parse(IdTextBox.Text),
                    Name = NameTextBox.Text,
                    FreeChargeSlots = int.Parse(chargeslotstextbox.Text),
                    BaseStationLocation = newLocation
                };

                try
                {
                    bl.SetBaseStation(newBaseStation);
                    MessageBox.Show("Your base station was added successfully", "success!");
                    BaseStationToList baseStationList = bl.GetBaseStationList().ToList().Find(i => i.Id == newBaseStation.Id);
                    basestationListWindow.baseStationToList.Add(baseStationList);
                    close = false;
                    Close();
                    basestationListWindow.IsEnabled = true;

                
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
        public BaseStation baseStation;
        int index;

        public BaseStationWindow(BlApi.IBL blObject,BaseStationListWindow baseStationList,int id,int indexId)
        {
            InitializeComponent();
            UpdateGrid.Visibility = Visibility.Visible;
            bl = blObject;
            basestationListWindow = baseStationList;
            index = indexId;
            baseStation = bl.GetBaseStation(id);
            DataContext = baseStation;
           // DroneChargeTextBox.Text= bl.GetBaseStation(id).DroneChargingList.;

        }

        private void UpdateBaseStation_Click(object sender, RoutedEventArgs e)
        {
          
            bl.UpdateBaseStaison(baseStation.Id, NameTextBox1.Text, FreeChargeSlotTextBox.Text);
            MessageBox.Show("The BaseStation Was Updated successfully", "success!");
            baseStation = bl.GetBaseStation(baseStation.Id);
            DataContext = baseStation;
            int index = basestationListWindow.BaseStationListView.SelectedIndex;
            BaseStationToList baseStationToList = basestationListWindow.baseStationToList[index];
            baseStationToList.FreeChargeSlots =int.Parse(FreeChargeSlotTextBox.Text);
            baseStationToList.Name = NameTextBox1.Text;
            basestationListWindow.baseStationToList[index] = baseStationToList;

            NameTextBox1.IsReadOnly = true;
            FreeChargeSlotTextBox.IsReadOnly = true;
            //BaseStationListView.baseStationToList.Items.refresh();
            basestationListWindow.BaseStationListView.Items.Refresh();
        }

        

        private void DroneInfo_Click(object sender, MouseButtonEventArgs e)
        {
            DroneCharging drone = (DroneCharging)DroneChargingView.SelectedItem;
            int droneIndex = DroneChargingView.SelectedIndex;
            this.IsEnabled = false;
            if (drone != null)
                new DroneWindow(bl, drone.Id, droneIndex).Show();

        }

        private void chargeslotstextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (chargeslotstextbox.Text.StartsWith('-'))
            {
                chargeslotstextbox.Text = "Charge Slots Can't Be A Negative Number";
                chargeslotstextbox.BorderBrush = Brushes.Red;
                chargeslotstextbox.Foreground = Brushes.Red;
            }
        }
        private void DroneInfo_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (chargeslotstextbox.Text == "Charge Slots Can't Be A Negative Number")
            {
                chargeslotstextbox.Clear();
                chargeslotstextbox.Foreground = Brushes.Black;
                chargeslotstextbox.BorderBrush = Brushes.Transparent;
            }
        }

       
}
   
}
