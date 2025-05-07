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
        private string eyeIcon = "Show";

        [ObservableProperty]
        private string errorMessage;

        //Commands
        public ICommand LoginCommand { get; }
        public ICommand TogglePasswordCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }

        //Client
        private readonly HttpClient _client;

        //Constructor
        public Login()
        {
            _client = new HttpClient();
            LoginCommand = new RelayCommand(LoginUser);
            TogglePasswordCommand = new RelayCommand(TogglePasswordVisibility);
            NavigateToRegisterCommand = new RelayCommand(NavigateToRegister);
        }

        private async void LoginUser()
        {
            //Empty
            if (string.IsNullOrWhiteSpace(User.Email) || string.IsNullOrWhiteSpace(User.Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter both email and password.", "OK");
                return;
            }

            //Gmail format
            if (!User.Email.EndsWith("@gmail.com"))
            {
                await Application.Current.MainPage.DisplayAlert("Invalid Email", "Email must end with '@gmail.com'.", "OK");
                return;
            }

            //MockAPI
            var url = "https://6819ae131ac115563505b710.mockapi.io/Users";
            HttpResponseMessage response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonstring = await response.Content.ReadAsStringAsync();
                List<User> users = JsonConvert.DeserializeObject<List<User>>(jsonstring);

                //If exist -- Needs Update -- Check if user with username and password exist in the users list
                if (users.Any(x=> x.Email == User.Email && x.Password == User.Password))
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Login successful!", "OK");

                    Application.Current.MainPage = App.Services.GetRequiredService<Homepage>();
                }
                //Invalid
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Invalid email or password.", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An erro has occured please try again", "OK");
            }
        }

        private void TogglePasswordVisibility()
        {
            IsPasswordHidden = !IsPasswordHidden;
            EyeIcon = IsPasswordHidden ? "show.png" : "hide.png"; // Fix: Correct icon toggle
        }

        private void NavigateToRegister()
        {
            Application.Current.MainPage = App.Services.GetRequiredService<RegisterPage>();
        }
    }
}
