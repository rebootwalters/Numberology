using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class NumberBLL
    {
        #region Constructors and other stuff
  
        
        // a default constructor is REQUIRED to support MVC Model Binding
        // because of the DAL Constructor, the default constructor must be explicitly defined
        public NumberBLL()
        {
            
        }

        // the mapping between the DAL and the BLL is done through this constructor.
        // the default constuctor must be defined explicity because of the existance of this constructor
        internal NumberBLL( DataAccessLayer.NumberDAL numberDAL)
        {
        
            ID = numberDAL.ID;
            Name = numberDAL.Name;
            Doublestuff = numberDAL.Doublestuff;
            Floatstuff = numberDAL.Floatstuff;

        }

        #endregion

        
        public int ID { get;  set; }
        


        public string Name { get; set; }
        public double Doublestuff { get; set; }
        public float  Floatstuff { get; set; }

        internal List<RelatedNumberBLL> _relatedNumbers= null;
        public List<RelatedNumberBLL> RelatedNumbers
        {
            get
            {
                if (_relatedNumbers == null)
                {
                    throw new Exception("You must use a BLLContext to load the related items into this number before trying to read them: use logic like this ctx.LoadRelatedNumbersIntoNumber(number);");
                }
                // the list of related records is loaded into this item through the context by calling
                // ctx.LoadRelatedNumbersIntoNumber(number);
                return _relatedNumbers;
            }
        }

        internal List<RiddleBLL> _riddles = null;
        public List<RiddleBLL> Riddles
        {
            get
            {
                if (_riddles == null)
                {
                    throw new Exception("You must use a BLLContext to load the related riddle items into this number before trying to read them: use logic like this ctx.LoadRiddlesIntoNumber(number);");
                }
                // the list of related records is loaded into this item through the context by calling
                // ctx.LoadRelatedNumbersIntoNumber(number);
                return _riddles;
            }
        }


    }
}
