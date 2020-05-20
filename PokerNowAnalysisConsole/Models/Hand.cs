using System;
using System.Collections.Generic;
using System.Text;

namespace PokerNowAnalysisConsole.Models
{
    public class Hand
    {
        public int SmallBlind { get; set; }

        public int BigBlind { get; set; }

        public bool IsFlop { get; set; }

        public bool IsTurn { get; set; }

        public bool IsRiver { get; set; }

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

        public List<string> FlopPlayers { get; set; }

        public List<string> TurnPlayers { get; set; }

        public List<string> RiverPlayers { get; set; }        

        //public List<Player> PlayerBets { get; set; }

        public Hand()
        {
            Actions = new List<Action>();
            FlopPlayers = new List<string>();
            TurnPlayers = new List<string>();
            RiverPlayers = new List<string>();
            MinBet = 20;
        }
    }
}
