using System;
using System.Collections.Generic;
using System.Text;

namespace PokerNowAnalysisConsole.Models
{
    public class Player
    {
        public string Name { get; set; }

        public int CurrentStack { get; set; }

        public int StartingStack { get; set; }

        public bool isStanding { get; set; }
    }
}
