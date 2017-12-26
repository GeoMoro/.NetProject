using Data.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistance
{
    public interface IDatabaseContext
    {

        DbSet<UserStatus> UserStatus { get; set; }
        DbSet<Presence> Presences { get; set; }
        DbSet<Lecture> Lectures { get; set; }
        DbSet<Answer> Answers { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<Kata> Katas { get; set; }
        DbSet<News> News { get; set; }
        DbSet<Attendance> Attendances { get; set; }

        int SaveChanges();
    }
}