﻿using MoreLinq;
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
                return MyTournamentTree.MyTournamentTree.Winner;
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
        public Tournament(string name="NoName", int nbTeam = 8, bool hasLoserBracket = false)
        {
            Participants = new HashSet<Student>();
            Teams = new HashSet<Team>();
            Status = TournamentStage.Open;
            Name = name;
            NbTeam = nbTeam;
            HasLoserBracket = hasLoserBracket;
        }

        public bool Register(Student participant)
        {
            return Participants.Add(participant);
        }

        public bool Deregister(Student participant)
        {
            return Participants.Remove(participant);
        }

        public void Start()
        {
            Clean();
            CreateTeam();
            BalanceTeam();
            CreateTournamentTree();
        }
        private void Clean()
        {
            Teams = new HashSet<Team>();
        }
        private void CreateTeam()
        {
            for (int i = 0; i < Math.Min(Participants.Count/nbPlayerPerTeam, NbTeam); i++)
            {
                Teams.Add(new Team(nbPlayerPerTeam, "TeamNumber"+i.ToString()));
            }

        }
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
        private void CreateTournamentTree()
        {
            MyTournamentTree = new TournamentTree(Teams, HasLoserBracket);
            Status = TournamentStage.Started;
        }
        public void Archive()
        {
            if(Status == TournamentStage.Started)
            {
                Status = TournamentStage.Completed;
            }
        }
    }
}
