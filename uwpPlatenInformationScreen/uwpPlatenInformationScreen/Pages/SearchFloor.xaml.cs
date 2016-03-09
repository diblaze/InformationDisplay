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
        public SearchFloor()
        {
            InitializeComponent();
        }

        private async void AutoSuggestBoxSearchRoom_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox) sender;
            ComboBoxItem comboItem = comboBox?.SelectedItem as ComboBoxItem;
            var house = comboItem?.Content?.ToString();

            if (house == null)
            {
                return;
            }

            BitmapImage floorImage = await MapSearcher.GetFloorImage(house);
            if (floorImage == null)
            {
                return;
            }
            DisplayImage(floorImage, ImageCanvas.Width, ImageCanvas.Height);
        }

        private void DisplayImage(ImageSource image, double width, double height)
        {
            //convert into a element that the canvas control can handle
            var floorImage = new Image {Source = image, Width = width, Height = height};
            //clear canvas from other images
            ImageCanvas.Children.Clear();

            ImageCanvas.Children.Add(floorImage);
            Canvas.SetTop(floorImage, 0);
            Canvas.SetLeft(floorImage, 0);
        }
    }
}