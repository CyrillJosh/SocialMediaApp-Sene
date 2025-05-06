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
            if (string.IsNullOrWhiteSpace(User.Firstname) ||
                string.IsNullOrWhiteSpace(User.Lastname) ||
                string.IsNullOrWhiteSpace(User.Email) ||
                string.IsNullOrWhiteSpace(User.PhoneNumber) ||
                string.IsNullOrWhiteSpace(User.Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please fill all fields.", "OK");
                return;
            }

            //Phone number length
            if (User.PhoneNumber.All(char.IsDigit) && User.PhoneNumber.Length <= 11)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Phone number must be exactly 11 digits.", "OK");
                return;
            }   

            //Email format
            if (!User.Email.EndsWith("@gmail.com"))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Email must end with '@gmail.com'.", "OK");
                return;
            }

            //Password format
            if (User.Password.Length < 8 || !User.Password.Any(char.IsDigit) || !User.Password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Password must be at least 8 characters long, contain a number, and a special character.", "OK");
                return;
            }

            //Confirm password
            if (User.Password != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            // Save user details for login validation - change to mock api
            Preferences.Set("RegisteredEmail", User.Email);
            Preferences.Set("RegisteredPassword", User.Password);
            Preferences.Set("RegisteredPhone", User.PhoneNumber);
            Preferences.Set("RegisteredBirthdate", User.BirthDate);
            Preferences.Set("IsRegistered", true);

            await Application.Current.MainPage.DisplayAlert("Success", "Registration complete!", "OK");
            await Application.Current.MainPage.Navigation.PopAsync(); // Navigate back to Login
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
