using System;
using System.Collections.Generic;
using System.Text;

namespace PokerNowAnalysisConsole.Models
{
    public class Action
    {
        public string Description { get; set; }

        public ActionType Type { get; set; }

        public int  Delta { get; set; }

        public Player Player { get; set; }
    }

    public enum ActionType
    {
        CreateGame,
        AddPlayer,
        PostSmallBlind,
        PostBigBlind,
        Fold,
        Call,
        Check,
        Raise,
        Flop,
        Turn,
        River,
        Win,
        Gained,
        StartHand,
        EndHand,
        ShowHand,
        PlayerQuit,
        StandUp,
        SitDown,
        GameStart,
        HandEnding
    }
}
