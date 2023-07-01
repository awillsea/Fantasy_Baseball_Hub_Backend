using Dapper.Contrib.Extensions;
using Fantasy_Baseball_Hub_Backend.Models.Logic;
using MySql.Data.MySqlClient;

namespace Fantasy_Baseball_Hub_Backend.Models
{
    [Table("pitcher_stats")]
    public class StandardPitcherStats
    { 
        [Key]
        public int fangraphs_player_id { get; set; }

        public int pitcher_id { get; set; }

        public decimal innings_pitched { get; set; }
        public int hits_allowed { get; set; }
        public int runs_allowed { get; set; }
        public int earned_runs { get; set; }
        public decimal era { get; set; }

        public int homeruns { get; set; }
        public int complete_game { get; set; }
        public int shutout { get; set; }
        public int walks { get; set; }
        public int ibb { get; set; }

        public int hit_batters { get; set; }
        public int wild_pitches { get; set; }

        public int balks { get; set; }
        public int strikeouts { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
        public int saves { get; set; }
        public int holds { get; set; }

        public int blown_saves { get; set; }
        public int total_batters_faced { get; set; }

        public int games_played { get; set; }
        public int games_started { get; set; }

        public static MySqlConnection DB;


        async public static void ScrapeSPS()
        {
            List<StandardPitcherStats> standardPitcherStats = new List<StandardPitcherStats>();
            standardPitcherStats = await ScrapeStandardPitcherStats.ScrapeFanGraphsStandardPitchingStats();
            InsertSPSIntoDataBase(standardPitcherStats);
        }
        public static void InsertSPSIntoDataBase(List<StandardPitcherStats> shs)
        {
            DB.Open();
            foreach (StandardPitcherStats info in shs)
            {
                DB.Insert(shs);
            }
            DB.Close();
        }

    }
}
