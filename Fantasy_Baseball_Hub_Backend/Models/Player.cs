using Dapper;
using Dapper.Contrib.Extensions;
using Fantasy_Baseball_Hub_Backend.Models.Logic;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System.IO;

namespace Fantasy_Baseball_Hub_Backend.Models
{
    [Table("player")]
    public class Player
    {
        [Key]
        public int player_id { get; set; }

        public int fangraphs_player_id { get; set; }

        public string name { get; set; }

        public string team { get; set; }
        public bool is_pitcher { get; set; }

        public string Position { get; set; }


        public static MySqlConnection DB;

        async public static void ScrapePlayer()
        {

            List<Player> players = new List<Player>();
            List<Player> noDupPlayerList = new List<Player>();
            List<Player> tempshs = new List<Player>();
            List<Player> tempsps = new List<Player>();

            List<StandardHitterStats> standardHitterStats = new List<StandardHitterStats>();
            List<StandardPitcherStats> standardPitcherStats = new List<StandardPitcherStats>();


            standardHitterStats = await ScrapeStandardHitterStats.ScrapeFanGraphsStandardHittingStats();
            standardPitcherStats = await ScrapeStandardPitcherStats.ScrapeFanGraphsStandardPitchingStats();
            tempshs = await ScrapeStandardHitterStats.ScrapeFanGraphsHittingPlayers();
            tempsps = await ScrapeStandardPitcherStats.ScrapeFanGraphsStandardPitchingPlayer();
            foreach (var p in tempshs)
            {
                Player newPlayer = new Player();
                newPlayer.fangraphs_player_id = p.fangraphs_player_id;
                newPlayer.name = p.name;
                newPlayer.team = p.team;
                newPlayer.Position = p.Position;
                newPlayer.is_pitcher = IsPitcher(newPlayer.Position);

                if (!newPlayer.is_pitcher)
                {
                    players.Add(newPlayer);
                }
            }
            foreach (var p in tempsps)
            {
                Player newPlayer = new Player();
                newPlayer.fangraphs_player_id = p.fangraphs_player_id;
                newPlayer.name = p.name;
                newPlayer.team = p.team;
                newPlayer.Position = p.Position;
                newPlayer.is_pitcher = IsPitcher(newPlayer.Position);
                if (newPlayer.is_pitcher)
                {
                    players.Add(newPlayer);
                }
            }

            //InsertPlayerAndHitterStats(players, standardHitterStats);
            //InsertHitterStatsIntoDataBase(standardHitterStats);
            //Dictionary<int, (string, int)> duplicates = new Dictionary<int, (string, int)>();

            //// Iterate through the players list

            //// Accessing the duplicates
            //foreach (var duplicate in duplicates)
            //{
            //    Console.WriteLine($"Duplicate fangraphs_player_id: {duplicate.Key}, Name: {duplicate.Value.Item1}, Count: {duplicate.Value.Item2}");
            //}
            InsertPlayerAndStats(players, standardHitterStats, standardPitcherStats);


            //InsertPlayerIntoDataBase(players);
            //InsertHitterAndPitcherStats(standardHitterStats);
            //, List<StandardPitcherStats> ps
        }

        public static bool IsPitcher(string position)
        {
            bool pitcher;
            if (position.ToLower().Contains('p'))
            {
                pitcher = true;

            }
            else
            {
                pitcher = false;
            }
            return pitcher;
        }


        public static void InsertPlayerIntoDataBase(List<Player> p)
        {
            DB.Open();
            foreach (Player info in p)
            {
                DB.Insert(info);
            }
            DB.Close();
        }


        //public static void InsertHitterAndPitcherStats(List<StandardHitterStats> hs)
        //{
        //    // , List<StandardPitcherStats> ps
        //    using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
        //    {
        //        connection.Open();

        //        foreach (StandardHitterStats s in hs)
        //        {
        //            var hitterParameters = new
        //            {

