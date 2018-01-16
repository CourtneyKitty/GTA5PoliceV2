using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace GTA5PoliceV2.Util
{
    class Success
    {
        public async Task sendSuccessAsync(ISocketMessageChannel channel, string title, string desc, Color color)
        {
            Statistics.AddOutgoingMessages();
            await Program.Logger(new LogMessage(LogSeverity.Verbose, "GTA5Police Success", title + ": " + desc));
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = (title);
            embed.Description = (desc);
            await channel.SendMessageAsync("", false, embed);
        }

        public async Task sendSuccessAsync(IMessageChannel channel, string title, string desc, Color color)
        {
            Statistics.AddOutgoingMessages();
            await Program.Logger(new LogMessage(LogSeverity.Verbose, "GTA5Police Success", title + ": " + desc));
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = (title);
            embed.Description = (desc);
            await channel.SendMessageAsync("", false, embed);
        }

        public async Task sendSuccessAsync(ISocketMessageChannel channel, string title, string desc, Color color, string url)
        {
            Statistics.AddOutgoingMessages();
            await Program.Logger(new LogMessage(LogSeverity.Verbose, "GTA5Police Success", title + ": " + desc));
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = (title);
            embed.Description = (desc);
            embed.WithUrl(url);
            await channel.SendMessageAsync("", false, embed);
        }

        public async Task sendSuccessAsync(IMessageChannel channel, string title, string desc, Color color, string url)
        {
            Statistics.AddOutgoingMessages();
            await Program.Logger(new LogMessage(LogSeverity.Verbose, "GTA5Police Success", title + ": " + desc));
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = (title);
            embed.Description = (desc);
            embed.WithUrl(url);
            await channel.SendMessageAsync("", false, embed);
        }

        public async Task sendSuccessTempAsync(ISocketMessageChannel channel, string title, string desc, Color color)
        {
            Statistics.AddOutgoingMessages();
            await Program.Logger(new LogMessage(LogSeverity.Verbose, "GTA5Police Success", title + ": " + desc));
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = (title);
            embed.Description = (desc);
            var message = await channel.SendMessageAsync("", false, embed);
            await Delete.DelayDeleteEmbedAsync(message, 30);
        }

        public async Task sendSuccessTempAsync(IMessageChannel channel, string title, string desc, Color color)
        {
            Statistics.AddOutgoingMessages();
            await Program.Logger(new LogMessage(LogSeverity.Verbose, "GTA5Police Success", title + ": " + desc));
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = (title);
            embed.Description = (desc);
            var errorMessage = await channel.SendMessageAsync("", false, embed);
            await Delete.DelayDeleteEmbedAsync(errorMessage, 30);
        }

        public async Task sendSuccessTempAsync(ISocketMessageChannel channel, string title, string desc, Color color, string url)
        {
            Statistics.AddOutgoingMessages();
            await Program.Logger(new LogMessage(LogSeverity.Verbose, "GTA5Police Success", title + ": " + desc));
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = (title);
            embed.Description = (desc);
            embed.WithUrl(url);
            var message = await channel.SendMessageAsync("", false, embed);
            await Delete.DelayDeleteEmbedAsync(message, 30);
        }

        public async Task sendSuccessTempAsync(IMessageChannel channel, string title, string desc, Color color, string url)
        {
            Statistics.AddOutgoingMessages();
            await Program.Logger(new LogMessage(LogSeverity.Verbose, "GTA5Police Success", title + ": " + desc));
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = (title);
            embed.Description = (desc);
            embed.WithUrl(url);
            var errorMessage = await channel.SendMessageAsync("", false, embed);
            await Delete.DelayDeleteEmbedAsync(errorMessage, 30);
        }

        public async Task sendSuccessTempAsync(IMessageChannel channel, string title, string desc, Color color, string url, int time)
        {
            Statistics.AddOutgoingMessages();
            await Program.Logger(new LogMessage(LogSeverity.Verbose, "GTA5Police Success", title + ": " + desc));
            var embed = new EmbedBuilder() { Color = color };
            embed.Title = (title);
            embed.Description = (desc);
            embed.WithUrl(url);
            var errorMessage = await channel.SendMessageAsync("", false, embed);
            await Delete.DelayDeleteEmbedAsync(errorMessage, time);
        }
    }
}
