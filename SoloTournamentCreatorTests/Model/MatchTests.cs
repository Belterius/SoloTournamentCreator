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
    public class MatchTests
    {
        [TestMethod()]
        public void MatchTest()
        {
            bool isMainMatch = true;
            int depth = 0;
            Match match = new Match(depth, isMainMatch);

            Assert.AreEqual(match.Depth, depth);
            Assert.AreEqual(match.IsMainMatch, isMainMatch);
            Assert.AreEqual(match.LoserScore, 0);
            Assert.AreEqual(match.WinnerScore, 0);
        }

        [TestMethod()]
        public void DeclareWinnerTest()
        {
            bool isMainMatch = true;
            int depth = 0;
            Match match = new Match(depth, isMainMatch);
            Team winner = new Team();

            match.DeclareWinner(winner);

            Assert.AreEqual(match.Winner, winner);
        }

        [TestMethod()]
        public void SetLastRoundAutoWinnerTest()
        {
            Match Final = new Match(0, true);
            Match HalfFinal1 = new Match(1, true);
            Match HalfFinal2 = new Match(1, true);
            Match SemiFinal1 = new Match(2, true);
            Match SemiFinal2 = new Match(2, true);
            Match SemiFinal3 = new Match(2, true);
            Match SemiFinal4 = new Match(2, true);
            Match HeighthFinal1 = new Match(3, true);
            Match HeighthFinal2 = new Match(3, true);
            Match HeighthFinal3 = new Match(3, true);
            Match HeighthFinal4 = new Match(3, true);
            Match HeighthFinal5 = new Match(3, true);
            Match HeighthFinal6 = new Match(3, true);
            Match HeighthFinal7 = new Match(3, true);
            Match HeighthFinal8 = new Match(3, true);
            Team ZeroFreeWin = new Team(5, "ZeroFreeWin");
            Team ZeroFreeWinBis = new Team(5, "ZeroFreeWinBis");
            Team OneFreeWin = new Team(5, "OneFreeWin");
            Team TwoFreeWin = new Team(5, "TwoFreeWin");

            HeighthFinal1.Winner = OneFreeWin;
            HeighthFinal3.Winner = ZeroFreeWin;
            HeighthFinal4.Winner = ZeroFreeWinBis;
            HeighthFinal5.Winner = TwoFreeWin;

            SemiFinal1.LeftContendant = HeighthFinal1;
            SemiFinal1.RightContendant = HeighthFinal2;
            SemiFinal2.LeftContendant = HeighthFinal3;
            SemiFinal2.RightContendant = HeighthFinal4;
            SemiFinal3.LeftContendant = HeighthFinal5;
            SemiFinal3.RightContendant = HeighthFinal6;
            SemiFinal4.LeftContendant = HeighthFinal7;
            SemiFinal4.RightContendant = HeighthFinal8;
            HalfFinal1.LeftContendant = SemiFinal1;
            HalfFinal1.RightContendant = SemiFinal2;
            HalfFinal2.LeftContendant = SemiFinal3;
            HalfFinal2.RightContendant = SemiFinal4;
            Final.LeftContendant = HalfFinal1;
            Final.RightContendant = HalfFinal2;

            Final.SetLastRoundAutoWinner();

            Assert.AreEqual(Final.Winner, null);
            Assert.AreEqual(HalfFinal1.Winner, null);
            Assert.AreEqual(HalfFinal2.Winner, null);
            Assert.AreEqual(SemiFinal1.Winner, OneFreeWin);
            Assert.AreEqual(SemiFinal2.Winner, null);
            Assert.AreEqual(SemiFinal3.Winner, TwoFreeWin);
            Assert.AreEqual(SemiFinal4.Winner, null);


        }

        [TestMethod()]
        public void SetAutoWinnerChainTest()
        {
            Match Final = new Match(0, true);
            Match HalfFinal1 = new Match(1, true);
            Match HalfFinal2 = new Match(1, true);
            Match SemiFinal1 = new Match(2, true);
            Match SemiFinal2 = new Match(2, true);
            Match SemiFinal3 = new Match(2, true);
            Match SemiFinal4 = new Match(2, true);
            Match HeighthFinal1 = new Match(3, true);
            Match HeighthFinal2 = new Match(3, true);
            Match HeighthFinal3 = new Match(3, true);
            Match HeighthFinal4 = new Match(3, true);
            Match HeighthFinal5 = new Match(3, true);
            Match HeighthFinal6 = new Match(3, true);
            Match HeighthFinal7 = new Match(3, true);
            Match HeighthFinal8 = new Match(3, true);
            Team ZeroFreeWin = new Team(5, "ZeroFreeWin");
            Team ZeroFreeWinBis = new Team(5, "ZeroFreeWinBis");
            Team OneFreeWin = new Team(5, "OneFreeWin");
            Team TwoFreeWin = new Team(5, "TwoFreeWin");

            HeighthFinal1.Winner = OneFreeWin;
            HeighthFinal3.Winner = ZeroFreeWin;
            HeighthFinal4.Winner = ZeroFreeWinBis;
            HeighthFinal5.Winner = TwoFreeWin;

            SemiFinal1.LeftContendant = HeighthFinal1;
            SemiFinal1.RightContendant = HeighthFinal2;
            SemiFinal2.LeftContendant = HeighthFinal3;
            SemiFinal2.RightContendant = HeighthFinal4;
            SemiFinal3.LeftContendant = HeighthFinal5;
            SemiFinal3.RightContendant = HeighthFinal6;
            SemiFinal4.LeftContendant = HeighthFinal7;
            SemiFinal4.RightContendant = HeighthFinal8;
            HalfFinal1.LeftContendant = SemiFinal1;
            HalfFinal1.RightContendant = SemiFinal2;
            HalfFinal2.LeftContendant = SemiFinal3;
            HalfFinal2.RightContendant = SemiFinal4;
            Final.LeftContendant = HalfFinal1;
            Final.RightContendant = HalfFinal2;

            PrivateObject obj = new PrivateObject(Final);
            var retVal = obj.Invoke("SetAutoWinnerChain");

            Assert.AreEqual(Final.Winner, null);
            Assert.AreEqual(HalfFinal1.Winner, null);
            Assert.AreEqual(HalfFinal2.Winner, TwoFreeWin);
            Assert.AreEqual(SemiFinal1.Winner, OneFreeWin);
            Assert.AreEqual(SemiFinal2.Winner, null);
            Assert.AreEqual(SemiFinal3.Winner, TwoFreeWin);
            Assert.AreEqual(SemiFinal4.Winner, null);
        }
    }
}