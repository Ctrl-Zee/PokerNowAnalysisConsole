using System;
using System.Collections.Generic;
using System.Text;

namespace PokerNowAnalysisConsole.Models
{
    public class Game
    {
        public List<Player> Players { get; set; }
        public Dictionary<int, Hand> Log { get; set; }
        public Dictionary<string, int> WinningHands { get; set; }
        public int HandsPlayed { get; set; }
        public int BiggestPotWon { get; set; }
        public string BestWinningHand { get; set; }
        public string StartingTime { get; set; }
        public string EndingTime { get; set; }

        public Game()
        {
            Players = new List<Player>();
            Log = new Dictionary<int, Hand>();
            WinningHands = new Dictionary<string, int>();
            HandsPlayed = 0;
            BiggestPotWon = 0;
        }
    }
}
