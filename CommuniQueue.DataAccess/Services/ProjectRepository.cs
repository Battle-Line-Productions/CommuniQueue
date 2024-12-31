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
    public async Task<IEnumerable<Project>> GetByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return [];
        }

        try
        {
            var test = await context.Permissions.ToListAsync();

            var permissions1 = context.Permissions
                .Include(permission => permission.Project)
                .Where(permission => permission.UserId == userId && permission.EntityType == EntityType.Project);
            var sql = permissions1.ToQueryString();
            var permissions = await permissions1.ToListAsync();

            if (permissions.Count == 0)
            {
                return [];
            }

            var projects = permissions.Select(permission => permission.Project).ToList();

            return projects;
        }
        catch (Exception ex)
        {
            return [];
        }
    }

}
