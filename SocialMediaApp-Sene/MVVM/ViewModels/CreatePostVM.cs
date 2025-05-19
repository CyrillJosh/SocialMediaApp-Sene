using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Socialmedia.MVVM.View;

namespace SocialMediaApp_Sene.MVVM.ViewModels
{
    public class CreatePostVM:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ICommand CreatePostCommand { get; }
        public ICommand ToHomePageCommand { get; }
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
            CreatePostCommand = new RelayCommand(AddPost);
            ToHomePageCommand = new RelayCommand(GoToHomePage);
            //To be debugged adding of images or video
            //AddMediaCommand = new Command(async () => await PickVideoAsync());
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
        private void GoToHomePage()
        {
            Application.Current.MainPage = App.Services.GetRequiredService<Homepage>();
        }

        private void AddPost()
        {
            Application.Current.MainPage = App.Services.GetRequiredService<Homepage>();
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
