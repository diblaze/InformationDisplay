using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using wpfTestGUI.Properties;

namespace wpfTestGUI
    {
    /// <summary>
    ///     Interaction logic for Timetable.xaml
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

        public Timetable( )
        {
            InitializeComponent();


            DateTime todayTime = DateTime.Today;
            //If today is a monday then set mondayInit to true.
            if ( todayTime.DayOfWeek == DayOfWeek.Monday )
            {
                Settings.Default.mondayInit = true;
                mondayInit = Settings.Default.mondayInit;
            }

            PopulateList();
        }

        /// <summary>
        ///     Populates the classID list by reading a external file.
        /// </summary>
        private void PopulateList( )
        {
            string path = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );
            if ( !File.Exists( path + "/" + "ClassIDS.txt" ) )
            {
                return;
            }


            using ( StreamReader sr = new StreamReader( path + "/" + "ClassIDS.txt" ) )
            {
                string line;
                while ( ( line = sr.ReadLine() ) != null )
                {
                    string[] lineSplit = line.Split( '@' );

                    string classId = lineSplit[1];
                    classId = classId.Trim( ' ' );

                    classIDlist.Add( classId );
                }
            }
        }

        /// <summary>
        ///     Changes the imageframe to show the correct timetable choosen by the user.
        /// </summary>
        /// <param name="sender"><c>Combobox</c> where the event took place.</param>
        /// <param name="selectionChangedEventArgs"></param>
        public void ChangeTimetable( object sender, SelectionChangedEventArgs selectionChangedEventArgs )
        {
            //get combobox
            ComboBox comboItem = sender as ComboBox;
            //if null then return
            if ( comboItem == null )
            {
                return;
            }

            //get the selecteditem
            ComboBoxItem cbi = ( ComboBoxItem ) comboItem.SelectedItem;

            //check what class the user selected, download accordingly
            switch ( cbi?.Content.ToString() )
            {
                    #region Download ID

                case "Ek13A":
                    DownloadTimetable( SchoolClasses.Ek13A, "EK13A" );
                    break;
                case "Ek13B":
                    DownloadTimetable( SchoolClasses.Ek13B, "EK13B" );
                    break;
                case "Ek14A":
                    DownloadTimetable( SchoolClasses.Ek14A, "EK14A" );
                    break;
                case "Ek14B":
                    DownloadTimetable( SchoolClasses.Ek14B, "EK14B" );
                    break;
                case "Ek15A":
                    DownloadTimetable( SchoolClasses.Ek15A, "EK15A" );
                    break;
                case "Ek15B":
                    DownloadTimetable( SchoolClasses.Ek15B, "EK15B" );
                    break;
                case "Hu13":
                    DownloadTimetable( SchoolClasses.Hu13, "HU13" );
                    break;
                case "Hu14":
                    DownloadTimetable( SchoolClasses.Hu14, "HU14" );
                    break;
                case "Hu15":
                    DownloadTimetable( SchoolClasses.Hu15, "HU15" );
                    break;
                case "Na13A":
                    DownloadTimetable( SchoolClasses.Na13A, "NA13A" );
                    break;
                case "Na13B":
                    DownloadTimetable( SchoolClasses.Na13B, "NA13B" );
                    break;
                case "Na13C":
                    DownloadTimetable( SchoolClasses.Na13C, "NA13C" );
                    break;
                case "Na14A":
                    DownloadTimetable( SchoolClasses.Na14A, "NA14A" );
                    break;
                case "Na14B":
                    DownloadTimetable( SchoolClasses.Na14B, "NA14B" );
                    break;
                case "Na15A":
                    DownloadTimetable( SchoolClasses.Na15A, "NA15A" );
                    break;
                case "Na15B":
                    DownloadTimetable( SchoolClasses.Na15B, "NA15B" );
                    break;
                case "Sa13":
                    DownloadTimetable( SchoolClasses.Sa13, "SA13" );
                    break;
                case "Sa14":
                    DownloadTimetable( SchoolClasses.Sa14, "SA14" );
                    break;
                case "Sa15A":
                    DownloadTimetable( SchoolClasses.Sa15A, "SA15A" );
                    break;
                case "Sa15B":
                    DownloadTimetable( SchoolClasses.Sa15B, "SA15B" );
                    break;
                case "Te13":
                    DownloadTimetable( SchoolClasses.Te13, "TE13" );
                    break;
                case "Te14":
                    DownloadTimetable( SchoolClasses.Te14, "TE14" );
                    break;
                case "Te15A":
                    DownloadTimetable( SchoolClasses.Te15A, "TE15A" );
                    break;
                case "Te15B":
                    DownloadTimetable( SchoolClasses.Te15B, "TE15B" );
                    break;

                    #endregion
            }
        }

        /// <summary>
        ///     Gets the current week number.
        /// </summary>
        /// <param name="dtPassed"><c>DateTime</c> for today</param>
        /// <returns></returns>
        public static int GetWeekNumber( DateTime dtPassed )
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear( dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday );
            return weekNum;
        }

        /// <summary>
        ///     Download the <c>timetable</c> for a specific class id.
        /// </summary>
        /// <param name="classToDownload">Position of class in list.</param>
        /// <param name="fileName">Name of image file</param>
        private void DownloadTimetable( SchoolClasses classToDownload, string fileName )
        {
            //Store our specific classId by using classToDownload.
            string urlId = classIDlist[( int ) classToDownload];
            DateTime todayTime = DateTime.Today;

            int week = GetWeekNumber( todayTime );

            StringBuilder sb = new StringBuilder();
            sb.Append(
                @"http://www.novasoftware.se/ImgGen/schedulegenerator.aspx?format=png&schoolid=83020/sv-se&type=1&id={" );
            sb.Append( urlId );
            sb.Append( @"}&period=&" );
            sb.Append( "week=" + week + "&" );
            sb.Append(
                "mode=0&printer=0&colors=32&head=0&clock=0&foot=0&day=0&width=1365&height=913&maxwidth=1365&maxheight=913)" );

            string urlToDownload = sb.ToString();

            //get location to .exe to later find downloaded image
            string path = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );

            //if the timetable already exists
            if ( File.Exists( path + "/" + "timetables/" + fileName + ".png" ) )
            {
                //if today is monday and we have to redownload all files
                if ( todayTime.DayOfWeek == DayOfWeek.Monday && mondayInit )
                {
                    DeleteAllFiles( path ); //remove all timetables
                    ForceDownload( urlToDownload, fileName, path ); //force a download
                    Settings.Default.mondayInit = false; //set mondayInit to false because we have init already
                    Settings.Default.Save(); //save incase of reboot.
                }
            }
            //else download new
            else
            {
                using ( WebClient webClient = new WebClient() )
                {
                    //Download timetable to image for future use - cuts download requirement
                    webClient.DownloadFile( new Uri( urlToDownload ), path + "/" + "timetables/" + fileName + ".png" );
                }
            }

            //change TimetableImage to downloaded timetable image
            BitmapImage bitmapImage = new BitmapImage( new Uri( path + "/timetables/" + fileName + ".png" ) );
            TimetableImage.Source = bitmapImage;
        }

        /// <summary>
        ///     Deletes all folders in a specific path.
        /// </summary>
        /// <param name="path">Path to a folder where the <c>deleting</c> will take place.</param>
        private static void DeleteAllFiles( string path )
        {
            DirectoryInfo directory = new DirectoryInfo( path + "/timetables" );

            foreach ( FileInfo file in directory.GetFiles() )
            {
                file.Delete();
            }
        }

        /// <summary>
        ///     Used when a force refresh is needed.
        /// </summary>
        /// <param name="urlToDownload">URL to download from.</param>
        /// <param name="fileName">The timetable file name.</param>
        /// <param name="path">Path to <c>timetable</c> folder.</param>
        private static void ForceDownload( string urlToDownload, string fileName, string path )
        {
            using ( WebClient webClient = new WebClient() )
            {
                //Download timetable to image for future use - cuts download requirement
                webClient.DownloadFile( new Uri( urlToDownload ), path + "/" + "timetables/" + fileName + ".png" );
            }
        }
        }
    }