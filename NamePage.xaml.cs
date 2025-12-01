namespace MovieExplorer;

public partial class NamePage : ContentPage
{
    public NamePage()
    {
        InitializeComponent();
    }
    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        string userName = NameEntry.Text;

        if (!string.IsNullOrWhiteSpace(userName))
        {
            Preferences.Set("UserName", userName);

            await Navigation.PushAsync(new MainPage());
        }
        else
        {
            await DisplayAlert("Error", "Please enter a valid name.", "OK");
        }
    }
}