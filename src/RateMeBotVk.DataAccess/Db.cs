using Microsoft.EntityFrameworkCore;
using RateMeBotVk.DataAccess.Models;

namespace RateMeBotVk.DataAccess;

public class Db : DbContext
{
    public Db(DbContextOptions<Db> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(x => x.Username)
            .IsUnique();
    }

    public DbSet<User> Users { get; set; }
}
