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
        private bool _HasLoserBracket;
        //Separated Right and Left starting position so we are sure to never have a double Bye on the first round
        List<Match> _LeftEndPoint;
        List<Match> _RightEndPoint;
        List<Match> _LoserBracketLeftEndPoint;
        List<Match> _LoserBracketRightEndPoint;
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

        public List<Match> LoserBracketLeftEndPoint
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

        public List<Match> LoserBracketRightEndPoint
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

        }
        public TournamentTree(HashSet<Team> teams, bool hasLoserBracket)
        {
            HasLoserBracket = hasLoserBracket;
            MyTournamentTree = new Match(0);
            MyThirdMatchPlace = new Match(0);
            LeftEndPoint = new List<Match>();
            RightEndPoint = new List<Match>();
            LoserBracketLeftEndPoint = new List<Match>();
            LoserBracketRightEndPoint = new List<Match>();
            MaxDepth = Math.Ceiling(Math.Log(teams.Count(), 2));
            GenerateTournamentTree(MyTournamentTree, 1);
            if (HasLoserBracket)
            {
                GenerateLoserBracketTree(MyThirdMatchPlace, 1);
            }
            else
            {
                GenerateThirdMatchPlaceTree(MyThirdMatchPlace);
            }
            SetTeamStartingPosition(teams);
            SetFreeWin();
        }

        private void GenerateThirdMatchPlaceTree(Match node)
        {
            
            node.LeftContendant = new Match(node.Depth + 1);
            node.RightContendant = new Match(node.Depth + 1);
        }
        private void GenerateLoserBracketTree(Match node, int depth)
        {
            node.RightContendant = new Match(depth);
            node.LeftContendant = new Match(depth);
            if((MaxDepth - 1) * 2 > depth)
            {
                GenerateLoserBracketTree(node.LeftContendant, depth + 1);
                if(depth%2 == 0)//Every other time the right contendant comme just straight from the main Tree
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
            if (LoserBracketLeftEndPoint.Where(x => x.Depth == loserDepth && x.Winner == team).SingleOrDefault() != null || LoserBracketRightEndPoint.Where(x => x.Depth == loserDepth && x.Winner == team).Single() != null)
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

        private TournamentTree(int depth)
        {
            //WARNING FUNCTION FOR TESTING PURPOSE ONLY, DO NOT CALL IT UNDER ANY OTHER CIRCUMSTANCE
            //It only allows to check the structure of the trees without having to bother creating teams
            MyTournamentTree = new Match(0);
            MyThirdMatchPlace = new Match(0);
            LeftEndPoint = new List<Match>();
            RightEndPoint = new List<Match>();
            LoserBracketLeftEndPoint = new List<Match>();
            LoserBracketRightEndPoint = new List<Match>();
            MaxDepth = depth;
            GenerateTournamentTree(MyTournamentTree, 1);
            GenerateLoserBracketTree(MyThirdMatchPlace, 1);
            
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
        public void UpdateThirdMatchPlace(Match match)
        {
            if (HasLoserBracket)
            {
                RemoveLoserPosition(match.Winner, match.Depth);

                if (match.LeftContendant.Winner != match.Winner)
                {
                    SetLoserPosition(match.LeftContendant.Winner, match.Depth);
                }
                else{
                    SetLoserPosition(match.RightContendant.Winner, match.Depth);
                }

            }
            else
            {
                if (MyTournamentTree?.LeftContendant?.Winner != null)
                {
                    if (MyTournamentTree.LeftContendant.Winner != MyTournamentTree.LeftContendant.LeftContendant.Winner)
                    {
                        MyThirdMatchPlace.LeftContendant.Winner = MyTournamentTree.LeftContendant.LeftContendant.Winner;
                    }
                    else
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
}
