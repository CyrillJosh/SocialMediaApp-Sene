using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Microsoft.Maui.Storage;
using SocialMediaApp_Sene;
using Socialmedia.MVVM.View;

namespace Socialmedia.MVVM.ViewModel
{
    public partial class Register : ObservableObject
    {
        //Properties
        [ObservableProperty]
        private string fullName;

        [ObservableProperty]
        private string email;

        private string pnum;
        public string Pnum
        {
            get => pnum;
            set
            {
                // Allow only numbers and limit to 11 digits
                if (value.All(char.IsDigit) && value.Length <= 11)
                {
                    SetProperty(ref pnum, value);
                }
            }
        }

        [ObservableProperty]
        private string birthdate;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string confirmPassword;

        [ObservableProperty]
        private bool isPasswordHidden = true;

        [ObservableProperty]
        private bool isConfirmPasswordHidden = true;

        [ObservableProperty]
        private string eyeIcon = "Show";

        [ObservableProperty]
        private string confirmEyeIcon = "Show";
        //Commands
        public ICommand ToggleConfirmPasswordCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand NavigateToLoginCommand { get; }
        public ICommand TogglePasswordCommand { get; }
        public ICommand OpenDatePickerCommand { get; }
        //Constructor
        public Register()
        {
            RegisterCommand = new RelayCommand(Registers);
            NavigateToLoginCommand = new RelayCommand(NavigateToLogin);
            TogglePasswordCommand = new RelayCommand(TogglePasswordVisibility);
            ToggleConfirmPasswordCommand = new RelayCommand(ToggleConfirmPasswordVisibility);
           
        }

        private async void Registers()
        {
            //Empty
            if (string.IsNullOrWhiteSpace(FullName) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Pnum) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please fill all fields.", "OK");
                return;
            }

            //Phone number length
            if (Pnum.Length != 11)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Phone number must be exactly 11 digits.", "OK");
                return;
            }   

            //Email format
            if (!Email.EndsWith("@gmail.com"))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Email must end with '@gmail.com'.", "OK");
                return;
            }

            //Password format
            if (Password.Length < 8 || !Password.Any(char.IsDigit) || !Password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Password must be at least 8 characters long, contain a number, and a special character.", "OK");
                return;
            }

            //Confirm password
            if (Password != ConfirmPassword)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            // Save user details for login validation - change to mock api
            Preferences.Set("RegisteredEmail", Email);
            Preferences.Set("RegisteredPassword", Password);
            Preferences.Set("RegisteredPhone", Pnum);
            Preferences.Set("RegisteredBirthdate", Birthdate);
            Preferences.Set("IsRegistered", true);

            await App.Current.MainPage.DisplayAlert("Success", "Registration complete!", "OK");
            await App.Current.MainPage.Navigation.PopAsync(); // Navigate back to Login
        }


        //Custom Methods
        private void NavigateToLogin()
        {
            Application.Current.MainPage = App.Services.GetRequiredService<LoginPage>();
        }

        private void TogglePasswordVisibility()
        {
            IsPasswordHidden = !IsPasswordHidden;
            EyeIcon = IsPasswordHidden ? "Show" : "Hide";
        }

        private void ToggleConfirmPasswordVisibility()
        {
            IsConfirmPasswordHidden = !IsConfirmPasswordHidden;
            ConfirmEyeIcon = IsConfirmPasswordHidden ? "Show" : "Hide";
        }
    }
}
