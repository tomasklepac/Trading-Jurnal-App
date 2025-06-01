using System.Windows.Input;
using TradingJournalApp.Models;
using TradingJournalApp.Data;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.Defaults;

namespace TradingJournalApp.ViewModels
{
    /// <summary>
    /// ViewModel for the Analytics page.
    /// </summary>
    public class AnalyticsViewModel : BaseViewModel
    {

        public ISeries[] DailyPnLSeries { get; set; } = [];

        public Axis[] DailyPnLAxisX { get; set; } = [];
        public Axis[] DailyPnLAxisY { get; set; } = [];

        /// <summary>
        /// Repository for the trades.
        /// </summary>
        public required TradeRepository repository { get; init; }

        /// <summary>
        /// Command to navigate to the journal page.
        /// </summary>
        public ICommand GoToJournalCommand { get; } = new Command(() => { });

        /// <summary>
        /// Command to navigate to the analytics page.
        /// </summary>
        public ICommand GoToAnalyticsCommand { get; } = new Command(() => { });

        /// <summary>
        /// Command to navigate to the settings page.
        /// </summary>
        public ICommand SelectThisWeekCommand { get; }

        /// <summary>
        /// Command to select the current month.
        /// </summary>
        public ICommand SelectThisMonthCommand { get; }

        /// <summary>
        /// Command to select the current year.
        /// </summary>
        public ICommand SelectThisYearCommand { get; }

        /// <summary>
        /// Command to select all trades.
        /// </summary>
        public ICommand SelectAllCommand { get; }

        /// <summary>
        /// Command to select a custom date range.
        /// </summary>
        public ICommand SelectCustomCommand { get; }


