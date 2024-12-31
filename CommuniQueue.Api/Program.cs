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
// File: Program.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.Api\Program.cs
// ---------------------------------------------------------------------------

#endregion

#region Usings

using System.Text.Json.Serialization;
using Asp.Versioning;
using BattlelineExtras.Http.Extensions;
using CommuniQueue;
using CommuniQueue.Api.Extensions;
using CommuniQueue.Api.Handlers;
using CommuniQueue.Api.Middleware;
using CommuniQueue.Contracts.Models;
using CommuniQueue.DataAccess;
using CommuniQueue.ServiceDefaults;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using ServiceExtensions = BattlelineExtras.Services.Extensions.ServiceExtensions;

#endregion

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomLogger(builder.Configuration);

builder.AddServiceDefaults();

builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.AddNpgsqlDbContext<AppDbContext>("communiqueuedb", null,
    options => { options.UseSnakeCaseNamingConvention(); });

builder.AddNpgsqlDbContext<TenantStoreDbContext>("communiqueuedb", null,
    options => { options.UseSnakeCaseNamingConvention(); });

builder.Services.AddMultiTenant<AppTenantInfo>()
    .WithEFCoreStore<TenantStoreDbContext, AppTenantInfo>()
    .WithRouteStrategy("tenantId");

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

builder.Services.AddPagedResponseBuilder();
ServiceExtensions.AddCachingService(builder.Services);
ServiceExtensions.AddDateTimeProvider(builder.Services);
builder.Services.AddDataServices();
builder.Services.AddAppServices();

var app = builder.Build();

app.UseMultiTenant();

app.MapDefaultEndpoints();

app.UseCors();

app.UseHttpsRedirection();

app.UseCorrelationTokenMiddleware();
app.UseRequestResponseLogging();

await app.MigrateDatabaseAsync();

app.UseMiddleware<TenantResolutionMiddleware>();

app.MapProjectEndpoints();
app.MapContainerEndpoints();
app.MapTemplateEndpoints();
app.MapApiKeyEndpoints();
app.MapPermissionEndpoints();
app.MapUserEndpoints();
app.MapTenantEndpoints();
app.MapTenantUserManagementEndpoints();

await app.RunAsync();
