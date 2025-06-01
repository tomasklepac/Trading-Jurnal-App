using CommunityToolkit.Maui.Views;

namespace TradingJournalApp.Views.Popups
{
    /// <summary>
    /// Popup for selecting a date range.
    /// </summary>
    public partial class DateRangePopup : Popup
    {
        /// <summary>
        /// Constructor for DateRangePopup.
        /// </summary>
        /// <param name="currentFrom"></param>
        /// <param name="currentTo"></param>
        public DateRangePopup(DateTime currentFrom, DateTime currentTo)
        {
            InitializeComponent();

            fromDatePicker.Date = currentFrom == DateTime.MinValue ? DateTime.Today : currentFrom;
            toDatePicker.Date = currentTo == DateTime.MinValue ? DateTime.Today : currentTo;
        }

        /// <summary>
        /// Event handler for when the "Apply" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnApplyClicked(object sender, EventArgs e)
        {
            var from = fromDatePicker.Date;
            var to = toDatePicker.Date;

            if (from > to)
            {
                Element? parent = this;
                while (parent != null && parent is not Page)
                {
                    parent = parent.Parent;
                }

                if (parent is Page page)
                {
                    await page.DisplayAlert(
                        "Invalid Range",
                        "The start date must be before or equal to the end date.",
                        "OK"
                    );
                }

                return;
            }

            var selected = new Tuple<DateTime, DateTime>(from, to);
            Close(selected);
        }

        /// <summary>
        /// Event handler for when the "Cancel" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancelClicked(object sender, EventArgs e)
        {
            Close(null);
        }
    }
}
