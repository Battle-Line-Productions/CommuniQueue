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
// File: ProjectFilter.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Contracts\Models\Filters\ProjectFilter.cs
// ---------------------------------------------------------------------------
#endregion

namespace CommuniQueue.Contracts.Models.Filters;

public class ProjectFilter
{
    public List<Guid?> Ids { get; set; } = [];
    public List<string?> Names { get; set; } = [];

    public static bool TryParse(string? value, out ProjectFilter? result)
    {
        result = new ProjectFilter();

        if (string.IsNullOrEmpty(value))
        {
            return true; // Return true with an empty filter if the value is null or empty
        }

        var pairs = value.Split('&');
        foreach (var pair in pairs)
        {
            var keyValue = pair.Split('=');
            if (keyValue.Length != 2)
            {
                continue;
            }

            var key = keyValue[0].ToLowerInvariant();
            var values = keyValue[1].Split(',');

            switch (key)
            {
                case "ids":
                    result.Ids = values.Select(v => Guid.TryParse(v, out var guid) ? guid : (Guid?)null).Where(g => g.HasValue).ToList();
                    break;
                case "names":
                    result.Names = values.Select(v => string.IsNullOrEmpty(v) ? null : v).ToList();
                    break;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 23 + (Ids.GetHashCode());
            hash = hash * 23 + (Names.GetHashCode());
            return hash;
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is ProjectFilter filter &&
               Ids == filter.Ids &&
               Names == filter.Names;
    }
}
