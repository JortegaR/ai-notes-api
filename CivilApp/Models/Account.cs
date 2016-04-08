using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CivilApp.Models
{
    public class Account
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string password { get; set; }
        public bool IsPersisten { get; set; }

        public string OldUrl { get; set; }
    }
}