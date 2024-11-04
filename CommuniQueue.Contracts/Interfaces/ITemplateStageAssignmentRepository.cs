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
// File: ITemplateStageAssignmentRepository.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Interfaces\ITemplateStageAssignmentRepository.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.Contracts.Models;

namespace CommuniQueue.Contracts.Interfaces;

public interface ITemplateStageAssignmentRepository
{
    Task<TemplateStageAssignment> CreateAsync(TemplateStageAssignment assignment);
    Task DeleteAsync(Guid assignmentId);
    Task<IEnumerable<TemplateStageAssignment>> GetByStageIdAsync(Guid stageId);
    Task<TemplateStageAssignment> GetByStageAndTemplateVersionIdAsync(Guid stageId, Guid templateVersionId);
    Task<bool> ExistsForStageAndTemplateVersionAsync(Guid stageId, Guid templateVersionId);
}
