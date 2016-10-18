namespace SoloTournamentCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tournamentupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tournaments", "NbTeam", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tournaments", "NbTeam");
        }
    }
}
