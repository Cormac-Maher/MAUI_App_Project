using Microsoft.Maui.Controls;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace MovieExplorer
{
    public partial class MainPage : ContentPage
    {
        private HttpClient _httpClient;
        string userName;
        public MainPage()
        {
            InitializeComponent();
            LoadJsonAsync();
            userName = Preferences.Get("UserName", string.Empty);

            if (!string.IsNullOrEmpty(userName))
            {
                GreetingLabel.Text = $"Hello, {userName}! Welcome back to Movie Explorer";
            }
            else
            {
                GreetingLabel.Text = "Hello! Please enter your name.";
            }
            CreateTheGrid(); 
        }
  
/*        private void EnterName_Clicked(object? sender, EventArgs e)
        {            
            SubmitBtn1.SetValue(Button.IsEnabledProperty, false);
        }  */

        private void CreateTheGrid()
        {
            for (int i = 0; i < 3; i++)
            {
                GridPageContent.AddRowDefinition(new RowDefinition());
                GridPageContent.AddColumnDefinition(new ColumnDefinition());

            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    VerticalStackLayout info = new VerticalStackLayout
                    {
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Start,
                        Spacing = 2
                    };

                    info.Children.Add(new Label { Text = "Long Long Title", TextColor = Colors.Black, FontAttributes = FontAttributes.Bold, FontSize = 38 });
                    info.Children.Add(new Label { Text = "Year", TextColor = Colors.Black, FontSize = 18 });
                    info.Children.Add(new Label { Text = "Genre", TextColor = Colors.Black, FontSize = 18 });
                    info.Children.Add(new Label { Text = "Director", TextColor = Colors.Black, FontSize = 18 });
                    info.Children.Add(new Label { Text = "Rating", TextColor = Colors.Black, FontSize = 18 });
                    info.Children.Add(new Label { Text = "Emoji", TextColor = Colors.Black, FontSize = 18 });
                    Border styledBorder = new Border
                    {
                        BackgroundColor = Colors.Red,
                        Stroke = Colors.Black,
                        StrokeThickness = 3,
                        Content = info
                    };
                    GridPageContent.Add(styledBorder, i, j);

                    TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
  
                    styledBorder.GestureRecognizers.Add(tapGestureRecognizer);
                }
                Console.WriteLine();
            }
        }

        string fileName = "moviesemoji.json";
        string fileUrl = "https://raw.githubusercontent.com/DonH-ITS/jsonfiles/refs/heads/main/moviesemoji.json";
        private async void LoadJsonAsync()
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, fileName);
            string jsonContent;
            _httpClient = new HttpClient();
            if (File.Exists(path))
            {
   //             using var reader = new StreamReader(path);
                jsonContent = await File.ReadAllTextAsync(path);
                Console.WriteLine("Already dowloaded code!");
            }
            else
            {
                try {
                    var response = await _httpClient.GetAsync(fileUrl);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        jsonContent = await response.Content.ReadAsStringAsync();

                        await File.WriteAllTextAsync(path, jsonContent);
                    }
                }
                catch(Exception ex) {
                    Console.WriteLine($"Error: {ex.Message}");
                    return;
                }

//                using var httpClient = new HttpClient();
//                using var stream = await httpClient.GetStreamAsync(fileUrl);
//                using var reader = new StreamReader(stream);
//                jsonContent = await reader.ReadToEndAsync();
//                using var writer = new StreamWriter(path, false);
//               await writer.WriteAsync(jsonContent);
            }
        }
    }
}
