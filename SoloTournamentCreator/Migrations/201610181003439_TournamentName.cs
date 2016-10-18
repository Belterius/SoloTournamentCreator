namespace SoloTournamentCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TournamentName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tournaments", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tournaments", "Name");
        }
    }
}
