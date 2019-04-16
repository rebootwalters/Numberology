using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class RiddleBLL
    {
        public RiddleBLL()
        {

        }

        internal RiddleBLL(  DataAccessLayer.RiddleDAL r)
        {
            RiddleID = r.RiddleID;
            Riddle = r.Riddle;
            Answer = r.Answer;
        }

        public int RiddleID { get; set; }
        public string Riddle { get; set; }
        public string Answer { get; set; }

        internal List<NumberBLL> _numbers;

        public List<NumberBLL> Numbers
        {
            get
            {
                if (_numbers == null)
                {
                    throw new Exception("You must use a BLLContext to load the related items into this number before trying to read them: use logic like this ctx.LoadNumbersIntoRiddle(number);");
                }
                // the list of related records is loaded into this item through the context by calling
                // ctx.LoadRelatedNumbersIntoNumber(number);
                return _numbers;
            }
        }





    }
}
