using Microsoft.Extensions.Options;
using Youtube.Api.Configuration;
using Youtube.Api.Models;

namespace Youtube.Api.Services;

public interface IYoutubeApiService
{
    Task<YouTubeChannelResponse?> GetChannelInfoAsync(string requestChannelId);
}

public class YoutubeApiService(
    HttpClient httpClient,
    IOptions<YoutubeApiOptions> options,
    ILogger<YoutubeApiService> logger) : IYoutubeApiService
{
    public async Task<YouTubeChannelResponse?> GetChannelInfoAsync(string channelId)
    {
        logger.LogInformation("Fetching channel info for channel ID: {ChannelId}", channelId);

        try
        {
            var requestUri = $"{options.Value.ApiUrl}/channels?part=statistics," +
                             $"snippet&id={channelId}&key={options.Value.ApiKey}";

            var response = await httpClient.GetFromJsonAsync<YouTubeChannelResponse>(requestUri);

            if (response?.Items is null || response.Items.Count == 0)
            {
                logger.LogWarning("No channel found with ID: {ChannelId}", channelId);
                return null;
            }

            return response;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error fetching channel info for channel ID: {ChannelId}", channelId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error fetching channel info for channel ID: {ChannelId}", channelId);
            throw;
        }
    }

}