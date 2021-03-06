using System.Threading.Tasks;
using Discord.Commands;

namespace Automata.Modules
{
    // Create a module with no prefix
    public class EchoModule : ModuleBase<SocketCommandContext>
    {
        // ~echo hello world -> hello world
        [Command("echo")]
        [Summary("Echoes a message.")]
        public Task SayAsync([Remainder] [Summary("The text to echo")] string echo)
            => ReplyAsync(echo);
		
        // ReplyAsync is a method on ModuleBase 
    }
}