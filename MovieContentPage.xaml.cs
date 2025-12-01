namespace MovieExplorer;

public partial class MovieContentPage : ContentPage
{
    public MovieContentPage(Movies movie)
    {
        InitializeComponent();
        BindingContext = movie;
    }
    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}