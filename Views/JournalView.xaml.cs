using CommunityToolkit.Maui.Views;
using TradingJournalApp.Views.Popups;
using TradingJournalApp.Models;
using TradingJournalApp.Data;
using TradingJournalApp.ViewModels;

namespace TradingJournalApp.Views
{
    /// <summary>
    /// JournalView.xaml's code-behind file.
    /// </summary>
    public partial class JournalView : ContentView
    {
        /// <summary>
        /// Repository for accessing trade data.
        /// </summary>
        private readonly TradeRepository repository;

        /// <summary>
        /// Constructor for the JournalView.
        /// </summary>
        public JournalView()
        {
            InitializeComponent();

            string dbPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "trades.db");
            repository = new TradeRepository(dbPath);

            BindingContext = new JournalViewModel(repository);
        }

        /// <summary>
        /// Event handler for when the "Add Trade" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnGlobalAddTradeClicked(object sender, EventArgs e)
        {
            var popup = new AddTradePopup();

            Element? parent = this;
            while (parent != null && parent is not Page)
            {
                parent = parent.Parent;
            }

            if (parent is Page page)
            {
                var result = await page.ShowPopupAsync(popup) as Trade;

                if (result != null)
                {
                    await repository.AddTradeAsync(result);

                    if (BindingContext is JournalViewModel vm)
                        await vm.ReloadTradeDays();

                    await DisplayAlert("Success", "Trade was saved.", "OK");
                }
            }
        }

        /// <summary>
        /// Event handler for when the "Trades" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnTradesClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is TradeDay tradeDay)
            {
                var popup = new DayTradesPopup(tradeDay.DateRaw);

                Element? parent = this;
                while (parent != null && parent is not Page)
                {
                    parent = parent.Parent;
                }

                if (parent is Page page)
                {
                    await page.ShowPopupAsync(popup);

                    if (BindingContext is JournalViewModel vm)
                        await vm.ReloadTradeDays();
                }
            }
        }

        /// <summary>
        /// Displays an alert dialog with the specified title, message, and cancel button text.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        private Task DisplayAlert(string title, string message, string cancel)
        {
            Element? parent = this;
            while (parent != null && parent is not Page)
            {
                parent = parent.Parent;
            }

            if (parent is Page page)
            {
                return page.DisplayAlert(title, message, cancel);
            }

            return Task.CompletedTask;
        }

    }
}
