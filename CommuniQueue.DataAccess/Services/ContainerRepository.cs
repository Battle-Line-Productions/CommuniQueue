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

using CommuniQueue.Contracts.Interfaces.Repositories;
using CommuniQueue.Contracts.Models;
using Microsoft.EntityFrameworkCore;

#endregion

namespace CommuniQueue.DataAccess.Services;

public class ContainerRepository(AppDbContext context) : BaseRepository<Container>(context), IContainerRepository
{
    public async Task<IEnumerable<Container>> GetByProjectIdAsync(Guid projectId)
    {
        return await context.Containers
            .Where(c => c.ProjectId == projectId)
            .ToListAsync();
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
}
