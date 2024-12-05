using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using DDDNetCore;
using Serilog;
using DotNetEnv;
using Serilog.Sinks.MSSqlServer;
using Serilog.Events;
using Microsoft.Extensions.Configuration;

public class Program
{
    public static void Main(string[] args)
    {
        // Load environment variables from the .env file
        Env.Load("Confidential/.env");
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                // Load configuration to access connection strings
                var configurationRoot = config.Build();

                // Set up Serilog logger
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                    .Enrich.FromLogContext()
                    .WriteTo.MSSqlServer(
                        connectionString: Env.GetString("DATABASE_CONNECTION_STRING"), 
                        sinkOptions: new MSSqlServerSinkOptions
                        {
                            AutoCreateSqlTable = true,
                            TableName = "Logs"
                        })
                    .CreateLogger();
            })
            .UseSerilog() 
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
