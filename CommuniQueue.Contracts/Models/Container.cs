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
// File: Container.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Models\Container.cs
// ---------------------------------------------------------------------------

#endregion

#region Usings

using BattlelineExtras.Contracts.Interfaces;

#endregion

namespace CommuniQueue.Contracts.Models;

public class Container : IEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Guid? ParentId { get; set; }
    public Container? Parent { get; set; }
    public ICollection<Container> Children { get; set; } = [];
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
    public ICollection<Template> Templates { get; set; } = [];
    public bool IsRoot { get; set; }
    public Guid Id { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
}
