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
// Project Name: CommuniQueue
// File: ProjectService.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue\Services\ProjectService.cs
// ---------------------------------------------------------------------------

#endregion

#region Usings

using System.Linq.Expressions;
using BattlelineExtras.Contracts.Extensions;
using BattlelineExtras.Contracts.Models;
using CommuniQueue.Contracts.Interfaces.Repositories;
using CommuniQueue.Contracts.Models.Filters;
using CommuniQueue.Predicate;

#endregion

namespace CommuniQueue.Services;

public class ProjectService(
    IProjectRepository projectRepository,
    IPermissionRepository permissionRepository,
    IStageRepository stageRepository,
    IContainerRepository containerRepository,
    ITemplateRepository templateRepository,
    IKpiQueryService<Project> projectKpiService,
    IUserRepository userRepository)
    : IProjectService
{
    private const string SubCode = "ProjectService";

    public async Task<ResponseDetail<ProjectKpis>> GetProjectMetrics(ProjectFilter? filter)
    {
        var filterPredicate = ProjectDataPredicateBuilder.GetFilterPredicate(filter);
        Expression<Func<Project, object>> groupBy = entity => 1;
        Expression<Func<IGrouping<object, Project>, ProjectKpis>> select = group => new()
        {
            TemplateCount = group.Sum(project => project.Templates.Count),
            StageCount = group.Sum(project =>
                project.Templates.SelectMany(t => t.Versions.Where(v => v.StageAssignments.Count > 0)).Count()),
            ContainerCount = group.Sum(project => project.Containers.Count),
        };

        var (kpis, _) = await projectKpiService.GetAllGroupedProjectedAsync(select, groupBy, null, filterPredicate);

        var result = kpis.FirstOrDefault() ?? new ProjectKpis
        {
            TemplateCount = 0,
            StageCount = 0,
            ContainerCount = 0,
            ApiKeyCount = 0,
            TeamMemberCount = 0
        };

        return result.BuildResponseDetail(ResultStatus.Ok200, "Project Metrics", SubCode);
    }

    public async Task<ResponseDetail<Project>> CreateProjectAsync(string name, string description, string requesterSsoId)
    {
        try
        {
            return await projectRepository.ExecuteInTransactionAsync(async () =>
            {
                var user = await userRepository.GetBySsoIdAsync(requesterSsoId);

                if (user == null)
                {
                    return ((Project?)null).BuildResponseDetail(ResultStatus.NotFound404, "Create Project", SubCode)
                        .AddErrorDetail("CreateProject", $"User with SSOID {requesterSsoId} not found");
                }

                // First, create the Project without the RootContainerId
                var project = new Project
                {
                    Name = name,
                    Description = description
                };

                project = await projectRepository.CreateAsync(project);

                // Now create the Container with the ProjectId
                var rootContainer = new Container
                {
                    Name = "Root",
                    IsRoot = true,
                    ProjectId = project.Id
                };

                await containerRepository.CreateAsync(rootContainer);

                // Create the Permission
                var permission = new Permission
                {
                    UserId = user.Id,
                    EntityId = project.Id,
                    EntityType = EntityType.Project,
                    PermissionLevel = PermissionLevel.SuperAdmin
                };

                await permissionRepository.CreateAsync(permission);

                return project.BuildResponseDetail(ResultStatus.Created201, "Create Project", SubCode);
            });
        }
        catch (Exception ex)
        {
            return ((Project?)null).BuildResponseDetail(ResultStatus.Fatal500, "Create Project", SubCode)
                .AddErrorDetail("CreateProject", ex.Message);
        }
    }

    public async Task<ResponseDetail<Project>> GetProjectByIdAsync(Guid projectId)
    {
        try
        {
            var project = await projectRepository.GetByIdAsync(projectId);
            if (project == null)
            {
                return ((Project?)null).BuildResponseDetail(ResultStatus.NotFound404, "Get Project by ID", SubCode)
                    .AddErrorDetail("GetProjectById", $"Project with ID {projectId} not found");
            }

            return project.BuildResponseDetail(ResultStatus.Ok200, "Get Project by ID", SubCode);
        }
        catch (Exception ex)
        {
            return ((Project?)null).BuildResponseDetail(ResultStatus.Fatal500, "Get Project by ID", SubCode)
                .AddErrorDetail("GetProjectById", ex.Message);
        }
    }

    public async Task<ResponseDetail<List<Project>>> GetProjectsByUserIdAsync(string ssoUserId)
    {
        try
        {
            var user = await userRepository.GetBySsoIdAsync(ssoUserId);
            if (user == null)
            {
                return ((List<Project>)null).BuildResponseDetail(ResultStatus.NotFound404, "Get Projects by User ID", SubCode)
                    .AddErrorDetail("GetProjectsByUserId", $"User with SSOID {ssoUserId} not found");
            }

            var projects = await projectRepository.GetByUserIdAsync(user.Id);
            return projects.ToList().BuildResponseDetail(ResultStatus.Ok200, "Get Projects by User ID", SubCode);
        }
        catch (Exception ex)
        {
            return new List<Project>().BuildResponseDetail(ResultStatus.Fatal500, "Get Projects by User ID", SubCode)
                .AddErrorDetail("GetProjectsByUserId", ex.Message);
        }
    }

    public async Task<ResponseDetail<Project>> UpdateProjectAsync(Guid projectId, string name, string description, string requesterSsoId)
    {
        try
        {
            var user = await userRepository.GetBySsoIdAsync(requesterSsoId);

            if (user == null)
            {
                return ((Project?)null).BuildResponseDetail(ResultStatus.NotFound404, "Update Project", SubCode)
                    .AddErrorDetail("UpdateProject", $"User with SSOID {requesterSsoId} not found");
            }

            var project = await projectRepository.GetByIdAsync(projectId);
            if (project == null)
            {
                return ((Project?)null).BuildResponseDetail(ResultStatus.NotFound404, "Update Project", SubCode)
                    .AddErrorDetail("UpdateProject", $"Project with ID {projectId} not found");
            }

            project.Name = name;
            project.Description = description;
            var updatedProject = await projectRepository.UpdateAsync(project);
            return updatedProject.BuildResponseDetail(ResultStatus.Ok200, "Update Project", SubCode);
        }
        catch (Exception ex)
        {
            return ((Project?)null).BuildResponseDetail(ResultStatus.Fatal500, "Update Project", SubCode)
                .AddErrorDetail("UpdateProject", ex.Message);
        }
    }

    public async Task<ResponseDetail<bool>> DeleteProjectAsync(Guid projectId, string requesterSsoId)
    {
        try
        {
            var user = await userRepository.GetBySsoIdAsync(requesterSsoId);

            if (user == null)
            {
                return false.BuildResponseDetail(ResultStatus.NotFound404, "Delete Project", SubCode)
                    .AddErrorDetail("DeleteProject", $"User with SSOID {requesterSsoId} not found");
            }

            var project = await projectRepository.GetByIdAsync(projectId);

            if (project == null)
            {
                return false.BuildResponseDetail(ResultStatus.NotFound404, "Delete Project", SubCode)
                    .AddErrorDetail("DeleteProject", $"Project with ID {projectId} not found");
            }

            var containers = await containerRepository.GetByProjectIdAsync(projectId);

            foreach (var container in containers)
            {
                var templates = await templateRepository.GetByContainerIdAsync(container.Id);

                foreach (var template in templates)
                {
                    var templatePermissions = await permissionRepository.ListEntityTypeById(template.Id, EntityType.Template);

                    foreach (var templatePermission in templatePermissions)
                    {
                        await permissionRepository.DeleteAsync(templatePermission);
                    }

                    await templateRepository.DeleteAsync(template);
                }

                var containerPermissions = await permissionRepository.ListEntityTypeById(container.Id, EntityType.Container);

                foreach (var containerPermission in containerPermissions)
                {
                    await permissionRepository.DeleteAsync(containerPermission);
                }

                await containerRepository.DeleteAsync(container);
            }

            var projectPermissions = await permissionRepository.ListEntityTypeById(project.Id, EntityType.Project);

            foreach (var permission in projectPermissions)
            {
                await permissionRepository.DeleteAsync(permission);
            }

            await projectRepository.DeleteAsync(projectId);
            return true.BuildResponseDetail(ResultStatus.Ok200, "Delete Project", SubCode);
        }
        catch (Exception ex)
        {
            return false.BuildResponseDetail(ResultStatus.Fatal500, "Delete Project", SubCode)
                .AddErrorDetail("DeleteProject", ex.Message);
        }
    }

    public async Task<ResponseDetail<bool>> AddUserToProjectAsync(Guid projectId, Guid userId,
        PermissionLevel permissionLevel, string requesterSsoId)
    {
        try
        {
            var user = await userRepository.GetBySsoIdAsync(requesterSsoId);

            if (user == null)
            {
                return false.BuildResponseDetail(ResultStatus.NotFound404, "Add User To Project", SubCode)
                    .AddErrorDetail("AddUserToProject", $"User with SSOID {requesterSsoId} not found");
            }

            if (await permissionRepository.ExistsAsync(userId, projectId, EntityType.Project))
            {
                return false.BuildResponseDetail(ResultStatus.Error400, "Add User to Project", SubCode)
                    .AddErrorDetail("AddUserToProject", "User already exists in the project");
            }

            var permission = new Permission
            {
                UserId = userId,
                EntityId = projectId,
                EntityType = EntityType.Project,
                PermissionLevel = permissionLevel
            };

            await permissionRepository.CreateAsync(permission);
            return true.BuildResponseDetail(ResultStatus.Created201, "Add User to Project", SubCode);
        }
        catch (Exception ex)
        {
            return false.BuildResponseDetail(ResultStatus.Fatal500, "Add User to Project", SubCode)
                .AddErrorDetail("AddUserToProject", ex.Message);
        }
    }

    public async Task<ResponseDetail<bool>> RemoveUserFromProjectAsync(Guid projectId, Guid userId, string requesterSsoId)
    {
        try
        {
            var user = await userRepository.GetBySsoIdAsync(requesterSsoId);

            if (user == null)
            {
                return false.BuildResponseDetail(ResultStatus.NotFound404, "Remove User From Project", SubCode)
                    .AddErrorDetail("RemoveUserFromProject", $"User with SSOID {requesterSsoId} not found");
            }

            if (!await permissionRepository.ExistsAsync(userId, projectId, EntityType.Project))
            {
                return false.BuildResponseDetail(ResultStatus.NotFound404, "Remove User from Project", SubCode)
                    .AddErrorDetail("RemoveUserFromProject", "User not found in the project");
            }

            await permissionRepository.DeleteAsync(userId, projectId, EntityType.Project);
            return true.BuildResponseDetail(ResultStatus.Ok200, "Remove User from Project", SubCode);
        }
        catch (Exception ex)
        {
            return false.BuildResponseDetail(ResultStatus.Fatal500, "Remove User from Project", SubCode)
                .AddErrorDetail("RemoveUserFromProject", ex.Message);
        }
    }

    public async Task<ResponseDetail<bool>> UpdateUserPermissionInProjectAsync(Guid projectId, Guid userId,
        PermissionLevel newPermissionLevel, string requesterSsoId)
    {
        try
        {
            var user = await userRepository.GetBySsoIdAsync(requesterSsoId);

            if (user == null)
            {
                return false.BuildResponseDetail(ResultStatus.NotFound404, "Update User Permission In Project", SubCode)
                    .AddErrorDetail("UpdateUserPermissionInProject", $"User with SSOID {requesterSsoId} not found");
            }

            var permission = await permissionRepository.GetAsync(userId, projectId, EntityType.Project);
            if (permission == null)
            {
                return false.BuildResponseDetail(ResultStatus.NotFound404, "Update User Permission in Project", SubCode)
                    .AddErrorDetail("UpdateUserPermissionInProject", "User permission not found in the project");
            }

            permission.PermissionLevel = newPermissionLevel;
            await permissionRepository.UpdateAsync(permission);
            return true.BuildResponseDetail(ResultStatus.Ok200, "Update User Permission in Project", SubCode);
        }
        catch (Exception ex)
        {
            return false.BuildResponseDetail(ResultStatus.Fatal500, "Update User Permission in Project", SubCode)
                .AddErrorDetail("UpdateUserPermissionInProject", ex.Message);
        }
    }

    public async Task<ResponseDetail<List<Stage>>> GetProjectStagesAsync(Guid projectId)
    {
        try
        {
            if (!await projectRepository.ExistsAsync(projectId))
            {
                return new List<Stage>().BuildResponseDetail(ResultStatus.NotFound404, "Get Project Stages", SubCode)
                    .AddErrorDetail("GetProjectStages", $"Project with ID {projectId} not found");
            }

            var stages = await stageRepository.GetByProjectIdAsync(projectId);
            return stages.ToList().BuildResponseDetail(ResultStatus.Ok200, "Get Project Stages", SubCode);
        }
        catch (Exception ex)
        {
            return new List<Stage>().BuildResponseDetail(ResultStatus.Fatal500, "Get Project Stages", SubCode)
                .AddErrorDetail("GetProjectStages", ex.Message);
        }
    }

    public async Task<ResponseDetail<Stage>> AddStageToProjectAsync(Guid projectId, string stageName, int order, string requesterSsoId)
    {
        try
        {
            var user = await userRepository.GetBySsoIdAsync(requesterSsoId);

            if (user == null)
            {
                return ((Stage?)null).BuildResponseDetail(ResultStatus.NotFound404, "Add Stage to Project", SubCode)
                    .AddErrorDetail("AddStageToProject", $"User with SSOID {requesterSsoId} not found");
            }

            if (!await projectRepository.ExistsAsync(projectId))
            {
                return ((Stage?)null).BuildResponseDetail(ResultStatus.NotFound404, "Add Stage to Project", SubCode)
                    .AddErrorDetail("AddStageToProject", $"Project with ID {projectId} not found");
            }

            var stage = new Stage
            {
                ProjectId = projectId,
                Name = stageName,
                Order = order
            };

            stage = await stageRepository.CreateAsync(stage);
            return stage.BuildResponseDetail(ResultStatus.Created201, "Add Stage to Project", SubCode);
        }
        catch (Exception ex)
        {
            return ((Stage?)null).BuildResponseDetail(ResultStatus.Fatal500, "Add Stage to Project", SubCode)
                .AddErrorDetail("AddStageToProject", ex.Message);
        }
    }

    public async Task<ResponseDetail<bool>> RemoveStageFromProjectAsync(Guid projectId, Guid stageId, string requesterSsoId)
    {
        try
        {
            var user = await userRepository.GetBySsoIdAsync(requesterSsoId);

            if (user == null)
            {
                return false.BuildResponseDetail(ResultStatus.NotFound404, "Remove Stage from Project", SubCode)
                    .AddErrorDetail("RemoveStageFromProject", $"User with SSOID {requesterSsoId} not found");
            }

            var stage = await stageRepository.GetByIdAsync(stageId);
            if (stage == null || stage.ProjectId != projectId)
            {
                return false.BuildResponseDetail(ResultStatus.NotFound404, "Remove Stage from Project", SubCode)
                    .AddErrorDetail("RemoveStageFromProject", "Stage not found in the project");
            }

            await stageRepository.DeleteAsync(stageId);
            return true.BuildResponseDetail(ResultStatus.Ok200, "Remove Stage from Project", SubCode);
        }
        catch (Exception ex)
        {
            return false.BuildResponseDetail(ResultStatus.Fatal500, "Remove Stage from Project", SubCode)
                .AddErrorDetail("RemoveStageFromProject", ex.Message);
        }
    }

    public async Task<ResponseDetail<Stage>> UpdateProjectStageAsync(Guid projectId, Guid stageId, string name,
        int order, string requesterSsoId)
    {
        try
        {
            var user = await userRepository.GetBySsoIdAsync(requesterSsoId);

            if (user == null)
            {
                return ((Stage?)null).BuildResponseDetail(ResultStatus.NotFound404, "Update Project Stage", SubCode)
                    .AddErrorDetail("UpdateProjectStage", $"User with SSOID {requesterSsoId} not found");
            }

            var stage = await stageRepository.GetByIdAsync(stageId);
            if (stage == null || stage.ProjectId != projectId)
            {
                return ((Stage?)null).BuildResponseDetail(ResultStatus.NotFound404, "Update Project Stage", SubCode)
                    .AddErrorDetail("UpdateProjectStage", "Stage not found in the project");
            }

            stage.Name = name;
            stage.Order = order;
            var updatedStage = await stageRepository.UpdateAsync(stage);
            return updatedStage.BuildResponseDetail(ResultStatus.Ok200, "Update Project Stage", SubCode);
        }
        catch (Exception ex)
        {
            return ((Stage?)null).BuildResponseDetail(ResultStatus.Fatal500, "Update Project Stage", SubCode)
                .AddErrorDetail("UpdateProjectStage", ex.Message);
        }
    }
}
