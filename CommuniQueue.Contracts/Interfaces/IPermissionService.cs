#region Copyright
// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
//
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
//
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 11/23/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.Contracts
// File: IPermissionService.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Interfaces\IPermissionService.cs
// ---------------------------------------------------------------------------
#endregion

using BattlelineExtras.Contracts.Models;
using CommuniQueue.Contracts.Models;

namespace CommuniQueue.Contracts.Interfaces;

public interface IPermissionService
{
    Task<ResponseDetail<Permission?>> CreatePermissionAsync(Guid userId, Guid entityId, EntityType entityType, PermissionLevel permissionLevel, string requesterSsoId);
    Task<ResponseDetail<Permission>> GetPermissionAsync(Guid userId, Guid entityId, EntityType entityType);
    Task<ResponseDetail<List<Permission>>> GetPermissionsByEntityAsync(Guid entityId, EntityType entityType);
    Task<ResponseDetail<Permission>> UpdatePermissionAsync(Guid userId, Guid entityId, EntityType entityType, PermissionLevel newPermissionLevel, string requesterSsoId);
    Task<ResponseDetail<bool>> DeletePermissionAsync(Guid userId, Guid entityId, EntityType entityType, string requesterSsoId);
}

