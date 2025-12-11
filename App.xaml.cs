namespace MovieExplorer
{
    public partial class App : Application
    {
        public App()
        {

            InitializeComponent();

            string savedColor = Preferences.Get("BackgroundColor", "DarkSlateGray");
            Application.Current.Resources["AppBackgroundColor"] = Color.FromArgb(savedColor);

            string userName = Preferences.Get("UserName", string.Empty);
            if (string.IsNullOrEmpty(userName))
                MainPage = new NavigationPage(new NamePage());
            else
                MainPage = new NavigationPage(new MainPage());
            MainPage = new AppShell();
        }
        


    }
}
