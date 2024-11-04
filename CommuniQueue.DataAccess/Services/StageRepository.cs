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
// File: StageRepository.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.DataAccess\Services\StageRepository.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.Contracts.Interfaces;
using CommuniQueue.Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace CommuniQueue.DataAccess.Services;

public class StageRepository(AppDbContext context) : IStageRepository
{
    public async Task<Stage> CreateAsync(Stage stage)
    {
        context.Stages.Add(stage);
        await context.SaveChangesAsync();
        return stage;
    }

    public async Task<Stage?> GetByIdAsync(Guid stageId)
    {
        return await context.Stages.FindAsync(stageId);
    }

    public async Task<IEnumerable<Stage>> GetByProjectIdAsync(Guid projectId)
    {
        return await context.Stages
            .Where(s => s.ProjectId == projectId)
            .OrderBy(s => s.Order)
            .ToListAsync();
    }

    public async Task<Stage> UpdateAsync(Stage stage)
    {
        context.Stages.Update(stage);
        await context.SaveChangesAsync();
        return stage;
    }

    public async Task DeleteAsync(Guid stageId)
    {
        var stage = await context.Stages.FindAsync(stageId);
        if (stage != null)
        {
            context.Stages.Remove(stage);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid stageId)
    {
        return await context.Stages.AnyAsync(s => s.Id == stageId);
    }
}
