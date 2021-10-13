using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Automata.Models;
using Automata.Repositories;
using Discord;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Automata.Services
{
    public class RuntimeConfigurationService : DiscordClientService
    {
        private readonly CommandHandlerService _commandHandlerService;
        private readonly ClientRepository _clientRepository;
        private readonly ILogger<RuntimeConfigurationService> _logger;
        private readonly RuntimeConfigurationOptions _runtimeConfiguration;

        public RuntimeConfigurationService(CommandHandlerService commandHandlerService, DiscordSocketClient client, ClientRepository clientRepository, IConfiguration configuration, ILogger<RuntimeConfigurationService> logger) : base(client, logger)
        {
            _commandHandlerService = commandHandlerService;
            _clientRepository = clientRepository;
            _logger = logger;
            _runtimeConfiguration = configuration.GetSection("RuntimeConfiguration")
                .Get<RuntimeConfigurationOptions>();
        }

        private void SetLaunchStatus(string launchStatus)
        {
            _logger.LogInformation("Launch Status: {Status}", launchStatus);
            _clientRepository.SetStatusAsync(launchStatus);
        }

        private async void PrepareCommandHandler(char? prefix)
        {
           
            if (!prefix.HasValue)
            {
                char defaultPrefix = '!';
                _logger.LogInformation("No launch prefix provided, using default: {Prefix}", defaultPrefix);
                prefix = defaultPrefix;
            }
            else
            {
                _logger.LogInformation("Launch Prefix: {Prefix}", prefix);
            }
            
            await _commandHandlerService.InstallCommandsAsync(prefix.Value);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            PrepareCommandHandler(_runtimeConfiguration?.launchPrefix);
            SetLaunchStatus(_runtimeConfiguration?.launchStatus);
            return Task.CompletedTask;
        }
    }
}