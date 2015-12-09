using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Windows.ApplicationModel;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace uwInformationDisplay
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    //SchoolClasses made for easier and understandable access to classIDlist
    internal enum SchoolClasses
    {
        Ek13A,
        Ek13B,
        Ek14A,
        Ek14B,
        Ek15A,
        Ek15B,
        Hu13,
        Hu14,
        Hu15,
        Na13A,
        Na13B,
        Na13C,
        Na14A,
        Na14B,
        Na15A,
        Na15B,
        Sa13,
        Sa14,
        Sa15A,
        Sa15B,
        Te13,
        Te14,
        Te15A,
        Te15B
    }

    public partial class Timetable
    {
        private readonly List<string> classIDlist = new List<string>();
        private readonly bool mondayInit;

        public Timetable()
        {
            InitializeComponent();


            var todayTime = DateTime.Today;
            //If today is a monday then set mondayInit to true.
            //TODO: Monday check.
            //if (todayTime.DayOfWeek == DayOfWeek.Monday)
            //{
            //    Settings.Default.mondayInit = true;
            //    mondayInit = Settings.Default.mondayInit;
            //}

            PopulateList();
        }

        /// <summary>
        ///     Populates the classID list by reading a external file.
        /// </summary>
        private async void PopulateList()
        {
            var installFolder = Package.Current.InstalledLocation;
            var file = await installFolder.GetFileAsync("ClassIDS.txt");
            if (file == null)
            {
                return;
            }

            var text = await FileIO.ReadLinesAsync(file);
            foreach (var lines in text)
            {
                var lineSplit = lines.Split('@');

                var classId = lineSplit[1];
                classId = classId.Trim(' ');

                classIDlist.Add(classId);
            }
        }

        /// <summary>
        ///     Changes the imageframe to show the correct timetable choosen by the user.
        /// </summary>
        /// <param name="sender"><c>Combobox</c> where the event took place.</param>
        /// <param name="selectionChangedEventArgs"></param>
        public void ChangeTimetable(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            //get combobox
            var comboItem = sender as ComboBox;
            //if null then return
            if (comboItem == null)
            {
                return;
            }

            //get the selecteditem
            var cbi = (ComboBoxItem) comboItem.SelectedItem;

            //check what class the user selected, download accordingly
            switch (cbi?.Content?.ToString())
            {
                    #region Download ID

                case "Ek13A":
                    DownloadTimetable(SchoolClasses.Ek13A, "EK13A");
                    break;
                case "Ek13B":
                    DownloadTimetable(SchoolClasses.Ek13B, "EK13B");
                    break;
                case "Ek14A":
                    DownloadTimetable(SchoolClasses.Ek14A, "EK14A");
                    break;
                case "Ek14B":
                    DownloadTimetable(SchoolClasses.Ek14B, "EK14B");
                    break;
                case "Ek15A":
                    DownloadTimetable(SchoolClasses.Ek15A, "EK15A");
                    break;
                case "Ek15B":
                    DownloadTimetable(SchoolClasses.Ek15B, "EK15B");
                    break;
                case "Hu13":
                    DownloadTimetable(SchoolClasses.Hu13, "HU13");
                    break;
                case "Hu14":
                    DownloadTimetable(SchoolClasses.Hu14, "HU14");
                    break;
                case "Hu15":
                    DownloadTimetable(SchoolClasses.Hu15, "HU15");
                    break;
                case "Na13A":
                    DownloadTimetable(SchoolClasses.Na13A, "NA13A");
                    break;
                case "Na13B":
                    DownloadTimetable(SchoolClasses.Na13B, "NA13B");
                    break;
                case "Na13C":
                    DownloadTimetable(SchoolClasses.Na13C, "NA13C");
                    break;
                case "Na14A":
                    DownloadTimetable(SchoolClasses.Na14A, "NA14A");
                    break;
                case "Na14B":
                    DownloadTimetable(SchoolClasses.Na14B, "NA14B");
                    break;
                case "Na15A":
                    DownloadTimetable(SchoolClasses.Na15A, "NA15A");
                    break;
                case "Na15B":
                    DownloadTimetable(SchoolClasses.Na15B, "NA15B");
                    break;
                case "Sa13":
                    DownloadTimetable(SchoolClasses.Sa13, "SA13");
                    break;
                case "Sa14":
                    DownloadTimetable(SchoolClasses.Sa14, "SA14");
                    break;
                case "Sa15A":
                    DownloadTimetable(SchoolClasses.Sa15A, "SA15A");
                    break;
                case "Sa15B":
                    DownloadTimetable(SchoolClasses.Sa15B, "SA15B");
                    break;
                case "Te13":
                    DownloadTimetable(SchoolClasses.Te13, "TE13");
                    break;
                case "Te14":
                    DownloadTimetable(SchoolClasses.Te14, "TE14");
                    break;
                case "Te15A":
                    DownloadTimetable(SchoolClasses.Te15A, "TE15A");
                    break;
                case "Te15B":
                    DownloadTimetable(SchoolClasses.Te15B, "TE15B");
                    break;

                    #endregion
            }
        }

        /// <summary>
        ///     Gets the current week number.
        /// </summary>
        /// <param name="dtPassed"><c>DateTime</c> for today</param>
        /// <returns></returns>
        public static int GetWeekNumber(DateTime dtPassed)
        {
            var ciCurr = CultureInfo.CurrentCulture;
            var weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }

        /// <summary>
        ///     Download the <c>timetable</c> for a specific class id.
        /// </summary>
        /// <param name="classToDownload">Position of class in list.</param>
        /// <param name="fileName">Name of image file</param>
        private async void DownloadTimetable(SchoolClasses classToDownload, string fileName)
        {
            //Store our specific classId by using classToDownload.
            var urlId = classIDlist[(int) classToDownload];
            var todayTime = DateTime.Today;

            var week = GetWeekNumber(todayTime);

            var sb = new StringBuilder();
            sb.Append(
                @"http://www.novasoftware.se/ImgGen/schedulegenerator.aspx?format=png&schoolid=83020/sv-se&type=1&id={");
            sb.Append(urlId);
            sb.Append(@"}&period=&");
            sb.Append("week=" + week + "&");
            sb.Append(
                "mode=0&printer=0&colors=32&head=0&clock=0&foot=0&day=0&width=1442&height=1240&maxwidth=1442&maxheight=1240)");

            var urlToDownload = sb.ToString();

            //get location to .exe to later find downloaded image
            //string path = Path.GetDirectoryName();
            //Uri pathToExe = BaseUri;

            //Uri pathToTimeTableFolder = new Uri(BaseUri, "/timetables/");
            var installFolder = ApplicationData.Current.LocalFolder;
            var timetableFolder = await installFolder.TryGetItemAsync("timetables") as StorageFolder;
            if (timetableFolder == null)
            {
                RecreateTimeTablesFolder();
                timetableFolder = await installFolder.TryGetItemAsync("timetables") as StorageFolder;
            }
            var timetableFile = (StorageFile) await timetableFolder.TryGetItemAsync(fileName + ".png");


            //if the timetable already exists
            if (timetableFile != null)
            {
                if (todayTime.DayOfWeek == DayOfWeek.Monday && mondayInit)
                {
                    RecreateTimeTablesFolder();
                    ForceDownload(urlToDownload, fileName, installFolder + "/timetables/");

                    //TODO: Add monday check
                }
            }
            //else download new
            else
            {
                var uri = new Uri(urlToDownload);
                var downloader = new BackgroundDownloader();
                var downloadedTimetable = await timetableFolder.CreateFileAsync(fileName + ".png",
                    CreationCollisionOption.ReplaceExisting);
                var download = downloader.CreateDownload(uri, downloadedTimetable);
                await download.StartAsync().AsTask();
            }

            //change TimetableImage to downloaded timetable image
            var uriTimetableFolder = new Uri(timetableFolder.Path);
            var bitmapImage = new BitmapImage(new Uri(uriTimetableFolder + "/" + fileName + ".png"));
            ImageTimetable.Source = bitmapImage;
        }

        /// <summary>
        ///     Deletes the timetable folder and creates it again.
        /// </summary>
        /// <param name="path">Path to a folder where the <c>deleting</c> will take place.</param>
        private static async void RecreateTimeTablesFolder()
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var timetableFolder = await storageFolder.TryGetItemAsync("timetables") as StorageFolder;

            if (timetableFolder != null)
            {
                await timetableFolder.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }

            timetableFolder = await storageFolder.CreateFolderAsync("timetables");
        }

        /// <summary>
        ///     Used when a force refresh is needed.
        /// </summary>
        /// <param name="urlToDownload">URL to download from.</param>
        /// <param name="fileName">The timetable file name.</param>
        /// <param name="path">Path to <c>timetable</c> folder.</param>
        private static async void ForceDownload(string urlToDownload, string fileName, string path)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var timetableFolder = await storageFolder.TryGetItemAsync("timetables") as StorageFolder;

            var uri = new Uri(urlToDownload);
            var downloader = new BackgroundDownloader();
            var downloadedTimetable = await timetableFolder.CreateFileAsync(fileName + ".png",
                CreationCollisionOption.ReplaceExisting);
            var download = downloader.CreateDownload(uri, downloadedTimetable);
            await download.StartAsync().AsTask();
        }
    }
}