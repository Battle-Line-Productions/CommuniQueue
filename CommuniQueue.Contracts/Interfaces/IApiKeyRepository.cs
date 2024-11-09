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
// Project Name: CommuniQueue.Contracts
// File: IApiKeyRepository.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Interfaces\IApiKeyRepository.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.Contracts.Models;

namespace CommuniQueue.Contracts.Interfaces;

public interface IApiKeyRepository
{
    Task<ApiKey> CreateAsync(ApiKey apiKey);
    Task<ApiKey?> GetByIdAsync(Guid apiKeyId);
    Task<ApiKey?> GetByHashAsync(string keyHash);
    Task<IEnumerable<ApiKey>> GetByProjectIdAsync(Guid projectId);
    Task<ApiKey> UpdateAsync(ApiKey apiKey);
    Task DeleteAsync(Guid apiKeyId);
    Task<bool> ExistsAsync(Guid apiKeyId);
}
