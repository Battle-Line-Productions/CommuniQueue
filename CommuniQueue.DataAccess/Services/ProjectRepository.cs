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

using CommuniQueue.Contracts.Interfaces;
using CommuniQueue.Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace CommuniQueue.DataAccess.Services;

public class ProjectRepository(AppDbContext context) : IProjectRepository
{
    public async Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> operation)
    {
        var strategy = context.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var result = await operation();
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        });
    }

    public async Task<Project> CreateAsync(Project project)
    {
        context.Projects.Add(project);
        await context.SaveChangesAsync();
        return project;
    }

    public async Task<Project?> GetByIdAsync(Guid projectId)
    {
        return await context.Projects.FindAsync(projectId);
    }

    public async Task<IEnumerable<Project?>> GetByUserIdAsync(Guid userId)
    {
        return await context.Permissions
            .Where(p => p.UserId == userId && p.EntityType == EntityType.Project)
            .Include(p => p.Project)
            .Select(p => p.Project)
            .ToListAsync();
    }

    public async Task<Project> UpdateAsync(Project project)
    {
        context.Projects.Update(project);
        await context.SaveChangesAsync();
        return project;
    }

    public async Task DeleteAsync(Guid projectId)
    {
        var project = await context.Projects.FindAsync(projectId);
        if (project != null)
        {
            context.Projects.Remove(project);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid projectId)
    {
        return await context.Projects.AnyAsync(p => p.Id == projectId);
    }
}
