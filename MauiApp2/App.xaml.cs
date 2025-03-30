namespace MauiApp2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Upewnij się, że ustawiasz główną stronę jako NavigationPage
            MainPage = new NavigationPage(new NewPage1());
        }
    }
}