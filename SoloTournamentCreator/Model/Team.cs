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
        private int _NbPlayerMax;
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

        public int NbPlayerMax
        {
            get
            {
                return _NbPlayerMax;
            }

            set
            {
                _NbPlayerMax = value;
            }
        }

        private Team()
        {
            TeamMember = new HashSet<Student>();
        }
        public Team(int nbPlayerMax = 5, string teamName = "NoName")
        {
            TeamMember = new HashSet<Student>();
            TeamName = teamName;
            NbPlayerMax = nbPlayerMax;
        }

        /// <summary>
        /// If the team isn't full, add a Student to the team members.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public bool AddMember(Student member)
        {
            if (TeamMember.Count >= NbPlayerMax)
                throw new InvalidOperationException("You can't add that many member to a team");
            return TeamMember.Add(member);
        }

        /// <summary>
        /// Remove a Student from the team
        /// <para/> Return true if the Student was part of the team, false else
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public bool RemoveMember(Student member)
        {
            return TeamMember.Remove(member);
        }

        /// <summary>
        /// Rename the team name
        /// </summary>
        /// <param name="teamName"></param>
        /// <returns></returns>
        public bool Rename(string teamName)
        {
            //Possibility to add a check for offensive name or else,I don't need it for now
            if (teamName == null || teamName == "")
                return false;

            TeamName = teamName;
            return true;
        }
    }
}
