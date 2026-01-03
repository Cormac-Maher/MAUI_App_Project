using System.Reflection.PortableExecutable;

namespace MovieExplorer;

public partial class AddMoviePage : ContentPage
{
    public Movies NewMovie { get; private set; }

    public AddMoviePage()
    {
        InitializeComponent();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (!int.TryParse(YearEntry.Text, out int year)) year = 0;
        if (!double.TryParse(RatingEntry.Text, out double rating)) rating = 0;              // year and rating set to numbers 
        List<string> genreList;
        if (string.IsNullOrWhiteSpace(GenreEntry.Text))                      // Genres set as a list
        {
            genreList = new List<string>();
        }
        else
        {
            genreList = new List<string>();
            genreList.Add(GenreEntry.Text);
        }

        NewMovie = new Movies
        {
            title = TitleEntry.Text,
            year = year,
            director = DirectorEntry.Text,
            rating = rating,
            emoji = EmojiEntry.Text,
            genre = genreList
        };

        await Navigation.PopModalAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
