using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Numberology;
using System.Web.Mvc;

namespace Numberology.Models
{
    public class DropDownViewForRelatedNumbers
    {
       public BusinessLogicLayer.RelatedNumberBLL relatedNumber { get; set; }
       public IEnumerable<SelectListItem> AllParentValues { get; set; }
       public int SelectedParentID { get; set; }
      // public string SelectedParentName { get; set; }

       public void SetValues(List<BusinessLogicLayer.NumberBLL> numbers )
        {
            List<SelectListItem> rv  = new List<SelectListItem>();
            foreach(var number in numbers)
            {
                SelectListItem item = new SelectListItem();
                item.Value = number.ID.ToString();
                item.Text = number.Name;
                rv.Add(item);
            }
            AllParentValues = rv;
        }
    }
}