using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;
using WireMock.Server;

namespace Youtube.Api.Tests.Infrastructure;

public class CustomWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:17.2")
        .WithDatabase("youtube_test_db")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithCleanUp(true)
        .WithAutoRemove(true)
        .Build();

    public WireMockServer WireMockServer { get; set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:Database", _dbContainer.GetConnectionString());
        builder.UseSetting("YouTube:ApiUrl", $"{WireMockServer.Urls[0]}/youtube/v3");
        builder.UseSetting("YouTube:ApiKey", "test_api_key");
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        WireMockServer = WireMockServer.Start();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        WireMockServer.Stop();
        await DisposeAsync();
    }
}