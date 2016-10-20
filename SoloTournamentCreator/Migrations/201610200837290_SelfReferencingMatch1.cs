namespace SoloTournamentCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SelfReferencingMatch1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Matches", "TournamentTree_TournamentTreeId", "dbo.TournamentTrees");
            DropForeignKey("dbo.Matches", "TournamentTree_TournamentTreeId1", "dbo.TournamentTrees");
            DropIndex("dbo.Matches", new[] { "TournamentTree_TournamentTreeId" });
            DropIndex("dbo.Matches", new[] { "TournamentTree_TournamentTreeId1" });
            DropColumn("dbo.Matches", "TournamentTree_TournamentTreeId");
            DropColumn("dbo.Matches", "TournamentTree_TournamentTreeId1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Matches", "TournamentTree_TournamentTreeId1", c => c.Int());
            AddColumn("dbo.Matches", "TournamentTree_TournamentTreeId", c => c.Int());
            CreateIndex("dbo.Matches", "TournamentTree_TournamentTreeId1");
            CreateIndex("dbo.Matches", "TournamentTree_TournamentTreeId");
            AddForeignKey("dbo.Matches", "TournamentTree_TournamentTreeId1", "dbo.TournamentTrees", "TournamentTreeId");
            AddForeignKey("dbo.Matches", "TournamentTree_TournamentTreeId", "dbo.TournamentTrees", "TournamentTreeId");
        }
    }
}
