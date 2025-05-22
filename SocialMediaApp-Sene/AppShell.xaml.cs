using Socialmedia.MVVM.View;
using SocialMediaApp_Sene.MVVM.Models;
using SocialMediaApp_Sene.MVVM.Views;

namespace SocialMediaApp_Sene
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("home", typeof(Homepage));
            Routing.RegisterRoute("profile", typeof(ProfilePage));
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            // Optional: clear session
            UserSession.CurrentUser = null;
            // Redirect to login page
            Application.Current.MainPage = App.Services.GetRequiredService<LoginPage>();
        }

    }
}
