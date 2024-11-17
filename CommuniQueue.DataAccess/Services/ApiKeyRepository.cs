#region Copyright
// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
// 
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
// 
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 11/05/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.DataAccess
// File: ApiKeyRepository.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.DataAccess\Services\ApiKeyRepository.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.Contracts.Interfaces.Repositories;
using CommuniQueue.Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace CommuniQueue.DataAccess.Services;

public class ApiKeyRepository(AppDbContext context) : BaseRepository<ApiKey>(context), IApiKeyRepository
{
    public async Task<ApiKey?> GetByHashAsync(string keyHash)
    {
        return await context.ApiKeys.FirstOrDefaultAsync(ak => ak.KeyHash == keyHash);
    }

    public async Task<IEnumerable<ApiKey>> GetByProjectIdAsync(Guid projectId)
    {
        return await context.ApiKeys
            .Where(ak => ak.ProjectId == projectId)
            .ToListAsync();
    }
}
