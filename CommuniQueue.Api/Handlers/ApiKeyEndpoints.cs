#region Copyright
// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
// 
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
// 
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 11/09/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.Api
// File: ApiKeyEndpoints.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Api\Handlers\ApiKeyEndpoints.cs
// ---------------------------------------------------------------------------
#endregion

using Asp.Versioning;
using BattlelineExtras.Contracts.Models;
using BattlelineExtras.Http.Utility;
using CommuniQueue.Contracts.Interfaces;
using CommuniQueue.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommuniQueue.Api.Handlers;

public static class ApiKeyEndpoints
{
    private const string BaseRoute = "api/v{version:apiVersion}/apikeys";

    public static void MapApiKeyEndpoints(this IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost($"{BaseRoute}/generate", GenerateApiKey)
            .Produces<ResponseDetail<(ApiKey?, string?)>>(StatusCodes.Status201Created)
            .Produces<ResponseDetail<(ApiKey?, string?)>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Generate a new API key",
                Description = "Generates a new API key for a specific project"
            });

        app.MapGet($"{BaseRoute}/{{apiKeyId}}", GetApiKeyById)
            .Produces<ResponseDetail<ApiKey>>()
            .Produces<ResponseDetail<ApiKey>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get API key by ID",
                Description = "Retrieves an API key by its ID"
            });

        app.MapGet($"{BaseRoute}/project/{{projectId}}", GetApiKeysByProjectId)
            .Produces<ResponseDetail<List<ApiKey>>>()
            .Produces<ResponseDetail<List<ApiKey>>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get API keys by project ID",
                Description = "Retrieves all API keys for a specific project"
            });

        app.MapPost($"{BaseRoute}/validate", ValidateApiKey)
            .Produces<ResponseDetail<bool>>()
            .Produces<ResponseDetail<bool>>(StatusCodes.Status401Unauthorized)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Validate API key",
                Description = "Validates the provided API key"
            });

        app.MapPost($"{BaseRoute}/expire/{{apiKeyId}}", ExpireApiKey)
            .Produces<ResponseDetail<bool>>()
            .Produces<ResponseDetail<bool>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Expire API key",
                Description = "Expires the specified API key"
            });

        app.MapGet($"{BaseRoute}/valid/{{projectId}}", HasValidApiKey)
            .Produces<ResponseDetail<bool>>()
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Check for valid API key",
                Description = "Checks if a project has a valid API key"
            });
    }

    private static async Task<IResult> GenerateApiKey(
        [FromServices] IApiKeyService apiKeyService,
        [FromBody] GenerateApiKeyRequest request)
    {
        var result = await apiKeyService.GenerateApiKeyAsync(request.ProjectId, request.StartDate, request.EndDate, request.Scopes);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> GetApiKeyById(
        [FromServices] IApiKeyService apiKeyService,
        Guid apiKeyId)
    {
        var result = await apiKeyService.GetApiKeyByIdAsync(apiKeyId);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> GetApiKeysByProjectId(
        [FromServices] IApiKeyService apiKeyService,
        Guid projectId)
    {
        var result = await apiKeyService.GetApiKeysByProjectIdAsync(projectId);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> ValidateApiKey(
        [FromServices] IApiKeyService apiKeyService,
        [FromBody] ValidateApiKeyRequest request)
    {
        var result = await apiKeyService.ValidateApiKeyAsync(request.ApiKey);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> ExpireApiKey(
        [FromServices] IApiKeyService apiKeyService,
        Guid apiKeyId)
    {
        var result = await apiKeyService.ExpireApiKeyAsync(apiKeyId);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> HasValidApiKey(
        [FromServices] IApiKeyService apiKeyService,
        Guid projectId)
    {
        var result = await apiKeyService.HasValidApiKeyAsync(projectId);
        return ApiResponse.GetActionResult(result);
    }
}

public record GenerateApiKeyRequest(Guid ProjectId, DateTime StartDate, DateTime EndDate, List<string> Scopes);
public record ValidateApiKeyRequest(string ApiKey);
