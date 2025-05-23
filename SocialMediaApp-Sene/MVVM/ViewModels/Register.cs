using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Microsoft.Maui.Storage;
using SocialMediaApp_Sene;
using Socialmedia.MVVM.View;
using SocialMediaApp_Sene.MVVM.Models;
using Newtonsoft.Json;
using System.Text;
using SocialMediaApp_Sene.Services;

namespace Socialmedia.MVVM.ViewModel
{
    public partial class Register : ObservableObject
    {
        //Properties
        [ObservableProperty]
        private User user = new User();

        public DateTime datenow { get; } = DateTime.Now;

        [ObservableProperty]
        private string confirmPassword;

        [ObservableProperty]
        private bool isPasswordHidden = true;

        [ObservableProperty]
        private bool isConfirmPasswordHidden = true;

        [ObservableProperty]
        private string eyeIcon = "view.png";

        [ObservableProperty]
        private string confirmEyeIcon = "view.png";

        [ObservableProperty]
        private ErrorService errorServices;

        //Client
        private readonly HttpClient _client;

        //Commands
        public ICommand ToggleConfirmPasswordCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand NavigateToLoginCommand { get; }
        public ICommand TogglePasswordCommand { get; }
        public ICommand OpenDatePickerCommand { get; }
        public ICommand OkayCommand { get; }

        //Constructor
        public Register()
        {
            _client = new HttpClient();
            ErrorServices = new ErrorService();
            OkayCommand = new Command(ErrorServices.Okay);
            RegisterCommand = new RelayCommand(Registers);
            NavigateToLoginCommand = new RelayCommand(NavigateToLogin);
            TogglePasswordCommand = new RelayCommand(TogglePasswordVisibility);
            ToggleConfirmPasswordCommand = new RelayCommand(ToggleConfirmPasswordVisibility);
        }

        //Register
        private async void Registers()
        {
            ErrorServices.ShowActivity = true;
            ErrorServices.ActivityIndicator = true;
            ErrorServices.MessageVisible = false;
            //Empty
            if (string.IsNullOrWhiteSpace(User.Firstname) ||
                string.IsNullOrWhiteSpace(User.Lastname) ||
                string.IsNullOrWhiteSpace(User.Username) ||
                string.IsNullOrWhiteSpace(User.Email) ||
                string.IsNullOrWhiteSpace(User.PhoneNumber) ||
                string.IsNullOrWhiteSpace(User.BirthDate.ToString()) ||
                string.IsNullOrWhiteSpace(User.Password) ||
                string.IsNullOrWhiteSpace(User.Gender) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ErrorServices.DisplayMessage("Error", "Please fill all fields.");
                return;
            }

            //Phone number length
            if (User.PhoneNumber.All(char.IsDigit) && User.PhoneNumber.Length < 11)
            {
                ErrorServices.DisplayMessage("Error", "Phone number must be exactly 11 digits.");
                return;
            }   

            //Email format
            if (!User.Email.EndsWith("@gmail.com"))
            {
                ErrorServices.DisplayMessage("Error", "Email must end with '@gmail.com'.");
                return;
            }
            //var url= "https://6819ae131ac115563505b710.mockapi.io/Users" //CY
            var url = "https://682527810f0188d7e72c2016.mockapi.io/Users";
            HttpResponseMessage chkuser = await _client.GetAsync(url);

            if (chkuser.IsSuccessStatusCode)
            {
                string jsonstring = await chkuser.Content.ReadAsStringAsync();
                List<User> users = JsonConvert.DeserializeObject<List<User>>(jsonstring);
                //Username Exist
                if (users.Any(x => x.Username == User.Username))
                {
                    ErrorServices.DisplayMessage("Error", "Username already exists please try another");
                    return;
                }
            }
            //Connection Error
            else
            {
                ErrorServices.DisplayMessage("Error", "An error has occured please try again");
            }

            //Password format
            if (User.Password.Length < 8 || !User.Password.Any(char.IsDigit) || !User.Password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                ErrorServices.DisplayMessage("Error", "Password must be at least 8 characters long, contain a number, and a special character.");
                return;
            }

            //Confirm password
            if (User.Password != ConfirmPassword)
            {
                ErrorServices.DisplayMessage("Error", "Passwords do not match.");
                return;
            }

            var json = JsonConvert.SerializeObject(User);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync(url, content);
            //Registration Success
            if(response.IsSuccessStatusCode)
            {
                ErrorServices.DisplayMessage("Success","Successfully Registered!",false);
                Application.Current.MainPage = App.Services.GetRequiredService<LoginPage>(); // Navigate back to Login
            }
            //unsuccessful
            else
            {
                ErrorServices.DisplayMessage("Error", "An error has occured please try again");
            }
        }

        //Custom Methods
        //Go to Login
        private void NavigateToLogin()
        {
            Application.Current.MainPage = App.Services.GetRequiredService<LoginPage>();
        }

        //Password
        private void TogglePasswordVisibility()
        {
            EyeIcon = IsPasswordHidden ? "hide.png" : "view.png";
            IsPasswordHidden = !IsPasswordHidden;
        }

        //ConfirmPassword
        private void ToggleConfirmPasswordVisibility()
        {
            ConfirmEyeIcon = IsConfirmPasswordHidden ? "hide.png" : "view.png";
            IsConfirmPasswordHidden = !IsConfirmPasswordHidden;
        }
    }
}
