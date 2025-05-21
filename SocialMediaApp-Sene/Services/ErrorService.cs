using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SocialMediaApp_Sene.Services
{
    public partial class ErrorService : ObservableObject
    {
        [ObservableProperty]
        private bool activityIndicator;

        [ObservableProperty]
        private bool showActivity;

        [ObservableProperty]
        private string? title;

        [ObservableProperty]
        private string? message;

        [ObservableProperty]
        private bool messageVisible;

        public void Okay()
        {
            ShowActivity = false;
            MessageVisible = false;
            Title = "";
            Message = "";
        }

        public void DisplayError(string title, string message)
        {
            Title = title;
            Message = message;
            MessageVisible = true;
            ActivityIndicator = false;
            ShowActivity = true;
        }
        public async void DisplaySuccess(string message)
        {
            var toast = Toast.Make(message, ToastDuration.Short, 14);
            await toast.Show();

            // Hide any activity indicators or messages, just in case
            ShowActivity = false;
            MessageVisible = false;
            ActivityIndicator = false;
        }
    }
}
