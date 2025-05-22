using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using Font = Microsoft.Maui.Font;

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

        [ObservableProperty]
        private bool buttonVisible;

        public void Okay()
        {
            ShowActivity = false;
            MessageVisible = false;
            ActivityIndicator = false;
            ButtonVisible = false;
            Title = "";
            Message = "";
        }

        public void DisplayMessage(string title, string message, bool button = true)
        {
            Title = title;
            Message = message;
            MessageVisible = true;
            ActivityIndicator = false;
            ShowActivity = true;
            ButtonVisible = button;
        }
        public void SetAllFalse()
        {
            ShowActivity = false;
            MessageVisible = false;
            ActivityIndicator = false;
            ButtonVisible = false;
            Title = "";
            Message = "";
        }
    }
}
