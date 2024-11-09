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

using CommuniQueue.Contracts.Interfaces;
using CommuniQueue.Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace CommuniQueue.DataAccess.Services;

public class ApiKeyRepository(AppDbContext context) : IApiKeyRepository
{
    public async Task<ApiKey> CreateAsync(ApiKey apiKey)
    {
        context.ApiKeys.Add(apiKey);
        await context.SaveChangesAsync();
        return apiKey;
    }

    public async Task<ApiKey?> GetByIdAsync(Guid apiKeyId)
    {
        return await context.ApiKeys.FindAsync(apiKeyId);
    }

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

    public async Task<ApiKey> UpdateAsync(ApiKey apiKey)
    {
        context.ApiKeys.Update(apiKey);
        await context.SaveChangesAsync();
        return apiKey;
    }

    public async Task DeleteAsync(Guid apiKeyId)
    {
        var apiKey = await context.ApiKeys.FindAsync(apiKeyId);
        if (apiKey != null)
        {
            context.ApiKeys.Remove(apiKey);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid apiKeyId)
    {
        return await context.ApiKeys.AnyAsync(ak => ak.Id == apiKeyId);
    }
}
