using System.Collections.ObjectModel;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WindowsAppTimetable;
using WindowsAppTimetable.NovaSoftware;

namespace Sample.Views
{
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<ScheduleID> ScheduleIds { get; set; }

        public MainPage()
        {
            InitializeComponent();

            ScheduleIds = new ObservableCollection<ScheduleID>();


        }

        private async void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            prDownload.IsActive = true;
            prDownload.Visibility = Visibility.Visible;

            if (ScheduleIds.Count > 0)
            {
                ScheduleIds.Clear();
            }

            await ScheduleFacade.PopulateScheduleIdsAsync(ScheduleIds);
            //await ScheduleFacade.DownloadSpecificScheduleAsync("9A2C3C57-CC60-4AF2-8F2E-DA599A7F723E");

            prDownload.IsActive = false;
            prDownload.Visibility = Visibility.Collapsed;
            
        }
    }
}
