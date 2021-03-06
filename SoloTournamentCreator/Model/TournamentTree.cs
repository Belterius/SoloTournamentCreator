﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloTournamentCreator.Model
{
    public class TournamentTree
    {
        [Key]
        public int TournamentTreeId { get; set; }
        private double _MaxDepth;
        private bool _HasLoserBracket;
        //Separated Right and Left starting position so we are sure to never have a double Bye on the first round
        List<Match> _LeftEndPoint;
        List<Match> _RightEndPoint;
        List<Match> _LoserBracketLeftEndPoint;
        List<Match> _LoserBracketRightEndPoint;
        Match _MyMainTournamentTree;
        Match _MySecondaryTournamentTree;

        public Match MyMainTournamentTree
        {
            get
            {
                return _MyMainTournamentTree;
            }

            set
            {
                _MyMainTournamentTree = value;
            }
        }

        public Match MySecondaryTournamentTree
        {
            get
            {
                return _MySecondaryTournamentTree;
            }

            set
            {
                _MySecondaryTournamentTree = value;
            }
        }
        private List<Match> LeftEndPoint
        {
            get
            {
                return _LeftEndPoint;
            }

            set
            {
                _LeftEndPoint = value;
            }
        }

        private List<Match> RightEndPoint
        {
            get
            {
                return _RightEndPoint;
            }

            set
            {
                _RightEndPoint = value;
            }
        }

        public double MaxDepth
        {
            get
            {
                return _MaxDepth;
            }

            set
            {
                _MaxDepth = value;
            }
        }

        private List<Match> LoserBracketLeftEndPoint
        {
            get
            {
                return _LoserBracketLeftEndPoint;
            }

            set
            {
                _LoserBracketLeftEndPoint = value;
            }
        }

        private List<Match> LoserBracketRightEndPoint
        {
            get
            {
                return _LoserBracketRightEndPoint;
            }

            set
            {
                _LoserBracketRightEndPoint = value;
            }
        }

        public bool HasLoserBracket
        {
            get
            {
                return _HasLoserBracket;
            }

            set
            {
                _HasLoserBracket = value;
            }
        }

        private TournamentTree()
        {
            LeftEndPoint = new List<Match>();
            RightEndPoint = new List<Match>();
            LoserBracketLeftEndPoint = new List<Match>();
            LoserBracketRightEndPoint = new List<Match>();
        }
        public TournamentTree(HashSet<Team> teams, bool hasLoserBracket) : this()
        {
            HasLoserBracket = hasLoserBracket;
            MyMainTournamentTree = new Match(0, true);
            MySecondaryTournamentTree = new Match(0, false);
            MaxDepth = Math.Ceiling(Math.Log(teams.Count(), 2));
            GenerateTournamentTree(MyMainTournamentTree, 1);
            if (HasLoserBracket)
            {
                GenerateLoserBracketTree(MySecondaryTournamentTree, 1);
            }
            else
            {
                GenerateThirdMatchPlaceTree(MySecondaryTournamentTree);
            }
            SetTeamStartingPosition(teams);
            SetFreeWin();
        }

        /// <summary>
        /// Generate a third place match tree
        /// </summary>
        /// <param name="node">The Final Node for the third place match</param>
        private void GenerateThirdMatchPlaceTree(Match node)
        {
            
            node.LeftContendant = new Match(node.Depth + 1, false);
            node.RightContendant = new Match(node.Depth + 1, false);
        }
        /// <summary>
        /// Generate a loser bracket tree of a set depth
        /// <para />we create a right and left node on which we'll attach the rest of the tree
        /// <para />the left node are for team progressing throught the bracket, the right node are for the team that have just been kicked from the main tournament
        /// <para /> when the depth reaches "(MaxDepth - 1) * 2" we stop expanding the Tree, MaxDepth being the Depth of our Main Tournament
        /// </summary>
        /// <param name="node">The parent node</param>
        /// <param name="depth">The depth of the Node</param>
        private void GenerateLoserBracketTree(Match node, int depth)
        {
            node.RightContendant = new Match(depth, false);
            node.LeftContendant = new Match(depth, false);
            if((MaxDepth - 1) * 2 > depth)
            {
                GenerateLoserBracketTree(node.LeftContendant, depth + 1);
                if(depth % 2 == 0)//Every other time the right contendant comme just straight from the main Tree
                {
                    GenerateLoserBracketTree(node.RightContendant, depth + 1);
                }else
                {
                    LoserBracketRightEndPoint.Add(node.RightContendant);
                }
            }else
            {
                LoserBracketLeftEndPoint.Add(node.LeftContendant);
                LoserBracketRightEndPoint.Add(node.RightContendant);
            }
            //Think if it's better to have TournamentTree.Left = TournamentTree TournamentTree.Right = MyThirdMatchPlace
            //Or if I generate a Final Third tree with one match <-- easier but let pretty I think
        }
        /// <summary>
        /// Set the possibles spawn point for the team being kicked from the main bracket onto the loser bracket
        /// </summary>
        /// <param name="match">The node from which we'll start looking for end points</param>
        private void SetLoserEndPoint(Match match)
        {
            if(match.RightContendant == null && match.LeftContendant == null)
            {
                return;
            }
            if(match.RightContendant != null)
            {
                SetLoserEndPoint(match.RightContendant);
                if (match.RightContendant.RightContendant == null && match.RightContendant.LeftContendant == null)
                {
                    LoserBracketRightEndPoint.Add(match.RightContendant);
                }
            }
            if (match.LeftContendant != null)
            {
                SetLoserEndPoint(match.LeftContendant);
                if (match.LeftContendant.RightContendant == null && match.LeftContendant.LeftContendant == null)
                {
                    LoserBracketLeftEndPoint.Add(match.LeftContendant);
                }
            }
            
        }
        /// <summary>
        /// Place a team onto one of the possible spawn point of the corresponding stage
        /// </summary>
        /// <param name="team">The team to place</param>
        /// <param name="depth">The depth at which the team lost in the main bracket</param>
        private void SetLoserPosition(Team team, int depth)
        {
            int loserDepth;
            if(depth == 1)
            {
                loserDepth = 1;
            }else
            if(depth == MaxDepth)
            {
                loserDepth = (depth - 1) * 2;
            }else
            {
                loserDepth = depth * 2 - 1;
            }
            Random randomizer = new Random();
            if (LoserBracketLeftEndPoint.Where(x => x.Depth == loserDepth && x.Winner == team).SingleOrDefault() != null || LoserBracketRightEndPoint.Where(x => x.Depth == loserDepth && x.Winner == team).SingleOrDefault() != null)
            {
                return;
            }
            if(depth == MaxDepth)
            {
                //special case, because I can have a non-full Tree, and I don't want to have an empty match on the irght and left side
                try
                {
                    LoserBracketLeftEndPoint.OrderBy(i => randomizer.Next()).Where(x => x.Depth == loserDepth && x.Winner == null).First().Winner = team;
                }
                catch (InvalidOperationException)
                {
                    //if I don't have any LeftEndPoint, I can start filling the Right ones
                    LoserBracketRightEndPoint.OrderBy(i => randomizer.Next()).Where(x => x.Depth == loserDepth && x.Winner == null).First().Winner = team;
                }

            }
            else
            {
                LoserBracketRightEndPoint.OrderBy(i => randomizer.Next()).Where(x => x.Depth == loserDepth && x.Winner == null).First().Winner = team;
            }
        }
        /// <summary>
        /// Remove a team from it's spawn point in the loser bracket, should only be called if wrong results had been reported 
        /// </summary>
        /// <param name="team">The team to remove</param>
        /// <param name="depth">The depth at which the team lost in the main bracket</param>
        private void RemoveLoserPosition(Team team, int depth)
        {
            int loserDepth;
            if (depth == 1)
            {
                loserDepth = 1;
            }
            else
            if (depth == MaxDepth)
            {
                loserDepth = (depth - 1) * 2;
            }
            else
            {
                loserDepth = depth * 2 - 1;
            }
            Random randomizer = new Random();
            if (LoserBracketLeftEndPoint.Where(x => x.Depth == loserDepth && x.Winner == team).SingleOrDefault() != null || LoserBracketRightEndPoint.Where(x => x.Depth == loserDepth && x.Winner == team).SingleOrDefault() != null)
            {
                try
                {
                    LoserBracketLeftEndPoint.Where(x => x.Depth == loserDepth && x.Winner == team).Single().Winner = null;
                }
                catch (InvalidOperationException)
                {
                    LoserBracketRightEndPoint.Where(x => x.Depth == loserDepth && x.Winner == team).Single().Winner = null;
                }

            }
        }
        /// <summary>
        /// WARNING FUNCTION FOR TESTING PURPOSE ONLY, DO NOT CALL IT UNDER ANY OTHER CIRCUMSTANCE
        /// <para /> It only allows to check the structure of the trees without having to bother creating teams
        /// </summary>
        /// <param name="depth">The depth of the main bracket to create</param>
        public TournamentTree(int depth)
        {
            
            MyMainTournamentTree = new Match(0, true);
            MySecondaryTournamentTree = new Match(0, true);
            LeftEndPoint = new List<Match>();
            RightEndPoint = new List<Match>();
            LoserBracketLeftEndPoint = new List<Match>();
            LoserBracketRightEndPoint = new List<Match>();
            MaxDepth = depth;
            GenerateTournamentTree(MyMainTournamentTree, 1);
            GenerateLoserBracketTree(MySecondaryTournamentTree, 1);
            
        }

        /// <summary>
        /// Create a tournament bracket of size MaxDepth.
        /// </summary>
        /// <param name="node">The parent node to the rest of the tree</param>
        /// <param name="depth">The depth of the parent node</param>
        private void GenerateTournamentTree(Match node, int depth)
        {
            node.LeftContendant = new Match(depth, true);
            node.RightContendant = new Match(depth, true);
            if (MaxDepth > depth)
            {
                GenerateTournamentTree(node.LeftContendant, depth + 1);
                GenerateTournamentTree(node.RightContendant, depth + 1);
            }else
            {
                LeftEndPoint.Add(node.LeftContendant);
                RightEndPoint.Add(node.RightContendant);
            }
        }

        /// <summary>
        /// Place all the team to a possible spawn point.
        /// <para /> place on all the left end point first to avoid having an empty match and some full matchs
        /// </summary>
        /// <param name="teams">All the team participating</param>
        private void SetTeamStartingPosition(HashSet<Team> teams)
        {
            Random randomizer = new Random();
            HashSet<Team> teamList = new HashSet<Team>(teams);
            var randomlyOrderedRightEndpoint = RightEndPoint.OrderBy(i => randomizer.Next());
            var randomlyOrderedLeftEndpoint = LeftEndPoint.OrderBy(i => randomizer.Next());
            foreach (Match match in randomlyOrderedLeftEndpoint)
            {
                if (teamList.Count == 0)
                {
                    break;
                }

                Team[] teamAvailable = teamList.ToArray();
                int randomChoice = randomizer.Next(teamAvailable.Length);
                Team selectedTeam = teamAvailable[randomChoice];
                teamList.Remove(selectedTeam);

                match.Winner = selectedTeam;
            }
            foreach (Match match in randomlyOrderedRightEndpoint)
            {
                if(teamList.Count == 0)
                {
                    break;
                }

                Team[] teamAvailable = teamList.ToArray();
                int randomChoice = randomizer.Next(teamAvailable.Length);
                Team selectedTeam = teamAvailable[randomChoice];
                teamList.Remove(selectedTeam);

                match.Winner = selectedTeam;
            }

        }
        /// <summary>
        /// Print a visual output of the main bracket onto the console
        /// </summary>
        private void printTree()
        {
            Queue<Match> queue = new Queue<Match>();
            queue.Enqueue(MyMainTournamentTree);
            string output = "";
            while (queue.Count != 0)
            {
                Match current = queue.Dequeue();
                if (current.LeftContendant != null) queue.Enqueue(current.LeftContendant);
                if (current.RightContendant != null) queue.Enqueue(current.RightContendant);

                if (current.RightContendant == null && current.LeftContendant == null)
                {
                    if (current.Winner != null)
                        output += current.Winner.TeamName + " ";
                    else
                        output += "bye";

                }
            }
            Console.WriteLine(output);
        }
        /// <summary>
        /// Make all team with no adversary progress throught their match
        /// </summary>
        private void SetFreeWin()
        {
            MyMainTournamentTree.SetLastRoundAutoWinner();
        }
        /// <summary>
        /// Depending of the result of a main bracket match, take the necessary steps to update the loser bracket/third place match
        /// </summary>
        /// <param name="match">The main bracket match that ended</param>
        public void UpdateSecondaryBracket(Match match)
        {
            if(MySecondaryTournamentTree == null)
            {
                return;
            }
            if (! match.IsMainMatch)
            {
                //If the match is from the Loser Bracket already, we don't need to do any additionnal work
                return;
            }
            if (HasLoserBracket)
            {
                if(LoserBracketLeftEndPoint.Count == 0 && LoserBracketRightEndPoint.Count == 0)
                {
                    // We can't save our EndPoint because we don't have any direct acces to our Matchs
                    //In the case of the MainTournamentTree it doesn't matter because we only use the EndPoint when creating our Tree, but here we have EndPoint at all differents stages
                    //So we need a way to keep them in case we don't finish the tournament in one go
                    //the best way to do that, is simply to regenerate our end points if we don't have any (Loaded from the DB)
                    SetLoserEndPoint(MySecondaryTournamentTree);
                }
                RemoveLoserPosition(match.Winner, match.Depth + 1);

                if (match.LeftContendant.Winner != match.Winner)
                {
                    SetLoserPosition(match.LeftContendant.Winner, match.Depth + 1);
                }
                else{
                    SetLoserPosition(match.RightContendant.Winner, match.Depth + 1);
                }

            }
            else
            {
                if (MyMainTournamentTree?.LeftContendant?.Winner != null)
                {
                    if (MyMainTournamentTree.LeftContendant.Winner != MyMainTournamentTree.LeftContendant.LeftContendant.Winner)
                    {
                        MySecondaryTournamentTree.LeftContendant.Winner = MyMainTournamentTree.LeftContendant.LeftContendant.Winner;
                    }
                    else
                    {
                        MySecondaryTournamentTree.LeftContendant.Winner = MyMainTournamentTree.LeftContendant.RightContendant.Winner;
                    }
                }
                if (MyMainTournamentTree?.RightContendant?.Winner != null)
                {
                    if (MyMainTournamentTree.RightContendant.Winner != MyMainTournamentTree.RightContendant.LeftContendant.Winner)
                    {
                        MySecondaryTournamentTree.RightContendant.Winner = MyMainTournamentTree.RightContendant.LeftContendant.Winner;
                    }
                    else
                    {
                        MySecondaryTournamentTree.RightContendant.Winner = MyMainTournamentTree.RightContendant.RightContendant.Winner;
                    }
                }
            }
            
        }

        /// <summary>
        /// Fusion of the main and secondary bracket into the final tournament tree
        /// </summary>
        internal void StartFinalStage()
        {
            if (MySecondaryTournamentTree == null)
                return;

            var tempo = new Match(0, true);
            tempo.LeftContendant = MyMainTournamentTree;
            tempo.RightContendant = MySecondaryTournamentTree;
            MyMainTournamentTree = tempo;
            MySecondaryTournamentTree = null;
        }
    }
}
