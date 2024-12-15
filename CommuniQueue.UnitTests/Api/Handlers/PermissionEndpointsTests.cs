#region Copyright
// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
//
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
//
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 12/14/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.UnitTests
// File: PermissionEndpointsTests.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.UnitTests\Api\Handlers\PermissionEndpointsTests.cs
// ---------------------------------------------------------------------------
#endregion

using BattlelineExtras.Contracts.Models;
using CommuniQueue.Api.Handlers;
using CommuniQueue.Contracts.Interfaces;
using CommuniQueue.Contracts.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;

namespace CommuniQueue.UnitTests.Api.Handlers;

public class PermissionEndpointsTests
{
    private readonly IPermissionService _mockPermissionService = Substitute.For<IPermissionService>();

    [Fact]
    public async Task CreatePermission_WithValidData_ReturnsCorrectResult()
    {
        var request = new CreatePermissionRequest(
            UserId: Guid.CreateVersion7(),
            EntityId: Guid.CreateVersion7(),
            EntityType: "Project",
            PermissionLevel: PermissionLevel.Admin
        );
        var expectedPermission = new Permission
        {
            Id = Guid.CreateVersion7(),
            UserId = request.UserId,
            EntityId = request.EntityId,
            EntityType = EntityType.Project,
            PermissionLevel = request.PermissionLevel,
            CreatedDateTime = DateTime.UtcNow,
            UpdatedDateTime = DateTime.UtcNow
        };
        var expectedResult = new ResponseDetail<Permission>
        {
            Status = ResultStatus.Created201,
            Title = "Permission Created",
            SubCode = "PermissionCreated",
            Data = expectedPermission
        };
        _mockPermissionService.CreatePermissionAsync(
            request.UserId,
            request.EntityId,
            EntityType.Project,
            request.PermissionLevel
        ).Returns(expectedResult);

        var result = await PermissionEndpoints.CreatePermission(_mockPermissionService, request);

        var createdResult = Assert.IsType<Created<ResponseDetail<Permission>>>(result);
        Assert.Equal(expectedResult, createdResult.Value);
    }

    [Fact]
    public async Task CreatePermission_WithInvalidEntityType_ReturnsBadRequest()
    {
        var request = new CreatePermissionRequest(
            UserId: Guid.CreateVersion7(),
            EntityId: Guid.CreateVersion7(),
            EntityType: "InvalidType",
            PermissionLevel: PermissionLevel.Admin
        );

        var result = await PermissionEndpoints.CreatePermission(_mockPermissionService, request);

        var badRequestResult = Assert.IsType<BadRequest<string>>(result);
        Assert.Equal($"Invalid EntityType: {request.EntityType}", badRequestResult.Value);
    }

    [Fact]
    public async Task GetPermission_WithValidData_ReturnsCorrectResult()
    {
        var userId = Guid.CreateVersion7();
        var entityId = Guid.CreateVersion7();
        var entityType = "Project";
        var expectedPermission = new Permission
        {
            Id = Guid.CreateVersion7(),
            UserId = userId,
            EntityId = entityId,
            EntityType = EntityType.Project,
            PermissionLevel = PermissionLevel.Admin,
            CreatedDateTime = DateTime.UtcNow,
            UpdatedDateTime = DateTime.UtcNow
        };
        var expectedResult = new ResponseDetail<Permission>
        {
            Status = ResultStatus.Ok200,
            Title = "Permission Retrieved",
            SubCode = "PermissionRetrieved",
            Data = expectedPermission
        };
        _mockPermissionService.GetPermissionAsync(userId, entityId, EntityType.Project)
            .Returns(expectedResult);

        var result = await PermissionEndpoints.GetPermission(_mockPermissionService, userId, entityId, entityType);

        var okResult = Assert.IsType<Ok<ResponseDetail<Permission>>>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }

    [Fact]
    public async Task GetPermission_WithInvalidEntityType_ReturnsBadRequest()
    {
        var userId = Guid.CreateVersion7();
        var entityId = Guid.CreateVersion7();
        var entityType = "InvalidType";

        var result = await PermissionEndpoints.GetPermission(_mockPermissionService, userId, entityId, entityType);

        var badRequestResult = Assert.IsType<BadRequest<string>>(result);
        Assert.Equal($"Invalid EntityType: {entityType}", badRequestResult.Value);
    }

