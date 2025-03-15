namespace Youtube.Api.Models;

public class YouTubeChannelResponse
{
    public List<ChannelItem> Items { get; set; }
    public PageInfo PageInfo { get; set; }
}