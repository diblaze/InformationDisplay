using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using uwpPlatenInformationScreen.Models;

namespace uwpPlatenInformationScreen.Managers
{
    public static class LunchManager
    {
        /// <summary>
        ///     Retreives JSON Data from Mashie by parsing the HTML.
        ///     Because of Mashie not having an public API I had to make my own workaround.
        /// </summary>
        /// <returns>JSON String with the data.</returns>
        private static async Task<string> GetJsonDataFromMashieAsync()
        {
            //Url to page to parse.
            const string urlToFetchFrom = @"https://mpi.mashie.eu/public/menu/motala+kommun/af77367d";
            var webClient = new HtmlWeb();

            //download HTML from url.
            HtmlDocument document = await webClient.LoadFromWebAsync(urlToFetchFrom);
            //find root of HTML document.
            HtmlNode root = document.DocumentNode;
            //find all script nodes.
            var nodes = root.Descendants("script");

            //find the node that contains the "weekData" string.
            foreach (string fixedScript in from node in nodes
                                           where node.InnerHtml.Contains("weekData")
                                           select node.InnerHtml.Substring(20))
            {
                return fixedScript;
            }

            //if the parse failed
            return "Couldn't find Json data from Mashie";
        }

        /// <summary>
        ///     Retrives JSON data from Mashie and converts it into an object to use.
        /// </summary>
        /// <returns>
        ///     <c>Lunch</c>
        /// </returns>
        public static async Task<RootObject> GetTodaysLunchAsync()
        {
            try
            {
                //get json data
                string jsonMessage = await GetJsonDataFromMashieAsync();
                //fix the date time
                jsonMessage = FixDateTimeJsonData(jsonMessage);

                //deserialize json data into object
                var serializer = new DataContractJsonSerializer(typeof (RootObject));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonMessage));

                //read into object
                var result = (RootObject) serializer.ReadObject(ms);
                return result;
            }
            catch (Exception ex)
            {
                string temp = ex.Message;
                return null;
            }
        }

        /// <summary>
        ///     Convert JSON message into a readable Microsot JSON message.
        /// </summary>
        /// <param name="jsonMessage">JSON data from Mashie.</param>
        /// <returns>
        ///     Fixed <c>JSON</c>
        /// </returns>
        private static string FixDateTimeJsonData(string jsonMessage)
        {
            //The issue was the following: "new Date(12345)" is not reconizable by Microsoft's JSON deserializer, however "/Date(12345)/" is!
            //By using Regex we are able to replace and fix the JSON string before making it into an object by using the following code:

            string tempFixed = Regex.Replace(jsonMessage, @"new Date[(]", @"""\/Date(");
            tempFixed = Regex.Replace(tempFixed, @"d*([)])", @")\/""");

            return tempFixed;
        }
    }
}