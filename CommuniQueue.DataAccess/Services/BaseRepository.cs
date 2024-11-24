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
// Project Name: CommuniQueue.DataAccess
// File: BaseRepository.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.DataAccess\Services\BaseRepository.cs
// ---------------------------------------------------------------------------
#endregion

using BattlelineExtras.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommuniQueue.DataAccess.Services;

public class BaseRepository<TEntity>(AppDbContext context) where TEntity : class, IEntity
{
    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.Set<TEntity>().AnyAsync(p => p.Id == id);
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await context.Set<TEntity>().ToListAsync();
    }

    public async Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> operation)
    {
        var strategy = context.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var result = await operation();
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        });
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        context.Set<TEntity>().Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await context.Set<TEntity>().FindAsync(id);
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await context.Set<TEntity>().FindAsync(id);
        if (entity != null)
        {
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
        await context.SaveChangesAsync();
    }
}
