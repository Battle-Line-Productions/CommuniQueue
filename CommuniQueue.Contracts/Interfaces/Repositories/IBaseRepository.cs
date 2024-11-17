#region Copyright
// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
// 
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
// 
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 11/17/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.Contracts
// File: IBaseRepository.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Interfaces\IBaseRepository.cs
// ---------------------------------------------------------------------------
#endregion

using BattlelineExtras.Contracts.Interfaces;

namespace CommuniQueue.Contracts.Interfaces.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class, IEntity
{
    Task<bool> ExistsAsync(Guid id);
    Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> operation);
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task DeleteAsync(Guid id);
    Task DeleteAsync(TEntity entity);
}
