using System;
using System.IO;
using System.Threading.Tasks;
using Automata.Repositories;
using Automata.Services;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Automata
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                await CreateHostBuilder().Build().RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to build the host");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder()
        {

            BuildLogger();


            var builder = new HostBuilder()
                .ConfigureAppConfiguration(x =>
                {
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", false, true)
                        .Build();

                    x.AddConfiguration(configuration);

                })
                .ConfigureDiscordHost((context, config) =>
                {
                    config.SocketConfig = new DiscordSocketConfig
                    {
                        LogLevel = LogSeverity.Verbose,
                        AlwaysDownloadUsers = true,
                        MessageCacheSize = 200,
                        
                    };
                    config.Token = context.Configuration["botToken"];
                })
                .UseCommandService((context, config) =>
                {
                    config = new CommandServiceConfig()
                    {
                        CaseSensitiveCommands = true,
                        LogLevel = LogSeverity.Verbose
                    };
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<ClientRepository>();
                    services.AddHostedService<RuntimeConfigurationService>();
                })
                .UseSerilog()
                .UseConsoleLifetime();
            
            return builder;
            

        }

        public static void BuildLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
    

}