        //                fangraphs_player_id = s.fangraphs_player_id,
        //                games_played = s.games_played,
        //                at_bats = s.at_bats,
        //                plate_appearances = s.plate_appearances,
        //                hits = s.hits,
        //                singles = s.singles,
        //                doubles = s.doubles,
        //                triples = s.triples,
        //                home_runs = s.home_runs,
        //                runs = s.runs,
        //                rbis = s.rbis,
        //                walks = s.walks,
        //                ibb = s.ibb,
        //                strikeouts = s.strikeouts,
        //                hit_by_pitch = s.hit_by_pitch,
        //                sf = s.sf,
        //                sh = s.sh,
        //                gdp = s.gdp,
        //                sb = s.sb,
        //                cs = s.cs,
        //                batting_avg = s.batting_avg
        //            };



        //            connection.Execute(
        //                "insert into hitter_stats set fangraphs_player_id =  (select fangraphs_player_id from player where player.fangraphs_player_id = @fangraphs_player_id ), games_played = @games_played, at_bats = @at_bats, plate_appearances = @plate_appearances, hits = @hits, singles = @singles, doubles = @doubles," +
        //                "triples = @triples, home_runs = @home_runs, runs = @runs, rbis =@rbis, walks = @walks, ibb = @ibb, strikeouts = @strikeouts, hit_by_pitch = @hit_by_pitch, sf = @sf, sh = @sh, gdp = @gdp, sb = @sb, cs = @cs, batting_avg = @batting_avg;) "
        //                ,
        //                hitterParameters);
        //        }



        //DB.Close();
        //DB.Open();
        //foreach (StandardPitcherStats s in ps)
        //{
        //    var pitcherParameters = new
        //    {
        //        fangraphs_player_id = s.fangraphs_player_id,
        //        wins = s.wins,
        //        losses = s.losses,
        //        era = s.era,
        //        games_played = s.games_played,
        //        games_started = s.games_started,
        //        complete_game = s.complete_game,
        //        shutout = s.shutout,
        //        saves = s.saves,
        //        holds = s.holds,
        //        blown_saves = s.blown_saves,
        //        innings_pitched = s.innings_pitched,
        //        total_batters_faced = s.total_batters_faced,
        //        hits_allowed = s.hits_allowed,
        //        runs_allowed = s.runs_allowed,
        //        earned_runs = s.earned_runs,
        //        homeruns = s.homeruns,
        //        walks = s.walks,
        //        ibb = s.ibb,
        //        hit_batters = s.hit_batters,
        //        wild_pitches = s.wild_pitches,
        //        balks = s.balks,
        //        strikeouts = s.strikeouts
        //    };

        //    connection.Execute(
        //        "INSERT INTO pitcher_stats (fangraphs_player_id, wins, losses, era, games_played, games_started, complete_game, shutout, saves, holds, " +
        //        "blown_saves, innings_pitched, total_batters_faced, hits_allowed, runs_allowed, earned_runs, homeruns, walks, ibb, " +
        //        "hit_batters, wild_pitches, balks, strikeouts) " +
        //        "Select fangraphs_player_id from player where fangraphs_player_id = @fangraphs_player_id), @wins, @losses, @era, @games_played, @games_started, " +
        //        "@complete_game, @shutout, @saves, @holds, @blown_saves, @innings_pitched, " +
        //        "@total_batters_faced, @hits_allowed, @runs_allowed, @earned_runs, @homeruns, " +
        //        "@walks, @ibb, @hit_batters, @wild_pitches, @balks, @strikeouts)",
        //        pitcherParameters);
        //}
        //DB.Close();
        //}
        //}
        //}
        //public static void InsertHitterStatsIntoDataBase(List<StandardHitterStats> p)
        //{
        //    DB.Open();
        //    foreach (StandardHitterStats info in p)
        //    {
        //        DB.Insert(info);
        //    }
        //    DB.Close();
        //}
        // ****************************** working ********************************** //
        //public static void InsertPlayerAndHitterStats(List<Player> players, List<StandardHitterStats> shs)
        //{


        //    using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
        //    {
        //        connection.Open();

        //        // List to store player_id values
        //        List<int> playerIds = new List<int>();

