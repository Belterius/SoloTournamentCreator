using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloTournamentCreator.Model
{
    /// <summary>
    /// CustomLeagueDto
    /// </summary>
    public class CLD
    {
        public int CLDId { get; set; }
        public string Name { get; set; }
        public string ParticipantId { get; set; }
        public RiotApi.Net.RestClient.Helpers.Enums.GameQueueType Queue { get; set; }
        public RiotApi.Net.RestClient.Helpers.Enums.Tier Tier { get; set; }

        public CLD()
        {

        }
        public CLD(RiotApi.Net.RestClient.Dto.League.LeagueDto leagueDto)
        {
            Name = leagueDto.Name;
            ParticipantId = leagueDto.ParticipantId;
            Queue = leagueDto.Queue;
            Tier = leagueDto.Tier;
        }
    }

    //CustomLeagueEntryDto : AWFULL name, but because of the way EntityFramework maps the ForeignKey in my SKL, on of my Foreign Key ended up being FK_Students_CustomLeagueEntryDtoes_DetailSoloQueueData_CustomLeagueEntryDtoId
    //And was too long for MySQL ...
    //it was not possible to rename the mapping of internal table CLED, so I had no completly rename it, or remap student as something like S ...
    //Decided renaming this class was more clear.

    //Edit : So I don't know why, but after renaming the table and regeneration my Migration script, EntityFramework still decided to name the table as before
    //Decided to still create the same FK, but this time MySQL executed the code, and it worked
    //I then had to rename the table back to CustomLeagueEntryDtoes because no table named CLED existed ...
    //Not sure why it acted like that, I'm leaving the comment in case I have the same problem another time.
    /// <summary>
    /// CustomLeagueEntryDto
    /// </summary>
    public class CLED
    {
        //Requiered to have a Key to be able to save the data in EntityFrameWork, so had to create a custom "copy"
        public string Division { get; set; }
        public bool IsFreshBlood { get; set; }
        public bool IsHotStreak { get; set; }
        public bool IsInactive { get; set; }
        public bool IsVeteran { get; set; }
        public int LeaguePoints { get; set; }
        public int Losses { get; set; }
        public CustomMiniSeries MiniSeries { get; set; }
        public string PlayerOrTeamId { get; set; }
        public string PlayerOrTeamName { get; set; }
        public int Wins { get; set; }
        [Key]
        public int CLEDId { get; set; }

        public CLED()
        {

        }
        public CLED(RiotApi.Net.RestClient.Dto.League.LeagueDto.LeagueEntryDto leagueEntryDto)
        {
            Division = leagueEntryDto.Division;
            IsFreshBlood = leagueEntryDto.IsFreshBlood;
            IsHotStreak = leagueEntryDto.IsHotStreak;
            IsInactive = leagueEntryDto.IsInactive;
            IsVeteran = leagueEntryDto.IsVeteran;
            LeaguePoints = leagueEntryDto.LeaguePoints;
            Losses = leagueEntryDto.Losses;
            PlayerOrTeamId = leagueEntryDto.PlayerOrTeamId;
            PlayerOrTeamName = leagueEntryDto.PlayerOrTeamName;
            Wins = leagueEntryDto.Wins;

            if (leagueEntryDto.MiniSeries == null)
                MiniSeries = null;
            else
                MiniSeries = new CustomMiniSeries(leagueEntryDto.MiniSeries);
        }
    }
    public class CustomMiniSeries
    {
        //Requiered to have a Key to be able to save the data in EntityFrameWork, so had to create a custom "copy"
        [Key]
        public int CustomMiniSeriesId { get; set; }
        public int Losses { get; set; }
        public string Progress { get; set; }
        public int Target { get; set; }
        public int Wins { get; set; }
        public CustomMiniSeries()
        {

        }
        public CustomMiniSeries(RiotApi.Net.RestClient.Dto.League.LeagueDto.LeagueEntryDto.MiniSeriesDto miniSeries)
        {
            Wins = miniSeries.Wins;
            Losses = miniSeries.Losses;
            Progress = miniSeries.Progress;
            Target = miniSeries.Target;
        }
    }
}
