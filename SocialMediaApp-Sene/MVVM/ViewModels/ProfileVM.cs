using CommunityToolkit.Mvvm.ComponentModel;
using SocialMediaApp_Sene.MVVM.Models;
using SocialMediaApp_Sene.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp_Sene.MVVM.ViewModels
{
    public partial class ProfileVM : ObservableObject
    {
        [ObservableProperty]
        private User currentUser;

        [ObservableProperty]
        private ObservableCollection<Post> userPosts;
        public ProfileVM()
        {
            LoadUserProfile();
        }
        private void LoadUserProfile()
        {
            // Get current logged-in user from session
            CurrentUser = UserSession.CurrentUser;

            // Assuming you have a Posts collection accessible in HomePageViewModel or via a service
            // For demo, assume you fetch all posts from some service or static store
            var allPosts = PostService.GetAllPosts(); // Replace with your actual data source

            // Filter posts by current user only
            var posts = allPosts.Where(p => p.UserId == CurrentUser.id).ToList();

            UserPosts = new ObservableCollection<Post>(posts);
        }
    }
}
