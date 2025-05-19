using Microsoft.Maui.Controls;
using SocialMediaApp_Sene;
using System;
using System.Threading.Tasks;

namespace Socialmedia.MVVM.View
{
    public partial class Splash : ContentPage
    {
        public Splash()
        {
            InitializeComponent();
            StartTimer();
        }

        private void StartTimer()
        {
            TimeSpan splashDuration = TimeSpan.FromSeconds(2.4); // gif duration

            Dispatcher.StartTimer(splashDuration, () =>
            {
                Application.Current.MainPage = App.Services.GetRequiredService<LoginPage>();
                return false; // Return false to stop the timer after execution
            });
        }
    }
}
