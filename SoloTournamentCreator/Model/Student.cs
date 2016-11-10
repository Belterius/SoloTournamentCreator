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
using System.ComponentModel.DataAnnotations.Schema;

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
        private long _SummonerID;
        private CLD _SummonerSoloQueueData;
        private CLED _DetailSoloQueueData;
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
                return _SummonerData?.Name;
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

        public CLD SummonerSoloQueueData
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

        public CLED DetailSoloQueueData
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

        public long SummonerID
        {
            get
            {
                return _SummonerID;
            }

            set
            {
                _SummonerID = value;
            }
        }

        private Student()
        {

        }
        /// <summary>
        /// Should ONLY, EVER be used for testing purpose
        /// </summary>
        /// <param name="testConfirm">MUST be set to "test" for the function to work</param>
        public Student(string testConfirm)
        {
            if(testConfirm != "test")
            {
                throw new NotSupportedException();
            }
            SummonerData = new SummonerDto();
        }
        /// <summary>
        /// <para/>Create a new Student
        /// <para/>Will use the Pseudo given to request from the Riot API its Summoner Data, if that fails (pseudo doesn't exist), the constructor will fail.
        /// <para/>It will then use the SummonerID to request the Summoner SoloQueue Data, if it fails (the player is Unranked) it will set the SoloQueueData and DetailData to null.
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="pseudo"></param>
        /// <param name="gradYear">The year at which the student is expected to graduate</param>
        public Student(string mail, string firstName, string lastName, string pseudo, int gradYear)
        {
            Mail = mail;
            FirstName = firstName;
            LastName = lastName;
            GraduationYear = gradYear;
            try
            {
                SummonerData = ApiRequest.GetSummonerData(pseudo.Replace(" ", string.Empty));
                SummonerID = SummonerData.Id; //WARNING : EntityFrameWork WILL override the SummonerData.Id to its own, so we NEED to save the Riot SummonerID BEFORE saving into EntityFramework !
            }
            catch (Exception)
            {
                throw;
            }
            try
            {
                LeagueDto MySummonerSoloQueueData = ApiRequest.GetSummonerSoloQueueRating(SummonerID);
                SummonerSoloQueueData = new CLD(MySummonerSoloQueueData);
                DetailSoloQueueData = new CLED(MySummonerSoloQueueData.Entries.First());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                SummonerSoloQueueData = null; //Unranked
                DetailSoloQueueData = null;
            }
        }

        /// <summary>
        /// Use the SummonerID to update the SoloQueueData and DetailData
        /// </summary>
        /// <returns></returns>
        public bool RefreshData()
        {
            try
            {
                LeagueDto MySummonerSoloQueueData = ApiRequest.GetSummonerSoloQueueRating(SummonerID);
                SummonerSoloQueueData = new CLD(MySummonerSoloQueueData);
                DetailSoloQueueData = new CLED(MySummonerSoloQueueData.Entries.First());
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                SummonerSoloQueueData = null; //Unranked
                DetailSoloQueueData = null;
                return false;
            }
        }

        /// <summary>
        /// <para/>Compare the SummonerID of each entity, return true if they are equal.
        /// </summary>
        /// <param name="obj">another Student to compare to</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Student))
            {
                return false;
            }
            if (this.SummonerData == null || ((Student)obj).SummonerData == null)
            {
                return false;
            }
            if (this.SummonerID == ((Student)obj).SummonerID)
            {
                return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            var testc =  base.GetHashCode() +  this.SummonerID.GetHashCode() ;
            return base.GetHashCode() +  this.SummonerID.GetHashCode();
        }
    }
}
