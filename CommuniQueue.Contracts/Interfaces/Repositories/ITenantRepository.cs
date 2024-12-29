#region Copyright
// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
// 
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
// 
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 12/29/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.Contracts
// File: ITenantRepository.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Interfaces\Repositories\ITenantRepository.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.Contracts.Models;

namespace CommuniQueue.Contracts.Interfaces.Repositories;

public interface ITenantRepository
{
    Task<List<AppTenantInfo>> GetTenantsByUserId(Guid userId);
    Task<AppTenantInfo?> GetTenantById(string id);
    Task<AppTenantInfo?> GetTenantByIdentifier(string identifier);
    Task<AppTenantInfo?> GetTenantByName(string name);
    Task<AppTenantInfo> CreateTenant(AppTenantInfo tenant);
}
