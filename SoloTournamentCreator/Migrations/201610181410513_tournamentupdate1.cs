namespace SoloTournamentCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tournamentupdate1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Students", "Tournament_TournamentId", "dbo.Tournaments");
            DropIndex("dbo.Students", new[] { "Tournament_TournamentId" });
            CreateTable(
                "dbo.TournamentStudents",
                c => new
                    {
                        Tournament_TournamentId = c.Int(nullable: false),
                        Student_StudentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tournament_TournamentId, t.Student_StudentId })
                .ForeignKey("dbo.Tournaments", t => t.Tournament_TournamentId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Student_StudentId, cascadeDelete: true)
                .Index(t => t.Tournament_TournamentId)
                .Index(t => t.Student_StudentId);
            
            DropColumn("dbo.Students", "Tournament_TournamentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "Tournament_TournamentId", c => c.Int());
            DropForeignKey("dbo.TournamentStudents", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.TournamentStudents", "Tournament_TournamentId", "dbo.Tournaments");
            DropIndex("dbo.TournamentStudents", new[] { "Student_StudentId" });
            DropIndex("dbo.TournamentStudents", new[] { "Tournament_TournamentId" });
            DropTable("dbo.TournamentStudents");
            CreateIndex("dbo.Students", "Tournament_TournamentId");
            AddForeignKey("dbo.Students", "Tournament_TournamentId", "dbo.Tournaments", "TournamentId");
        }
    }
}
