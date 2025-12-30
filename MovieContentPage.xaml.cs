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
        SaveToHistory(movie);

        string path = Path.Combine(FileSystem.AppDataDirectory, "favourites.json");                     // i made json file for favourites in local data directory.
        List<string> favourites = new();                                                    // List to hold favourite movies

        if (File.Exists(path))              // Checks is the json file exists
        {
            string json = File.ReadAllText(path);                                   // reads the json file
            favourites = JsonSerializer.Deserialize<List<string>>(json) ?? new();       // deserializes the json content into the list
        }

        var entry = favourites.FirstOrDefault(f => f.StartsWith(movie.title + "|"));    // Checks if the movie is in the fav list
        if (entry != null)
        {
            string[] parts = entry.Split('|');                                        // if movie is favourited:
            if (parts.Length == 2)
            {
                FavouriteButton2.Source = "fullheart.png";
                FavouriteTimestamp.Text = $"Favourited on {parts[1]}";
            }
        }

        else                                                            // if movie is not favourited:
        {
            FavouriteButton2.Source = "emptyheart.png";
            FavouriteTimestamp.Text = "";
        }
    }

    private async void Back_Clicked(object sender, EventArgs e)         //back button   
    {
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
                favourites = JsonSerializer.Deserialize<List<string>>(json) ?? new();      // deserializes the json content
            }

            var existing = favourites.FirstOrDefault(f => f.StartsWith(movie.title + "|"));
            if (existing != null)
            {
                favourites.Remove(existing);
                FavouriteButton2.Source = "emptyheart.png";                       // If movie is already in favourites, it is removed and heart is emptied
                FavouriteTimestamp.Text = "";
            }
            else
            {
                string entry = $"{movie.title}|{DateTime.Now}";            // Adds movie to favourites with timestamp
                favourites.Add(entry);
                FavouriteButton2.Source = "fullheart.png";
                FavouriteTimestamp.Text = $"Favourited on {DateTime.Now}";      
                await FavouriteButton2.ScaleTo(0.8, 80);                            // Animations for the heart
                await FavouriteButton2.ScaleTo(1.2, 80); 
                await FavouriteButton2.ScaleTo(1.0, 80);
                await Task.Delay(400);
            }

            string updatedJson = JsonSerializer.Serialize(favourites);                // Serializes the updated list back to json
            await File.WriteAllTextAsync(path, updatedJson);                        // the updated json goes back to the file
        }
    }

    private void SaveToHistory(Movies movie)
    {
        string path = Path.Combine(FileSystem.AppDataDirectory, "history.json"); 
        List<string> history = new(); 
        if (File.Exists(path)) 
        { 
            string json = File.ReadAllText(path); 
            history = JsonSerializer.Deserialize<List<string>>(json) ?? new(); 
        } 
        var existing = history.FirstOrDefault(h => h.StartsWith(movie.title + "|")); 
        if (existing != null) 
        { 
            history.Remove(existing);
        }
        string entry = $"{movie.title}|{DateTime.Now}"; 
        history.Insert(0, entry); 

        string updatedJson = JsonSerializer.Serialize(history);
        File.WriteAllText(path, updatedJson); 
    }


    }
