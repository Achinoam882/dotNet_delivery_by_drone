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
        bool close = true;
        BlApi.IBL bl;
        CollectionView view;
        public ObservableCollection<BO.DroneToList> droneToLists;
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
        private void AddDroneClick(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl, this).Show();
            this.IsEnabled = false;
          

        }
        private void DroneToLists_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AcordingToStatusSelectorChanged();
        
        }


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
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Reset.Visibility = Visibility.Visible;
            AcordingToStatusSelectorChanged();
        }

       
        private void MaxWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Reset.Visibility = Visibility.Visible;
            AcordingToStatusSelectorChanged();
        }
        private void ResetClick(object sender, RoutedEventArgs e)
        {
            DronesListView.ItemsSource = bl.GetDroneList();
            StatusSelector.SelectedItem = null;
            WeightSelector.SelectedItem = null;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            close = false;
            this.Close();
            
        }

        private void DroneAct_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList drone = (DroneToList)DronesListView.SelectedItem;
            int droneIndex = DronesListView.SelectedIndex;
            this.IsEnabled = false;
            if (drone != null)
                  new DroneWindow(bl, this, drone.Id, droneIndex).Show();
        }
        protected override void OnClosing(CancelEventArgs e)//bonus
        {
            base.OnClosing(e);

            e.Cancel = close;
        }
        private void ClearOutlinedComboBox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Group_click(object sender, RoutedEventArgs e)
        {
            if(GroupButton.Name == "GroupButton")

            {
                view = (CollectionView)CollectionViewSource.GetDefaultView(DronesListView.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Status");
                view.GroupDescriptions.Add(groupDescription);

            }

           

        }
    

    }

    
       
    }

    


