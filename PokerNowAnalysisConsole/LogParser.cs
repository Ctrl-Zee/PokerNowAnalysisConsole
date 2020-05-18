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
                    //CalculateHand(hand);

                    game.Log.Add(game.HandsPlayed, hand);
                    hand = new Hand();
                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(line))
                    {
                        //hand.Actions.Add(DetermineAction(hand, line));
                        DetermineAction(hand, line);
                    }
                }
            }

        }

        //private void CalculateHand(Hand hand)
        //{
        //    foreach (var action in hand.Actions)
        //    {

        //    }
        //}

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
                UpdateStack(theAction.Player, theAction.Delta, hand.MinBet);
                UpdatePot("PreFlop", 10, hand);
            }
            else if (action.Contains("big blind"))
            {
                theAction.Type = ActionType.PostSmallBlind;
                theAction.Description = action;
                theAction.Delta = 20;
                theAction.Player = ParsePlayer(action);
                UpdateStack(theAction.Player, theAction.Delta, hand.MinBet);
                UpdatePot("PreFlop", 20, hand);
            }
            else if (action.Contains("folds"))
            {
                theAction.Type = ActionType.Fold;
                theAction.Description = action;
                theAction.Delta = 0;
                theAction.Player = ParsePlayer(action);
            }
            else if (action.Contains("calls"))
            {
                theAction.Type = ActionType.Call;
                theAction.Description = action;
                theAction.Delta = ParseAmount(action, ActionType.Call);
                theAction.Player = ParsePlayer(action);
                UpdateStack(theAction.Player, theAction.Delta, hand.MinBet);
            }
            else if (action.Contains("checks"))
            {
                theAction.Type = ActionType.Check;
                theAction.Description = action;
                theAction.Delta = 0;
                theAction.Player = ParsePlayer(action);
            }
            else if (action.Contains("raises"))
            {
                theAction.Type = ActionType.Raise;
                theAction.Description = action;
                theAction.Delta = ParseAmount(action, ActionType.Raise);
                theAction.Player = ParsePlayer(action);
                UpdateStack(theAction.Player, theAction.Delta, hand.MinBet);
            }
            else if (action.Contains("flop"))
            {
                theAction.Type = ActionType.Flop;
                theAction.Description = action;
                theAction.Delta = 0;
                ClearLastBets();
            }
            else if (action.Contains("turn"))
            {
                theAction.Type = ActionType.Turn;
                theAction.Description = action;
                theAction.Delta = 0;
                ClearLastBets();
            }
            else if (action.Contains("river"))
            {
                theAction.Type = ActionType.River;
                theAction.Description = action;
                theAction.Delta = 0;
                ClearLastBets();
            }
            else if (action.Contains("wins"))
            {
                theAction.Type = ActionType.Win;
                theAction.Description = action;
                theAction.Delta = ParseAmount(action, ActionType.Win);
                theAction.Player = ParsePlayer(action);
                UpdateStack(theAction.Player, theAction.Delta, hand.MinBet, true);
            }
            else if (action.Contains("gained"))
            {
                theAction.Type = ActionType.Gained;
                theAction.Description = action;
                theAction.Delta = ParseAmount(action, ActionType.Gained);
                theAction.Player = ParsePlayer(action);
                UpdateStack(theAction.Player, theAction.Delta, hand.MinBet, true);
            }
            else if (action.Contains("ending hand"))
            {
                theAction.Type = ActionType.EndHand;
                theAction.Description = action;
                theAction.Delta = 0;
                ClearLastBets();
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
            }
            else if (action.Contains("quits"))
            {
                theAction.Type = ActionType.PlayerQuit;
                theAction.Description = action;
                theAction.Delta = 0;
                theAction.Player = ParsePlayer(action);
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
            else if (actionType == ActionType.CreateGame ||actionType == ActionType.AddPlayer)
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
            Player newPlayer = new Player()
            {
                Name = name,
                StartingStack = startingStack,
                CurrentStack = startingStack,
                IsStanding = false,
                LastBet = 0
            };

            game.Players?.Add(newPlayer);
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

        private void UpdateStack(Player player, int delta, int minBet, bool win = false)
        {            
            int amount = 0;
            //int modifier = 0;
            //if (win)
            //{
            //    modifier = 1;
            //}
            //else
            //{
            //    modifier = -1;                
            //}

            if (win)
            {
                amount = delta;
            }
            else
            {
                if (player.LastBet < minBet && player.LastBet != 0)
                {
                    amount = (delta - player.LastBet) * -1;
                }
                else if (player.LastBet > 0)
                {
                    amount = (player.LastBet - delta) * -1;
                }
                else
                {
                    amount = delta * -1;
                }
            }            

            player.LastBet = delta;
            player.CurrentStack += amount;
        }

        private void UpdatePot(string stage, int value, Hand hand)
        {
            if (stage == "PreFlop")
            {
                hand.PreFlopPot += value;
            }
            else if (stage == "Flop")
            {
                hand.FlopPot += value;
            }
            else if (stage == "Turn")
            {
                hand.TurnPot += value;
            }
            else if (stage == "River")
            {
                hand.RiverPot += value;
            }

            hand.CurrentPot += value;
        }

        private void ClearLastBets()
        {
            game.Players.ForEach(p => p.LastBet = 0);
        }
    }
}
