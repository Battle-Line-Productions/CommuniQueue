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
// File: ContainerEndpointsTests.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.UnitTests\Api\Handlers\ContainerEndpointsTests.cs
// ---------------------------------------------------------------------------
#endregion

using BattlelineExtras.Contracts.Models;
using CommuniQueue.Api.Handlers;
using CommuniQueue.Contracts.Interfaces;
using CommuniQueue.Contracts.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;

namespace CommuniQueue.UnitTests.Api.Handlers;

public class ContainerEndpointsTests
{
    private readonly IContainerService _mockContainerService = Substitute.For<IContainerService>();

    //[Fact]
    //public async Task CreateContainer_ReturnsCorrectResult()
    //{
    //    var projectId = Guid.CreateVersion7();
    //    var parentId = Guid.CreateVersion7();
    //    var request = new CreateContainerRequest("Test Container", "Test Description", projectId, parentId);
    //    var expectedContainer = new Container
    //    {
    //        Id = Guid.CreateVersion7(),
    //        Name = request.Name,
    //        Description = request.Description,
    //        ProjectId = request.ProjectId,
    //        ParentId = request.ParentContainerId,
    //        CreatedDateTime = DateTime.UtcNow,
    //        UpdatedDateTime = DateTime.UtcNow,
    //        IsRoot = false
    //    };
    //    var expectedResult = new ResponseDetail<Container>
    //    {
    //        Status = ResultStatus.Created201,
    //        Title = "Container Created",
    //        SubCode = "ContainerCreated",
    //        Data = expectedContainer
    //    };
    //    _mockContainerService.CreateContainerAsync(request.Name, request.Description, request.ProjectId, request.ParentContainerId)
    //        .Returns(expectedResult);

    //    var result = await ContainerEndpoints.CreateContainer(_mockContainerService, request);

    //    var createdResult = Assert.IsType<Created<ResponseDetail<Container>>>(result);
    //    Assert.Equal(expectedResult, createdResult.Value);
    //}

    [Fact]
    public async Task GetContainerById_ReturnsCorrectResult()
    {
        var containerId = Guid.CreateVersion7();
        var expectedContainer = new Container
        {
            Id = containerId,
            Name = "Test Container",
            Description = "Test Description",
            ProjectId = Guid.CreateVersion7(),
            CreatedDateTime = DateTime.UtcNow,
            UpdatedDateTime = DateTime.UtcNow,
            IsRoot = false
        };
        var expectedResult = new ResponseDetail<Container>
        {
            Status = ResultStatus.Ok200,
            Title = "Container Retrieved",
            SubCode = "ContainerRetrieved",
            Data = expectedContainer
        };
        _mockContainerService.GetContainerByIdAsync(containerId).Returns(expectedResult);

        var result = await ContainerEndpoints.GetContainerById(_mockContainerService, containerId);

        var okResult = Assert.IsType<Ok<ResponseDetail<Container>>>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }

    [Fact]
    public async Task GetContainersByProjectId_ReturnsCorrectResult()
    {
        var projectId = Guid.CreateVersion7();
        var containers = new List<Container>
        {
            new()
            {
                Id = Guid.CreateVersion7(),
                Name = "Container 1",
                Description = "Description 1",
                ProjectId = projectId,
                CreatedDateTime = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow,
                IsRoot = false
            },
            new()
            {
                Id = Guid.CreateVersion7(),
                Name = "Container 2",
                Description = "Description 2",
                ProjectId = projectId,
                CreatedDateTime = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow,
                IsRoot = false
            }
        };
        var expectedResult = new ResponseDetail<List<Container>>
        {
            Status = ResultStatus.Ok200,
            Title = "Containers Retrieved",
            SubCode = "ContainersRetrieved",
            Data = containers
        };
        _mockContainerService.GetContainersByProjectIdAsync(projectId).Returns(expectedResult);

        var result = await ContainerEndpoints.GetContainersByProjectId(_mockContainerService, projectId);

        var okResult = Assert.IsType<Ok<ResponseDetail<List<Container>>>>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }

