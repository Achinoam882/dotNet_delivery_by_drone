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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        BlApi.IBL bl;

         bool homeClicked=false;
         bool powerClicked=false;
        bool BackClicked=false;
        int index;
        public Drone drone;
        private DroneListWindow DroneListWindow;
        #region add drone
        /// <summary>
        ///constractor to add drone
        /// </summary>v
        public DroneWindow(BlApi.IBL blObject, DroneListWindow droneListWindow)
        {
            InitializeComponent();
            AddDroneGrid.Visibility = Visibility.Visible;
            bl = blObject;
            DroneListWindow = droneListWindow;
            MaxWeightSelector1.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            FirstCahrgingStationSelector1.ItemsSource = blObject.GetBaseStationList(x => x.FreeChargeSlots > 0);

        }
        /// <summary>
        ///Cancel addition drone
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult boxresult = MessageBox.Show("Are you sure you want to cancel this addition?", "info!", MessageBoxButton.YesNo, MessageBoxImage.Information);
            switch (boxresult)
            {

                case MessageBoxResult.Yes:



                    Close();
                    DroneListWindow.IsEnabled = true;

                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        ///Add new drone to the list
        /// </summary>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (IdTextBox.Text.Length != 0 && ModelTextBox.Text.Length != 0 && MaxWeightSelector1.SelectedItem != null && FirstCahrgingStationSelector1.SelectedItem != null)
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
                    //close = false;
                    Close();
                    DroneListWindow.IsEnabled = true;

                }
                catch (AddingProblemException ex)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    if (IdTextBox.Focus())
                    {
                        IdTextBox.BorderBrush = Brushes.Red; //bonus 
                    }
                    if (MaxWeightSelector1.Focus())
                    {
                        MaxWeightSelector1.BorderBrush = Brushes.Red; //bonus
                    }
                    if (FirstCahrgingStationSelector1.Focus())
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

        /// <summary>
        ///integrity check for add negative id drone
        /// </summary>
        private void IdCheck_LostFocus(object sender, RoutedEventArgs e)
        {
            if (IdTextBox.Text.StartsWith('-'))
            {
                IdTextBox.Text = "Id can't be a negative number";
                IdTextBox.BorderBrush = Brushes.Red;
                IdTextBox.Foreground = Brushes.Red;
            }
        }
        /// <summary>
        ///integrity check for add negative id drone
        /// </summary>
        private void IdCheck_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IdTextBox.Text == "Id can't be a negative number")
            {
                IdTextBox.Clear();
                IdTextBox.Foreground = Brushes.Black;
                IdTextBox.BorderBrush = Brushes.Transparent;
            }
        }
        /// <summary>
        ///integrity check for add long model  drone
        /// </summary>
        private void ModelCheck_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ModelTextBox.Text.Length > 5)
            {
                ModelTextBox.Text = "Model name is too long";
                ModelTextBox.BorderBrush = Brushes.Red;
                ModelTextBox.Foreground = Brushes.Red;
            }
        }
        /// <summary>
        ///integrity check for add too long model  drone
        /// </summary>
        private void ModelCheck_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ModelTextBox.Text == "Model name is too long")
            {
                ModelTextBox.Clear();
                ModelTextBox.Foreground = Brushes.Black;
                ModelTextBox.BorderBrush = Brushes.Transparent;
            }
        }
        #endregion add drone


        #region  drone actions
        /// <summary>
        ///constractor to update drone window 
        /// </summary>
        public DroneWindow(BlApi.IBL blObject, DroneListWindow droneListWindow, int Id, int Indexdrone)
        {
            InitializeComponent();
            UpDateGrid.Visibility = Visibility.Visible;
            DroneGrid.Visibility = Visibility.Visible;
            bl = blObject;
            DroneListWindow = droneListWindow;
            index = Indexdrone;
            drone = bl.GetDrone(Id);
            DataContext = drone;
            StatusDroneclicks(drone.Status);
            BatteryChange(drone.Battery);
        }
        /// <summary>
        ///prograssbar battery change
        /// </summary>
        private void BatteryChange(double battery)
        {
            if (battery < 50)
            {
                if (battery > 15)
                {
                    PrograssbarBattery.Foreground = Brushes.Yellow;
                }
                else
                {
                    PrograssbarBattery.Foreground = Brushes.Red;
                }
            }
            else
                PrograssbarBattery.Foreground = Brushes.Green;

        }
        /// <summary>
        ///Status drone 
        /// </summary>
        public void StatusDroneclicks(DroneStatuses status)

        {
            switch ((DroneStatuses)status)
            {
                case DroneStatuses.Free:



                    StopChargingDrone.IsEnabled = false;
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
                    DroneNotAssign.Visibility = Visibility.Hidden;
                    DroneNotAssignn.Visibility = Visibility.Hidden;
                    ParcelInTransfer.Visibility = Visibility.Visible;
                    textboxParcelintransfer.Visibility = Visibility.Visible;
                    if (!drone.DroneParcel.ParcelStatus)
                    {
                        PickupParcelByDrone.IsEnabled = true;
                        StopChargingDrone.IsEnabled = false;
                        SendDroneToCharge.IsEnabled = false;
                        AssignDroneToParcel.IsEnabled = false;
                        DeliverDroneToParcel.IsEnabled = false;
                    }
                    else
                    {
                        DeliverDroneToParcel.IsEnabled = true;
                        StopChargingDrone.IsEnabled = false;
                        SendDroneToCharge.IsEnabled = false;
                        AssignDroneToParcel.IsEnabled = false;
                        PickupParcelByDrone.IsEnabled = false;
                    }
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        ///Update the model drone and refresh the drone list
        /// </summary>
        private void UpDateModel_Click(object sender, RoutedEventArgs e)
        {
            bl.ChangeDroneModel(drone.Id, ModelTextBox2.Text);
            MessageBox.Show("The Model Was Updated successfully", "success!");
            drone = bl.GetDrone(drone.Id);
            DataContext = drone;
            if (DroneListWindow != null)
            {

                //int index = DroneListWindow.DronesListView.SelectedIndex;
                //DroneToList droneToList = DroneListWindow.droneToLists[index];
                //droneToList.Model = ModelTextBox2.Text;
                //DroneListWindow.droneToLists[index] = droneToList;
                DroneListWindow.DronesListView.Items.Refresh();
                DroneListWindow.AcordingToStatusSelectorChanged();
            }
            ModelTextBox2.IsReadOnly = true;
        }





        /// <summary>
        ///Pick Up parcel by drone
        /// </summary>
        private void PickupParcelByDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.PickUpParcelByDrone(drone.Id);
                // textboxParcelintransfer.Text = drone.DroneParcel.ToString();
                textboxParcelintransfer.Visibility = Visibility.Visible;
                ParcelInTransfer.Visibility = Visibility.Visible;

                // ParcelInTransfer.Visibility = Visibility.Visible;
                MessageBox.Show("Pickup Parcel By Drone was successfully done", "success!");
                drone = bl.GetDrone(drone.Id);
                DataContext = drone;
                if (DroneListWindow != null)
                {
                    DroneListWindow.DronesListView.Items.Refresh();

                    DroneListWindow.AcordingToStatusSelectorChanged();
                }

                DeliverDroneToParcel.IsEnabled = true;
                PickupParcelByDrone.IsEnabled = false;
                AssignDroneToParcel.IsEnabled = false;
                DroneNotAssign.Visibility = Visibility.Hidden;
                BatteryChange(drone.Battery);

            }
            catch (UpdateProblemException ex)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /// <summary>
        ///Assign drone to parcel
        /// </summary>
        private void AssignDroneToParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.AssignParcelToDrone(drone.Id);
                DroneNotAssign.Visibility = Visibility.Hidden;
                DroneNotAssignn.Visibility = Visibility.Hidden;
                ParcelInTransfer.Visibility = Visibility.Visible;

                textboxParcelintransfer.Visibility = Visibility.Visible;
                // bl.GetDrone(drone.Id);
                drone = bl.GetDrone(drone.Id);
                DataContext = drone;
                if (DroneListWindow != null)
                {


                    DroneListWindow.DronesListView.Items.Refresh();
                    DroneListWindow.AcordingToStatusSelectorChanged();
                }
                MessageBox.Show("Assign Parcel By Drone was successfully done", "success!");
                AssignDroneToParcel.IsEnabled = false;
                DeliverDroneToParcel.IsEnabled = false;
                StopChargingDrone.IsEnabled = false;
                PickupParcelByDrone.IsEnabled = true;
                SendDroneToCharge.IsEnabled = false;





            }
            catch (UpdateProblemException ex)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        ///Realese drone from charging
        /// </summary>
        private void StopChargingDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                //string stringOfTime = StopChargingDroneTime.Text;
                //TimeSpan.TryParse(stringOfTime, out time);
                bl.ReleaseFromCharging(drone.Id);
                MessageBox.Show("Releasing drone from charging was successful", "success!");
                drone = bl.GetDrone(drone.Id);
                DataContext = drone;

                if (DroneListWindow != null)
                {
                    DroneListWindow.AcordingToStatusSelectorChanged();
                    DroneListWindow.DronesListView.Items.Refresh();
                }

                AssignDroneToParcel.IsEnabled = true;
                StopChargingDrone.IsEnabled = false;
                if (drone.Battery == 100)
                {
                    SendDroneToCharge.IsEnabled = false;

                }
                else
                {
                    SendDroneToCharge.IsEnabled = true;
                }
                BatteryChange(drone.Battery);
            }
            catch (UpdateProblemException ex)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        /// <summary>
        ///Sending Drone To Charge
        /// </summary>
        private void SendDroneToCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.SendDroneToCharge(drone.Id);

                MessageBox.Show("Your Drone was sent to charge successfully", "success!");
                drone = bl.GetDrone(drone.Id);
                DataContext = drone;
                if (DroneListWindow != null)
                {

                    DroneListWindow.DronesListView.Items.Refresh();
                    DroneListWindow.AcordingToStatusSelectorChanged();
                }

                StopChargingDrone.IsEnabled = true;
                AssignDroneToParcel.IsEnabled = false;
                DeliverDroneToParcel.IsEnabled = false;
                PickupParcelByDrone.IsEnabled = false;
                SendDroneToCharge.IsEnabled = false;


            }
            catch (UpdateProblemException ex)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        ///Delivery parcel by drone
        /// </summary>
        private void DeliverDroneToParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.DroneDeliverParcel(drone.Id);
              

                MessageBox.Show("Delivering parcel was successful", "success!");
                textboxParcelintransfer.Visibility = Visibility.Hidden;
                ParcelInTransfer.Visibility = Visibility.Hidden;

                DroneNotAssign.Visibility = Visibility.Visible;
                DroneNotAssignn.Visibility = Visibility.Visible;
                drone = bl.GetDrone(drone.Id);
                DataContext = drone;
                if (DroneListWindow != null)
                {


                    DroneListWindow.DronesListView.Items.Refresh();
                    DroneListWindow.AcordingToStatusSelectorChanged();
                }
                SendDroneToCharge.IsEnabled = true;
                AssignDroneToParcel.IsEnabled = true;
                DeliverDroneToParcel.IsEnabled = false;
                BatteryChange(drone.Battery);

            }
            catch (UpdateProblemException ex)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        #endregion  drone actions

        #region drone window from base station
        /// <summary>
        ///Constractor to update  drone  window from base station
        /// </summary>
        public DroneWindow(BlApi.IBL blObject, int id, int droneIndex)
        {
            InitializeComponent();
            UpDateGrid.Visibility = Visibility.Visible;

            bl = blObject;
            index = droneIndex;
            drone = bl.GetDrone(id);
            DataContext = drone;
            StatusDroneclicks(drone.Status);
            BatteryChange(drone.Battery);


        }
        #endregion drone window from base station

        #region infor about parcel in transfer
        /// <summary>
        ///Open  parcel in transfer window
        /// </summary>
        private void textboxParcelintransfer_MouseEnter(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Parcel p = bl.GetParcel(drone.DroneParcel.Id);
                new ParcelWindow(bl, this, p.Id).ShowDialog();

            }
            catch (GetDetailsProblemException ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }



        }
        #endregion infor about parcel in transfer

        #region close clicks
        /// <summary>
        ///Back to previous window
        /// </summary>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (ManualButtom.Visibility == Visibility.Visible)
            {
                BackClicked = true;
                SimulatorOfDrone.CancelAsync();
                Cursor = Cursors.Wait;
            }
            else
                Close();
        }
        /// <summary>
        ///close all the windows.
        /// </summary>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (ManualButtom.Visibility == Visibility.Visible)
            {
                powerClicked = true;
                SimulatorOfDrone.CancelAsync();
                Cursor = Cursors.Wait;
            }
            else
                Application.Current.Shutdown();
        }
        /// <summary>
        ///back to previous window.
        /// </summary>
        private void BackWindow_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        ///return to home window.
        /// </summary>
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            if(ManualButtom.Visibility == Visibility.Visible)
            {
                homeClicked = true;

                SimulatorOfDrone.CancelAsync();
                Cursor = Cursors.Wait;



            }
            else
            if(DroneListWindow != null)
            {
                this.Close();
                DroneListWindow.Close();
            }
            #endregion close clicks
    }

        #region Simulator

        internal BackgroundWorker SimulatorOfDrone;//defining the simulator

        /// <summary>
        /// The function creates the process.
        /// </summary>
        private void Simultor()
        {
            SimulatorOfDrone = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            SimulatorOfDrone.DoWork += SimulatorOfDrone_DoWork; //Operation function.
            SimulatorOfDrone.ProgressChanged += SimulatorOfDrone_ProgressChanged; //changed function.

            SimulatorOfDrone.RunWorkerCompleted += SimulatorOfDrone_RunWorkerCompleted;
        }
        /// <summary>
        /// The function handles in case the user selects the automatic process button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Simulator_Click(object sender, RoutedEventArgs e)
        {

            Simultor();//to start the prosess
            SimulatorOfDrone.RunWorkerAsync();//run the prosess. מפעילה את המתודה שנרשמה לאירוע SimulatorOfDrone_DoWork
            ModelTextBox2.IsReadOnly = true;
            UpdateDroneName.Visibility = Visibility.Hidden;
            PickupParcelByDrone.Visibility = Visibility.Hidden;
            SendDroneToCharge.Visibility = Visibility.Hidden;
            StopChargingDrone.Visibility = Visibility.Hidden;
            DeliverDroneToParcel.Visibility = Visibility.Hidden;
            AssignDroneToParcel.Visibility = Visibility.Hidden;
            //BackWindow.Visibility= Visibility.Hidden;
            simulatorButtom.IsEnabled = false;
            ManualButtom.Visibility = Visibility.Visible;
        }
        private void Manual_Click(object sender, RoutedEventArgs e)
        {
            ManualButtom.Visibility = Visibility.Hidden;
            SimulatorOfDrone.CancelAsync(); //turns on the cancel flag to be true
            Cursor = Cursors.Wait;           
        }
        /// <summary>
        /// The function handles the display when changes made in the process are received.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>x
        private void SimulatorOfDrone_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            drone = bl.GetDrone(drone.Id);
            DataContext = drone;   
            DroneListWindow.AcordingToStatusSelectorChanged();
            DroneListWindow.DronesListView.Items.Refresh();
            if(drone.Status==DroneStatuses.Busy)
            {
                textboxParcelintransfer.Visibility = Visibility.Visible;
                ParcelInTransfer.Visibility = Visibility.Visible;
                DroneNotAssign.Visibility = Visibility.Hidden;
                DroneNotAssignn.Visibility = Visibility.Hidden;

            }
            if (drone.Status == DroneStatuses.Free)
            {
                ParcelInTransfer.Visibility = Visibility.Hidden;

                textboxParcelintransfer.Visibility = Visibility.Hidden;
                DroneNotAssign.Visibility = Visibility.Visible;
                DroneNotAssignn.Visibility = Visibility.Visible;

            }
            BatteryChange(drone.Battery);


        }   /// <summary>
            /// The function finish and completed the simulator
            /// </summary>
        private void SimulatorOfDrone_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (ManualButtom.Visibility == Visibility.Hidden)//if he pressed manual bottom
            {
                
                Cursor = Cursors.Arrow;
               
            }
             if (ManualButtom.Visibility == Visibility.Visible)//if  he pressed back,power,home buttoms
            {
                
                Cursor = Cursors.Arrow;
                if (homeClicked)
                {
                    this.Close();
                    DroneListWindow.Close();
                }
                else if(BackClicked)
                {
                    this.Close();
                }
                else if(powerClicked)
                {
                    Application.Current.Shutdown();
                }
            }
           
            StatusDroneclicksAfterSimulator();
            StatusDroneclicks(drone.Status);
          

        }
        /// <summary>
        /// Status Drone after simulator
        /// </summary>
        private void StatusDroneclicksAfterSimulator()
        {
            ManualButtom.Visibility = Visibility.Hidden;
            ModelTextBox2.IsReadOnly = false;
            UpdateDroneName.Visibility = Visibility.Visible;
            PickupParcelByDrone.Visibility = Visibility.Visible;
            SendDroneToCharge.Visibility = Visibility.Visible;
            StopChargingDrone.Visibility = Visibility.Visible;
            DeliverDroneToParcel.Visibility = Visibility.Visible;
            AssignDroneToParcel.Visibility = Visibility.Visible;
            simulatorButtom.IsEnabled = true;
        }

        /// <summary>
        ///When Simulator stops
        /// </summary>
        public bool IsTimeFinish()
        {
            return SimulatorOfDrone.CancellationPending; //return true when u want to close the sim and becomes true
        }
        /// <summary>
        /// function calls from simulator constractor in BL to report progress
        /// </summary>
        public void ToReportProgress()
        {

            SimulatorOfDrone.ReportProgress(0);//calls the function(SimulatorOfDrone_ProgressChange) that is signed up to ProgressChanged
        }
        /// <summary>
        /// start the simulator
        /// </summary>
        private void SimulatorOfDrone_DoWork(object sender, DoWorkEventArgs e) 
        {
            bl.SimulatorOn(drone.Id, ToReportProgress, IsTimeFinish);
        }

      

        
    }

    #endregion
}
