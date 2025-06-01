using TradingJournalApp.Views;

namespace TradingJournalApp
{
    /// <summary>
    /// Interaction logic for AppShell.xaml
    /// </summary>
    public partial class AppShell : Shell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppShell"/> class.
        /// </summary>
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(JournalView), typeof(JournalView));
            Routing.RegisterRoute(nameof(AnalyticsView), typeof(AnalyticsView));
        }
    }
}
