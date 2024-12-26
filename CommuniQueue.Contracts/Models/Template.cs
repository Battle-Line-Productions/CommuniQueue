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
// File: Template.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Models\Template.cs
// ---------------------------------------------------------------------------

#endregion

#region Usings

using BattlelineExtras.Contracts.Interfaces;
using CommuniQueue.Contracts.Interfaces;
using Finbuckle.MultiTenant;

#endregion

namespace CommuniQueue.Contracts.Models;

[MultiTenant]
public class Template : IMultiTenantEntity
{
    public string Name { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
    public Guid ContainerId { get; set; }
    public Container Container { get; set; }
    public ICollection<TemplateVersion> Versions { get; set; } = [];
    public Guid Id { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
    public string TenantId { get; set; }
    public AppTenantInfo Tenant { get; set; }
}
