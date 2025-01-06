#region Copyright

// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
//
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
//
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 12/29/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.Contracts
// File: ITenantUserManagementService.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Interfaces\ITenantUserManagementService.cs
// ---------------------------------------------------------------------------

#endregion

using BattlelineExtras.Contracts.Models;
using CommuniQueue.Contracts.Models;

namespace CommuniQueue.Contracts.Interfaces;

public interface ITenantUserManagementService
{
    Task<ResponseDetail<AppTenantInfo>> AddUserToTenant(Guid userId, string tenantId, string requesterSsoId);
    Task<ResponseDetail<AppTenantInfo>> RemoveUserFromTenant(Guid userId, string tenantId, string requesterSsoId);
}
