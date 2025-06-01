using CommunityToolkit.Maui.Views;
using TradingJournalApp.Models;

namespace TradingJournalApp.Views.Popups
{
    /// <summary>
    /// Popup for adding or editing a trade.
    /// </summary>
    public partial class AddTradePopup : Popup
    {
        private string _direction = "";
        private string _keyLevel = "";
        private string _confirmation = "";
        private string _continuation = "";
        private string _result = "";
        private readonly int? _editingTradeId = null;

        /// <summary>
        /// Constructor for the AddTradePopup.
        /// </summary>
        public AddTradePopup() : this(DateTime.Today) { }

        /// <summary>
        /// Constructor for the AddTradePopup with a specific date.
        /// </summary>
        /// <param name="date"></param>
        public AddTradePopup(DateTime date)
        {
            InitializeComponent();
            datePicker.Date = date;
            InitializeButtons();
        }

        /// <summary>
        /// Constructor for the AddTradePopup with an existing trade.
        /// </summary>
        /// <param name="existingTrade"></param>
        public AddTradePopup(Trade existingTrade)
        {
            InitializeComponent();
            _editingTradeId = existingTrade.Id;

            instrumentEntry.Text = existingTrade.Instrument;
            pnlEntry.Text = existingTrade.PnL.ToString();
            chartEntry.Text = existingTrade.Chart;
            noteEditor.Text = existingTrade.Note;

            _direction = existingTrade.Direction;
            _keyLevel = existingTrade.KeyLevel;
            _confirmation = existingTrade.Confirmation;
            _continuation = existingTrade.Continuation;
            _result = existingTrade.Result;

            InitializeButtons();
        }

        /// <summary>
        /// Initializes the buttons for the trade options.
        /// </summary>
        private void InitializeButtons()
        {
            CreateButtons(TradeOptions.Directions, Direction_Clicked, DirectionContainer, _direction);
            CreateButtons(TradeOptions.KeyLevels, KeyLevel_Clicked, KeyLevelContainer, _keyLevel);
            CreateButtons(TradeOptions.Confirmations, Confirmation_Clicked, ConfirmationContainer, _confirmation);
            CreateButtons(TradeOptions.Continuations, Continuation_Clicked, ContinuationContainer, _continuation);
            CreateButtons(TradeOptions.Results, Result_Clicked, ResultContainer, _result);
        }

        /// <summary>
        /// Creates buttons for the specified options and adds them to the container.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="clickHandler"></param>
        /// <param name="container"></param>
        /// <param name="selectedValue"></param>
        private void CreateButtons(List<string> options, EventHandler clickHandler, Microsoft.Maui.Controls.Layout container, string selectedValue = "")
        {
            foreach (var option in options)
            {
                var button = new Button
                {
                    Text = option,
                    BackgroundColor = option == selectedValue ? Color.FromArgb("#00FF99") : Colors.Transparent,
                    TextColor = option == selectedValue ? Colors.Black : Colors.White,
                    FontSize = 12,
                    Margin = new Thickness(4),
                };

                button.Clicked += clickHandler;
                container.Children.Add(button);
            }
        }

        /// <summary>
        /// Event handler for when a direction button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Direction_Clicked(object? sender, EventArgs e)
        {
            if (sender is Button button)
            {
                _direction = button.Text;
                HighlightSelectedButton(button);
            }
        }

        /// <summary>
        /// Event handler for when a key level button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyLevel_Clicked(object? sender, EventArgs e)
        {
            if (sender is Button button)
            {
                _keyLevel = button.Text;
                HighlightSelectedButton(button);
            }
        }

        /// <summary>
        /// Event handler for when a confirmation button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Confirmation_Clicked(object? sender, EventArgs e)
        {
            if (sender is Button button)
            {
                _confirmation = button.Text;
                HighlightSelectedButton(button);
            }
        }

        /// <summary>
        /// Event handler for when a continuation button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Continuation_Clicked(object? sender, EventArgs e)
        {
            if (sender is Button button)
            {
                _continuation = button.Text;
                HighlightSelectedButton(button);
            }
        }

        /// <summary>
        /// Event handler for when a result button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Result_Clicked(object? sender, EventArgs e)
        {
            if (sender is Button button)
            {
                _result = button.Text;
                HighlightSelectedButton(button);
            }
        }

        /// <summary>
        /// Highlights the selected button and resets the others.
        /// </summary>
        /// <param name="selected"></param>
        private void HighlightSelectedButton(Button selected)
        {
            var parent = selected.Parent as Microsoft.Maui.Controls.Layout;
            if (parent == null) return;

            foreach (var child in parent.Children.OfType<Button>())
            {
                child.BackgroundColor = Colors.Transparent;
                child.TextColor = Colors.White;
            }

            selected.BackgroundColor = Color.FromArgb("#00FF99");
            selected.TextColor = Colors.Black;
        }

        /// <summary>
        /// Event handler for when the save button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (!double.TryParse(pnlEntry.Text, out double pnl))
            {
                Element? parent = this;
                while (parent != null && parent is not Page)
                {
                    parent = parent.Parent;
                }

                if (parent is Page page)
                {
                    await page.DisplayAlert("Error", "Invalid PnL", "OK");
                }

                return;
            }

            var trade = new Trade
            {
                Id = _editingTradeId ?? 0,
                Instrument = instrumentEntry.Text ?? "",
                Date = datePicker.Date,
                Direction = _direction,
                KeyLevel = _keyLevel,
                Confirmation = _confirmation,
                Continuation = _continuation,
                Result = _result,
                PnL = pnl,
                Chart = chartEntry.Text ?? "",
                Note = noteEditor.Text ?? ""
            };

            Close(trade);
        }

        /// <summary>
        /// Event handler for when the cancel button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancelClicked(object sender, EventArgs e)
        {
            Close(null);
        }
    }
}
