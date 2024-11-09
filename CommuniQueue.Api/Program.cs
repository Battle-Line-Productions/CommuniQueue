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

using Asp.Versioning;
using BattlelineExtras.Http.Extensions;
using CommuniQueue;
using CommuniQueue.Api.Extensions;
using CommuniQueue.Api.Handlers;
using CommuniQueue.DataAccess;
using Microsoft.EntityFrameworkCore;
using ServiceExtensions = BattlelineExtras.Services.Extensions.ServiceExtensions;

#endregion

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomLogger(builder.Configuration);

builder.AddServiceDefaults();

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

builder.AddNpgsqlDbContext<AppDbContext>("communiqueuedb", null,
    options => { options.UseSnakeCaseNamingConvention(); });

builder.Services.AddPagedResponseBuilder();
ServiceExtensions.AddCachingService(builder.Services);
ServiceExtensions.AddDateTimeProvider(builder.Services);
builder.Services.AddDataServices();
builder.Services.AddAppServices();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.UseCorrelationTokenMiddleware();
app.UseRequestResponseLogging();

app.MigrateDatabase();

app.MapProjectEndpoints();
app.MapContainerEndpoints();
app.MapTemplateEndpoints();
app.MapApiKeyEndpoints();

app.Run();
