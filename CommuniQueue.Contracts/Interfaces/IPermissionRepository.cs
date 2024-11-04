#region Copyright
// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
// 
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
// 
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 10/14/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.Contracts
// File: IPermissionRepository.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Interfaces\IPermissionRepository.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.Contracts.Models;

namespace CommuniQueue.Contracts.Interfaces;

public interface IPermissionRepository
{
    Task<Permission> CreateAsync(Permission permission);
    Task<Permission?> GetAsync(Guid userId, Guid entityId, EntityType entityType);
    Task<Permission> UpdateAsync(Permission permission);
    Task DeleteAsync(Guid userId, Guid entityId, EntityType entityType);
    Task<bool> ExistsAsync(Guid userId, Guid entityId, EntityType entityType);
}
