namespace MovieExplorer
{
    public partial class App : Application
    {
        public App()
        {

            InitializeComponent();

            string savedColour = Preferences.Get("BackgroundColor", "DarkSlateGrey");             
            Application.Current.Resources["AppBackgroundColor"] = Color.FromArgb(savedColour);
            App.Current.Resources["AppBackgroundColor"] = Colors.DarkSlateGrey;

            string userName = Preferences.Get("UserName", string.Empty);
            if (string.IsNullOrEmpty(userName))
                MainPage = new NavigationPage(new NamePage());
            else
                MainPage = new NavigationPage(new MainPage());
            MainPage = new AppShell();
        }
        


    }
}
