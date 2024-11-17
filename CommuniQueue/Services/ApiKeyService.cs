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
// Project Name: CommuniQueue
// File: ApiKeyService.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue\Services\ApiKeyService.cs
// ---------------------------------------------------------------------------
#endregion

using System.Security.Cryptography;
using BattlelineExtras.Contracts.Extensions;
using BattlelineExtras.Contracts.Models;
using BattlelineExtras.Services.Crypto;
using CommuniQueue.Contracts.Interfaces.Repositories;

namespace CommuniQueue.Services;

public class ApiKeyService(
    IApiKeyRepository apiKeyRepository,
    IProjectRepository projectRepository,
    IHashing hashingService)
    : IApiKeyService
{
    private const string SubCode = "ApiKeyService";

    public async Task<ResponseDetail<(ApiKey?, string?)>> GenerateApiKeyAsync(Guid projectId, DateTime startDate, DateTime endDate, List<string> scopes)
    {
        if (!await projectRepository.ExistsAsync(projectId))
        {
            return ((ApiKey?)null, (string?)null).BuildResponseDetail(ResultStatus.NotFound404, "Generate API Key", SubCode)
                .AddErrorDetail("GenerateApiKey", $"Project with ID {projectId} not found");
        }

        var apiKeyValue = GenerateUniqueApiKey();
        var apiKeyHash = hashingService.GetBase64Hash(apiKeyValue);

        var apiKey = new ApiKey
        {
            KeyHash = apiKeyHash,
            ProjectId = projectId,
            StartDate = startDate,
            EndDate = endDate,
            Scopes = scopes,
            IsExpired = false
        };

        apiKey = await apiKeyRepository.CreateAsync(apiKey);
        return (apiKey, apiKeyValue).BuildResponseDetail(ResultStatus.Created201, "Generate API Key", SubCode);
    }

    public async Task<ResponseDetail<ApiKey>> GetApiKeyByIdAsync(Guid apiKeyId)
    {
        var apiKey = await apiKeyRepository.GetByIdAsync(apiKeyId);
        if (apiKey == null)
        {
            return ((ApiKey)null).BuildResponseDetail(ResultStatus.NotFound404, "Get API Key", SubCode)
                .AddErrorDetail("GetApiKey", $"API Key with ID {apiKeyId} not found");
        }
        return apiKey.BuildResponseDetail(ResultStatus.Ok200, "Get API Key", SubCode);
    }

    public async Task<ResponseDetail<List<ApiKey>>> GetApiKeysByProjectIdAsync(Guid projectId)
    {
        if (!await projectRepository.ExistsAsync(projectId))
        {
            return new List<ApiKey>().BuildResponseDetail(ResultStatus.NotFound404, "Get API Keys by Project ID", SubCode)
                .AddErrorDetail("GetApiKeysByProjectId", $"Project with ID {projectId} not found");
        }

        var apiKeys = await apiKeyRepository.GetByProjectIdAsync(projectId);
        return apiKeys.ToList().BuildResponseDetail(ResultStatus.Ok200, "Get API Keys by Project ID", SubCode);
    }

    public async Task<ResponseDetail<bool>> ValidateApiKeyAsync(string apiKey)
    {
        var keyHash = hashingService.GetBase64Hash(apiKey);
        var storedApiKey = await apiKeyRepository.GetByHashAsync(keyHash);
        if (storedApiKey == null || storedApiKey.IsExpired || DateTime.UtcNow < storedApiKey.StartDate || DateTime.UtcNow > storedApiKey.EndDate)
        {
            return false.BuildResponseDetail(ResultStatus.NotAuthorized401, "Validate API Key", SubCode)
                .AddErrorDetail("ValidateApiKey", "Invalid or expired API key");
        }
        return true.BuildResponseDetail(ResultStatus.Ok200, "Validate API Key", SubCode);
    }

    public async Task<ResponseDetail<bool>> ExpireApiKeyAsync(Guid apiKeyId)
    {
        var apiKey = await apiKeyRepository.GetByIdAsync(apiKeyId);
        if (apiKey == null)
        {
            return false.BuildResponseDetail(ResultStatus.NotFound404, "Expire API Key", SubCode)
                .AddErrorDetail("ExpireApiKey", $"API Key with ID {apiKeyId} not found");
        }

        apiKey.IsExpired = true;
        await apiKeyRepository.UpdateAsync(apiKey);
        return true.BuildResponseDetail(ResultStatus.Ok200, "Expire API Key", SubCode);
    }

    public async Task<ResponseDetail<bool>> HasValidApiKeyAsync(Guid projectId)
    {
        var apiKeys = await apiKeyRepository.GetByProjectIdAsync(projectId);
        var hasValidKey = apiKeys.Any(ak => !ak.IsExpired && DateTime.UtcNow >= ak.StartDate && DateTime.UtcNow <= ak.EndDate);
        return hasValidKey.BuildResponseDetail(ResultStatus.Ok200, "Check Valid API Key", SubCode);
    }

    private static string GenerateUniqueApiKey()
    {
        const int actualKeyLength = 128;

        using var randomNumberGenerator = RandomNumberGenerator.Create();
        var bytes = new byte[actualKeyLength];
        randomNumberGenerator.GetBytes(bytes);

        var base64 = Convert.ToBase64String(bytes);
        var sanitizedKey = base64.Replace("/", "").Replace("+", "").Replace("=", "");

        var key = sanitizedKey[..Math.Min(actualKeyLength, sanitizedKey.Length)];
        return key;
    }
}
