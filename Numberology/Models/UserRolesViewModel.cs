using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer;

namespace Numberology.Models
{
    public class SelectedUserRolesModel
    {
        public  UserBLL User { get; set; }
        
        public  SelectedUserRolesModel ()
        {
            Values = new[]
            {
                new SelectListItem { Value = "Guest", Text="Guest"},
                new SelectListItem { Value = "User", Text="User"},
                new SelectListItem { Value = "Contributer", Text="Contributer"},
                new SelectListItem { Value = "Administrator", Text="Administrator"},

            };
          
            
        }
        public string[] SelectedValues { get; set; }
        public void SetSelectedValuesFromUser()
        {
            SelectedValues = User.Roles.Split(' ');
        }
        public void SetRolesFromSelectedValues()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (string s in SelectedValues)
            {
                sb.Append(s);
                sb.Append(' ');
            }
            sb.Remove(sb.Length - 1, 1);
            User.Roles =  sb.ToString();

        }
        
        public IEnumerable<SelectListItem> Values { get; set; }
    }
}