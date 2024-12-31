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
// File: Project.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Models\Project.cs
// ---------------------------------------------------------------------------

#endregion

#region Usings

using CommuniQueue.Contracts.Interfaces;

#endregion

namespace CommuniQueue.Contracts.Models;

public class Project : IMultiTenantEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? CustomerId { get; set; }
    public ICollection<Stage> Stages { get; set; } = [];
    public ICollection<Container> Containers { get; set; } = [];
    public ICollection<Template> Templates { get; set; } = [];
    public ICollection<Permission> Permissions { get; set; } = [];
    public Guid Id { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
    public string TenantId { get; set; }
    public AppTenantInfo Tenant { get; set; }
}
