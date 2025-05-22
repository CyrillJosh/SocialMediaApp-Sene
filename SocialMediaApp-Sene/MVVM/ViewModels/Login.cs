using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Socialmedia.MVVM.View;
using Microsoft.Maui.Storage;
using SocialMediaApp_Sene;
using System.Diagnostics;
using SocialMediaApp_Sene.MVVM.Models;
using Newtonsoft.Json;
using SocialMediaApp_Sene.Services;

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
        private ErrorService errorServices;


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
            ErrorServices = new ErrorService();
            LoginCommand = new Command(LoginUser);
            TogglePasswordCommand = new Command(TogglePasswordVisibility);
            NavigateToRegisterCommand = new Command(NavigateToRegister);
            OkayCommand = new Command(ErrorServices.Okay);
        }

        //login
        private async void LoginUser()
        {
            //Set activity
            ErrorServices.ShowActivity = true;
            ErrorServices.ActivityIndicator = true;
            ErrorServices.MessageVisible = false;
            ErrorServices.ButtonVisible = false;

            //Check empty fields
            if (string.IsNullOrWhiteSpace(User.Username) || string.IsNullOrWhiteSpace(User.Password))
            {
                ErrorServices.DisplayMessage("Error", "Please enter your username and password.");
                return;
            }


            //MockAPI
            var url = "https://6819ae131ac115563505b710.mockapi.io/Users"; //CY
            //var url = "https://682527810f0188d7e72c2016.mockapi.io/Users"; //CHARLES
            HttpResponseMessage response = await _client.GetAsync(url);

            //response
            if (response.IsSuccessStatusCode)
            {
                string jsonstring = await response.Content.ReadAsStringAsync();
                List<User> users = JsonConvert.DeserializeObject<List<User>>(jsonstring);
                var matchedUser = users.FirstOrDefault(x => x.Username == User.Username && x.Password == User.Password);

                if (matchedUser != null)
                {
                    ErrorServices.DisplayMessage("Success","Login successful!",false);

                    await Task.Delay(1000);
                    UserSession.CurrentUser = matchedUser;

                    Application.Current.MainPage = App.Services.GetRequiredService<AppShell>();
                    ErrorServices.Okay();
                }
                //Invalid
                else
                {
                    ErrorServices.DisplayMessage("Error", "Invalid username or password.");
                    return;
                }
            }
            //unsuccessful
            else
            {
                ErrorServices.DisplayMessage("Error", "An error has occured please try again");
                return;
            }
        }

        //Password
        private void TogglePasswordVisibility()
        {
            EyeIcon = IsPasswordHidden ? "hide.png" : "view.png"; // Fix: Correct icon toggle
            IsPasswordHidden = !IsPasswordHidden;
        }

        //Go to registration
        private void NavigateToRegister()
        {
            Application.Current.MainPage = App.Services.GetRequiredService<RegisterPage>();
        }
    }
}
