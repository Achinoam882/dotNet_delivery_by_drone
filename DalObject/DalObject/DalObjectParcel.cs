﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;
using DAL;
using DalObject;



namespace Dal
{
   partial class DalObject :IDal
    {
        #region add parcel
        /// <summary>
        /// The function adds a Parcel to the list of Parcels.
        /// </summary>
        /// <param name="newParcel"></param>
        /// <returns></returns>
        public int SetParcel(Parcel newParcel)
        {
           

            newParcel.Id = DataSource.Config.IdParcel++;
            DataSource.ParcelList.Add(newParcel);
            return newParcel.Id;

        }
        #endregion add parcel

        #region print parcel
        /// <summary>
        /// The function returns the selected Parcel.
        /// </summary>
        /// <param name="idForAllObjects">Id of a selected Parcel</param>
        /// <returns>return empty ubjact if its not there</returns>
        public Parcel GetParcel(int idForAllObjects)
        {
            if ((DataSource.ParcelList.FindIndex(x => x.Id == idForAllObjects)==-1))
            {
                throw new NonExistingObjectException();
            }
            return DataSource.ParcelList.Find(x => x.Id == idForAllObjects);
        }

        #endregion print parcel

        #region to set a parcel to drone
        /// <summary>
        /// The function assigns a package to the drone.
        /// </summary>
        /// <param name="parcelId">Id of Parcel</param>
        /// <param name="droneId">Id of drone</param>
        public void SetParcelToDrone(int parcelId, int droneId)
        {
            if (!(DataSource.ParcelList.Exists(x => x.Id == parcelId)))
            {
                throw new NonExistingObjectException();
            }
            int parcelIndex = DataSource.ParcelList.FindIndex(x => x.Id == parcelId);
            Parcel temp = DataSource.ParcelList[parcelIndex];
            temp.DroneId = droneId;
            temp.Scheduled = DateTime.Now;
            DataSource.ParcelList[parcelIndex] = temp;

            if (!(DataSource.DroneList.Exists(x => x.Id == droneId)))
            {
                throw new NonExistingObjectException();
            }
            int droneIndex = DataSource.DroneList.FindIndex(x => x.Id == droneId);
            Drone droneTemp = DataSource.DroneList[droneIndex];
            DataSource.DroneList[droneIndex] = droneTemp;
        }
        #endregion to set a parcel to drone

        #region  picking up a parcel
        /// <summary>
        /// picked up package by the drone.
        /// </summary>
        /// <param name="parcelId">Id of Parcel</param>
        public void PickUpParcel(int parcelId)
        {
            if (!(DataSource.ParcelList.Exists(x => x.Id == parcelId)))
            {
                throw new NonExistingObjectException();
            }
            int parcelIndex = DataSource.ParcelList.FindIndex(x => x.Id == parcelId);
            Parcel temp = DataSource.ParcelList[parcelIndex];
            temp.PickUp = DateTime.Now;
            DataSource.ParcelList[parcelIndex] = temp;
        }
        #endregion  picking up a parcel

        #region parcel list
        /// <summary>
        /// The function returns an array of all Parcel.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<Parcel> GetParcelList(Predicate<Parcel>parcelpredicate=null)
        {
            return DataSource.ParcelList.FindAll(x => parcelpredicate == null ? true : parcelpredicate(x)).ToList();
        }
        #endregion parcel list

        #region free parcel list
        /// <summary>
        /// The function returns an array of all packages not associated with the Drone.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<Parcel> GetFreeParcelList()
        {
            return DataSource.ParcelList.FindAll(x => x.DroneId == 0).ToList();
        }
        #endregion free parcel list

        #region deliver to customer
        /// <summary>
        /// delivery package to the customer.
        /// </summary>
        /// <param name="parcelId">Id of Parcel</param>
        public void DeliverToCustomer(int parcelId)
        {
            if (!(DataSource.ParcelList.Exists(x => x.Id == parcelId)))
            {
                throw new NonExistingObjectException();
            }
            int parcelIndex = DataSource.ParcelList.FindIndex(x => x.Id == parcelId);
            Parcel temp = DataSource.ParcelList[parcelIndex];
            temp.Delivered = DateTime.Now;
            DataSource.ParcelList[parcelIndex] = temp;
            // DataSource.ParcelList[parcelIndex].Delivered= DateTime.Now;
        }
        #endregion deliver to customer

       
        #region to delete a parcel
        // <summary>
        ///The function delete an exists parcel.
        /// </summary>
        /// <param name="parcelId">Id of Parcel</param>
        public void DeleteParcel(int parcelId)
        {
            int index = DataSource.ParcelList.FindIndex(x => x.Id == parcelId);
            if (index == -1)
                throw new NonExistingObjectException();
            DataSource.ParcelList.RemoveAt(index);
        }
        #endregion to delete a parcel
    }
}
