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
using RiotSharp.SummonerEndpoint;
using RiotSharp.LeagueEndpoint;

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
        private long _SummonerID;
        private Summoner _MySummonerData;
        private List<League> _MyLeagues;
        private ICollection<Tournament> _ParticipatingTournament;
        public virtual ICollection<Team> MyTeams { get; set; }


        public League MyBestLeague
        {
            get
            {
                List<League> importantLeague = MyLeagues.Where(x => x.Queue == RiotSharp.Queue.RankedFlexSR || x.Queue == RiotSharp.Queue.RankedSolo5x5 ).ToList();
                return importantLeague.OrderBy(x => x, new LeagueComparer()).FirstOrDefault(); 
            }
        }
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
                return _MySummonerData?.Name;
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
        
        public int EstimatedStrenght
        {
            get
            {
                if(MyLeagues?.Where(x => x.Queue == RiotSharp.Queue.RankedSolo5x5).Count() == 0 && MyLeagues?.Where(x => x.Queue == RiotSharp.Queue.RankedFlexSR).Count() == 0)
                {
                    return 80;
                }
                //return GlobalConverters.RankingToPoint(SummonerSoloQueueData.Tier, DetailSoloQueueData.Division, DetailSoloQueueData.LeaguePoints);
                return 0;
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

        public Summoner MySummonerData
        {
            get
            {
                return _MySummonerData;
            }

            set
            {
                _MySummonerData = value;
            }
        }

        public List<League> MyLeagues
        {
            get
            {
                return _MyLeagues;
            }

            set
            {
                _MyLeagues = value;
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
            RiotSharp.RiotApi riotSharpClient = RiotSharp.RiotApi.GetInstance("RGAPI-e7c92b92-6c4f-4747-96a9-ac312213da2a");
            if (testConfirm != "test")
            {
                throw new NotSupportedException();
            }
            MySummonerData = riotSharpClient.GetSummoner(RiotSharp.Region.euw, "belterius");
            SummonerID = MySummonerData.Id; //WARNING : EntityFrameWork WILL override the SummonerData.Id to its own, so we NEED to save the Riot SummonerID BEFORE saving into EntityFramework !
            MyLeagues = MySummonerData.GetLeagues();
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
                MySummonerData = MyRiotClient.Instance.riotSharpClient.GetSummoner(RiotSharp.Region.euw, pseudo.Replace(" ", string.Empty));
                SummonerID = MySummonerData.Id; //WARNING : EntityFrameWork WILL override the SummonerData.Id to its own, so we NEED to save the Riot SummonerID BEFORE saving into EntityFramework !
                MyLeagues = MySummonerData.GetLeagues();
            }
            catch (Exception)
            {
                throw;
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
                MyLeagues = MySummonerData.GetLeagues();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
            if (this.MySummonerData == null || ((Student)obj).MySummonerData == null)
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
