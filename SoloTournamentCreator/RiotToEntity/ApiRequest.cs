using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloTournamentCreator.RiotToEntity
{
    public class ApiRequest
    {
        public static RiotSharp.TournamentEndpoint.Tournament CreateTournamentAPI(string url, string tournamentName)
        {
            var tournamentApi = RiotSharp.TournamentRiotApi.GetInstance(Properties.Settings.Default.RiotTournamentApiKey);
            var providerw = tournamentApi.CreateProvider(RiotSharp.Region.euw, url);
            return tournamentApi.CreateTournament(providerw.Id, tournamentName);
            
        }
        /*
        public static string CreateTournamentCode(RiotSharp.TournamentEndpoint.Tournament tournament, SoloTournamentCreator.Model.Tournament myTournament)
        {
            int teamSize = 5;
            List<long> allowedSummonerIds = myTournament.Participants.Select(x => x.SummonerID).ToList();
            return tournament.CreateTournamentCode(teamSize, allowedSummonerIds, RiotSharp.TournamentEndpoint.TournamentSpectatorType.All, RiotSharp.TournamentEndpoint.TournamentPickType.TournamentDraft, RiotSharp.TournamentEndpoint.TournamentMapType.SummonersRift, string.Empty);
        }
        */
        public static bool ApiKeyIsValid(string key)
        {
            RiotSharp.RiotApi riotSharpClientTest = RiotSharp.RiotApi.GetInstance(key);
            try
            {
                var mySummoner = riotSharpClientTest.GetSummoner(RiotSharp.Region.euw, "Belterius");
                var info = riotSharpClientTest.GetSummoner(RiotSharp.Region.euw, mySummoner.Id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static RiotSharp.LeagueEndpoint.League GetSampleChallenger()
        {
            try
            {
                return MyRiotClient.Instance.riotSharpClient.GetChallengerLeague(RiotSharp.Region.euw, RiotSharp.Queue.RankedSolo5x5);

            }
            catch (Exception)
            {
                throw;
            }
        }
        //public static RiotApi.Net.RestClient.Dto.Summoner.SummonerDto GetSummonerData(String pseudo)
        //{
        //    try
        //    {
        //        return MyRiotClient.Instance.riotClient.Summoner.GetSummonersByName(RiotApiConfig.Regions.EUW, pseudo.ToLower())[pseudo.ToLower()];
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //public static RiotApi.Net.RestClient.Dto.Summoner.SummonerDto GetSummonerData(long summonerID)
        //{
        //    try
        //    {
        //        return MyRiotClient.Instance.riotClient.Summoner.GetSummonersById(RiotApiConfig.Regions.EUW, summonerID.ToString())[summonerID.ToString()];
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //public static RiotApi.Net.RestClient.Dto.League.LeagueDto GetSummonerSoloQueueRating(long summonerID)
        //{
        //    try
        //    {
        //        return MyRiotClient.Instance.riotClient.League.GetSummonerLeagueEntriesByIds(RiotApiConfig.Regions.EUW, summonerID)[summonerID.ToString()].Where(x => x.Queue == RiotApi.Net.RestClient.Helpers.Enums.GameQueueType.RANKED_SOLO_5x5).First();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //public static RiotApi.Net.RestClient.Dto.League.LeagueDto GetSummoner5v5TeamRating(long summonerID)
        //{
        //    try
        //    {
        //        return MyRiotClient.Instance.riotClient.League.GetSummonerLeagueEntriesByIds(RiotApiConfig.Regions.EUW, summonerID)[summonerID.ToString()].Where(x => x.Queue == RiotApi.Net.RestClient.Helpers.Enums.GameQueueType.RANKED_TEAM_5x5).First();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //public static RiotApi.Net.RestClient.Dto.League.LeagueDto GetSummoner3v3TeamRating(long summonerID)
        //{
        //    try
        //    {
        //        return MyRiotClient.Instance.riotClient.League.GetSummonerLeagueEntriesByIds(RiotApiConfig.Regions.EUW, summonerID)[summonerID.ToString()].Where(x => x.Queue == RiotApi.Net.RestClient.Helpers.Enums.GameQueueType.RANKED_TEAM_3x3).First();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
    }
}
