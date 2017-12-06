using Microsoft.EntityFrameworkCore;

namespace Data.Persistance
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        // Use entire link to entity for Scaffolding to work

        public DbSet<Data.Domain.Entities.UserAccount> UserAccounts { get; set; }
        public DbSet<Data.Domain.Entities.Presence> Presences { get; set; }
        public DbSet<Data.Domain.Entities.Faction> Factions { get; set; }
        public DbSet<Data.Domain.Entities.Lecture> Lectures { get; set; }
        public DbSet<Data.Domain.Entities.Answer> Answers { get; set; }
        public DbSet<Data.Domain.Entities.Question> Questions { get; set; }

    }
}
