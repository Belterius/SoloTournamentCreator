namespace SoloTournamentCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
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
                        SummonerSoloQueueData_Name = c.String(unicode: false),
                        SummonerSoloQueueData_ParticipantId = c.String(unicode: false),
                        SummonerSoloQueueData_Queue = c.Int(nullable: false),
                        SummonerSoloQueueData_Tier = c.Int(nullable: false),
                        DetailSoloQueueData_CustomLeagueEntryDtoId = c.Int(),
                        SummonerData_Id = c.Long(),
                    })
                .PrimaryKey(t => t.StudentId)
                .ForeignKey("dbo.CustomLeagueEntryDtoes", t => t.DetailSoloQueueData_CustomLeagueEntryDtoId)
                .ForeignKey("dbo.SummonerDtoes", t => t.SummonerData_Id)
                .Index(t => t.DetailSoloQueueData_CustomLeagueEntryDtoId)
                .Index(t => t.SummonerData_Id);
            
            CreateTable(
                "dbo.CustomLeagueEntryDtoes",
                c => new
                    {
                        CustomLeagueEntryDtoId = c.Int(nullable: false, identity: true),
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
                    })
                .PrimaryKey(t => t.CustomLeagueEntryDtoId)
                .ForeignKey("dbo.CustomMiniSeries", t => t.MiniSeries_CustomMiniSeriesId)
                .Index(t => t.MiniSeries_CustomMiniSeriesId);
            
            CreateTable(
                "dbo.CustomMiniSeries",
                c => new
                    {
                        CustomMiniSeriesId = c.Int(nullable: false, identity: true),
                        Losses = c.Int(nullable: false),
                        Progress = c.String(unicode: false),
                        Target = c.Int(nullable: false),
                        Wins = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CustomMiniSeriesId);
            
            CreateTable(
                "dbo.Tournaments",
                c => new
                    {
                        TournamentId = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Name = c.String(unicode: false),
                        NbTeam = c.Int(nullable: false),
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
                        MyThirdMatchPlace_MatchId = c.Int(),
                        MyTournamentTree_MatchId = c.Int(),
                    })
                .PrimaryKey(t => t.TournamentTreeId)
                .ForeignKey("dbo.Matches", t => t.MyThirdMatchPlace_MatchId)
                .ForeignKey("dbo.Matches", t => t.MyTournamentTree_MatchId)
                .Index(t => t.MyThirdMatchPlace_MatchId)
                .Index(t => t.MyTournamentTree_MatchId);
            
            CreateTable(
                "dbo.SummonerDtoes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        ProfileIconId = c.Int(nullable: false),
                        RevisionDate = c.Long(nullable: false),
                        SummonerLevel = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
            DropForeignKey("dbo.Students", "SummonerData_Id", "dbo.SummonerDtoes");
            DropForeignKey("dbo.Teams", "Tournament_TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.TournamentStudents", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.TournamentStudents", "Tournament_TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.Tournaments", "MyTournamentTree_TournamentTreeId", "dbo.TournamentTrees");
            DropForeignKey("dbo.TournamentTrees", "MyTournamentTree_MatchId", "dbo.Matches");
            DropForeignKey("dbo.TournamentTrees", "MyThirdMatchPlace_MatchId", "dbo.Matches");
            DropForeignKey("dbo.StudentTeams", "Team_TeamId", "dbo.Teams");
            DropForeignKey("dbo.StudentTeams", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.Students", "DetailSoloQueueData_CustomLeagueEntryDtoId", "dbo.CustomLeagueEntryDtoes");
            DropForeignKey("dbo.CustomLeagueEntryDtoes", "MiniSeries_CustomMiniSeriesId", "dbo.CustomMiniSeries");
            DropForeignKey("dbo.Matches", "RightContendantId", "dbo.Matches");
            DropForeignKey("dbo.Matches", "LeftContendantId", "dbo.Matches");
            DropIndex("dbo.TournamentStudents", new[] { "Student_StudentId" });
            DropIndex("dbo.TournamentStudents", new[] { "Tournament_TournamentId" });
            DropIndex("dbo.StudentTeams", new[] { "Team_TeamId" });
            DropIndex("dbo.StudentTeams", new[] { "Student_StudentId" });
            DropIndex("dbo.TournamentTrees", new[] { "MyTournamentTree_MatchId" });
            DropIndex("dbo.TournamentTrees", new[] { "MyThirdMatchPlace_MatchId" });
            DropIndex("dbo.Tournaments", new[] { "MyTournamentTree_TournamentTreeId" });
            DropIndex("dbo.CustomLeagueEntryDtoes", new[] { "MiniSeries_CustomMiniSeriesId" });
            DropIndex("dbo.Students", new[] { "SummonerData_Id" });
            DropIndex("dbo.Students", new[] { "DetailSoloQueueData_CustomLeagueEntryDtoId" });
            DropIndex("dbo.Teams", new[] { "Tournament_TournamentId" });
            DropIndex("dbo.Matches", new[] { "Winner_TeamId" });
            DropIndex("dbo.Matches", new[] { "RightContendantId" });
            DropIndex("dbo.Matches", new[] { "LeftContendantId" });
            DropTable("dbo.TournamentStudents");
            DropTable("dbo.StudentTeams");
            DropTable("dbo.SummonerDtoes");
            DropTable("dbo.TournamentTrees");
            DropTable("dbo.Tournaments");
            DropTable("dbo.CustomMiniSeries");
            DropTable("dbo.CustomLeagueEntryDtoes");
            DropTable("dbo.Students");
            DropTable("dbo.Teams");
            DropTable("dbo.Matches");
        }
    }
}
