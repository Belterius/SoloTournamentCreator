namespace SoloTournamentCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomMiniSeries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomMiniSeries",
                c => new
                    {
                        CustomMiniSeriesId = c.Int(nullable: false, identity: true),
                        Losses = c.Int(nullable: false),
                        Progress = c.String(),
                        Target = c.Int(nullable: false),
                        Wins = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CustomMiniSeriesId);
            
            AddColumn("dbo.CustomLeagueEntryDtoes", "MiniSeries_CustomMiniSeriesId", c => c.Int());
            CreateIndex("dbo.CustomLeagueEntryDtoes", "MiniSeries_CustomMiniSeriesId");
            AddForeignKey("dbo.CustomLeagueEntryDtoes", "MiniSeries_CustomMiniSeriesId", "dbo.CustomMiniSeries", "CustomMiniSeriesId");
            DropColumn("dbo.CustomLeagueEntryDtoes", "MiniSeries_Losses");
            DropColumn("dbo.CustomLeagueEntryDtoes", "MiniSeries_Progress");
            DropColumn("dbo.CustomLeagueEntryDtoes", "MiniSeries_Target");
            DropColumn("dbo.CustomLeagueEntryDtoes", "MiniSeries_Wins");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomLeagueEntryDtoes", "MiniSeries_Wins", c => c.Int(nullable: false));
            AddColumn("dbo.CustomLeagueEntryDtoes", "MiniSeries_Target", c => c.Int(nullable: false));
            AddColumn("dbo.CustomLeagueEntryDtoes", "MiniSeries_Progress", c => c.String());
            AddColumn("dbo.CustomLeagueEntryDtoes", "MiniSeries_Losses", c => c.Int(nullable: false));
            DropForeignKey("dbo.CustomLeagueEntryDtoes", "MiniSeries_CustomMiniSeriesId", "dbo.CustomMiniSeries");
            DropIndex("dbo.CustomLeagueEntryDtoes", new[] { "MiniSeries_CustomMiniSeriesId" });
            DropColumn("dbo.CustomLeagueEntryDtoes", "MiniSeries_CustomMiniSeriesId");
            DropTable("dbo.CustomMiniSeries");
        }
    }
}
