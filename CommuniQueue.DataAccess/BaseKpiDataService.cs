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
// File: BaseKpiDataService.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.DataAccess\BaseKpiDataService.cs
// ---------------------------------------------------------------------------
#endregion

using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CommuniQueue.Contracts.Models;

namespace CommuniQueue.DataAccess;

public interface IBaseKpiDataServiceFactory<TEntity, TContext>
    where TEntity : class
    where TContext : DbContext
{
    IKpiQueryService<TEntity, TContext> Create();
}

public class BaseKpiDataServiceFactory<TEntity, TContext>(IDbContextFactory<TContext> contextFactory)
    : IBaseKpiDataServiceFactory<TEntity, TContext>
    where TEntity : class
    where TContext : DbContext
{
    public IKpiQueryService<TEntity, TContext> Create()
    {
        var context = contextFactory.CreateDbContext();
        return new BaseKpiDataService<TEntity, TContext>(context);
    }
}

public interface IKpiQueryService<TEntity, TContext> : IDisposable
    where TEntity : class
    where TContext : DbContext
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

public class BaseKpiDataService<TEntity, TContext>(TContext context) : IKpiQueryService<TEntity, TContext>
    where TEntity : class
    where TContext : DbContext
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();
    private bool _disposed = false;

    public async Task<(IEnumerable<TEntity> data, int totalRecords)> GetAllAsync(
        List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? includes = null,
        Expression<Func<TEntity, bool>>? filterPredicate = null,
        PaginationFilter? paginationFilter = null)
    {
        var query = _dbSet.AsQueryable();

        if (filterPredicate != null)
            query = query.Where(filterPredicate);

        var totalRecords = await query.CountAsync();

        if (includes != null)
            query = includes.Aggregate(query, (current, include) => include(current));

        if (paginationFilter != null)
            query = query
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize);

        var data = await query.ToListAsync();

        return (data, totalRecords);
    }

    public async Task<(IEnumerable<TProjection> data, int totalRecords)> GetAllProjectedAsync<TProjection>(
        Expression<Func<TEntity, TProjection>> projection,
        List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? includes = null,
        Expression<Func<TEntity, bool>>? filterPredicate = null,
        PaginationFilter? paginationFilter = null)
        where TProjection : class
    {
        var query = _dbSet.AsQueryable();

        if (filterPredicate != null)
            query = query.Where(filterPredicate);

        if (includes != null)
            query = includes.Aggregate(query, (current, include) => include(current));

        var totalRecords = await query.CountAsync();

        var projectedQuery = query.Select(projection);

        if (paginationFilter != null)
            projectedQuery = projectedQuery
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize);

        var data = await projectedQuery.ToListAsync();

        return (data, totalRecords);
    }

    public async Task<(IEnumerable<TProjection> data, int totalRecords)> GetAllGroupedProjectedAsync<TProjection, TKey>(
        Expression<Func<IGrouping<TKey, TEntity>, TProjection>> groupProjection,
        Expression<Func<TEntity, TKey>> groupBy,
        List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? includes = null,
        Expression<Func<TEntity, bool>>? filterPredicate = null,
        PaginationFilter? paginationFilter = null)
        where TProjection : class
    {
        var query = _dbSet.AsQueryable();

        if (filterPredicate != null)
            query = query.Where(filterPredicate);

        if (includes != null)
            query = includes.Aggregate(query, (current, include) => include(current));

        var groupedQuery = query.GroupBy(groupBy).Select(groupProjection);

        var totalRecords = await query.GroupBy(groupBy).CountAsync();

        if (paginationFilter != null)
            groupedQuery = groupedQuery
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize);

        var sql = groupedQuery.ToQueryString();

        var data = await groupedQuery.ToListAsync();

        return (data, totalRecords);
    }

    public async Task<IEnumerable<TProjection>> GetAllGroupedDoubleProjectedAsync<TGrouping, TIntermediate, TProjection>(
        Expression<Func<TEntity, TGrouping>> groupBy,
        Expression<Func<IGrouping<TGrouping, TEntity>, TIntermediate>> intermediateProjection,
        Func<IEnumerable<TIntermediate>, IEnumerable<TProjection>> finalProjection,
        Expression<Func<TEntity, bool>>? filterPredicate = null)
        where TIntermediate : class
        where TProjection : class
    {
        var query = _dbSet.AsQueryable();

        if (filterPredicate != null)
            query = query.Where(filterPredicate);

        var intermediateQuery = query
            .GroupBy(groupBy)
            .Select(intermediateProjection);

        // Execute the intermediate query in database
        var intermediateResults = await intermediateQuery.ToListAsync();

        // Apply the final projection in memory
        var result = finalProjection(intermediateResults);

        return result;
    }

    public async Task<(IEnumerable<TProjection> data, int totalRecords)> GetAllOrderedGroupedProjectedAsync<TProjection, TKey>(
        Expression<Func<IGrouping<TKey, TEntity>, TProjection>> groupProjection,
        Expression<Func<TEntity, TKey>> groupBy,
        Func<IQueryable<TProjection>, IOrderedQueryable<TProjection>> applyOrdering,
        List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? includes = null,
        Expression<Func<TEntity, bool>>? filterPredicate = null,
        PaginationFilter? paginationFilter = null)
        where TProjection : class
    {
        var query = _dbSet.AsQueryable();

        if (filterPredicate != null)
            query = query.Where(filterPredicate);

        if (includes != null)
            query = includes.Aggregate(query, (current, include) => include(current));

        var groupedQuery = query.GroupBy(groupBy).Select(groupProjection);

        var orderedQuery = applyOrdering(groupedQuery);

        var totalRecords = await orderedQuery.CountAsync();

        if (paginationFilter != null)
            orderedQuery = orderedQuery
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize) as IOrderedQueryable<TProjection>;

        var data = await orderedQuery!.ToListAsync();

        return (data, totalRecords);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            context.Dispose();
        }

        _disposed = true;
    }
}
