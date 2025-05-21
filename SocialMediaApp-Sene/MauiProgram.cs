 using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Socialmedia.MVVM.View;
using Socialmedia.MVVM.ViewModel;
using SocialMediaApp_Sene.MVVM.Views;
using SocialMediaApp_Sene.Services;

namespace SocialMediaApp_Sene
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()

                //Initialize the .NET MAUI Community Toolkit MediaElement by adding the below line of code
                .UseMauiCommunityToolkitMediaElement()
                .UseMauiCommunityToolkit()
                // After initializing the .NET MAUI Community Toolkit, optionally add additional fonts

                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<ErrorService>();

            builder.Services.AddSingleton<Splash>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<RegisterPage>();
            builder.Services.AddSingleton<Homepage>();
            builder.Services.AddSingleton<CreatePost>();

            return builder.Build();
        }
    }
}
