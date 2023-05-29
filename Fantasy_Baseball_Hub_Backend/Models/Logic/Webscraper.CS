using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Fantasy_Baseball_Hub_Backend.Models.Logic;
namespace Fantasy_Baseball_Hub_Backend.Models.Logic
{
    public class Webscraper
    {

        public static void ScrapeBase()
        {
            ScrapeAdvanceHitterStats.ScrapeFanGraphsAdvanceHittingStats();
        }

        
    }


}



