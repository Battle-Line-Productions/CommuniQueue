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
// File: ContainerService.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue\Services\ContainerService.cs
// ---------------------------------------------------------------------------

#endregion

#region Usings

using BattlelineExtras.Contracts.Extensions;
using BattlelineExtras.Contracts.Models;

#endregion

namespace CommuniQueue.Services;

public class ContainerService(IContainerRepository containerRepository, IProjectRepository projectRepository)
    : IContainerService
{
    private const string SubCode = "ContainerService";

    public async Task<ResponseDetail<Container>> CreateContainerAsync(string name, string description, Guid projectId,
        Guid? parentContainerId)
    {
        try
        {
            var project = await projectRepository.GetByIdAsync(projectId);
            if (project == null)
            {
                return ((Container?)null).BuildResponseDetail(ResultStatus.NotFound404, "Create Container", SubCode)
                    .AddErrorDetail("CreateContainer", "Project not found");
            }

            if (parentContainerId.HasValue)
            {
                var parentContainer = await containerRepository.GetByIdAsync(parentContainerId.Value);
                if (parentContainer == null || parentContainer.ProjectId != projectId)
                {
                    return ((Container?)null).BuildResponseDetail(ResultStatus.NotFound404, "Create Container", SubCode)
                        .AddErrorDetail("CreateContainer",
                            "Parent container not found or does not belong to the specified project");
                }
            }

            var container = new Container
            {
                Name = name,
                Description = description,
                ProjectId = projectId,
                ParentId = parentContainerId,
                IsRoot = false
            };

            var createdContainer = await containerRepository.CreateAsync(container);
            return createdContainer.BuildResponseDetail(ResultStatus.Created201, "Create Container", SubCode);
        }
        catch (Exception ex)
        {
            return ((Container?)null).BuildResponseDetail(ResultStatus.Fatal500, "Create Container", SubCode)
                .AddErrorDetail("CreateContainer", ex.Message);
        }
    }

    public async Task<ResponseDetail<Container>> GetContainerByIdAsync(Guid containerId)
    {
        try
        {
            var container = await containerRepository.GetByIdAsync(containerId);
            if (container == null)
            {
                return ((Container?)null).BuildResponseDetail(ResultStatus.NotFound404, "Get Container by ID", SubCode)
                    .AddErrorDetail("GetContainerById", $"Container with ID {containerId} not found");
            }

            return container.BuildResponseDetail(ResultStatus.Ok200, "Get Container by ID", SubCode);
        }
        catch (Exception ex)
        {
            return ((Container?)null).BuildResponseDetail(ResultStatus.Fatal500, "Get Container by ID", SubCode)
                .AddErrorDetail("GetContainerById", ex.Message);
        }
    }

    public async Task<ResponseDetail<List<Container?>>> GetContainersByProjectIdAsync(Guid projectId)
    {
        try
        {
            var containers = await containerRepository.GetByProjectIdAsync(projectId);
            return containers.ToList().BuildResponseDetail(ResultStatus.Ok200, "Get Containers by Project ID", SubCode);
        }
        catch (Exception ex)
        {
            return new List<Container?>()
                .BuildResponseDetail(ResultStatus.Fatal500, "Get Containers by Project ID", SubCode)
                .AddErrorDetail("GetContainersByProjectId", ex.Message);
        }
    }

    public async Task<ResponseDetail<Container>> UpdateContainerAsync(Guid containerId, string name, string description)
    {
        try
        {
            var container = await containerRepository.GetByIdAsync(containerId);
            if (container == null)
            {
                return ((Container?)null).BuildResponseDetail(ResultStatus.NotFound404, "Update Container", SubCode)
                    .AddErrorDetail("UpdateContainer", $"Container with ID {containerId} not found");
            }

            container.Name = name;
            container.Description = description;
            var updatedContainer = await containerRepository.UpdateAsync(container);
            return updatedContainer.BuildResponseDetail(ResultStatus.Ok200, "Update Container", SubCode);
        }
        catch (Exception ex)
        {
            return ((Container?)null).BuildResponseDetail(ResultStatus.Fatal500, "Update Container", SubCode)
                .AddErrorDetail("UpdateContainer", ex.Message);
        }
    }

    public async Task<ResponseDetail<bool>> DeleteContainerAsync(Guid containerId)
    {
        try
        {
            var container = await containerRepository.GetByIdAsync(containerId);
            if (container == null)
            {
                return false.BuildResponseDetail(ResultStatus.NotFound404, "Delete Container", SubCode)
                    .AddErrorDetail("DeleteContainer", "Container not found");
            }

            if (container.IsRoot)
            {
                return false.BuildResponseDetail(ResultStatus.Error400, "Delete Container", SubCode)
                    .AddErrorDetail("DeleteContainer", "Cannot delete root container");
            }

            await containerRepository.DeleteAsync(containerId);
            return true.BuildResponseDetail(ResultStatus.Ok200, "Delete Container", SubCode);
        }
        catch (Exception ex)
        {
            return false.BuildResponseDetail(ResultStatus.Fatal500, "Delete Container", SubCode)
                .AddErrorDetail("DeleteContainer", ex.Message);
        }
    }

    public async Task<ResponseDetail<List<Container>>> GetChildContainersAsync(Guid parentContainerId)
    {
        try
        {
            var childContainers = await containerRepository.GetChildrenAsync(parentContainerId);
            return childContainers.ToList().BuildResponseDetail(ResultStatus.Ok200, "Get Child Containers", SubCode);
        }
        catch (Exception ex)
        {
            return new List<Container>().BuildResponseDetail(ResultStatus.Fatal500, "Get Child Containers", SubCode)
                .AddErrorDetail("GetChildContainers", ex.Message);
        }
    }

    public async Task<ResponseDetail<bool>> MoveContainerAsync(Guid containerId, Guid? newParentContainerId)
    {
        try
        {
            var container = await containerRepository.GetByIdAsync(containerId);
            if (container == null)
            {
                return false.BuildResponseDetail(ResultStatus.NotFound404, "Move Container", SubCode)
                    .AddErrorDetail("MoveContainer", $"Container with ID {containerId} not found");
            }

            container.ParentId = newParentContainerId;
            await containerRepository.UpdateAsync(container);
            return true.BuildResponseDetail(ResultStatus.Ok200, "Move Container", SubCode);
        }
        catch (Exception ex)
        {
            return false.BuildResponseDetail(ResultStatus.Fatal500, "Move Container", SubCode)
                .AddErrorDetail("MoveContainer", ex.Message);
        }
    }
}
