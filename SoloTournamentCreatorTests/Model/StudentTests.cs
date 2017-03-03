using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoloTournamentCreator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotSharp;

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
            std1.MySummonerData.Name = "Name1";
            std2.MySummonerData.Name = "Name2";
            std2.MySummonerData.Id = std1.MySummonerData.Id;

            Assert.AreEqual(std1, std2);
        }

        [TestMethod()]
        public void EqualsFalseTest()
        {
            Student std1 = new Student("test");
            Student std2 = new Student("test");
            std1.MySummonerData.Name = "Name1";
            std2.StudentId = std1.StudentId + 1;
            std2.SummonerID = std1.SummonerID + 1;
            std2.MySummonerData.Name = "Name1";
            std2.MySummonerData.Id = std1.MySummonerData.Id + 1;

            Assert.AreNotEqual(std1, std2);
        }
    }
}