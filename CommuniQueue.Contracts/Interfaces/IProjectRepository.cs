#region Copyright
// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
// 
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
// 
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 10/14/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.Contracts
// File: IProjectRepository.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Interfaces\IProjectRepository.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.Contracts.Models;

namespace CommuniQueue.Contracts.Interfaces;

public interface IProjectRepository
{
    Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> operation);
    Task<Project> CreateAsync(Project project);
    Task<Project?> GetByIdAsync(Guid projectId);
    Task<IEnumerable<Project?>> GetByUserIdAsync(Guid userId);
    Task<Project> UpdateAsync(Project project);
    Task DeleteAsync(Guid projectId);
    Task<bool> ExistsAsync(Guid projectId);
}
