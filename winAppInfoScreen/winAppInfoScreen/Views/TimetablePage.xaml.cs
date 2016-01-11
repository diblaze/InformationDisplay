using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace winAppInfoScreen.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TimetablePage : Page
    {
        private readonly List<string> classList = new List<string>(new[]
                                                                   {
                                                                       "Ek13A",
                                                                       "Ek13B",
                                                                       "Ek14A",
                                                                       "Ek14B",
                                                                       "Ek15A",
                                                                       "Ek15B",
                                                                       "Hu14",
                                                                       "Hu15",
                                                                       "Na13A",
                                                                       "Na13B",
                                                                       "Na13C",
                                                                       "Na14A",
                                                                       "Hu13",
                                                                       "Na14B",
                                                                       "Na15A",
                                                                       "Na15B",
                                                                       "Sa13",
                                                                       "Sa14",
                                                                       "Sa15A",
                                                                       "Sa15B",
                                                                       "Te13",
                                                                       "Te14",
                                                                       "Te15A",
                                                                       "Te15B"
                                                                   });

        public TimetablePage()
        {
            InitializeComponent();
            PopulateComboBox();
        }

        private void PopulateComboBox()
        {
            foreach (string id in classList)
            {
                cbClass.Items?.Add(id);
            }
        }

        private async void CbClass_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //get combobox
            var comboBox = sender as ComboBox;
            //get selected item
            string classToGet = comboBox?.SelectedItem?.ToString();

            imageProgress.Visibility = Visibility.Visible;
            imageProgress.IsActive = true;
            BitmapImage image =
                await TimetableManager.DownloadTimetable(classToGet, canvasArea.ActualHeight, canvasArea.ActualWidth);
            Image imageToShow = new Image {Source = image};
            canvasArea.Children.Add(imageToShow);
            imageProgress.Visibility = Visibility.Collapsed;
            imageProgress.IsActive = false;
        }
    }
}