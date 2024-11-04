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
// Project Name: CommuniQueue.Api
// File: MigrationManager.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Api\Extensions\MigrationManager.cs
// ---------------------------------------------------------------------------

#endregion

#region Usings

using CommuniQueue.DataAccess;
using Microsoft.EntityFrameworkCore;

#endregion

namespace CommuniQueue.Api.Extensions;

public static class MigrationManager
{
    public static WebApplication MigrateDatabase(this WebApplication webApp)
    {
        using var scope = webApp.Services.CreateScope();
        using var appContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        appContext.Database.Migrate();

        return webApp;
    }
}
