#region Copyright
// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
//
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
//
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 10/13/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.Contracts
// File: User.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Models\User.cs
// ---------------------------------------------------------------------------
#endregion

using BattlelineExtras.Contracts.Interfaces;

namespace CommuniQueue.Contracts.Models;

public class User : IEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
    public required string Email { get; set; }
    public required string SsoId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public ICollection<Permission>? Permissions { get; set; }
}
