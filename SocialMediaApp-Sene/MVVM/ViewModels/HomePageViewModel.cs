using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SocialMediaApp_Sene;
using SocialMediaApp_Sene.MVVM.Models;
using SocialMediaApp_Sene.MVVM.ViewModels;
using SocialMediaApp_Sene.MVVM.Views;
using SocialMediaApp_Sene.Services;
using System.Collections.ObjectModel;
using System.Text;
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
        private ErrorService errorService;

        private List<User> _allUsers = new();

        [ObservableProperty]
        private User currentUser;

        //Commands
        public ICommand ToggleFlyoutCommand { get; }
        public ICommand SendMessageCommand { get; }
        public ICommand CloseFlyoutCommand { get; } 
        public ICommand NavigateToAddPostCommand { get; }
        public ICommand DeletePostCommand { get; }
        public ICommand EditPostCommand { get; }
        //Constructor
        public HomePageViewModel()
        {
            CurrentUser = UserSession.CurrentUser;
            ErrorService = new ErrorService();
            NavigateToAddPostCommand = new RelayCommand(NavigateToAddPost);
            DeletePostCommand = new Command<PostVM>(async (post) => await DeletePost(post));
            EditPostCommand = new Command<PostVM>(async (post) => await NavigateToEditPage(post));
            _client = new HttpClient();
        }

        //Load Posts
        public async Task LoadPosts()
        {
            //clear
            Posts.Clear();
            var postResponse = await _client.GetAsync("https://6819ae131ac115563505b710.mockapi.io/Posts"); //Cy
            var userResponse = await _client.GetAsync("https://6819ae131ac115563505b710.mockapi.io/Users"); //Cy
            //var postResponse = await _client.GetAsync("https://682527810f0188d7e72c2016.mockapi.io/Post"); //CHARLES
            //var userResponse = await _client.GetAsync("https://682527810f0188d7e72c2016.mockapi.io/Users"); //CHARLES

            if (userResponse.IsSuccessStatusCode && postResponse.IsSuccessStatusCode)
            {
                string jsonUser = await userResponse.Content.ReadAsStringAsync();
                string jsonPost = await postResponse.Content.ReadAsStringAsync();
                _allUsers = JsonConvert.DeserializeObject<List<User>>(jsonUser);
                List<Post> listPosts = JsonConvert.DeserializeObject<List<Post>>(jsonPost);

                foreach (var addedPost in listPosts.Where(p=>p.Status!=false))
                {
                    var user = _allUsers.FirstOrDefault(u => u.id == addedPost.UserId);
                    Posts.Add(new PostVM()
                    {
                        User = user,
                        Post = addedPost,
                        IsEditable = user.id == CurrentUser.id ? true : false
                    });
                }
                //Sort by date
                Posts.ToList().OrderByDescending(x => x.Post.DateCreated).ToObservableCollection();
            }
            else
                ErrorService.DisplayMessage("Error", "Please enter your username and password.");
        }

        //Delete Post
        private async Task DeletePost(PostVM post)
        {
            ErrorService.StartActivity();
            //Soft Deletion
            post.Post.Status = false;
            var json = JsonConvert.SerializeObject(post.Post);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            //var postsUrl = $"https://682527810f0188d7e72c2016.mockapi.io/Post/{post.Post.id}";//Charles
            var postsUrl = $"https://6819ae131ac115563505b710.mockapi.io/Posts/{post.Post.id}";//CY
            var response = await _client.PatchAsync(postsUrl, content);
            if (response.IsSuccessStatusCode)
            {
                ErrorService.DisplayMessage("Success", "Deleted successfully!", false);
                await Task.Delay(500);
                ErrorService.Okay();
                await LoadPosts(); // refresh list
            }
            else
            {
                ErrorService.DisplayMessage("Error", "An error has occured please try again");
            }
        }

        private async Task NavigateToEditPage(PostVM post)
        {
            var page = App.Services.GetRequiredService<CreatePost>();
            if (page.BindingContext is CreatePostVM vm)
            {
                vm.LoadPostForEdit(post.Post);
            }
            Application.Current.MainPage = page;

        }
        private void NavigateToAddPost()
        {
            var page = App.Services.GetRequiredService<CreatePost>();
            if (page.BindingContext is CreatePostVM vm)
            {
                vm.LoadPostForEdit(null); // clear for new post
            }
            Application.Current.MainPage = page;
        }
    }
}
