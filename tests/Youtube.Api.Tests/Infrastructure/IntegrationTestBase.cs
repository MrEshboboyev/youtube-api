using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Youtube.Api.Data;
using Youtube.Api.Models;

namespace Youtube.Api.Tests.Infrastructure;

public abstract class IntegrationTestBase : IClassFixture<CustomWebAppFactory>, IAsyncLifetime
{
    protected readonly HttpClient _client;
    protected readonly CustomWebAppFactory _factory;
    
    protected IntegrationTestBase(CustomWebAppFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        await _factory.InitializeAsync();
    }

    public async Task DisposeAsync()
    {
        await _factory.DisposeAsync();
    }

    // Get Content Creator By id
    protected async Task<ContentCreator?> GetContentCreatorByIdAsync(Guid id)
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await dbContext.ContentCreators.FindAsync(id);
    }

    // Create Content Creator
    protected async Task AddContentCreatorAsync(ContentCreator contentCreator)
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.ContentCreators.Add(contentCreator);
        await dbContext.SaveChangesAsync();
    }

    // Clean Database async
    protected async Task CleanDatabaseAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.ContentCreators.ExecuteDeleteAsync();
    }

    // Setup YouTube api mock with channelId and YouTubeChannelResponse
    protected void SetupYoutubeApiMock(string channelId, YouTubeChannelResponse response)
    {
        _factory.WireMockServer
            .Given(Request.Create()
                .WithPath($"/youtube/v3/channels")
                .WithParam("id", channelId)
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyAsJson(response));
    }

    // Setup YouTube api not found mock with channelId
    protected void SetupYoutubeApiNotFoundMock(string channelId)
    {
        _factory.WireMockServer
            .Given(Request.Create()
                .WithPath($"/youtube/v3/channels")
                .WithParam("id", channelId)
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyAsJson(new YouTubeChannelResponse
                {
                    Items = [],
                    PageInfo = new PageInfo
                    {
                        TotalResults = 0,
                        ResultsPerPage = 0
                    }
                }));
    }
}
