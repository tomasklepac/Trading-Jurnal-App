namespace TradingJournalApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates the main window of the application.
        /// </summary>
        /// <param name="activationState"></param>
        /// <returns></returns>
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

    }
}