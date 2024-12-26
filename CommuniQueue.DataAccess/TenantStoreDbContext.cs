#region Copyright
// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
//
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
//
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 12/24/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.DataAccess
// File: TenantStoreDbContext.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.DataAccess\TenantStoreDbContext.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.Contracts.Models;
using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;
using Microsoft.EntityFrameworkCore;

namespace CommuniQueue.DataAccess;

public class TenantStoreDbContext(DbContextOptions<TenantStoreDbContext> options)
    : EFCoreStoreDbContext<AppTenantInfo>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppTenantInfo>().ToTable("app_tenant_info");
    }
}
