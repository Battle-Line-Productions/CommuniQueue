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
// File: UserTenantMembership.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Models\UserTenantMembership.cs
// ---------------------------------------------------------------------------
#endregion

using BattlelineExtras.Contracts.Interfaces;

namespace CommuniQueue.Contracts.Models;

public class UserTenantMembership : IEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string TenantId { get; set; }
    public User? User { get; set; }
    public AppTenantInfo? Tenant { get; set; }
    public GlobalRoleType GlobalRole { get; set; } = GlobalRoleType.Contributor;
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
}
