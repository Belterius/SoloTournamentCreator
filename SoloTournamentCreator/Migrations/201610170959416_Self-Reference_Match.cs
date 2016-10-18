namespace SoloTournamentCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SelfReference_Match : DbMigration
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
                        Winner_TeamId = c.Int(),
                        TournamentTree_TournamentTreeId = c.Int(),
                        TournamentTree_TournamentTreeId1 = c.Int(),
                    })
                .PrimaryKey(t => t.MatchId)
                .ForeignKey("dbo.Matches", t => t.LeftContendantId)
                .ForeignKey("dbo.Matches", t => t.RightContendantId)
                .ForeignKey("dbo.Teams", t => t.Winner_TeamId)
                .ForeignKey("dbo.TournamentTrees", t => t.TournamentTree_TournamentTreeId)
                .ForeignKey("dbo.TournamentTrees", t => t.TournamentTree_TournamentTreeId1)
                .Index(t => t.LeftContendantId)
                .Index(t => t.RightContendantId)
                .Index(t => t.Winner_TeamId)
                .Index(t => t.TournamentTree_TournamentTreeId)
                .Index(t => t.TournamentTree_TournamentTreeId1);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamId = c.Int(nullable: false, identity: true),
                        TeamName = c.String(),
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
                        Mail = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        GraduationYear = c.Int(nullable: false),
                        SummonerLeagueData_Name = c.String(),
                        SummonerLeagueData_ParticipantId = c.String(),
                        SummonerLeagueData_Queue = c.Int(nullable: false),
                        SummonerLeagueData_Tier = c.Int(nullable: false),
                        SummonerData_Id = c.Long(),
                        Team_TeamId = c.Int(),
                        Tournament_TournamentId = c.Int(),
                    })
                .PrimaryKey(t => t.StudentId)
                .ForeignKey("dbo.SummonerDtoes", t => t.SummonerData_Id)
                .ForeignKey("dbo.Teams", t => t.Team_TeamId)
                .ForeignKey("dbo.Tournaments", t => t.Tournament_TournamentId)
                .Index(t => t.SummonerData_Id)
                .Index(t => t.Team_TeamId)
                .Index(t => t.Tournament_TournamentId);
            
            CreateTable(
                "dbo.SummonerDtoes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        ProfileIconId = c.Int(nullable: false),
                        RevisionDate = c.Long(nullable: false),
                        SummonerLevel = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tournaments",
                c => new
                    {
                        TournamentId = c.Int(nullable: false, identity: true),
                        IsStarted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TournamentId);
            
            CreateTable(
                "dbo.TournamentTrees",
                c => new
                    {
                        TournamentTreeId = c.Int(nullable: false, identity: true),
                        MyTournamentTree_MatchId = c.Int(),
                    })
                .PrimaryKey(t => t.TournamentTreeId)
                .ForeignKey("dbo.Matches", t => t.MyTournamentTree_MatchId)
                .Index(t => t.MyTournamentTree_MatchId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Matches", "TournamentTree_TournamentTreeId1", "dbo.TournamentTrees");
            DropForeignKey("dbo.TournamentTrees", "MyTournamentTree_MatchId", "dbo.Matches");
            DropForeignKey("dbo.Matches", "TournamentTree_TournamentTreeId", "dbo.TournamentTrees");
            DropForeignKey("dbo.Teams", "Tournament_TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.Students", "Tournament_TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.Matches", "Winner_TeamId", "dbo.Teams");
            DropForeignKey("dbo.Students", "Team_TeamId", "dbo.Teams");
            DropForeignKey("dbo.Students", "SummonerData_Id", "dbo.SummonerDtoes");
            DropForeignKey("dbo.Matches", "RightContendantId", "dbo.Matches");
            DropForeignKey("dbo.Matches", "LeftContendantId", "dbo.Matches");
            DropIndex("dbo.TournamentTrees", new[] { "MyTournamentTree_MatchId" });
            DropIndex("dbo.Students", new[] { "Tournament_TournamentId" });
            DropIndex("dbo.Students", new[] { "Team_TeamId" });
            DropIndex("dbo.Students", new[] { "SummonerData_Id" });
            DropIndex("dbo.Teams", new[] { "Tournament_TournamentId" });
            DropIndex("dbo.Matches", new[] { "TournamentTree_TournamentTreeId1" });
            DropIndex("dbo.Matches", new[] { "TournamentTree_TournamentTreeId" });
            DropIndex("dbo.Matches", new[] { "Winner_TeamId" });
            DropIndex("dbo.Matches", new[] { "RightContendantId" });
            DropIndex("dbo.Matches", new[] { "LeftContendantId" });
            DropTable("dbo.TournamentTrees");
            DropTable("dbo.Tournaments");
            DropTable("dbo.SummonerDtoes");
            DropTable("dbo.Students");
            DropTable("dbo.Teams");
            DropTable("dbo.Matches");
        }
    }
}
