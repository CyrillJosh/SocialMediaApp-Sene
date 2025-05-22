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

        //Commands
        public ICommand CreatePostCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand OkayCommand { get; }

        //Constructor
        public CreatePostVM()
        {
            ErrorService = new ErrorService();
            CreatePostCommand = new RelayCommand(async () => await AddPost());
            CancelCommand = new RelayCommand(GoToHomePage);
            OkayCommand = new Command(ErrorService.Okay);
        }

        //Return to HomePage
        private async void GoToHomePage()
        {
            var homepage = App.Services.GetRequiredService<Homepage>();
            var vm = homepage.BindingContext as HomePageViewModel;
            if (vm != null)
                await vm.LoadPosts(); // reload latest posts

            Application.Current.MainPage = homepage;
        }

        //AddPost
        private async Task AddPost()
        {
            ErrorService.ShowActivity = true;
            ErrorService.ActivityIndicator = true;
            ErrorService.MessageVisible = false;
            ErrorService.ButtonVisible = false;
            if (string.IsNullOrWhiteSpace(Title))
            {
                ErrorService.DisplayMessage("Error", "Please enter a Title.");
                return;
            }
            var newPost = new Post
            {
                Title = Title,
                Content = Content,
                UserId = UserSession.CurrentUser.id,
                DateCreated = DateTime.Now
            };
            var json = JsonConvert.SerializeObject(newPost);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("https://6819ae131ac115563505b710.mockapi.io/Posts", content);//CY
            //var response = await _client.PostAsync("https://682527810f0188d7e72c2016.mockapi.io/Posts", content);//CHARLES
            if (response.IsSuccessStatusCode)
            {
                ErrorService.DisplayMessage("Success", "Successfully Registered!", false);
                await Task.Delay(1000);
                GoToHomePage();
            }
        }
    }
}
