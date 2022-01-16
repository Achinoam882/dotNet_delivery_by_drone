using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Threading;
using static BL.BL;

namespace BL
{
    /// <summary>
    /// drone simulator.
    /// </summary>
    class Simulator
    {

        private const double kmPerHour = 3600;
        BL bl;
        DalApi.IDal dalObject;
        public Simulator(BL myBl, int droneID, Action ToReportProgress, Func<bool> IsTimeFinish)
        {
            bl = myBl;
            dalObject = bl.dalObject;
            double distance;
            double tempBattery;


            DroneToList droneToList = bl.GetDroneList().First(x => x.Id == droneID);
            while (!IsTimeFinish())
            {
                switch (droneToList.Status)
                {
                    case DroneStatuses.Free:
                        try
                        {
                            bl.AssignParcelToDrone(droneID);
                            ToReportProgress();
                        }
                        catch
                        {
                            if (droneToList.Battery < 100)
                            {
                                tempBattery = droneToList.Battery;
                                IEnumerable<DO.BaseStation> baseStationsDal = dalObject.GetBaseStationList();
                                IEnumerable<BaseStation> baseStationsBL = from item in baseStationsDal
                                                                          select new BaseStation()
                                                                          {
                                                                              Id = item.Id,
                                                                              Name = item.Name,
                                                                              BaseStationLocation = new Location() { Longitude = item.Longitude, Latitude = item.Latitude },
                                                                              FreeChargeSlots = item.ChargeSlots,
                                                                              DroneChargingList = new List<DroneCharging>()
                                                                          };
                                distance = bl.mindistanceBetweenLocationBaseStation(baseStationsBL, droneToList.DroneLocation).Item2;
                                while (distance > 0)
                                {

                                    droneToList.Battery -= Available;
                                    ToReportProgress();
                                    distance -= 1;
                                    Thread.Sleep(1500);
                                }
                                droneToList.Battery = tempBattery;
                                ToReportProgress();

                                bl.SendDroneToCharge(droneID);
                               

                            }

                        }
                        break;
                    case DroneStatuses.InMaintenance:
                        while (droneToList.Battery < 100)
                        {
                            droneToList.Battery += 2;//everysec more 2%
                           
                            if (droneToList.Battery > 100)
                            {
                                droneToList.Battery = 100;
                            }
                            ToReportProgress();
                            Thread.Sleep(500);                          
                        }
                        bl.ReleaseFromCharging(droneID);
                        ToReportProgress();
                        break;
                    case DroneStatuses.Busy:
                        Drone drone = bl.GetDrone(droneID);
                        if (!drone.DroneParcel.ParcelStatus)//Pickup
                        {
                            tempBattery = droneToList.Battery;
                            Location droneLoc = new Location { Longitude = droneToList.DroneLocation.Longitude, Latitude = droneToList.DroneLocation.Latitude };
                            distance = drone.DroneParcel.TransportDistance;//distance between sedner and drone location

                            while (distance > 0)        

                            {
                                droneToList.Battery -= Available;
                                distance -= 1;
                                ToReportProgress();
                                Thread.Sleep(1500);

                            }
                            droneToList.Battery = tempBattery;
                            droneToList.DroneLocation = droneLoc;
                            bl.PickUpParcelByDrone(droneID);
                            ToReportProgress();

                        }
                        else if (drone.DroneParcel.ParcelStatus)//Deliver
                        {
                            tempBattery = droneToList.Battery;
                            Location droneLoc = new Location { Longitude = droneToList.DroneLocation.Longitude, Latitude = droneToList.DroneLocation.Latitude };
                            distance = drone.DroneParcel.TransportDistance;//distance between sedner and reciver
                            while (distance > 0)
                            {
                                switch (drone.DroneParcel.Weight)
                                {
                                    case WeightCategories.Light:
                                        droneToList.Battery -= LightWeightCarrier;
                                        break;
                                    case WeightCategories.Medium:
                                        droneToList.Battery -= MediumWeightCarrier;

                                        break;
                                    case WeightCategories.Heavy:
                                        droneToList.Battery -= HeavyWeightCarrier;

                                        break;
                                    default:
                                        break;
                                }
                                ToReportProgress();
                                distance -= 1;
                                Thread.Sleep(1500);
                            }

                            droneToList.Battery = tempBattery;
                            droneToList.DroneLocation = droneLoc;
                            bl.DroneDeliverParcel(droneID);
                            ToReportProgress();
                        }
                        break;
                    default:
                        break;
                }
                Thread.Sleep(1500);
            }




        }

        
     }
    

}
