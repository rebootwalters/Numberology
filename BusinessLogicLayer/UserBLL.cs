using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class UserBLL
    {
        public UserBLL()
        {

        }

        

        internal UserBLL( UserDAL u)
        {
            UserID = u.UserID;
            EMailAddress = u.EMailAddress;
            Name = u.Name;
            Password = u.Password;
            Salt = u.Salt;
            Roles = u.Roles;
            Verified = u.Verified;
            Comments = u.Comments;

        }

        public int UserID { get; set; }
        public string EMailAddress { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Roles { get; set; }
        public string Verified { get; set; }
        public string Comments { get; set; }
    }
}
