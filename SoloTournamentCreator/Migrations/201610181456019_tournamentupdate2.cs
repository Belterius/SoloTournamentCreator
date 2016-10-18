namespace SoloTournamentCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tournamentupdate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tournaments", "MyTournamentTree_TournamentTreeId", c => c.Int());
            CreateIndex("dbo.Tournaments", "MyTournamentTree_TournamentTreeId");
            AddForeignKey("dbo.Tournaments", "MyTournamentTree_TournamentTreeId", "dbo.TournamentTrees", "TournamentTreeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tournaments", "MyTournamentTree_TournamentTreeId", "dbo.TournamentTrees");
            DropIndex("dbo.Tournaments", new[] { "MyTournamentTree_TournamentTreeId" });
            DropColumn("dbo.Tournaments", "MyTournamentTree_TournamentTreeId");
        }
    }
}
