#region Copyright

// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
// 
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
// 
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 11/03/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.DataAccess
// File: AppDbContext.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.DataAccess\AppDbContext.cs
// ---------------------------------------------------------------------------

#endregion

#region Usings

using BattlelineExtras.Contracts.Interfaces;
using CommuniQueue.Contracts.Models;
using Microsoft.EntityFrameworkCore;

#endregion

namespace CommuniQueue.DataAccess;

// dotnet tool update --global dotnet-ef
// cd path/to/CommuniQueue.Api
// dotnet ef migrations add {COMMENT} --project ../CommuniQueue.DataAccess/CommuniQueue.DataAccess.csproj
// dotnet ef database update --project ../CommuniQueue.DataAccess/CommuniQueue.DataAccess.csproj

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Container> Containers { get; set; }
    public DbSet<Template> Templates { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Stage> Stages { get; set; }
    public DbSet<TemplateVersion> TemplateVersions { get; set; }
    public DbSet<TemplateStageAssignment> TemplateStageAssignments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.SsoId).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => new { e.UserId, e.EntityId, e.EntityType }).IsUnique();
            entity.HasOne(e => e.User)
                .WithMany(u => u.Permissions)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(p => p.Project)
                .WithMany()
                .HasForeignKey(p => p.EntityId)
                .HasPrincipalKey(pr => pr.Id)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Container>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Project)
                .WithMany(p => p.Containers)
                .HasForeignKey(c => c.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Template>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(t => t.Container)
                .WithMany(c => c.Templates)
                .HasForeignKey(t => t.ContainerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.Project)
                .WithMany(p => p.Templates)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TemplateVersion>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(tv => tv.Template)
                .WithMany(t => t.Versions)
                .HasForeignKey(tv => tv.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TemplateStageAssignment>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(tsa => tsa.TemplateVersion)
                .WithMany(tv => tv.StageAssignments)
                .HasForeignKey(tsa => tsa.TemplateVersionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(tsa => tsa.Stage)
                .WithMany()
                .HasForeignKey(tsa => tsa.StageId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.StageId, e.TemplateVersionId }).IsUnique();
        });

        modelBuilder.Entity<Stage>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(s => s.Project)
                .WithMany(p => p.Stages)
                .HasForeignKey(s => s.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.ProjectId, e.Name }).IsUnique();
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(p => p.RootContainer)
                .WithOne()
                .HasForeignKey<Project>(p => p.RootContainerId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e is { Entity: IEntity, State: EntityState.Added or EntityState.Modified });

        foreach (var entityEntry in entries)
        {
            ((IEntity)entityEntry.Entity).UpdatedDateTime = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                ((IEntity)entityEntry.Entity).Id = Guid.NewGuid();
                ((IEntity)entityEntry.Entity).CreatedDateTime = DateTime.UtcNow;
            }
        }

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e is { Entity: IEntity, State: EntityState.Added or EntityState.Modified });

        foreach (var entityEntry in entries)
        {
            ((IEntity)entityEntry.Entity).UpdatedDateTime = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                ((IEntity)entityEntry.Entity).Id = Guid.NewGuid();
                ((IEntity)entityEntry.Entity).CreatedDateTime = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
