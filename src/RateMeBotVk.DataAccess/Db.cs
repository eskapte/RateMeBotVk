using Microsoft.EntityFrameworkCore;
using RateMeBotVk.DataAccess.Models;
using System;

namespace RateMeBotVk.DataAccess;

public class Db : DbContext
{
    public Db(DbContextOptions<Db> options) : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(user =>
        {
            user.HasIndex(x => x.Username)
                .IsUnique();
            user.Property(x => x.Created)
                .HasDefaultValueSql("getdate()");
            user.Property(x => x.Updated)
                .ValueGeneratedOnUpdate();
        });
            
        modelBuilder.Entity<Rate>(rate =>
        {
            rate.Property(x => x.Date)
                .HasDefaultValueSql("getdate()");
            rate.HasOne(x => x.RatedUser)
                .WithMany(x => x.GotRates)
                .HasForeignKey(x => x.RatedUserId)
                .OnDelete(DeleteBehavior.NoAction);
            rate.HasOne(x => x.RatingUser)
                .WithMany(x => x.SentRates)
                .HasForeignKey(x => x.RatingUserId)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Rate> Rates { get; set; } = default!;
}
