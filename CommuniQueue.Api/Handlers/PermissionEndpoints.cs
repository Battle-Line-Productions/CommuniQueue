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
// Project Name: CommuniQueue.Api
// File: PermissionEndpoints.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Api\Handlers\PermissionEndpoints.cs
// ---------------------------------------------------------------------------
#endregion

using Asp.Versioning;
using BattlelineExtras.Contracts.Models;
using BattlelineExtras.Http.Utility;
using CommuniQueue.Contracts.Interfaces;
using CommuniQueue.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommuniQueue.Api.Handlers;

public static class PermissionEndpoints
{
    private const string BaseRoute = "api/v{version:apiVersion}/permissions";

    public static void MapPermissionEndpoints(this IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost(BaseRoute, CreatePermission)
            .Produces<ResponseDetail<Permission>>(StatusCodes.Status201Created)
            .Produces<ResponseDetail<Permission>>(StatusCodes.Status400BadRequest)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Create a new permission",
                Description = "Creates a new permission for a user on a specific entity"
            });

        app.MapGet($"{BaseRoute}/{{userId:guid}}/{{entityId:guid}}/{{entityType}}", GetPermission)
            .Produces<ResponseDetail<Permission>>()
            .Produces<ResponseDetail<Permission>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get permission",
                Description = "Retrieves a specific permission"
            });

        app.MapGet($"{BaseRoute}/entity/{{entityId:guid}}/{{entityType}}", GetPermissionsByEntity)
            .Produces<ResponseDetail<List<Permission>>>()
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get permissions by entity",
                Description = "Retrieves all permissions for a specific entity"
            });

        app.MapPut(BaseRoute, UpdatePermission)
            .Produces<ResponseDetail<Permission>>()
            .Produces<ResponseDetail<Permission>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update permission",
                Description = "Updates an existing permission"
            });

        app.MapDelete($"{BaseRoute}/{{userId:guid}}/{{entityId:guid}}/{{entityType}}", DeletePermission)
            .Produces<ResponseDetail<bool>>()
            .Produces<ResponseDetail<bool>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Delete permission",
                Description = "Deletes a specific permission"
            });
    }

    public static async Task<IResult> CreatePermission(
        [FromServices] IPermissionService permissionService,
        [FromBody] CreatePermissionRequest request)
    {
        if (Enum.TryParse<EntityType>(request.EntityType, true, out var parsedEntityType))
        {
            var result = await permissionService.CreatePermissionAsync(request.UserId, request.EntityId, parsedEntityType, request.PermissionLevel);
            return ApiResponse.GetActionResult(result);
        }

        return Results.BadRequest($"Invalid EntityType: {request.EntityType}");
    }

    public static async Task<IResult> GetPermission(
        [FromServices] IPermissionService permissionService,
        Guid userId,
        Guid entityId,
        string entityType)
    {
        if (Enum.TryParse<EntityType>(entityType, true, out var parsedEntityType))
        {
            var result = await permissionService.GetPermissionAsync(userId, entityId, parsedEntityType);
            return ApiResponse.GetActionResult(result);
        }

        return Results.BadRequest($"Invalid EntityType: {entityType}");
    }

    public static async Task<IResult> GetPermissionsByEntity(
        [FromServices] IPermissionService permissionService,
        Guid entityId,
        string entityType)
    {
        if (Enum.TryParse<EntityType>(entityType, true, out var parsedEntityType))
        {
            var result = await permissionService.GetPermissionsByEntityAsync(entityId, parsedEntityType);
            return ApiResponse.GetActionResult(result);
        }

        return Results.BadRequest($"Invalid EntityType: {entityType}");
    }

    public static async Task<IResult> UpdatePermission(
        [FromServices] IPermissionService permissionService,
        [FromBody] UpdatePermissionRequest request)
    {
        if (Enum.TryParse<EntityType>(request.EntityType, true, out var parsedEntityType))
        {
            var result = await permissionService.UpdatePermissionAsync(request.UserId, request.EntityId, parsedEntityType, request.NewPermissionLevel);
            return ApiResponse.GetActionResult(result);
        }

        return Results.BadRequest($"Invalid EntityType: {request.EntityType}");
    }

    public static async Task<IResult> DeletePermission(
        [FromServices] IPermissionService permissionService,
        Guid userId,
        Guid entityId,
        string entityType)
    {
        if (Enum.TryParse<EntityType>(entityType, true, out var parsedEntityType))
        {
            var result = await permissionService.DeletePermissionAsync(userId, entityId, parsedEntityType);
            return ApiResponse.GetActionResult(result);
        }

        return Results.BadRequest($"Invalid EntityType: {entityType}");
    }
}

public record CreatePermissionRequest(Guid UserId, Guid EntityId, string EntityType, PermissionLevel PermissionLevel);
public record UpdatePermissionRequest(Guid UserId, Guid EntityId, string EntityType, PermissionLevel NewPermissionLevel);
