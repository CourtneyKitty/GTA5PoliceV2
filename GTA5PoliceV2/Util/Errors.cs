using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace GTA5PoliceV2.Util
{
    class Errors
    {
        public async Task sendError(ISocketMessageChannel channel, string error, Color color)
        {
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = ("ERROR");
            embed.Description = (error);
            await channel.SendMessageAsync("", false, embed);
        }

        public async Task sendError(IMessageChannel channel, string error, Color color)
        {
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = ("ERROR");
            embed.Description = (error);
            await channel.SendMessageAsync("", false, embed);
            Console.WriteLine("Error message was sent to the user.");

        }

        public async Task sendErrorTemp(ISocketMessageChannel channel, string error, Color color)
        {
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = ("ERROR");
            embed.Description = (error);
            var errorMessage = await channel.SendMessageAsync("", false, embed);
            await Delete.DelayDeleteEmbed(errorMessage, 30);
        }

        public async Task sendErrorTemp(IMessageChannel channel, string error, Color color)
        {
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = ("ERROR");
            embed.Description = (error);
            var errorMessage = await channel.SendMessageAsync("", false, embed);
            await Delete.DelayDeleteEmbed(errorMessage, 30);
        }
    }
}
