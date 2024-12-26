#region Copyright
// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
//
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
//
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 11/05/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.Contracts
// File: ApiKey.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Models\ApiKey.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.Contracts.Interfaces;
using Finbuckle.MultiTenant;

namespace CommuniQueue.Contracts.Models;

[MultiTenant]
public class ApiKey : IMultiTenantEntity
{
    public Guid Id { get; set; }
    public string KeyHash { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsExpired { get; set; }
    public List<string> Scopes { get; set; }
    public string TenantId { get; set; }
    public AppTenantInfo Tenant { get; set; }
}
