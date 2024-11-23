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
// Project Name: CommuniQueue.Contracts
// File: IProjectService.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Interfaces\IProjectService.cs
// ---------------------------------------------------------------------------

#endregion

#region Usings

using BattlelineExtras.Contracts.Models;
using CommuniQueue.Contracts.Models;
using CommuniQueue.Contracts.Models.Filters;

#endregion

namespace CommuniQueue.Contracts.Interfaces;

public interface IProjectService
{
    Task<ResponseDetail<Project>> CreateProjectAsync(string name, string description, Guid ownerId);
    Task<ResponseDetail<Project>> GetProjectByIdAsync(Guid projectId);
    Task<ResponseDetail<List<Project?>>> GetProjectsByUserIdAsync(Guid userId);
    Task<ResponseDetail<Project>> UpdateProjectAsync(Guid projectId, string name, string description);
    Task<ResponseDetail<bool>> DeleteProjectAsync(Guid projectId);
    Task<ResponseDetail<bool>> AddUserToProjectAsync(Guid projectId, Guid userId, PermissionLevel permissionLevel);
    Task<ResponseDetail<bool>> RemoveUserFromProjectAsync(Guid projectId, Guid userId);

    Task<ResponseDetail<bool>> UpdateUserPermissionInProjectAsync(Guid projectId, Guid userId,
        PermissionLevel newPermissionLevel);

    Task<ResponseDetail<List<Stage>>> GetProjectStagesAsync(Guid projectId);
    Task<ResponseDetail<Stage>> AddStageToProjectAsync(Guid projectId, string stageName, int order);
    Task<ResponseDetail<bool>> RemoveStageFromProjectAsync(Guid projectId, Guid stageId);
    Task<ResponseDetail<Stage>> UpdateProjectStageAsync(Guid projectId, Guid stageId, string name, int order);

    Task<ResponseDetail<ProjectKpis>> GetProjectMetrics(ProjectFilter? filter);
}
