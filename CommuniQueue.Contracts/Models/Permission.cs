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
// File: Permission.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Models\Permission.cs
// ---------------------------------------------------------------------------

#endregion

#region Usings

using BattlelineExtras.Contracts.Interfaces;

#endregion

namespace CommuniQueue.Contracts.Models;

public class Permission : IEntity
{
    public Guid UserId { get; set; }
    public Guid EntityId { get; set; }
    public EntityType EntityType { get; set; }
    public PermissionLevel PermissionLevel { get; set; }

    public virtual User User { get; set; }
    public virtual Project? Project { get; set; }
    public Guid Id { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
}
