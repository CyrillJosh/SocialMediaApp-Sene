using CommunityToolkit.Mvvm.ComponentModel;
using SocialMediaApp_Sene.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp_Sene.MVVM.ViewModels
{
    public class PostVM
    {
        //Fields
        public User User { get; set; }
        public Post Post { get; set; }

        public bool IsEditable { get; set; }
    }
}
