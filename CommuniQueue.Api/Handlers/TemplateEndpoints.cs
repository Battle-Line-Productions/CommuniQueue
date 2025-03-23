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
// File: TemplateEndpoints.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Api\Handlers\TemplateEndpoints.cs
// ---------------------------------------------------------------------------
#endregion

using BattlelineExtras.Contracts.Models;
using BattlelineExtras.Http.Utility;
using CommuniQueue.Api.Extensions;
using CommuniQueue.Contracts.Interfaces;
using CommuniQueue.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommuniQueue.Api.Handlers;

public static class TemplateEndpoints
{
    private const string BaseRoute = "api/v{version:apiVersion}/tenant/{tenantId}/templates";

    public static void MapTemplateEndpoints(this IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost(BaseRoute, CreateTemplate)
            .Produces<ResponseDetail<Template>>(StatusCodes.Status201Created)
            .Produces<ResponseDetail<Template>>(StatusCodes.Status400BadRequest)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Create a new template",
                Description = "Creates a new template with the provided details"
            });

        app.MapGet($"{BaseRoute}/{{templateId:guid}}", GetTemplateById)
            .Produces<ResponseDetail<Template>>()
            .Produces<ResponseDetail<Template>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get a template by ID",
                Description = "Retrieves the details of a specific template by its ID"
            });

        app.MapGet($"{BaseRoute}/project/{{projectId:guid}}", GetTemplatesByProjectId)
            .Produces<ResponseDetail<List<Template>>>()
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get templates by project ID",
                Description = "Retrieves all templates associated with a specific project"
            });

        app.MapGet($"{BaseRoute}/container/{{containerId:guid}}", GetTemplatesByContainerId)
            .Produces<ResponseDetail<List<Template>>>()
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get templates by container ID",
                Description = "Retrieves all templates associated with a specific container"
            });

        app.MapGet($"{BaseRoute}/{{templateId:guid}}/latest-version", GetLatestVersion)
            .Produces<ResponseDetail<TemplateVersion>>()
            .Produces<ResponseDetail<TemplateVersion>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get latest version of a template",
                Description = "Retrieves the latest version of a specific template"
            });

        app.MapGet($"{BaseRoute}/{{templateId:guid}}/versions", GetAllVersions)
            .Produces<ResponseDetail<List<TemplateVersion>>>()
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get all versions of a template",
                Description = "Retrieves all versions of a specific template"
            });

        app.MapPost($"{BaseRoute}/{{templateId:guid}}/versions", CreateNewVersion)
            .Produces<ResponseDetail<TemplateVersion>>(StatusCodes.Status201Created)
            .Produces<ResponseDetail<TemplateVersion>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Create a new version of a template",
                Description = "Creates a new version for a specific template"
            });

        app.MapPost($"{BaseRoute}/versions/{{templateVersionId:guid}}/assign", AssignVersionToStage)
            .Produces<ResponseDetail<bool>>()
            .Produces<ResponseDetail<bool>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Assign a template version to a stage",
                Description = "Assigns a specific template version to a stage"
            });

        app.MapDelete($"{BaseRoute}/versions/{{templateVersionId:guid}}/unassign", RemoveVersionFromStage)
            .Produces<ResponseDetail<bool>>()
            .Produces<ResponseDetail<bool>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Remove a template version from a stage",
                Description = "Removes a specific template version from a stage"
            });

        app.MapGet($"{BaseRoute}/{{templateId:guid}}/stage/{{stageId:guid}}", GetVersionAssignedToStage)
            .Produces<ResponseDetail<TemplateVersion>>()
            .Produces<ResponseDetail<TemplateVersion>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get version assigned to a stage",
                Description = "Retrieves the template version assigned to a specific stage for a template"
            });
    }

    private static async Task<IResult> CreateTemplate(
        [FromServices] ITemplateService templateService,
        [FromBody] CreateTemplateRequest request, HttpContext context)
    {
        var userId = context.User.GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return TypedResults.Unauthorized();
        }

        var result = await templateService.CreateTemplateAsync(request.ProjectId, request.ContainerId, request.Name, request.Subject, request.Body, userId);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> GetTemplateById(
        [FromServices] ITemplateService templateService,
        Guid templateId)
    {
        var result = await templateService.GetTemplateByIdAsync(templateId);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> GetTemplatesByProjectId(
        [FromServices] ITemplateService templateService,
        Guid projectId)
    {
        var result = await templateService.GetTemplatesByProjectIdAsync(projectId);
        return ApiResponse.GetListActionResult(result);
    }

    private static async Task<IResult> GetTemplatesByContainerId(
        [FromServices] ITemplateService templateService,
        Guid containerId)
    {
        var result = await templateService.GetTemplatesByContainerIdAsync(containerId);
        return ApiResponse.GetListActionResult(result);
    }

    private static async Task<IResult> GetLatestVersion(
        [FromServices] ITemplateService templateService,
        Guid templateId)
    {
        var result = await templateService.GetLatestVersionAsync(templateId);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> GetAllVersions(
        [FromServices] ITemplateService templateService,
        Guid templateId)
    {
        var result = await templateService.GetAllVersionsAsync(templateId);
        return ApiResponse.GetListActionResult(result);
    }

    private static async Task<IResult> CreateNewVersion(
        [FromServices] ITemplateService templateService,
        Guid templateId,
        [FromBody] CreateVersionRequest request, HttpContext context)
    {
        var userId = context.User.GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return TypedResults.Unauthorized();
        }

        var result = await templateService.CreateNewVersionAsync(templateId, request.Subject, request.Body, userId);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> AssignVersionToStage(
        [FromServices] ITemplateService templateService,
        Guid templateVersionId,
        [FromBody] AssignVersionToStageRequest request, HttpContext context)
    {
        var userId = context.User.GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return TypedResults.Unauthorized();
        }

        var result = await templateService.AssignVersionToStageAsync(templateVersionId, request.StageId, userId);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> RemoveVersionFromStage(
        [FromServices] ITemplateService templateService,
        Guid templateVersionId,
        [FromBody] RemoveVersionFromStageRequest request, HttpContext context)
    {
        var userId = context.User.GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return TypedResults.Unauthorized();
        }

        var result = await templateService.RemoveVersionFromStageAsync(templateVersionId, request.StageId, userId);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> GetVersionAssignedToStage(
        [FromServices] ITemplateService templateService,
        Guid templateId,
        Guid stageId)
    {
        var result = await templateService.GetVersionAssignedToStageAsync(templateId, stageId);
        return ApiResponse.GetActionResult(result);
    }
}

// Request models
public record CreateTemplateRequest(Guid ProjectId, Guid ContainerId, string Name, string Subject, string Body);

public record CreateVersionRequest(string Subject, string Body);

public record AssignVersionToStageRequest(Guid StageId);

public record RemoveVersionFromStageRequest(Guid StageId);
