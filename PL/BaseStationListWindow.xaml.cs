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
        #region constractor
        public ObservableCollection<BO.BaseStationToList> baseStationToList;
        /// <summary>
        ///constractor of base station list window.
        /// </summary>
        
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
        #endregion constractor

        #region info double click
        /// <summary>
        ///Open a window with the datails of the selected station and do act on this.
        /// </summary>
        private void BaeStationAct_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            BaseStationToList baseStation = (BaseStationToList)BaseStationListView.SelectedItem;
            int baseStationIndex = BaseStationListView.SelectedIndex;
            //this.IsEnabled = false;
            if (baseStation != null)
                new BaseStationWindow(bl, this, baseStation.Id, baseStationIndex).ShowDialog();
            //BaseStationListView.ItemsSource=baseStationToList.refresh();
        }
        #endregion info double click

        #region add base station 
        /// <summary>
        ///Open a window to add a new base station to the list
        /// </summary>
        private void AddBaseStation_Click(object sender, RoutedEventArgs e)
        {
            
            new BaseStationWindow(bl, this).ShowDialog();
           // this.IsEnabled = false;
        }
        #endregion add base station 

        #region Grpouing

        /// <summary>
        ///Group the list of stations according to Free Charge Slots
        /// </summary>

        private void Group_click(object sender, RoutedEventArgs e)
        {
            
                view = (CollectionView)CollectionViewSource.GetDefaultView(BaseStationListView.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription(" FreeChargeSlots");
                view.GroupDescriptions.Add(groupDescription);
            
        }
        /// <summary>
        ///refresh the list of base station
        /// </summary>
        private void clearButtom_Click(object sender, RoutedEventArgs e)
        {
            BaseStationListView.ItemsSource = bl.GetBaseStationList();
           
        }
        #endregion Grouing

        #region close and home click
        /// <summary>
        ///close all windows
        /// </summary>
        private void Close_Click(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();
        }
        /// <summary>
        ///return to home window
        /// </summary>
        private void Home_click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion close and home click

    }

}
