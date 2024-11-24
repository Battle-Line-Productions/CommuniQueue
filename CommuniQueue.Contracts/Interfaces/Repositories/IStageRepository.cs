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
// File: IStageRepository.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Interfaces\IStageRepository.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.Contracts.Models;

namespace CommuniQueue.Contracts.Interfaces.Repositories;

public interface IStageRepository : IBaseRepository<Stage>
{
    Task<List<Stage>> GetAllAsync();
    Task<IEnumerable<Stage>> GetByProjectIdAsync(Guid projectId);
}
