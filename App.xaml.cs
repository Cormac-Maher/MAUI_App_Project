namespace MovieExplorer
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            string userName = Preferences.Get("UserName", string.Empty);

            if (string.IsNullOrEmpty(userName))
            {
                MainPage = new NavigationPage(new NamePage());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            } 
            MainPage = new NavigationPage(new MainPage());


            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Colors.DarkSlateGray, // same as your page background
                BarTextColor = Colors.White
            };



        }
    }
}