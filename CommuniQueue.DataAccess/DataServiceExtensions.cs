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
// Project Name: CommuniQueue.DataAccess
// File: DataServiceExtensions.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.DataAccess\DataServiceExtensions.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.Contracts.Interfaces;
using CommuniQueue.Contracts.Interfaces.Repositories;
using CommuniQueue.DataAccess.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CommuniQueue.DataAccess;

public static class DataServiceExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IKpiQueryService<>), typeof(BaseKpiDataService<>));
        services.AddScoped(typeof(IBaseKpiDataServiceFactory<>), typeof(BaseKpiDataServiceFactory<>));

        services.AddScoped<IContainerRepository, ContainerRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IStageRepository, StageRepository>();
        services.AddScoped<ITemplateRepository, TemplateRepository>();
        services.AddScoped<ITemplateVersionRepository, TemplateVersionRepository>();
        services.AddScoped<ITemplateStageAssignmentRepository, TemplateStageAssignmentRepository>();
        services.AddScoped<IContainerRepository, ContainerRepository>();
        services.AddScoped<IApiKeyRepository, ApiKeyRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
