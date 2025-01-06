#region Copyright
// ---------------------------------------------------------------------------
// Copyright (c) 2025 Battleline Productions LLC. All rights reserved.
//
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
//
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 01/05/2025
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.Api
// File: JwtExtensions.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Api\Extensions\JwtExtensions.cs
// ---------------------------------------------------------------------------
#endregion

using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using CommuniQueue.Contracts.Models;

namespace CommuniQueue.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
