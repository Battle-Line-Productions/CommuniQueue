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
// File: ContainerRepository.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.DataAccess\Services\ContainerRepository.cs
// ---------------------------------------------------------------------------

#endregion

#region Usings

using CommuniQueue.Contracts.Interfaces;
using CommuniQueue.Contracts.Models;
using Microsoft.EntityFrameworkCore;

#endregion

namespace CommuniQueue.DataAccess.Services;

public class ContainerRepository(AppDbContext context) : IContainerRepository
{
    public async Task<Container> CreateAsync(Container container)
    {
        context.Containers.Add(container);
        await context.SaveChangesAsync();
        return container;
    }

    public async Task<Container?> GetByIdAsync(Guid containerId)
    {
        return await context.Containers.FindAsync(containerId);
    }

    public async Task<IEnumerable<Container>> GetByProjectIdAsync(Guid projectId)
    {
        return await context.Containers
            .Where(c => c.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<Container> UpdateAsync(Container container)
    {
        context.Containers.Update(container);
        await context.SaveChangesAsync();
        return container;
    }

    public async Task DeleteAsync(Guid containerId)
    {
        var container = await context.Containers.FindAsync(containerId);
        if (container != null)
        {
            context.Containers.Remove(container);
            await context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Container>> GetChildrenAsync(Guid parentContainerId)
    {
        return await context.Containers
            .Where(c => c.ParentId == parentContainerId)
            .ToListAsync();
    }

    public async Task<Container> GetRootContainerForProjectAsync(Guid projectId)
    {
        return await context.Containers
            .FirstAsync(c => c.ProjectId == projectId && c.IsRoot);
    }

    public async Task<bool> ExistsAsync(Guid containerId)
    {
        return await context.Containers.AnyAsync(c => c.Id == containerId);
    }
}
