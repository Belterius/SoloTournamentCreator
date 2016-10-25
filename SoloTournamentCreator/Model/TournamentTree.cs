using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        //Separated Right and Left starting position so we are sure to never have a double Bye on the first round
        List<Match> _LeftEndPoint;
        List<Match> _RightEndPoint;
        Match _MyTournamentTree;
        Match _MyThirdMatchPlace;
        public Match MyTournamentTree
        {
            get
            {
                return _MyTournamentTree;
            }

            set
            {
                _MyTournamentTree = value;
            }
        }
        public Match MyThirdMatchPlace
        {
            get
            {
                return _MyThirdMatchPlace;
            }

            set
            {
                _MyThirdMatchPlace = value;
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

        private TournamentTree()
        {

        }
        public TournamentTree(HashSet<Team> teams)
        {
            MyTournamentTree = new Match(0);
            MyThirdMatchPlace = new Match(0);
            MyThirdMatchPlace.LeftContendant = new Match(MyThirdMatchPlace.Depth + 1);
            MyThirdMatchPlace.RightContendant = new Match(MyThirdMatchPlace.Depth + 1);
            LeftEndPoint = new List<Match>();
            RightEndPoint = new List<Match>();
            MaxDepth = Math.Ceiling(Math.Log(teams.Count(), 2));
            GenerateTournamentTree(MyTournamentTree, 1);
            SetTeamStartingPosition(teams);
            SetFreeWin();
        }
        public TournamentTree(int depth)
        {
            MyTournamentTree = new Match(0);
            LeftEndPoint = new List<Match>();
            RightEndPoint = new List<Match>();
            MaxDepth = depth;
            GenerateTournamentTree(MyTournamentTree, 1);
        }

        private void GenerateTournamentTree(Match node, int depth)
        {
            if(MaxDepth > depth)
            {
                node.LeftContendant = new Match(depth);
                GenerateTournamentTree(node.LeftContendant, depth + 1);
                node.RightContendant = new Match(depth);
                GenerateTournamentTree(node.RightContendant, depth + 1);
            }else
            {
                node.LeftContendant = new Match(depth);
                node.RightContendant = new Match(depth);
                LeftEndPoint.Add(node.LeftContendant);
                RightEndPoint.Add(node.RightContendant);
            }
        }
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

        private void printTree()
        {
            Queue<Match> queue = new Queue<Match>();
            queue.Enqueue(MyTournamentTree);
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
        private void SetFreeWin()
        {
            MyTournamentTree.SetAutoWinner();
        }
        public void UpdateThirdMatchPlace()
        {
            if(MyTournamentTree?.LeftContendant?.Winner != null)
            {
                if(MyTournamentTree.LeftContendant.Winner != MyTournamentTree.LeftContendant.LeftContendant.Winner)
                {
                    MyThirdMatchPlace.LeftContendant.Winner = MyTournamentTree.LeftContendant.LeftContendant.Winner;
                }else
                {
                    MyThirdMatchPlace.LeftContendant.Winner = MyTournamentTree.LeftContendant.RightContendant.Winner;
                }
            }
            if (MyTournamentTree?.RightContendant?.Winner != null)
            {
                if (MyTournamentTree.RightContendant.Winner != MyTournamentTree.RightContendant.LeftContendant.Winner)
                {
                    MyThirdMatchPlace.RightContendant.Winner = MyTournamentTree.RightContendant.LeftContendant.Winner;
                }
                else
                {
                    MyThirdMatchPlace.RightContendant.Winner = MyTournamentTree.RightContendant.RightContendant.Winner;
                }
            }
        }
    }
}
