namespace MovieExplorer
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MovieContentPage), typeof(MovieContentPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(FavouritesPage), typeof(FavouritesPage));
        }
    }
}
