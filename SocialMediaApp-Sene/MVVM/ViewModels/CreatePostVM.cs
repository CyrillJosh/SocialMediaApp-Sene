using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Socialmedia.MVVM.View;
using Socialmedia.MVVM.ViewModel;
using SocialMediaApp_Sene.MVVM.Models;

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
        public ICommand CreatePostCommand { get; }
        public ICommand CancelCommand { get; }
        public string fullName => $"{App.CurrentUser.Firstname} {App.CurrentUser.Lastname}";

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
            CreatePostCommand = new RelayCommand(async () => await AddPost());
            CancelCommand = new RelayCommand(GoToHomePage);
            //To be debugged adding of images or video
            //AddMediaCommand = new Command(async () => await PickVideoAsync());
            CurrentUser = App.CurrentUser; // You must define this somewhere accessible
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
            var homepage = App.Services.GetRequiredService<Homepage>();
            var vm = homepage.BindingContext as HomePageViewModel;
            if (vm != null)
                await vm.LoadPosts(); // reload latest posts

            Application.Current.MainPage = homepage;
        }

        private async Task AddPost()
        {
            if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Content))
                return;

            var newPost = new Post
            {
                Title = Title,
                Content = Content,
                UserId = CurrentUser.id
            };

            var json = JsonConvert.SerializeObject(newPost);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            //var response = await _client.PostAsync("https://6819ae131ac115563505b710.mockapi.io/Post", content); //Cy
            var response = await _client.PostAsync("https://682527810f0188d7e72c2016.mockapi.io/Post", content);

            if (response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Successfully added.", "OK");
                GoToHomePage();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to create post.", "OK");
            }
        }
    }
}
