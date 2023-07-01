using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Fantasy_Baseball_Hub_Backend.Models.Logic;
namespace Fantasy_Baseball_Hub_Backend.Models.Logic
{
    public class Webscraper
    {

        async public static void ScrapeBase()
        {
            //ScrapeAdvancePitcherStats.ScrapeFanGraphsAdvancePitchingStats();
            //ScrapeStandardPitcherStats.ScrapeFanGraphsStandardPitchingStats();
            //ScrapeAdvanceHitterStats.ScrapeFanGraphsAdvanceHittingStats();
            await ScrapeStandardHitterStats.ScrapeFanGraphsStandardHittingStats();
        }

        
    }


}