        //        // Insert players and retrieve player_id values
        //        foreach (var player in players)
        //        {
        //            var playerParameters = new
        //            {
        //                player_fangraphs_player_id = player.fangraphs_player_id,
        //                player_name = player.name,
        //                player_team = player.team,
        //                player_position = player.Position,
        //                player_is_pitcher = player.is_pitcher,
        //            };

        //            var playerId = connection.ExecuteScalar<int>(
        //                          "INSERT INTO player (fangraphs_player_id, name, team, Position, is_pitcher) VALUES (@player_fangraphs_player_id, @player_name,@player_team, @player_position, @player_is_pitcher);" +
        //                          "SELECT LAST_INSERT_ID();",
        //                          playerParameters);

        //            playerIds.Add(playerId);



        //        }

        //        // Insert hitter stats with mapped player_id values
        //        for (int i = 0; i < Math.Min(playerIds.Count, shs.Count); i++)
        //        {
        //            var hitterParameters = new
        //            {
        //                hitter_id = playerIds[i],
        //                games_played = shs[i].games_played,
        //                at_bats = shs[i].at_bats,
        //                plate_appearances = shs[i].plate_appearances,
        //                hits = shs[i].hits,
        //                singles = shs[i].singles,
        //                doubles = shs[i].doubles,
        //                triples = shs[i].triples,
        //                home_runs = shs[i].home_runs,
        //                runs = shs[i].runs,
        //                rbis = shs[i].rbis,
        //                walks = shs[i].walks,
        //                ibb = shs[i].ibb,
        //                strikeouts = shs[i].strikeouts,
        //                hit_by_pitch = shs[i].hit_by_pitch,
        //                sf = shs[i].sf,
        //                sh = shs[i].sh,
        //                gdp = shs[i].gdp,
        //                sb = shs[i].sb,
        //                cs = shs[i].cs,
        //                batting_avg = shs[i].batting_avg

        //            };

        //            connection.Execute(
        //                "INSERT INTO hitter_stats (hitter_id, games_played, at_bats, plate_appearances, hits, singles, doubles, " +
        //                "triples, home_runs, runs, rbis, walks, ibb, strikeouts, hit_by_pitch, sf, sh, gdp, sb, cs, batting_avg) " +
        //                "VALUES (@hitter_id, @games_played, @at_bats, @plate_appearances, @hits, @singles, @doubles, @triples, " +
        //                "@home_runs, @runs, @rbis, @walks, @ibb, @strikeouts, @hit_by_pitch, @sf, @sh, @gdp, @sb, @cs, @batting_avg)",
        //                hitterParameters);
        //        }
        //    }
        //}

        // ****************************** working ********************************** //



        //public static void InsertPlayerAndStats(List<Player> players, List<StandardHitterStats> shs, List<StandardPitcherStats> pss)
        //{
        //    using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
        //    {
        //        connection.Open();

        //        // List to store player IDs
        //        List<int> playerIds = new List<int>();

        //        // Insert players and retrieve player IDs
        //        foreach (var player in players)
        //        {
        //            var playerParameters = new
        //            {
        //                player_fangraphs_player_id = player.fangraphs_player_id,
        //                player_name = player.name,
        //                player_team = player.team,
        //                player_position = player.Position,
        //                player_is_pitcher = player.is_pitcher,
        //            };

        //            var playerId = connection.ExecuteScalar<int>(
        //                "INSERT INTO player (fangraphs_player_id, name, team, Position, is_pitcher) " +
        //                "VALUES (@player_fangraphs_player_id, @player_name, @player_team, @player_position, @player_is_pitcher);" +
        //                "SELECT LAST_INSERT_ID();",
        //                playerParameters);

        //            playerIds.Add(playerId);
        //        }

