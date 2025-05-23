using SocialMediaApp_Sene.MVVM.ViewModels;

namespace SocialMediaApp_Sene.MVVM.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ProfileVM vm)
        {
            await vm.LoadUserProfile();
        }
    }

}