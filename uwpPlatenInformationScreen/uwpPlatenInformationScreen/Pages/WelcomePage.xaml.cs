using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using uwpPlatenInformationScreen.Managers;
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

        /// <summary>
        /// Populates the lunch text.
        /// </summary>
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
            TextToday.FontSize = 48;
            TextTodaysFood.FontSize = 30;

            switch (nameOfDay)
            {
                case "Måndag":
                    TextToday.Text = "Måndag";
                    
                    temp = lunchMenu.Weeks[0].Days[0].DayMenus;
                    foreach (DayMenu dayMenu in temp)
                    {
                        stringBuilder.Append(dayMenu.MenuAlternativeName + " : ");
                        stringBuilder.AppendLine(dayMenu.DayMenuName);
                    }
                    break;
                case "Tisdag":
                    TextToday.Text = "Tisdag";
                    temp = lunchMenu.Weeks[0].Days[1].DayMenus;
                    foreach (DayMenu dayMenu in temp)
                    {
                        stringBuilder.Append(dayMenu.MenuAlternativeName + " : ");
                        stringBuilder.AppendLine(dayMenu.DayMenuName);
                    }
                    break;
                case "Onsdag":
                    TextToday.Text = "Onsdag";
                    temp = lunchMenu.Weeks[0].Days[2].DayMenus;
                    foreach (DayMenu dayMenu in temp)
                    {
                        stringBuilder.Append(dayMenu.MenuAlternativeName + " : ");
                        stringBuilder.AppendLine(dayMenu.DayMenuName);
                    }
                    break;
                case "Torsdag":
                    TextToday.Text = "Torsdag";
                    temp = lunchMenu.Weeks[0].Days[3].DayMenus;
                    foreach (DayMenu dayMenu in temp)
                    {
                        stringBuilder.Append(dayMenu.MenuAlternativeName + " : ");
                        stringBuilder.AppendLine(dayMenu.DayMenuName);
                    }
                    break;
                case "Fredag":
                    TextToday.Text = "Fredag";
                    temp = lunchMenu.Weeks[0].Days[4].DayMenus;
                    foreach (DayMenu dayMenu in temp)
                    {
                        stringBuilder.Append(dayMenu.MenuAlternativeName + " : ");
                        stringBuilder.AppendLine(dayMenu.DayMenuName);
                    }
                    break;
            }

            string createdString = stringBuilder.ToString();
            TextTodaysFood.Text = createdString;
        }

        /// <summary>
        /// Loads the lunch menu.
        /// </summary>
        private async void LoadLunchMenu()
        {
            lunchMenu = new RootObject();
            lunchMenu = await LunchManager.GetTodaysLunchAsync();
            PopulateLunchText();
        }
    }
}