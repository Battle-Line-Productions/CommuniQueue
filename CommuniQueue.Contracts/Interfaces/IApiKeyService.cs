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
// File: IApiKeyService.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Interfaces\IApiKeyService.cs
// ---------------------------------------------------------------------------
#endregion

using BattlelineExtras.Contracts.Models;
using CommuniQueue.Contracts.Models;

namespace CommuniQueue.Contracts.Interfaces;

public interface IApiKeyService
{
    Task<ResponseDetail<(ApiKey?, string?)>> GenerateApiKeyAsync(Guid projectId, DateTime startDate, DateTime endDate, List<string> scopes);
    Task<ResponseDetail<ApiKey>> GetApiKeyByIdAsync(Guid apiKeyId);
    Task<ResponseDetail<List<ApiKey>>> GetApiKeysByProjectIdAsync(Guid projectId);
    Task<ResponseDetail<bool>> ValidateApiKeyAsync(string apiKey);
    Task<ResponseDetail<bool>> ExpireApiKeyAsync(Guid apiKeyId);
    Task<ResponseDetail<bool>> HasValidApiKeyAsync(Guid projectId);
}
