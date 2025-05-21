using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp_Sene.MVVM.Models
{
    public partial class ErrorActivity:ObservableObject
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

        public ErrorActivity()
        {
            ShowActivity = false;
            MessageVisible = false;
            ActivityIndicator = false;
        }
        public void Okay()
        {
            ShowActivity = false;
            MessageVisible = false;
            Title = "";
            Message = "";
        }

        public void DisplayMessage(string title, string message)
        {
            Title = title;
            Message = message;
            MessageVisible = true;
            ActivityIndicator = false;
        }
    }
}
