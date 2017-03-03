using MoreLinq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloTournamentCreator.Model
{
    public class Tournament
    {
        public enum TournamentStage
        {
            Open,
            Started,
            Completed
        }
        [Key]
        public int TournamentId { get; set; }
        private readonly int nbPlayerPerTeam = 5;
        private bool _HasLoserBracket;
        private int _NbTeam;
        TournamentStage _Status;
        HashSet<Student> _Participants;
        HashSet<Team> _Teams;
        TournamentTree _MyTournamentTree;
        string _Name;

        public TournamentStage Status
        {
            get
            {
                return _Status;
            }

            set
            {
                _Status = value;
            }
        }
        public int nbParticipant
        {
            get
            {
                return _Participants.Count;
            }
        }
        public int nbParticipantMax
        {
            get
            {
                return NbTeam*nbPlayerPerTeam;
            }
        }

        public HashSet<Student> Participants
        {
            get
            {
                return _Participants;
            }

            set
            {
                _Participants = value;
            }
        }

        public HashSet<Team> Teams
        {
            get
            {
                return _Teams;
            }

            set
            {
                _Teams = value;
            }
        }

        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
            }
        }

        public int NbTeam
        {
            get
            {
                return _NbTeam;
            }

            set
            {
                _NbTeam = value;
            }
        }

        public TournamentTree MyTournamentTree
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

        public Team TournamentWinner
        {
            get
            {
                return MyTournamentTree.MyMainTournamentTree.Winner;
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

        private Tournament()
        {
            Participants = new HashSet<Student>();
            Teams = new HashSet<Team>();
        }
        /// <summary>
        /// Create a Tournament
        /// </summary>
        /// <param name="name">The name of the Tournament</param>
        /// <param name="nbTeam">The Max number of team</param>
        /// <param name="hasLoserBracket">True if require a loser bracket, false if only third place match</param>
        public Tournament(string name="NoName", int nbTeam = 8, bool hasLoserBracket = false) : this()
        {
            Status = TournamentStage.Open;
            Name = name;
            NbTeam = nbTeam;
            HasLoserBracket = hasLoserBracket;
        }
        /// <summary>
        /// Register a Student to the Tournament
        /// </summary>
        /// <param name="participant">The Student to register</param>
        /// <returns>True if worked, false if the student was already registered</returns>
        public bool Register(Student participant)
        {
            return Participants.Add(participant);
        }
        /// <summary>
        /// Deregister a Student from the Tournament
        /// </summary>
        /// <param name="participant">The Student to deregister</param>
        /// <returns>True if worked, false if the Student wasn't registered</returns>
        public bool Deregister(Student participant)
        {
            return Participants.Remove(participant);
        }
        /// <summary>
        /// Start the tournament.
        /// <para />Create balanced team fron all the registered Students.
        /// <para />Create a main bracket (and loser bracket if required) and place the Teams in it.
        /// </summary>
        public void Start()
        {
            Clean();
            CreateTeam();
            RefreshPlayers();
            BalanceTeam();
            CreateTournamentTree();
        }
        /// <summary>
        /// Merge the winner and loser bracket into the final bracket
        /// </summary>
        public void StartFinalStage()
        {
            MyTournamentTree?.StartFinalStage();
        }
        /// <summary>
        /// Remove all existing team
        /// </summary>
        private void Clean()
        {
            Teams = new HashSet<Team>();
        }
        /// <summary>
        /// Create the number of team required to fit all the participants OR to reach the max number of team allowed
        /// </summary>
        private void CreateTeam()
        {
            for (int i = 0; i < Math.Min(Participants.Count/nbPlayerPerTeam, NbTeam); i++)
            {
                Teams.Add(new Team(nbPlayerPerTeam, "TeamNumber"+i.ToString()));
            }

        }
        /// <summary>
        /// Update all the players data to make sure to have up to date information
        /// </summary>
        public void RefreshPlayers()
        {
            foreach(Student player in Participants)
            {
                player.RefreshData();
            }

        }
        /// <summary>
        /// Depending on the level of each players, create the most balanced teams possible
        /// </summary>
        private void BalanceTeam()
        {
            HashSet<Team> availableTeam = new HashSet<Team>(Teams);
            int nbIteration = Math.Min(Participants.Count/ nbPlayerPerTeam, NbTeam) * nbPlayerPerTeam;
            for (; nbIteration > 0; nbIteration--){
                Team weakestTeam = availableTeam.MinBy(x => x.TeamPower);
                Student strongestParticipant = Participants.MaxBy(x => x.EstimatedStrenght);
                Participants.Remove(strongestParticipant);
                weakestTeam.AddMember(strongestParticipant);
                if(weakestTeam.TeamMember.Count() >= nbPlayerPerTeam)
                {
                    availableTeam.Remove(weakestTeam);
                }
            }
        }
        /// <summary>
        /// Create the loser and winner bracket and officially pass our Tournament onto the Started status
        /// </summary>
        private void CreateTournamentTree()
        {
            MyTournamentTree = new TournamentTree(Teams, HasLoserBracket);
            Status = TournamentStage.Started;
        }
        /// <summary>
        /// Pass the Tournament from the Started to the Completed Status
        /// </summary>
        public void Archive()
        {
            if(Status == TournamentStage.Started)
            {
                Status = TournamentStage.Completed;
            }
        }
    }
}
