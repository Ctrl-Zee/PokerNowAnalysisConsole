using System;
using System.Collections.Generic;
using System.Text;

namespace PokerNowAnalysisConsole.Models
{
    public class Player
    {
        public string Name { get; set; }

        public int StartingStack { get; set; }

        public int FinalStack { get; set; }

        public int HandsFolded { get; set; }

        public int HandsWon { get; set; }

        public int FlopsSeen { get; set; }

        public int TurnsSeen { get; set; }

        public int RiversSeen { get; set; }

        public int TimesBusted { get; set; }


    }
}
