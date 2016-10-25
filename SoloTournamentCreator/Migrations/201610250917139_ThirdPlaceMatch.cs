namespace SoloTournamentCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThirdPlaceMatch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TournamentTrees", "MyThirdMatchPlace_MatchId", c => c.Int());
            CreateIndex("dbo.TournamentTrees", "MyThirdMatchPlace_MatchId");
            AddForeignKey("dbo.TournamentTrees", "MyThirdMatchPlace_MatchId", "dbo.Matches", "MatchId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TournamentTrees", "MyThirdMatchPlace_MatchId", "dbo.Matches");
            DropIndex("dbo.TournamentTrees", new[] { "MyThirdMatchPlace_MatchId" });
            DropColumn("dbo.TournamentTrees", "MyThirdMatchPlace_MatchId");
        }
    }
}
