using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloTournamentCreator.Model
{
    /// <summary>
    /// A match between two teams, the LeftContendant winner and the RightContendant Winner.
    /// <para/>The winner become the Match Winner.
    /// <para/>a depth of 0 correspond to the final, of 1 to the demi final ...
    /// </summary>
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
        private bool _IsMainMatch;
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
                if (RightContendant == null && LeftContendant == null && IsMainMatch)
                    return "Bye";
                return "???";
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

        public bool IsMainMatch
        {
            get
            {
                return _IsMainMatch;
            }

            set
            {
                _IsMainMatch = value;
            }
        }

        private Match()
        {
            WinnerScore = 0;
            LoserScore = 0;
        }
        /// <summary>
        /// Create a Match
        /// </summary>
        /// <param name="depth">The depth of the match in the bracket (0: Finale, 1:Semi-Finale ...)</param>
        /// <param name="isMainMatch">true if it belongs in the main bracket, false else</param>
        public Match(int depth, bool isMainMatch)
        {
            WinnerScore = 0;
            LoserScore = 0;
            Depth = depth;
            IsMainMatch = isMainMatch;
        }
        private Match(Team winner)
        {
            WinnerScore = 0;
            LoserScore = 0;
            Winner = winner;
        }
        /// <summary>
        /// Set the winner of the match, it should always be the right or left contendant winner, unless it is the first match (then right and left contendant are null)
        /// </summary>
        /// <param name="winner"></param>
        public void DeclareWinner(Team winner)
        {
            Winner = winner;
        }
        /// <summary>
        /// Will give a free win to all team having no rival for the last match of the Tree, should be used to init a Tournament once all team have been placed (il will take care of the Bye)
        /// </summary>
        public void SetLastRoundAutoWinner()
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
            RightContendant.SetLastRoundAutoWinner();
            LeftContendant.SetLastRoundAutoWinner();
        }
        /// <summary>
        /// Will resolve ALL free wins for the tournament
        /// <para/>WARNING : this is NEVER appropriate for a loser bracket as new team can appear in the bracket at any stage
        /// Note : it should normally not be used in a normal bracket either because only the last stage should contain "free wins"
        /// <para/> Return true if all matchs have been resolved (a winner have been OR there's no winner because there's no team participating)
        /// Return false if there is a match result missing (Two teams are competing in a match and there's no winner set)
        /// </summary>
        /// <returns>
        /// </returns>
        private bool SetAutoWinnerChain()
        {
            bool? leftIsResolved = LeftContendant?.SetAutoWinnerChain();
            bool? rightIsResolved = RightContendant?.SetAutoWinnerChain();

            if (RightContendant == null && LeftContendant == null)
            {
                return true;
            }
            if (RightContendant.Winner != null && LeftContendant.Winner != null)
            {
                return false;
            }
            if (RightContendant?.Winner != null && LeftContendant?.Winner == null && leftIsResolved == true)
            {
                Winner = RightContendant.Winner;
                RightContendant.WinNext = true;
                return true;
            }
            if (LeftContendant?.Winner != null && RightContendant?.Winner == null && rightIsResolved == true )
            {
                Winner = LeftContendant.Winner;
                LeftContendant.WinNext = true;
                return true;
            }
            if (rightIsResolved == leftIsResolved == true)
            {
                return true;
            }
            return false;
        }
    }
}
