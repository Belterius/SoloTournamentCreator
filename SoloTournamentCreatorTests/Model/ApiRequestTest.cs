using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoloTournamentCreator.RiotToEntity;
using RiotApi.Net.RestClient.Configuration;

namespace SoloTournamentCreatorTests.Model
{
    [TestClass]
    public class ApiRequestTest
    {
        [TestMethod]
        public void TestGetSummonerByName()
        {
            string name = "belterius";
            try
            {
                var test = MyRiotClient.Instance.riotClient.Summoner.GetSummonersByName(RiotApiConfig.Regions.EUW, name)[name];
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }

        }
    }
}
