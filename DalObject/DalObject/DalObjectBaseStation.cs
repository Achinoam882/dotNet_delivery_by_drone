﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;
using DalObject;
namespace Dal
{
     partial class DalObject:IDal
    {
        #region add base station
        /// <summary>
        /// The function adds a station to the list of Basestations.
        /// </summary>
        /// <param name="newbaseStation"></param>
        public void SetBaseStation(BaseStation newBaseStation)
        {
            if (DataSource.BaseStationList.FindIndex(x => x.Id == newBaseStation.Id) != -1)
            {
                throw new AddExistingObjectException();
            }
            DataSource.BaseStationList.Add(newBaseStation);
        }
        #endregion add base station


        #region print base station
        /// <summary>
        /// The function returns the selected base station.
        /// </summary>
        /// <param name="idForAllObjects">Id of a selected BaseStation </param>
        /// <returns> return empty ubjact if its not there</returns>
        public BaseStation GetBaseStation(int idForAllObjects)
        {
            if (!(DataSource.BaseStationList.Exists(x => x.Id == idForAllObjects)))
            {
                throw new NonExistingObjectException();
            }
            return DataSource.BaseStationList.Find(x => x.Id == idForAllObjects);
        }
        #endregion print base station


        #region print base station list
        /// <summary>
        /// The function returns an array of all base stations.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<BaseStation> GetBaseStationList(Predicate<BaseStation> BaseStationpredicate = null)
        {
            return DataSource.BaseStationList.FindAll(x => BaseStationpredicate == null ? true : BaseStationpredicate(x)).ToList();
        }
        #endregion add base station list


        #region print free charge slots base station list
        /// <summary>
        /// The function returns base stations with free charge positions.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<BaseStation> GetBaseStationFreeChargeSlots()
        {
            return DataSource.BaseStationList.FindAll(x => x.ChargeSlots > 0).ToList();
        }
        #endregion print free charge slots base station list


        #region to update a BaseStation
        /// <summary>
        /// The function update name anf charge slots of base stations .
        /// </summary>
        public void UpDateBaseStation(BaseStation newBaseStation)
        {
            if (!(DataSource.BaseStationList.Exists(x => x.Id == newBaseStation.Id)))
                throw new NonExistingObjectException();
            DataSource.BaseStationList[DataSource.BaseStationList.FindIndex(x => x.Id == newBaseStation.Id)] = newBaseStation;
        }
        #endregion to update a BaseStation

        #region to update less Charge Slots
        /// <summary>
        /// The function update less charge slots in base station.
        /// </summary>
        public void LessChargeSlots(int baseStationId)
        {

            int indexaforBaseStationId = DataSource.BaseStationList.FindIndex(x => x.Id == baseStationId);
            BaseStation temp = DataSource.BaseStationList[indexaforBaseStationId];
            temp.ChargeSlots--;
            DataSource.BaseStationList[indexaforBaseStationId] = temp;

        }
        #endregion to update  less Charge Slots



        #region to update more Charge Slots
        /// <summary>
        /// The function update more charge slots in base station.
        /// </summary>
        public void MoreChargeSlots(int baseStationId)
        {

            BaseStation temp = GetBaseStation(baseStationId);
            temp.ChargeSlots++;
            DataSource.BaseStationList[DataSource.BaseStationList.FindIndex(x => x.Id == baseStationId)] = temp;

        }
        #endregion to update more Charge Slots
    }


}
