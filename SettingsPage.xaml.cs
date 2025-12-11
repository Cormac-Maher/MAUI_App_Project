namespace MovieExplorer;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }
    private void OnColorSelected(object sender, EventArgs e)
    {
        var selectedColor = ColorPicker.SelectedItem?.ToString();

        if (!string.IsNullOrEmpty(selectedColor))
        {
            Preferences.Set("BackgroundColor", selectedColor);
            Application.Current.Resources["AppBackgroundColor"] = Color.FromArgb(selectedColor);
        }
    }

    private async void OnFavouritesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FavouritesPage());
    }
}
