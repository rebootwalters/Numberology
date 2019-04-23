using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class RelatedNumberBLL
    {
        #region Constructors and other stuff
       
        public   RelatedNumberBLL()
        {
         
        }
        
        internal RelatedNumberBLL( DataAccessLayer.RelatedNumberDAL relatedNumberDAL)
        {
          
            ID = relatedNumberDAL.ID;
            RelatedName = relatedNumberDAL.RelatedName;
            RelatedLanguage = relatedNumberDAL.RelatedLanguage;
            ParentNumberID = relatedNumberDAL.ParentNumberID;

        }

        internal RelatedNumberBLL(NumberBLL Parent, DataAccessLayer.RelatedNumberDAL relatedNumberDAL)
        {
            _parentNumber = Parent;
            ID = relatedNumberDAL.ID;
            RelatedName = relatedNumberDAL.RelatedName;
            RelatedLanguage = relatedNumberDAL.RelatedLanguage;
            ParentNumberID = relatedNumberDAL.ParentNumberID;

        }
        #endregion


        public int ID { get;  set; }
        public int ParentNumberID { get;  set; }
        


        public string RelatedName { get; set; }
        public string RelatedLanguage { get; set; }
  

       internal NumberBLL _parentNumber;
        public NumberBLL ParentNumber
        {
            get
            {
                if (_parentNumber == null)
                {
                    throw new Exception("You must use a BLLContext object to set the Parent before trying to read from it:  use logic like this   ctx.LoadParentIntoRelatedNumber(relatednumber);");
                }
                return _parentNumber;
            }
            set
            {
                _parentNumber = value;
                ParentNumberID = value.ID;
            }
        }

    }
}
