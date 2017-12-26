using Discord.Commands;
using System.Threading.Tasks;
using GTA5PoliceV2.Config;
using GTA5PoliceV2.Util;
using GTA5PoliceV2.Connection;

namespace GTA5PoliceV2.Modules
{
    public class PublicModule : ModuleBase
    {
        [Command("status")]
        public async Task Status()
        {
            ServerStatus status = new ServerStatus();
            //status.pingServers();

            var channel = Context.Channel;
            var user = Context.User;
            await Context.Message.DeleteAsync();
            await status.displayStatus(channel, user);
        }
    }
}
