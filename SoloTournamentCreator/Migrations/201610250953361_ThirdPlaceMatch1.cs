namespace SoloTournamentCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThirdPlaceMatch1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Matches", "Depth", c => c.Int(nullable: false));
            AddColumn("dbo.TournamentTrees", "MaxDepth", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TournamentTrees", "MaxDepth");
            DropColumn("dbo.Matches", "Depth");
        }
    }
}
