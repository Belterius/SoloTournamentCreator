namespace SoloTournamentCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManyToManyStudentTeam : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Students", "Team_TeamId", "dbo.Teams");
            DropIndex("dbo.Students", new[] { "Team_TeamId" });
            CreateTable(
                "dbo.StudentTeams",
                c => new
                    {
                        Student_StudentId = c.Int(nullable: false),
                        Team_TeamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_StudentId, t.Team_TeamId })
                .ForeignKey("dbo.Students", t => t.Student_StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Teams", t => t.Team_TeamId, cascadeDelete: true)
                .Index(t => t.Student_StudentId)
                .Index(t => t.Team_TeamId);
            
            DropColumn("dbo.Students", "Team_TeamId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "Team_TeamId", c => c.Int());
            DropForeignKey("dbo.StudentTeams", "Team_TeamId", "dbo.Teams");
            DropForeignKey("dbo.StudentTeams", "Student_StudentId", "dbo.Students");
            DropIndex("dbo.StudentTeams", new[] { "Team_TeamId" });
            DropIndex("dbo.StudentTeams", new[] { "Student_StudentId" });
            DropTable("dbo.StudentTeams");
            CreateIndex("dbo.Students", "Team_TeamId");
            AddForeignKey("dbo.Students", "Team_TeamId", "dbo.Teams", "TeamId");
        }
    }
}
