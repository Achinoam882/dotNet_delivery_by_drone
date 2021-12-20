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
                throw new AddingProblemException("קוד תחנה לא יכול להיות מספר שלילי");
            if (newBaseStation.FreeChargeSlots < 0)
                throw new AddingProblemException("מספר עמדות פנויות לא יכול להיות מספר שלילי");
            if (newBaseStation.BaseStationLocation.Longitude < 34.3 || newBaseStation.BaseStationLocation.Latitude > 35.5)
                throw new AddingProblemException("המיקום שנבחר לא נמצא בגבולות הארץ");
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
            catch (IDAL.DO.AddExistingObjectException ex)
            {
                throw new AddingProblemException("קוד תחנה זה כבר קיים במערכת", ex);

            }
        }
        #endregion add base station

        #region update Base Staison
        public void UpdateBaseStaison(int baseStationId, string baseStationName, string chargeSlots)
        {
            try
            {
                int chargeSlotsTottal, usedChargeSlotsTottal;
                IDAL.DO.BaseStation newBaseStation = dalObject.GetBaseStation(baseStationId);
                if (baseStationName != "")
                    newBaseStation.Name = baseStationName;
                if (chargeSlots != "")
                {
                    while (!int.TryParse(chargeSlots, out chargeSlotsTottal)) ;
                    usedChargeSlotsTottal = dalObject.GetChargeSlotsList(x => x.StationId == baseStationId).ToList().Count;
                    if ((chargeSlotsTottal - usedChargeSlotsTottal) < 0)
                    {
                        throw new UpdateProblemException("מספר עמדות טעינה לא התקבל");
                    }
                    newBaseStation.ChargeSlots = chargeSlotsTottal - usedChargeSlotsTottal;
                    dalObject.UpDateBaseStation(newBaseStation);

                }
            }
            catch (IDAL.DO.NonExistingObjectException ex)
            {
                throw new UpdateProblemException("קוד תחנה זה לא קיים במערכת", ex);
            }
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
                throw new GetDetailsProblemException("קוד זה לא קיים");
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
                blBaseStation.DroneChargingList.ToList().Add(new DroneCharging { Id = item.DroneId, Battery = dronesListBL.Find(x => x.Id == item.DroneId).Battery });
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
