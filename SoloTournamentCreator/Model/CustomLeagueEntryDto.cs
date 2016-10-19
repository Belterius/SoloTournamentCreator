using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloTournamentCreator.Model
{
    [Serializable]
    public class CustomLeagueEntryDto
    {
        //Requiered to have a Key and be able to save the data in EntityFrameWork
        public string Division { get; set; }
        public bool IsFreshBlood { get; set; }
        public bool IsHotStreak { get; set; }
        public bool IsInactive { get; set; }
        public bool IsVeteran { get; set; }
        public int LeaguePoints { get; set; }
        public int Losses { get; set; }
        public RiotApi.Net.RestClient.Dto.League.LeagueDto.LeagueEntryDto.MiniSeriesDto MiniSeries { get; set; }
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
            MiniSeries = leagueEntryDto.MiniSeries;
            PlayerOrTeamId = leagueEntryDto.PlayerOrTeamId;
            PlayerOrTeamName = leagueEntryDto.PlayerOrTeamName;
            Wins = leagueEntryDto.Wins;
        }
    }
}
