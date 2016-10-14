namespace SoloTournamentCreator
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class MyDatabase : DbContext
    {
        // Your context has been configured to use a 'MyDatabase' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'SoloTournamentCreator.MyDatabase' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'MyDatabase' 
        // connection string in the application configuration file.
        public MyDatabase() : base("name=MyDatabase")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Model.Match> MyMatchs { get; set; }
        public virtual DbSet<Model.Student> MyStudents { get; set; }
        public virtual DbSet<Model.Team> MyTeams { get; set; }
        public virtual DbSet<Model.Tournament> MyTournaments { get; set; }
        public virtual DbSet<Model.TournamentTree> MyTournamentTrees { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}