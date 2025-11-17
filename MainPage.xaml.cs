using Microsoft.Maui.Controls;
using System.Windows;
using System.Windows.Input;

namespace MovieExplorer
{
    public partial class MainPage : ContentPage
    {
        string fileName = "moviesemoji.json";
        string fileUrl = "https://raw.githubusercontent.com/DonH-ITS/jsonfiles/refs/heads/main/moviesemoji.json";
        string userName;
        public MainPage()
        {
            InitializeComponent();
            LoadJsonAsync();
            userName = Preferences.Get("UserName", string.Empty);

            if (!string.IsNullOrEmpty(userName))
            {
                GreetingLabel.Text = $"Hello, {userName}! Welcome back";
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
                for (int j = 0; j < 3; j++)
                {
                    Border styledBorder = new Border
                    {
                        BackgroundColor = Colors.Red,
                        Stroke = Colors.Black,
                        StrokeThickness = 3
                    };
                    GridPageContent.Add(styledBorder, i, j);

                    TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
  
                    styledBorder.GestureRecognizers.Add(tapGestureRecognizer);
                }
                Console.WriteLine();
            }
        }
        private async void LoadJsonAsync()
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, fileName);
            string jsonContent;

            if (File.Exists(path))
            {
                using var reader = new StreamReader(path);
                jsonContent = await File.ReadAllTextAsync(path);
            }
            else
            {
                using var httpClient = new HttpClient();
                using var stream = await httpClient.GetStreamAsync(fileUrl);
                using var reader = new StreamReader(stream);
                jsonContent = await reader.ReadToEndAsync();
                using var writer = new StreamWriter(path, false);
                await writer.WriteAsync(jsonContent);
            }
        }
    }
}
