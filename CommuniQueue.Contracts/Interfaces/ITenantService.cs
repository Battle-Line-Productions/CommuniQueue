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
// File: ITenantService.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Interfaces\ITenantService.cs
// ---------------------------------------------------------------------------
#endregion

using BattlelineExtras.Contracts.Models;
using CommuniQueue.Contracts.Models;

namespace CommuniQueue.Contracts.Interfaces;

public interface ITenantService
{
    Task<ResponseDetail<AppTenantInfo>> CreateTenantAsync(string tenantName, string tenantDescription, string ssoUserId);
    Task<ResponseDetail<List<AppTenantInfo>>> ListTenantsByUser(string ssoUserId);
    Task<ResponseDetail<AppTenantInfo>> UpdateTenantAsync(string tenantId, string tenantName, string tenantDescription, string requesterSsoId);
}
