#region Copyright
// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
//
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
//
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 12/24/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.Contracts
// File: AppTenantInfo.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Models\AppTenantInfo.cs
// ---------------------------------------------------------------------------
#endregion

using Finbuckle.MultiTenant.Abstractions;

namespace CommuniQueue.Contracts.Models;

public class AppTenantInfo : ITenantInfo
{
    public Guid OwnerUserId { get; set; }
    public string Id { get; set; }
    public string Identifier { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }

    public virtual ICollection<UserTenantMembership> UserTenantMemberships { get; set; }
}
