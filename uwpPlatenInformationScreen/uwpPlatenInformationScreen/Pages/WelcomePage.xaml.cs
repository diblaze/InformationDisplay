using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using uwpPlatenInformationScreen.Models;

namespace uwpPlatenInformationScreen.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WelcomePage : Page
    {
        private static RootObject lunchMenu;

        public WelcomePage()
        {
            InitializeComponent();

            LoadLunchMenu();
        }

        private void PopulateLunchText()
        {
            DateTime todayDateTime = DateTime.Today;
            string nameOfDay = todayDateTime.DayOfWeek.ToString();

            //If computer is set in English, overwrite into Swedish weekdays.
            switch (nameOfDay)
            {
                case "Monday":
                    nameOfDay = "Måndag";
                    break;
                case "Tuesday":
                    nameOfDay = "Tisdag";
                    break;
                case "Wednesday":
                    nameOfDay = "Onsdag";
                    break;
                case "Thursday":
                    nameOfDay = "Torsdag";
                    break;
                case "Friday":
                    nameOfDay = "Friday";
                    break;
                default:
                    break;
            }

            if (lunchMenu == null)
            {
                return;
            }

            var stringBuilder = new StringBuilder();
            List<DayMenu> temp;

            switch (nameOfDay)
            {
                case "Måndag":
                    stringBuilder.AppendLine("Måndag");
                    temp = lunchMenu.Weeks[0].Days[0].DayMenus;
                    foreach (DayMenu dayMenu in temp)
                    {
                        stringBuilder.Append(dayMenu.MenuAlternativeName + ":");
                        stringBuilder.AppendLine(dayMenu.DayMenuName);
                    }
                    break;
                case "Tisdag":
                    stringBuilder.AppendLine("Tisdag");
                    temp = lunchMenu.Weeks[0].Days[1].DayMenus;
                    foreach (DayMenu dayMenu in temp)
                    {
                        stringBuilder.Append(dayMenu.MenuAlternativeName + ":");
                        stringBuilder.AppendLine(dayMenu.DayMenuName);
                    }
                    break;
                case "Onsdag":
                    stringBuilder.AppendLine("Onsdag");
                    temp = lunchMenu.Weeks[0].Days[2].DayMenus;
                    foreach (DayMenu dayMenu in temp)
                    {
                        stringBuilder.Append(dayMenu.MenuAlternativeName + ":");
                        stringBuilder.AppendLine(dayMenu.DayMenuName);
                    }
                    break;
                case "Torsdag":
                    stringBuilder.AppendLine("Tisdag");
                    temp = lunchMenu.Weeks[0].Days[3].DayMenus;
                    foreach (DayMenu dayMenu in temp)
                    {
                        stringBuilder.Append(dayMenu.MenuAlternativeName + ":");
                        stringBuilder.AppendLine(dayMenu.DayMenuName);
                    }
                    break;
                case "Fredag":
                    stringBuilder.AppendLine("Fredag");
                    temp = lunchMenu.Weeks[0].Days[4].DayMenus;
                    foreach (DayMenu dayMenu in temp)
                    {
                        stringBuilder.Append(dayMenu.MenuAlternativeName + ":");
                        stringBuilder.AppendLine(dayMenu.DayMenuName);
                    }
                    break;
            }

            string createdString = stringBuilder.ToString();
            TextTodaysFood.Text = createdString;
            

        }

        private async void LoadLunchMenu()
        {
            lunchMenu = new RootObject();
            lunchMenu = await LunchManager.GetTodaysLunch();
            PopulateLunchText();
        }
    }
}