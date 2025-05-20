using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp_Sene.Services
{
    public partial class ErrorHandlingService : ObservableObject
    {
        [ObservableProperty]
        private bool activityIndicator;

        [ObservableProperty]
        private bool showActivity;

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private bool messageVisible;
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