        private ObservableCollection<DailyPnLPoint> pnlSeries = new();
        public ObservableCollection<DailyPnLPoint> PnLSeries
        {
            get => pnlSeries;
            set
            {
                pnlSeries = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The total number of trades.
        /// </summary>
        private int tradeCount;
        public int TradeCount
        {
            get => tradeCount;
            set
            {
                tradeCount = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// The winrate of the trades.
        /// </summary>
        private double winRate;
        public double WinRate
        {
            get => winRate;
            set
            {
                winRate = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// The average win of the trades.
        /// </summary>
        private double avgWin;
        public double AvgWin
        {
            get => avgWin;
            set
            {
                avgWin = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The average loss of the trades.
        /// </summary>
        private double avgLoss;
        public double AvgLoss
        {
            get => avgLoss;
            set
            {
                avgLoss = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The total profit and loss of the trades.
        /// </summary>
        private double totalPnL;
        public double TotalPnL
        {
            get => totalPnL;
            set
            {
                totalPnL = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalPnLColor));
            }
        }

        /// <summary>
        /// The ratio of long trades to total trades.
        /// </summary>
        private double longRatio;
        public double LongRatio
        {
            get => longRatio;
            set
            {
                longRatio = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The ratio of short trades to total trades.
        /// </summary>
        private double shortRatio;
        public double ShortRatio
        {
            get => shortRatio;
            set
            {
                shortRatio = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Color for the total PnL value based on its sign (positive or negative).
        /// </summary>
        public Color TotalPnLColor => TotalPnL >= 0 ? Colors.LimeGreen : Colors.Red;

        /// <summary>
        /// Enum representing the different date range types for filtering trades.
        /// </summary>
        private DateRangeType selectedRangeType = DateRangeType.ThisMonth;
        public DateRangeType SelectedRangeType
        {
            get => selectedRangeType;
            set
            {
                selectedRangeType = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The start date for the selected date range.
        /// </summary>
        private DateTime selectedFromDate;
        public DateTime SelectedFromDate
        {
            get => selectedFromDate;
            set
            {
                selectedFromDate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ActiveRangeText));
            }
        }

        /// <summary>
        /// The end date for the selected date range.
        /// </summary>
        private DateTime selectedToDate;
        public DateTime SelectedToDate
        {
            get => selectedToDate;
            set
            {
                selectedToDate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ActiveRangeText));
            }
        }


        /// <summary>
        /// Constructor for the AnalyticsViewModel.
        /// </summary>
        public AnalyticsViewModel()
        {
            ApplyDateRange(DateRangeType.ThisMonth);

            SelectThisWeekCommand = new Command(() => ApplyDateRange(DateRangeType.ThisWeek));
            SelectThisMonthCommand = new Command(() => ApplyDateRange(DateRangeType.ThisMonth));
            SelectThisYearCommand = new Command(() => ApplyDateRange(DateRangeType.ThisYear));
            SelectAllCommand = new Command(() => ApplyDateRange(DateRangeType.All));
            SelectCustomCommand = new Command(async () =>
            {
                if (RequestCustomDateRangePopup != null)
                    await RequestCustomDateRangePopup.Invoke(SelectedFromDate, SelectedToDate);
            });
        }

        /// <summary>
        /// Loads the statistics from the database.
        /// </summary>
        public async void LoadStats()
        {
            if (repository is null)
                return;

            var trades = await repository.GetAllTradesAsync();

            if (trades.Count == 0)
            {
                SetDefaults();
                return;
            }

            var filtered = trades.Where(t => t.Date >= SelectedFromDate && t.Date <= SelectedToDate).ToList();

            if (filtered.Count == 0)
            {
                SetDefaults();
                return;
            }

            CalculateTotalTrades(filtered);
            CalculateWinRate(filtered);
            CalculateAvgWin(filtered);
            CalculateAvgLoss(filtered);
            CalculateTotalPnL(filtered);
            CalculateLongShortRatio(filtered);
            var dailyPnLPoints = CalculateDailyPnL(filtered);
            UpdateDailyPnLSeries(dailyPnLPoints);
        }

        /// <summary>
        /// Sets default values for the statistics.
        /// </summary>
        private void SetDefaults()
        {
            WinRate = 0;
            AvgWin = 0;
            AvgLoss = 0;
            TotalPnL = 0;
            LongRatio = 0;
            ShortRatio = 0;
        }

        /// <summary>
        /// Calculates the total number of trades.
        /// </summary>
        /// <param name="trades"></param>
        private void CalculateTotalTrades(List<Trade> trades)
        {
            TradeCount = trades.Count;
        }

        /// <summary>
        /// Calculates the win rate based on the trades.
        /// </summary>
        /// <param name="trades"></param>
        private void CalculateWinRate(List<Trade> trades)
        {
            int wins = trades.Count(t => t.Result == "WIN");
            WinRate = (double)wins / trades.Count * 100;
        }

        /// <summary>
        /// Calculates the average win based on the trades.
        /// </summary>
        /// <param name="trades"></param>
        private void CalculateAvgWin(List<Trade> trades)
        {
            var wins = trades.Where(t => t.Result == "WIN").ToList();
            AvgWin = wins.Count > 0 ? wins.Average(t => t.PnL) : 0;
        }

        /// <summary>
        /// Calculates the average loss based on the trades.
        /// </summary>
        /// <param name="trades"></param>
        private void CalculateAvgLoss(List<Trade> trades)
        {
            var losses = trades.Where(t => t.Result == "LOSS").ToList();
            AvgLoss = losses.Count > 0 ? losses.Average(t => t.PnL) : 0;
        }

        /// <summary>
        /// Calculates the total PnL based on the trades.
        /// </summary>
        /// <param name="trades"></param>
        private void CalculateTotalPnL(List<Trade> trades)
        {
            TotalPnL = trades.Sum(t => t.PnL);
        }

        /// <summary>
        /// Calculates the long and short ratio based on the trades.
        /// </summary>
        /// <param name="trades"></param>
        private void CalculateLongShortRatio(List<Trade> trades)
        {
            int total = trades.Count;
            if (total == 0)
            {
                LongRatio = 0;
                ShortRatio = 0;
                return;
            }

            int longCount = trades.Count(t => t.Direction == "Long");
            int shortCount = trades.Count(t => t.Direction == "Short");

            LongRatio = (double)longCount / total * 100;
            ShortRatio = (double)shortCount / total * 100;
        }

        /// <summary>
        /// Calculates the PnL of each day.
        /// </summary>
        /// <param name="trades"></param>
        private List<DailyPnLPoint> CalculateDailyPnL(List<Trade> trades)
        {
            return trades
                .GroupBy(t => t.Date.Date)
                .OrderBy(g => g.Key)
                .Select(g => new DailyPnLPoint
                {
                    Date = g.Key,
                    TotalPnL = g.Sum(t => t.PnL)
                })
                .ToList();
        }

        /// <summary>
        /// Updates the daily PnL series for the chart.
        /// </summary>
        /// <param name="points"></param>
        private void UpdateDailyPnLSeries(List<DailyPnLPoint> points)
        {
            var cumulativePoints = new List<ObservablePoint>();
            double cumulativePnL = 0;

            foreach (var point in points.OrderBy(p => p.Date))
            {
                cumulativePnL += point.TotalPnL;

                cumulativePoints.Add(new ObservablePoint(
                    (float)(point.Date - DateTime.UnixEpoch).TotalDays,
                    cumulativePnL
                ));
            }

            DailyPnLSeries = new ISeries[]
            {
                new LineSeries<ObservablePoint>
                {
                    Values = cumulativePoints,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.Cyan) { StrokeThickness = 2 },
                    GeometrySize = 6
                }
            };

            DailyPnLAxisX = new Axis[]
            {
                new Axis
                {
                    Labeler = value => DateTime.UnixEpoch.AddDays(value).ToString("MM/dd"),
                    LabelsRotation = 45,
                    TextSize = 12
                }
            };

            DailyPnLAxisY = new Axis[]
            {
                new Axis
                {
                    Labeler = value => $"{value:F0} USD",
                    TextSize = 12
                }
            };

            OnPropertyChanged(nameof(DailyPnLSeries));
            OnPropertyChanged(nameof(DailyPnLAxisX));
            OnPropertyChanged(nameof(DailyPnLAxisY));
        }



        /// <summary>
        /// Applies the selected date range for filtering trades.
        /// </summary>
        /// <param name="type"></param>
        private void ApplyDateRange(DateRangeType type)
        {
            SelectedRangeType = type;

            DateTime now = DateTime.Now;

            switch (type)
            {
                case DateRangeType.ThisWeek:
                    int delta = (int)now.DayOfWeek - (int)DayOfWeek.Monday;
                    if (delta < 0) delta += 7;
                    SelectedFromDate = now.Date.AddDays(-delta);
                    SelectedToDate = now.Date;
                    break;

                case DateRangeType.ThisMonth:
                    SelectedFromDate = new DateTime(now.Year, now.Month, 1);
                    SelectedToDate = now.Date;
                    break;

                case DateRangeType.ThisYear:
                    SelectedFromDate = new DateTime(now.Year, 1, 1);
                    SelectedToDate = now.Date;
                    break;

                case DateRangeType.All:
                    SelectedFromDate = DateTime.MinValue;
                    SelectedToDate = DateTime.MaxValue;
                    break;

                case DateRangeType.Custom:
                    break;
            }
            LoadStats();
            OnPropertyChanged(nameof(ActiveRangeText));
        }

        /// <summary>
        /// Gets the text for the active date range.
        /// </summary>
        public string ActiveRangeText
        {
            get
            {
                if (SelectedRangeType == DateRangeType.All)
                    return "Showing stats for: All Time";

                return $"Showing stats for: {SelectedFromDate:MMM d, yyyy} – {SelectedToDate:MMM d, yyyy}";
            }
        }

        /// <summary>
        /// Event handler for when the custom date range is requested.
        /// </summary>
        public Func<DateTime, DateTime, Task>? RequestCustomDateRangePopup { get; set; }

        /// <summary>
        /// Applies initial date range.
        /// </summary>
        public void ApplyInitialRange()
        {
            ApplyDateRange(SelectedRangeType);
        }
    }
}
