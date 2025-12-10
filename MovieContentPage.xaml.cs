namespace MovieExplorer;

public partial class MovieContentPage : ContentPage
{
    public MovieContentPage(Movies movie)
    {
        InitializeComponent();
        BindingContext = movie;

        var favourites = Preferences.Get("Favourites", string.Empty).Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();

        if (favourites.Contains(movie.title))
        {
            FavouriteButton.Text = "Remove from Favourites";
        }
        else
        {
            { FavouriteButton.Text = "Mark as Favourite"; }
        }
    }
    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }


    private void Favourite_Clicked(object sender, EventArgs e)           // Method to add or remove movie from favourites
    {
        if (BindingContext is Movies movie)
        {
            var favourites = Preferences.Get("Favourites", string.Empty).Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();   // Puts favourites into a list

            if (favourites.Contains(movie.title))
            {
                favourites.Remove(movie.title);
                Preferences.Set("Favourites", string.Join(" | ", favourites));
                FavouriteButton.Text = "Mark as Favourite";
                DisplayAlert("Removed", $"{movie.title} removed from favourites.", "OK");
            }
            else
            {
                favourites.Add(movie.title);
                Preferences.Set("Favourites", string.Join(" | ", favourites));
                FavouriteButton.Text = "Remove from Favourites";
                DisplayAlert("Added", $"{movie.title} added to favourites!", "OK");
            }
        }
    }
}
