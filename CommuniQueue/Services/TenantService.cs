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
// File: TenantService.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue\Services\TenantService.cs
// ---------------------------------------------------------------------------
#endregion

using BattlelineExtras.Contracts.Models;
using CommuniQueue.Contracts.Interfaces.Repositories;
using CommuniQueue.Extensions;

namespace CommuniQueue.Services;

public class TenantService(IUserRepository userRepo, ITenantRepository tenantRepo) : ITenantService
{
    private const string SubCode = "TenantService";

    public async Task<ResponseDetail<AppTenantInfo>> CreateTenantAsync(string tenantName, string tenantDescription, string ssoUserId)
    {
        ResponseDetail<AppTenantInfo> responseDetail;
        try
        {
            var user = await userRepo.GetBySsoIdAsync(ssoUserId);

            if (user == null)
            {
                responseDetail = new()
                {
                    SubCode = SubCode,
                    Title = "CreateTenant",
                    Data = null,
                    Status = ResultStatus.NotFound404,
                    ErrorDetails =
                    [
                        new()
                        {
                            Instance = "MissingUser",
                            Detail = $"User by SSOId {ssoUserId} not found"
                        }
                    ]
                };

                return responseDetail;
            }

            var now = DateTime.UtcNow;

            var tenantId = Guid.CreateVersion7().ToString();

            var tenantUser = new UserTenantMembership
            {
                Id = Guid.CreateVersion7(),
                TenantId = tenantId,
                UserId = user.Id,
                CreatedDateTime = now,
                UpdatedDateTime = now,
                GlobalRole = GlobalRoleType.Owner
            };

            var tenant = new AppTenantInfo
            {
                Id = tenantId,
                Name = tenantName,
                Description = tenantDescription,
                OwnerUserId = user.Id,
                Identifier = tenantName.ToAlphanumericOnly().ToLowerInvariant(),
                CreatedDateTime = now,
                UpdatedDateTime = now,
                UserTenantMemberships = [tenantUser]
            };

            await tenantRepo.CreateTenant(tenant);

            responseDetail = new()
            {
                SubCode = SubCode,
                Title = "CreateTenant",
                Data = tenant,
                Status = ResultStatus.Created201,
            };

            return responseDetail;
        }
        catch (Exception ex)
        {
            responseDetail = new()
            {
                SubCode = SubCode,
                Title = "CreateTenant",
                Data = null,
                Status = ResultStatus.Fatal500,
                ErrorDetails =
                [
                    new()
                    {
                        Instance = "MissingUser",
                        Detail = $"Error Message: {ex.Message}"
                    }
                ]
            };

            return responseDetail;
        }
    }

    public async Task<ResponseDetail<List<AppTenantInfo>>> ListTenantsByUser(string ssoUserId)
    {
        var user = await userRepo.GetBySsoIdAsync(ssoUserId);

        if (user == null)
        {
            var responseDetail = new ResponseDetail<List<AppTenantInfo>>
            {
                SubCode = SubCode,
                Title = "ListTenantsByUser",
                Data = [],
                Status = ResultStatus.NotFound404,
                ErrorDetails =
                [
                    new()
                    {
                        Instance = "MissingUser",
                        Detail = $"User by SSOId {ssoUserId} not found"
                    }
                ]
            };

            return responseDetail;
        }

        var tenants = await tenantRepo.GetTenantsByUserId(user.Id);

        return new()
        {
            SubCode = SubCode,
            Title = "ListTenantsByUser",
            Data = tenants,
            Status = ResultStatus.Ok200
        };
    }

    public async Task<ResponseDetail<AppTenantInfo>> UpdateTenantAsync(string tenantId, string tenantName, string tenantDescription, string requesterSsoId)
    {

        var user = await userRepo.GetBySsoIdAsync(requesterSsoId);

        if (user == null)
        {
            var responseDetail = new ResponseDetail<AppTenantInfo>
            {
                SubCode = SubCode,
                Title = "Remove Version from Stage",
                Data = null,
                Status = ResultStatus.NotFound404,
                ErrorDetails =
                [
                    new()
                    {
                        Instance = "RemoveVersionFromStage",
                        Detail = $"User with SSOID {requesterSsoId} not found"
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
                Title = "UpdateTenant",
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

        tenant.Name = tenantName;
        tenant.Description = tenantDescription;
        tenant.UpdatedDateTime = DateTime.UtcNow;
        tenant.Identifier = tenantName.ToAlphanumericOnly().ToLowerInvariant();

        await tenantRepo.UpdateTenant(tenant);

        return new()
        {
            SubCode = SubCode,
            Title = "UpdateTenant",
            Data = tenant,
            Status = ResultStatus.Ok200
        };
    }
}
