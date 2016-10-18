using RiotApi.Net.RestClient;
using RiotApi.Net.RestClient.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloTournamentCreator.RiotToEntity
{
    public class ApiRequest
    {
        public static bool ApiKeyIsValid(string key)
        {
            IRiotClient riotClientTest = new RiotClient(key);
            try
            {
                var mySummoner = riotClientTest.Summoner.GetSummonersByName(RiotApiConfig.Regions.EUW, "Belterius");
                var info = riotClientTest.League.GetSummonerLeagueEntriesByIds(RiotApiConfig.Regions.EUW, mySummoner["belterius"].Id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static IEnumerable<RiotApi.Net.RestClient.Dto.League.LeagueDto.LeagueEntryDto> GetSampleChallenger()
        {
            try
            {
                return MyRiotClient.Instance.riotClient.League.GetChallengerTierLeagues(RiotApiConfig.Regions.EUW, RiotApi.Net.RestClient.Helpers.Enums.GameQueueType.RANKED_SOLO_5x5).Entries;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static RiotApi.Net.RestClient.Dto.Summoner.SummonerDto GetSummonerData(String pseudo)
        {
            try
            {
                return MyRiotClient.Instance.riotClient.Summoner.GetSummonersByName(RiotApiConfig.Regions.EUW, pseudo.ToLower())[pseudo.ToLower()];
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static RiotApi.Net.RestClient.Dto.Summoner.SummonerDto GetSummonerData(long summonerID)
        {
            try
            {
                return MyRiotClient.Instance.riotClient.Summoner.GetSummonersById(RiotApiConfig.Regions.EUW, summonerID.ToString())[summonerID.ToString()];
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static RiotApi.Net.RestClient.Dto.League.LeagueDto GetSummonerSoloQueueRating(long summonerID)
        {
            try
            {
                return MyRiotClient.Instance.riotClient.League.GetSummonerLeagueEntriesByIds(RiotApiConfig.Regions.EUW, summonerID)[summonerID.ToString()].Where(x => x.Queue == RiotApi.Net.RestClient.Helpers.Enums.GameQueueType.RANKED_SOLO_5x5).First();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static RiotApi.Net.RestClient.Dto.League.LeagueDto GetSummoner5v5TeamRating(long summonerID)
        {
            try
            {
                return MyRiotClient.Instance.riotClient.League.GetSummonerLeagueEntriesByIds(RiotApiConfig.Regions.EUW, summonerID)[summonerID.ToString()].Where(x => x.Queue == RiotApi.Net.RestClient.Helpers.Enums.GameQueueType.RANKED_TEAM_5x5).First();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static RiotApi.Net.RestClient.Dto.League.LeagueDto GetSummoner3v3TeamRating(long summonerID)
        {
            try
            {
                return MyRiotClient.Instance.riotClient.League.GetSummonerLeagueEntriesByIds(RiotApiConfig.Regions.EUW, summonerID)[summonerID.ToString()].Where(x => x.Queue == RiotApi.Net.RestClient.Helpers.Enums.GameQueueType.RANKED_TEAM_3x3).First();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
