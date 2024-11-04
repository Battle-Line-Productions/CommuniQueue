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
// File: TemplateStageAssignment.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Models\TemplateStageAssignment.cs
// ---------------------------------------------------------------------------
#endregion

using BattlelineExtras.Contracts.Interfaces;

namespace CommuniQueue.Contracts.Models;

public class TemplateStageAssignment : IEntity
{
    public Guid Id { get; set; }
    public Guid TemplateVersionId { get; set; }
    public TemplateVersion TemplateVersion { get; set; }
    public Guid StageId { get; set; }
    public Stage Stage { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
}
