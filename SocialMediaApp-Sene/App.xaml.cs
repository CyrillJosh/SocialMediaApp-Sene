using Socialmedia.MVVM.View;

namespace SocialMediaApp_Sene
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; set; }
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            Services = serviceProvider;
            MainPage = new Splash();
        }
    }
}
