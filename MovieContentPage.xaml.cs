namespace MovieExplorer;

public partial class MovieContentPage : ContentPage
{
    public MovieContentPage()
    {
        InitializeComponent();
    }
    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}