using IBL.BO;
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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        IBL.IBL bl;
        public bool close = true;
        private DroneListWindow DroneListWindow;
        #region add drone
        public DroneWindow(IBL.IBL blObject, DroneListWindow droneListWindow)
        {
            InitializeComponent();
            AddDroneGrid.Visibility = Visibility.Visible;
            bl = blObject;
            DroneListWindow = droneListWindow;
            MaxWeightSelector1.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            FirstCahrgingStationSelector1.ItemsSource = blObject.GetBaseStationList(x => x.FreeChargeSlots > 0);
    
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult boxresult= MessageBox.Show("Are you sure you want to cancel this addition?", "info!", MessageBoxButton.YesNo, MessageBoxImage.Information);
            switch (boxresult)
            {
                
                case MessageBoxResult.Yes:
                    
                  
                    close = false;
                    Close();
                    DroneListWindow.IsEnabled = true;
                   
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
            
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if(IdTextBox.Text.Length!=0 && ModelTextBox.Text.Length!=0 && MaxWeightSelector1.SelectedItem!= null && FirstCahrgingStationSelector1.SelectedItem!=null)
            {
                DroneToList newDrone = new DroneToList
                {
                    Id = int.Parse(IdTextBox.Text),
                    Model = ModelTextBox.Text,
                    MaxWeight = (WeightCategories)MaxWeightSelector1.SelectedItem,

                };
                try
                {
                    bl.SetDrone(newDrone, ((BaseStationToList)FirstCahrgingStationSelector1.SelectedItem).Id);
                   MessageBox.Show("Your drone was added successfully", "success!"); 
                     
                     newDrone = bl.GetDroneList().ToList().Find(i => i.Id == newDrone.Id);
                     DroneListWindow.droneToLists.Add(newDrone);
                    close = false;
                    Close();
                    DroneListWindow.IsEnabled = true;
                        
                }
                catch(AddingProblemException ex)
                {
                   SystemSounds.Beep.Play();
                  MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                  if (IdTextBox.Focus())
                   {
                     IdTextBox.BorderBrush = Brushes.Red; //bonus 
                   }
                   if(MaxWeightSelector1.Focus())
                   {
                      MaxWeightSelector1.BorderBrush = Brushes.Red; //bonus
                    }
                   if(FirstCahrgingStationSelector1.Focus())
                   {
                      FirstCahrgingStationSelector1.BorderBrush = Brushes.Red; //bonus 
                   }
                         
                    
                   
                }
            }
            else
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Please make sure all fields are filled", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);               
            }

            

    }


        private void IdCheck_LostFocus(object sender, RoutedEventArgs e)
        {
            if (IdTextBox.Text.StartsWith('-'))
            {
                IdTextBox.Text = "Id can't be a negative number";
                IdTextBox.BorderBrush = Brushes.Red;
                IdTextBox.Foreground = Brushes.Red;
            }
        }
        private void IdCheck_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IdTextBox.Text == "Id can't be a negative number")
            {
                IdTextBox.Clear();
                IdTextBox.Foreground = Brushes.Gray;
                IdTextBox.BorderBrush = Brushes.Gray;
            }
        }
        private void ModelCheck_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ModelTextBox.Text.Length > 5)
            {
                ModelTextBox.Text = "Model name is too long";
                ModelTextBox.BorderBrush = Brushes.Red;
                ModelTextBox.Foreground = Brushes.Red;
            }
        }
        private void ModelCheck_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ModelTextBox.Text == "Model name is too long")
            {
                ModelTextBox.Clear();
                ModelTextBox.Foreground = Brushes.Gray;
                ModelTextBox.BorderBrush = Brushes.Gray;
            }
        }
        #endregion add drone
        int index;
         public Drone drone;
       
        #region  drone actions
         public DroneWindow(IBL.IBL blObject, DroneListWindow droneListWindow, int Id,int Indexdrone)
        {
            InitializeComponent();
           UpDateGrid.Visibility = Visibility.Visible;
            bl = blObject;
            DroneListWindow = droneListWindow;
            index = Indexdrone;
            drone = bl.GetDrone(Id);
             DataContext = drone;
            StatusDroneclicks(drone.Status);
        }
        public void StatusDroneclicks(DroneStatuses status)

        {
            switch ((DroneStatuses)status)
            {
                case DroneStatuses.Free:

                   
                    StopChargingDroneTime.IsEnabled = false;
                    StopChargingDrone.IsEnabled=false;
                    DeliverDroneToParcel.IsEnabled = false;
                    PickupParcelByDrone.IsEnabled = false;

                    break;
                case DroneStatuses.InMaintenance:
                   
                    SendDroneToCharge.IsEnabled = false;
                    AssignDroneToParcel.IsEnabled = false;
                    DeliverDroneToParcel.IsEnabled = false;
                    PickupParcelByDrone.IsEnabled = false;

                    break;
                case DroneStatuses.Busy:
                    if (!drone.DroneParcel.ParcelStatus)
                        PickupParcelByDrone.IsEnabled = true;
                    else
                        DeliverDroneToParcel.IsEnabled = true;
                    StopChargingDrone.IsEnabled = false;
                    StopChargingDroneTime.IsEnabled = false;
                    SendDroneToCharge.IsEnabled = false;
                    AssignDroneToParcel.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }



        private void UpDateModel_Click(object sender, RoutedEventArgs e)
        {
            bl.ChangeDroneModel(drone.Id, ModelTextBox2.Text);
            MessageBox.Show("The Model Was Updated successfully","success!");
            ModelTextBox2.IsEnabled = false;
            DroneListWindow.AcordingToStatusSelectorChanged();          
        }



      

        private void PickupParcelByDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.PickUpParcelByDrone(drone.Id);
                textboxParcelintransfer.Text = drone.DroneParcel.ToString();
                MessageBox.Show("Pickup Parcel By Drone was successfully done", "success!");
              DroneListWindow.AcordingToStatusSelectorChanged();
              drone = bl.GetDrone(drone.Id);
              DataContext = drone;
              DeliverDroneToParcel.IsEnabled = true;
              PickupParcelByDrone.IsEnabled = false;
               AssignDroneToParcel.IsEnabled = false;
               DroneNotAssign.Visibility = Visibility.Hidden;

            }
            catch(UpdateProblemException ex)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AssignDroneToParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.AssignParcelToDrone(drone.Id);
                DroneNotAssign.Visibility = Visibility.Hidden;
                textboxParcelintransfer.Text = drone.DroneParcel.ToString();
                textboxParcelintransfer.Visibility = Visibility.Visible;
                ParcelInTransfer.Visibility = Visibility.Visible;
               MessageBox.Show("Assign Parcel By Drone was successfully done", "success!");
                DroneListWindow.AcordingToStatusSelectorChanged();
                drone = bl.GetDrone(drone.Id);
                 DataContext = drone;
                DeliverDroneToParcel.IsEnabled = false;
                PickupParcelByDrone.IsEnabled = true;
                AssignDroneToParcel.IsEnabled = false;



            }
            catch (UpdateProblemException ex)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StopChargingDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TimeSpan time;
                string stringOfTime = StopChargingDroneTime.Text;
                TimeSpan.TryParse(stringOfTime, out time);
                bl.ReleaseFromCharging(drone.Id, time);
                MessageBox.Show("Releasing drone from charging was successful", "success!");
                StopChargingDroneTime.Text = " ";
                DroneListWindow.AcordingToStatusSelectorChanged();
                drone = bl.GetDrone(drone.Id);
                DataContext = drone;
                AssignDroneToParcel.IsEnabled = true;
                if (drone.Battery == 100)
                {
                    StopChargingDroneTime.IsEnabled = false;
                    StopChargingDrone.IsEnabled = false;
                    
                }
                else
                {
                    StopChargingDroneTime.IsEnabled = true;
                    StopChargingDrone.IsEnabled = true;
                }
            }
            catch (UpdateProblemException ex)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        private void SendDroneToCharge_Click(object sender, RoutedEventArgs e)
        {
        try
        {
            bl.SendDroneToCharge(drone.Id);

            MessageBox.Show("Your Drone was sent to charge successfully", "success!");
            DroneListWindow.AcordingToStatusSelectorChanged();
            drone = bl.GetDrone(drone.Id);
            DataContext = drone;
            StopChargingDroneTime.IsEnabled = true;
            StopChargingDrone.IsEnabled = true;
            }
            catch (UpdateProblemException ex)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeliverDroneToParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.DroneDeliverParcel(drone.Id);
                ParcelInTransfer.Visibility = Visibility.Hidden;
                textboxParcelintransfer.Visibility = Visibility.Hidden;
                 MessageBox.Show("Delivering parcel was successful", "success!");
                 DroneListWindow.AcordingToStatusSelectorChanged();
                 drone = bl.GetDrone(drone.Id);
                 DataContext = drone;
                SendDroneToCharge.IsEnabled = true;
                AssignDroneToParcel.IsEnabled = true;
                DeliverDroneToParcel.IsEnabled = false;
                DroneNotAssign.Visibility = Visibility.Visible;
               
            }
            catch (UpdateProblemException ex)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

      

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            DroneListWindow.IsEnabled = true;
            close = false;
            Close();
        }

        #endregion  drone actions

        private void StopChargingDroneTime_LostFocus(object sender, RoutedEventArgs e)
        {
            if (StopChargingDroneTime.Text.StartsWith('-'))
            {
                StopChargingDroneTime.Text = "Time can't be a negative number";
                StopChargingDroneTime.BorderBrush = Brushes.Red;
                StopChargingDroneTime.Foreground = Brushes.Red;
            }
        }

        private void StopChargingDroneTime_previewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (StopChargingDroneTime.Text == "Time can't be a negative number")
            {
                StopChargingDroneTime.Clear();
                StopChargingDroneTime.Foreground = Brushes.Gray;
                StopChargingDroneTime.BorderBrush = Brushes.Gray;
            }
        }
        protected override void OnClosing(CancelEventArgs e)//bonus
        {
            base.OnClosing(e);
            
            e.Cancel = close;
        }

        
    }

}
