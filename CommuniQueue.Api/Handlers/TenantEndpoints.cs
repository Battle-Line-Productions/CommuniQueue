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
// File: TenantEndpoints.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Api\Handlers\TenantEndpoints.cs
// ---------------------------------------------------------------------------
#endregion

using Asp.Versioning;
using BattlelineExtras.Contracts.Models;
using BattlelineExtras.Http.Utility;
using CommuniQueue.Contracts.Interfaces;
using CommuniQueue.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommuniQueue.Api.Handlers;

public static class TenantEndpoints
{
    private const string BaseRoute = "api/v{version:apiVersion}/tenants";

    public static void MapTenantEndpoints(this IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost(BaseRoute, CreateTenant)
            .Produces<ResponseDetail<AppTenantInfo>>(StatusCodes.Status201Created)
            .Produces<ResponseDetail<AppTenantInfo>>(StatusCodes.Status404NotFound)
            .Produces<ResponseDetail<AppTenantInfo>>(StatusCodes.Status500InternalServerError)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Create a new tenant",
                Description = "Creates a new tenant, optionally linking it to a user by SSO ID"
            });

        app.MapGet($"{BaseRoute}/user/{{ssoUserId}}", ListTenantsByUser)
            .Produces<ResponseDetail<List<AppTenantInfo>>>()
            .Produces<ResponseDetail<List<AppTenantInfo>>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "List all tenants for a user",
                Description = "Lists all tenants associated with a user, identified by SSO ID"
            });

        app.MapPut($"{BaseRoute}/{{tenantId}}", UpdateTenant)
            .Produces<ResponseDetail<AppTenantInfo>>()
            .Produces<ResponseDetail<AppTenantInfo>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update an existing tenant",
                Description = "Updates the name and description of an existing tenant"
            });
    }

    private static async Task<IResult> CreateTenant(
        [FromServices] ITenantService tenantService,
        [FromBody] CreateTenantRequest request)
    {
        var result = await tenantService.CreateTenantAsync(
            request.TenantName,
            request.TenantDescription,
            request.SsoUserId
        );

        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> ListTenantsByUser(
        [FromServices] ITenantService tenantService,
        string ssoUserId)
    {
        var result = await tenantService.ListTenantsByUser(ssoUserId);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> UpdateTenant(
        [FromServices] ITenantService tenantService,
        string tenantId,
        [FromBody] UpdateTenantRequest request)
    {
        var result = await tenantService.UpdateTenantAsync(
            tenantId,
            request.TenantName,
            request.TenantDescription
        );

        return ApiResponse.GetActionResult(result);
    }
}

public record CreateTenantRequest(string TenantName, string TenantDescription, string SsoUserId);
public record UpdateTenantRequest(string TenantName, string TenantDescription);
