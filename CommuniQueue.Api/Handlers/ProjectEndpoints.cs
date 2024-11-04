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
// File: ProjectEndpoints.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Api\Handlers\ProjectEndpoints.cs
// ---------------------------------------------------------------------------

#endregion

using Asp.Versioning;
using BattlelineExtras.Contracts.Models;
using BattlelineExtras.Http.Utility;
using CommuniQueue.Contracts.Interfaces;
using CommuniQueue.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommuniQueue.Api.Handlers;

public static class ProjectEndpoints
{
    private const string BaseRoute = "api/v{version:apiVersion}/projects";

    public static void MapProjectEndpoints(this IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost(BaseRoute, CreateProject)
            .Produces<ResponseDetail<Project>>(StatusCodes.Status201Created)
            .Produces<ResponseDetail<Project>>(StatusCodes.Status400BadRequest)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Create a new project",
                Description = "Creates a new project with the provided details"
            });

        app.MapGet($"{BaseRoute}/{{projectId:guid}}", GetProjectById)
            .Produces<ResponseDetail<Project>>()
            .Produces<ResponseDetail<Project>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get a project by ID",
                Description = "Retrieves the details of a specific project by its ID"
            });

        app.MapGet($"{BaseRoute}/user/{{userId:guid}}", GetProjectsByUserId)
            .Produces<ResponseDetail<List<Project>>>()
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get projects by user ID",
                Description = "Retrieves all projects associated with a specific user"
            });

        app.MapPut($"{BaseRoute}/{{projectId:guid}}", UpdateProject)
            .Produces<ResponseDetail<Project>>()
            .Produces<ResponseDetail<Project>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update a project",
                Description = "Updates the details of an existing project"
            });

        app.MapDelete($"{BaseRoute}/{{projectId:guid}}", DeleteProject)
            .Produces<ResponseDetail<bool>>()
            .Produces<ResponseDetail<bool>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Delete a project",
                Description = "Deletes a specific project by its ID"
            });

        app.MapPost($"{BaseRoute}/{{projectId:guid}}/users", AddUserToProject)
            .Produces<ResponseDetail<bool>>(StatusCodes.Status201Created)
            .Produces<ResponseDetail<bool>>(StatusCodes.Status400BadRequest)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Add a user to a project",
                Description = "Adds a user to a specific project with a given permission level"
            });

        app.MapDelete($"{BaseRoute}/{{projectId:guid}}/users/{{userId:guid}}", RemoveUserFromProject)
            .Produces<ResponseDetail<bool>>()
            .Produces<ResponseDetail<bool>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Remove a user from a project",
                Description = "Removes a specific user from a project"
            });

        app.MapPut($"{BaseRoute}/{{projectId:guid}}/users/{{userId:guid}}/permission", UpdateUserPermissionInProject)
            .Produces<ResponseDetail<bool>>()
            .Produces<ResponseDetail<bool>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update user permission in a project",
                Description = "Updates the permission level of a user in a specific project"
            });

        app.MapGet($"{BaseRoute}/{{projectId:guid}}/stages", GetProjectStages)
            .Produces<ResponseDetail<List<Stage>>>()
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get project stages",
                Description = "Retrieves all stages for a specific project"
            });

        app.MapPost($"{BaseRoute}/{{projectId:guid}}/stages", AddStageToProject)
            .Produces<ResponseDetail<Stage>>(StatusCodes.Status201Created)
            .Produces<ResponseDetail<Stage>>(StatusCodes.Status400BadRequest)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Add a stage to a project",
                Description = "Adds a new stage to a specific project"
            });

        app.MapDelete($"{BaseRoute}/{{projectId:guid}}/stages/{{stageId:guid}}", RemoveStageFromProject)
            .Produces<ResponseDetail<bool>>()
            .Produces<ResponseDetail<bool>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Remove a stage from a project",
                Description = "Removes a specific stage from a project"
            });

        app.MapPut($"{BaseRoute}/{{projectId:guid}}/stages/{{stageId:guid}}", UpdateProjectStage)
            .Produces<ResponseDetail<Stage>>()
            .Produces<ResponseDetail<Stage>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update a project stage",
                Description = "Updates the details of a specific stage in a project"
            });
    }


    private static async Task<IResult> CreateProject(
        [FromServices] IProjectService projectService,
        [FromBody] CreateProjectRequest request)
    {
        var result = await projectService.CreateProjectAsync(request.Name, request.Description, request.OwnerId);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> GetProjectById(
        [FromServices] IProjectService projectService,
        Guid projectId)
    {
        var result = await projectService.GetProjectByIdAsync(projectId);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> GetProjectsByUserId(
        [FromServices] IProjectService projectService,
        Guid userId)
    {
        var result = await projectService.GetProjectsByUserIdAsync(userId);
        return ApiResponse.GetListActionResult(result);
    }

    private static async Task<IResult> UpdateProject(
        [FromServices] IProjectService projectService,
        Guid projectId,
        [FromBody] UpdateProjectRequest request)
    {
        var result = await projectService.UpdateProjectAsync(projectId, request.Name, request.Description);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> DeleteProject(
        [FromServices] IProjectService projectService,
        Guid projectId)
    {
        var result = await projectService.DeleteProjectAsync(projectId);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> AddUserToProject(
        [FromServices] IProjectService projectService,
        Guid projectId,
        [FromBody] AddUserToProjectRequest request)
    {
        var result = await projectService.AddUserToProjectAsync(projectId, request.UserId, request.PermissionLevel);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> RemoveUserFromProject(
        [FromServices] IProjectService projectService,
        Guid projectId,
        Guid userId)
    {
        var result = await projectService.RemoveUserFromProjectAsync(projectId, userId);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> UpdateUserPermissionInProject(
        [FromServices] IProjectService projectService,
        Guid projectId,
        Guid userId,
        [FromBody] UpdateUserPermissionRequest request)
    {
        var result =
            await projectService.UpdateUserPermissionInProjectAsync(projectId, userId, request.NewPermissionLevel);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> GetProjectStages(
        [FromServices] IProjectService projectService,
        Guid projectId)
    {
        var result = await projectService.GetProjectStagesAsync(projectId);
        return ApiResponse.GetListActionResult(result);
    }

    private static async Task<IResult> AddStageToProject(
        [FromServices] IProjectService projectService,
        Guid projectId,
        [FromBody] AddStageToProjectRequest request)
    {
        var result = await projectService.AddStageToProjectAsync(projectId, request.Name, request.Order);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> RemoveStageFromProject(
        [FromServices] IProjectService projectService,
        Guid projectId,
        Guid stageId)
    {
        var result = await projectService.RemoveStageFromProjectAsync(projectId, stageId);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> UpdateProjectStage(
        [FromServices] IProjectService projectService,
        Guid projectId,
        Guid stageId,
        [FromBody] UpdateProjectStageRequest request)
    {
        var result = await projectService.UpdateProjectStageAsync(projectId, stageId, request.Name, request.Order);
        return ApiResponse.GetActionResult(result);
    }
}

// Request models
public record CreateProjectRequest(string Name, string Description, Guid OwnerId);

public record UpdateProjectRequest(string Name, string Description);

public record AddUserToProjectRequest(Guid UserId, PermissionLevel PermissionLevel);

public record UpdateUserPermissionRequest(PermissionLevel NewPermissionLevel);

public record AddStageToProjectRequest(string Name, int Order);

public record UpdateProjectStageRequest(string Name, int Order);
