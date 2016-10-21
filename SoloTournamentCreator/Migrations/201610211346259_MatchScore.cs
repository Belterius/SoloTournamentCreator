namespace SoloTournamentCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MatchScore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Matches", "WinnerScore", c => c.Int(nullable: false));
            AddColumn("dbo.Matches", "LoserScore", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Matches", "LoserScore");
            DropColumn("dbo.Matches", "WinnerScore");
        }
    }
}
