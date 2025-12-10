using System.Text.Json;

namespace MovieExplorer;

public partial class FavouritesPage : ContentPage
{ 
	private List<Movies> _allMovies = new();   
	public FavouritesPage()
	{
		InitializeComponent();

        var favourites = Preferences.Get("Favourites", string.Empty).Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();
        var favouriteMovies = _allMovies.Where(m => favourites.Contains(m.title)).ToList();
        CreateTheGrid(favouriteMovies);
    }

    private void CreateTheGrid(List<Movies> movies)    // Same code as MainPage.xaml.cs to create the grid
    {
        GridPageContent.RowDefinitions.Clear();
        GridPageContent.ColumnDefinitions.Clear();
        GridPageContent.Children.Clear();

        int columns = 3;
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
                        StrokeThickness = 3,
                        Content = info
                    };

                    var tapGesture = new TapGestureRecognizer();     // Tap gesture recogniser so user can tap on a movie to see details
                    tapGesture.Tapped += async (s, e) =>
                    {
                        await Navigation.PushAsync(new MovieContentPage(movie));     // When the user taps on a movie, the Movie Content page will load
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


    private void OnSearch(object sender, TextChangedEventArgs e)               // Code for search bar
    {
        var keyword = e.NewTextValue?.ToLower();

        if (string.IsNullOrWhiteSpace(keyword))
        {
            CreateTheGrid(_allMovies);
        }
        else
        {
            var filteredMovies = _allMovies.Where(m => m.title.ToLower().Contains(keyword)).ToList();       //Shows only the movies that contain the searched word

            CreateTheGrid(filteredMovies);
        }
    }
}