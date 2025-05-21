using Socialmedia.MVVM.View;
using SocialMediaApp_Sene.MVVM.Models;

namespace SocialMediaApp_Sene
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; set; }
        public static User CurrentUser { get; set; }

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            Services = serviceProvider;
            MainPage = Services.GetRequiredService<Splash>();
        }
    }
}
