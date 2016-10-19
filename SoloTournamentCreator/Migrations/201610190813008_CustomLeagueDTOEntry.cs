namespace SoloTournamentCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomLeagueDTOEntry : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomLeagueEntryDtoes",
                c => new
                    {
                        CustomLeagueEntryDtoId = c.Int(nullable: false, identity: true),
                        Division = c.String(),
                        IsFreshBlood = c.Boolean(nullable: false),
                        IsHotStreak = c.Boolean(nullable: false),
                        IsInactive = c.Boolean(nullable: false),
                        IsVeteran = c.Boolean(nullable: false),
                        LeaguePoints = c.Int(nullable: false),
                        Losses = c.Int(nullable: false),
                        MiniSeries_Losses = c.Int(nullable: false),
                        MiniSeries_Progress = c.String(),
                        MiniSeries_Target = c.Int(nullable: false),
                        MiniSeries_Wins = c.Int(nullable: false),
                        PlayerOrTeamId = c.String(),
                        PlayerOrTeamName = c.String(),
                        Wins = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CustomLeagueEntryDtoId);
            
            AddColumn("dbo.Students", "DetailSoloQueueData_CustomLeagueEntryDtoId", c => c.Int());
            CreateIndex("dbo.Students", "DetailSoloQueueData_CustomLeagueEntryDtoId");
            AddForeignKey("dbo.Students", "DetailSoloQueueData_CustomLeagueEntryDtoId", "dbo.CustomLeagueEntryDtoes", "CustomLeagueEntryDtoId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "DetailSoloQueueData_CustomLeagueEntryDtoId", "dbo.CustomLeagueEntryDtoes");
            DropIndex("dbo.Students", new[] { "DetailSoloQueueData_CustomLeagueEntryDtoId" });
            DropColumn("dbo.Students", "DetailSoloQueueData_CustomLeagueEntryDtoId");
            DropTable("dbo.CustomLeagueEntryDtoes");
        }
    }
}
