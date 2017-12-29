using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace GTA5PoliceV2.Util
{
    class Success
    {
        public async Task sendSuccess(ISocketMessageChannel channel, string title, string desc, Color color)
        {
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = (title);
            embed.Description = (desc);
            await channel.SendMessageAsync("", false, embed);
        }

        public async Task sendSuccess(IMessageChannel channel, string title, string desc, Color color)
        {
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = (title);
            embed.Description = (desc);
            await channel.SendMessageAsync("", false, embed);
        }

        public async Task sendSuccess(ISocketMessageChannel channel, string title, string desc, Color color, string url)
        {
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = (title);
            embed.Description = (desc);
            embed.WithUrl(url);
            await channel.SendMessageAsync("", false, embed);
        }

        public async Task sendSuccess(IMessageChannel channel, string title, string desc, Color color, string url)
        {
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = (title);
            embed.Description = (desc);
            embed.WithUrl(url);
            await channel.SendMessageAsync("", false, embed);
        }

        public async Task sendSuccessTemp(ISocketMessageChannel channel, string title, string desc, Color color)
        {
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = (title);
            embed.Description = (desc);
            var message = await channel.SendMessageAsync("", false, embed);
            await Delete.DelayDeleteEmbed(message, 30);
        }

        public async Task sendSuccessTemp(IMessageChannel channel, string title, string desc, Color color)
        {
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = (title);
            embed.Description = (desc);
            var errorMessage = await channel.SendMessageAsync("", false, embed);
            await Delete.DelayDeleteEmbed(errorMessage, 30);
        }

        public async Task sendSuccessTemp(ISocketMessageChannel channel, string title, string desc, Color color, string url)
        {
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = (title);
            embed.Description = (desc);
            embed.WithUrl(url);
            var message = await channel.SendMessageAsync("", false, embed);
            await Delete.DelayDeleteEmbed(message, 30);
        }

        public async Task sendSuccessTemp(IMessageChannel channel, string title, string desc, Color color, string url)
        {
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = (title);
            embed.Description = (desc);
            embed.WithUrl(url);
            var errorMessage = await channel.SendMessageAsync("", false, embed);
            await Delete.DelayDeleteEmbed(errorMessage, 30);
        }
    }
}
