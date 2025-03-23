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
// File: ApiKeyEndpointTests.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.UnitTests\Api\Handlers\ApiKeyEndpointTests.cs
// ---------------------------------------------------------------------------
#endregion

using BattlelineExtras.Contracts.Models;
using CommuniQueue.Api.Handlers;
using CommuniQueue.Contracts.Interfaces;
using CommuniQueue.Contracts.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;

namespace CommuniQueue.UnitTests.Api.Handlers;

public class ApiKeyEndpointsTests
{
    private readonly IApiKeyService _mockApiKeyService = Substitute.For<IApiKeyService>();

    //[Fact]
    //public async Task GenerateApiKey_ReturnsCorrectResult()
    //{
    //    var request = new GenerateApiKeyRequest(Guid.NewGuid(), DateTime.Now, DateTime.Now.AddDays(30),
    //        ["scope1", "scope2"]);
    //    var expectedResult = new ResponseDetail<(ApiKey?, string?)>
    //    {
    //        Status = ResultStatus.Created201,
    //        Title = "API Key Generated",
    //        SubCode = "ApiKeyGenerated",
    //        Data = (new ApiKey(), "generated_key")
    //    };
    //    _mockApiKeyService.GenerateApiKeyAsync(request.ProjectId, request.StartDate, request.EndDate, request.Scopes)
    //        .Returns(expectedResult);

    //    var result = await ApiKeyEndpoints.GenerateApiKey(_mockApiKeyService, request);

    //    var okResult = Assert.IsType<Created<ResponseDetail<(ApiKey?, string?)>>>(result);
    //    Assert.Equal(expectedResult, okResult.Value);
    //}

    [Fact]
    public async Task GetApiKeyById_ReturnsCorrectResult()
    {
        var apiKeyId = Guid.NewGuid();
        var expectedResult = new ResponseDetail<ApiKey>
        {
            Status = ResultStatus.Ok200,
            Title = "API Key Retrieved",
            SubCode = "ApiKeyRetrieved",
            Data = new()
        };
        _mockApiKeyService.GetApiKeyByIdAsync(apiKeyId).Returns(expectedResult);

        var result = await ApiKeyEndpoints.GetApiKeyById(_mockApiKeyService, apiKeyId);

        var okResult = Assert.IsType<Ok<ResponseDetail<ApiKey>>>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }

    [Fact]
    public async Task GetApiKeysByProjectId_ReturnsCorrectResult()
    {
        var projectId = Guid.NewGuid();
        var expectedResult = new ResponseDetail<List<ApiKey>>
        {
            Status = ResultStatus.Ok200,
            Title = "API Keys Retrieved",
            SubCode = "ApiKeysRetrieved",
            Data = [new ApiKey(), new ApiKey()]
        };
        _mockApiKeyService.GetApiKeysByProjectIdAsync(projectId).Returns(expectedResult);

        var result = await ApiKeyEndpoints.GetApiKeysByProjectId(_mockApiKeyService, projectId);

        var okResult = Assert.IsType<Ok<ResponseDetail<List<ApiKey>>>>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }

    [Fact]
    public async Task ValidateApiKey_ReturnsCorrectResult()
    {
        var request = new ValidateApiKeyRequest("test_api_key");
        var expectedResult = new ResponseDetail<bool>
        {
            Status = ResultStatus.Ok200,
            Title = "API Key Validated",
            SubCode = "ApiKeyValidated",
            Data = true
        };
        _mockApiKeyService.ValidateApiKeyAsync(request.ApiKey).Returns(expectedResult);

        var result = await ApiKeyEndpoints.ValidateApiKey(_mockApiKeyService, request);

        var okResult = Assert.IsType<Ok<ResponseDetail<bool>>>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }

    //[Fact]
    //public async Task ExpireApiKey_ReturnsCorrectResult()
    //{
    //    var apiKeyId = Guid.NewGuid();
    //    var expectedResult = new ResponseDetail<bool>
    //    {
    //        Status = ResultStatus.Ok200,
    //        Title = "API Key Expired",
    //        SubCode = "ApiKeyExpired",
    //        Data = true
    //    };
    //    _mockApiKeyService.ExpireApiKeyAsync(apiKeyId).Returns(expectedResult);

    //    var result = await ApiKeyEndpoints.ExpireApiKey(_mockApiKeyService, apiKeyId);

    //    var okResult = Assert.IsType<Ok<ResponseDetail<bool>>>(result);
    //    Assert.Equal(expectedResult, okResult.Value);
    //}

    [Fact]
    public async Task HasValidApiKey_ReturnsCorrectResult()
    {
        var projectId = Guid.NewGuid();
        var expectedResult = new ResponseDetail<bool>
        {
            Status = ResultStatus.Ok200,
            Title = "Valid API Key Check",
            SubCode = "ValidApiKeyCheck",
            Data = true
        };
        _mockApiKeyService.HasValidApiKeyAsync(projectId).Returns(expectedResult);

        var result = await ApiKeyEndpoints.HasValidApiKey(_mockApiKeyService, projectId);

        var okResult = Assert.IsType<Ok<ResponseDetail<bool>>>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }
}
