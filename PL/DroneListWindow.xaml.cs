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



namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        
        BlApi.IBL bl;
        CollectionView view;
        public ObservableCollection<BO.DroneToList> droneToLists;
        #region constractor
        /// <summary>
        ///Constractor to drone list window
        /// </summary>
        public DroneListWindow(BlApi.IBL blObject)
        {
            InitializeComponent();
            bl = blObject;
            droneToLists = new ObservableCollection<DroneToList>();
            List<BO.DroneToList> dronesBl = bl.GetDroneList().ToList();
            foreach (var x in dronesBl)
            {
                droneToLists.Add(x);
            }
           
            DronesListView.ItemsSource = droneToLists;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            droneToLists.CollectionChanged += DroneToLists_CollectionChanged;
        }
        #endregion constractor

        #region add drone
        /// <summary>
        ///Open add drone window 
        /// </summary>
        private void AddDroneClick(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl, this).ShowDialog();
           // this.IsEnabled = false;
          

        }
        #endregion add drone

        #region lists changes from selectoes ets
        /// <summary>
        ///Collection Change
        /// </summary>
        private void DroneToLists_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AcordingToStatusSelectorChanged();
        
        }

        /// <summary>
        ///the function filters by weight and status drone
        /// </summary>
        public void AcordingToStatusSelectorChanged()
        {
           
            if (WeightSelector.SelectedItem == null && StatusSelector.SelectedItem == null)
                DronesListView.ItemsSource = droneToLists;
            else if (WeightSelector.SelectedItem == null)
                DronesListView.ItemsSource = droneToLists.ToList().FindAll(x => x.Status == (DroneStatuses)StatusSelector.SelectedItem);
            else if (StatusSelector.SelectedItem == null)
                DronesListView.ItemsSource = droneToLists.ToList().FindAll(x => x.MaxWeight == (WeightCategories)WeightSelector.SelectedItem);
            else
                DronesListView.ItemsSource = droneToLists.ToList().FindAll(x => x.MaxWeight == (WeightCategories)WeightSelector.SelectedItem &&
            x.Status == (DroneStatuses)StatusSelector.SelectedItem);

        }
        /// <summary>
        ///Selection Changed by status drone
        /// </summary>
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Reset.Visibility = Visibility.Visible;
            AcordingToStatusSelectorChanged();
        }
        /// <summary>
        ///Selection Changed by weight drone
        /// </summary>
        private void MaxWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Reset.Visibility = Visibility.Visible;
            AcordingToStatusSelectorChanged();
        }
        /// <summary>
        ///reset the selection
        /// </summary>
        private void ResetClick(object sender, RoutedEventArgs e)
        {
            view = (CollectionView)CollectionViewSource.GetDefaultView(DronesListView.ItemsSource);
            view.GroupDescriptions.Clear();
            DronesListView.ItemsSource = bl.GetDroneList();
            StatusSelector.SelectedItem = null;
            WeightSelector.SelectedItem = null;
        }
        #endregion lists changes from selectoes ets

        #region close clicks
        /// <summary>
        ///back to previous window
        /// </summary>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            
        }
        /// <summary>
        ///Close all windows
        /// </summary>
        private void Close_Click(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();
        }
        #endregion close clicks

        #region drone info
        /// <summary>
        ///Open new drone window with his details and do act on this drone
        /// </summary>
        private void DroneAct_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList drone = (DroneToList)DronesListView.SelectedItem;
            int droneIndex = DronesListView.SelectedIndex;
            //this.IsEnabled = false;
            if (drone != null)
                  new DroneWindow(bl, this, drone.Id, droneIndex).Show();
        }
        #endregion drone info

        #region group click
        /// <summary>
        ///group the list by status drone
        /// </summary>
        private void Group_click(object sender, RoutedEventArgs e)
        {
                view = (CollectionView)CollectionViewSource.GetDefaultView(DronesListView.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Status");
                view.GroupDescriptions.Add(groupDescription);
           
                
               
                
                
           
        }
        #endregion group click
        


    }

    
       
    }

    


