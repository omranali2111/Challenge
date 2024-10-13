using System;
using System.Collections.Generic;
using ChallengeBackEnd.Model;
using Microsoft.EntityFrameworkCore;

namespace ChallengeBackEnd.Data;

public partial class MyDatabaseContext : DbContext
{
    public MyDatabaseContext()
    {
    }

    public MyDatabaseContext(DbContextOptions<MyDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasOne(s => s.Class)
            .WithMany(c => c.Students)
            .HasForeignKey(s => s.ClassId);

        modelBuilder.Entity<Student>()
            .HasOne(s => s.Country)
            .WithMany(c => c.Students)
            .HasForeignKey(s => s.CountryId);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
