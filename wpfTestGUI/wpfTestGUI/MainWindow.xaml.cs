using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpfTestGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
        {

        private string _showing = "";
        public MainWindow()
        {
            /*
            <!-- Header = #379bf9
            --!>
            */

            
            
            InitializeComponent();
            ContentFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }

        private void NavigateToPage( object sender, MouseButtonEventArgs e )
        {
            ListBoxItem tabSelected = sender as ListBoxItem;

            if ( tabSelected == null )
            {
                return;
            }

            switch ( tabSelected.Name )
            {
                case "lbiInformation":
                    ContentFrame.Navigate( new Information() );
                    _showing = "Information";
                    break;
                case "lbiTimetable":
                    ContentFrame.Navigate( new Timetable() );
                    _showing = "Timetable";
                    break;
                case "lbiMap":
                    ContentFrame.Content = null;
                    _showing = "Map";
                    break;
                default:
                    ContentFrame.Content = null;
                    _showing = "";
                    break;
            }
        }
    }
}
