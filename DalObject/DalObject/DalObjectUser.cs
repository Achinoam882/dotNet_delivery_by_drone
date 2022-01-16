using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;
using Dal;
using DalObject;


namespace Dal
{
    partial class DalObject : IDal
    {
        #region User
        /// <summary>
        ///  Receives user name and returns the appropriate object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUser(string id)
        {
            var user = DataSource.Users.Find(u => u.UserName == id);
            if (user != null)
                if (user.DelUser == false)
                    return user;
            throw new NonExistingObjectException();
        }
        /// <summary>
        /// The function returns all users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetUsers()
        {
            return from User in DataSource.Users
                   where User.DelUser == true//???
                   select User;
        }
        /// <summary>
        /// /The function receives an object user to add
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user)
        {
            var userIndex = DataSource.Users.FindIndex(u => u.UserName == user.UserName);
            if (userIndex != -1)
                if (DataSource.Users[userIndex].DelUser == true)
                {
                    DataSource.Users[userIndex].DelUser = false;
                    return;
                }
                else
                    throw new DO.AddExistingObjectException();
            DataSource.Users.Add(user);
        }
        /// <summary>
        /// The method receives an object user to update
        /// </summary>
        /// <param name="user"></param>
        public void UpdatUser(User user)
        {
            var toUpdateIndex = DataSource.Users.FindIndex(u => u.UserName == user.UserName);
            if (toUpdateIndex != -1)
                if (DataSource.Users[toUpdateIndex].DelUser == false)
                    DataSource.Users[toUpdateIndex] = user;
                else
                    throw new NonExistingObjectException();
            else
                throw new NonExistingObjectException();
        }
        /// <summary>
        /// The method gets an object user to delete
        /// </summary>
        /// <param name="user"></param>
        public void DeleteUser(User user)
        {
            var toDeleteIndex = DataSource.Users.FindIndex(u => u.UserName == user.UserName);
            if (toDeleteIndex != -1)
                if (DataSource.Users[toDeleteIndex].DelUser == false)
                    DataSource.Users[toDeleteIndex].DelUser = true;
                else
                    throw new NonExistingObjectException();
            else
                throw new NonExistingObjectException();
        }

        #endregion
    }
}
