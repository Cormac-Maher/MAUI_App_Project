using Microsoft.Maui.Controls;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace MovieExplorer
{
    public partial class MainPage : ContentPage
    {

  //    private List<Movies> _movies = new();
        string userName;
        public MainPage()
        {

            InitializeComponent();
            LoadJsonAsync();
            LoadMovies();
            userName = Preferences.Get("UserName", string.Empty);

            if (!string.IsNullOrEmpty(userName))
            {
                GreetingLabel.Text = $"Hello, {userName}! Welcome back to Movie Explorer";
            }
            else
            {
                GreetingLabel.Text = "Hello! Please enter your name.";
            }
        }

  




        private void CreateTheGrid(List<Movies> movies)  
        {
            GridPageContent.RowDefinitions.Clear();    
            GridPageContent.ColumnDefinitions.Clear(); 
            GridPageContent.Children.Clear();          

            int columns = 3;
            int rows = (int)Math.Ceiling((double)movies.Count / columns);

            for (int r = 0; r < rows; r++)
                GridPageContent.AddRowDefinition(new RowDefinition());

            for (int c = 0; c < columns; c++)
                GridPageContent.AddColumnDefinition(new ColumnDefinition());

            int index = 0;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    if (index >= movies.Count) break;

                    var movie = movies[index];  

                 
                    VerticalStackLayout info = new()
                    {
                        Spacing = 2,
                        BindingContext = movie 
                    };

                    info.Children.Add(CreateBoundLabel("title", 38, true));
                    info.Children.Add(CreateBoundLabel("year"));
                    info.Children.Add(CreateBoundLabel("genre"));
                    info.Children.Add(CreateBoundLabel("director"));
                    info.Children.Add(CreateBoundLabel("rating"));
                    info.Children.Add(CreateBoundLabel("emoji"));

                    Border styledBorder = new Border
                    {
                        BackgroundColor = Colors.Red,
                        Stroke = Colors.Black,
                        StrokeThickness = 3,
                        Content = info
                    };

                    GridPageContent.Add(styledBorder, c, r);
                    index++;
                }
            }
        }
        private Label CreateBoundLabel(string property, int fontSize = 18, bool bold = false)
        {
            var label = new Label
            {
                FontSize = fontSize,
                TextColor = Colors.Black,
            };
            label.SetBinding(Label.TextProperty, property);
            return label;
        }



        private HttpClient _httpClient = new HttpClient();

        string fileName = "moviesemoji.json";
        string fileUrl = "https://raw.githubusercontent.com/DonH-ITS/jsonfiles/refs/heads/main/moviesemoji.json";

        private readonly MovieService _movieService = new MovieService();
        private async void LoadMovies()
        {
            MoviesCollectionView.ItemsSource = await _movieService.GetMoviesAsync();
        }


        private async void LoadJsonAsync()
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, fileName);
            string jsonContent;
            _httpClient = new HttpClient();
            if (File.Exists(path))
            {
                jsonContent = await File.ReadAllTextAsync(path);
                Console.WriteLine("Already dowloaded code!");
                var movies = JsonSerializer.Deserialize<List<Movies>>(jsonContent);
   //             GridView.ItemsSource = movies;
                CreateTheGrid(movies);
            }
            else
            {
                try {
                    var response = await _httpClient.GetAsync(fileUrl);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        jsonContent = await response.Content.ReadAsStringAsync();

                        await File.WriteAllTextAsync(path, jsonContent);
                        var movies = JsonSerializer.Deserialize<List<Movies>>(jsonContent);
                        Console.WriteLine($"Movies loaded: {movies?.Count}");
                        //                    GridView.ItemsSource = movies;
                        CreateTheGrid(movies);
                    }
                }
                catch(Exception ex) {
                    Console.WriteLine($"Error: {ex.Message}");
                    return;
                }
            }

        }

    }
}

/**      public async Task<List<Movies>> GetMoviesAsync(string fileUrl)
      {
          var movies = await _httpClient.GetFromJsonAsync<List<Movies>>(fileUrl);
          return movies;
      }**/
