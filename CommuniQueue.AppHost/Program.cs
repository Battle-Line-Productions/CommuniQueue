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
// Project Name: CommuniQueue.AppHost
// File: Program.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.AppHost\Program.cs
// ---------------------------------------------------------------------------

#endregion

#region Usings

using Projects;

#endregion

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<CommuniQueue_Api>("communiqueue-api");

builder.Build().Run();
