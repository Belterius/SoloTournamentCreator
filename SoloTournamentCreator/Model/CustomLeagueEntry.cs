using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloTournamentCreator.Model
{
    public class CustomLeagueEntryDto
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
        public int CustomLeagueEntryDtoId { get; set; }

        public CustomLeagueEntryDto()
        {

        }
        public CustomLeagueEntryDto(RiotApi.Net.RestClient.Dto.League.LeagueDto.LeagueEntryDto leagueEntryDto)
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
