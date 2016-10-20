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
        //Separated Right and Left starting position so we are sure to never have a double Bye on the first round
        List<Match> _LeftEndPoint;
        List<Match> _RightEndPoint;
        Match _MyTournamentTree;
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
        private TournamentTree()
        {

        }
        public TournamentTree(HashSet<Team> teams)
        {
            MyTournamentTree = new Match();
            LeftEndPoint = new List<Match>();
            RightEndPoint = new List<Match>();
            GenerateTournamentTree(MyTournamentTree, Math.Ceiling(Math.Log(teams.Count(), 2)));
            SetTeamStartingPosition(teams);
            printTree();
        }

        private void GenerateTournamentTree(Match node, double depth)
        {
            if(depth > 1)
            {
                node.LeftContendant = new Match();
                GenerateTournamentTree(node.LeftContendant, depth - 1);
                node.RightContendant = new Match();
                GenerateTournamentTree(node.RightContendant, depth - 1);
            }else
            {
                node.LeftContendant = new Match();
                node.RightContendant = new Match();
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
    }
}
