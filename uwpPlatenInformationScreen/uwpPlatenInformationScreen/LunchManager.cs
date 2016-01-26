using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using uwpPlatenInformationScreen.Models;

namespace uwpPlatenInformationScreen
{
    public static class LunchManager
    {
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

        public static async Task<RootObject> GetTodaysLunch(ObservableCollection<RootObject> menu )
        {
            string jsonMessage = await GetJsonDataFromMashie();

            //deserialize json data into object
            var serializer = new DataContractJsonSerializer(typeof (RootObject));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonMessage));

            RootObject result = (RootObject) serializer.ReadObject(ms);
            return result;
        }
    }
}