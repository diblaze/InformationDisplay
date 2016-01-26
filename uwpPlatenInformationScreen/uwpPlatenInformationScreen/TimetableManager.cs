using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace winAppInfoScreen
{
    /// <summary>
    ///     Because of NovaSoftware not allowing me to use their API, I need to make my own workaround.
    ///     This class will take care of downloading and showing timetables.
    /// </summary>
    public class TimetableManager
    {
        /// <summary>
        ///     Gets the current week number.
        /// </summary>
        /// <returns>Current week number</returns>
        private static int GetWeekNumber()
        {
            //get todays date to calculate week
            DateTime todayTime = DateTime.Today;

            //get current device culture
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(todayTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }

        /// <summary>
        ///     Downloads timetable if it is not already downloaded, or if the downloaded timetable has expired, or if the
        ///     downloaded timetable has wrong dimensions.
        /// </summary>
        /// <param name="classToDownload"><c>Id</c> of the class to download.</param>
        /// <param name="canvasHeight">
        ///     <c>Height</c> of the page (substract <c>100</c> from this value to account for the different
        ///     rows)
        /// </param>
        /// <param name="canvasWidth"><c>Width</c> of the page (actual value)</param>
        /// <returns></returns>
        public static async Task<BitmapImage> GetTimetable(string classToDownload, double canvasHeight, double canvasWidth)
        {
            int week = GetWeekNumber();

            //using string interpolation to format the request url
            string urlToDownload =
                $@"http://www.novasoftware.se/ImgGen/schedulegenerator.aspx?format=png&schoolid=83020/sv-se&type=0&id={
                    classToDownload}|step&period=&week={week}&mode=0&printer=0&colors=32&head=1&clock=0&foot=1&day=0&width={
                    canvasWidth}&height={canvasHeight}&count=1&decrypt=0";

            //get location to Cache Folder to check if schedule is already downloaded
            StorageFolder localCache = ApplicationData.Current.LocalCacheFolder;
            var timetableFile = (StorageFile) await localCache.TryGetItemAsync(classToDownload + ".png");

            //change TimetableImage to already downloaded timetable image (if we do not need to download new)
            if (timetableFile != null)
            {
                //TODO: Add checks for old images and faulty resized images.
                var oldImage = new BitmapImage(new Uri(timetableFile.Path));
                return oldImage;
            }

            await DownloadImageAsync(classToDownload, urlToDownload, localCache);

            //change TimetableImage to downloaded timetable image
            var image = new BitmapImage(new Uri(localCache.Path + "/" + classToDownload + ".png"));
            return image;
        }

        /// <summary>
        ///     Downloads the timetable image async
        /// </summary>
        /// <param name="classToDownload"><c>Id</c> of the class to download.</param>
        /// <param name="urlToDownload"><c>Request url</c>.</param>
        /// <param name="localCache"><c>StorageFolder</c> of cache.</param>
        /// <returns></returns>
        private static async Task DownloadImageAsync(string classToDownload, string urlToDownload, StorageFolder localCache)
        {
            //create uri from Url
            var uri = new Uri(urlToDownload);

            //create file to write to
            StorageFile downloadedTimetable =
                await localCache.CreateFileAsync(classToDownload + ".png", CreationCollisionOption.ReplaceExisting);

            //download image
            var downloader = new BackgroundDownloader();
            DownloadOperation download = downloader.CreateDownload(uri, downloadedTimetable);
            await download.StartAsync().AsTask();
        }
    }
}