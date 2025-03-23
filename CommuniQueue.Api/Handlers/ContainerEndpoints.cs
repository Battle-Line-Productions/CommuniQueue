#region Copyright
// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
//
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
//
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 11/03/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.Api
// File: ContainerEndpoints.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Api\Handlers\ContainerEndpoints.cs
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

public static class ContainerEndpoints
{
    private const string BaseRoute = "api/v{version:apiVersion}/tenant/{tenantId}/containers";

    public static void MapContainerEndpoints(this IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost(BaseRoute, CreateContainer)
            .Produces<ResponseDetail<Container>>(StatusCodes.Status201Created)
            .Produces<ResponseDetail<Container>>(StatusCodes.Status400BadRequest)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Create a new container",
                Description = "Creates a new container with the provided details"
            });

        app.MapGet($"{BaseRoute}/{{containerId:guid}}", GetContainerById)
            .Produces<ResponseDetail<Container>>()
            .Produces<ResponseDetail<Container>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get a container by ID",
                Description = "Retrieves the details of a specific container by its ID"
            });

        app.MapGet($"{BaseRoute}/project/{{projectId:guid}}", GetContainersByProjectId)
            .Produces<ResponseDetail<List<Container>>>()
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get containers by project ID",
                Description = "Retrieves all containers associated with a specific project"
            });

        app.MapPut($"{BaseRoute}/{{containerId:guid}}", UpdateContainer)
            .Produces<ResponseDetail<Container>>()
            .Produces<ResponseDetail<Container>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update a container",
                Description = "Updates the details of an existing container"
            });

        app.MapDelete($"{BaseRoute}/{{containerId:guid}}", DeleteContainer)
            .Produces<ResponseDetail<bool>>()
            .Produces<ResponseDetail<bool>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Delete a container",
                Description = "Deletes a specific container by its ID"
            });

        app.MapGet($"{BaseRoute}/{{parentContainerId:guid}}/children", GetChildContainers)
            .Produces<ResponseDetail<List<Container>>>()
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get child containers",
                Description = "Retrieves all child containers of a specific parent container"
            });

        app.MapPut($"{BaseRoute}/{{containerId:guid}}/move", MoveContainer)
            .Produces<ResponseDetail<bool>>()
            .Produces<ResponseDetail<bool>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Move a container",
                Description = "Moves a container to a new parent container"
            });
    }

    public static async Task<IResult> CreateContainer(
        [FromServices] IContainerService containerService,
        [FromBody] CreateContainerRequest request, HttpContext context)
    {
        var userId = context.User.GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return TypedResults.Unauthorized();
        }

        var result = await containerService.CreateContainerAsync(request.Name, request.Description, request.ProjectId, request.ParentContainerId, userId);
        return ApiResponse.GetActionResult(result);
    }

    public static async Task<IResult> GetContainerById(
        [FromServices] IContainerService containerService,
        Guid containerId)
    {
        var result = await containerService.GetContainerByIdAsync(containerId);
        return ApiResponse.GetActionResult(result);
    }

    public static async Task<IResult> GetContainersByProjectId(
        [FromServices] IContainerService containerService,
        Guid projectId)
    {
        var result = await containerService.GetContainersByProjectIdAsync(projectId);
        return ApiResponse.GetListActionResult(result);
    }

    public static async Task<IResult> UpdateContainer(
        [FromServices] IContainerService containerService,
        Guid containerId,
        [FromBody] UpdateContainerRequest request, HttpContext context)
    {
        var userId = context.User.GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return TypedResults.Unauthorized();
        }

        var result = await containerService.UpdateContainerAsync(containerId, request.Name, request.Description, userId);
        return ApiResponse.GetActionResult(result);
    }

    public static async Task<IResult> DeleteContainer(
        [FromServices] IContainerService containerService,
        Guid containerId, HttpContext context)
    {
        var userId = context.User.GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return TypedResults.Unauthorized();
        }

        var result = await containerService.DeleteContainerAsync(containerId, userId);
        return ApiResponse.GetActionResult(result);
    }

    public static async Task<IResult> GetChildContainers(
        [FromServices] IContainerService containerService,
        Guid parentContainerId)
    {
        var result = await containerService.GetChildContainersAsync(parentContainerId);
        return ApiResponse.GetListActionResult(result);
    }

    public static async Task<IResult> MoveContainer(
        [FromServices] IContainerService containerService,
        Guid containerId,
        [FromBody] MoveContainerRequest request, HttpContext context)
    {
        var userId = context.User.GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return TypedResults.Unauthorized();
        }

        var result = await containerService.MoveContainerAsync(containerId, request.NewParentContainerId, userId);
        return ApiResponse.GetActionResult(result);
    }
}

// Request models
public record CreateContainerRequest(string Name, string Description, Guid ProjectId, Guid? ParentContainerId);

public record UpdateContainerRequest(string Name, string Description);

public record MoveContainerRequest(Guid? NewParentContainerId);
