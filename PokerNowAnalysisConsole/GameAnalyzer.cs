using PokerNowAnalysisConsole.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokerNowAnalysisConsole
{
    public class GameAnalyzer
    {
        private Game _gameResults;

        public GameAnalyzer(Game gameResults)
        {
            _gameResults = gameResults;
        }

        public void AnalyzeGame()
        {
            Console.WriteLine($"Total Hands Played: {GetHandsPlayed()}");
            Console.WriteLine($"Biggest Pot Won: {GetBiggestPotWon()}");

            Console.ReadLine();
        }

        public int GetHandsPlayed()
        {
            return _gameResults.HandsPlayed;
        }

        public int GetBiggestPotWon()
        {
            return _gameResults.BiggestPotWon;
        }
    }
}
