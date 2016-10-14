using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloTournamentCreator.Model
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }
        private int NbPlayerMax;
        HashSet<Student> _TeamMember;
        string _TeamName;

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

        public int TeamPower
        {
            get
            {
                return TeamMember.Sum(x => x.EstimatedStrenght);
            }
        }

        public string TeamName
        {
            get
            {
                return _TeamName;
            }

            set
            {
                _TeamName = value;
            }
        }

        public Team(int nbPlayerMax = 5, string teamName = "NoName")
        {
            TeamMember = new HashSet<Student>();
            TeamName = teamName;
            NbPlayerMax = nbPlayerMax;
        }

        public bool AddMember(Student member)
        {
            if (TeamMember.Count >= NbPlayerMax)
                return false;
            return TeamMember.Add(member);
        }

        public bool RemoveMember(Student member)
        {
            return TeamMember.Remove(member);
        }
    }
}
