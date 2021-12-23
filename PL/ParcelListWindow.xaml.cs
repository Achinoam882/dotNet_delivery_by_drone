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
    /// Interaction logic for ParcelListWindow.xaml
    /// </summary>
    public partial class ParcelListWindow : Window
    {
        BlApi.IBL bl;
        CollectionView view;

        //bool close = true;
        public ObservableCollection<BO.ParcelToList> parcelToList;
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
        private void ParcelToList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AcordingToStatusSelectorChanged();

        }


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
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            clearButtom.Visibility = Visibility.Visible;
            AcordingToStatusSelectorChanged();
        }
        private void PrioritySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            clearButtom.Visibility = Visibility.Visible;
            AcordingToStatusSelectorChanged();
        }


        private void MaxWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            clearButtom.Visibility = Visibility.Visible;
            AcordingToStatusSelectorChanged();
        }
        



        private void ParcelAct_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelToList parcel = (ParcelToList)ParcelListView.SelectedItem;
            int parcelIndex = ParcelListView.SelectedIndex;
            this.IsEnabled = false;
            if (parcel != null)
                new ParcelWindow(bl, this, parcel.Id, parcelIndex).Show();
        }
              
            
        private void AddParcel_Click(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(bl, this).Show();
            this.IsEnabled = false;
        }

        private void clearButtom_Click(object sender, RoutedEventArgs e)
        {
            ParcelListView.ItemsSource = bl.GetParcelList();
            statusselector.SelectedItem = null;
            weightselector.SelectedItem = null;
            Priorityselector.SelectedItem = null;
        }

        private void Group_click(object sender, RoutedEventArgs e)
        {

            if (GroupButton.Name == "GroupButton")

            {
                view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelListView.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Sender");
                view.GroupDescriptions.Add(groupDescription);
                GroupButton.Content = "Clear";
                GroupButton.Name = "ClearButton";

            }
            else
            {

                view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelListView.ItemsSource);
                view.GroupDescriptions.Clear();
                GroupButton.Content = "Grouping By Sender";
                GroupButton.Name = "GroupButton";
            }
        }
        private void Group_click2(object sender, RoutedEventArgs e)
        {

            if (GroupButton2.Name == "GroupButton2")

            {
                view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelListView.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Receiving");
                view.GroupDescriptions.Add(groupDescription);
                GroupButton2.Content = "Clear";
                GroupButton2.Name = "ClearButton2";

            }
            else
            {

                view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelListView.ItemsSource);
                view.GroupDescriptions.Clear();
                GroupButton2.Content = "Grouping By Receiver";
                GroupButton2.Name = "GroupButton2";
            }
        }
    }
}
