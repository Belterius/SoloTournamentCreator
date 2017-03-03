using RiotSharp.LeagueEndpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotSharp;
using RiotSharp.LeagueEndpoint.Enums;
using System.ComponentModel.DataAnnotations;

namespace SoloTournamentCreator.Model
{
    public class CSL
    {
        [Key]
        public int CSLId { get; set; }
        public List<CSLE> Entries { get; set; }
        public String Name { get; set; }
        public String ParticipantId { get; set; }
        public Queue Queue { get; set; }
        public Tier Tier { get; set; }
        
        public CSL(RiotSharp.LeagueEndpoint.League league)
        {
            Entries = new List<CSLE>();
            foreach(LeagueEntry leagueEntry in league.Entries)
            {
                CSLE CLE = new CSLE(leagueEntry);
                Entries.Add(CLE);
            }
            Name = league.Name;
            ParticipantId = league.ParticipantId;
            Queue = league.Queue;
            Tier = league.Tier;
        }
        public CSL()
        {

        }

    }

    //CustomSharpLeagueEntry
    public class CSLE
    {
        [Key]
        public int CSLEId { get; set; }
        public String Division { get; set; }
        public bool IsFreshBlood { get; set; }
        public bool IsHotStreak { get; set; }
        public bool IsInactive { get; set; }
        public bool IsVeteran { get; set; }
        public int LeaguePoints { get; set; }
        public int Losses { get; set; }
        public CustomMiniSeries MiniSeries { get; set; }
        public String PlayerOrTeamId { get; set; }
        public String PlayerOrTeamName { get; set; }
        public int Wins { get; set; }
        public CSLE(RiotSharp.LeagueEndpoint.LeagueEntry leagueEntry)
        {
            Division = leagueEntry.Division;
            IsFreshBlood = leagueEntry.IsFreshBlood;
            IsHotStreak = leagueEntry.IsHotStreak;
            IsInactive = leagueEntry.IsInactive;
            IsVeteran = leagueEntry.IsVeteran;
            LeaguePoints = leagueEntry.LeaguePoints;
            Losses = leagueEntry.Losses;
            PlayerOrTeamId = leagueEntry.PlayerOrTeamId;
            PlayerOrTeamName = leagueEntry.PlayerOrTeamName;
            Wins = leagueEntry.Wins;
            if(leagueEntry.MiniSeries != null)
                MiniSeries = new CustomMiniSeries(leagueEntry.MiniSeries);
        }
        public CSLE()
        {

        }
    }
    public class CustomMiniSeries
    {
        //Requiered to have a Key to be able to save the data in EntityFrameWork, so had to create a custom "copy"
        [Key]
        public int CustomMiniSeriesId { get; set; }
        public int Losses { get; set; }
        public Char[] Progress { get; set; }
        public int Target { get; set; }
        public int Wins { get; set; }
        public CustomMiniSeries(RiotSharp.LeagueEndpoint.MiniSeries miniSeries)
        {
            Wins = miniSeries.Wins;
            Losses = miniSeries.Losses;
            Progress = miniSeries.Progress;
            Target = miniSeries.Target;
        }
        public CustomMiniSeries()
        {

        }
    }
}
