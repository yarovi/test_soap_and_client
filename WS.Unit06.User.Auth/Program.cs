using WS.Unit06.User.Auth;
using Microsoft.Extensions.Configuration;
internal class Program
{
    private static void Main(string[] args)
    {
        var host = new WebHostBuilder()
            .UseKestrel(x => x.AllowSynchronousIO = true)
            .UseUrls("http://*:9093")
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true);
            })
            .UseStartup<Startup>()
            .ConfigureLogging(x =>
            {
                x.AddDebug();
                x.AddConsole();
            })
            .Build();
        host.Run();
    }
}