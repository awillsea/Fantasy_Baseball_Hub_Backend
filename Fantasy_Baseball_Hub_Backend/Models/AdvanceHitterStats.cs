using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;

namespace Fantasy_Baseball_Hub_Backend.Models
{
    
    public class AdvanceHitterStats :Player 
    {
        public int hitter_id { get; set; }
        public int GamesPlayed { get; set; }
        public int PlateApperances { get; set; }
        public int HomeRuns { get; set; }
        public int Runs { get; set; }
        public int RBI { get; set; }
        public int StolenBase { get; set; }
        public decimal WalkPercentage { get; set; }
        public decimal StrikeoutPercentage { get; set; }
        public decimal IsolatedPower { get; set; }
        public decimal BattingAverageonballsInPlay { get; set; }
        public decimal BattingAverage { get; set; }
        public decimal OnBasePercentage { get; set; }
        public decimal SluggingPercentage { get; set; }
        public decimal weightedOnBasePercentage { get; set; }
        public decimal ExpectedWeightedOnBasePercentage { get; set; }
        public decimal WeightedRunsCreatedPlus { get; set; }
        public decimal BaseRunning { get; set; }
        public decimal Offense { get; set; }

        public decimal Defense { get; set; }

        public decimal WinsAboveReplacement { get; set; }
    }
}
