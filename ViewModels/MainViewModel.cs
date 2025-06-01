using System.Windows.Input;

namespace TradingJournalApp.ViewModels
{
    /// <summary>
    /// ViewModel for the main page.
    /// </summary>
    public enum SelectedTab
    {
        Journal,
        Analytics
    }

    /// <summary>
    /// ViewModel for the main page.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        /// <summary>
        /// Current selected tab.
        /// </summary>
        private SelectedTab currentTab = SelectedTab.Journal;

        /// <summary>
        /// Property that indicates the current selected tab.
        /// </summary>
        public SelectedTab CurrentTab
        {
            get => currentTab;
            set
            {
                currentTab = value;
                OnPropertyChanged(nameof(CurrentTab));
                OnPropertyChanged(nameof(IsJournalTab));
                OnPropertyChanged(nameof(IsAnalyticsTab));
            }
        }

        /// <summary>
        /// Property that indicates if the Journal tab is selected.
        /// </summary>
        public bool IsJournalTab => CurrentTab == SelectedTab.Journal;

        /// <summary>
        /// Property that indicates if the Analytics tab is selected.
        /// </summary>
        public bool IsAnalyticsTab => CurrentTab == SelectedTab.Analytics;

        /// <summary>
        /// Command to select the Journal tab.
        /// </summary>
        public ICommand SelectJournalCommand { get; }

        /// <summary>
        /// Command to select the Analytics tab.
        /// </summary>
        public ICommand SelectAnalyticsCommand { get; }

        /// <summary>
        /// The constructor for MainViewModel.
        /// </summary>
        public MainViewModel()
        {
            SelectJournalCommand = new Command(() => CurrentTab = SelectedTab.Journal);
            SelectAnalyticsCommand = new Command(() => CurrentTab = SelectedTab.Analytics);
        }
    }
}
