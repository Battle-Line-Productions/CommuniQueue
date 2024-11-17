#region Copyright
// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
// 
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
// 
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 10/14/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.DataAccess
// File: PermissionRepository.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.DataAccess\Services\PermissionRepository.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.Contracts.Interfaces;
using CommuniQueue.Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace CommuniQueue.DataAccess.Services;

public class PermissionRepository(AppDbContext context) : IPermissionRepository
{
    public async Task<Permission> CreateAsync(Permission permission)
    {
        context.Permissions.Add(permission);
        await context.SaveChangesAsync();
        return permission;
    }

    public async Task<Permission?> GetAsync(Guid userId, Guid entityId, EntityType entityType)
    {
        return await context.Permissions
            .FirstOrDefaultAsync(p => p.UserId == userId && p.EntityId == entityId && p.EntityType == entityType);
    }

    public async Task<List<Permission>> ListEntityTypeById(Guid entityId, EntityType entityType)
    {
        return await context.Permissions
            .Where(p => p.EntityId == entityId && p.EntityType == entityType).ToListAsync();
    }

    public async Task<Permission> UpdateAsync(Permission permission)
    {
        context.Permissions.Update(permission);
        await context.SaveChangesAsync();
        return permission;
    }

    public async Task DeleteAsync(Guid userId, Guid entityId, EntityType entityType)
    {
        var permission = await GetAsync(userId, entityId, entityType);
        if (permission != null)
        {
            context.Permissions.Remove(permission);
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(Permission permission)
    {
        context.Permissions.Remove(permission);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid entityId, EntityType entityType)
    {
        return await context.Permissions.AnyAsync(p =>
            p.UserId == userId && p.EntityId == entityId && p.EntityType == entityType);
    }
}
