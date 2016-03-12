using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Popups;
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
    public sealed partial class SearchFloor : Page
    {
        //temp holder for floor picked
        private string _floor = "";
        //temp holder for house picked
        private string _house = "";

        public SearchFloor()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Handles the OnSelectionChanged event of the ComboBoxPickHouse control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs" /> instance containing the event data.</param>
        private void ComboBoxPickHouse_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //get the combobox
            var comboBox = (ComboBox) sender;
            //get the selecteditem
            var comboItem = comboBox?.SelectedItem as ComboBoxItem;
            //get the string from the selecteditem.
            string house = comboItem?.Content?.ToString();

            //if there is not string, an error has occured.
            if (house == null)
            {
                return;
            }

            _house = house;
            //populate the combobox according to the picked house.
            PopulateComboBox(house);
            //activate the combobox.
            ComboBoxPickFloor.IsEnabled = true;
        }

        /// <summary>
        ///     Populates the ComboBox.
        /// </summary>
        /// <param name="house">The house picked.</param>
        private void PopulateComboBox(string house)
        {
            switch (house)
            {
                case "A":
                    ComboBoxPickFloor.ItemsSource = _houseAList;
                    break;
                //case "E":
                //    ComboBoxPickFloor.ItemsSource = houseEList;
                //    break;
                case "C":
                    ComboBoxPickFloor.ItemsSource = houseCList;
                    break;
                case "D":
                    ComboBoxPickFloor.ItemsSource = houseDList;
                    break;
            }
        }

        /// <summary>
        ///     Displays the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        private void DisplayImage(ImageSource image, double width, double height)
        {
            //convert into a element that the canvas control can handle
            var floorImage = new Image {Source = image, Width = width, Height = height};
            //clear canvas from other images
            ImageCanvas.Children.Clear();
            //add the picture to the canvas.
            ImageCanvas.Children.Add(floorImage);
            Canvas.SetTop(floorImage, 0);
            Canvas.SetLeft(floorImage, 0);
        }

        /// <summary>
        ///     Handles the OnSelectionChanged event of the ComboBoxPickFloor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs" /> instance containing the event data.</param>
        private async void ComboBoxPickFloor_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //get the combobox
            var comboBox = (ComboBox) sender;
            //get the string
            string floor = comboBox?.SelectedItem as string;

            //if the string is empty, then an error occured.
            if (floor == null)
            {
                return;
            }

            _floor = floor;

            //get image
            await GetFloorImageAsync();
        }

        /// <summary>
        ///     Gets the floor image asynchronous.
        /// </summary>
        private async Task GetFloorImageAsync()
        {
            //get correct floor image
            BitmapImage floorImage = await MapSearcher.GetFloorImageAsync(_house, _floor);
            //if we have not got a floor image back
            if (floorImage == null)
            {
                //show error
                var errorDialog = new MessageDialog("Fel vid visning av bild", "Fel");
                await errorDialog.ShowAsync();
                return;
            }
            //if we have a floor image, show image.
            ImageCanvas.Width = MainGrid.ActualWidth - 100;
            ImageCanvas.Height = MainGrid.ActualHeight - 100;
            DisplayImage(floorImage, ImageCanvas.Width, ImageCanvas.Height);
        }

        #region houseLists

        private readonly List<string> _houseAList = new List<string> {"A200", "A300", "A400"};
        //private readonly List<string> houseEList = new List<string> { "E200", "B300", "B400" };
        private readonly List<string> houseCList = new List<string> {"C200", "C300", "C400"};
        private readonly List<string> houseDList = new List<string> {"D200", "D300", "D400"};

        #endregion
    }
}