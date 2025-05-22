using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SocialMediaApp_Sene;
using SocialMediaApp_Sene.MVVM.Models;
using SocialMediaApp_Sene.MVVM.ViewModels;
using SocialMediaApp_Sene.MVVM.Views;
using SocialMediaApp_Sene.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Socialmedia.MVVM.ViewModel
{
    public partial class HomePageViewModel : ObservableObject
    {
        //Properties
        private readonly HttpClient _client;

        [ObservableProperty]
        private ObservableCollection<PostVM> posts = new ObservableCollection<PostVM>();

        [ObservableProperty]
        private ErrorService errorServices;

        private List<User> _allUsers = new();

        //Commands
        public ICommand ToggleFlyoutCommand { get; }
        public ICommand SendMessageCommand { get; }
        public ICommand CloseFlyoutCommand { get; } 
        public ICommand NavigateToAddPostCommand { get; }
        //Constructor
        public HomePageViewModel()
        {
            NavigateToAddPostCommand = new RelayCommand(NavigateToAddPost);
            ErrorServices = new ErrorService();
            _client = new HttpClient();
            LoadPosts();
        }

        //Load posts
        public async Task LoadPosts()
        {
            //clear
            Posts.Clear();
            var postResponse = await _client.GetAsync("https://6819ae131ac115563505b710.mockapi.io/Post"); //Cy
            var userResponse = await _client.GetAsync("https://6819ae131ac115563505b710.mockapi.io/Post"); //Cy
            //var postResponse = await _client.GetAsync("https://682527810f0188d7e72c2016.mockapi.io/Post"); //CHARLES
            //var userResponse = await _client.GetAsync("https://682527810f0188d7e72c2016.mockapi.io/Users"); //CHARLES

            //Get User
            if (userResponse.IsSuccessStatusCode)
            {
                var userJson = await userResponse.Content.ReadAsStringAsync();
                _allUsers = JsonConvert.DeserializeObject<List<User>>(userJson);
            }
            //unsuccessful
            else
            {
                ErrorServices.DisplayMessage("Error", "An error has occured please try again");
                return;
            }
            //Get Posts
            if (postResponse.IsSuccessStatusCode)
            {
                var postJson = await postResponse.Content.ReadAsStringAsync();
                var listPosts = JsonConvert.DeserializeObject<List<Post>>(postJson);

                foreach (var addedPost in listPosts)
                {
                    var user = _allUsers.FirstOrDefault(u => u.id == addedPost.UserId);
                    Posts.Add(new PostVM()
                    {
                        User = user,
                        Post = addedPost
                    });
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
