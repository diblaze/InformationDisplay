using System.Linq;
using Windows.UI.Xaml.Controls;
using Intense.Presentation;
using uwpPlatenInformationScreen.Pages;
using uwpPlatenInformationScreen.Presentation;

namespace uwpPlatenInformationScreen
{
    public sealed partial class Shell : UserControl
    {
        public Shell()
        {
            InitializeComponent();

            var vm = new ShellViewModel();
            vm.TopItems.Add(new NavigationItem {Icon = "", DisplayName = "Allmänt", PageType = typeof (WelcomePage)});
            vm.TopItems.Add(new NavigationItem {Icon = "", DisplayName = "Schema", PageType = typeof (TimetablePage)});
            vm.TopItems.Add(new NavigationItem {Icon = "", DisplayName = "Hitta runt", PageType = typeof (Map)});

            vm.BottomItems.Add(new NavigationItem {Icon = "", DisplayName = "About this screen", PageType = typeof (AboutPage)});

            // select the first top item
            vm.SelectedItem = vm.TopItems.First();

            ViewModel = vm;
        }

        public ShellViewModel ViewModel
        {
            get;
            private set;
        }

        public Frame RootFrame => Frame;
    }
}