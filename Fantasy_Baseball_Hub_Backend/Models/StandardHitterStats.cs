using Dapper;
using Dapper.Contrib.Extensions;
using Fantasy_Baseball_Hub_Backend.Models.Logic;
using MySql.Data.MySqlClient;
using System.Data;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace Fantasy_Baseball_Hub_Backend.Models
{
    [Table("hitter_stats")]
    public class StandardHitterStats 
    {
        [Key]
        public int fangraphs_player_id { get; set; }

        public int hitter_id { get; set; }
        public int games_played { get; set; }
        public int at_bats { get; set; }
        public int plate_appearances { get; set; }
        public int hits { get; set; }
        public int singles { get; set; }
        public int doubles { get; set; }
        public int triples { get; set; }
        public int home_runs { get; set; }
        public int runs { get; set; }
        public int rbis { get; set; }
        public int walks { get; set; }
        public int ibb { get; set; }
        public int strikeouts { get; set; }
        public int hit_by_pitch { get; set; }
        public int sf { get; set; }
        public int sh { get; set; }
        public int gdp { get; set; }
        public int sb{ get; set; }
        public int cs { get; set; }

        public decimal batting_avg { get; set; }
        
        public static MySqlConnection DB;

        async public static void ScrapeSHS()
        {
            List<StandardHitterStats> standardHitterStats = new List<StandardHitterStats>();
            standardHitterStats = await ScrapeStandardHitterStats.ScrapeFanGraphsStandardHittingStats();
            InsertSHSIntoDataBase(standardHitterStats);
            //Test(standardHitterStats);
        }
        public static void InsertSHSIntoDataBase(List<StandardHitterStats> shs)
        {
            using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            {

                connection.Open();
                foreach (var info in shs)
                {
                    //StandardHitterStats newPlayer = new StandardHitterStats();

                    //newPlayer.games_played = info.games_played;
                    //newPlayer.at_bats = info.at_bats;
                    //newPlayer.plate_appearances = info.games_played;
                    //newPlayer.hits = info.hits;
                    //newPlayer.singles = info.singles;
                    //newPlayer.doubles = info.doubles;
                    //newPlayer.triples = info.triples;
                    //newPlayer.home_runs = info.home_runs;
                    //newPlayer.runs = info.runs;
                    //newPlayer.rbis = info.rbis;
                    //newPlayer.walks = info.walks;
                    //newPlayer.ibb = info.ibb;
                    //newPlayer.strikeouts = info.strikeouts;
                    //newPlayer.hit_by_pitch = info.hit_by_pitch;
                    //newPlayer.sf = info.sf;
                    //newPlayer.sh = info.sh;
                    //newPlayer.gdp = info.gdp;
                    //newPlayer.sb = info.sb;
                    //newPlayer.cs = info.cs;
                    //newPlayer.batting_avg =info.batting_avg;
                    connection.Insert(info);
                }
            }

            //string query = "INSERT INTO hitter_stats (hitter_id, games_played, at_bats, plate_appearances, hits, singles, doubles, triples, home_runs, runs, rbis, walks, ibb, strikeouts, hit_by_pitch, sf, sh, gdp, sb, cs, batting_avg) " +
            //   "VALUES (@hitter_id, @games_played, @at_bats, @plate_appearances, @hits, @singles, @doubles, @triples, @home_runs, @runs, @rbis, @walks, @ibb, @strikeouts, @hit_by_pitch, @sf, @sh, @gdp, @sb, @cs, @batting_avg)";


            //using (MySqlConnection connection = new MySqlConnection(DB.ConnectionString))
            //{
            //    connection.Open();
            //    foreach (var info in shs)
            //    {
            //        var parameters = new
            //        {
            //            games_played = info.games_played,
            //            at_bats = info.at_bats,
            //            plate_appearances = info.plate_appearances,
            //            hits = info.hits,
            //            singles = info.singles,
            //            doubles = info.doubles,
            //            triples = info.triples,
            //            home_runs = info.home_runs,
            //            runs = info.runs,
            //            rbis = info.rbis,
            //            walks = info.walks,
            //            ibb = info.ibb,
            //            strikeouts = info.strikeouts,
            //            hit_by_pitch = info.hit_by_pitch,
            //            sf = info.sf,
            //            sh = info.sh,
            //            gdp = info.gdp,
            //            sb = info.sb,
            //            cs = info.cs,
            //            batting_avg = info.batting_avg
            //        };

            //        connection.Execute(query, parameters);
            //    }
            //}


            //string query = "insert into hitter_stats(hitter_id) select player_id from player ";



            //MySqlCommand cmd = DB.CreateCommand();
            //cmd.CommandText = query;



            //DB.Open();

            ////cmd.ExecuteNonQuery();


            //    foreach (var info in shs)
            //    {
            //       DB.Insert(info);
            //    }


            //DB.Close();





        }

        //public static void Test(List<StandardHitterStats> shs)
        //{

        //    string sql = @"INSERT INTO hitter_stats OUTPUT INSERTED.player_id VALUES(@currentPlayersName)";
        //    MySqlCommand cmd = new MySqlCommand(sql, DB);
        //    foreach(var p in shs) 
        //    {
        //        cmd.Parameters.AddWithValue("@currentPlayersName", p.name);
               


        //    }
        //    var playerid = (int)cmd.ExecuteScalar();

        //    string sql2 = "insert into hitter_stats(home_runs) values (10); ";
        //    MySqlCommand newCommand = new MySqlCommand(sql2, DB);
        //    cmd.Parameters.AddWithValue("@player_id", playerid);
            //string conString = DB.ConnectionString;
            //string query = "insert into hitter_stats(hitter_id, home_runs) values ((select player_id from player where name =@currentPlayersName),10); ";

            //using (MySqlConnection connection = new MySqlConnection(conString))
            //using (MySqlCommand command = new MySqlCommand(query, connection)) 
            //{
            //    connection.Open();
            //    foreach(var p in shs)
            //    {
            //        command.Parameters.Add("@currentPlayersName", MySqlDbType.Text);
            //        command.Parameters["@currentPlayersName"].Value = p.name;
            //        command.ExecuteNonQuery();
            //    }

            //}

        //}
    }
}
