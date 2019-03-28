using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class RelatedNumberDAL
    {
        public int ID { get; set; }
        public string RelatedName { get; set; }
        public string RelatedLanguage { get; set; }
        public int ParentNumberID { get; set; }


    }


}
