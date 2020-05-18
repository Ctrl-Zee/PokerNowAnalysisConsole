using System;
using System.Collections.Generic;
using System.Text;

namespace PokerNowAnalysisConsole.Models
{
    public class Hand
    {
        public int SmallBlind { get; set; }

        public int BigBlind { get; set; }

        public string Flop { get; set; }

        public string Turn { get; set; }

        public string River { get; set; }

        public int PreFlopPot { get; set; }

        public int FlopPot { get; set; }

        public int TurnPot { get; set; }

        public int RiverPot { get; set; }

        public int CurrentPot { get; set; }

        public int MinBet { get; set; }

        public string Winner { get; set; }

        public int  WinningAmount { get; set; }

        public string WinningHand { get; set; }

        public List<Action> Actions { get; set; }

        //public List<Player> PlayerBets { get; set; }

        public Hand()
        {
            Actions = new List<Action>();
            MinBet = 20;
        }
    }
}
