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
        private readonly GetMovies _movieService = new GetMovies();
        private List<Movies> _allMovies = new();   // Made a list with all the movies
        string userName;
        public MainPage()
        {
            InitializeComponent();
            userName = Preferences.Get("UserName", string.Empty);
            if (!string.IsNullOrEmpty(userName))
            {
                GreetingLabel.Text = $"Hello {userName}! Welcome to the Movie Explorer";
            }
            else
            {
                GreetingLabel.Text = "Hello! Please enter your name.";
            }
            LoadJsonAsync();
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(SettingsPage));

        }




        private void CreateTheGrid(List<Movies> movies)
        {
            GridPageContent.RowDefinitions.Clear();
            GridPageContent.ColumnDefinitions.Clear();
            GridPageContent.Children.Clear();

            int columns = 2;
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
                            Padding = 10,
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




        private HttpClient _httpClient = new HttpClient();

        string fileName = "moviesemoji.json";
        string fileUrl = "https://raw.githubusercontent.com/DonH-ITS/jsonfiles/refs/heads/main/moviesemoji.json";

        //        private readonly MovieService _movieService = new MovieService();


        private async void LoadJsonAsync()
        {
            _allMovies = await _movieService.LoadMoviesAsync();
            CreateTheGrid(_allMovies);
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

        public void SortMovies(string sortBy)
        {
            switch (sortBy)
            {
                case "Title":
                    _allMovies = _allMovies.OrderBy(m => m.title).ToList();
                    break;
                case "Director":
                    _allMovies = _allMovies.OrderBy(m => m.director).ToList();
                    break;
                case "Year":
                    _allMovies = _allMovies.OrderBy(m => m.year).ToList();
                    break;
                case "Rating":
                    _allMovies = _allMovies
                        .OrderByDescending(m => m.rating)
                        .ToList();
                    break;
            }

            CreateTheGrid(_allMovies);
        }

        private void SortByClicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            SortMovies(btn.Text);
        }



    }
}


