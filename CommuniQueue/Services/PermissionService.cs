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
// Project Name: CommuniQueue
// File: PermissionService.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue\Services\PermissionService.cs
// ---------------------------------------------------------------------------
#endregion

using BattlelineExtras.Contracts.Extensions;
using BattlelineExtras.Contracts.Models;
using CommuniQueue.Contracts.Interfaces.Repositories;

namespace CommuniQueue.Services;

public class PermissionService(IPermissionRepository permissionRepository, IUserRepository userRepository)
    : IPermissionService
{
    private const string SubCode = "PermissionService";

    public async Task<ResponseDetail<Permission?>> CreatePermissionAsync(Guid userId, Guid entityId, EntityType entityType, PermissionLevel permissionLevel, string requesterSsoId)
    {
        var user = await userRepository.GetBySsoIdAsync(requesterSsoId);

        if (user == null)
        {
            return ((Permission)null).BuildResponseDetail(ResultStatus.NotFound404, "Create Permission", SubCode)
                .AddErrorDetail("CreatePermission", $"User with SSOID {requesterSsoId} not found");
        }

        if (!await userRepository.ExistsAsync(userId))
        {
            return ((Permission)null).BuildResponseDetail(ResultStatus.NotFound404, "Create Permission", SubCode)
                .AddErrorDetail("CreatePermission", $"User with ID {userId} not found");
        }

        var existingPermission = await permissionRepository.GetAsync(userId, entityId, entityType);
        if (existingPermission != null)
        {
            return ((Permission)null).BuildResponseDetail(ResultStatus.Conflict409, "Create Permission", SubCode)
                .AddErrorDetail("CreatePermission", "Permission already exists");
        }

        var newPermission = new Permission
        {
            UserId = userId,
            EntityId = entityId,
            EntityType = entityType,
            PermissionLevel = permissionLevel
        };

        var createdPermission = await permissionRepository.CreateAsync(newPermission);
        return createdPermission.BuildResponseDetail(ResultStatus.Created201, "Create Permission", SubCode);
    }

    public async Task<ResponseDetail<Permission>> GetPermissionAsync(Guid userId, Guid entityId, EntityType entityType)
    {
        var permission = await permissionRepository.GetAsync(userId, entityId, entityType);
        if (permission == null)
        {
            return ((Permission)null).BuildResponseDetail(ResultStatus.NotFound404, "Get Permission", SubCode)
                .AddErrorDetail("GetPermission", "Permission not found");
        }
        return permission.BuildResponseDetail(ResultStatus.Ok200, "Get Permission", SubCode);
    }

    public async Task<ResponseDetail<List<Permission>>> GetPermissionsByEntityAsync(Guid entityId, EntityType entityType)
    {
        var permissions = await permissionRepository.ListEntityTypeById(entityId, entityType);
        return permissions.BuildResponseDetail(ResultStatus.Ok200, "Get Permissions by Entity", SubCode);
    }

    public async Task<ResponseDetail<Permission>> UpdatePermissionAsync(Guid userId, Guid entityId, EntityType entityType, PermissionLevel newPermissionLevel, string requesterSsoId)
    {
        var user = await userRepository.GetBySsoIdAsync(requesterSsoId);

        if (user == null)
        {
            return ((Permission)null).BuildResponseDetail(ResultStatus.NotFound404, "Update Permission", SubCode)
                .AddErrorDetail("UpdatePermission", $"User with SSOID {requesterSsoId} not found");
        }

        var permission = await permissionRepository.GetAsync(userId, entityId, entityType);
        if (permission == null)
        {
            return ((Permission)null).BuildResponseDetail(ResultStatus.NotFound404, "Update Permission", SubCode)
                .AddErrorDetail("UpdatePermission", "Permission not found");
        }

        permission.PermissionLevel = newPermissionLevel;
        var updatedPermission = await permissionRepository.UpdateAsync(permission);
        return updatedPermission.BuildResponseDetail(ResultStatus.Ok200, "Update Permission", SubCode);
    }

    public async Task<ResponseDetail<bool>> DeletePermissionAsync(Guid userId, Guid entityId, EntityType entityType, string requesterSsoId)
    {
        var user = await userRepository.GetBySsoIdAsync(requesterSsoId);

        if (user == null)
        {
            return false.BuildResponseDetail(ResultStatus.NotFound404, "Delete Permission", SubCode)
                .AddErrorDetail("DeletePermission", $"User with SSOID {requesterSsoId} not found");
        }

        if (!await permissionRepository.ExistsAsync(userId, entityId, entityType))
        {
            return false.BuildResponseDetail(ResultStatus.NotFound404, "Delete Permission", SubCode)
                .AddErrorDetail("DeletePermission", "Permission not found");
        }

        await permissionRepository.DeleteAsync(userId, entityId, entityType);
        return true.BuildResponseDetail(ResultStatus.Ok200, "Delete Permission", SubCode);
    }
}
