using System;

namespace PokerNowAnalysisConsole
{
    class Program
    {
        static void Main(string[] args)
        {            
            string filePath = @"C:\pokernowlogs\Week5.txt";
            LogParser parser = new LogParser();
            parser.ParseLog(filePath);
            Console.WriteLine(parser.game.HandsPlayed);
        }
    }
}
