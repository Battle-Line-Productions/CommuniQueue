#region Copyright
// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
//
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
//
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 12/23/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.Contracts
// File: GlobalRoleType.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Models\GlobalRoleType.cs
// ---------------------------------------------------------------------------
#endregion

namespace CommuniQueue.Contracts.Models;

public enum GlobalRoleType
{
    Owner = 0,
    SuperAdmin = 1,
    Contributor = 2,
    ReadOnly = 3
}
