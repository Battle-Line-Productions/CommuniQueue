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
// File: IContainerService.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Interfaces\IContainerService.cs
// ---------------------------------------------------------------------------

#endregion

#region Usings

using BattlelineExtras.Contracts.Models;
using CommuniQueue.Contracts.Models;

#endregion

namespace CommuniQueue.Contracts.Interfaces;

public interface IContainerService
{
    Task<ResponseDetail<Container>> CreateContainerAsync(string name, string description, Guid projectId,
        Guid? parentContainerId, string requesterSsoId);

    Task<ResponseDetail<Container>> GetContainerByIdAsync(Guid containerId);
    Task<ResponseDetail<List<Container?>>> GetContainersByProjectIdAsync(Guid projectId);
    Task<ResponseDetail<Container>> UpdateContainerAsync(Guid containerId, string name, string description, string requesterSsoId);
    Task<ResponseDetail<bool>> DeleteContainerAsync(Guid containerId, string requesterSsoId);
    Task<ResponseDetail<List<Container>>> GetChildContainersAsync(Guid parentContainerId);
    Task<ResponseDetail<bool>> MoveContainerAsync(Guid containerId, Guid? newParentContainerId, string requesterSsoId);
}
