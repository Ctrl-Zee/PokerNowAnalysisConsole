using System;
using System.Collections.Generic;
using System.Text;

namespace PokerNowAnalysisConsole.Models
{
    public class Game
    {
        public List<Player> Players { get; set; }
        public Dictionary<int, Hand> Log { get; set; }
        public int HandsPlayed { get; set; }

        public Game()
        {
            Players = new List<Player>();
            Log = new Dictionary<int, Hand>();
            HandsPlayed = 0;
        }
    }
}
