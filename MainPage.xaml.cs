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
        public readonly GetMovies _movieService = new GetMovies();
        public List<Movies> _allMovies = new();                                 // Made a list with all the movies
        string userName;
        public MainPage()
        {
            InitializeComponent();
            userName = Preferences.Get("UserName", string.Empty);          // Check if user has a username  
            if (!string.IsNullOrEmpty(userName))
            {
                GreetingLabel.Text = $"Hello {userName}! Welcome to the Movie Explorer";
            }
            else
            {
                GreetingLabel.Text = "";
            }
        }

        protected override async void OnAppearing()      // delays loading the json to prevent crash
        {
            base.OnAppearing();
            await Task.Delay(100);
            await LoadJsonAsync();
        }
        private async Task LoadJsonAsync()
        {
            _allMovies = await _movieService.LoadMoviesAsync();             // loads movies from json file using GetMovies class

            if (_allMovies == null || _allMovies.Count == 0)
            {
                await DisplayAlert("Error", "No movies loaded. Check internet connection.", "OK");
            }

            CreateTheGrid(_allMovies);

            var genres = _allMovies.Where(m => m.genre != null).SelectMany(m => m.genre).ToList();   // adds all genres to a list 
            GenrePicker.ItemsSource = genres;                          // sets the genre picker items to the genres list
            System.Diagnostics.Debug.WriteLine("MOVIES LOADED: " + _allMovies.Count);

        }

        public void CreateTheGrid(List<Movies> movies)         // Created grid in c# similar to whack a mole lab
        {
            GridPageContent.RowDefinitions.Clear();
            GridPageContent.ColumnDefinitions.Clear();
            GridPageContent.Children.Clear();

            int columns = 2;
            int rows = (int)Math.Ceiling((double)movies.Count / columns);        // the grid size adjusts to number of movies

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

                        info.Children.Add(CreateLabel("title", 38));             // for each box in the grid, create labels for movie properties
                        info.Children.Add(CreateLabel("year", 18));

                        var genreLabel = new Label
                        {
                            FontSize = 18,
                            TextColor = Colors.Black,                                            // had to do this to get genres label to work
                            Text = movie.genre == null ? "" : string.Join(", ", movie.genre)    // this joins genres with comas
                        };
                        info.Children.Add(genreLabel);
                        info.Children.Add(CreateLabel("director", 18));
                        info.Children.Add(CreateLabel("rating", 18));
                        info.Children.Add(CreateLabel("emoji", 18));

                        var binButton = new ImageButton         // delete button
                        {
                            Source = "realbin.png",
                            BackgroundColor = Colors.Red,
                            HorizontalOptions = LayoutOptions.End,
                            WidthRequest = 30,
                            HeightRequest = 30
                        };

                        binButton.Clicked += (s, e) => DeleteMovie(movie);    // when the delete button is click, the movie is sent to delete method
                        info.Children.Add(binButton);

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

                        GridPageContent.Add(styledBorder, c, r);       // adds the styled border to the grid
                        i++;
                    }
                }
            }
        }
        private Label CreateLabel(string property, int fontSize)    // create label method for the movie info in the grid
        {
            var label = new Label
            {
                FontSize = fontSize,
                TextColor = Colors.Black,
            };
            label.SetBinding(Label.TextProperty, property);
            return label;
        }

        private async void DeleteMovie(Movies movie)       // method to delete a movie
        {
            bool confirm = await DisplayAlert("Confirm Delete",
                $"Are you sure you want to delete {movie.title}?",
                "Yes", "No");

            if (confirm)
            {
                _allMovies.Remove(movie);                  // deletes movie from the list and reloads the grid
                CreateTheGrid(_allMovies);

                string path = Path.Combine(FileSystem.AppDataDirectory, "moviesemoji.json");        // removes move from the json file
                string updatedJson = JsonSerializer.Serialize(_allMovies);                       // Serializes the updated list back to json
                await File.WriteAllTextAsync(path, updatedJson);

                string favPath = Path.Combine(FileSystem.AppDataDirectory, "favourites.json"); if (File.Exists(favPath))  // checks if the movie is favourited, if so it is removed from favouries too
                {
                    string favJson = await File.ReadAllTextAsync(favPath);
                    List<string> favourites = JsonSerializer.Deserialize<List<string>>(favJson) ?? new();

                    var entry = favourites.FirstOrDefault(f => f.StartsWith(movie.title + "|"));
                    if (entry != null)
                    {
                        favourites.Remove(entry);
                        string updatedFavJson = JsonSerializer.Serialize(favourites);
                        await File.WriteAllTextAsync(favPath, updatedFavJson);
                    }
                }
            }
        }

        private async void OpenQuiz_Clicked(object sender, EventArgs e)
        {
            var movies = await new GetMovies().LoadMoviesAsync();
            await Navigation.PushAsync(new QuizPage(movies));
        }
        private async void OnAddMovieClicked(object sender, EventArgs e)     // methed to add a movie
        {
            var addPage = new AddMoviePage();
            await Navigation.PushModalAsync(addPage);              // opens the Add Movie page

            addPage.Disappearing += async (s, args) =>               // When the page is closed   
            {
                if (addPage.NewMovie != null)
                {
                    _allMovies.Add(addPage.NewMovie);
                    CreateTheGrid(_allMovies);

                    string path = Path.Combine(FileSystem.AppDataDirectory, "moviesemoji.json");   // adds the new movie to the json file
                    string updatedJson = JsonSerializer.Serialize(_allMovies);                // Serializes the updated list back to json
                    await File.WriteAllTextAsync(path, updatedJson);
                }
            };
        }
        private async void OnSettingsClicked(object sender, EventArgs e)    // Settings button method
        {
            await Shell.Current.GoToAsync(nameof(SettingsPage));

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

        public void SortMovies(string sort)          // sort movies based on the button clicked
        {
            switch (sort)
            {
                case "Title":
                    _allMovies = _allMovies.OrderBy(m => m.title).ToList();
                    break;
                case "Director":
                    _allMovies = _allMovies.OrderBy(m => m.director).ToList();
                    break;
                case "Year":
                    _allMovies = _allMovies.OrderByDescending(m => m.year).ToList();
                    break;
                case "Rating":
                    _allMovies = _allMovies.OrderByDescending(m => m.rating).ToList();
                    break;
            }

            CreateTheGrid(_allMovies);
        }

        private async void SortByClicked(object sender, EventArgs e)      // Sometimes the app would crash when the ueer spammed sort buttons, so I disabled the buttons for a few seconds after sorting
        {
            Button btn = (Button)sender;

            SortTitle.IsEnabled = false;
            SortYear.IsEnabled = false;
            SortRating.IsEnabled = false;
            SortDirector.IsEnabled = false;

            SortTitle.BackgroundColor = Colors.Gray;        // disable buttons and turn the grey
            SortYear.BackgroundColor = Colors.Gray;
            SortRating.BackgroundColor = Colors.Gray;
            SortDirector.BackgroundColor = Colors.Gray;
            btn.BackgroundColor = Colors.Red;

            GenrePicker.SelectedIndex = -1;
            GreetingLabel.Text = "Sorting Movies";
            await Task.Delay(500);
            GreetingLabel.Text = "Sorting Movies.";
            await Task.Delay(500);
            GreetingLabel.Text = "Sorting Movies..";
            await Task.Delay(500);
            GreetingLabel.Text = "Sorting Movies...";
            await Task.Delay(500);

            SortMovies(btn.Text);
            GreetingLabel.Text = $"Hello {userName}! Welcome to the Movie Explorer";
            await Task.Delay(3000);

            SortTitle.IsEnabled = true;
            SortYear.IsEnabled = true;
            SortRating.IsEnabled = true;
            SortDirector.IsEnabled = true;

            SortTitle.BackgroundColor = Colors.Red;
            SortYear.BackgroundColor = Colors.Red;
            SortRating.BackgroundColor = Colors.Red;           // re enables buttons
            SortDirector.BackgroundColor = Colors.Red;
        }

        private async void OnGenreSelected(object sender, EventArgs e)               // method for genre picker
        {
            var genrePicker = (Picker)sender;
            var selectedGenre = genrePicker.SelectedItem as string;

            if (string.IsNullOrEmpty(selectedGenre))
            {
                CreateTheGrid(_allMovies);
            }
            else
            {
                var filteredMovies = _allMovies.Where(m => m.genre != null && m.genre.Contains(selectedGenre)).ToList();     // shows only movies that contain a selected genre

                SortTitle.IsEnabled = false;
                SortYear.IsEnabled = false;
                SortRating.IsEnabled = false;
                SortDirector.IsEnabled = false;

                SortTitle.BackgroundColor = Colors.Gray;
                SortYear.BackgroundColor = Colors.Gray;
                SortRating.BackgroundColor = Colors.Gray;
                SortDirector.BackgroundColor = Colors.Gray;

                GreetingLabel.Text = "Filtering Movies";
                await Task.Delay(200);
                GreetingLabel.Text = "Filtering Movies.";
                await Task.Delay(200);
                GreetingLabel.Text = "Filtering Movies..";
                await Task.Delay(200);
                GreetingLabel.Text = "Filtering Movies...";
                await Task.Delay(200);

                CreateTheGrid(filteredMovies);
                GreetingLabel.Text = $"Hello {userName}! Welcome to the Movie Explorer";
                await Task.Delay(1000);

                SortTitle.IsEnabled = true;
                SortYear.IsEnabled = true;
                SortRating.IsEnabled = true;
                SortDirector.IsEnabled = true;

                SortTitle.BackgroundColor = Colors.Red;
                SortYear.BackgroundColor = Colors.Red;
                SortRating.BackgroundColor = Colors.Red;
                SortDirector.BackgroundColor = Colors.Red;
            }
        }
    }
}
