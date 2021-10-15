using Discord;
using Discord.Commands;

namespace Automata.Modules.BaseModules
{
    public abstract class AttachmentModule : ModuleBase<SocketCommandContext>
    {
        public virtual void BeforeUpload()
        {
            // Mark Uploading
        }

        public virtual void AfterUpload()
        {
            // Mark Uploaded
        }

        public void Upload()
        {
            // Upload process should always be the same, not overridable
        }
    }
}