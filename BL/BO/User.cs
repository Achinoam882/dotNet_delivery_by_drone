using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class User
    {

        /// <summary>
        /// sets and gets
        /// </summary>
            public int Id { get; set; }
            public int Salt { get; set; }
            public string HashedPassword { get; set; }
            public string UserName { get; set; }//users Name
            public bool AllowingAccess { get; set; }//User access
            public bool DelUser { set; get; }//A flage for Deletion
            public string password { set; get; }
            public override string ToString() => this.ToStringProperty();
        
    }
}