    //[Fact]
    //public async Task UpdateContainer_ReturnsCorrectResult()
    //{
    //    var containerId = Guid.CreateVersion7();
    //    var request = new UpdateContainerRequest("Updated Name", "Updated Description");
    //    var expectedContainer = new Container
    //    {
    //        Id = containerId,
    //        Name = request.Name,
    //        Description = request.Description,
    //        ProjectId = Guid.CreateVersion7(),
    //        CreatedDateTime = DateTime.UtcNow,
    //        UpdatedDateTime = DateTime.UtcNow,
    //        IsRoot = false
    //    };
    //    var expectedResult = new ResponseDetail<Container>
    //    {
    //        Status = ResultStatus.Ok200,
    //        Title = "Container Updated",
    //        SubCode = "ContainerUpdated",
    //        Data = expectedContainer
    //    };
    //    _mockContainerService.UpdateContainerAsync(containerId, request.Name, request.Description)
    //        .Returns(expectedResult);

    //    var result = await ContainerEndpoints.UpdateContainer(_mockContainerService, containerId, request);

    //    var okResult = Assert.IsType<Ok<ResponseDetail<Container>>>(result);
    //    Assert.Equal(expectedResult, okResult.Value);
    //}

    //[Fact]
    //public async Task DeleteContainer_ReturnsCorrectResult()
    //{
    //    var containerId = Guid.CreateVersion7();
    //    var expectedResult = new ResponseDetail<bool>
    //    {
    //        Status = ResultStatus.Ok200,
    //        Title = "Container Deleted",
    //        SubCode = "ContainerDeleted",
    //        Data = true
    //    };
    //    _mockContainerService.DeleteContainerAsync(containerId).Returns(expectedResult);

    //    var result = await ContainerEndpoints.DeleteContainer(_mockContainerService, containerId);

    //    var okResult = Assert.IsType<Ok<ResponseDetail<bool>>>(result);
    //    Assert.Equal(expectedResult, okResult.Value);
    //}

    [Fact]
    public async Task GetChildContainers_ReturnsCorrectResult()
    {
        var parentContainerId = Guid.CreateVersion7();
        var children = new List<Container>
        {
            new()
            {
                Id = Guid.CreateVersion7(),
                Name = "Child Container 1",
                Description = "Child Description 1",
                ProjectId = Guid.CreateVersion7(),
                ParentId = parentContainerId,
                CreatedDateTime = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow,
                IsRoot = false
            },
            new()
            {
                Id = Guid.CreateVersion7(),
                Name = "Child Container 2",
                Description = "Child Description 2",
                ProjectId = Guid.CreateVersion7(),
                ParentId = parentContainerId,
                CreatedDateTime = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow,
                IsRoot = false
            }
        };
        var expectedResult = new ResponseDetail<List<Container>>
        {
            Status = ResultStatus.Ok200,
            Title = "Child Containers Retrieved",
            SubCode = "ChildContainersRetrieved",
            Data = children
        };
        _mockContainerService.GetChildContainersAsync(parentContainerId).Returns(expectedResult);

        var result = await ContainerEndpoints.GetChildContainers(_mockContainerService, parentContainerId);

        var okResult = Assert.IsType<Ok<ResponseDetail<List<Container>>>>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }

    //[Fact]
    //public async Task MoveContainer_ReturnsCorrectResult()
    //{
    //    var containerId = Guid.CreateVersion7();
    //    var newParentId = Guid.CreateVersion7();
    //    var request = new MoveContainerRequest(newParentId);
    //    var expectedResult = new ResponseDetail<bool>
    //    {
    //        Status = ResultStatus.Ok200,
    //        Title = "Container Moved",
    //        SubCode = "ContainerMoved",
    //        Data = true
    //    };
    //    _mockContainerService.MoveContainerAsync(containerId, request.NewParentContainerId)
    //        .Returns(expectedResult);

    //    var result = await ContainerEndpoints.MoveContainer(_mockContainerService, containerId, request);

