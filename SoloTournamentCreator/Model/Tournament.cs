using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloTournamentCreator.Model
{
    public class Tournament
    {
        private readonly int nbPlayerPerTeam = 5;
        private readonly int nbTeam = 8;
        bool _isStarted;
        HashSet<Student> _Participants;
        HashSet<Team> _Teams;

        public bool IsStarted
        {
            get
            {
                return _isStarted;
            }

            set
            {
                _isStarted = value;
            }
        }
        public int nbParticipant
        {
            get
            {
                return _Participants.Count;
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

        public Tournament()
        {
            Participants = new HashSet<Student>();
            Teams = new HashSet<Team>();
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
            throw new NotImplementedException();
        }

        private void CreateTeam()
        {
            for (int i = 0; i < (int)Participants.Count / nbPlayerPerTeam; i++)
            {
                Teams.Add(new Team(nbPlayerPerTeam, "TeamNumber"+i.ToString()));
            }

        }
        private void BalanceTeam()
        {
            HashSet<Team> availableTeam = new HashSet<Team>(Teams);
            int nbIteration = Math.Min((int)Participants.Count/ nbPlayerPerTeam, nbTeam) * nbPlayerPerTeam;
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
    }
}
