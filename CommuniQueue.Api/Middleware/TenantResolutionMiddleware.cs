#region Copyright
// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
//
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
//
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 12/30/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.Api
// File: TenantResolutionMiddlelware.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Api\Middleware\TenantResolutionMiddlelware.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.DataAccess;
using Finbuckle.MultiTenant;

namespace CommuniQueue.Api.Middleware;

public class TenantResolutionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext, AppDbContext tenantStore)
    {
        var routeTenantId = httpContext.Request.RouteValues["tenantId"]?.ToString();
        if (!string.IsNullOrEmpty(routeTenantId))
        {
            var tenantInfo = await tenantStore.AppTenantInfo.FindAsync(routeTenantId);

            if (tenantInfo != null)
            {
                httpContext.SetTenantInfo(tenantInfo, true);
            }
        }

        await next(httpContext);
    }
}
