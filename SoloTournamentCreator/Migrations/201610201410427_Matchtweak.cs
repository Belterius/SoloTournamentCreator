namespace SoloTournamentCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Matchtweak : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Matches", "WinNext", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Matches", "WinNext");
        }
    }
}
