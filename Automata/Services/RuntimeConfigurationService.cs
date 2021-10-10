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
        private readonly ClientRepository _clientRepository;
        private readonly ILogger<RuntimeConfigurationService> _logger;
        private readonly RuntimeConfigurationOptions _runtimeConfiguration;

        public RuntimeConfigurationService(DiscordSocketClient client, ClientRepository clientRepository, IConfiguration configuration, ILogger<RuntimeConfigurationService> logger) : base(client, logger)
        {
            _clientRepository = clientRepository;
            _logger = logger;
            _runtimeConfiguration = configuration.GetSection("RuntimeConfiguration")
                .Get<RuntimeConfigurationOptions>();
        }

        private void SetLaunchStatus(string launchStatus)
        {
            _clientRepository.SetStatusAsync(launchStatus);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetLaunchStatus(_runtimeConfiguration?.launchStatus);
            return Task.CompletedTask;
        }
    }
}