        //        // Insert hitter stats with mapped player IDs
        //        for (int i = 0; i < Math.Min(playerIds.Count, shs.Count); i++)
        //        {
        //            var hitterParameters = new
        //            {
        //                hitter_id = playerIds[i],
        //                games_played = shs[i].games_played,
        //                at_bats = shs[i].at_bats,
        //                plate_appearances = shs[i].plate_appearances,
        //                hits = shs[i].hits,
        //                singles = shs[i].singles,
        //                doubles = shs[i].doubles,
        //                triples = shs[i].triples,
        //                home_runs = shs[i].home_runs,
        //                runs = shs[i].runs,
        //                rbis = shs[i].rbis,
        //                walks = shs[i].walks,
        //                ibb = shs[i].ibb,
        //                strikeouts = shs[i].strikeouts,
        //                hit_by_pitch = shs[i].hit_by_pitch,
        //                sf = shs[i].sf,
        //                sh = shs[i].sh,
        //                gdp = shs[i].gdp,
        //                sb = shs[i].sb,
        //                cs = shs[i].cs,
        //                batting_avg = shs[i].batting_avg
        //            };

        //            connection.Execute(
        //                "INSERT INTO hitter_stats (hitter_id, games_played, at_bats, plate_appearances, hits, singles, doubles, " +
        //                "triples, home_runs, runs, rbis, walks, ibb, strikeouts, hit_by_pitch, sf, sh, gdp, sb, cs, batting_avg) " +
        //                "VALUES (@hitter_id, @games_played, @at_bats, @plate_appearances, @hits, @singles, @doubles, @triples, " +
        //                "@home_runs, @runs, @rbis, @walks, @ibb, @strikeouts, @hit_by_pitch, @sf, @sh, @gdp, @sb, @cs, @batting_avg)",
        //                hitterParameters);
        //        }

        //        // Insert pitcher stats with mapped player IDs
        //        for (int i = 0; i < Math.Min(playerIds.Count, pss.Count); i++)
        //        {
        //            int pitcherIdIndex = i + playerIds.Count - shs.Count;
        //            var pitcherParameters = new
        //            {
        //                pitcher_id = playerIds[pitcherIdIndex],
        //                pitcher_wins = pss[pitcherIdIndex].wins,
        //                player_losses = pss[pitcherIdIndex].losses,
        //                player_era = pss[pitcherIdIndex].era,
        //                player_games_played = pss[pitcherIdIndex].games_played,
        //                player_games_started = pss[pitcherIdIndex].games_started,
        //                player_complete_game = pss[pitcherIdIndex].complete_game,
        //                player_shutout = pss[pitcherIdIndex].shutout,
        //                player_saves = pss[pitcherIdIndex].saves,
        //                player_holds = pss[pitcherIdIndex].holds,
        //                player_blown_saves = pss[pitcherIdIndex].blown_saves,
        //                player_inngings_pitched = pss[pitcherIdIndex].innings_pitched,
        //                player_total_batters_faced = pss[pitcherIdIndex].total_batters_faced,
        //                player_hits_allowed = pss[pitcherIdIndex].hits_allowed,
        //                player_runs_allowed = pss[pitcherIdIndex].runs_allowed,
        //                player_earned_runs = pss[pitcherIdIndex].earned_runs,
        //                player_homeruns = pss[pitcherIdIndex].homeruns,
        //                player_walks = pss[pitcherIdIndex].walks,
        //                player_ibb = pss[pitcherIdIndex].ibb,
        //                player_hit_batters = pss[pitcherIdIndex].hit_batters,
        //                player_wild_pitches = pss[pitcherIdIndex].wild_pitches,
        //                player_balks = pss[pitcherIdIndex].balks,
        //                player_strikeouts = pss[pitcherIdIndex].strikeouts,
        //        };

        //                            connection.Execute(
        //            "INSERT INTO pitcher_stats (pitcher_id, wins, losses, era, games_played, games_started, complete_game, shutout, saves, holds, " +
        //            "blown_saves, innings_pitched, total_batters_faced, hits_allowed, runs_allowed, earned_runs, homeruns, walks, ibb, " +
        //            "hit_batters, wild_pitches, balks, strikeouts) " +
        //            "VALUES (@pitcher_id, @pitcher_wins, @player_losses, @player_era, @player_games_played, @player_games_started, " +
        //            "@player_complete_game, @player_shutout, @player_saves, @player_holds, @player_blown_saves, @player_inngings_pitched, " +
        //            "@player_total_batters_faced, @player_hits_allowed, @player_runs_allowed, @player_earned_runs, @player_homeruns, " +
        //            "@player_walks, @player_ibb, @player_hit_batters, @player_wild_pitches, @player_balks, @player_strikeouts)",
        //            pitcherParameters);
        //        }
        //    }
        //}



