namespace SoloTournamentCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateSoloQueue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "SummonerSoloQueueData_Name", c => c.String());
            AddColumn("dbo.Students", "SummonerSoloQueueData_ParticipantId", c => c.String());
            AddColumn("dbo.Students", "SummonerSoloQueueData_Queue", c => c.Int(nullable: false));
            AddColumn("dbo.Students", "SummonerSoloQueueData_Tier", c => c.Int(nullable: false));
            DropColumn("dbo.Students", "SummonerLeagueData_Name");
            DropColumn("dbo.Students", "SummonerLeagueData_ParticipantId");
            DropColumn("dbo.Students", "SummonerLeagueData_Queue");
            DropColumn("dbo.Students", "SummonerLeagueData_Tier");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "SummonerLeagueData_Tier", c => c.Int(nullable: false));
            AddColumn("dbo.Students", "SummonerLeagueData_Queue", c => c.Int(nullable: false));
            AddColumn("dbo.Students", "SummonerLeagueData_ParticipantId", c => c.String());
            AddColumn("dbo.Students", "SummonerLeagueData_Name", c => c.String());
            DropColumn("dbo.Students", "SummonerSoloQueueData_Tier");
            DropColumn("dbo.Students", "SummonerSoloQueueData_Queue");
            DropColumn("dbo.Students", "SummonerSoloQueueData_ParticipantId");
            DropColumn("dbo.Students", "SummonerSoloQueueData_Name");
        }
    }
}
