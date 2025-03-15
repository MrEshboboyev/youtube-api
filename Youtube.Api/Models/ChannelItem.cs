namespace Youtube.Api.Models;

public class ChannelItem
{
    public string Id { get; set; }
    public Statistics Statistics { get; set; }
    public Snippet Snippet { get; set; }
}

public class Snippet
{
    public string CustomUrl { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}