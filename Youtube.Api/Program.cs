using Microsoft.EntityFrameworkCore;
using Youtube.Api.Configuration;
using Youtube.Api.Data;
using Youtube.Api.Extensions;
using Youtube.Api.Models;
using Youtube.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
});

builder.Services.Configure<YoutubeApiOptions>(
    builder.Configuration.GetSection(YoutubeApiOptions.SectionName));

builder.Services.AddHttpClient<IYoutubeApiService, YoutubeApiService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var contentCreatorsGroup = app.MapGroup("/api/content-creators");

contentCreatorsGroup.MapGet("/", async (AppDbContext dbContext, ILogger<Program> logger) =>
{
    logger.LogWarning("Getting all content creators");
    return await dbContext.ContentCreators.ToListAsync();
});

// GET content creator by ID
contentCreatorsGroup.MapGet("/{id:guid}", async (
    Guid id,
    AppDbContext dbContext,
    ILogger<Program> logger) =>
{
    logger.LogInformation("Getting content creator with ID: {Id}", id);

    var contentCreator = await dbContext.ContentCreators.FindAsync(id);

    if (contentCreator is null)
    {
        logger.LogWarning("Content creator with ID {Id} not found", id);
        return Results.NotFound();
    }

    return Results.Ok(contentCreator);
});

// POST content creator
contentCreatorsGroup.MapPost("/", async (
    CreatorContentCreatorRequest request,
    AppDbContext dbContext,
    IYoutubeApiService youtubeApiService,
    ILogger<Program> logger) =>
{
    logger.LogInformation("Creating new content creator from channel ID: {ChannelId}",
        request.ChannelId);

    // Get channel info from YouTube API
    var channelInfo = await youtubeApiService.GetChannelInfoAsync(request.ChannelId);

    if (channelInfo?.Items is null || channelInfo.Items.Count == 0)
    {
        logger.LogWarning("Channel with ID {ChannelId} not found", request.ChannelId);
        return Results.NotFound($"Channel with ID '{request.ChannelId}' not found");
    }

    var channelItem = channelInfo.Items[0];

    // Check if a content creator with the same channel ID already exists
    if (await dbContext.ContentCreators.AnyAsync(cc => cc.ChannelName == channelItem.Snippet.CustomUrl))
    {
        logger.LogWarning("Content creator with channel ID {ChannelId} already exists", 
            request.ChannelId);
        return Results.Conflict(
            $"Content creator with channel name '{channelItem.Snippet.CustomUrl}' already exists");
    }

    // Parse subscriber count
    int.TryParse(channelItem.Statistics.SubscriberCount, out var subscriberCount);

    // Create new Content creator from channel info
    var contentCreator = new ContentCreator
    {
        Id = Guid.NewGuid(),    
        ChannelName = channelItem.Snippet.CustomUrl ?? channelItem.Id ?? string.Empty,
        DisplayName = channelItem.Snippet.Title ?? string.Empty,
        Description = channelItem.Snippet.Description,
        SubscriberCount = subscriberCount,
        CreatedAt = DateTime.UtcNow
    };

    dbContext.ContentCreators.Add(contentCreator);
    await dbContext.SaveChangesAsync();

    logger.LogInformation("Content creator created successfully with ID: {Id}", contentCreator.Id);

    return Results.Created($"/api/content-creators/{contentCreator.Id}", contentCreator);
});

// Apply migrations automatically
app.ApplyMigrations();

app.Run();