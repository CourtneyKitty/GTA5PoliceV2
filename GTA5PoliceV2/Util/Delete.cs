using Discord;
using System.Threading.Tasks;

namespace GTA5PoliceV2.Util
{
    class Delete
    {
        public static async Task DelayDeleteMessage(IUserMessage message, int time = 0)
        {
            var delete = Task.Run(async () =>
            {
                if (time == 0) await Task.Delay(2500);
                else await Task.Delay(time * 1000);

                await message.DeleteAsync();
            });
        }

        public static async Task DelayDeleteEmbed(IMessage message, int time = 0)
        {
            var delete = Task.Run(async () =>
            {
                if (time == 0) await Task.Delay(2500);
                else await Task.Delay(time * 1000);

                await message.DeleteAsync();
            });
        }
    }
}
