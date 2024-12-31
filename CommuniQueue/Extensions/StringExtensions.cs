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
// Project Name: CommuniQueue
// File: StringExtensions.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue\Extensions\StringExtensions.cs
// ---------------------------------------------------------------------------
#endregion

using System.Text.RegularExpressions;

namespace CommuniQueue.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Returns a copy of the string containing only alphanumeric characters (letters and digits).
    /// All other characters are removed.
    /// </summary>
    /// <param name="input">The string to sanitize.</param>
    /// <returns>A sanitized string containing only letters and digits, or an empty string if input is null or empty.</returns>
    public static string ToAlphanumericOnly(this string input)
    {
        return string.IsNullOrWhiteSpace(input) ? string.Empty :
            // Replace all non-alphanumeric characters with nothing
            Regex.Replace(input, "[^a-zA-Z0-9]", "");
    }
}
