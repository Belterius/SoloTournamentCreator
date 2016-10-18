namespace SoloTournamentCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TournamentStage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tournaments", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.Tournaments", "IsStarted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tournaments", "IsStarted", c => c.Boolean(nullable: false));
            DropColumn("dbo.Tournaments", "Status");
        }
    }
}
