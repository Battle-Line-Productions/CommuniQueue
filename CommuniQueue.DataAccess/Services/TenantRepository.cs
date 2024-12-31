#region Copyright
// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
//
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
//
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 12/29/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.DataAccess
// File: TenantRepository.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.DataAccess\Services\TenantRepository.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.Contracts.Interfaces.Repositories;
using CommuniQueue.Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace CommuniQueue.DataAccess.Services;

public class TenantRepository(AppDbContext context) : ITenantRepository
{
    public async Task<List<AppTenantInfo>> GetTenantsByUserId(Guid userId)
    {
        return await context.AppTenantInfo
            .Where(tenant => tenant.UserTenantMemberships
                .Any(membership => membership.UserId == userId))
            .ToListAsync();
    }

    public async Task<AppTenantInfo?> GetTenantById(string id)
    {
        return await context.AppTenantInfo.Include(x => x.UserTenantMemberships).SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<AppTenantInfo?> GetTenantByIdentifier(string identifier)
    {
        return await context.AppTenantInfo.SingleOrDefaultAsync(x => x.Identifier == identifier);
    }

    public async Task<AppTenantInfo?> GetTenantByName(string name)
    {
        return await context.AppTenantInfo.SingleOrDefaultAsync(x => x.Name == name);
    }

    public async Task<AppTenantInfo> CreateTenant(AppTenantInfo tenant)
    {
        await context.AppTenantInfo.AddAsync(tenant);
        await context.SaveChangesAsync();

        return tenant;
    }

    public async Task<AppTenantInfo> UpdateTenant(AppTenantInfo tenant)
    {
        context.AppTenantInfo.Update(tenant);
        await context.SaveChangesAsync();

        return tenant;
    }
}
