using SoloTournamentCreator.RiotToEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotApi.Net.RestClient.Dto.League;
using SoloTournamentCreator.Helper;
using System.ComponentModel.DataAnnotations;

namespace SoloTournamentCreator.Model
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        private string _Mail;
        private string _FirstName;
        private string _LastName;
        private int _GraduationYear;
        private RiotApi.Net.RestClient.Dto.Summoner.SummonerDto _SummonerData;
        private LeagueDto _SummonerSoloQueueData;

        public string Mail
        {
            get
            {
                return _Mail;
            }

            set
            {
                _Mail = value;
            }
        }

        public string FirstName
        {
            get
            {
                return _FirstName;
            }

            set
            {
                _FirstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return _LastName;
            }

            set
            {
                _LastName = value;
            }
        }
        
        public string Pseudo
        {
            get
            {
                return _SummonerData.Name;
            }
        }

        public int GraduationYear
        {
            get
            {
                return _GraduationYear;
            }

            set
            {
                _GraduationYear = value;
            }
        }

        public RiotApi.Net.RestClient.Dto.Summoner.SummonerDto SummonerData
        {
            get
            {
                return _SummonerData;
            }

            set
            {
                _SummonerData = value;
            }
        }

        public LeagueDto SummonerLeagueData
        {
            get
            {
                return _SummonerSoloQueueData;
            }

            set
            {
                _SummonerSoloQueueData = value;
            }
        }
        public int EstimatedStrenght
        {
            get
            {
                if(SummonerLeagueData == null)
                {
                    return 8;
                }
                return Converters.RankingToPoint(SummonerLeagueData.Tier, SummonerLeagueData.Entries.First().Division);
            }
        }

        public Student(string mail, string firstName, string lastName, string pseudo, int gradYear)
        {
            Mail = mail;
            FirstName = firstName;
            LastName = lastName;
            GraduationYear = gradYear;
            try
            {
                SummonerData = ApiRequest.GetSummonerData(pseudo);
            }
            catch (Exception)
            {
                throw;
            }
            try
            {
                SummonerLeagueData = ApiRequest.GetSummonerSoloQueueRating(SummonerData.Id);
            }
            catch (Exception)
            {
                SummonerLeagueData = null; //Unranked
            }
        }

        public bool RefreshData()
        {
            try
            {
                SummonerData = ApiRequest.GetSummonerData(this.SummonerData.Id);
                SummonerLeagueData = ApiRequest.GetSummonerSoloQueueRating(SummonerData.Id);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
