using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public partial class BL
    {
        #region add base station
        /// <summary>
        /// The function adds a station to the list of Basestations.
        /// </summary>
        /// <param name="newbaseStation"></param>
        public void SetBaseStation(BaseStation newBaseStation)
        {
            //exception of the logical layer//name??
            if (newBaseStation.Id < 0)
                throw new AddingProblemException("ID cant be a negative number");
            if (newBaseStation.FreeChargeSlots < 0)
                throw new AddingProblemException("free charge slots cant be a negative number");
            if (newBaseStation.BaseStationLocation.Latitude < 34.3 || newBaseStation.BaseStationLocation.Longitude > 35.5)
                throw new AddingProblemException("the location that was chosen isnt in the country");
            IDAL.DO.BaseStation baseStationDal = new IDAL.DO.BaseStation()
            {
                Id = newBaseStation.Id,
                ChargeSlots = newBaseStation.FreeChargeSlots,
                Name = newBaseStation.Name,
                Longitude = newBaseStation.BaseStationLocation.Longitude,
                Latitude = newBaseStation.BaseStationLocation.Latitude,

            };
            try
            {
                dalObject.SetBaseStation(baseStationDal);
            }
            catch (IDAL.DO.AddExistingObjectException )
            {
                throw new AddingProblemException("Base station excits already");

            }
        }
        #endregion add base station

        #region update Base Staison
        public void UpdateBaseStaison(int baseStationId, string baseStationName, string chargeSlots)
        {
            IDAL.DO.BaseStation newBaseStation = new IDAL.DO.BaseStation();
            try
            {
                newBaseStation = dalObject.GetBaseStation(baseStationId);
                if (baseStationName != "")
                {
                    newBaseStation.Name = baseStationName;
                }
            }
            catch (IDAL.DO.NonExistingObjectException)
            {
                throw new UpdateProblemException("ID base station  doesnt exists in the system");
            }
           
            if (chargeSlots != "")
                {
                int chargeSlotsTottal, usedChargeSlotsTottal;
                while (!int.TryParse(chargeSlots, out chargeSlotsTottal)) ;
                    usedChargeSlotsTottal = dalObject.GetChargeSlotsList(x => x.StationId == baseStationId).ToList().Count;
                    if ((chargeSlotsTottal - usedChargeSlotsTottal) < 0)
                    {
                        throw new UpdateProblemException("number of charging slotes were not received");
                    }
                    newBaseStation.ChargeSlots = chargeSlotsTottal - usedChargeSlotsTottal;
                }
            dalObject.UpDateBaseStation(newBaseStation);

        }

        #endregion update Base Staison

        #region display base station
        public BaseStation GetBaseStation(int idForDisplayBaseStation)
        {
            IDAL.DO.BaseStation dalBase = new IDAL.DO.BaseStation();
            try
            {
                dalBase = dalObject.GetBaseStation(idForDisplayBaseStation);
            }
            catch (IDAL.DO.NonExistingObjectException)
            {
                throw new GetDetailsProblemException("ID  doesnt exists");
            }
            Location dalBaseStationLoc = new Location() { Longitude = dalBase.Longitude, Latitude = dalBase.Latitude };
            BaseStation blBaseStation = new BaseStation()
            {
                Id = dalBase.Id,
                Name = dalBase.Name,
                BaseStationLocation = dalBaseStationLoc,
                FreeChargeSlots = dalBase.ChargeSlots,
                DroneChargingList = new List<DroneCharging>()
            };
            List<IDAL.DO.DroneCharge> droneInCharge = dalObject.GetChargeSlotsList(i => i.StationId == idForDisplayBaseStation).ToList();
            foreach (var item in droneInCharge)
            {
                blBaseStation.DroneChargingList.Add(new DroneCharging { Id = item.DroneId, Battery = dronesListBL.Find(x => x.Id == item.DroneId).Battery });
            }
            return blBaseStation;
        }
        #endregion display base station

        #region display base station list
        public IEnumerable<BaseStationToList> GetBaseStationList(Predicate<BaseStationToList> predicate = null)
        {
            List<BaseStationToList> baseStationBL = new List<BaseStationToList>();
            List<IDAL.DO.BaseStation> holdDalBaseStation = dalObject.GetBaseStationList().ToList();
            foreach (var item in holdDalBaseStation)
            {
                baseStationBL.Add(new BaseStationToList
                {
                    Id = item.Id,
                    Name = item.Name,
                    FreeChargeSlots = item.ChargeSlots,
                    TakenChargeSlots = dalObject.GetChargeSlotsList(x => x.StationId == item.Id).ToList().Count
                });
            }

            return baseStationBL.FindAll(x => predicate == null ? true : predicate(x));
        }
        #endregion display base station list




    }
}
