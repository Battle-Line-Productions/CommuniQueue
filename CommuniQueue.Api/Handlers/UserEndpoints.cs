#region Copyright
// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
//
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
//
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 11/24/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.Api
// File: UserEndpoints.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Api\Handlers\UserEndpoints.cs
// ---------------------------------------------------------------------------
#endregion

using Asp.Versioning;
using BattlelineExtras.Contracts.Models;
using BattlelineExtras.Http.Utility;
using CommuniQueue.Contracts.Interfaces;
using CommuniQueue.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommuniQueue.Api.Handlers;

public static class UserEndpoints
{
    private const string BaseRoute = "api/v{version:apiVersion}/users";

    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost($"{BaseRoute}/initial", GetOrCreateUser)
            .Produces<ResponseDetail<User>>()
            .Produces<ResponseDetail<User>>(StatusCodes.Status201Created)
            .Produces<ResponseDetail<User>>(StatusCodes.Status409Conflict)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Requests a user, if one is not found, creates the user from the body",
                Description = "Requests a user, if one is not found, creates the user from the body"
            });

        app.MapPost(BaseRoute, CreateUser)
            .Produces<ResponseDetail<User>>(StatusCodes.Status201Created)
            .Produces<ResponseDetail<User>>(StatusCodes.Status409Conflict)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Create a new user",
                Description = "Creates a new user with the provided email and SSO ID"
            });

        app.MapGet($"{BaseRoute}/{{userId}}", GetUserById)
            .Produces<ResponseDetail<User>>()
            .Produces<ResponseDetail<User>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get user by ID",
                Description = "Retrieves a user by their ID"
            });

        app.MapGet($"{BaseRoute}/sso/{{ssoId}}", GetUserBySsoId)
            .Produces<ResponseDetail<User>>()
            .Produces<ResponseDetail<User>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get user by SSO ID",
                Description = "Retrieves a user by their SSO ID"
            });

        app.MapGet($"{BaseRoute}/email/{{email}}", GetUserByEmail)
            .Produces<ResponseDetail<User>>()
            .Produces<ResponseDetail<User>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get user by email",
                Description = "Retrieves a user by their email address"
            });

        app.MapGet(BaseRoute, GetAllUsers)
            .Produces<ResponseDetail<List<User>>>()
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get all users",
                Description = "Retrieves a list of all users"
            });

        app.MapPut($"{BaseRoute}/{{userId}}", UpdateUser)
            .Produces<ResponseDetail<User>>()
            .Produces<ResponseDetail<User>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update user",
                Description = "Updates a user's email address"
            });

        app.MapDelete($"{BaseRoute}/{{userId}}", DeleteUser)
            .Produces<ResponseDetail<bool>>()
            .Produces<ResponseDetail<bool>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Delete user",
                Description = "Deletes a user by their ID"
            });

        app.MapGet($"{BaseRoute}/{{userId}}/permissions", GetUserPermissions)
            .Produces<ResponseDetail<List<Permission>>>()
            .Produces<ResponseDetail<List<Permission>>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get user permissions",
                Description = "Retrieves a list of permissions for a specific user"
            });

        app.MapGet($"{BaseRoute}/search", SearchUsers)
            .Produces<ResponseDetail<List<User>>>()
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Search users",
                Description = "Searches for users based on a partial match of first name, last name, or email"
            });

        app.MapGet($"{BaseRoute}/entity/{{entityType}}/{{entityId}}/permissions", GetUsersWithEntityPermissions)
            .Produces<ResponseDetail<List<User>>>()
            .Produces<ResponseDetail<List<User>>>(StatusCodes.Status404NotFound)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get users with entity permissions",
                Description = "Retrieves a list of users who have permissions for a specific entity"
            });
    }

    private static async Task<IResult> GetOrCreateUser(
        [FromServices] IUserService userService,
        [FromBody] CreateUserRequest request)
    {
        var result = await userService.GetOrCreateUserAsync(request.Email, request.SsoId, request.FirstName, request.LastName);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> GetUserById(
        [FromServices] IUserService userService,
        Guid userId)
    {
        var result = await userService.GetUserByIdAsync(userId);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> GetUserBySsoId(
        [FromServices] IUserService userService,
        string ssoId)
    {
        var result = await userService.GetUserBySsoIdAsync(ssoId);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> GetUserByEmail(
        [FromServices] IUserService userService,
        string email)
    {
        var result = await userService.GetUserByEmailAsync(email);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> GetAllUsers(
        [FromServices] IUserService userService)
    {
        var result = await userService.GetAllUsersAsync();
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> DeleteUser(
        [FromServices] IUserService userService,
        Guid userId)
    {
        var result = await userService.DeleteUserAsync(userId);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> GetUserPermissions(
        [FromServices] IUserService userService,
        Guid userId)
    {
        var result = await userService.GetUserPermissionsAsync(userId);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> SearchUsers(
        [FromServices] IUserService userService,
        [FromQuery] string searchTerm)
    {
        var result = await userService.SearchUsersAsync(searchTerm);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> GetUsersWithEntityPermissions(
        [FromServices] IUserService userService,
        [FromRoute] EntityType entityType,
        [FromRoute] Guid entityId)
    {
        var result = await userService.GetUsersWithEntityPermissionsAsync(entityId, entityType);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> CreateUser(
        [FromServices] IUserService userService,
        [FromBody] CreateUserRequest request)
    {
        var result = await userService.CreateUserAsync(request.Email, request.SsoId, request.FirstName, request.LastName);
        return ApiResponse.GetActionResult(result);
    }

    private static async Task<IResult> UpdateUser(
        [FromServices] IUserService userService,
        Guid userId,
        [FromBody] UpdateUserRequest request)
    {
        var result = await userService.UpdateUserAsync(userId, request.Email, request.IsActive, request.FirstName, request.LastName);
        return ApiResponse.GetActionResult(result);
    }
}

public record CreateUserRequest(string Email, string SsoId, string FirstName, string LastName);
public record UpdateUserRequest(string Email, bool IsActive, string FirstName, string LastName);
