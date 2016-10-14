using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloTournamentCreator.Model
{
    public class Tournament
    {
        bool _isStarted;
        HashSet<Student> _Participants;

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

        public Tournament()
        {
            Participants = new HashSet<Student>();
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
    }
}
