using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Numberology.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ReturnURL { get; set; }

        public string Message { get; set; }

        public LoginModel SetMessage(string message)
        {
            Message = message;
            return this;
        }
    }
}