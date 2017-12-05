using Data.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistance
{
    public interface IDatabaseContext
    {
        DbSet<UserAccount> UserAccounts { get; set; }
        DbSet<Presence> Presences { get; set; }

        int SaveChanges();
    }
}