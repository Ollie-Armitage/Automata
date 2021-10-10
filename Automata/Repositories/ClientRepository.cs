using System;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace Automata.Repositories
{
    public class ClientRepository
    {
        private readonly DiscordSocketClient _client;
        private readonly ILogger<ClientRepository> _logger;

        public ClientRepository(DiscordSocketClient client, ILogger<ClientRepository> logger)
        {
            this._client = client;
            this._logger = logger;
        }

        public void SetStatusAsync(string status = null)
        {
            const UserStatus defaultStatus = UserStatus.Online;
            if(string.IsNullOrWhiteSpace(status))
            {
                _logger.LogDebug("SetStatusAsync called with empty string, using default: {Status}", defaultStatus);
                this.SetStatusAsync(defaultStatus);
                return;
            }

            if (Enum.TryParse(status, true, out UserStatus newStatus))
            {
                _logger.LogDebug("Setting bot status to: {Status}", newStatus);
                this.SetStatusAsync(newStatus);
                return;
            }
            
            _logger.LogDebug("Invalid bot status provided, using default: {Default}", defaultStatus);
            this.SetStatusAsync(defaultStatus);
        }

        private void SetStatusAsync(UserStatus status)
        {
            _client.SetStatusAsync(status);
        }
        
    }
}