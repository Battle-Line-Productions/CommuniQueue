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
// Project Name: CommuniQueue.Contracts
// File: IUserService.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Interfaces\IUserService.cs
// ---------------------------------------------------------------------------
#endregion

using BattlelineExtras.Contracts.Models;
using CommuniQueue.Contracts.Models;

namespace CommuniQueue.Contracts.Interfaces;

public interface IUserService
{
    Task<ResponseDetail<User>> GetOrCreateUserAsync(string email, string ssoId, string firstName, string lastName);
    Task<ResponseDetail<User>> CreateUserAsync(string email, string ssoId, string firstName, string lastName);
    Task<ResponseDetail<User>> GetUserByIdAsync(Guid userId);
    Task<ResponseDetail<User>> GetUserBySsoIdAsync(string ssoId);
    Task<ResponseDetail<User>> GetUserByEmailAsync(string email);
    Task<ResponseDetail<List<User>>> GetAllUsersAsync();
    Task<ResponseDetail<User>> UpdateUserAsync(Guid userId, string email, bool requestIsActive, string firstName, string lastName);
    Task<ResponseDetail<bool>> DeleteUserAsync(Guid userId);
    Task<ResponseDetail<List<Permission>>> GetUserPermissionsAsync(Guid userId);
    Task<ResponseDetail<List<User>>> SearchUsersAsync(string searchTerm);
    Task<ResponseDetail<List<User>>> GetUsersWithEntityPermissionsAsync(Guid entityId, EntityType entityType);
}
