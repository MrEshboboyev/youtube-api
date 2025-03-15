namespace Youtube.Api.Models;

public class ContentCreator
{
    public Guid Id { get; set; }
    public string ChannelName { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public int SubscriberCount { get; set; }
    public DateTime CreatedAt { get; set; }
}