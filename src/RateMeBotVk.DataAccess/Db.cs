using Microsoft.EntityFrameworkCore;
using RateMeBotVk.DataAccess.Infrastructure;
using RateMeBotVk.DataAccess.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RateMeBotVk.DataAccess;

public class Db : DbContext
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Rate> Rates { get; set; } = default!;

    public Db(DbContextOptions<Db> options) : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(user =>
        {
            user.Property(x => x.Created)
                .HasDefaultValueSql("getdate()");

            user.HasIndex(x => x.Username)
                .IsUnique();
        });

        modelBuilder.Entity<Rate>(rate =>
        {
            rate.Property(x => x.Created)
                .HasDefaultValueSql("getdate()");

            rate.HasOne(x => x.RatedUser)
                .WithMany(x => x.GotRates)
                .HasForeignKey(x => x.RatedUserId)
                .OnDelete(DeleteBehavior.NoAction);

            rate.HasOne(x => x.RatingUser)
                .WithMany(x => x.SentRates)
                .HasForeignKey(x => x.RatingUserId)
                .OnDelete(DeleteBehavior.NoAction);

            rate.HasIndex(x => x.RatedUserId);
            rate.HasIndex(x => x.RatingUserId);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now;
        var updatedEntities = this.ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Modified || x.State == EntityState.Added)
            .Select(x => x.Entity);

        foreach (var entity in updatedEntities)
        {
            if (entity is Updatable updated)
            {
                updated.Update();
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
