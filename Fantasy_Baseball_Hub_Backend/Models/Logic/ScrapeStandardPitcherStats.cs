using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Fantasy_Baseball_Hub_Backend.Models.Logic
{
    public class ScrapeStandardPitcherStats
    {
        async public static Task<List<StandardPitcherStats>> ScrapeFanGraphsStandardPitchingStats()
        {

            // Only Scraping 21 pages of pitchers

            int pagesToScrapeCount = 21;


            // have playerCount here to make sure the total matches what is expected
            //
            //uncomment when needed
            //
            //int playerCount = 0;
            //
            // *** Make sure to uncomment lines 147 & 151 as well (Counter & Console)

            //      ** List Containg Class Player info **
            List<StandardPitcherStats> listOfPlayers = new List<StandardPitcherStats>();

            //      ** List that will Contain HTMLNODES so we can sift through the data later **
            List<HtmlNode> totalListOfPlayerNodes = new List<HtmlNode>();

            List<List<string>> listOfPlayersInfo = new List<List<string>>();


            // send get request to URL, in this case fangraphs.com
            for (int i = 1; i <= pagesToScrapeCount; i++)
            {
                string url = $"https://www.fangraphs.com/leaders.aspx?pos=all&stats=pit&lg=all&qual=0&type=0&season=2023&month=0&season1=2023&ind=0&team=0&rost=0&age=0&filter=&players=0&startdate=2023-01-01&enddate=2023-12-31&page={i}_30";
                var httpClient = new HttpClient();
                var html = httpClient.GetStringAsync(url).Result;
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);


                // Get list of Players

                //      ** storing the nodes in both the listOfPlayerNodes, the first one is gathers the "odd rows" 
                //       based on the Css selector in fangraphs are set up, and the second is grabing the "even" **
                var listOfPlayerNodes = htmlDocument.DocumentNode.QuerySelectorAll("tr.rgRow");
                var listOfPlayerNodes2 = htmlDocument.DocumentNode.QuerySelectorAll("tr.rgAltRow");

                totalListOfPlayerNodes.AddRange(listOfPlayerNodes);
                totalListOfPlayerNodes.AddRange(listOfPlayerNodes2);


            }

            // all the nodes inside the trs are going to be  looped through, and will get more specific with what we are looking for, 
            // once we have declared the child nodes we are looking for its time to loop through that node.
            // the child node will contain all the data tables inside each row of player from fangraphs table we scraped.
            // IE. First we asked to take all the info from the table with the css selectors altrow and row
            // now that all the info via nodes are saved in our list. Next we loop through that list to get each "coloum" aka table data or td inside each row
            // each td/coloum in that row we are grabbing the HTML and saving that text to our temp list playerInfo.
            // after its gone through One row it will save that newly created list of strings aka playerInfo and add it to ListOfPlayerInfo. 
            // which is a list containing multiple smaller list of strings or an other way to think about it 
            // a list storing each coloum of a row which is also a player. 
            // broke the web document to its most basic form in order to give our logic the ability to understand what its working with and too save it to our database
            //


            //  ** Now that all the HtmlNodes are in one list, its time to loop through it to extract what we need **
            foreach (var row in totalListOfPlayerNodes)
            {
                //      ** This is selecting all the child node that are table data with the class name grid_line_regular **
                var rowOfplayerStats = row.QuerySelectorAll("td.grid_line_regular");

                // created a list to store all the strings from each table data with the css selector(grid_line_regular)
                List<string> playerInfo = new List<string>();

                foreach (var tableData in rowOfplayerStats)
                {
                    // grabing the html from each item and triming off any excess white space
                    var UniqueColoumInfo = tableData.InnerHtml.Trim();

                    // adding it to my playinfoList
                    playerInfo.Add(UniqueColoumInfo);

                }

                // adding that playerInfo to this list of playerInfo
                listOfPlayersInfo.Add(playerInfo);


            }

            // looping through listplayerInfo each item in the listofplayersInfo is a player containing a list of strings.

            foreach (var player in listOfPlayersInfo)
            {   // created a Tuple so my Functions FindPlayerIDANdPostion() can take in one argument but return two results, the ID and Postion
                Tuple<int, string> playersIDandPosition = FindPlayerIDAndPosition(player[1]);
                // Created a new instance of Class Player named newPlayer
                //each list<string> aka player is being inserted into the Object Player by using the index of the list of info. 
                StandardPitcherStats newPlayer = new StandardPitcherStats();
                newPlayer.fangraphs_player_id = playersIDandPosition.Item1;
                newPlayer.wins = Int32.Parse(player[3]);
                newPlayer.losses = Int32.Parse(player[4]);
                newPlayer.era = Decimal.Parse(player[5]);
                newPlayer.games_played = Int32.Parse(player[6]);
                newPlayer.games_started = Int32.Parse(player[7]);
                newPlayer.complete_game = Int32.Parse(player[8]);
                newPlayer.shutout = Int32.Parse(player[9]);
                newPlayer.saves = Int32.Parse(player[10]);
                newPlayer.holds = Int32.Parse(player[11]);
                newPlayer.blown_saves = Int32.Parse(player[12]);
                newPlayer.innings_pitched = Decimal.Parse(player[13]);
                newPlayer.total_batters_faced = Int32.Parse(player[14]);
                newPlayer.hits_allowed = Int32.Parse(player[15]);
                newPlayer.runs_allowed = Int32.Parse(player[16]);
                newPlayer.earned_runs = Int32.Parse(player[17]);
                newPlayer.homeruns = Int32.Parse(player[18]);
                newPlayer.walks = Int32.Parse(player[19]);
                newPlayer.ibb = Int32.Parse(player[20]);
                newPlayer.hit_batters = Int32.Parse(player[21]);
                newPlayer.wild_pitches= Int32.Parse(player[22]);
                newPlayer.balks = Int32.Parse(player[23]);
                newPlayer.strikeouts = Int32.Parse(player[24]);




                //Console.WriteLine(newPlayer.name + " " + newPlayer.runs_allowed + " " + newPlayer.earned_runs);
                // Once its done going through the individual list<string> player and filling the newPlayer,
                // i added the newPlayer to a list of Object Player names listofPlayers
                listOfPlayers.Add(newPlayer);

            }
            return listOfPlayers;
            // Console.WriteLine(playerCount);




        }



        async public static Task<List<Player>> ScrapeFanGraphsStandardPitchingPlayer()
        {

            // Only Scraping 21 pages of pitchers

            int pagesToScrapeCount = 21;


            // have playerCount here to make sure the total matches what is expected
            //
            //uncomment when needed
            //
            //int playerCount = 0;
            //
            // *** Make sure to uncomment lines 147 & 151 as well (Counter & Console)

            //      ** List Containg Class Player info **
            List<Player> listOfPlayers = new List<Player>();

            //      ** List that will Contain HTMLNODES so we can sift through the data later **
            List<HtmlNode> totalListOfPlayerNodes = new List<HtmlNode>();

            List<List<string>> listOfPlayersInfo = new List<List<string>>();


            // send get request to URL, in this case fangraphs.com
            for (int i = 1; i <= pagesToScrapeCount; i++)
            {
                string url = $"https://www.fangraphs.com/leaders.aspx?pos=all&stats=pit&lg=all&qual=0&type=0&season=2023&month=0&season1=2023&ind=0&team=0&rost=0&age=0&filter=&players=0&startdate=2023-01-01&enddate=2023-12-31&page={i}_30";
                var httpClient = new HttpClient();
                var html = httpClient.GetStringAsync(url).Result;
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);


                // Get list of Players

                //      ** storing the nodes in both the listOfPlayerNodes, the first one is gathers the "odd rows" 
                //       based on the Css selector in fangraphs are set up, and the second is grabing the "even" **
                var listOfPlayerNodes = htmlDocument.DocumentNode.QuerySelectorAll("tr.rgRow");
                var listOfPlayerNodes2 = htmlDocument.DocumentNode.QuerySelectorAll("tr.rgAltRow");

                totalListOfPlayerNodes.AddRange(listOfPlayerNodes);
                totalListOfPlayerNodes.AddRange(listOfPlayerNodes2);


            }

            // all the nodes inside the trs are going to be  looped through, and will get more specific with what we are looking for, 
            // once we have declared the child nodes we are looking for its time to loop through that node.
            // the child node will contain all the data tables inside each row of player from fangraphs table we scraped.
            // IE. First we asked to take all the info from the table with the css selectors altrow and row
            // now that all the info via nodes are saved in our list. Next we loop through that list to get each "coloum" aka table data or td inside each row
            // each td/coloum in that row we are grabbing the HTML and saving that text to our temp list playerInfo.
            // after its gone through One row it will save that newly created list of strings aka playerInfo and add it to ListOfPlayerInfo. 
            // which is a list containing multiple smaller list of strings or an other way to think about it 
            // a list storing each coloum of a row which is also a player. 
            // broke the web document to its most basic form in order to give our logic the ability to understand what its working with and too save it to our database
            //


            //  ** Now that all the HtmlNodes are in one list, its time to loop through it to extract what we need **
            foreach (var row in totalListOfPlayerNodes)
            {
                //      ** This is selecting all the child node that are table data with the class name grid_line_regular **
                var rowOfplayerStats = row.QuerySelectorAll("td.grid_line_regular");

                // created a list to store all the strings from each table data with the css selector(grid_line_regular)
                List<string> playerInfo = new List<string>();

                foreach (var tableData in rowOfplayerStats)
                {
                    // grabing the html from each item and triming off any excess white space
                    var UniqueColoumInfo = tableData.InnerHtml.Trim();

                    // adding it to my playinfoList
                    playerInfo.Add(UniqueColoumInfo);

                }

                // adding that playerInfo to this list of playerInfo
                listOfPlayersInfo.Add(playerInfo);


            }

            // looping through listplayerInfo each item in the listofplayersInfo is a player containing a list of strings.
            int pcount = 0;
            foreach (var player in listOfPlayersInfo)
            {   // created a Tuple so my Functions FindPlayerIDANdPostion() can take in one argument but return two results, the ID and Postion
                Tuple<int, string> playersIDandPosition = FindPlayerIDAndPosition(player[1]);
                // Created a new instance of Class Player named newPlayer
                //each list<string> aka player is being inserted into the Object Player by using the index of the list of info. 
                Player newPlayer = new Player();
                newPlayer.fangraphs_player_id = playersIDandPosition.Item1;
                newPlayer.name = Regex.Replace(player[1], @"<a\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>", "$1");
                newPlayer.team = Regex.Replace(player[2], @"<a\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>", "$1");
                newPlayer.Position = playersIDandPosition.Item2;




                //if (listOfPlayers.Any(p => p.fangraphs_player_id == newPlayer.fangraphs_player_id))
                //{
                //    Console.WriteLine(newPlayer.name);

                //    // Find all indices of players with the same fangraphs_player_id
                //    var duplicateIndices = listOfPlayers
                //        .Select((player, index) => new { Player = player, Index = index })
                //        .Where(x => x.Player.fangraphs_player_id == newPlayer.fangraphs_player_id)
                //        .Select(x => x.Index);

                //    foreach (var index in duplicateIndices)
                //    {
                //        Console.WriteLine($"Duplicate found at index: {index}");
                //    }
                //}
                listOfPlayers.Add(newPlayer);

            }
            

            return listOfPlayers;





        }
        // function for taking the href link from fangrpahs and parsing through it to extract just their PlayerId and the position of the player

        public static Tuple<int, string> FindPlayerIDAndPosition(string stringOfPlayersHrefLink)
        {
            // playerID and playerPosition are whats going to be returned in our Tuple
            int playerID = 0;
            string playerPosition = "";


            // making a string variable to store the substring that is going to be used to ignore any Href containing it
            // two Href Links are povided in playerInfos table data.
            // we dont want any info from the one containing ignorethisHref string
            string ignorthisHref = "leaders.aspx";
            if (!stringOfPlayersHrefLink.Contains(ignorthisHref))
            {
                // ** Preface the next few lines for futre me and other others reading
                // i am sure there is a much easier way to do the following but ATM my brain 
                //couldnt process anything else so, we got what we got for now **
                string output = "";
                string subLength = "";
                // since the name has/will be extracted else where we dont need to worry about keep it.
                // the href string looks something like the following:
                // <a href="statss.aspx?playerid=13324&position=2B/3B">Ildemaro Vargas</a>

                // getting the index of the char ?
                int firstIndex = stringOfPlayersHrefLink.IndexOf("?");
                // getting the index of the char >
                int lastIndex = stringOfPlayersHrefLink.IndexOf(">");
                // creating a substring that is removing everything up to the ? 
                // the result should look something like ?playerid=13324&position=2B/3B">Ildemaro Vargas</a>

                subLength = stringOfPlayersHrefLink.Substring(0, lastIndex);
                // taking that substring and making an other substring removing everything after the > char
                // the result should look something like ?playerid=13324&position=2B/3B"
                output = subLength.Substring(firstIndex);
                // finding the index of char =
                // creating an other substring thats going to the & symbol
                // the result should look like ?playerid=13324
                int indexOfAndSymbol = output.IndexOf("&");
                string removedBackHalfForId = output.Substring(0, indexOfAndSymbol);
                // creating an other substring thats going one index past the = and taking the next 4 indexies 
                // the result should look like 13324
                int indexOfEqual = output.IndexOf("=");
                string playerIdSubString = removedBackHalfForId.Substring((indexOfEqual + 1));
                // parseing the string into a number to store in our player class
                playerID = Int32.Parse(playerIdSubString);
                // creating.... YET an other ... substring removing everything up to the position of the first = sign this is what output currently looks like ?playerid=13324&position=2B/3B"
                // this is what the results should look like 13324&position=2B/3B"
                string positionSubstring = output.Substring((indexOfEqual + 1));
                // getting the position of the last remaining = sign in our substring
                indexOfEqual = positionSubstring.IndexOf("=");
                // this is what string position should look like 2B/3B"
                string position = positionSubstring.Substring((indexOfEqual + 1));
                // setting the returned variable to a substring of string position removing the " at the end and the final result should be 2B/3B
                playerPosition = position.Substring(0, position.Length - 1);

            }


            Tuple<int, string> results = new Tuple<int, string>(playerID, playerPosition);

            return results;
        }
    }
}
