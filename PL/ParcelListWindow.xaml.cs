using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelListWindow.xaml
    /// </summary>
    public partial class ParcelListWindow : Window
    {
        BlApi.IBL bl;
        CollectionView view;

     
        public ObservableCollection<BO.ParcelToList> parcelToList;
        #region constractor
        /// <summary>
        /// Constractor for parcel list window
        /// </summary>
        public ParcelListWindow(BlApi.IBL blObject)
        {
            InitializeComponent();
            parcelToList = new ObservableCollection<ParcelToList>();
            bl = blObject;
            List<BO.ParcelToList> parcelBL = bl.GetParcelList().ToList();
            foreach (var x in parcelBL)
            {
                parcelToList.Add(x);
            }
            parcelToList.CollectionChanged += ParcelToList_CollectionChanged;
            ParcelListView.ItemsSource = parcelToList;
            statusselector.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
            weightselector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            Priorityselector.ItemsSource = Enum.GetValues(typeof(Priorities));

        }
        #endregion constractor
        #region list  view selection change
        /// <summary>
        /// Collection change
        /// </summary>
        private void ParcelToList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AcordingToStatusSelectorChanged();

        }

        /// <summary>
        /// filter the list by weight and status parcel
        /// </summary>
        public void AcordingToStatusSelectorChanged()
        {

            if (weightselector.SelectedItem == null && statusselector.SelectedItem == null && Priorityselector.SelectedItem == null)
                ParcelListView.ItemsSource = parcelToList;
            else if (weightselector.SelectedItem == null && statusselector.SelectedItem == null)
                ParcelListView.ItemsSource = parcelToList.ToList().FindAll(x => x.Priority == (Priorities)Priorityselector.SelectedItem);
            else if (statusselector.SelectedItem == null && Priorityselector.SelectedItem == null)
                ParcelListView.ItemsSource = parcelToList.ToList().FindAll(x => x.Weight == (WeightCategories)weightselector.SelectedItem);
            else if (weightselector.SelectedItem == null && Priorityselector.SelectedItem == null)
                ParcelListView.ItemsSource = parcelToList.ToList().FindAll(x => x.Status == (ParcelStatus)statusselector.SelectedItem);
            else if (weightselector.SelectedItem == null )
            {
                ParcelListView.ItemsSource = parcelToList.ToList().FindAll(x => x.Status == (ParcelStatus)statusselector.SelectedItem&& x.Priority == (Priorities)Priorityselector.SelectedItem);

            }
            else if (statusselector.SelectedItem == null)
            {
                ParcelListView.ItemsSource = parcelToList.ToList().FindAll(x => x.Weight == (WeightCategories)weightselector.SelectedItem&& x.Priority == (Priorities)Priorityselector.SelectedItem);

            }
            else
            {
                ParcelListView.ItemsSource = parcelToList.ToList().FindAll(x => x.Weight == (WeightCategories)weightselector.SelectedItem&& x.Status == (ParcelStatus)statusselector.SelectedItem);

            }

        }
        /// <summary>
        /// filter the list by date requested parcel
        /// </summary>
        private void datepicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ParcelListView.ItemsSource = parcelToList.ToList().FindAll(x => bl.GetParcel(x.Id).Requested >= datepicker.SelectedDate );

        }
        /// <summary>
        /// filter the list by status  parcel
        /// </summary>
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            clearButtom.Visibility = Visibility.Visible;
            AcordingToStatusSelectorChanged();
        }
        /// <summary>
        /// filter the list by priority  parcel
        /// </summary>
        private void PrioritySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            clearButtom.Visibility = Visibility.Visible;
            AcordingToStatusSelectorChanged();
        }
        /// <summary>
        /// filter the list by weight  parcel
        /// </summary>
        private void MaxWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            clearButtom.Visibility = Visibility.Visible;
            AcordingToStatusSelectorChanged();
        }
        // <summary>
        /// Refresh the list of parcel
        /// </summary>
        private void clearButtom_Click(object sender, RoutedEventArgs e)
        {
            view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelListView.ItemsSource);
            view.GroupDescriptions.Clear();
            ParcelListView.ItemsSource = bl.GetParcelList();
            statusselector.SelectedItem = null;
            weightselector.SelectedItem = null;
            Priorityselector.SelectedItem = null;
        }
        // <summary>
        /// Group the list parcel by the sender
        /// </summary>
        private void Group_click(object sender, RoutedEventArgs e)
        {

                view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelListView.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Sender");
                view.GroupDescriptions.Add(groupDescription);


            

        }

        #endregion list  view selection change
        #region parcel info
        // <summary>
        ///Open parcel window with his detail and do act on this
        /// </summary>
        private void ParcelAct_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelToList parcel = (ParcelToList)ParcelListView.SelectedItem;
            int parcelIndex = ParcelListView.SelectedIndex;
            //this.IsEnabled = false;
            if (parcel != null)
                new ParcelWindow(bl, this, parcel.Id, parcelIndex).ShowDialog();
        }
        #endregion parcel info 
        #region  add parcel 
        // <summary>
        /// Open window to add new parcel
        /// </summary>
        private void AddParcel_Click(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(bl, this).ShowDialog();
           // this.IsEnabled = false;
        }
        #endregion  add parcel 
        #region close
        // <summary>
        ///close all the windows.
        /// </summary>
        private void Close_Click(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();
        }
        // <summary>
        ///return to home window.
        /// </summary>
        private void Home_Click(object sender, RoutedEventArgs e)
        {


            Close();

        }
        #endregion close


    }
}
