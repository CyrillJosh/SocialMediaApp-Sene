using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Socialmedia.MVVM.View;
using Microsoft.Maui.Storage;
using SocialMediaApp_Sene;
using System.Diagnostics;
using SocialMediaApp_Sene.MVVM.Models;
using Newtonsoft.Json;

namespace Socialmedia.MVVM.ViewModel
{
    public partial class Login : ObservableObject
    {
        //Properties
        [ObservableProperty]
        private User user = new User();

        [ObservableProperty]
        private bool isPasswordHidden = true;

        [ObservableProperty]
        private string eyeIcon = "view.png";

        [ObservableProperty]
        private string errorMessage;

        //[ObservableProperty]
        //private bool activityIndicator;

        //[ObservableProperty]
        //private bool showActivity;

        //[ObservableProperty]
        //private string title;

        //[ObservableProperty]
        //private string message;

        //[ObservableProperty]
        //private bool messageVisible;

        //Commands
        public ICommand LoginCommand { get; }
        public ICommand TogglePasswordCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }

        public ICommand OkayCommand { get; }

        //Client
        private readonly HttpClient _client;

        //Constructor
        public Login()
        {
            _client = new HttpClient();
            LoginCommand = new Command(LoginUser);
            TogglePasswordCommand = new Command(TogglePasswordVisibility);
            NavigateToRegisterCommand = new Command(NavigateToRegister);
            OkayCommand = new Command(Okay);
        }

        private async void LoginUser()
        {
            ShowActivity = true;
            ActivityIndicator = true;
            MessageVisible = false;
            //Empty
            if (string.IsNullOrWhiteSpace(User.Username) || string.IsNullOrWhiteSpace(User.Password))
            {
                DisplayError("Error", "Please enter your username and password.");
            }


            //MockAPI
            var url = "https://6819ae131ac115563505b710.mockapi.io/Users";
            HttpResponseMessage response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonstring = await response.Content.ReadAsStringAsync();
                List<User> users = JsonConvert.DeserializeObject<List<User>>(jsonstring);

                //If exist -- Needs Update -- Check if user with username and password exist in the users list
                if (users.Any(x=> x.Username == User.Username && x.Password == User.Password))
                {
                    ActivityIndicator = false;
                    await Application.Current.MainPage.DisplayAlert("Success", "Login successful!", "OK");
                    Application.Current.MainPage = App.Services.GetRequiredService<Homepage>();
                }
                //Invalid
                else
                {
                    DisplayError("Error", "Invalid username or password.");

                    //ActivityIndicator = false;
                    //await Application.Current.MainPage.DisplayAlert("Error", "Invalid username or password.", "OK");
                }
            }
            else
            {
                //ActivityIndicator = false;
                DisplayError("Error", "An error has occured please try again");
            }
        }

        private void TogglePasswordVisibility()
        {
            EyeIcon = IsPasswordHidden ? "hide.png" : "view.png"; // Fix: Correct icon toggle
            IsPasswordHidden = !IsPasswordHidden;
        }

        private void NavigateToRegister()
        {
            Application.Current.MainPage = App.Services.GetRequiredService<RegisterPage>();
        }

        private void Okay()
        {
            ShowActivity = false;
            MessageVisible = false;
            Title = "";
            Message = "";
            ActivityIndicator = false;
            return;
        }

        private void DisplayError(string title, string message)
        {
            MessageVisible = true;
            Title = title;
            Message = message;
            ActivityIndicator = false;
        }
    }
}
