namespace Fantasy_Baseball_Hub_Backend.Models
{
    public class StandardHitterStats : Player
    {
        public int GamesPlayed { get; set; }
        public int AtBats { get; set; }
        public int PlateApperances { get; set; }
        public int Hits { get; set; }
        public int Singles { get; set; }
        public int Doubles { get; set; }
        public int Triples { get; set; }
        public int HomeRuns { get; set; }
        public int Runs { get; set; }
        public int RBI { get; set; }
        public int Walks { get; set; }
        public int IntentionalWalks { get; set; }
        public int StrikeOuts { get; set; }
        public int HitByPitch { get; set; }
        public int SacraficeFly { get; set; }
        public int SacraficeHit { get; set; }
        public int GroundIntoDoublePlays { get; set; }
        public int StolenBase { get; set; }
        public int CaughtStealing { get; set; }

        public decimal BattingAverage { get; set; }
    }
}
