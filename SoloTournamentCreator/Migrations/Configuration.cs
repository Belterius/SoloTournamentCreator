namespace SoloTournamentCreator.Migrations
{
    using Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SoloTournamentCreator.SavingContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SoloTournamentCreator.SavingContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.

            //context.MyStudents.AddOrUpdate(new Student("mail@", "Loic", "Bailleul", "Belterius", 2018));
            //context.MyStudents.AddOrUpdate(new Student("mail@", "Olivier", "Cano", "Kindermoumoute", 2018));
            //context.MyStudents.AddOrUpdate(new Student("mail@", "Nicolas", "Wycaert", "Ninochin", 2018));
            //context.MyStudents.AddOrUpdate(new Student("mail@", "Christopher", "LaBrute", "Darkmonstre", 2018));
            //context.SaveChanges();
        }
    }
}
