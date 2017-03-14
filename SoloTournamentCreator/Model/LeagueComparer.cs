using RiotSharp.LeagueEndpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloTournamentCreator.Model
{
    public class LeagueComparer : IComparer<CSL>
    {
        public int Compare(CSL a, CSL b)
        {
            if (a != null && b != null)
            {
                if (a.Tier == b.Tier)
                {
                    if(a.Entries[0].LeaguePoints == b.Entries[0].LeaguePoints)
                    {
                        return 0;
                    }
                    return a.Entries[0].LeaguePoints > b.Entries[0].LeaguePoints ? 1 : -1;
                }
                switch (a.Tier)
                {
                    case RiotSharp.LeagueEndpoint.Enums.Tier.Challenger :
                        return -1;
                    case RiotSharp.LeagueEndpoint.Enums.Tier.Master:
                        if (b.Tier == RiotSharp.LeagueEndpoint.Enums.Tier.Challenger)
                            return 1;
                        return -1;
                    case RiotSharp.LeagueEndpoint.Enums.Tier.Diamond:
                        if (b.Tier == RiotSharp.LeagueEndpoint.Enums.Tier.Challenger || b.Tier == RiotSharp.LeagueEndpoint.Enums.Tier.Master)
                            return 1;
                        return -1;
                    case RiotSharp.LeagueEndpoint.Enums.Tier.Platinum:
                        if (b.Tier == RiotSharp.LeagueEndpoint.Enums.Tier.Challenger || b.Tier == RiotSharp.LeagueEndpoint.Enums.Tier.Master || b.Tier == RiotSharp.LeagueEndpoint.Enums.Tier.Diamond)
                            return 1;
                        return -1;
                    case RiotSharp.LeagueEndpoint.Enums.Tier.Gold:
                        if (b.Tier == RiotSharp.LeagueEndpoint.Enums.Tier.Silver || b.Tier == RiotSharp.LeagueEndpoint.Enums.Tier.Bronze)
                            return -1;
                        return 1;
                    case RiotSharp.LeagueEndpoint.Enums.Tier.Silver:
                        if (b.Tier == RiotSharp.LeagueEndpoint.Enums.Tier.Bronze)
                            return -1;
                        return 1;
                    case RiotSharp.LeagueEndpoint.Enums.Tier.Bronze:
                        return 1;
                }
            }

            if (a == null || b == null)
            {
                if (ReferenceEquals(a, b))
                {
                    return 0;
                }
                return a == null ? -1 : 1;
            }
            return Comparer<string>.Default.Compare(a.Name, b.Name);
        }
    }
}