        //public static void InsertPlayerAndStats(List<Player> players, List<StandardHitterStats> shs, List<StandardPitcherStats> pss)
        //{
        //    using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
        //    {
        //        connection.Open();

        //        // Dictionary to map fangraphs_player_id to player_id
        //        Dictionary<int, int> playerIdMap = new Dictionary<int, int>();

        //        // Insert players and retrieve player IDs
        //        foreach (var player in players)
        //        {
        //            var playerParameters = new
        //            {
        //                player_fangraphs_player_id = player.fangraphs_player_id,
        //                player_name = player.name,
        //                player_team = player.team,
        //                player_position = player.Position,
        //                player_is_pitcher = player.is_pitcher,
        //            };

        //            var playerId = connection.ExecuteScalar<int>(
        //                "INSERT INTO player (fangraphs_player_id, name, team, Position, is_pitcher) " +
        //                "VALUES (@player_fangraphs_player_id, @player_name, @player_team, @player_position, @player_is_pitcher);" +
        //                "SELECT LAST_INSERT_ID();",
        //                playerParameters);

        //            playerIdMap.Add(player.fangraphs_player_id, playerId);
        //        }

        //        // Insert hitter stats with mapped player IDs
        //        foreach (var sh in shs)
        //        {
        //            if (playerIdMap.TryGetValue(sh.fangraphs_player_id, out int hitterId))
        //            {
        //                var hitterParameters = new
        //                {
        //                    hitter_id = hitterId,
        //                    fangraphs_player_id = sh.fangraphs_player_id,
        //                    games_played = sh.games_played,
        //                    at_bats = sh.at_bats,
        //                    plate_appearances = sh.plate_appearances,
        //                    hits = sh.hits,
        //                    singles = sh.singles,
        //                    doubles = sh.doubles,
        //                    triples = sh.triples,
        //                    home_runs = sh.home_runs,
        //                    runs = sh.runs,
        //                    rbis = sh.rbis,
        //                    walks = sh.walks,
        //                    ibb = sh.ibb,
        //                    strikeouts = sh.strikeouts,
        //                    hit_by_pitch = sh.hit_by_pitch,
        //                    sf = sh.sf,
        //                    sh = sh.sh,
        //                    gdp = sh.gdp,
        //                    sb = sh.sb,
        //                    cs = sh.cs,
        //                    batting_avg = sh.batting_avg
        //                };

        //                connection.Execute(
        //                    "INSERT INTO hitter_stats (hitter_id, fangraphs_player_id, games_played, at_bats, plate_appearances, hits, singles, doubles, " +
        //                    "triples, home_runs, runs, rbis, walks, ibb, strikeouts, hit_by_pitch, sf, sh, gdp, sb, cs, batting_avg) " +
        //                    "VALUES (@hitter_id, @fangraphs_player_id, @games_played, @at_bats, @plate_appearances, @hits, @singles, @doubles, @triples, " +
        //                    "@home_runs, @runs, @rbis, @walks, @ibb, @strikeouts, @hit_by_pitch, @sf, @sh, @gdp, @sb, @cs, @batting_avg)",
        //                    hitterParameters);
        //            }
        //        }

