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
// Project Name: CommuniQueue
// File: UserService.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue\Services\UserService.cs
// ---------------------------------------------------------------------------
#endregion

using BattlelineExtras.Contracts.Extensions;
using BattlelineExtras.Contracts.Models;
using CommuniQueue.Contracts.Interfaces.Repositories;

namespace CommuniQueue.Services;

public class UserService(IUserRepository userRepository, IPermissionRepository permissionRepository)
    : IUserService
{
    private const string SubCode = "UserService";

    public async Task<ResponseDetail<User>> GetOrCreateUserAsync(string email, string ssoId, string firstName, string lastName)
    {
        try
        {
            var user = await userRepository.GetBySsoIdAsync(ssoId);
            if (user != null)
                return user.BuildResponseDetail(ResultStatus.Ok200, "Get or Create User: Found User", SubCode);

            user = new User
            {
                Email = email,
                SsoId = ssoId,
                IsActive = true,
                FirstName = firstName,
                LastName = lastName
            };

            user = await userRepository.CreateAsync(user);
            return user.BuildResponseDetail(ResultStatus.Created201, "Get or Create User: Created User", SubCode);
        }
        catch (Exception ex)
        {
            return ((User?)null).BuildResponseDetail(ResultStatus.Fatal500, "Get or Create User", SubCode)
                .AddErrorDetail("GetUserBySsoId", ex.Message);
        }
    }

    public async Task<ResponseDetail<User>> CreateUserAsync(string email, string ssoId, string firstName, string lastName)
    {
        try
        {
            if (await userRepository.ExistsBySsoIdAsync(ssoId))
            {
                return ((User?)null).BuildResponseDetail(ResultStatus.Conflict409, "Create User", SubCode)
                    .AddErrorDetail("CreateUser", $"User with SSO ID {ssoId} already exists");
            }

            var user = new User
            {
                Email = email,
                SsoId = ssoId,
                IsActive = true,
                FirstName = firstName,
                LastName = lastName
            };

            user = await userRepository.CreateAsync(user);
            return user.BuildResponseDetail(ResultStatus.Created201, "Create User", SubCode);
        }
        catch (Exception ex)
        {
            return ((User?)null).BuildResponseDetail(ResultStatus.Fatal500, "Create User", SubCode)
                .AddErrorDetail("CreateUser", ex.Message);
        }
    }

    public async Task<ResponseDetail<List<User>>> SearchUsersAsync(string searchTerm)
    {
        try
        {
            var users = await userRepository.SearchUsersAsync(searchTerm);
            return users.ToList().BuildResponseDetail(ResultStatus.Ok200, "Search Users", SubCode);
        }
        catch (Exception ex)
        {
            return new List<User>().BuildResponseDetail(ResultStatus.Fatal500, "Search Users", SubCode)
                .AddErrorDetail("SearchUsers", ex.Message);
        }
    }

    public async Task<ResponseDetail<User>> GetUserByIdAsync(Guid userId)
    {
        try
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ((User?)null).BuildResponseDetail(ResultStatus.NotFound404, "Get User by ID", SubCode)
                    .AddErrorDetail("GetUserById", $"User with ID {userId} not found");
            }
            return user.BuildResponseDetail(ResultStatus.Ok200, "Get User by ID", SubCode);
        }
        catch (Exception ex)
        {
            return ((User?)null).BuildResponseDetail(ResultStatus.Fatal500, "Get User by ID", SubCode)
                .AddErrorDetail("GetUserById", ex.Message);
        }
    }

    public async Task<ResponseDetail<User>> GetUserBySsoIdAsync(string ssoId)
    {
        try
        {
            var user = await userRepository.GetBySsoIdAsync(ssoId);
            if (user == null)
            {
                return ((User?)null).BuildResponseDetail(ResultStatus.NotFound404, "Get User by SSO ID", SubCode)
                    .AddErrorDetail("GetUserBySsoId", $"User with SSO ID {ssoId} not found");
            }
            return user.BuildResponseDetail(ResultStatus.Ok200, "Get User by SSO ID", SubCode);
        }
        catch (Exception ex)
        {
            return ((User?)null).BuildResponseDetail(ResultStatus.Fatal500, "Get User by SSO ID", SubCode)
                .AddErrorDetail("GetUserBySsoId", ex.Message);
        }
    }

    public async Task<ResponseDetail<User>> GetUserByEmailAsync(string email)
    {
        try
        {
            var user = await userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return ((User?)null).BuildResponseDetail(ResultStatus.NotFound404, "Get User by Email", SubCode)
                    .AddErrorDetail("GetUserByEmail", $"User with email {email} not found");
            }
            return user.BuildResponseDetail(ResultStatus.Ok200, "Get User by Email", SubCode);
        }
        catch (Exception ex)
        {
            return ((User?)null).BuildResponseDetail(ResultStatus.Fatal500, "Get User by Email", SubCode)
                .AddErrorDetail("GetUserByEmail", ex.Message);
        }
    }

    public async Task<ResponseDetail<List<User>>> GetAllUsersAsync()
    {
        try
        {
            var users = await userRepository.GetAllAsync();
            return users.ToList().BuildResponseDetail(ResultStatus.Ok200, "Get All Users", SubCode);
        }
        catch (Exception ex)
        {
            return new List<User>().BuildResponseDetail(ResultStatus.Fatal500, "Get All Users", SubCode)
                .AddErrorDetail("GetAllUsers", ex.Message);
        }
    }

    public async Task<ResponseDetail<User>> UpdateUserAsync(Guid userId, string email, bool requestIsActive, string firstName, string lastName)
    {
        try
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ((User?)null).BuildResponseDetail(ResultStatus.NotFound404, "Update User", SubCode)
                    .AddErrorDetail("UpdateUser", $"User with ID {userId} not found");
            }

            user.Email = email;
            user.IsActive = requestIsActive;
            user.FirstName = firstName;
            user.LastName = lastName;

            var updatedUser = await userRepository.UpdateAsync(user);
            return updatedUser.BuildResponseDetail(ResultStatus.Ok200, "Update User", SubCode);
        }
        catch (Exception ex)
        {
            return ((User?)null).BuildResponseDetail(ResultStatus.Fatal500, "Update User", SubCode)
                .AddErrorDetail("UpdateUser", ex.Message);
        }
    }

    public async Task<ResponseDetail<bool>> DeleteUserAsync(Guid userId)
    {
        try
        {
            if (!await userRepository.ExistsAsync(userId))
            {
                return false.BuildResponseDetail(ResultStatus.NotFound404, "Delete User", SubCode)
                    .AddErrorDetail("DeleteUser", $"User with ID {userId} not found");
            }

            await userRepository.DeleteAsync(userId);
            return true.BuildResponseDetail(ResultStatus.Ok200, "Delete User", SubCode);
        }
        catch (Exception ex)
        {
            return false.BuildResponseDetail(ResultStatus.Fatal500, "Delete User", SubCode)
                .AddErrorDetail("DeleteUser", ex.Message);
        }
    }

    public async Task<ResponseDetail<List<Permission>>> GetUserPermissionsAsync(Guid userId)
    {
        try
        {
            if (!await userRepository.ExistsAsync(userId))
            {
                return new List<Permission>().BuildResponseDetail(ResultStatus.NotFound404, "Get User Permissions", SubCode)
                    .AddErrorDetail("GetUserPermissions", $"User with ID {userId} not found");
            }

            var permissions = await permissionRepository.GetByUserId(userId);
            return permissions.ToList().BuildResponseDetail(ResultStatus.Ok200, "Get User Permissions", SubCode);
        }
        catch (Exception ex)
        {
            return new List<Permission>().BuildResponseDetail(ResultStatus.Fatal500, "Get User Permissions", SubCode)
                .AddErrorDetail("GetUserPermissions", ex.Message);
        }
    }

    public async Task<ResponseDetail<List<User>>> GetUsersWithEntityPermissionsAsync(Guid entityId, EntityType entityType)
    {
        try
        {
            var permissions = await permissionRepository.ListEntityTypeById(entityId, entityType);
            var userIds = permissions.Select(p => p.UserId).Distinct();
            var users = await userRepository.GetUsersByIdsAsync(userIds);

            // Group permissions by UserId
            var permissionsByUser = permissions.GroupBy(p => p.UserId)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Add permissions to each user object
            foreach (var user in users)
            {
                user.Permissions = permissionsByUser.TryGetValue(user.Id, out var userPermissions)
                    ? userPermissions
                    : new List<Permission>();
            }

            return users.ToList().BuildResponseDetail(ResultStatus.Ok200, "Get Users With Entity Permissions", SubCode);
        }

        catch (Exception ex)
        {
            return new List<User>().BuildResponseDetail(ResultStatus.Fatal500, "Get Users With Entity Permissions", SubCode)
                .AddErrorDetail("GetUsersWithEntityPermissions", ex.Message);
        }
    }
}
