﻿using SoloTournamentCreator.RiotToEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoloTournamentCreator.Helper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RiotSharp.SummonerEndpoint;
using RiotSharp.LeagueEndpoint;
using RiotSharp.LeagueEndpoint.Enums;

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
        private long _SavedSummonerID;
        private Summoner _MySummonerData;
        private List<CSL> _MyLeagues;
        private Tier _BestRankPreviousSeason;
        private ICollection<Tournament> _ParticipatingTournament;
        public virtual ICollection<Team> MyTeams { get; set; }


        public CSL MyBestLeague
        {
            get
            {
                List<CSL> importantLeague = MyLeagues.Where(x => x.Queue == RiotSharp.Misc.Queue.RankedFlexSR || x.Queue == RiotSharp.Misc.Queue.RankedSolo5x5 ).ToList();
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
                if(MyLeagues == null)
                {
                    return GlobalConverters.RankingToPoint(BestRankPreviousSeason, "V", 50);
                }
                if(MyLeagues?.Where(x => x.Queue == RiotSharp.Misc.Queue.RankedSolo5x5).Count() == 0 && MyLeagues?.Where(x => x.Queue == RiotSharp.Misc.Queue.RankedFlexSR).Count() == 0)
                {
                    return GlobalConverters.RankingToPoint(BestRankPreviousSeason, "V", 50);
                }
                return GlobalConverters.RankingToPoint(MyBestLeague.Tier, MyBestLeague.Entries.Where(x => x.PlayerOrTeamId == SavedSummonerID.ToString()).Single().Division, MyBestLeague.Entries.Where(x => x.PlayerOrTeamId == SavedSummonerID.ToString()).Single().LeaguePoints);
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

        public List<CSL> MyLeagues
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

        public Tier BestRankPreviousSeason
        {
            get
            {
                return _BestRankPreviousSeason;
            }

            set
            {
                _BestRankPreviousSeason = value;
            }
        }

        public long SavedSummonerID
        {
            get
            {
                return _SavedSummonerID;
            }

            set
            {
                _SavedSummonerID = value;
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
            RiotSharp.RiotApi riotSharpClient = RiotSharp.RiotApi.GetInstance("",200,500);
            if (testConfirm != "test")
            {
                throw new NotSupportedException();
            }
            MySummonerData = riotSharpClient.GetSummonerByName(RiotSharp.Misc.Region.euw, "belterius");
            SummonerID = MySummonerData.Id; //WARNING : EntityFrameWork WILL override the SummonerData.Id to its own, so we NEED to save the Riot SummonerID BEFORE saving into EntityFramework !
            //var AllMyLeagues = MySummonerData.GetLeagues();            
            var AllMyLeagues = riotSharpClient.GetLeagues(RiotSharp.Misc.Region.euw, SummonerID);
            foreach (var league in AllMyLeagues)
            {
                MyLeagues.Add(new CSL(league));
            }
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
            //SET FOREIGN_KEY_CHECKS=0;
            Mail = mail;
            FirstName = firstName;
            LastName = lastName;
            GraduationYear = gradYear;
            try
            {
                MySummonerData = MyRiotClient.Instance.riotSharpClient.GetSummonerByName(RiotSharp.Misc.Region.euw, pseudo.Replace(" ", string.Empty));
                SummonerID = MySummonerData.Id; //WARNING : EntityFrameWork WILL override the SummonerData.Id to its own, so we NEED to save the Riot SummonerID BEFORE saving into EntityFramework !
                SavedSummonerID = MySummonerData.Id;
                //MySummonerData.GetEntireLeagues();
                //var AllMyLeagues = MySummonerData.GetLeagues();
                var AllMyLeagues = MyRiotClient.Instance.riotSharpClient.GetLeagues(RiotSharp.Misc.Region.euw, SavedSummonerID);
                MyLeagues = new List<CSL>();
                foreach (var league in AllMyLeagues)
                {
                    MyLeagues.Add(new CSL(league));
                }
            }
            catch (RiotSharp.RiotSharpException ex)
            {
                if(ex.Message == "404, Resource not found")
                {
                    //Unranked
                    try
                    {
                        RiotSharp.GameEndpoint.Game lastGame = MySummonerData.GetRecentGames()[0];
                        var gameData = MyRiotClient.Instance.riotSharpClient.GetMatch(RiotSharp.Misc.Region.euw, lastGame.GameId);
                        var playerData = gameData.Participants.Where(x => x.ChampionId == lastGame.ChampionId && x.TeamId == lastGame.TeamId).Single();
                        BestRankPreviousSeason = playerData.HighestAchievedSeasonTier;
                    }
                    catch (Exception e)
                    {
                        if (e.Message == "Error setting value to 'GameMode' on 'RiotSharp.GameEndpoint.Game'." || e.Message == "404, Resource not found")
                        {
                            Console.WriteLine($"****************************************************************************************************");
                            Console.WriteLine($"ERROR CREATING {firstName} {lastName} {pseudo}");
                            Console.WriteLine(e);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
            try
            {
                

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
                MySummonerData.Id = SavedSummonerID;
                MyLeagues = new List<CSL>();
                //var AllMyLeagues = MySummonerData.GetLeagues();
                var AllMyLeagues = MyRiotClient.Instance.riotSharpClient.GetLeagues(RiotSharp.Misc.Region.euw, MySummonerData.Id);
                foreach (var league in AllMyLeagues)
                {
                    MyLeagues.Add(new CSL(league));
                }
                return true;
            }
            catch (RiotSharp.RiotSharpException ex)
            {
                if (ex.Message == "404, Resource not found")
                {
                    //Unranked
                    try
                    {
                        RiotSharp.GameEndpoint.Game lastGame = MySummonerData.GetRecentGames()[0];
                        var gameData = MyRiotClient.Instance.riotSharpClient.GetMatch(RiotSharp.Misc.Region.euw, lastGame.GameId);
                        var playerData = gameData.Participants.Where(x => x.ChampionId == lastGame.ChampionId && x.TeamId == lastGame.TeamId).Single();
                        BestRankPreviousSeason = playerData.HighestAchievedSeasonTier;
                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine(ex);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            finally
            {
                MySummonerData.Id = SummonerID;
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
