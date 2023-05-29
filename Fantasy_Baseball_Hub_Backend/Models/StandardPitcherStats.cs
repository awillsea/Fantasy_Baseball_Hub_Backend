namespace Fantasy_Baseball_Hub_Backend.Models
{
    public class StandardPitcherStats : Player 
    {
        public int Wins { get; set; }
        public int Loses { get; set; }
        public decimal EarnedRunAverage { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesStarted { get; set; }
        public int CompleteGame { get; set; }
        public int ShutOut { get; set; }

        public int Saves { get; set; }

        public int Holds { get; set; }

        public int BlownSaves { get; set; }

        public decimal InningsPitched { get; set; }

        public int TotalBattersFaced { get; set; }
        public int Hits { get; set; }
        public int EarnedRuns { get; set; }
        public int HomeRuns { get; set; }
        public int Walks { get; set; }
        public int IntentionalWalks { get; set; }
        public int HitByPitch { get; set; }
        public int WildPitch { get; set; }
        public int Balks { get; set; }
        public int StrikeOuts { get; set; }
    }
}
