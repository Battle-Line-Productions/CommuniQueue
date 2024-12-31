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

using System.Reflection.Metadata;
using System.Text.Json;
using BattlelineExtras.Contracts.Interfaces;
using CommuniQueue.Contracts.Models;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

#endregion

namespace CommuniQueue.DataAccess;

// dotnet tool update --global dotnet-ef
// cd path/to/CommuniQueue.Api
// dotnet ef migrations add InitialMigration --project ../CommuniQueue.DataAccess/CommuniQueue.DataAccess.csproj  --context AppDbContext
// dotnet ef database update --project ../CommuniQueue.DataAccess/CommuniQueue.DataAccess.csproj
// dotnet ef migrations remove --project ../CommuniQueue.DataAccess/CommuniQueue.DataAccess.csproj

public class AppDbContext(IMultiTenantContextAccessor multiTenantContextAccessor, DbContextOptions<AppDbContext> options) : MultiTenantDbContext(multiTenantContextAccessor, options)
{
    public DbSet<AppTenantInfo> AppTenantInfo => Set<AppTenantInfo>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserTenantMembership> UserTenantMemberships => Set<UserTenantMembership>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Container> Containers => Set<Container>();
    public DbSet<Template> Templates => Set<Template>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<Stage> Stages => Set<Stage>();
    public DbSet<TemplateVersion> TemplateVersions => Set<TemplateVersion>();
    public DbSet<TemplateStageAssignment> TemplateStageAssignments => Set<TemplateStageAssignment>();
    public DbSet<ApiKey> ApiKeys => Set<ApiKey>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var tenant = multiTenantContextAccessor.MultiTenantContext;

        Console.WriteLine("shit");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var tenant = multiTenantContextAccessor.MultiTenantContext;

        modelBuilder.Entity<AppTenantInfo>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.HasIndex(t => t.OwnerUserId);
            entity.HasIndex(t => t.Name).IsUnique();
        });

        modelBuilder.Entity<UserTenantMembership>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasIndex(x => new { x.UserId, x.TenantId })
                .IsUnique();

            entity.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Tenant)
                .WithMany()
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => x.UserId);
            entity.HasIndex(x => x.TenantId);
        });

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
                .WithMany(pr => pr.Permissions)
                .HasForeignKey(p => p.EntityId)
                .HasPrincipalKey(pr => pr.Id)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.Tenant)
                .WithMany()
                .HasForeignKey(p => p.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.IsMultiTenant();
        });
        //var permissionKey = modelBuilder.Entity<Permission>().Metadata.GetKeys().First();
        //modelBuilder.Entity<Permission>().IsMultiTenant().AdjustKey(permissionKey, modelBuilder).AdjustIndexes();

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
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.Tenant)
                .WithMany()
                .HasForeignKey(p => p.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.IsMultiTenant();
        });
        //var containerKey = modelBuilder.Entity<Container>().Metadata.GetKeys().First();
        //modelBuilder.Entity<Container>().IsMultiTenant().AdjustKey(containerKey, modelBuilder).AdjustIndexes();

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

            entity.HasOne(p => p.Tenant)
                .WithMany()
                .HasForeignKey(p => p.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.IsMultiTenant();
        });
        //var templateKey = modelBuilder.Entity<Template>().Metadata.GetKeys().First();
        //modelBuilder.Entity<Template>().IsMultiTenant().AdjustKey(templateKey, modelBuilder).AdjustIndexes();

        modelBuilder.Entity<TemplateVersion>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(tv => tv.Template)
                .WithMany(t => t.Versions)
                .HasForeignKey(tv => tv.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.Tenant)
                .WithMany()
                .HasForeignKey(p => p.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.IsMultiTenant();
        });
        //var templateVersionKey = modelBuilder.Entity<TemplateVersion>().Metadata.GetKeys().First();
        //modelBuilder.Entity<TemplateVersion>().IsMultiTenant().AdjustKey(templateVersionKey, modelBuilder).AdjustIndexes();

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

            entity.HasOne(p => p.Tenant)
                .WithMany()
                .HasForeignKey(p => p.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.IsMultiTenant();
        });
        //var templateStageAssignmentKey = modelBuilder.Entity<TemplateStageAssignment>().Metadata.GetKeys().First();
        //modelBuilder.Entity<TemplateStageAssignment>().IsMultiTenant().AdjustKey(templateStageAssignmentKey, modelBuilder).AdjustIndexes();

        modelBuilder.Entity<Stage>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(s => s.Project)
                .WithMany(p => p.Stages)
                .HasForeignKey(s => s.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.ProjectId, e.Name }).IsUnique();

            entity.HasOne(p => p.Tenant)
                .WithMany()
                .HasForeignKey(p => p.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.IsMultiTenant();
        });
        //var stageKey = modelBuilder.Entity<Stage>().Metadata.GetKeys().First();
        //modelBuilder.Entity<Stage>().IsMultiTenant().AdjustKey(stageKey, modelBuilder).AdjustIndexes();

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(p => p.Tenant)
                .WithMany()
                .HasForeignKey(p => p.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.IsMultiTenant();
        });
        //var projectKey = modelBuilder.Entity<Project>().Metadata.GetKeys().First();
        //modelBuilder.Entity<Project>().IsMultiTenant().AdjustKey(projectKey, modelBuilder).AdjustIndexes();

        modelBuilder.Entity<ApiKey>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.KeyHash).IsRequired();

            entity.HasOne(e => e.Project)
                .WithMany()
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.Scopes)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null));

            entity.HasOne(p => p.Tenant)
                .WithMany()
                .HasForeignKey(p => p.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.IsMultiTenant();
        });
        //var apiKeyKey = modelBuilder.Entity<ApiKey>().Metadata.GetKeys().First();
        //modelBuilder.Entity<ApiKey>().IsMultiTenant().AdjustKey(apiKeyKey, modelBuilder).AdjustIndexes();
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            if (entry.Entity is IEntity entity)
            {
                entity.UpdatedDateTime = now;

                if (entry.State == EntityState.Added)
                {
                    entity.Id = Guid.CreateVersion7();
                    entity.CreatedDateTime = now;
                }
            }

            if (entry.Entity is ITenantInfo tenant)
            {
                if (entry.State == EntityState.Added)
                {
                    if (string.IsNullOrWhiteSpace(tenant.Id))
                    {
                        tenant.Id = Guid.CreateVersion7().ToString();
                    }
                }

                if (entry.Entity is AppTenantInfo appTenant)
                {
                    if (entry.State == EntityState.Added)
                    {
                        appTenant.CreatedDateTime = now;
                    }

                    appTenant.UpdatedDateTime = now;
                }
            }
        }

        this.EnforceMultiTenant();

        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            if (entry.Entity is IEntity entity)
            {
                entity.UpdatedDateTime = now;

                if (entry.State == EntityState.Added)
                {
                    entity.Id = Guid.CreateVersion7();
                    entity.CreatedDateTime = now;
                }
            }

            if (entry.Entity is ITenantInfo tenant)
            {
                if (entry.State == EntityState.Added)
                {
                    if (string.IsNullOrWhiteSpace(tenant.Id))
                    {
                        tenant.Id = Guid.CreateVersion7().ToString();
                    }
                }

                if (entry.Entity is AppTenantInfo appTenant)
                {
                    if (entry.State == EntityState.Added)
                    {
                        appTenant.CreatedDateTime = now;
                    }
                    appTenant.UpdatedDateTime = now;
                }
            }
        }

        this.EnforceMultiTenant();

        return await base.SaveChangesAsync(cancellationToken);
    }
}
