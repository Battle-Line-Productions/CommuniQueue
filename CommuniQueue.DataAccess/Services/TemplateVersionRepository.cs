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
// File: TemplateVersionRepository.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.DataAccess\Services\TemplateVersionRepository.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.Contracts.Interfaces.Repositories;
using CommuniQueue.Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace CommuniQueue.DataAccess.Services;

public class TemplateVersionRepository(AppDbContext context) : BaseRepository<TemplateVersion>(context), ITemplateVersionRepository
{
    public async Task<IEnumerable<TemplateVersion>> GetByTemplateIdAsync(Guid templateId)
    {
        return await context.TemplateVersions
            .Where(tv => tv.TemplateId == templateId)
            .OrderByDescending(tv => tv.VersionNumber)
        .ToListAsync();
    }

    public async Task<TemplateVersion> GetLatestVersionAsync(Guid templateId)
    {
        return await context.TemplateVersions
            .Where(tv => tv.TemplateId == templateId)
            .OrderByDescending(tv => tv.VersionNumber)
            .FirstAsync();
    }
}
