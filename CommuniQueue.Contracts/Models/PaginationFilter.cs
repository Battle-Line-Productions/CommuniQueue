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
// File: PaginationFilter.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Models\PaginationFilter.cs
// ---------------------------------------------------------------------------
#endregion

namespace CommuniQueue.Contracts.Models;

public class PaginationFilter
{
    private const int MaxPageSize = 2000;
    private int _pageNumber = 1;
    private int _pageSize = 25;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = _pageNumber < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set
        {
            _pageSize = value switch
            {
                < 1 => 25,
                > MaxPageSize => MaxPageSize,
                _ => value
            };
        }
    }
}
