using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace uwpPlatenInformationScreen.Managers
{
    /// <summary>
    ///     Because of NovaSoftware not allowing me to use their API, I needed to make my own workaround.
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
            //get week number
            int week = GetWeekNumber();

            //using string interpolation to format the request url
            string urlToDownload =
                $@"http://www.novasoftware.se/ImgGen/schedulegenerator.aspx?format=png&schoolid=83020/sv-se&type=0&id={
                    classToDownload}|step&period=&week={week}&mode=0&printer=0&colors=32&head=1&clock=0&foot=1&day=0&width={
                    canvasWidth}&height={canvasHeight}&count=1&decrypt=0";

            //get location to Cache Folder to check if schedule is already downloaded
            StorageFolder localCache = ApplicationData.Current.LocalCacheFolder;
            //delete all timetables from earlier weeks
            await DeleteOldTimetables(localCache, week);
            //is the timetable already downloaded?
            var timetableFile = (StorageFile) await localCache.TryGetItemAsync(classToDownload + ".Week " + week + ".png");

            //change TimetableImage to already downloaded timetable image (if we do not need to download new)
            if (timetableFile != null)
            {
                bool downloadNew = false;
                var oldImage = new BitmapImage(new Uri(timetableFile.Path));

                //Check if the old image has bad dimensions.
                downloadNew = await CheckIfBadDimensions(canvasHeight, timetableFile);

                if (!downloadNew)
                {
                    return oldImage;
                }
            }

            await DownloadImageAsync(classToDownload, urlToDownload, localCache);

            //change TimetableImage to downloaded timetable image
            var image = new BitmapImage(new Uri(localCache.Path + "/" + classToDownload + ".Week " + week + ".png"));
            return image;
        }

        /// <summary>
        ///     Deletes all old timetables.
        /// </summary>
        /// <param name="localCache"><c>Local cache</c> folder</param>
        /// <param name="week">Current <c>week number</c></param>
        /// <returns></returns>
        private static async Task DeleteOldTimetables(IStorageFolder localCache, int week)
        {
            var filesInFolder = await localCache.GetFilesAsync();
            foreach (StorageFile file in filesInFolder.Where(file => !file.Name.Contains(".Week " + week)))
            {
                await file.DeleteAsync();
            }
        }

        /// <summary>
        ///     Checks if the current timetable image has the wrong dimensions.
        /// </summary>
        /// <param name="canvasHeight">Height of the <c>canvas</c></param>
        /// <param name="timetableFile">
        ///     <c>Timetable image</c>
        /// </param>
        /// <returns></returns>
        private static async Task<bool> CheckIfBadDimensions(double canvasHeight, IStorageFile timetableFile)
        {
            bool downloadNew = false;
            //Bitmap decoder to read width and height from PNG.
            BitmapDecoder bitmapDecoder = null;
            try
            {
                //Open the file async
                using (IRandomAccessStream stream = await timetableFile.OpenAsync(FileAccessMode.Read))
                {
                    bitmapDecoder = await BitmapDecoder.CreateAsync(stream);
                }
            }
                //Sometimes do not work, can't figure out what the problem is. This "catch" helps me avoid crashes.
            catch (Exception)
            {
                // ignored
            }

            if (bitmapDecoder != null)
            {
                //If the image does not have the same height as the canvas, then download new one
                if (bitmapDecoder.PixelHeight != canvasHeight)
                {
                    downloadNew = true;
                }
            }
            //if our Try Catch failed, then just download new image either way.
            else
            {
                downloadNew = true;
            }

            return downloadNew;
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
            //get week number
            int week = GetWeekNumber();

            //create uri from Url
            var uri = new Uri(urlToDownload);

            //create file to write to
            StorageFile downloadedTimetable =
                await
                localCache.CreateFileAsync(classToDownload + ".Week " + week + ".png", CreationCollisionOption.ReplaceExisting);

            //download image
            var downloader = new BackgroundDownloader();
            DownloadOperation download = downloader.CreateDownload(uri, downloadedTimetable);
            await download.StartAsync().AsTask();
        }
    }
}