        //        // Insert pitcher stats with mapped player IDs
        //        foreach (var ps in pss)
        //        {
        //            if (playerIdMap.TryGetValue(ps.fangraphs_player_id, out int pitcherId))
        //            {
        //                var pitcherParameters = new
        //                {
        //                    pitcher_id = pitcherId,
        //                    fangraphs_player_id = ps.fangraphs_player_id,
        //                    wins = ps.wins,
        //                    losses = ps.losses,
        //                    era = ps.era,
        //                    games_played = ps.games_played,
        //                    games_started = ps.games_started,
        //                    complete_game = ps.complete_game,
        //                    shutout = ps.shutout,
        //                    saves = ps.saves,
        //                    holds = ps.holds,
        //                    blown_saves = ps.blown_saves,
        //                    innings_pitched = ps.innings_pitched,
        //                    total_batters_faced = ps.total_batters_faced,
        //                    hits_allowed = ps.hits_allowed,
        //                    runs_allowed = ps.runs_allowed,
        //                    earned_runs = ps.earned_runs,
        //                    homeruns = ps.homeruns,
        //                    walks = ps.walks,
        //                    ibb = ps.ibb,
        //                    hit_batters = ps.hit_batters,
        //                    wild_pitches = ps.wild_pitches,
        //                    balks = ps.balks,
        //                    strikeouts = ps.strikeouts
        //                };

        //                connection.Execute(
        //                    "INSERT INTO pitcher_stats (pitcher_id, fangraphs_player_id, wins, losses, era, games_played, games_started, complete_game, shutout, saves, holds, " +
        //                    "blown_saves, innings_pitched, total_batters_faced, hits_allowed, runs_allowed, earned_runs, homeruns, walks, ibb, " +
        //                    "hit_batters, wild_pitches, balks, strikeouts) " +
        //                    "VALUES (@pitcher_id, @fangraphs_player_id, @wins, @losses, @era, @games_played, @games_started, " +
        //                    "@complete_game, @shutout, @saves, @holds, @blown_saves, @innings_pitched, " +
        //                    "@total_batters_faced, @hits_allowed, @runs_allowed, @earned_runs, @homeruns, " +
        //                    "@walks, @ibb, @hit_batters, @wild_pitches, @balks, @strikeouts)",
        //                    pitcherParameters);
        //            }
        //        }
        //    }
        //}

