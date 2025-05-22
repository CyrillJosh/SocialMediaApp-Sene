using SocialMediaApp_Sene.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp_Sene.Services
{
    public static class PostService
    {
        //List
        private static List<Post> _posts = new List<Post>();

        public static List<Post> GetAllPosts() => _posts;

        public static void SetPosts(List<Post> posts)
        {
            _posts = posts;
        }
    }
}
