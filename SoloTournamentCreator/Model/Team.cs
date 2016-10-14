using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloTournamentCreator.Model
{
    public class Team
    {
        HashSet<Student> _TeamMember;

        public HashSet<Student> TeamMember
        {
            get
            {
                return _TeamMember;
            }

            set
            {
                _TeamMember = value;
            }
        }


        public bool AddMember(Student member)
        {
            return TeamMember.Add(member);
        }

        public bool RemoveMember(Student member)
        {
            return TeamMember.Remove(member);
        }
    }
}
