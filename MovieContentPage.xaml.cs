//using Android.Graphics;
using System.Text.Json;

namespace MovieExplorer;

public partial class MovieContentPage : ContentPage
{
    private Movies _movie;

    public MovieContentPage(Movies movie)
    {
        InitializeComponent();
        _movie = movie;
        BindingContext = _movie;


        string path = Path.Combine(FileSystem.AppDataDirectory, "favourites.json");
        List<string> favourites = new();

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            favourites = JsonSerializer.Deserialize<List<string>>(json) ?? new();
        }

        var entry = favourites.FirstOrDefault(f => f.StartsWith(movie.title + "|"));
        if (entry != null)
        {
            FavouriteButton.Text = "Remove from Favourites";
            string[] parts = entry.Split('|');
            if (parts.Length == 2)
            {
                FavouriteTimestamp.Text = $"Favourited on {parts[1]}";
            }
        }

        else
        {
            FavouriteButton.Text = "Mark as Favourite";
            FavouriteTimestamp.Text = "";
        }
    }

    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirm Delete",
            $"Are you sure you want to delete {_movie.title}?",
            "Yes", "No");

        if (confirm)
        {
            if (Navigation.NavigationStack.FirstOrDefault() is MainPage mainPage)
            {
                mainPage._allMovies.Remove(_movie);
                mainPage.CreateTheGrid(mainPage._allMovies);
                string path = Path.Combine(FileSystem.AppDataDirectory, "moviesemoji.json");
                string updatedJson = JsonSerializer.Serialize(mainPage._allMovies);
                await File.WriteAllTextAsync(path, updatedJson);

            }
            
        }
        await Navigation.PopAsync();
    } 
    private async void Favourite_Clicked(object sender, EventArgs e)           // Method to add or remove movie from favourites
    {
        if (BindingContext is Movies movie)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, "favourites.json");
            List<string> favourites = new();

            if (File.Exists(path))
            {
                string json = await File.ReadAllTextAsync(path);
                favourites = JsonSerializer.Deserialize<List<string>>(json) ?? new();
            }

            var existing = favourites.FirstOrDefault(f => f.StartsWith(movie.title + "|"));
            if (existing != null)
            {
                favourites.Remove(existing);
                FavouriteButton.Text = "Mark as Favourite";
                FavouriteTimestamp.Text = "";
                await DisplayAlert("Removed", $"{movie.title} removed from favourites.", "OK");
            }
            else
            {
                string entry = $"{movie.title}|{DateTime.Now}";
                favourites.Add(entry);
                FavouriteButton.Text = "Remove from Favourites";
                FavouriteTimestamp.Text = $"Favourited on {DateTime.Now}";
                await DisplayAlert("Added", $"{movie.title} added to favourites!", "OK");
            }

            string updatedJson = JsonSerializer.Serialize(favourites);
            await File.WriteAllTextAsync(path, updatedJson);
        }
    }
}
