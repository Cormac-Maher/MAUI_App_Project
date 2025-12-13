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
            DisplayAlert("", "Updated background colour!", "OK");
        }
    }

    private async void OnFavouritesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FavouritesPage());
    }
}
