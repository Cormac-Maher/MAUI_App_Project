using System.Text.Json;

namespace MovieExplorer;

public partial class FavouritesPage : ContentPage
{ 
	private List<Movies> _allMovies = new();
    private readonly GetMovies _movieService = new GetMovies();

    public FavouritesPage()
	{
		InitializeComponent();

        LoadFavourites();
    }
    private async void LoadFavourites()
    {
        _allMovies = await _movieService.LoadMoviesAsync();

        string path = Path.Combine(FileSystem.AppDataDirectory, "favourites.json");
        List<string> favourites = new();                                                                      // finds favourites and loads grid with only favourite movies

        if (File.Exists(path))
        {
            string json = await File.ReadAllTextAsync(path);
            favourites = JsonSerializer.Deserialize<List<string>>(json) ?? new();
        }

        var favouriteMovies = _allMovies.Where(m => favourites.Any(f => f.StartsWith(m.title + "|"))).ToList();

        CreateTheGrid(favouriteMovies);
    }
    private void CreateTheGrid(List<Movies> movies)                                                 // Same code as MainPage.xaml.cs to create the grid and search bar
    {
        GridPageContent.RowDefinitions.Clear();
        GridPageContent.ColumnDefinitions.Clear();
        GridPageContent.Children.Clear();

        int columns = 2;
        int rows = (int)Math.Ceiling((double)movies.Count / columns);

        for (int r = 0; r < rows; r++)
            GridPageContent.AddRowDefinition(new RowDefinition());

        for (int c = 0; c < columns; c++)
            GridPageContent.AddColumnDefinition(new ColumnDefinition { Width = GridLength.Star });    

        int i = 0;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (i >= movies.Count) break;
                {
                    var movie = movies[i];


                    VerticalStackLayout info = new()
                    {
                        Spacing = 2,
                        BindingContext = movie
                    };

                    info.Children.Add(CreateLabel("title", 38));
                    info.Children.Add(CreateLabel("year", 18));

                    var genreLabel = new Label
                    {
                        FontSize = 18,
                        TextColor = Colors.Black,
                        Text = movie.genre == null ? "" : string.Join(", ", movie.genre)
                    };
                    info.Children.Add(genreLabel);
                    info.Children.Add(CreateLabel("director", 18));
                    info.Children.Add(CreateLabel("rating", 18));
                    info.Children.Add(CreateLabel("emoji", 18));

                    Border styledBorder = new Border
                    {
                        BackgroundColor = Colors.Red,
                        Stroke = Colors.Black,
                        Padding = 10,
                        Content = info
                    };

                    var tapGesture = new TapGestureRecognizer();    
                    tapGesture.Tapped += async (s, e) =>
                    {
                        await Navigation.PushAsync(new MovieContentPage(movie));    
                    };
                    styledBorder.GestureRecognizers.Add(tapGesture); 

                    GridPageContent.Add(styledBorder, c, r);
                    i++;
                }
            }
        }
    }
    private Label CreateLabel(string property, int fontSize)
    {
        var label = new Label
        {
            FontSize = fontSize,
            TextColor = Colors.Black,
        };
        label.SetBinding(Label.TextProperty, property);
        return label;
    }


    private void OnSearch(object sender, TextChangedEventArgs e)          
    {
        var keyword = e.NewTextValue?.ToLower();

        if (string.IsNullOrWhiteSpace(keyword))
        {
            CreateTheGrid(_allMovies);
        }
        else
        {
            var filteredMovies = _allMovies.Where(m => m.title.ToLower().Contains(keyword)).ToList();      

            CreateTheGrid(filteredMovies);
        }
    }

    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}