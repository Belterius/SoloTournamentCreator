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
    public class TournamentTests
    {
        [TestMethod()]
        public void ArchiveTest()
        {
            Tournament tournament = new Tournament();
            tournament.Status = Tournament.TournamentStage.Started;

            tournament.Archive();

            Assert.AreEqual(tournament.Status, Tournament.TournamentStage.Completed);
        }

        [TestMethod()]
        public void StartFinalStageTest()
        {
            Tournament tournament = new Tournament();
            tournament.MyTournamentTree = new TournamentTree(2);
            var expectedLeft = tournament.MyTournamentTree.MyMainTournamentTree;
            var expectedRight = tournament.MyTournamentTree.MySecondaryTournamentTree;

            tournament.StartFinalStage();

            Assert.AreEqual(tournament.MyTournamentTree.MyMainTournamentTree.LeftContendant, expectedLeft);
            Assert.AreEqual(tournament.MyTournamentTree.MyMainTournamentTree.RightContendant, expectedRight);

        }

        [TestMethod()]
        public void StartTest()
        {
            Tournament tournament = new Tournament();

            tournament.Start();

            Assert.AreEqual(tournament.Status, Tournament.TournamentStage.Started);
        }

        [TestMethod()]
        public void DeregisterTest()
        {
            Tournament tournament = new Tournament();
            Student test = new Student("test");
            tournament.Register(test);

            tournament.Deregister(test);

            Assert.AreEqual(tournament.Participants.Contains(test), false);
        }

        [TestMethod()]
        public void RegisterTest()
        {
            Tournament tournament = new Tournament();
            Student test = new Student("test");

            tournament.Register(test);

            Assert.AreEqual(tournament.Participants.Contains(test), true);
        }
    }
}