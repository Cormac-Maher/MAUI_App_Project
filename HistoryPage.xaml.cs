using System.Text.Json;

namespace MovieExplorer;

public partial class HistoryPage : ContentPage
{
    public class HistoryItem
    {
        public string Title { get; set; }
        public string Timestamp { get; set; }
    }
    private List<Movies> _allMovies; 
    public HistoryPage()
    {
        InitializeComponent();
        LoadHistory();
    }
    private async void LoadHistory()
    {
        string path = Path.Combine(FileSystem.AppDataDirectory, "history.json");                // history is saved in local app data directory

        if (!File.Exists(path))
            return;

        string json = File.ReadAllText(path);
        List<string> entries = JsonSerializer.Deserialize<List<string>>(json) ?? new();

        _allMovies = await new GetMovies().LoadMoviesAsync();

        foreach (var entry in entries)                                                // Loops through history, splitting each entry into title and timestamp
        {
            var parts = entry.Split('|');
            string title = parts[0];
            string timestamp = parts[1];

            var movie = _allMovies.FirstOrDefault(m => m.title == title);          // find movie in all movies list
            if (movie == null)
                continue;

            var viewButton = new Button                                 // button to open the movie page
            {
                Text = "View",
                BackgroundColor = Colors.Red,
                BorderColor = Colors.Black,
                BorderWidth = 2,
                TextColor = Colors.Black,
                BindingContext = movie
            };
            viewButton.Clicked += OpenMovie_Clicked;

            var border = new Border
            {
                Stroke = Colors.Black,
                BackgroundColor = Colors.Red,                                          // border ui
                StrokeThickness = 2,
                Padding = 10,
                Content = new VerticalStackLayout
                {
                    Children =
                    {
                        new Label { Text = title, FontSize = 25, TextColor = Colors.Black},
                        new Label { Text = "Last viewed: " + timestamp, FontSize = 15, TextColor = Colors.Black},
                        viewButton
                    }
                }
            };
            HistoryList.Children.Add(border);
        }
    }

    private async void OpenMovie_Clicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is Movies movie)
        {
            await Navigation.PushAsync(new MovieContentPage(movie));
        }
    }

    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

}
