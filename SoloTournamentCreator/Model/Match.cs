using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int? LeftContendantId { get; set; }
        Match _LeftContendant;
        public int? RightContendantId { get; set; }
        Match _RightContendant;
        private int _WinnerScore;
        private int _LoserScore;
        private int _Depth;
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
        public string Display
        {
            get
            {
                if (Winner != null)
                    return Winner.TeamName;
                if(RightContendant != null || LeftContendant != null)
                    return "???";
                return "Bye";
            }
        }
        public bool? WinNext { get; set; }
        [ForeignKey("LeftContendantId")]
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
        [ForeignKey("RightContendantId")]
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

        public int WinnerScore
        {
            get
            {
                return _WinnerScore;
            }

            set
            {
                _WinnerScore = value;
            }
        }

        public int LoserScore
        {
            get
            {
                return _LoserScore;
            }

            set
            {
                _LoserScore = value;
            }
        }

        public int Depth
        {
            get
            {
                return _Depth;
            }

            set
            {
                _Depth = value;
            }
        }

        private Match()
        {
            WinnerScore = 0;
            LoserScore = 0;
        }
        public Match(int depth)
        {
            WinnerScore = 0;
            LoserScore = 0;
            Depth = depth;
        }
        public Match(Team winner)
        {
            WinnerScore = 0;
            LoserScore = 0;
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
        public void SetAutoWinner()
        {
            if (RightContendant == null && LeftContendant == null)
                return;
            if(RightContendant.Winner != null && RightContendant.RightContendant == null && RightContendant.LeftContendant == null && LeftContendant.Winner == null)
            {
                Winner = RightContendant.Winner;
                RightContendant.WinNext = true;
                return;
            }
            if (LeftContendant.Winner != null && LeftContendant.RightContendant == null && LeftContendant.LeftContendant == null && RightContendant.Winner == null)
            {
                Winner = LeftContendant.Winner;
                LeftContendant.WinNext = true;
                return;
            }
            RightContendant.SetAutoWinner();
            LeftContendant.SetAutoWinner();
        }
    }
}
