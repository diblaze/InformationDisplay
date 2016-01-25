using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;

namespace winAppInfoScreen
{
    public class TimetableManager
    {
        /// <summary>
        ///     Gets the current week number.
        /// </summary>
        /// <returns>Current week number</returns>
        public static int GetWeekNumber()
        {
            //get todays date to calculate week
            DateTime todayTime = DateTime.Today;

            //get current device culture
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(todayTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }

        public static async Task<BitmapImage> DownloadTimetable(string classToDownload, double canvasHeight, double canvasWidth)
        {
            int week = GetWeekNumber();

            //new instance of stringbuilder to create our request URL.
            var sb = new StringBuilder();
            sb.Append(
                      @"http://www.novasoftware.se/ImgGen/schedulegenerator.aspx?format=png&schoolid=83020/sv-se&type=0&id=");
            sb.Append(classToDownload);
            sb.Append(@"|step&period=&week=");
            sb.Append(week);
            sb.Append(@"&mode=0&printer=0&colors=32&head=1&clock=0&foot=1&day=0&width=");
            sb.Append(canvasWidth);
            sb.Append(@"&height=");
            sb.Append(canvasHeight);
            sb.Append(@"&count=1&decrypt=0");

            string urlToDownload = sb.ToString();

            //get location to Cache Folder to check if schedule is already downloaded
            StorageFolder localCache = ApplicationData.Current.LocalCacheFolder;
            var timetableFile = (StorageFile) await localCache.TryGetItemAsync(classToDownload + ".png");

            if (timetableFile != null)
            {
                var dialog = new MessageDialog("Schedule already downloaded");
                await dialog.ShowAsync();
                return null;
            }

            var uri = new Uri(urlToDownload);
            var downloader = new BackgroundDownloader();
            StorageFile downloadedTimetable =
                await localCache.CreateFileAsync(classToDownload + ".png", CreationCollisionOption.ReplaceExisting);
            DownloadOperation download = downloader.CreateDownload(uri, downloadedTimetable);
            await download.StartAsync().AsTask();

            //change TimetableImage to downloaded timetable image
            //BitmapImage bitmapImage = new BitmapImage(new Uri(localCache + "/timetables/" + fileName + ".png"));
            //TimetableImage.Source = bitmapImage;
            var image = new BitmapImage(new Uri(localCache.Path + "/" + classToDownload + ".png"));
            return image;
        }
    }
}