    [Fact]
    public async Task GetPermissionsByEntity_WithValidData_ReturnsCorrectResult()
    {
        var entityId = Guid.CreateVersion7();
        var entityType = "Project";
        var expectedPermissions = new List<Permission>
        {
            new()
            {
                Id = Guid.CreateVersion7(),
                UserId = Guid.CreateVersion7(),
                EntityId = entityId,
                EntityType = EntityType.Project,
                PermissionLevel = PermissionLevel.Admin,
                CreatedDateTime = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.CreateVersion7(),
                UserId = Guid.CreateVersion7(),
                EntityId = entityId,
                EntityType = EntityType.Project,
                PermissionLevel = PermissionLevel.ReadOnly,
                CreatedDateTime = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow
            }
        };
        var expectedResult = new ResponseDetail<List<Permission>>
        {
            Status = ResultStatus.Ok200,
            Title = "Permissions Retrieved",
            SubCode = "PermissionsRetrieved",
            Data = expectedPermissions
        };
        _mockPermissionService.GetPermissionsByEntityAsync(entityId, EntityType.Project)
            .Returns(expectedResult);

        var result = await PermissionEndpoints.GetPermissionsByEntity(_mockPermissionService, entityId, entityType);

        var okResult = Assert.IsType<Ok<ResponseDetail<List<Permission>>>>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }

    [Fact]
    public async Task UpdatePermission_WithValidData_ReturnsCorrectResult()
    {
        var request = new UpdatePermissionRequest(
            UserId: Guid.CreateVersion7(),
            EntityId: Guid.CreateVersion7(),
            EntityType: "Project",
            NewPermissionLevel: PermissionLevel.Contributor
        );
        var expectedPermission = new Permission
        {
            Id = Guid.CreateVersion7(),
            UserId = request.UserId,
            EntityId = request.EntityId,
            EntityType = EntityType.Project,
            PermissionLevel = request.NewPermissionLevel,
            CreatedDateTime = DateTime.UtcNow,
            UpdatedDateTime = DateTime.UtcNow
        };
        var expectedResult = new ResponseDetail<Permission>
        {
            Status = ResultStatus.Ok200,
            Title = "Permission Updated",
            SubCode = "PermissionUpdated",
            Data = expectedPermission
        };
        _mockPermissionService.UpdatePermissionAsync(
            request.UserId,
            request.EntityId,
            EntityType.Project,
            request.NewPermissionLevel
        ).Returns(expectedResult);

        var result = await PermissionEndpoints.UpdatePermission(_mockPermissionService, request);

        var okResult = Assert.IsType<Ok<ResponseDetail<Permission>>>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }

    [Fact]
    public async Task DeletePermission_WithValidData_ReturnsCorrectResult()
    {
        var userId = Guid.CreateVersion7();
        var entityId = Guid.CreateVersion7();
        var entityType = "Project";
        var expectedResult = new ResponseDetail<bool>
        {
            Status = ResultStatus.Ok200,
            Title = "Permission Deleted",
            SubCode = "PermissionDeleted",
            Data = true
        };
        _mockPermissionService.DeletePermissionAsync(userId, entityId, EntityType.Project)
            .Returns(expectedResult);

        var result = await PermissionEndpoints.DeletePermission(_mockPermissionService, userId, entityId, entityType);

        var okResult = Assert.IsType<Ok<ResponseDetail<bool>>>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }

    [Fact]
    public async Task DeletePermission_WithInvalidEntityType_ReturnsBadRequest()
    {
        var userId = Guid.CreateVersion7();
        var entityId = Guid.CreateVersion7();
        var entityType = "InvalidType";

        var result = await PermissionEndpoints.DeletePermission(_mockPermissionService, userId, entityId, entityType);

        var badRequestResult = Assert.IsType<BadRequest<string>>(result);
        Assert.Equal($"Invalid EntityType: {entityType}", badRequestResult.Value);
    }

    [Fact]
    public async Task GetPermission_NotFound_ReturnsNotFound()
    {
        var userId = Guid.CreateVersion7();
        var entityId = Guid.CreateVersion7();
        var entityType = "Project";
        var expectedResult = new ResponseDetail<Permission>
        {
            Status = ResultStatus.NotFound404,
            Title = "Permission Not Found",
            SubCode = "PermissionNotFound",
            Data = null
        };
        _mockPermissionService.GetPermissionAsync(userId, entityId, EntityType.Project)
            .Returns(expectedResult);

        var result = await PermissionEndpoints.GetPermission(_mockPermissionService, userId, entityId, entityType);

        var notFoundResult = Assert.IsType<NotFound<ResponseDetail<Permission>>>(result);
        Assert.Equal(expectedResult, notFoundResult.Value);
    }

