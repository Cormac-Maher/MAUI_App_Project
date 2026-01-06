namespace MovieExplorer;

public partial class NamePage : ContentPage
{
    public NamePage()
    {
        InitializeComponent();
    }
    private async void OnSubmitClicked(object sender, EventArgs e)          // This page is for setting the users name
    {
        string userName = NameEntry.Text;

        if (!string.IsNullOrWhiteSpace(userName))
        {
            Preferences.Set("UserName", userName);

            Application.Current.MainPage = new AppShell();
        }
        else
        {
            await DisplayAlert("Error", "Please enter a valid name.", "OK");
        }
    }
}
