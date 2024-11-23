#region Copyright
// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
// 
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
// 
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 11/17/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.Contracts
// File: ProjectKpis.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Models\ProjectKpis.cs
// ---------------------------------------------------------------------------
#endregion

namespace CommuniQueue.Contracts.Models;

public class ProjectKpis
{
    public int TemplateCount { get; set; }
    public int ContainerCount { get; set; }
    public int StageCount { get; set; }
    public int ApiKeyCount { get; set; }
    public int TeamMemberCount { get; set; }
}
