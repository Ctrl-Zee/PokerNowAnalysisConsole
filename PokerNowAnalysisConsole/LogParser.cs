using PokerNowAnalysisConsole.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Action = PokerNowAnalysisConsole.Models.Action;

namespace PokerNowAnalysisConsole
{
    public class LogParser
    {
        public Game game;

        public LogParser()
        {
            game = new Game();
        }

        /// <summary>
        /// Parse the pokernow.club log to pull stats
        /// </summary>
        /// <param name="filePath">Path to the pokernow.club log</param>
        public void ParseLog(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            Array.Reverse(lines);
            List<string> log = lines.ToList();

            Hand hand = new Hand();

            // Loop through the log and pull out the hand actions
            foreach (string line in log)
            {
                if (line.Contains("-- ending hand"))
                {
                    game.HandsPlayed++;
                    game.Log.Add(game.HandsPlayed, hand);
                    hand = new Hand();
                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(line))
                    {
                        DetermineAction(hand, line);
                    }
                }
            }

        }

        /// <summary>
        /// Look at a line from the log and parse the action
        /// </summary>
        /// <param name="action">Line from the log</param>
        /// <returns></returns>
        private void DetermineAction(Hand hand, string action)
        {
            Action theAction = new Action();

            if (action.Contains("created the game"))
            {
                theAction.Type = ActionType.CreateGame;
                theAction.Description = action;
                theAction.Delta = ParseAmount(action, ActionType.CreateGame);
                AddPlayer(ParsePlayerName(action), theAction.Delta);
                theAction.Player = ParsePlayer(action);                
            }
            else if (action.Contains("approved the player") || action.Contains("joined"))
            {
                theAction.Type = ActionType.AddPlayer;
                theAction.Description = action;
                theAction.Delta = ParseAmount(action, ActionType.AddPlayer);
                AddPlayer(ParsePlayerName(action), theAction.Delta);
                theAction.Player = ParsePlayer(action);
            }
            else if (action.Contains("small blind"))
            {
                theAction.Type = ActionType.PostSmallBlind;
                theAction.Description = action;
                theAction.Delta = 10;
                theAction.Player = ParsePlayer(action);
            }
            else if (action.Contains("big blind"))
            {
                theAction.Type = ActionType.PostSmallBlind;
                theAction.Description = action;
                theAction.Delta = 20;
                theAction.Player = ParsePlayer(action);
            }
            else if (action.Contains("folds"))
            {
                theAction.Type = ActionType.Fold;
                theAction.Description = action;
                theAction.Delta = 0;
                theAction.Player = ParsePlayer(action);
                AddPlayerToPhase(hand, theAction.Player);
                theAction.Player.HandsFolded++;
            }
            else if (action.Contains("calls"))
            {
                theAction.Type = ActionType.Call;
                theAction.Description = action;
                theAction.Delta = ParseAmount(action, ActionType.Call);
                theAction.Player = ParsePlayer(action);
                AddPlayerToPhase(hand, theAction.Player);
            }
            else if (action.Contains("checks"))
            {
                theAction.Type = ActionType.Check;
                theAction.Description = action;
                theAction.Delta = 0;
                theAction.Player = ParsePlayer(action);
                AddPlayerToPhase(hand, theAction.Player);
            }
            else if (action.Contains("raises"))
            {
                theAction.Type = ActionType.Raise;
                theAction.Description = action;
                theAction.Delta = ParseAmount(action, ActionType.Raise);
                theAction.Player = ParsePlayer(action);
                AddPlayerToPhase(hand, theAction.Player);
            }
            else if (action.Contains("flop"))
            {
                theAction.Type = ActionType.Flop;
                theAction.Description = action;
                theAction.Delta = 0;
                hand.IsFlop = true;
            }
            else if (action.Contains("turn"))
            {
                theAction.Type = ActionType.Turn;
                theAction.Description = action;
                theAction.Delta = 0;
                hand.IsTurn = true;
            }
            else if (action.Contains("river"))
            {
                theAction.Type = ActionType.River;
                theAction.Description = action;
                theAction.Delta = 0;
                hand.IsRiver = true;
            }
            else if (action.Contains("wins"))
            {
                theAction.Type = ActionType.Win;
                theAction.Description = action;
                theAction.Delta = ParseAmount(action, ActionType.Win);
                theAction.Player = ParsePlayer(action);
                theAction.Player.HandsWon++;
                CheckPotSize(theAction.Delta);
                ParseWinningHand(action);
                AddPlayerToPhase(hand, theAction.Player);
            }
            else if (action.Contains("gained"))
            {
                theAction.Type = ActionType.Gained;
                theAction.Description = action;
                theAction.Delta = ParseAmount(action, ActionType.Gained);
                theAction.Player = ParsePlayer(action);
                theAction.Player.HandsWon++;
                CheckPotSize(theAction.Delta);
                AddPlayerToPhase(hand, theAction.Player);
            }
            else if (action.Contains("ending hand"))
            {
                theAction.Type = ActionType.EndHand;
                theAction.Description = action;
                theAction.Delta = 0;
            }
            else if (action.Contains("starting hand"))
            {
                theAction.Type = ActionType.StartHand;
                theAction.Description = action;
                theAction.Delta = 0;
            }
            else if (action.Contains("shows"))
            {
                theAction.Type = ActionType.ShowHand;
                theAction.Description = action;
                theAction.Delta = 0;
                theAction.Player = ParsePlayer(action);
            }
            else if (action.Contains("quits"))
            {
                theAction.Type = ActionType.PlayerQuit;
                theAction.Description = action;
                theAction.Delta = ParseAmount(action, ActionType.PlayerQuit);
                theAction.Player = ParsePlayer(action);
                PlayerQuits(theAction.Player, theAction.Delta);
            }
            else if (action.Contains("stand up"))
            {
                theAction.Type = ActionType.StandUp;
                theAction.Description = action;
                theAction.Delta = 0;
                theAction.Player = ParsePlayer(action);
            }
            else if (action.Contains("sit back"))
            {
                theAction.Type = ActionType.SitDown;
                theAction.Description = action;
                theAction.Delta = 0;
                theAction.Player = ParsePlayer(action);
            }
            else
            {
                throw new Exception("This is an unknown action");
            }

            hand.Actions.Add(theAction);
        }

