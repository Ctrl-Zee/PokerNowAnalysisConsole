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
            Console.WriteLine($"Total Hands Played: {_gameResults.HandsPlayed}");
            Console.WriteLine($"Biggest Pot Won: {_gameResults.BiggestPotWon}");
            Console.WriteLine($"Start Time: {_gameResults.StartingTime}");
            Console.WriteLine($"Start Time: {_gameResults.EndingTime}");
            GetPlayerStats();

            Console.ReadLine();
        }

        public void GetPlayerStats()
        {
            foreach (var player in _gameResults.Players)
            {
                Console.WriteLine();
                Console.WriteLine($"------------ {player.Name} ------------");
                Console.WriteLine($"Quiting Stack: {player.QuitingStack}");
                Console.WriteLine($"Standing Stack: {player.StandingStack}");
                Console.WriteLine($"Hands Played: {player.LastHandNumberPlayed}");
                Console.WriteLine($"Hands Won: {player.HandsWon}");
                Console.WriteLine($"Hands Folded: {player.HandsFolded}");
                Console.WriteLine($"Flops Seen: {player.FlopsSeen}");
                Console.WriteLine($"Turns Seen: {player.TurnsSeen}");
                Console.WriteLine($"Rivers Seen: {player.RiversSeen}");
                Console.WriteLine($"Times Busted: {player.TimesBusted}");

            }
        }
    }
}