        public static void InsertPlayerAndStats(List<Player> players, List<StandardHitterStats> shs, List<StandardPitcherStats> pss)
        {
            //ExportToExcel(players, shs, pss);
            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {
                connection.Open();

                // Dictionary to map fangraphs_player_id to player_id
                Dictionary<int, int> playerIdMap = new Dictionary<int, int>();
                Dictionary<int, List<string>> duplicates = new Dictionary<int, List<string>>();

                int insertionIdCounter = 1;

                // Insert players and retrieve player IDs
                foreach (var player in players)
                {
                    var playerParameters = new
                    {
                        player_fangraphs_player_id = player.fangraphs_player_id,
                        player_name = player.name,
                        player_team = player.team,
                        player_position = player.Position,
                        player_is_pitcher = player.is_pitcher,
                    };
                    Console.WriteLine("**************Start of Logs*********");
                    Console.WriteLine($"Inside the List of players list \n name: {player.name} fid:{player.fangraphs_player_id}");
                    try
                    {
                        int playerId = insertionIdCounter++;
                        connection.ExecuteScalar(
                             "INSERT INTO player (fangraphs_player_id, name, team, Position, is_pitcher) " +
                             "VALUES (@player_fangraphs_player_id, @player_name, @player_team, @player_position, @player_is_pitcher) " +
                             "ON DUPLICATE KEY UPDATE fangraphs_player_id = CONCAT(fangraphs_player_id, '000'); ", playerParameters);

                        Console.WriteLine("Player ID returned for insertion: " + playerId);

                        if (!playerIdMap.ContainsKey(player.fangraphs_player_id))
                        {
                            Console.WriteLine("Inside the PlayerIDMap -**- adding FID to GOOD LIST");
                            Console.WriteLine(player.fangraphs_player_id + " " + playerId);
                            playerIdMap.Add(player.fangraphs_player_id, playerId);
                        }
                        else
                        {
                            foreach (var p in playerIdMap)
                            {
                                if (p.Key == player.fangraphs_player_id)
                                {


                                    Console.WriteLine("The Following Match and is the reason it has moved into the else statement of the PlayerIdMap");
                                    Console.WriteLine(p.Key + "is the same as" + player.fangraphs_player_id);
                                    Console.WriteLine(player.name + "==" + p.Value);
                                }
                            }

                            if (!duplicates.ContainsKey(player.fangraphs_player_id))
                            {
                                Console.WriteLine(player.fangraphs_player_id + "this id is already mapped && it was not in the dup map so we are adding it to this map");
                                Console.WriteLine(player.name + "is the sucker causing issues");
                                duplicates.Add(player.fangraphs_player_id, new List<string>());
                            }

                            Console.WriteLine("--" + player.fangraphs_player_id + "adding the name to the list of strings that will be mapped to FID " + "--");
                            Console.WriteLine(player.name + "again");

                            duplicates[player.fangraphs_player_id].Add(player.name);
                        }
                    }
                    catch (MySqlException e)
                    {
                        if (e.Number == 1062) // MySQL error number for duplicate entry
                        {
                            Console.WriteLine($"Duplicate entry for player: {player.name}");
                        }
                        else
                        {
                            Console.WriteLine($"Error occurred for player: {player.name}, Error: {e.Message}");
                        }
                    }
                }
                Console.WriteLine("**************End of Logs*********");


                // Insert hitter stats with mapped player IDs
                foreach (var sh in shs)
                {
                    if (playerIdMap.TryGetValue(sh.fangraphs_player_id, out int hitterId))
                    {
                        // Check if the fangraphs_player_id is a duplicate in the pitcher stats
                        if (!duplicates.ContainsKey(sh.fangraphs_player_id))
                        {
                            var hitterParameters = new
                            {
                                hitter_id = hitterId,
                                fangraphs_player_id = sh.fangraphs_player_id,
                                games_played = sh.games_played,
                                at_bats = sh.at_bats,
                                plate_appearances = sh.plate_appearances,
                                hits = sh.hits,
                                singles = sh.singles,
                                doubles = sh.doubles,
                                triples = sh.triples,
                                home_runs = sh.home_runs,
                                runs = sh.runs,
                                rbis = sh.rbis,
                                walks = sh.walks,
                                ibb = sh.ibb,
                                strikeouts = sh.strikeouts,
                                hit_by_pitch = sh.hit_by_pitch,
                                sf = sh.sf,
                                sh = sh.sh,
                                gdp = sh.gdp,
                                sb = sh.sb,
                                cs = sh.cs,
                                batting_avg = sh.batting_avg
                            };

                            connection.Execute(
                                "INSERT INTO hitter_stats (hitter_id, fangraphs_player_id, games_played, at_bats, plate_appearances, hits, singles, doubles, " +
                                "triples, home_runs, runs, rbis, walks, ibb, strikeouts, hit_by_pitch, sf, sh, gdp, sb, cs, batting_avg) " +
                                "VALUES (@hitter_id, @fangraphs_player_id, @games_played, @at_bats, @plate_appearances, @hits, @singles, @doubles, @triples, " +
                                "@home_runs, @runs, @rbis, @walks, @ibb, @strikeouts, @hit_by_pitch, @sf, @sh, @gdp, @sb, @cs, @batting_avg)",
                                hitterParameters);
                        }
                    }
                }

                // Insert pitcher stats with mapped player IDs
                foreach (var ps in pss)
                {
                    if (playerIdMap.TryGetValue(ps.fangraphs_player_id, out int pitcherId))
                    {
                        // Check if the fangraphs_player_id is a duplicate in the hitter stats
                        if (!duplicates.ContainsKey(ps.fangraphs_player_id))
                        {
                            var pitcherParameters = new
                            {
                                pitcher_id = pitcherId,
                                fangraphs_player_id = ps.fangraphs_player_id,
                                wins = ps.wins,
                                losses = ps.losses,
                                era = ps.era,
                                games_played = ps.games_played,
                                games_started = ps.games_started,
                                complete_game = ps.complete_game,
                                shutout = ps.shutout,
                                saves = ps.saves,
                                holds = ps.holds,
                                blown_saves = ps.blown_saves,
                                innings_pitched = ps.innings_pitched,
                                total_batters_faced = ps.total_batters_faced,
                                hits_allowed = ps.hits_allowed,
                                runs_allowed = ps.runs_allowed,
                                earned_runs = ps.earned_runs,
                                homeruns = ps.homeruns,
                                walks = ps.walks,
                                ibb = ps.ibb,
                                hit_batters = ps.hit_batters,
                                wild_pitches = ps.wild_pitches,
                                balks = ps.balks,
                                strikeouts = ps.strikeouts
                            };

                            connection.Execute(
                                "INSERT INTO pitcher_stats (pitcher_id,fangraphs_player_id, wins, losses, era, games_played, games_started, complete_game, shutout, saves, holds, " +
                                "blown_saves, innings_pitched, total_batters_faced, hits_allowed, runs_allowed, earned_runs, homeruns, walks, ibb, " +
                                "hit_batters, wild_pitches, balks, strikeouts) " +
                                "VALUES (@pitcher_id, @fangraphs_player_id, @wins, @losses, @era, @games_played, @games_started, " +
                                "@complete_game, @shutout, @saves, @holds, @blown_saves, @innings_pitched, " +
                                "@total_batters_faced, @hits_allowed, @runs_allowed, @earned_runs, @homeruns, " +
                                "@walks, @ibb, @hit_batters, @wild_pitches, @balks, @strikeouts)",
                                pitcherParameters);
                        }
                    }
                }

                //Print out duplicates and associated stats
                // foreach (var duplicate in duplicates)
                //{
                //    Console.WriteLine($"Duplicate fangraphs_player_id: {duplicate.Key}");
                //    foreach (var playerName in duplicate.Value)
                //    {
                //        Console.WriteLine($"Player Name: {playerName}");
                //        // Retrieve and print associated stats from hitter_stats table (if available)
                //        var hitterStats = connection.QueryFirstOrDefault<StandardHitterStats>(
                //            "SELECT * FROM hitter_stats WHERE fangraphs_player_id = @fangraphs_player_id",
                //            new { fangraphs_player_id = duplicate.Key });

                //        if (hitterStats != null)
                //        {
                //            Console.WriteLine("Hitter Stats:");
                //            Console.WriteLine($"Games Played: {hitterStats.games_played}");
                //            // Print other stats as desired
                //        }

                //        // Retrieve and print associated stats from pitcher_stats table (if available)
                //        var pitcherStats = connection.QueryFirstOrDefault<StandardPitcherStats>(
                //            "SELECT * FROM pitcher_stats WHERE fangraphs_player_id = @fangraphs_player_id",
                //            new { fangraphs_player_id = duplicate.Key });

                //        if (pitcherStats != null)
                //        {
                //            Console.WriteLine("Pitcher Stats:");
                //            Console.WriteLine($"Wins: {pitcherStats.wins}");
                //            // Print other stats as desired
                //        }
                    //}
                //}
            }
        }
    }
}

        //public static void ExportToExcel(List<Player> players, List<StandardHitterStats> shs, List<StandardPitcherStats> pss)
        //{
        //    string directoryPath = @"C:\Users\alexa\OneDrive\Documents\ExportedData";
        //    string fileName = "FantasyBaseballData.xlsx";
        //    string filePath = Path.Combine(directoryPath, fileName);

        //    using (ExcelPackage package = new ExcelPackage())
        //    {
        //        // Create worksheet for players
        //        ExcelWorksheet playersSheet = package.Workbook.Worksheets.Add("Players");
        //        playersSheet.Cells.LoadFromCollection(players, true);

        //        // Create worksheet for hitter stats
        //        ExcelWorksheet shsSheet = package.Workbook.Worksheets.Add("HitterStats");
        //        shsSheet.Cells.LoadFromCollection(shs, true);

        //        // Create worksheet for pitcher stats
        //        ExcelWorksheet pssSheet = package.Workbook.Worksheets.Add("PitcherStats");
        //        pssSheet.Cells.LoadFromCollection(pss, true);

        //        // Save the Excel file
        //        FileInfo excelFile = new FileInfo(filePath);
        //        package.SaveAs(excelFile);
        //    }
        //}





