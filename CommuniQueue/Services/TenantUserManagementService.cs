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
// Project Name: CommuniQueue
// File: TenantUserManagementService.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue\Services\TenantUserManagementService.cs
// ---------------------------------------------------------------------------

#endregion

using BattlelineExtras.Contracts.Models;
using CommuniQueue.Contracts.Interfaces.Repositories;

namespace CommuniQueue.Services;

public class TenantUserManagementService(IUserRepository userRepo, ITenantRepository tenantRepo)
    : ITenantUserManagementService
{
    private const string SubCode = "TenantUserManagementService";

    public async Task<ResponseDetail<AppTenantInfo>> AddUserToTenant(Guid userId, string tenantId)
    {
        var user = await userRepo.GetByIdAsync(userId);

        if (user == null)
        {
            var responseDetail = new ResponseDetail<AppTenantInfo>
            {
                SubCode = SubCode,
                Title = "AddUserToTenant",
                Data = null,
                Status = ResultStatus.NotFound404,
                ErrorDetails =
                [
                    new()
                    {
                        Instance = "MissingUser",
                        Detail = $"User by ID {userId} not found"
                    }
                ]
            };

            return responseDetail;
        }

        var tenant = await tenantRepo.GetTenantById(tenantId);

        if (tenant == null)
        {
            var responseDetail = new ResponseDetail<AppTenantInfo>
            {
                SubCode = SubCode,
                Title = "AddUserToTenant",
                Data = null,
                Status = ResultStatus.NotFound404,
                ErrorDetails =
                [
                    new()
                    {
                        Instance = "MissingTenant",
                        Detail = $"Tenant with Id {tenantId} not found"
                    }
                ]
            };

            return responseDetail;
        }

        var now = DateTime.UtcNow;

        var tenantUser = new UserTenantMembership
        {
            Id = Guid.CreateVersion7(),
            TenantId = tenantId,
            UserId = user.Id,
            Tenant = tenant,
            User = user,
            CreatedDateTime = now,
            UpdatedDateTime = now
        };

        tenant.UserTenantMemberships.Add(tenantUser);

        await tenantRepo.UpdateTenant(tenant);

        return new ResponseDetail<AppTenantInfo>
        {
            SubCode = SubCode,
            Title = "AddUserToTenant",
            Data = tenant,
            Status = ResultStatus.Ok200
        };
    }

    public async Task<ResponseDetail<AppTenantInfo>> RemoveUserFromTenant(Guid userId, string tenantId)
    {
        var user = await userRepo.GetByIdAsync(userId);

        if (user == null)
        {
            return new ResponseDetail<AppTenantInfo>
            {
                SubCode = SubCode,
                Title = "RemoveUserFromTenant",
                Data = null,
                Status = ResultStatus.NotFound404,
                ErrorDetails =
                [
                    new()
                    {
                        Instance = "MissingUser",
                        Detail = $"User with ID {userId} not found."
                    }
                ]
            };
        }

        var tenant = await tenantRepo.GetTenantById(tenantId);

        if (tenant == null)
        {
            return new ResponseDetail<AppTenantInfo>
            {
                SubCode = SubCode,
                Title = "RemoveUserFromTenant",
                Data = null,
                Status = ResultStatus.NotFound404,
                ErrorDetails =
                [
                    new()
                    {
                        Instance = "MissingTenant",
                        Detail = $"Tenant with ID {tenantId} not found."
                    }
                ]
            };
        }

        var membership = tenant.UserTenantMemberships
            .FirstOrDefault(m => m.UserId == user.Id);

        if (membership == null)
        {
            return new ResponseDetail<AppTenantInfo>
            {
                SubCode = SubCode,
                Title = "RemoveUserFromTenant",
                Data = null,
                Status = ResultStatus.NotFound404,
                ErrorDetails =
                [
                    new()
                    {
                        Instance = "MissingMembership",
                        Detail = $"No membership found for User {userId} in Tenant {tenantId}."
                    }
                ]
            };
        }

        tenant.UserTenantMemberships.Remove(membership);

        await tenantRepo.UpdateTenant(tenant);

        return new ResponseDetail<AppTenantInfo>
        {
            SubCode = SubCode,
            Title = "RemoveUserFromTenant",
            Data = tenant,
            Status = ResultStatus.Ok200
        };
    }
}