        private int ParseAmount(string action, ActionType actionType)
        {
            int amount = 0;
            string[] tokens = action.Split(' ');

            if (actionType == ActionType.Call || actionType == ActionType.Raise || actionType == ActionType.Gained)
            {
                amount = Int32.Parse(tokens[tokens.Length - 1]);
            }
            else if (actionType == ActionType.CreateGame || actionType == ActionType.AddPlayer || actionType == ActionType.PlayerQuit)
            {
                amount = Int32.Parse(tokens[tokens.Length - 1].Replace(".", ""));
            }
            else
            {
                var log = action;
                var amountIndex = Array.IndexOf(tokens, "wins") + 1;
                amount = Int32.Parse(tokens[amountIndex]);
            }

            return amount;
        }

        private void ParseWinningHand(string action)
        {
            int start = action.IndexOf("with") + 4;
            int end = action.IndexOf(',');
            string winningHand = action.Substring(start, end - start);
            IncrementWinningHandType(winningHand);
        }

        private Player ParsePlayer(string action)
        {
            int first = action.IndexOf('\"') + 1;
            int last = action.LastIndexOf('\"') - first;
            string name = action.Substring(first, last);
            return GetPlayer(name);
        }

        private string ParsePlayerName(string action)
        {
            int first = action.IndexOf('\"') + 1;
            int last = action.LastIndexOf('\"') - first;
            return action.Substring(first, last);
        }

        private void AddPlayer(string name, int startingStack)
        {
            // check if player exists
            Player player = GetPlayer(name);

            if (player == null)
            {
                Player newPlayer = new Player()
                {
                    Name = name,
                    StartingStack = startingStack
                };

                game.Players?.Add(newPlayer);
            }            
        }

        private Player GetPlayer(string name)
        {
            return game.Players?.Find(p => p.Name == name);
        }

        private void RemovePlayer(string name)
        {
            Player player = GetPlayer(name);
            game.Players.Remove(player);
        }

        private void PlayerQuits(Player player, int delta)
        {
            if (delta > 0)
            {
                player.FinalStack = delta;
            }
            else
            {
                player.TimesBusted++;
            }
        }

        private void CheckPotSize(int amount)
        {
            if (amount > game.BiggestPotWon)
            {
                game.BiggestPotWon = amount;
            }
        }

        private void IncrementWinningHandType(string winningHand)
        {
            game.WinningHands.TryGetValue(winningHand, out var count);
            game.WinningHands[winningHand] = count + 1;
        }        

        private void AddPlayerToPhase(Hand hand, Player player)
        {
            if (hand.IsRiver)
            {
                if (hand.RiverPlayers.Where(p => p.Contains(player.Name)).FirstOrDefault() == null)
                {
                    hand.RiverPlayers.Add(player.Name);
                    player.RiversSeen++;
                }
            }
            else if (hand.IsTurn)
            {
                if (hand.TurnPlayers.Where(p => p.Contains(player.Name)).FirstOrDefault() == null)
                {
                    hand.TurnPlayers.Add(player.Name);
                    player.TurnsSeen++;
                }
            }
            else if (hand.IsFlop)
            {
                if (hand.FlopPlayers.Where(p => p.Contains(player.Name)).FirstOrDefault() == null)
                {
                    hand.FlopPlayers.Add(player.Name);
                    player.FlopsSeen++;
                }
            }
        }        
    }
}
