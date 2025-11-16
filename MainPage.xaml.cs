using Microsoft.Maui.Controls;
using System.Windows;
using System.Windows.Input;

namespace MovieExplorer
{
    public partial class MainPage : ContentPage
    {
        int i = 0;
        string userName;
        public MainPage()
        {
            InitializeComponent();

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
    }
}
