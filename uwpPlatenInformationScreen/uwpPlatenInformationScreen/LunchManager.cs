using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace uwpPlatenInformationScreen
{
    public static class LunchManager
    {
        public static async Task<string> GetHtmlDataFromMashie()
        {
            const string urlToFetchFrom = @"https://mpi.mashie.eu/public/menu/motala+kommun/af77367d";
            var webClient = new HtmlWeb();
            
            HtmlDocument document = await webClient.LoadFromWebAsync(urlToFetchFrom);
            HtmlNode root = document.DocumentNode;
            var nodes = root.Descendants().Where(n => n.GetAttributeValue("class", "").Equals("webmenu-weekday webmenu-today"));

            foreach (HtmlNode node in document.DocumentNode.ChildNodes.Elements("html"))
            {
                HtmlNode temp = node;
            }

            return "";
        }
    }
}