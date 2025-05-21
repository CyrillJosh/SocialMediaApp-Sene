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
        private readonly HttpClient _client = new HttpClient();

        [ObservableProperty]
        private ObservableCollection<Post> posts = new ObservableCollection<Post>();

        //Commands
        public ICommand ToggleFlyoutCommand { get; }
        public ICommand SendMessageCommand { get; }
        public ICommand CloseFlyoutCommand { get; } 
        public ICommand NavigateToAddPostCommand { get; }
        //Constructor
        public HomePageViewModel()
        {
            NavigateToAddPostCommand = new RelayCommand(NavigateToAddPost);
            LoadPosts();
        }

        public async Task LoadPosts()
        {
            Posts.Clear();
            var httpClient = new HttpClient();
            //var response = await httpClient.GetAsync("https://6819ae131ac115563505b710.mockapi.io/Post"); //Cy
            var url = "https://682527810f0188d7e72c2016.mockapi.io/Post";
            HttpResponseMessage response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var listPosts = JsonConvert.DeserializeObject<List<Post>>(json);
                foreach (var addedPost in listPosts)
                {
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