    [Fact]
    public async Task UpdatePermission_WithInvalidEntityType_ReturnsBadRequest()
    {
        var request = new UpdatePermissionRequest(
            UserId: Guid.CreateVersion7(),
            EntityId: Guid.CreateVersion7(),
            EntityType: "InvalidType",
            NewPermissionLevel: PermissionLevel.Contributor
        );

        var result = await PermissionEndpoints.UpdatePermission(_mockPermissionService, request);

        var badRequestResult = Assert.IsType<BadRequest<string>>(result);
        Assert.Equal($"Invalid EntityType: {request.EntityType}", badRequestResult.Value);
    }

    [Fact]
    public async Task UpdatePermission_NotFound_ReturnsNotFound()
    {
        var request = new UpdatePermissionRequest(
            UserId: Guid.CreateVersion7(),
            EntityId: Guid.CreateVersion7(),
            EntityType: "Project",
            NewPermissionLevel: PermissionLevel.Contributor
        );
        var expectedResult = new ResponseDetail<Permission>
        {
            Status = ResultStatus.NotFound404,
            Title = "Permission Not Found",
            SubCode = "PermissionNotFound",
            Data = null
        };
        _mockPermissionService.UpdatePermissionAsync(
            request.UserId,
            request.EntityId,
            EntityType.Project,
            request.NewPermissionLevel
        ).Returns(expectedResult);

        var result = await PermissionEndpoints.UpdatePermission(_mockPermissionService, request);

        var notFoundResult = Assert.IsType<NotFound<ResponseDetail<Permission>>>(result);
        Assert.Equal(expectedResult, notFoundResult.Value);
    }

    [Theory]
    [InlineData("Project")]
    [InlineData("Container")]
    [InlineData("Template")]
    public async Task CreatePermission_WithAllValidEntityTypes_ReturnsCorrectResult(string entityType)
    {
        var request = new CreatePermissionRequest(
            UserId: Guid.CreateVersion7(),
            EntityId: Guid.CreateVersion7(),
            EntityType: entityType,
            PermissionLevel: PermissionLevel.Admin
        );
        var parsedEntityType = Enum.Parse<EntityType>(entityType);
        var expectedPermission = new Permission
        {
            Id = Guid.CreateVersion7(),
            UserId = request.UserId,
            EntityId = request.EntityId,
            EntityType = parsedEntityType,
            PermissionLevel = request.PermissionLevel,
            CreatedDateTime = DateTime.UtcNow,
            UpdatedDateTime = DateTime.UtcNow
        };
        var expectedResult = new ResponseDetail<Permission>
        {
            Status = ResultStatus.Created201,
            Title = "Permission Created",
            SubCode = "PermissionCreated",
            Data = expectedPermission
        };
        _mockPermissionService.CreatePermissionAsync(
            request.UserId,
            request.EntityId,
            parsedEntityType,
            request.PermissionLevel
        ).Returns(expectedResult);

        var result = await PermissionEndpoints.CreatePermission(_mockPermissionService, request);

        var createdResult = Assert.IsType<Created<ResponseDetail<Permission>>>(result);
        Assert.Equal(expectedResult, createdResult.Value);
    }

    [Theory]
    [InlineData(PermissionLevel.ReadOnly)]
    [InlineData(PermissionLevel.Contributor)]
    [InlineData(PermissionLevel.Admin)]
    [InlineData(PermissionLevel.SuperAdmin)]
    public async Task CreatePermission_WithAllPermissionLevels_ReturnsCorrectResult(PermissionLevel permissionLevel)
    {
        var request = new CreatePermissionRequest(
            UserId: Guid.CreateVersion7(),
            EntityId: Guid.CreateVersion7(),
            EntityType: "Project",
            PermissionLevel: permissionLevel
        );
        var expectedPermission = new Permission
        {
            Id = Guid.CreateVersion7(),
            UserId = request.UserId,
            EntityId = request.EntityId,
            EntityType = EntityType.Project,
            PermissionLevel = permissionLevel,
            CreatedDateTime = DateTime.UtcNow,
            UpdatedDateTime = DateTime.UtcNow
        };
        var expectedResult = new ResponseDetail<Permission>
        {
            Status = ResultStatus.Created201,
            Title = "Permission Created",
            SubCode = "PermissionCreated",
            Data = expectedPermission
        };
        _mockPermissionService.CreatePermissionAsync(
            request.UserId,
            request.EntityId,
            EntityType.Project,
            permissionLevel
        ).Returns(expectedResult);

        var result = await PermissionEndpoints.CreatePermission(_mockPermissionService, request);

        var createdResult = Assert.IsType<Created<ResponseDetail<Permission>>>(result);
        Assert.Equal(expectedResult, createdResult.Value);
        Assert.Equal(permissionLevel, createdResult.Value.Data.PermissionLevel);
    }
}
