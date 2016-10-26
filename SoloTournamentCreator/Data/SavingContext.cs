namespace SoloTournamentCreator
{
    using Data;
    using MySql.Data.Entity;
    using System;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Linq;

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class SavingContext : DbContext
    {
        // Your context has been configured to use a 'MyDatabase' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'SoloTournamentCreator.MyDatabase' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'MyDatabase' 
        // connection string in the application configuration file.
        public SavingContext() : base("name=SavingContext")
        {
        }
        public SavingContext(string nameOrConnectionString) 
        : base("name=SavingContext")
        {
        }
        public SavingContext(DbConnection existingConnection, bool contextOwnsConnection)
        : base(existingConnection, contextOwnsConnection)
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new NonPublicColumnAttributeConvention());
            modelBuilder.Entity<Model.Match>().
            HasOptional(e => e.LeftContendant).
            WithMany().
            HasForeignKey(m => m.LeftContendantId);
            modelBuilder.Entity<Model.Match>().
            HasOptional(e => e.RightContendant).
            WithMany().
            HasForeignKey(m => m.RightContendantId);
            
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