using Microsoft.Maui.Graphics.Win2D;

namespace MovieExplorer;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    private async void OnColourSelected(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            await button.ScaleTo(2, 150);  
            await button.RotateTo(720, 400);  
            await button.ScaleTo(1.0, 300);   
            button.Rotation = 0;

            Color colour = button.BackgroundColor;
            App.Current.Resources["AppBackgroundColor"] = colour;
            await DisplayAlert("", "Updated Background Colour!", "OK");
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
