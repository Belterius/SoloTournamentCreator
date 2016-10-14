using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloTournamentCreator.Model
{
    public class Match
    {
        [Key]
        public int MatchId { get; set; }
        Team _Winner;
        Match _LeftContendant;
        Match _RightContendant;

        public Team Winner
        {
            get
            {
                return _Winner;
            }

            set
            {
                _Winner = value;
            }
        }

        public Match LeftContendant
        {
            get
            {
                return _LeftContendant;
            }

            set
            {
                _LeftContendant = value;
            }
        }

        public Match RightContendant
        {
            get
            {
                return _RightContendant;
            }

            set
            {
                _RightContendant = value;
            }
        }

        public Match()
        {
        }
        public Match(Team winner)
        {
            Winner = winner;
        }

        public void DeclareWinner(Match winner)
        {
            Winner = winner.Winner;
        }
        public void DeclareWinner(Team winner)
        {
            Winner = winner;
        }
    }
}
