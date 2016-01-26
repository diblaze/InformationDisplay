using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using uwpPlatenInformationScreen.Models;

namespace uwpPlatenInformationScreen.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WelcomePage : Page
    {
        private ObservableCollection<RootObject> lunchMenu; 
        public WelcomePage()
        {
            this.InitializeComponent();
            LoadLunchMenu();
            lunchMenu = new ObservableCollection<RootObject>();
        }

        private async void LoadLunchMenu()
        {
            //var todaysLunch = await LunchManager.GetTodaysLunch(lunchMenu);

        }
    }
}
