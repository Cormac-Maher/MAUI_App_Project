using Microsoft.Maui.Graphics.Win2D;

namespace MovieExplorer;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    private void OnColourSelected(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            Color colour = button.BackgroundColor;
            App.Current.Resources["AppBackgroundColor"] = colour;
            DisplayAlert("", "Updated Background Colour!", "OK");
        }
    }

    private void OnGridSelected(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            Color colour = button.BackgroundColor;
            App.Current.Resources["GridBackgroundColor"] = colour;
            DisplayAlert("", "Updated Grid Background Colour!", "OK");
        }
    }


    private async void OnFavouritesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FavouritesPage());
    }
    private async void OnChangeName(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NamePage());
    }
    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
