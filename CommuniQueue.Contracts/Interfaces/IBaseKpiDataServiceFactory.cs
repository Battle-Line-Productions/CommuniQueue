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
// File: IBaseKpiDataServiceFactory.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.DataAccess\IBaseKpiDataServiceFactory.cs
// ---------------------------------------------------------------------------

#endregion

namespace CommuniQueue.Contracts.Interfaces;

public interface IBaseKpiDataServiceFactory<TEntity>
    where TEntity : class
{
    IKpiQueryService<TEntity> Create();
}
