using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace uwpPlatenInformationScreen.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Map : Page
    {
        public Map()
        {
            InitializeComponent();
        }

        private void BtnSearchRoom_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (SearchRoom));
        }

        private void BtnSearchFloor_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (SearchFloor));
        }
    }
}