using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp_Sene.MVVM.Models
{
        public class User
        {
            //Fields
            public int Id { get; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Gender { get; set; } 
            public DateTime BirthDate { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; } 
            public string Username { get; set; }
            public string Password { get; set; }
        }
}
