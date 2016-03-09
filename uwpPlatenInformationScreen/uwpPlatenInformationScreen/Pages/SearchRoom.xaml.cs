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
    public sealed partial class SearchRoom : Page
    {
        public SearchRoom()
        {
            InitializeComponent();
            
        }

        /// <summary>
        ///     Automates the suggest box search room_ on suggestion chosen.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="AutoSuggestBoxSuggestionChosenEventArgs" /> instance containing the event data.</param>
        private async void AutoSuggestBoxSearchRoom_OnSuggestionChosen(AutoSuggestBox sender,
                                                                       AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            //room selected
            string room = args.SelectedItem as string;
            ImageCanvas.Width = MainGrid.ActualWidth;
            ImageCanvas.Height = MainGrid.ActualHeight;

            BitmapImage roomImage = await MapSearcher.GetRoomImage(room);
            if (roomImage == null)
            {
                return;
            }
            DisplayImage(roomImage, ImageCanvas.Width, ImageCanvas.Height);
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
            var roomImage = new Image {Source = image, Width = width, Height = height};
            //clear canvas from other images
            ImageCanvas.Children.Clear();

            ImageCanvas.Children.Add(roomImage);
            Canvas.SetTop(roomImage, 0);
            Canvas.SetLeft(roomImage, 0);
        }

        /// <summary>
        ///     Automates the suggest box search room_ on text changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="AutoSuggestBoxTextChangedEventArgs" /> instance containing the event data.</param>
        private void AutoSuggestBoxSearchRoom_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            //if reason for text changed was not user input, ignore.
            if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput)
            {
                return;
            }

            //text inputed by user
            string textChanged = sender.Text;

            //find all rooms with the matching inputed text
            var matchingRooms = MapSearcher.SearchForRoomAsync(textChanged);

            //set the autosuggestbox to show all matching rooms
            AutoSuggestBoxSearchRoom.ItemsSource = matchingRooms;
        }
    }
}