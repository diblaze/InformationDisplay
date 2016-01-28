using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using uwpPlatenInformationScreen.Models;

namespace uwpPlatenInformationScreen
{
    public static class LunchManager
    {
        /// <summary>
        /// Retreives JSON Data from Mashie by parsing the HTML.
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetJsonDataFromMashie()
        {
            const string urlToFetchFrom = @"https://mpi.mashie.eu/public/menu/motala+kommun/af77367d";
            var webClient = new HtmlWeb();

            HtmlDocument document = await webClient.LoadFromWebAsync(urlToFetchFrom);
            HtmlNode root = document.DocumentNode;
            var nodes = root.Descendants("script");

            foreach (string fixedScript in from node in nodes
                                           where node.InnerHtml.Contains("weekData")
                                           select node.InnerHtml.Substring(20))
            {
                return fixedScript;
            }

            return "Couldn't find Json data from Mashie";
        }

        /// <summary>
        /// Retrives JSON data from Mashie and converts it into an object to use.
        /// </summary>
        /// <param name="menu">Collection with the lunch menu.</param>
        /// <returns></returns>
        public static async Task<RootObject> GetTodaysLunch()
        {
            try
            {
                string jsonMessage = await GetJsonDataFromMashie();
                jsonMessage = FixDataTimeJsonData(jsonMessage);

                //deserialize json data into object
                var serializer = new DataContractJsonSerializer(typeof (RootObject));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonMessage));

                var result = (RootObject) serializer.ReadObject(ms);
                return result;
            }
            catch (Exception ex)
            {
                var temp = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// Convert JSON message into a readable Microsot JSON message.
        /// </summary>
        /// <param name="jsonMessage">JSON data from Mashie.</param>
        /// <returns></returns>
        private static string FixDataTimeJsonData(string jsonMessage)
        {
            string tempFixed = Regex.Replace(jsonMessage, @"new Date[(]", @"""\/Date(");
            tempFixed = Regex.Replace(tempFixed, @"d*([)])", @")\/""");

            return tempFixed;
        }
    }
}