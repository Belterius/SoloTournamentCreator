namespace SoloTournamentCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PublicTeamSize : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teams", "NbPlayerMax", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teams", "NbPlayerMax");
        }
    }
}
