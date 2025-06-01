using CommunityToolkit.Maui.Views;
using TradingJournalApp.ViewModels;
using TradingJournalApp.Views.Popups;
using TradingJournalApp.Models;
using TradingJournalApp.Data;

namespace TradingJournalApp.Views
{
    /// <summary>
    /// AnalyticsView.xaml's code-behind file.
    /// </summary>
    public partial class AnalyticsView : ContentView
    {
        /// <summary>
        /// Constructor for AnalyticsView.
        /// </summary>
        public AnalyticsView()
        {
            InitializeComponent();

            string dbPath = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "trades.db");

            var repo = new TradeRepository(dbPath);

            var vm = new AnalyticsViewModel
            {
                repository = repo
            };

            vm.RequestCustomDateRangePopup = ShowCustomDateRangePopup;
            BindingContext = vm;
            vm.ApplyInitialRange();
        }

        /// <summary>
        /// Event handler for when the "Custom" button is clicked.
        /// </summary>
        /// <param name="currentFrom"></param>
        /// <param name="currentTo"></param>
        /// <returns></returns>
        private async Task ShowCustomDateRangePopup(DateTime currentFrom, DateTime currentTo)
        {
            var popup = new DateRangePopup(currentFrom, currentTo);

            Element? parent = this;
            while (parent != null && parent is not Page)
            {
                parent = parent.Parent;
            }

            if (parent is Page page)
            {
                var result = await page.ShowPopupAsync(popup) as Tuple<DateTime, DateTime>;

                if (result != null && BindingContext is AnalyticsViewModel vm)
                {
                    vm.SelectedFromDate = result.Item1;
                    vm.SelectedToDate = result.Item2;
                    vm.SelectedRangeType = DateRangeType.Custom;
                    vm.LoadStats();
                }
            }
        }
    }
}
