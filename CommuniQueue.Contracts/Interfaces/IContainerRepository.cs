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
// File: IContainerRepository.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Interfaces\IContainerRepository.cs
// ---------------------------------------------------------------------------

#endregion

#region Usings

using CommuniQueue.Contracts.Models;

#endregion

namespace CommuniQueue.Contracts.Interfaces;

public interface IContainerRepository
{
    Task<Container> CreateAsync(Container container);
    Task<Container?> GetByIdAsync(Guid containerId);
    Task<IEnumerable<Container>> GetByProjectIdAsync(Guid projectId);
    Task<Container> UpdateAsync(Container container);
    Task DeleteAsync(Guid containerId);
    Task<IEnumerable<Container>> GetChildrenAsync(Guid parentContainerId);
    Task<Container> GetRootContainerForProjectAsync(Guid projectId);
    Task<bool> ExistsAsync(Guid containerId);
}
