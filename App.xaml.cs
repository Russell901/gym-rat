using gym_rat.Views;

namespace gym_rat
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(LandingPage), typeof(LandingPage));

        }
    }
}
