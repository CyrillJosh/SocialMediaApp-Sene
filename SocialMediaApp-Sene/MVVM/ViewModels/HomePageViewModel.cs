using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SocialMediaApp_Sene;
using SocialMediaApp_Sene.MVVM.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Socialmedia.MVVM.ViewModel
{
    public partial class HomePageViewModel : ObservableObject
    {
        //Properties
        [ObservableProperty]
        private double flyoutMenuPosition = -250; // Hidden initially

        public ObservableCollection<string> Messages { get; set; } = new ObservableCollection<string>();

        [ObservableProperty]
        private string messageText;

        //Commands
        public ICommand ToggleFlyoutCommand { get; }
        public ICommand SendMessageCommand { get; }
        public ICommand CloseFlyoutCommand { get; } 
        public ICommand NavigateToAddPostCommand { get; }
        //Constructor
        public HomePageViewModel()
        {
            ToggleFlyoutCommand = new RelayCommand(ToggleFlyoutMenu);
            SendMessageCommand = new RelayCommand(SendMessage);
            CloseFlyoutCommand = new RelayCommand(CloseFlyout); 
            NavigateToAddPostCommand = new RelayCommand(NavigateToAddPost);
        }

        private void CloseFlyout()
        {
            FlyoutMenuPosition = -250; // Hide the flyout menu
        }
        private void ToggleFlyoutMenu()
        {
            FlyoutMenuPosition = (FlyoutMenuPosition == 0) ? -250 : 0; // Slide in/out
        }

        private void SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(MessageText))
            {
                Messages.Add(MessageText);
                MessageText = string.Empty;
            }
        }

        private void NavigateToAddPost()
        {
            var page = App.Services.GetRequiredService<CreatePost>();
            Application.Current.MainPage = page;
        }
    }
}
