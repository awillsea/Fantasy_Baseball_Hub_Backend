namespace Fantasy_Baseball_Hub_Backend.Models
{
    public class AdvancePitcherStats : Player 
    {
        public int Wins { get; set; }
        public int Loses { get; set; }
        public int Saves { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesStarted { get; set; }
        public decimal InningsPitched { get; set; }
        public decimal StrikeoutsPerNineInnings { get; set; }
        public decimal WalksPerNineInnings { get; set; }
        public decimal HomeRunsPerNineInnings { get; set; }
        public decimal BattingAverageOnBallsInPlay { get; set; }
        public decimal LeftOnBasePercentage { get; set; }
        public decimal GroundBallPercentage { get; set; }
        public decimal HomeRunToFlyBallRation { get; set; }

        public decimal? AverageFastBallVelocity { get; set; }
        public decimal EarnedRunAverage { get; set; }
        public decimal ExpectedEarnRunAverage { get; set; }

        public decimal FieldingIndependentPitching { get; set; }
        public decimal ExpectedFeildingIndependentPitching { get; set; }

        public decimal WinsAboveReplacement { get; set; }

        
    }
}
