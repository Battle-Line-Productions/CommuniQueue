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
// File: TemplateService.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue\Services\TemplateService.cs
// ---------------------------------------------------------------------------
#endregion

using BattlelineExtras.Contracts.Extensions;
using BattlelineExtras.Contracts.Models;

namespace CommuniQueue.Services;

public class TemplateService(
    ITemplateRepository templateRepository,
    ITemplateVersionRepository templateVersionRepository,
    ITemplateStageAssignmentRepository templateStageAssignmentRepository,
    IStageRepository stageRepository,
    IContainerRepository containerRepository)
    : ITemplateService
{
    private const string SubCode = "TemplateService";

    public async Task<ResponseDetail<Template>> CreateTemplateAsync(Guid projectId, Guid containerId, string name, string subject, string body)
    {
        try
        {
            var container = await containerRepository.GetByIdAsync(containerId);
            if (container == null || container.ProjectId != projectId)
            {
                return ((Template?)null).BuildResponseDetail(ResultStatus.NotFound404, "Create Template", SubCode)
                    .AddErrorDetail("CreateTemplate", "Container not found or does not belong to the specified project");
            }

            var template = new Template
            {
                Name = name,
                ProjectId = projectId,
                ContainerId = containerId
            };

            template = await templateRepository.CreateAsync(template);

            var initialVersion = new TemplateVersion
            {
                TemplateId = template.Id,
                VersionNumber = 1,
                Subject = subject,
                Body = body
            };

            await templateVersionRepository.CreateAsync(initialVersion);

            return template.BuildResponseDetail(ResultStatus.Created201, "Create Template", SubCode);
        }
        catch (Exception ex)
        {
            return ((Template?)null).BuildResponseDetail(ResultStatus.Fatal500, "Create Template", SubCode)
                .AddErrorDetail("CreateTemplate", ex.Message);
        }
    }

    public async Task<ResponseDetail<Template>> GetTemplateByIdAsync(Guid templateId)
    {
        try
        {
            var template = await templateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                return ((Template?)null).BuildResponseDetail(ResultStatus.NotFound404, "Get Template by ID", SubCode)
                    .AddErrorDetail("GetTemplateById", $"Template with ID {templateId} not found");
            }
            return template.BuildResponseDetail(ResultStatus.Ok200, "Get Template by ID", SubCode);
        }
        catch (Exception ex)
        {
            return ((Template?)null).BuildResponseDetail(ResultStatus.Fatal500, "Get Template by ID", SubCode)
                .AddErrorDetail("GetTemplateById", ex.Message);
        }
    }

    public async Task<ResponseDetail<List<Template>>> GetTemplatesByProjectIdAsync(Guid projectId)
    {
        try
        {
            var templates = await templateRepository.GetByProjectIdAsync(projectId);
            return templates.ToList().BuildResponseDetail(ResultStatus.Ok200, "Get Templates by Project ID", SubCode);
        }
        catch (Exception ex)
        {
            return new List<Template>().BuildResponseDetail(ResultStatus.Fatal500, "Get Templates by Project ID", SubCode)
                .AddErrorDetail("GetTemplatesByProjectId", ex.Message);
        }
    }

    public async Task<ResponseDetail<List<Template>>> GetTemplatesByContainerIdAsync(Guid containerId)
    {
        try
        {
            var templates = await templateRepository.GetByContainerIdAsync(containerId);
            return templates.ToList().BuildResponseDetail(ResultStatus.Ok200, "Get Templates by Container ID", SubCode);
        }
        catch (Exception ex)
        {
            return new List<Template>().BuildResponseDetail(ResultStatus.Fatal500, "Get Templates by Container ID", SubCode)
                .AddErrorDetail("GetTemplatesByContainerId", ex.Message);
        }
    }

    public async Task<ResponseDetail<TemplateVersion>> GetLatestVersionAsync(Guid templateId)
    {
        try
        {
            var latestVersion = await templateVersionRepository.GetLatestVersionAsync(templateId);
            if (latestVersion == null)
            {
                return ((TemplateVersion?)null).BuildResponseDetail(ResultStatus.NotFound404, "Get Latest Version", SubCode)
                    .AddErrorDetail("GetLatestVersion", $"No versions found for template with ID {templateId}");
            }
            return latestVersion.BuildResponseDetail(ResultStatus.Ok200, "Get Latest Version", SubCode);
        }
        catch (Exception ex)
        {
            return ((TemplateVersion?)null).BuildResponseDetail(ResultStatus.Fatal500, "Get Latest Version", SubCode)
                .AddErrorDetail("GetLatestVersion", ex.Message);
        }
    }

    public async Task<ResponseDetail<List<TemplateVersion>>> GetAllVersionsAsync(Guid templateId)
    {
        try
        {
            var versions = await templateVersionRepository.GetByTemplateIdAsync(templateId);
            return versions.ToList().BuildResponseDetail(ResultStatus.Ok200, "Get All Versions", SubCode);
        }
        catch (Exception ex)
        {
            return new List<TemplateVersion>().BuildResponseDetail(ResultStatus.Fatal500, "Get All Versions", SubCode)
                .AddErrorDetail("GetAllVersions", ex.Message);
        }
    }

    public async Task<ResponseDetail<TemplateVersion>> CreateNewVersionAsync(Guid templateId, string subject, string body)
    {
        try
        {
            var template = await templateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                return ((TemplateVersion?)null).BuildResponseDetail(ResultStatus.NotFound404, "Create New Version", SubCode)
                    .AddErrorDetail("CreateNewVersion", "Template not found");
            }

            var latestVersion = await templateVersionRepository.GetLatestVersionAsync(templateId);
            var newVersionNumber = latestVersion.VersionNumber + 1;

            var newVersion = new TemplateVersion
            {
                TemplateId = templateId,
                VersionNumber = newVersionNumber,
                Subject = subject,
                Body = body
            };

            newVersion = await templateVersionRepository.CreateAsync(newVersion);
            return newVersion.BuildResponseDetail(ResultStatus.Created201, "Create New Version", SubCode);
        }
        catch (Exception ex)
        {
            return ((TemplateVersion?)null).BuildResponseDetail(ResultStatus.Fatal500, "Create New Version", SubCode)
                .AddErrorDetail("CreateNewVersion", ex.Message);
        }
    }

    public async Task<ResponseDetail<bool>> AssignVersionToStageAsync(Guid templateVersionId, Guid stageId)
    {
        try
        {
            var templateVersion = await templateVersionRepository.GetByIdAsync(templateVersionId);
            if (templateVersion == null)
            {
                return false.BuildResponseDetail(ResultStatus.NotFound404, "Assign Version to Stage", SubCode)
                    .AddErrorDetail("AssignVersionToStage", "Template version not found");
            }

            var stage = await stageRepository.GetByIdAsync(stageId);
            if (stage == null)
            {
                return false.BuildResponseDetail(ResultStatus.NotFound404, "Assign Version to Stage", SubCode)
                    .AddErrorDetail("AssignVersionToStage", "Stage not found");
            }

            // Remove existing assignment for this template in the stage
            var existingAssignment = await templateStageAssignmentRepository.GetByStageAndTemplateVersionIdAsync(stageId, templateVersion.TemplateId);
            if (existingAssignment != null)
            {
                await templateStageAssignmentRepository.DeleteAsync(existingAssignment.Id);
            }

            // Create new assignment
            var newAssignment = new TemplateStageAssignment
            {
                TemplateVersionId = templateVersionId,
                StageId = stageId
            };

            await templateStageAssignmentRepository.CreateAsync(newAssignment);
            return true.BuildResponseDetail(ResultStatus.Ok200, "Assign Version to Stage", SubCode);
        }
        catch (Exception ex)
        {
            return false.BuildResponseDetail(ResultStatus.Fatal500, "Assign Version to Stage", SubCode)
                .AddErrorDetail("AssignVersionToStage", ex.Message);
        }
    }

    public async Task<ResponseDetail<bool>> RemoveVersionFromStageAsync(Guid templateVersionId, Guid stageId)
    {
        try
        {
            var assignment = await templateStageAssignmentRepository.GetByStageAndTemplateVersionIdAsync(stageId, templateVersionId);
            if (assignment == null)
            {
                return false.BuildResponseDetail(ResultStatus.NotFound404, "Remove Version from Stage", SubCode)
                    .AddErrorDetail("RemoveVersionFromStage", "Assignment not found");
            }

            await templateStageAssignmentRepository.DeleteAsync(assignment.Id);
            return true.BuildResponseDetail(ResultStatus.Ok200, "Remove Version from Stage", SubCode);
        }
        catch (Exception ex)
        {
            return false.BuildResponseDetail(ResultStatus.Fatal500, "Remove Version from Stage", SubCode)
                .AddErrorDetail("RemoveVersionFromStage", ex.Message);
        }
    }

    public async Task<ResponseDetail<TemplateVersion>> GetVersionAssignedToStageAsync(Guid templateId, Guid stageId)
    {
        try
        {
            var assignments = await templateStageAssignmentRepository.GetByStageIdAsync(stageId);
            var assignmentForTemplate = assignments.FirstOrDefault(a => a.TemplateVersion.TemplateId == templateId);

            if (assignmentForTemplate == null)
            {
                return ((TemplateVersion?)null).BuildResponseDetail(ResultStatus.NotFound404, "Get Version Assigned to Stage", SubCode)
                    .AddErrorDetail("GetVersionAssignedToStage", "No version assigned to this stage for the specified template");
            }

            return assignmentForTemplate.TemplateVersion.BuildResponseDetail(ResultStatus.Ok200, "Get Version Assigned to Stage", SubCode);
        }
        catch (Exception ex)
        {
            return ((TemplateVersion?)null).BuildResponseDetail(ResultStatus.Fatal500, "Get Version Assigned to Stage", SubCode)
                .AddErrorDetail("GetVersionAssignedToStage", ex.Message);
        }
    }
}
