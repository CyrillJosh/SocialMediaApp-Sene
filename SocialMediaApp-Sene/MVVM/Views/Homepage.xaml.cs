using Socialmedia.MVVM.ViewModel;

namespace Socialmedia.MVVM.View;

public partial class Homepage : ContentPage
{
    public Homepage()
    {
        InitializeComponent();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is HomePageViewModel vm)
        {
            await vm.LoadPosts();
        }
    }
}