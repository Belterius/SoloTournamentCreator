﻿using RiotApi.Net.RestClient;
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
    }
}
