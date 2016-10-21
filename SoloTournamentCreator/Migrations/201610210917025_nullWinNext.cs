namespace SoloTournamentCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullWinNext : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Matches", "WinNext", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Matches", "WinNext", c => c.Boolean(nullable: false));
        }
    }
}
