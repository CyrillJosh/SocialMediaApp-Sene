using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SocialMediaApp_Sene;
using SocialMediaApp_Sene.MVVM.Models;
using SocialMediaApp_Sene.MVVM.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Socialmedia.MVVM.ViewModel
{
    public partial class HomePageViewModel : ObservableObject
    {
        //Properties
        private readonly HttpClient _client;

        [ObservableProperty]
        private ObservableCollection<Post> posts = new ObservableCollection<Post>();

        private List<User> _allUsers;

        //Commands
        public ICommand ToggleFlyoutCommand { get; }
        public ICommand SendMessageCommand { get; }
        public ICommand CloseFlyoutCommand { get; } 
        public ICommand NavigateToAddPostCommand { get; }
        //Constructor
        public HomePageViewModel()
        {
            NavigateToAddPostCommand = new RelayCommand(NavigateToAddPost);
            _client = new HttpClient();
            LoadPosts();
        }

        public async Task LoadPosts()
        {
            Posts.Clear();
            //var response = await httpClient.GetAsync("https://6819ae131ac115563505b710.mockapi.io/Post"); //Cy
            var postResponse = await _client.GetAsync("https://682527810f0188d7e72c2016.mockapi.io/Post");
            var userResponse = await _client.GetAsync("https://682527810f0188d7e72c2016.mockapi.io/Users");

            if (userResponse.IsSuccessStatusCode)
            {
                var userJson = await userResponse.Content.ReadAsStringAsync();
                _allUsers = JsonConvert.DeserializeObject<List<User>>(userJson);
            }

            if (postResponse.IsSuccessStatusCode)
            {
                var postJson = await postResponse.Content.ReadAsStringAsync();
                var listPosts = JsonConvert.DeserializeObject<List<Post>>(postJson);

                foreach (var addedPost in listPosts)
                {
                    var user = _allUsers.FirstOrDefault(u => u.id == addedPost.UserId);
                    addedPost.AuthorName = user != null ? $"{user.Firstname} {user.Lastname}" : "Unknown";
                    Posts.Add(addedPost);
                }
            }
        }

        private void NavigateToAddPost()
        {
            var page = App.Services.GetRequiredService<CreatePost>();
            Application.Current.MainPage = page;
        }
    }
}
