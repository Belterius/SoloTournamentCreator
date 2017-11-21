namespace SoloTournamentCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Matches",
                c => new
                    {
                        MatchId = c.Int(nullable: false, identity: true),
                        LeftContendantId = c.Int(),
                        RightContendantId = c.Int(),
                        WinNext = c.Boolean(),
                        WinnerScore = c.Int(nullable: false),
                        LoserScore = c.Int(nullable: false),
                        Depth = c.Int(nullable: false),
                        IsMainMatch = c.Boolean(nullable: false),
                        Winner_TeamId = c.Int(),
                    })
                .PrimaryKey(t => t.MatchId)
                .ForeignKey("dbo.Matches", t => t.LeftContendantId)
                .ForeignKey("dbo.Matches", t => t.RightContendantId)
                .ForeignKey("dbo.Teams", t => t.Winner_TeamId)
                .Index(t => t.LeftContendantId)
                .Index(t => t.RightContendantId)
                .Index(t => t.Winner_TeamId);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamId = c.Int(nullable: false, identity: true),
                        TeamName = c.String(unicode: false),
                        NbPlayerMax = c.Int(nullable: false),
                        Tournament_TournamentId = c.Int(),
                    })
                .PrimaryKey(t => t.TeamId)
                .ForeignKey("dbo.Tournaments", t => t.Tournament_TournamentId)
                .Index(t => t.Tournament_TournamentId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentId = c.Int(nullable: false, identity: true),
                        Mail = c.String(unicode: false),
                        FirstName = c.String(unicode: false),
                        LastName = c.String(unicode: false),
                        GraduationYear = c.Int(nullable: false),
                        SummonerID = c.Long(nullable: false),
                        BestRankPreviousSeason = c.Int(nullable: false),
                        SavedSummonerID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.StudentId)
                .ForeignKey("dbo.Summoners", t => t.SummonerID, cascadeDelete: true)
                .Index(t => t.SummonerID);
            
            CreateTable(
                "dbo.CSLs",
                c => new
                    {
                        CSLId = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        ParticipantId = c.String(unicode: false),
                        Queue = c.String(unicode: false),
                        Tier = c.Int(nullable: false),
                        Student_StudentId = c.Int(),
                    })
                .PrimaryKey(t => t.CSLId)
                .ForeignKey("dbo.Students", t => t.Student_StudentId)
                .Index(t => t.Student_StudentId);
            
            CreateTable(
                "dbo.CSLEs",
                c => new
                    {
                        CSLEId = c.Int(nullable: false, identity: true),
                        Division = c.String(unicode: false),
                        IsFreshBlood = c.Boolean(nullable: false),
                        IsHotStreak = c.Boolean(nullable: false),
                        IsInactive = c.Boolean(nullable: false),
                        IsVeteran = c.Boolean(nullable: false),
                        LeaguePoints = c.Int(nullable: false),
                        Losses = c.Int(nullable: false),
                        PlayerOrTeamId = c.String(unicode: false),
                        PlayerOrTeamName = c.String(unicode: false),
                        Wins = c.Int(nullable: false),
                        MiniSeries_CustomMiniSeriesId = c.Int(),
                        CSL_CSLId = c.Int(),
                    })
                .PrimaryKey(t => t.CSLEId)
                .ForeignKey("dbo.CustomMiniSeries", t => t.MiniSeries_CustomMiniSeriesId)
                .ForeignKey("dbo.CSLs", t => t.CSL_CSLId)
                .Index(t => t.MiniSeries_CustomMiniSeriesId)
                .Index(t => t.CSL_CSLId);
            
            CreateTable(
                "dbo.CustomMiniSeries",
                c => new
                    {
                        CustomMiniSeriesId = c.Int(nullable: false, identity: true),
                        Losses = c.Int(nullable: false),
                        Target = c.Int(nullable: false),
                        Wins = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CustomMiniSeriesId);
            
            CreateTable(
                "dbo.Summoners",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProfileIconId = c.Int(nullable: false),
                        RevisionDate = c.DateTime(nullable: false, precision: 0),
                        Level = c.Long(nullable: false),
                        Region = c.Int(nullable: false),
                        AccountId = c.Long(nullable: false),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tournaments",
                c => new
                    {
                        TournamentId = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Name = c.String(unicode: false),
                        NbTeam = c.Int(nullable: false),
                        HasLoserBracket = c.Boolean(nullable: false),
                        MyTournamentTree_TournamentTreeId = c.Int(),
                    })
                .PrimaryKey(t => t.TournamentId)
                .ForeignKey("dbo.TournamentTrees", t => t.MyTournamentTree_TournamentTreeId)
                .Index(t => t.MyTournamentTree_TournamentTreeId);
            
            CreateTable(
                "dbo.TournamentTrees",
                c => new
                    {
                        TournamentTreeId = c.Int(nullable: false, identity: true),
                        MaxDepth = c.Double(nullable: false),
                        HasLoserBracket = c.Boolean(nullable: false),
                        MyMainTournamentTree_MatchId = c.Int(),
                        MySecondaryTournamentTree_MatchId = c.Int(),
                    })
                .PrimaryKey(t => t.TournamentTreeId)
                .ForeignKey("dbo.Matches", t => t.MyMainTournamentTree_MatchId)
                .ForeignKey("dbo.Matches", t => t.MySecondaryTournamentTree_MatchId)
                .Index(t => t.MyMainTournamentTree_MatchId)
                .Index(t => t.MySecondaryTournamentTree_MatchId);
            
            CreateTable(
                "dbo.StudentTeams",
                c => new
                    {
                        Student_StudentId = c.Int(nullable: false),
                        Team_TeamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_StudentId, t.Team_TeamId })
                .ForeignKey("dbo.Students", t => t.Student_StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Teams", t => t.Team_TeamId, cascadeDelete: true)
                .Index(t => t.Student_StudentId)
                .Index(t => t.Team_TeamId);
            
            CreateTable(
                "dbo.TournamentStudents",
                c => new
                    {
                        Tournament_TournamentId = c.Int(nullable: false),
                        Student_StudentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tournament_TournamentId, t.Student_StudentId })
                .ForeignKey("dbo.Tournaments", t => t.Tournament_TournamentId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Student_StudentId, cascadeDelete: true)
                .Index(t => t.Tournament_TournamentId)
                .Index(t => t.Student_StudentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Matches", "Winner_TeamId", "dbo.Teams");
            DropForeignKey("dbo.Teams", "Tournament_TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.TournamentStudents", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.TournamentStudents", "Tournament_TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.Tournaments", "MyTournamentTree_TournamentTreeId", "dbo.TournamentTrees");
            DropForeignKey("dbo.TournamentTrees", "MySecondaryTournamentTree_MatchId", "dbo.Matches");
            DropForeignKey("dbo.TournamentTrees", "MyMainTournamentTree_MatchId", "dbo.Matches");
            DropForeignKey("dbo.StudentTeams", "Team_TeamId", "dbo.Teams");
            DropForeignKey("dbo.StudentTeams", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.Students", "SummonerID", "dbo.Summoners");
            DropForeignKey("dbo.CSLs", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.CSLEs", "CSL_CSLId", "dbo.CSLs");
            DropForeignKey("dbo.CSLEs", "MiniSeries_CustomMiniSeriesId", "dbo.CustomMiniSeries");
            DropForeignKey("dbo.Matches", "RightContendantId", "dbo.Matches");
            DropForeignKey("dbo.Matches", "LeftContendantId", "dbo.Matches");
            DropIndex("dbo.TournamentStudents", new[] { "Student_StudentId" });
            DropIndex("dbo.TournamentStudents", new[] { "Tournament_TournamentId" });
            DropIndex("dbo.StudentTeams", new[] { "Team_TeamId" });
            DropIndex("dbo.StudentTeams", new[] { "Student_StudentId" });
            DropIndex("dbo.TournamentTrees", new[] { "MySecondaryTournamentTree_MatchId" });
            DropIndex("dbo.TournamentTrees", new[] { "MyMainTournamentTree_MatchId" });
            DropIndex("dbo.Tournaments", new[] { "MyTournamentTree_TournamentTreeId" });
            DropIndex("dbo.CSLEs", new[] { "CSL_CSLId" });
            DropIndex("dbo.CSLEs", new[] { "MiniSeries_CustomMiniSeriesId" });
            DropIndex("dbo.CSLs", new[] { "Student_StudentId" });
            DropIndex("dbo.Students", new[] { "SummonerID" });
            DropIndex("dbo.Teams", new[] { "Tournament_TournamentId" });
            DropIndex("dbo.Matches", new[] { "Winner_TeamId" });
            DropIndex("dbo.Matches", new[] { "RightContendantId" });
            DropIndex("dbo.Matches", new[] { "LeftContendantId" });
            DropTable("dbo.TournamentStudents");
            DropTable("dbo.StudentTeams");
            DropTable("dbo.TournamentTrees");
            DropTable("dbo.Tournaments");
            DropTable("dbo.Summoners");
            DropTable("dbo.CustomMiniSeries");
            DropTable("dbo.CSLEs");
            DropTable("dbo.CSLs");
            DropTable("dbo.Students");
            DropTable("dbo.Teams");
            DropTable("dbo.Matches");
        }
    }
}
