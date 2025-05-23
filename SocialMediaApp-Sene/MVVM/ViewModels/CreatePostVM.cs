using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Socialmedia.MVVM.View;
using Socialmedia.MVVM.ViewModel;
using SocialMediaApp_Sene.MVVM.Models;
using SocialMediaApp_Sene.MVVM.Views;
using SocialMediaApp_Sene.Services;

namespace SocialMediaApp_Sene.MVVM.ViewModels
{
    public partial class CreatePostVM : ObservableObject
    {
        //Feilds
        private readonly HttpClient _client = new HttpClient();
        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string content;

        [ObservableProperty]
        private User currentUser;

        [ObservableProperty]
        private ErrorService errorService;

        private Post editingPost;
        
        //Commands
        public ICommand CreatePostCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand OkayCommand { get; }

        //Constructor
        public CreatePostVM()
        {
            ErrorService = new ErrorService();
            CurrentUser = UserSession.CurrentUser;
            CreatePostCommand = new Command(async () => await SavePost());
            CancelCommand = new RelayCommand(GoToHomePage);
            OkayCommand = new Command(ErrorService.Okay);
        }

        public void LoadPostForEdit(Post post)
        {
            if (post == null) return;
            editingPost = post;
            Title = post.Title;
            Content = post.Content;
            // Load other fields if needed
        }

        private async Task SavePost()
        {
            ErrorService.StartActivity();

            if (string.IsNullOrWhiteSpace(Title))
            {
                ErrorService.DisplayMessage("Error", "Please enter a Title.");
                return;
            }

            if (editingPost == null)
            {
                // Create new post
                var newPost = new Post
                {
                    Title = Title,
                    Content = Content,
                    UserId = CurrentUser.id,
                    DateCreated = DateTime.Now
                };

                var json = JsonConvert.SerializeObject(newPost);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("https://682527810f0188d7e72c2016.mockapi.io/Post", content);

                if (response.IsSuccessStatusCode)
                {
                    ErrorService.DisplayMessage("Success", "Post created successfully!", false);
                    await Task.Delay(1000);
                    GoToHomePage();
                }
            }
            else
            {
                // Update existing post
                editingPost.Title = Title;
                editingPost.Content = Content;

                var json = JsonConvert.SerializeObject(editingPost);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                //var url = $"https://682527810f0188d7e72c2016.mockapi.io/Post/{editingPost.Id}";//Charles
                var url = $"https://6819ae131ac115563505b710.mockapi.io/Posts/{editingPost.Id}";//CY
                var response = await _client.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    ErrorService.DisplayMessage("Success", "Post updated successfully!", false);
                    await Task.Delay(1000);
                    GoToHomePage();
                }
                else
                {
                    ErrorService.DisplayMessage("Error", "An error has occured please try again");
                }
            }
        }

        private async void GoToHomePage()
        {
            var homepage = App.Services.GetRequiredService<AppShell>();
            var vm = homepage.BindingContext as HomePageViewModel;
            if (vm != null)
                await vm.LoadPosts(); // reload latest posts

            Application.Current.MainPage = homepage;
        }

        //private async Task AddPost()
        //{
        //    ErrorService.ShowActivity = true;
        //    ErrorService.ActivityIndicator = true;
        //    ErrorService.MessageVisible = false;
        //    if (string.IsNullOrWhiteSpace(Title))
        //    {
        //        ErrorService.DisplayMessage("Error", "Please enter a Title.");
        //        return;
        //    }
        //    var newPost = new Post
        //    {
        //        Title = Title,
        //        Content = Content,
        //        UserId = UserSession.CurrentUser.id,
        //        AuthorName = $"{UserSession.CurrentUser.Firstname} {UserSession.CurrentUser.Lastname}"
        //    };
        //    var json = JsonConvert.SerializeObject(newPost);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");
        //    var response = await _client.PostAsync("https://682527810f0188d7e72c2016.mockapi.io/Post", content);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        ErrorService.DisplayMessage("Success", "Successfully Registered!", false);
        //        await Task.Delay(1000);
        //        GoToHomePage();
        //    }
        //}
    }
}
