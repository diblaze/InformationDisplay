using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace uwInformationDisplay
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ButtonHamburger_OnClick(object sender, RoutedEventArgs e)
        {
            SplitViewHamburger.IsPaneOpen = !SplitViewHamburger.IsPaneOpen;
        }

        private void ListBoxHamburger_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClosePane();

            if (ListBoxItemInformation.IsSelected)
            {
            }
            else if (ListBoxItemMap.IsSelected)
            {
            }
            else if (ListBoxItemTimetable.IsSelected)
            {
                MainFrame.Navigate(typeof (Timetable));
            }
        }

        private void ClosePane()
        {
            if (SplitViewHamburger.IsPaneOpen)
            {
                SplitViewHamburger.IsPaneOpen = false;
            }
        }

        private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}