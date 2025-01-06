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
// File: ProjectRepository.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.DataAccess\Services\ProjectRepository.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.Contracts.Interfaces.Repositories;
using CommuniQueue.Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace CommuniQueue.DataAccess.Services;

public class ProjectRepository(AppDbContext context) : BaseRepository<Project>(context), IProjectRepository
{
    private readonly AppDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<IEnumerable<Project>> GetByUserIdAsync(Guid userId)
    {
        var query = _context.Permissions
            .Where(x => x.EntityType == EntityType.Project && x.UserId == userId);

        var matchingPermissions = await query.ToListAsync();

        var entityIdsFromPermissions = matchingPermissions.Select(x => x.EntityId).ToList();

        if (entityIdsFromPermissions.Count == 0)
        {
            return [];
        }

        var projects = await _context.Projects
            .Where(x => entityIdsFromPermissions.Contains(x.Id))
            .ToListAsync();

        return projects;
    }
}
