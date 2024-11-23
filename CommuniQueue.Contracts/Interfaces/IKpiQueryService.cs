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
// File: IKpiQueryService.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.DataAccess\IKpiQueryService.cs
// ---------------------------------------------------------------------------

#endregion

using System.Linq.Expressions;
using CommuniQueue.Contracts.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace CommuniQueue.Contracts.Interfaces;

public interface IKpiQueryService<TEntity> : IDisposable
    where TEntity : class
{
    Task<(IEnumerable<TEntity> data, int totalRecords)> GetAllAsync(
        List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? includes = null,
        Expression<Func<TEntity, bool>>? filterPredicate = null,
        PaginationFilter? paginationFilter = null);

    Task<(IEnumerable<TProjection> data, int totalRecords)> GetAllProjectedAsync<TProjection>(
        Expression<Func<TEntity, TProjection>> projection,
        List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? includes = null,
        Expression<Func<TEntity, bool>>? filterPredicate = null,
        PaginationFilter? paginationFilter = null) where TProjection : class;

    Task<(IEnumerable<TProjection> data, int totalRecords)> GetAllGroupedProjectedAsync<TProjection, TKey>(
        Expression<Func<IGrouping<TKey, TEntity>, TProjection>> groupProjection,
        Expression<Func<TEntity, TKey>> groupBy,
        List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? includes = null,
        Expression<Func<TEntity, bool>>? filterPredicate = null,
        PaginationFilter? paginationFilter = null)
        where TProjection : class;

    Task<(IEnumerable<TProjection> data, int totalRecords)> GetAllOrderedGroupedProjectedAsync<TProjection, TKey>(
        Expression<Func<IGrouping<TKey, TEntity>, TProjection>> groupProjection,
        Expression<Func<TEntity, TKey>> groupBy,
        Func<IQueryable<TProjection>, IOrderedQueryable<TProjection>> applyOrdering,
        List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? includes = null,
        Expression<Func<TEntity, bool>>? filterPredicate = null,
        PaginationFilter? paginationFilter = null)
        where TProjection : class;

    Task<IEnumerable<TProjection>> GetAllGroupedDoubleProjectedAsync<TGrouping, TIntermediate, TProjection>(
        Expression<Func<TEntity, TGrouping>> groupBy,
        Expression<Func<IGrouping<TGrouping, TEntity>, TIntermediate>> intermediateProjection,
        Func<IEnumerable<TIntermediate>, IEnumerable<TProjection>> finalProjection,
        Expression<Func<TEntity, bool>>? filterPredicate = null)
        where TIntermediate : class
        where TProjection : class;
}
