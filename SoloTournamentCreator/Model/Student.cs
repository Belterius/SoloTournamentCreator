using SoloTournamentCreator.RiotToEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotApi.Net.RestClient.Dto.League;
using SoloTournamentCreator.Helper;
using System.ComponentModel.DataAnnotations;
using RiotApi.Net.RestClient.Dto.Summoner;

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
        private SummonerDto _SummonerData;
        private LeagueDto _SummonerSoloQueueData;
        private CustomLeagueEntryDtoes _DetailSoloQueueData;
        private ICollection<Tournament> _ParticipatingTournament;
        public virtual ICollection<Team> MyTeams { get; set; }
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

        public SummonerDto SummonerData
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

        public LeagueDto SummonerSoloQueueData
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
                if(SummonerSoloQueueData == null)
                {
                    return 80;
                }
                return GlobalConverters.RankingToPoint(SummonerSoloQueueData.Tier, DetailSoloQueueData.Division, DetailSoloQueueData.LeaguePoints);
            }
        }

        public ICollection<Tournament> ParticipatingTournament
        {
            get
            {
                return _ParticipatingTournament;
            }

            set
            {
                _ParticipatingTournament = value;
            }
        }

        public CustomLeagueEntryDtoes DetailSoloQueueData
        {
            get
            {
                return _DetailSoloQueueData;
            }

            set
            {
                _DetailSoloQueueData = value;
            }
        }

        private Student()
        {

        }
        public Student(string mail, string firstName, string lastName, string pseudo, int gradYear)
        {
            Mail = mail;
            FirstName = firstName;
            LastName = lastName;
            GraduationYear = gradYear;
            try
            {
                SummonerData = ApiRequest.GetSummonerData(pseudo.Replace(" ", string.Empty));
            }
            catch (Exception)
            {
                throw;
            }
            try
            {
                SummonerSoloQueueData = ApiRequest.GetSummonerSoloQueueRating(SummonerData.Id);
                DetailSoloQueueData = new CustomLeagueEntryDtoes(SummonerSoloQueueData.Entries.First());
            }
            catch (Exception)
            {
                SummonerSoloQueueData = null; //Unranked
                DetailSoloQueueData = null;
            }
        }

        public bool RefreshData()
        {
            try
            {
                SummonerData = ApiRequest.GetSummonerData(this.SummonerData.Id);
                SummonerSoloQueueData = ApiRequest.GetSummonerSoloQueueRating(SummonerData.Id);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Student))
                return false;
            if(this.SummonerData == null || ((Student)obj).SummonerData == null)
            {
                if(this.Pseudo == ((Student)obj).Pseudo)
                    return true;
            }else
            if (this.SummonerData.Id == ((Student)obj).SummonerData.Id)
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