    //    var okResult = Assert.IsType<Ok<ResponseDetail<bool>>>(result);
    //    Assert.Equal(expectedResult, okResult.Value);
    //}

    //[Fact]
    //public async Task CreateContainer_WithNullParentId_ReturnsCorrectResult()
    //{
    //    var projectId = Guid.CreateVersion7();
    //    var request = new CreateContainerRequest("Root Container", "Root Description", projectId, null);
    //    var expectedContainer = new Container
    //    {
    //        Id = Guid.CreateVersion7(),
    //        Name = request.Name,
    //        Description = request.Description,
    //        ProjectId = request.ProjectId,
    //        ParentId = null,
    //        CreatedDateTime = DateTime.UtcNow,
    //        UpdatedDateTime = DateTime.UtcNow,
    //        IsRoot = true
    //    };
    //    var expectedResult = new ResponseDetail<Container>
    //    {
    //        Status = ResultStatus.Created201,
    //        Title = "Container Created",
    //        SubCode = "ContainerCreated",
    //        Data = expectedContainer
    //    };
    //    _mockContainerService.CreateContainerAsync(request.Name, request.Description, request.ProjectId, null)
    //        .Returns(expectedResult);

    //    var result = await ContainerEndpoints.CreateContainer(_mockContainerService, request);

    //    var createdResult = Assert.IsType<Created<ResponseDetail<Container>>>(result);
    //    Assert.Equal(expectedResult, createdResult.Value);
    //    Assert.Null(createdResult.Value.Data.ParentId);
    //    Assert.True(createdResult.Value.Data.IsRoot);
    //}

    //[Fact]
    //public async Task MoveContainer_WithNullNewParentId_ReturnsCorrectResult()
    //{
    //    var containerId = Guid.CreateVersion7();
    //    var request = new MoveContainerRequest(null);
    //    var expectedResult = new ResponseDetail<bool>
    //    {
    //        Status = ResultStatus.Ok200,
    //        Title = "Container Moved",
    //        SubCode = "ContainerMoved",
    //        Data = true
    //    };
    //    _mockContainerService.MoveContainerAsync(containerId, null)
    //        .Returns(expectedResult);

    //    var result = await ContainerEndpoints.MoveContainer(_mockContainerService, containerId, request);

    //    var okResult = Assert.IsType<Ok<ResponseDetail<bool>>>(result);
    //    Assert.Equal(expectedResult, okResult.Value);
    //}

    [Fact]
    public async Task GetContainerById_NotFound_ReturnsCorrectResult()
    {
        var containerId = Guid.CreateVersion7();
        var expectedResult = new ResponseDetail<Container>
        {
            Status = ResultStatus.NotFound404,
            Title = "Container Not Found",
            SubCode = "ContainerNotFound",
            Data = null
        };
        _mockContainerService.GetContainerByIdAsync(containerId).Returns(expectedResult);

        var result = await ContainerEndpoints.GetContainerById(_mockContainerService, containerId);

        var notFoundResult = Assert.IsType<NotFound<ResponseDetail<Container>>>(result);
        Assert.Equal(expectedResult, notFoundResult.Value);
    }

    //[Fact]
    //public async Task UpdateContainer_NotFound_ReturnsCorrectResult()
    //{
    //    var containerId = Guid.CreateVersion7();
    //    var request = new UpdateContainerRequest("Updated Name", "Updated Description");
    //    var expectedResult = new ResponseDetail<Container>
    //    {
    //        Status = ResultStatus.NotFound404,
    //        Title = "Container Not Found",
    //        SubCode = "ContainerNotFound",
    //        Data = null
    //    };
    //    _mockContainerService.UpdateContainerAsync(containerId, request.Name, request.Description)
    //        .Returns(expectedResult);

    //    var result = await ContainerEndpoints.UpdateContainer(_mockContainerService, containerId, request);

    //    var notFoundResult = Assert.IsType<NotFound<ResponseDetail<Container>>>(result);
    //    Assert.Equal(expectedResult, notFoundResult.Value);
    //}
}
