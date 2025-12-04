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

    private async void Favourite_Clicked(object sender, EventArgs e)
    {
        bool isFavourite = Preferences.Get("Favourite_" + ((Movies)BindingContext).title, false);
        isFavourite = !isFavourite;
        Preferences.Set("Favourite_" + ((Movies)BindingContext).title, isFavourite);
         
        string userName;

 /*       if (!string.IsNullOrWhiteSpace(userName))
        {
            Preferences.Set("UserName", userName);

            await Navigation.PushAsync(new MainPage());
        }
        else
        {
            await DisplayAlert("Error", "Please enter a valid name.", "OK");
        }     */
    }
}