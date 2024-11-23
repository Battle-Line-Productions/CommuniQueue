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
// Project Name: CommuniQueue
// File: ProjectDataFilter.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue\Predicate\ProjectDataFilter.cs
// ---------------------------------------------------------------------------
#endregion

using System.Linq.Expressions;
using CommuniQueue.Contracts.Models.Filters;
using LinqKit;

namespace CommuniQueue.Predicate;

public static class ProjectDataPredicateBuilder
{
    public static Expression<Func<Project, bool>> GetFilterPredicate(ProjectFilter? filter = null)
    {
        var predicate = PredicateBuilder.New<Project>();

        if (filter == null)
        {
            return predicate;
        }

        if (filter.Ids.Count > 0)
        {
            predicate = predicate.And(p => filter.Ids.Contains(p.Id));
        }

        if (filter.Names.Count > 0)
        {
            predicate = predicate.And(p => filter.Names.Contains(p.Name));
        }

        return predicate;
    }
}
