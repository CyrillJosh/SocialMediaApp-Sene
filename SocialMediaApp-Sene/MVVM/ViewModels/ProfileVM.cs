using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
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
        private ObservableCollection<PostVM> userPosts;
        //Client
        private readonly HttpClient _client;
        public ProfileVM()
        {
            _client = new HttpClient();
        }
        public async Task LoadUserProfile()
        {
            CurrentUser = UserSession.CurrentUser;


            var PostResponse = await _client.GetAsync("https://6819ae131ac115563505b710.mockapi.io/Posts"); //Cy
            var UserResponse = await _client.GetAsync("https://6819ae131ac115563505b710.mockapi.io/Users"); //Cy
            if (UserResponse.IsSuccessStatusCode && PostResponse.IsSuccessStatusCode)
            {
                string jsonUser = await UserResponse.Content.ReadAsStringAsync();
                string jsonPost = await PostResponse.Content.ReadAsStringAsync();
                List<User> users = JsonConvert.DeserializeObject<List<User>>(jsonUser);
                List<Post> posts = JsonConvert.DeserializeObject<List<Post>>(jsonPost);
                int currentUserId = UserSession.CurrentUser.id;
                CurrentUser = users.FirstOrDefault(u => u.id == currentUserId);

                foreach(var p in posts.Where(p => p.UserId == currentUser.id))
                {
                    UserPosts.Add(new PostVM()
                    {
                        User = CurrentUser,
                        Post = p,
                        IsEditable = p.UserId == CurrentUser.id ? true : false
                    });
                }
            }
        }
    }
}
