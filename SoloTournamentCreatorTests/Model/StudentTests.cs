using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoloTournamentCreator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloTournamentCreator.Model.Tests
{
    [TestClass()]
    public class StudentTests
    {
        [TestMethod()]
        public void RefreshDataTest()
        {
            //Use Riot Api, not appropriate to test, because it will use the Key API Data Cap, and would require to hqve the Key in clear in the code
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void EqualsTrueTest()
        {
            Student std1 = new Student("test");
            Student std2 = new Student("test");
            std1.SummonerData.Name = "Name1";
            std2.SummonerData.Name = "Name2";
            std2.SummonerData.Id = std1.SummonerData.Id;

            Assert.AreEqual(std1, std2);
        }

        [TestMethod()]
        public void EqualsFalseTest()
        {
            Student std1 = new Student("test");
            Student std2 = new Student("test");
            std1.SummonerData.Name = "Name1";
            std2.SummonerData.Name = "Name1";
            std2.SummonerData.Id = std1.SummonerData.Id + 1;

            Assert.AreNotEqual(std1, std2);
        }
    }
}