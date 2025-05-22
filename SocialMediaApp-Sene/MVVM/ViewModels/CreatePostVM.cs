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
        private readonly HttpClient _client = new HttpClient();
        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string content;

        [ObservableProperty]
        private User currentUser;

        [ObservableProperty]
        private ErrorService errorService;
        public ICommand CreatePostCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand OkayCommand { get; }

        //To be debugged adding of images or video
        //public ICommand AddMediaCommand { get; }
        //private string _videoPath;
        //public string VideoPath
        //{
        //    get => _videoPath;
        //    set
        //    {
        //        _videoPath = value;
        //        OnPropertyChanged();
        //    }
        //}

        public CreatePostVM()
        {
            ErrorService = new ErrorService();
            CreatePostCommand = new RelayCommand(async () => await AddPost());
            CancelCommand = new RelayCommand(GoToHomePage);
            //To be debugged adding of images or video
            //AddMediaCommand = new Command(async () => await PickVideoAsync());
            OkayCommand = new Command(ErrorService.Okay);
        }

        //To be debugged adding of images or video
        //private async Task PickVideoAsync()
        //{
        //    var res = await FilePicker.PickAsync(new PickOptions
        //    {
        //        PickerTitle = "Pick a video",
        //        FileTypes = FilePickerFileType.Videos
        //    });

        //    if (res != null)
        //    {
        //        VideoPath = res.FullPath;
        //    }
        //}
        private async void GoToHomePage()
        {
            var homepage = App.Services.GetRequiredService<AppShell>();
            var vm = homepage.BindingContext as HomePageViewModel;
            if (vm != null)
                await vm.LoadPosts(); // reload latest posts

            Application.Current.MainPage = homepage;
        }

        private async Task AddPost()
        {
            ErrorService.ShowActivity = true;
            ErrorService.ActivityIndicator = true;
            ErrorService.MessageVisible = false;
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
                AuthorName = $"{UserSession.CurrentUser.Firstname} {UserSession.CurrentUser.Lastname}"
            };
            var json = JsonConvert.SerializeObject(newPost);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("https://682527810f0188d7e72c2016.mockapi.io/Post", content);
            if (response.IsSuccessStatusCode)
            {
                ErrorService.DisplayMessage("Success", "Successfully Registered!", false);
                await Task.Delay(1000);
                GoToHomePage();
            }
        }
    }
}
