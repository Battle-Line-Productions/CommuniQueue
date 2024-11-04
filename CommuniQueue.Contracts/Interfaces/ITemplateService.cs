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
// File: ITemplateService.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Interfaces\ITemplateService.cs
// ---------------------------------------------------------------------------
#endregion

using BattlelineExtras.Contracts.Models;
using CommuniQueue.Contracts.Models;

namespace CommuniQueue.Contracts.Interfaces;

public interface ITemplateService
{
    Task<ResponseDetail<Template>> CreateTemplateAsync(Guid projectId, Guid containerId, string name, string subject, string body);
    Task<ResponseDetail<Template>> GetTemplateByIdAsync(Guid templateId);
    Task<ResponseDetail<List<Template>>> GetTemplatesByProjectIdAsync(Guid projectId);
    Task<ResponseDetail<List<Template>>> GetTemplatesByContainerIdAsync(Guid containerId);
    Task<ResponseDetail<TemplateVersion>> CreateNewVersionAsync(Guid templateId, string subject, string body);
    Task<ResponseDetail<TemplateVersion>> GetLatestVersionAsync(Guid templateId);
    Task<ResponseDetail<List<TemplateVersion>>> GetAllVersionsAsync(Guid templateId);
    Task<ResponseDetail<bool>> AssignVersionToStageAsync(Guid templateVersionId, Guid stageId);
    Task<ResponseDetail<bool>> RemoveVersionFromStageAsync(Guid templateVersionId, Guid stageId);
    Task<ResponseDetail<TemplateVersion>> GetVersionAssignedToStageAsync(Guid templateId, Guid stageId);
}
