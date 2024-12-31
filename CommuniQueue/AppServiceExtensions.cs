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
// Project Name: CommuniQueue
// File: AppServiceExtensions.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue\AppServiceExtensions.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CommuniQueue;

public static class AppServiceExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IStageService, StageService>();
        services.AddScoped<IContainerService, ContainerService>();
        services.AddScoped<ITemplateService, TemplateService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<ITenantUserManagementService, TenantUserManagementService>();

        return services;
    }
}
