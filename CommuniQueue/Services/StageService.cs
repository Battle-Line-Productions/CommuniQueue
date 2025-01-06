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
// File: StageService.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue\Services\StageService.cs
// ---------------------------------------------------------------------------
#endregion

using BattlelineExtras.Contracts.Extensions;
using BattlelineExtras.Contracts.Models;
using CommuniQueue.Contracts.Interfaces.Repositories;

namespace CommuniQueue.Services;

public class StageService(IStageRepository stageRepository, IProjectRepository projectRepository, IUserRepository userRepository)
    : IStageService
{
    private const string SubCode = "StageService";

    public async Task<ResponseDetail<Stage>> CreateStageAsync(Guid projectId, string name, int order, string requesterSsoId)
    {
        try
        {
            var user = await userRepository.GetBySsoIdAsync(requesterSsoId);

            if (user == null)
            {
                return ((Stage?)null).BuildResponseDetail(ResultStatus.NotFound404, "Create Stage", SubCode)
                    .AddErrorDetail("CreateStage", $"User with SSOID {requesterSsoId} not found");
            }

            if (!await projectRepository.ExistsAsync(projectId))
            {
                return ((Stage?)null).BuildResponseDetail(ResultStatus.NotFound404, "Create Stage", SubCode)
                    .AddErrorDetail("CreateStage", $"Project with ID {projectId} not found");
            }

            var stage = new Stage
            {
                ProjectId = projectId,
                Name = name,
                Order = order
            };

            stage = await stageRepository.CreateAsync(stage);
            return stage.BuildResponseDetail(ResultStatus.Created201, "Create Stage", SubCode);
        }
        catch (Exception ex)
        {
            return ((Stage?)null).BuildResponseDetail(ResultStatus.Fatal500, "Create Stage", SubCode)
                .AddErrorDetail("CreateStage", ex.Message);
        }
    }

    public async Task<ResponseDetail<Stage>> GetStageByIdAsync(Guid stageId)
    {
        try
        {
            var stage = await stageRepository.GetByIdAsync(stageId);
            if (stage == null)
            {
                return ((Stage?)null).BuildResponseDetail(ResultStatus.NotFound404, "Get Stage by ID", SubCode)
                    .AddErrorDetail("GetStageById", $"Stage with ID {stageId} not found");
            }
            return stage.BuildResponseDetail(ResultStatus.Ok200, "Get Stage by ID", SubCode);
        }
        catch (Exception ex)
        {
            return ((Stage?)null).BuildResponseDetail(ResultStatus.Fatal500, "Get Stage by ID", SubCode)
                .AddErrorDetail("GetStageById", ex.Message);
        }
    }

    public async Task<ResponseDetail<List<Stage>>> GetStagesByProjectIdAsync(Guid projectId)
    {
        try
        {
            var stages = await stageRepository.GetByProjectIdAsync(projectId);
            return stages.ToList().BuildResponseDetail(ResultStatus.Ok200, "Get Stages by Project ID", SubCode);
        }
        catch (Exception ex)
        {
            return new List<Stage>().BuildResponseDetail(ResultStatus.Fatal500, "Get Stages by Project ID", SubCode)
                .AddErrorDetail("GetStagesByProjectId", ex.Message);
        }
    }

    public async Task<ResponseDetail<Stage>> UpdateStageAsync(Guid stageId, string name, int order, string requesterSsoId)
    {
        try
        {
            var user = await userRepository.GetBySsoIdAsync(requesterSsoId);

            if (user == null)
            {
                return ((Stage?)null).BuildResponseDetail(ResultStatus.NotFound404, "Update Stage", SubCode)
                    .AddErrorDetail("UpdateStage", $"User with SSOID {requesterSsoId} not found");
            }

            var stage = await stageRepository.GetByIdAsync(stageId);
            if (stage == null)
            {
                return ((Stage?)null).BuildResponseDetail(ResultStatus.NotFound404, "Update Stage", SubCode)
                    .AddErrorDetail("UpdateStage", $"Stage with ID {stageId} not found");
            }

            stage.Name = name;
            stage.Order = order;
            var updatedStage = await stageRepository.UpdateAsync(stage);
            return updatedStage.BuildResponseDetail(ResultStatus.Ok200, "Update Stage", SubCode);
        }
        catch (Exception ex)
        {
            return ((Stage?)null).BuildResponseDetail(ResultStatus.Fatal500, "Update Stage", SubCode)
                .AddErrorDetail("UpdateStage", ex.Message);
        }
    }

    public async Task<ResponseDetail<bool>> DeleteStageAsync(Guid stageId, string requesterSsoId)
    {
        try
        {
            var user = await userRepository.GetBySsoIdAsync(requesterSsoId);

            if (user == null)
            {
                return false.BuildResponseDetail(ResultStatus.NotFound404, "Delete Stage", SubCode)
                    .AddErrorDetail("DeleteStage", $"User with SSOID {requesterSsoId} not found");
            }

            if (!await stageRepository.ExistsAsync(stageId))
            {
                return false.BuildResponseDetail(ResultStatus.NotFound404, "Delete Stage", SubCode)
                    .AddErrorDetail("DeleteStage", $"Stage with ID {stageId} not found");
            }

            await stageRepository.DeleteAsync(stageId);
            return true.BuildResponseDetail(ResultStatus.Ok200, "Delete Stage", SubCode);
        }
        catch (Exception ex)
        {
            return false.BuildResponseDetail(ResultStatus.Fatal500, "Delete Stage", SubCode)
                .AddErrorDetail("DeleteStage", ex.Message);
        }
    }
}
