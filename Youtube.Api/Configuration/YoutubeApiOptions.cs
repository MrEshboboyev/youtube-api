namespace Youtube.Api.Configuration;

public class YoutubeApiOptions
{
    public static string SectionName = "YoutubeOptions";
    public string ApiUrl { get; set; }
    public string ApiKey { get; set; }
}