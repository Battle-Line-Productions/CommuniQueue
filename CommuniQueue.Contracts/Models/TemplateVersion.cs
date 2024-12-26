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
// File: TemplateVersion.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Models\TemplateVersion.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.Contracts.Interfaces;
using Finbuckle.MultiTenant;

namespace CommuniQueue.Contracts.Models;

[MultiTenant]
public class TemplateVersion : IMultiTenantEntity
{
    public Guid Id { get; set; }
    public int VersionNumber { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    // Add other template content properties here
    public Guid TemplateId { get; set; }
    public Template Template { get; set; }
    public ICollection<TemplateStageAssignment> StageAssignments { get; set; } = [];
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
    public string TenantId { get; set; }
    public AppTenantInfo Tenant { get; set; }
}
