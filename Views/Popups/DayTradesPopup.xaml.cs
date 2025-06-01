using CommunityToolkit.Maui.Views;
using TradingJournalApp.Data;
using TradingJournalApp.Models;
using Microsoft.Maui.Controls.Shapes;

namespace TradingJournalApp.Views.Popups
{
    /// <summary>
    /// Popup for displaying trades for a specific day.
    /// </summary>
    public partial class DayTradesPopup : Popup
    {
        /// <summary>
        /// Repository for accessing trade data.
        /// </summary>
        private readonly TradeRepository repository;

        /// <summary>
        /// Selected day for which trades are displayed.
        /// </summary>
        private readonly DateTime selectedDay;

        /// <summary>
        /// Constructor for the DayTradesPopup.
        /// </summary>
        /// <param name="date"></param>
        public DayTradesPopup(DateTime date)
        {
            InitializeComponent();
            selectedDay = date;
            TitleLabel.Text = $"Trades for {selectedDay:MMMM dd, yyyy}";

            string dbPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "trades.db");
            repository = new TradeRepository(dbPath);

            _ = LoadTrades();
        }

        /// <summary>
        /// Loads trades for the selected day from the repository and displays them in the popup.
        /// </summary>
        /// <returns></returns>
        private async Task LoadTrades()
        {
            TradesList.Children.Clear();

            var trades = await repository.GetAllTradesAsync();
            var dayTrades = trades.Where(t => t.Date.Date == selectedDay.Date).ToList();

            foreach (var trade in dayTrades)
            {
                var card = new Border
                {
                    BackgroundColor = Color.FromArgb("#1A1A1A"),
                    StrokeShape = new RoundRectangle { CornerRadius = 10 },
                    Padding = 10,
                    Content = new VerticalStackLayout
                    {
                        Spacing = 5,
                        Children =
                    {
                        new Label
                        {
                            Text = $"{trade.Instrument} - {trade.Direction} - {trade.PnL} USD",
                            TextColor = Colors.White
                        },
                        new Label
                        {
                            Text = $"Key: {trade.KeyLevel} | Conf: {trade.Confirmation} | Cont: {trade.Continuation}",
                            FontSize = 12,
                            TextColor = Colors.Gray
                        },
                        new HorizontalStackLayout
                        {
                            Spacing = 10,
                            Children =
                            {
                                new Button
                                {
                                    Text = "Edit",
                                    BackgroundColor = Colors.Transparent,
                                    TextColor = Color.FromArgb("#00FF99"),
                                    Command = new Command(async () => await EditTrade(trade))
                                },
                                new Button
                                {
                                    Text = "Delete",
                                    BackgroundColor = Colors.Transparent,
                                    TextColor = Colors.Red,
                                    Command = new Command(async () => await DeleteTrade(trade))
                                }
                            }
                        }
                    }
                    }
                };
                TradesList.Children.Add(card);
            }
        }

        /// <summary>
        /// Edits an existing trade.
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        private async Task EditTrade(Trade trade)
        {
            var popup = new AddTradePopup(trade);

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
                    result.Id = trade.Id;
                    await repository.UpdateTradeAsync(result);
                    await LoadTrades();
                }
            }
        }

        /// <summary>
        /// Deletes a trade after confirmation.
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        private async Task DeleteTrade(Trade trade)
        {
            Element? parent = this;
            while (parent != null && parent is not Page)
            {
                parent = parent.Parent;
            }

            if (parent is Page page)
            {
                bool confirm = await page.DisplayAlert("Confirm", "Delete this trade?", "Yes", "No");
                if (confirm)
                {
                    await repository.DeleteTradeAsync(trade);
                    await LoadTrades();
                }
            }
        }

        /// <summary>
        /// Event handler for when the "Add Trade" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnAddTradeClicked(object sender, EventArgs e)
        {
            var popup = new AddTradePopup(selectedDay);

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
                    await LoadTrades();
                }
            }
        }
    }
}
