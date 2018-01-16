using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace GTA5PoliceV2.Util
{
    class Errors
    {
        public async Task sendErrorAsync(ISocketMessageChannel channel, string error, Color color)
        {
            Statistics.AddErrorsDetected();
            Statistics.AddOutgoingMessages();
            await Program.Logger(new LogMessage(LogSeverity.Error, "GTA5Police Errors", error));
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = ("ERROR");
            embed.Description = (error);
            await channel.SendMessageAsync("", false, embed);
        }

        public async Task sendErrorAsync(IMessageChannel channel, string error, Color color)
        {
            Statistics.AddErrorsDetected();
            Statistics.AddOutgoingMessages();
            await Program.Logger(new LogMessage(LogSeverity.Error, "GTA5Police Errors", error));
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = ("ERROR");
            embed.Description = (error);
            await channel.SendMessageAsync("", false, embed);
            Console.WriteLine("Error message was sent to the user.");
        }

        public async Task sendErrorTempAsync(ISocketMessageChannel channel, string error, Color color)
        {
            Statistics.AddErrorsDetected();
            Statistics.AddOutgoingMessages();
            await Program.Logger(new LogMessage(LogSeverity.Error, "GTA5Police Errors", error));
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = ("ERROR");
            embed.Description = (error);
            var errorMessage = await channel.SendMessageAsync("", false, embed);
            await Delete.DelayDeleteEmbedAsync(errorMessage, 30);
        }

        public async Task sendErrorTempAsync(IMessageChannel channel, string error, Color color)
        {
            Statistics.AddErrorsDetected();
            Statistics.AddOutgoingMessages();
            await Program.Logger(new LogMessage(LogSeverity.Error, "GTA5Police Errors", error));
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = ("ERROR");
            embed.Description = (error);
            var errorMessage = await channel.SendMessageAsync("", false, embed);
            await Delete.DelayDeleteEmbedAsync(errorMessage, 30);
        }
    }
}
