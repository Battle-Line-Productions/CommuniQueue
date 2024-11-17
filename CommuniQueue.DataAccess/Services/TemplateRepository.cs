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
// File: TemplateRepository.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.DataAccess\Services\TemplateRepository.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.Contracts.Interfaces.Repositories;
using CommuniQueue.Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace CommuniQueue.DataAccess.Services;

public class TemplateRepository(AppDbContext context) : BaseRepository<Template>(context), ITemplateRepository
{
    public async Task<IEnumerable<Template>> GetByProjectIdAsync(Guid projectId)
    {
        return await context.Templates
            .Where(t => t.ProjectId == projectId)
        .ToListAsync();
    }

    public async Task<IEnumerable<Template>> GetByContainerIdAsync(Guid containerId)
    {
        return await context.Templates
            .Where(t => t.ContainerId == containerId)
            .ToListAsync();
    }
}
