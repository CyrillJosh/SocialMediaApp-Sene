using Socialmedia.MVVM.ViewModel;

namespace Socialmedia.MVVM.View;

public partial class Homepage : ContentPage
{
    public Homepage()
    {
        InitializeComponent();
        BindingContext = new HomePageViewModel();
    }
}