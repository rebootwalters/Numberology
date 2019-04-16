using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
   public class UserDAL
    {
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
