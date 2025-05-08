using System;
using Socialmedia.MVVM.ViewModel;

namespace Socialmedia.MVVM.View;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
    }

    //UI/UX for default text "Birthdate" to display
    private void DateChanged(object sender, DateChangedEventArgs e)
    {
        DateEntry.Text = e.NewDate.ToString("MM/dd/yyyy");

        if (BindingContext is Register viewModel)
            viewModel.User.BirthDate = e.NewDate;
    }
}