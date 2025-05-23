
using System.Windows.Input;
using Socialmedia.MVVM.View;
using Socialmedia.MVVM.ViewModel;
using SocialMediaApp_Sene.MVVM.Models;
using SocialMediaApp_Sene.MVVM.Views;

namespace SocialMediaApp_Sene
{
    public partial class AppShell : Shell
    {
        public ICommand LogoutCommand { get; }

        public AppShell()
        {
            InitializeComponent();
            LogoutCommand = new Command(Logout);
            BindingContext = this;
        }

        private void Logout()
        {
            var newVm = App.Services.GetRequiredService<Login>();
            var loginPage = App.Services.GetRequiredService<LoginPage>();
            loginPage.BindingContext = newVm;
            Application.Current.MainPage = loginPage;

        }
    }
}
