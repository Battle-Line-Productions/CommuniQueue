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
// File: ITemplateRepository.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Interfaces\ITemplateRepository.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.Contracts.Models;

namespace CommuniQueue.Contracts.Interfaces;

public interface ITemplateRepository
{
    Task<IEnumerable<Template>> GetByProjectIdAsync(Guid projectId);
    Task<IEnumerable<Template>> GetByContainerIdAsync(Guid containerId);
    Task<Template> CreateAsync(Template template);
    Task<Template?> GetByIdAsync(Guid templateId);
    Task DeleteAsync(Guid templateId);
    Task DeleteAsync(Template template);
}
