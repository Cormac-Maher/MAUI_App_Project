

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
            await button.RotateTo(720, 400);                  // Amimates button when pressed
            await button.ScaleTo(1.0, 300);
            button.Rotation = 0;

            Color colour = button.BackgroundColor;
            App.Current.Resources["AppBackgroundColor"] = colour;               // Resource AppBackgroundColor is used to set background colour
        }
    }

    private async void OnFavouritesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FavouritesPage());                       //Goes to favourites page
    }
    private async void OnChangeName(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NamePage());                            // Goes to name page
    }

    private async void OnClearFavouritesClicked(object sender, EventArgs e)
    {
        string path = Path.Combine(FileSystem.AppDataDirectory, "favourites.json");             //When the user clears favourites the favourites json file is deleted
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        await DisplayAlert("", "All favourites have been removed.", "OK");
    }

    private async void OnHistoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HistoryPage());
    }

    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
