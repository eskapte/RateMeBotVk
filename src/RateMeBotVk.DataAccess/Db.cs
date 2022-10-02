using Microsoft.EntityFrameworkCore;
using RateMeBotVk.DataAccess.Models;
using System;

namespace RateMeBotVk.DataAccess;

public class Db : DbContext
{
    public Db(DbContextOptions<Db> options) : base(options)
    {
        //Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region User
        modelBuilder.Entity<User>()
            .HasIndex(x => x.Username)
            .IsUnique();
        modelBuilder.Entity<User>()
            .Property(x => x.Created)
            .HasDefaultValueSql("getdate()");
        modelBuilder.Entity<User>()
            .Property(x => x.Updated)
            .ValueGeneratedOnUpdate();
        #endregion

        #region Rate
        modelBuilder.Entity<Rate>()
            .Property(x => x.Date)
            .HasDefaultValueSql("getdate()");
        #endregion 
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Rate> Rates { get; set; }
}
