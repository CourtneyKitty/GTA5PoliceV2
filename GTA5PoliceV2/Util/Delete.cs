using Discord;
using System.Threading.Tasks;

namespace GTA5PoliceV2.Util
{
    class Delete
    {
        public static async Task DelayDeleteMessageAsync(IUserMessage message, int time = 0)
        {
            var delete = Task.Run(async () =>
            {
                await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Antispam", "Delayed delete started for message."));
                if (time == 0) await Task.Delay(2500);
                else await Task.Delay(time * 1000);

                await message.DeleteAsync();
            });
        }

        public static async Task DelayDeleteEmbedAsync(IMessage message, int time = 0)
        {
            var delete = Task.Run(async () =>
            {
                await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Antispam", "Delayed delete started for embed."));
                if (time == 0) await Task.Delay(2500);
                else await Task.Delay(time * 1000);

                await message.DeleteAsync();
            });
        }
    }
}
