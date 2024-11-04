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
// File: IStageService.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Interfaces\IStageService.cs
// ---------------------------------------------------------------------------
#endregion

using BattlelineExtras.Contracts.Models;
using CommuniQueue.Contracts.Models;

namespace CommuniQueue.Contracts.Interfaces;

public interface IStageService
{
    Task<ResponseDetail<Stage>> CreateStageAsync(Guid projectId, string name, int order);
    Task<ResponseDetail<Stage>> GetStageByIdAsync(Guid stageId);
    Task<ResponseDetail<List<Stage>>> GetStagesByProjectIdAsync(Guid projectId);
    Task<ResponseDetail<Stage>> UpdateStageAsync(Guid stageId, string name, int order);
    Task<ResponseDetail<bool>> DeleteStageAsync(Guid stageId);
}
