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
    public class TeamTests
    {
        [TestMethod()]
        public void TeamTest()
        {
            string teamName = "MyTeamName";
            int nbPlayer = 5;
            Team team;
            
            team = new Team(nbPlayer, teamName);

            Assert.AreEqual(team.NbPlayerMax, nbPlayer);
            Assert.AreEqual(team.TeamName, teamName);
        }
        [TestMethod()]
        public void AddMemberBasicTest()
        {
            Team team = new Team(3, "teamName");
            Student std = new Student("test");
            std.FirstName = "std1";
            Student std2 = new Student("test");
            std2.FirstName = "std2";

            team.AddMember(std);

            Assert.AreEqual(team.TeamMember.Contains(std), true);
            Assert.AreEqual(team.TeamMember.Contains(std2), false);
        }
        [TestMethod()]
        public void AddMemberOnFullTest()
        {
            Team team = new Team(3, "teamName");
            Student std = new Student("test");
            std.FirstName = "std1";
            Student std2 = new Student("test");
            std2.FirstName = "std2";
            Student std3 = new Student("test");
            std3.FirstName = "std3";
            Student std4 = new Student("test");
            std4.FirstName = "std4";
            team.AddMember(std);
            team.AddMember(std2);
            team.AddMember(std3);

            Assert.AreEqual(team.TeamMember.Contains(std), true);
            Assert.AreEqual(team.TeamMember.Contains(std2), true);
            Assert.AreEqual(team.TeamMember.Contains(std3), true);
            try
            {
                team.AddMember(std4);
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {

                Assert.AreEqual(team.TeamMember.Contains(std4), false);
            }

        }
        [TestMethod()]
        public void AddMemberAlreadyPresentTest()
        {
            Team team = new Team(3, "teamName");
            Student std = new Student("test");
            team.AddMember(std);

            Assert.AreEqual(team.AddMember(std), false);
        }

        [TestMethod()]
        public void RemoveMemberTest()
        {
            Team team = new Team(3, "teamName");
            Student std = new Student("test");
            team.AddMember(std);
            std.FirstName = "std1";
            Assert.AreEqual(team.AddMember(std), false);

            team.RemoveMember(std);

            Assert.AreEqual(team.TeamMember.Contains(std), false);
        }

        [TestMethod()]
        public void RenameTest()
        {
            string teamName = "MyTeamName";
            string newName = "MyNewName";
            int nbPlayer = 5;
            Team team;
            team = new Team(nbPlayer, teamName);
            Assert.AreEqual(team.TeamName, teamName);

            team.Rename(newName);

            Assert.AreEqual(team.TeamName, newName);
        }
    }
}