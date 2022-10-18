using Microsoft.EntityFrameworkCore;
using MudaBot.DataAccess.Infrastructure;
using MudaBot.DataAccess.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MudaBot.DataAccess;

public class Db : DbContext
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Dislike> Rates { get; set; } = default!;
    public DbSet<Comment> Comments { get; set; } = default!;

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
        });

        modelBuilder.Entity<Dislike>(dislike =>
        {
            dislike.HasOne(x => x.RatedUser)
                .WithMany(x => x.GotDislikes)
                .HasForeignKey(x => x.RatedUserId)
                .OnDelete(DeleteBehavior.NoAction);

            dislike.HasOne(x => x.RatingUser)
                .WithMany(x => x.SentDislikes)
                .HasForeignKey(x => x.RatingUserId)
                .OnDelete(DeleteBehavior.NoAction);

            dislike.HasIndex(x => x.RatedUserId);
            dislike.HasIndex(x => x.RatingUserId);
        });

        modelBuilder.Entity<Comment>(comment =>
        {
            comment.HasOne(x => x.Owner)
                .WithMany(x => x.SendComments)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);

            comment.HasOne(x => x.ReceivingUser)
                .WithMany(x => x.GotComments)
                .HasForeignKey(x => x.ReceivingUserId)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now;
        var updatedEntities = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Modified || x.State == EntityState.Added)
            .Select(x => new { Value = x.Entity, x.State});

        foreach (var entity in updatedEntities)
        {
            if (entity.Value is TimeAccounting updated)
            {
                if (entity.State == EntityState.Added) updated.OnCreate();
                if (entity.State == EntityState.Modified) updated.OnUpdate();
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
