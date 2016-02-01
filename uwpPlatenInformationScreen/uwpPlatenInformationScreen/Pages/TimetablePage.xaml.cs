using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using uwpPlatenInformationScreen.Managers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace uwpPlatenInformationScreen.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TimetablePage
    {
        //Because of NovaSoftware not allowing me to use their API, I needed to make my own workaround.
        public ObservableCollection<string> ClassCollection = new ObservableCollection<string>(new[]
                                                                                               {
                                                                                                   "EK13A",
                                                                                                   "EK13B",
                                                                                                   "EK14A",
                                                                                                   "EK14B",
                                                                                                   "EK15A",
                                                                                                   "EK15B",
                                                                                                   "HU13",
                                                                                                   "HU14",
                                                                                                   "HU15",
                                                                                                   "NA13A",
                                                                                                   "NA13B",
                                                                                                   "NA13C",
                                                                                                   "NA14A",
                                                                                                   "NA14B",
                                                                                                   "NA15A",
                                                                                                   "NA15B",
                                                                                                   "SA13",
                                                                                                   "SA14",
                                                                                                   "SA15A",
                                                                                                   "SA15B",
                                                                                                   "TE13",
                                                                                                   "TE14",
                                                                                                   "TE15A",
                                                                                                   "TE15B"
                                                                                               });

        public TimetablePage()
        {
            InitializeComponent();

            ComboBoxChooseTimetable.ItemsSource = ClassCollection;
        }

        //Has an user selected a timetable?
        private async void ComboBoxChooseTimetable_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox) sender;
            string selectedClass = comboBox?.SelectedItem?.ToString();

            if (selectedClass == null)
            {
                return;
            }

            //Update canvas dimensions
            ImageCanvas.Height = MainGrid.ActualHeight;
            ImageCanvas.Width = MainGrid.ActualWidth;

            BitmapImage timetableImage =
                await TimetableManager.GetTimetable(selectedClass, ImageCanvas.Height - 100, ImageCanvas.Width);

            ShowTimetable(timetableImage);
        }

        /// <summary>
        ///     Sets the canvas to show the downloaded timetable.
        /// </summary>
        /// <param name="imageSource">Image source to downloaded timetable.</param>
        private void ShowTimetable(ImageSource imageSource)
        {
            //convert into a element that the canvas control can handle
            var timetableImage = new Image {Source = imageSource};
            //clear canvas from other images
            ImageCanvas.Children.Clear();
            ImageCanvas.Children.Add(timetableImage);
        }
    }
}