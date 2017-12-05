using Data.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistance
{
    public interface IDatabaseContext
    {
        DbSet<UserAccount> UserAccounts { get; set; }

        DbSet<Answer> Answers { get; set; }

        int SaveChanges();
    }
}