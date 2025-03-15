using System.Net;
using System.Net.Http.Json;
using Youtube.Api.Models;
using Youtube.Api.Tests.Infrastructure;

namespace Youtube.Api.Tests;

public class ContentCreatorEndpointsTests(CustomWebAppFactory factory) : IntegrationTestBase(factory)
{
    // Tests here
    // 1. Get All returns all content creators
    [Fact]
    public async Task GetAll_ReturnsAllContentCreators()
    {
        // Arrange
        // two content creators
        var contentCreator1 = TestData.CreateTestContentCreator(
            channelName: "test-channel-1",
            DisplayName: "Test Channel 1",
            description: "Test descriptor 1",
            subscriberCount: 1000);

        var contentCreator2 = TestData.CreateTestContentCreator(
            channelName: "test-channel-2",
            DisplayName: "Test Channel 2",
            description: "Test descriptor 2",
            subscriberCount: 2000);

        await CleanDatabaseAsync();
        await AddContentCreatorAsync(contentCreator1);
        await AddContentCreatorAsync(contentCreator2);

        // Act
        var response = await _client.GetAsync("/api/content-creators");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var contentCreators = await response.Content.ReadFromJsonAsync<List<ContentCreator>>();
        Assert.NotNull(contentCreators);
        Assert.Equal(2, contentCreators.Count);
        
        var returnedCreator1 = contentCreators.FirstOrDefault(c => c.Id == contentCreator1.Id);
        var returnedCreator2 = contentCreators.FirstOrDefault(c => c.Id == contentCreator2.Id);
        
        Assert.NotNull(returnedCreator1);
        Assert.NotNull(returnedCreator2);

        Assert.Equal(contentCreator1.ChannelName, returnedCreator1.ChannelName);
        Assert.Equal(contentCreator1.DisplayName, returnedCreator1.DisplayName);
        Assert.Equal(contentCreator1.Description, returnedCreator1.Description);
        Assert.Equal(contentCreator1.SubscriberCount, returnedCreator1.SubscriberCount);
        
        Assert.Equal(contentCreator2.ChannelName, returnedCreator2.ChannelName);
        Assert.Equal(contentCreator2.DisplayName, returnedCreator2.DisplayName);
        Assert.Equal(contentCreator2.Description, returnedCreator2.Description);
        Assert.Equal(contentCreator2.SubscriberCount, returnedCreator2.SubscriberCount);
    }

    // 2. Get By Id with valid id returns a content creator 
    [Fact]
    public async Task GetById_WithValidId_ReturnsContentCreator()
    {
        // Arrange
        var contentCreator = TestData.CreateTestContentCreator(
            channelName: "test-channel-1",
            DisplayName: "Test Channel 1",
            description: "Test descriptor 1",
            subscriberCount: 1000);
        await CleanDatabaseAsync();
        await AddContentCreatorAsync(contentCreator);
        // Act
        var response = await _client.GetAsync($"/api/content-creators/{contentCreator.Id}");

        // Assert
        response.EnsureSuccessStatusCode();

        var returnedContentCreator = await response.Content.ReadFromJsonAsync<ContentCreator>();
        Assert.NotNull(returnedContentCreator);

        Assert.Equal(contentCreator.ChannelName, returnedContentCreator.ChannelName);
        Assert.Equal(contentCreator.DisplayName, returnedContentCreator.DisplayName);
        Assert.Equal(contentCreator.Description, returnedContentCreator.Description);
        Assert.Equal(contentCreator.SubscriberCount, returnedContentCreator.SubscriberCount);
    }

    // 3. Get By Id with invalid id returns not found
    [Fact]
    public async Task GetById_WithInvalid_ReturnsNotFound()
    {
        // Arrange
        await CleanDatabaseAsync();
        
        // Act
        var response = await _client.GetAsync($"/api/content-creators/{Guid.NewGuid()}");
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // 4. Create with valid channel id returns created content creator 
    [Fact]
    public async Task Create_WithValidChannelId_ReturnsCreatedContentCreator()
    {
        // Arrange
        var channelId = "UC_test-channel";
        var request = new CreateContentCreatorRequest
        {
            ChannelId = channelId
        };

        // youtube response from TestData
        var youtubeResponse = TestData.CreateTestYouTubeChannelResponse(
            channelId: channelId,
            title: "Test Channel",
            description: "Test description",
            customUrl: "testchannel",
            subscriberCount: "5000");

        // setup youtube api mock
        SetupYoutubeApiMock(channelId, youtubeResponse);

        // Act
        var response = await _client.PostAsJsonAsync("/api/content-creators", request);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var returnedContentCreator = await response.Content.ReadFromJsonAsync<ContentCreator>();
        Assert.NotNull(returnedContentCreator);
        Assert.Equal("testchannel", returnedContentCreator.ChannelName);
        Assert.Equal("Test Channel", returnedContentCreator.DisplayName);
        Assert.Equal("Test description", returnedContentCreator.Description);
        Assert.Equal(5000, returnedContentCreator.SubscriberCount);

        // Verify it was saved database
        var savedContentCreator = await GetContentCreatorByIdAsync(returnedContentCreator.Id);
        Assert.NotNull(savedContentCreator);
        Assert.Equal(savedContentCreator.Id, savedContentCreator.Id);
        Assert.Equal(savedContentCreator.ChannelName, savedContentCreator.ChannelName);
        Assert.Equal(savedContentCreator.DisplayName, savedContentCreator.DisplayName);
        Assert.Equal(savedContentCreator.Description, savedContentCreator.Description);
        Assert.Equal(savedContentCreator.SubscriberCount, savedContentCreator.SubscriberCount);
    }

    // 5. Create with invalid channel id returns not found
    [Fact]
    public async Task Create_WithInvalidChannelId_ReturnsNotFound()
    {
        // Arrange
        var channelId = "invalid-channel-id";
        var request = new CreateContentCreatorRequest
        {
            ChannelId = channelId
        };

        // setup youtube api mock
        SetupYoutubeApiNotFoundMock(channelId);
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/content-creators", request);
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // 6. Create with existing channel name returns conflict
    [Fact]
    public async Task Create_WithExistingChannelName_ReturnsConflict()
    {
        // Arrange
        // existing channel name and content creator
        var existingChannelName = "existing-channel";
        var existingContentCreator = TestData.CreateTestContentCreator(
            channelName: existingChannelName,
            DisplayName: "Existing Channel",
            description: "Existing description",
            subscriberCount: 1000);

        await AddContentCreatorAsync(existingContentCreator);

        var channelId = "UC_existing-channel";
        var request = new CreateContentCreatorRequest
        {
            ChannelId = channelId
        };

        // youtube response from TestData
        var youtubeResponse = TestData.CreateTestYouTubeChannelResponse(
            channelId: channelId,
            title: "Existing Channel",
            description: "Existing description",
            customUrl: "existing-channel",
            subscriberCount: "5000");

        // setup youtube api mock
        SetupYoutubeApiMock(channelId, youtubeResponse);

        // Act
        var response = await _client.PostAsJsonAsync("/api/content-creators", request);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
}
