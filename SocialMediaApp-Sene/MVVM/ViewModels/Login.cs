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
        private ErrorActivity errorActivity = new ErrorActivity();
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
            OkayCommand = new Command(ErrorActivity.Okay);
        }

        private async void LoginUser()
        {
            ErrorActivity.ShowActivity = true;
            ErrorActivity.ActivityIndicator = true;
            ErrorActivity.MessageVisible = false;

            if (string.IsNullOrWhiteSpace(User.Username) || string.IsNullOrWhiteSpace(User.Password))
            {
                ErrorActivity.DisplayMessage("Error", "Please enter your username and password.");
                return;
            }


            //MockAPI
            //var url = "https://6819ae131ac115563505b710.mockapi.io/Users"; //CY
            var url = "https://682527810f0188d7e72c2016.mockapi.io/Users";
            HttpResponseMessage response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonstring = await response.Content.ReadAsStringAsync();
                List<User> users = JsonConvert.DeserializeObject<List<User>>(jsonstring);
                var matchedUser = users.FirstOrDefault(x => x.Username == User.Username && x.Password == User.Password);

                //If exist -- Needs Update -- Check if user with username and password exist in the users list
                //if (users.Any(x => x.Username == User.Username && x.Password == User.Password))
                //{
                //    //ActivityIndicator = false;
                //    App.CurrentUser =

                //    iErrorHandlingService.DisplayMessage("Success", "Login successful!");
                //    Application.Current.MainPage = App.Services.GetRequiredService<Homepage>();
                //}
                if (matchedUser != null)
                {
                    App.CurrentUser = matchedUser; // ✅ Assign the logged-in user globally

                    ErrorActivity.DisplayMessage("Success", "Login successful!");
                    Application.Current.MainPage = App.Services.GetRequiredService<Homepage>();
                }
                //Invalid
                else
                {
                    ErrorActivity.DisplayMessage("Error", "Invalid username or password.");
                    user.Password = "";
                    return;
                }
            }
            else
            {
                //iErrorHandlingService.DisplayMessage("Error", "An error has occured please try again");
                return;
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
    }
}
