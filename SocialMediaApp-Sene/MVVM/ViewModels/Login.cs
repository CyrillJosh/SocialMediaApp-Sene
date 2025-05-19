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
            if (string.IsNullOrWhiteSpace(User.Username) || string.IsNullOrWhiteSpace(User.Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter your username and password.", "OK");
                return;
            }

  
            //MockAPI
            var url = "https://682527810f0188d7e72c2016.mockapi.io/Users";
            HttpResponseMessage response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonstring = await response.Content.ReadAsStringAsync();
                List<User> users = JsonConvert.DeserializeObject<List<User>>(jsonstring);

                //If exist -- Needs Update -- Check if user with username and password exist in the users list
                if (users.Any(x=> x.Username == User.Username && x.Password == User.Password))
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Login successful!", "OK");

                    Application.Current.MainPage = App.Services.GetRequiredService<Homepage>();
                }
                //Invalid
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Invalid username or password.", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error has occured please try again", "OK");
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
