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
// File: PermissionLevel.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Models\PermissionLevel.cs
// ---------------------------------------------------------------------------
#endregion

namespace CommuniQueue.Contracts.Models;

public enum PermissionLevel
{
    ReadOnly,
    Contributor,
    Admin,
    SuperAdmin
}
