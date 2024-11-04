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
// File: TemplateStageAssignmentRepository.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.DataAccess\Services\TemplateStageAssignmentRepository.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.Contracts.Interfaces;
using CommuniQueue.Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace CommuniQueue.DataAccess.Services;

public class TemplateStageAssignmentRepository(AppDbContext context) : ITemplateStageAssignmentRepository
{
    public async Task<TemplateStageAssignment> CreateAsync(TemplateStageAssignment assignment)
    {
        await context.TemplateStageAssignments.AddAsync(assignment);
        await context.SaveChangesAsync();
        return assignment;
    }

    public async Task DeleteAsync(Guid assignmentId)
    {
        var assignment = await context.TemplateStageAssignments.FindAsync(assignmentId);
        if (assignment != null)
        {
            context.TemplateStageAssignments.Remove(assignment);
            await context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<TemplateStageAssignment>> GetByStageIdAsync(Guid stageId)
    {
        return await context.TemplateStageAssignments
            .Where(tsa => tsa.StageId == stageId)
            .ToListAsync();
    }

    public async Task<TemplateStageAssignment> GetByStageAndTemplateVersionIdAsync(Guid stageId, Guid templateVersionId)
    {
        return await context.TemplateStageAssignments
            .FirstOrDefaultAsync(tsa => tsa.StageId == stageId && tsa.TemplateVersionId == templateVersionId);
    }

    public async Task<bool> ExistsForStageAndTemplateVersionAsync(Guid stageId, Guid templateVersionId)
    {
        return await context.TemplateStageAssignments
            .AnyAsync(tsa => tsa.StageId == stageId && tsa.TemplateVersionId == templateVersionId);
    }
}
