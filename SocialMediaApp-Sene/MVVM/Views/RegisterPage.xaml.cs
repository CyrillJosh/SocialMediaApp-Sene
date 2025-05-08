using Socialmedia.MVVM.ViewModel;

namespace Socialmedia.MVVM.View;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
    }

    private void DateChanged(object sender, DateChangedEventArgs e)
    {
        DateEntry.Text = datePicker.Date.ToString("mm/dd/yyyy");
    }
}