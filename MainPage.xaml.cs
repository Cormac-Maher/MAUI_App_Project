// using Android.Graphics;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
                GreetingLabel.Text = $"Hello {userName}! Welcome to the Movie Explorer";
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
                GridPageContent.AddColumnDefinition(new ColumnDefinition { Width = GridLength.Star });    // Using Star so all cols adjust to the size of the window

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

/*
  public string myLibraryName = "My Library";
  private Movies _selectedMovie;
  public event PropertyChangedEventHandler? PropertyChanged;
  protected virtual void OnPropertyChanged(string? propertyName = null)
  {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
  public ObservableCollection<Movies> Movies { get; set; }
  public Movies SelectedMovies
  {
      get { return _selectedMovie; }


      set
      {
          if (_selectedMovie != value)
          {
              _selectedMovie = value;
              OnPropertyChanged();
          }
      }
  }

  public string LibraryName
  {
      get { return myLibraryName; }

      set
      {
          if (myLibraryName != value)
          {
              myLibraryName = value;
              OnPropertyChanged();
          }
      }
  }
  private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {

      if (e.CurrentSelection.FirstOrDefault() is Movies selectedMovie)
      {
          var parameters = new Dictionary<string, object>
          {
              { "Movies", selectedMovie}
          };
          await Shell.Current.GoToAsync(nameof(MovieContentPage), parameters);
          ((CollectionView)sender).SelectedItem = null;
      }
      ;
  } */
