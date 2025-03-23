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
// Project Name: CommuniQueue.Api
// File: TenantUserManagementEndpoints.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Api\Handlers\TenantUserManagementEndpoints.cs
// ---------------------------------------------------------------------------
#endregion

using Asp.Versioning;
using BattlelineExtras.Contracts.Models;
using BattlelineExtras.Http.Utility;
using CommuniQueue.Api.Extensions;
using CommuniQueue.Contracts.Interfaces;
using CommuniQueue.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommuniQueue.Api.Handlers;

public static class TenantUserManagementEndpoints
{
    private const string BaseRoute = "api/v{version:apiVersion}/tenant/{tenantId}";

    public static void MapTenantUserManagementEndpoints(this IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost($"{BaseRoute}/users/{{userId:guid}}", AddUserToTenant)
            .Produces<ResponseDetail<AppTenantInfo>>(StatusCodes.Status201Created)
            .Produces<ResponseDetail<AppTenantInfo>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Add a user to a tenant",
                Description = "Adds a user to a specific tenant by userId and tenantId"
            });

        app.MapDelete($"{BaseRoute}/users/{{userId:guid}}", RemoveUserFromTenant)
            .Produces<ResponseDetail<AppTenantInfo>>()
            .Produces<ResponseDetail<AppTenantInfo>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Remove a user from a tenant",
                Description = "Removes a user from a specific tenant by userId and tenantId"
            });
    }

    private static async Task<IResult> AddUserToTenant(
        [FromServices] ITenantUserManagementService tenantUserManagementService,
        string tenantId,
        Guid userId, HttpContext context)
    {
        var requesterUserId = context.User.GetUserId();
        if (string.IsNullOrWhiteSpace(requesterUserId))
        {
            return TypedResults.Unauthorized();
        }

        var result = await tenantUserManagementService.AddUserToTenant(userId, tenantId, requesterUserId);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> RemoveUserFromTenant(
        [FromServices] ITenantUserManagementService tenantUserManagementService,
        string tenantId,
        Guid userId,
        HttpContext context)
    {
        var requsterUserId = context.User.GetUserId();
        if (string.IsNullOrWhiteSpace(requsterUserId))
        {
            return TypedResults.Unauthorized();
        }

        var result = await tenantUserManagementService.RemoveUserFromTenant(userId, tenantId, requsterUserId);
        return ApiResponse.GetActionResult(result);
    }
}
