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
    /// Interaction logic for BaseStationListWindow.xaml
    /// </summary>
    public partial class BaseStationListWindow : Window
    {
        CollectionView view;

        BlApi.IBL bl;
        bool close = true;
        public ObservableCollection<BO.BaseStationToList> baseStationToList;
        
        public BaseStationListWindow(BlApi.IBL blObject)
        {
            InitializeComponent();
            bl = blObject;
            baseStationToList = new ObservableCollection<BaseStationToList>();
            
            //var<BO.BaseStationToList>(from item in bl.GetBaseStationList()
            //                           orderby item
            //                           select item);
            List<BO.BaseStationToList> baseStationBL = bl.GetBaseStationList().ToList();
            foreach (var x in baseStationBL)
            {
                baseStationToList.Add(x);
            }
            BaseStationListView.ItemsSource = baseStationToList;
            //baseStationToList.CollectionChanged += BaseStationToList_CollectionChanged;
        }
      

        private void BaeStationAct_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            BaseStationToList baseStation = (BaseStationToList)BaseStationListView.SelectedItem;
            int baseStationIndex = BaseStationListView.SelectedIndex;
            //this.IsEnabled = false;
            if (baseStation != null)
                new BaseStationWindow(bl, this, baseStation.Id, baseStationIndex).Show();
            //BaseStationListView.ItemsSource=baseStationToList.refresh();
        }

        private void AddBaseStation_Click(object sender, RoutedEventArgs e)
        {
            
            new BaseStationWindow(bl, this).Show();
            this.IsEnabled = false;
        }
        protected override void OnClosing(CancelEventArgs e)//bonus
        {
            base.OnClosing(e);

            e.Cancel = close;
        }

        private void Group_click(object sender, RoutedEventArgs e)
        {
            if (GroupButton.Name == "GroupButton")

            {
                view = (CollectionView)CollectionViewSource.GetDefaultView(BaseStationListView.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription(" FreeChargeSlots");
                view.GroupDescriptions.Add(groupDescription);
                GroupButton.Content = "Clear";
                GroupButton.Name = "ClearButton";

            }
            else
            {

                view = (CollectionView)CollectionViewSource.GetDefaultView(BaseStationListView.ItemsSource);
                view.GroupDescriptions.Clear();
                GroupButton.Content = "Grouping By Free charges slots";
                GroupButton.Name = "GroupButton";
            }
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();
        }

    }
    
}
