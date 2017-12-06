using Data.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistance
{
    public interface IDatabaseContext
    {
        DbSet<UserAccount> UserAccounts { get; set; }
        DbSet<Presence> Presences { get; set; }
        DbSet<Faction> Factions { get; set; }
        DbSet<Lecture> Lectures { get; set; }
        DbSet<Answer> Answers { get; set; }
        DbSet<Question> Questions { get; set; }

        int SaveChanges();
    }
}