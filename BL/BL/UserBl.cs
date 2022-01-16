using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
{
    public partial class BL

    {
        #region  User
    
        /// <summary>
        /// A function that receives a DO type User  object and returns a BO type User
        /// </summary>
        /// <param name="UserDO"></param>
        /// <returns></returns>
        public User UserAdapter(DO.User UserDO)
        {
            BO.User UserBO = new User();
            UserDO.CopyPropertiesTo(UserBO);
            return UserBO;
        }
        /// <summary>
        /// A function that receives a BO type user object and returns a DO type user
        /// </summary>
        /// <param name="UserBO"></param>
        /// <returns></returns>
        public DO.User UserAdapter(BO.User UserBO)
        {
            DO.User UserDO = new DO.User();
            UserBO.CopyPropertiesTo(UserDO);
            return UserDO;
        }
        /// <summary>
        /// The function Receives an object user and enters the database
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user)
        {
            try
            {
                dalObject.AddUser(UserAdapter(user));

            }
            catch (DO.NonExistingObjectException )
            {
                throw new BO.AddingProblemException(" ID cant be a negative number");
            }
        }
        /// <summary>
        /// The function receives a User for updating
        /// </summary>
        public void UpdateUser(User user)
        {
            try
            {
                dalObject.UpdatUser(UserAdapter(user));
            }
            catch (DO.NonExistingObjectException)
            {
                throw new BO.UpdateProblemException("User doesnt exists in the system"); ;
            }
        }
        /// <summary>
        /// The function Receives an object user to Deletion
        /// </summary>
        /// <param name="user"></param>
        public void DeleteUser(User user)
        {
            try
            {
                dalObject.DeleteUser(UserAdapter(user));
            }
            catch (DO.NonExistingObjectException)
            {
                throw new UpdateProblemException("User has been deleted already");
            }
        }
        /// <summary>
        /// The function returns all user
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetUsers()
        {
            return from User in dalObject.GetUsers()
                   select UserAdapter(User);
        }
        /// <summary>
        /// The function receives an ID number and returns the corresponding object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUser(string  id)
        {
            try
            {
                return UserAdapter(dalObject.GetUser(id));
            }
            catch (DO.NonExistingObjectException)
            {
                throw new BO.GetDetailsProblemException("User doesnt exists in the system");
            }
        }
        #endregion
    }
}
