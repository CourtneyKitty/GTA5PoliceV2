using System.Threading.Tasks;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using Discord;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using GTA5PoliceV2.Config;
using GTA5PoliceV2.Util;
using GTA5PoliceV2.Connection;

namespace GTA5PoliceV2
{
    class CommandHandler
    {
        Errors errors = new Errors();
        private CommandService commands;
        private DiscordSocketClient bot;
        private IServiceProvider map;

        public CommandHandler(IServiceProvider provider)
        {
            map = provider;
            bot = map.GetService<DiscordSocketClient>();
            bot.UserJoined += AnnounceUserJoined;
            bot.UserLeft += AnnounceLeftUser;
            bot.Ready += SetGame;
            bot.MessageReceived += HandleCommand;
            bot.MessageReceived += ProfanityCheck;
            commands = map.GetService<CommandService>();
        }

        public async Task AnnounceLeftUser(SocketGuildUser user) {}

        public async Task AnnounceUserJoined(SocketGuildUser user) {}


        public async Task SetGame()
        {
            await bot.SetGameAsync(BotConfig.Load().Prefix + "help");
        }

        public async Task ConfigureAsync()
        {
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        public async Task HandleCommand(SocketMessage pMsg)
        {
            //Don't handle the command if it is a system message
            var message = pMsg as SocketUserMessage;
            if (message == null)
                return;
            var context = new SocketCommandContext(bot, message);

            //Mark where the prefix ends and the command begins
            int argPos = 0;
            //Determine if the message has a valid prefix, adjust argPos
            if (message.HasStringPrefix(BotConfig.Load().Prefix, ref argPos))
            {
                if (message.Author.IsBot)
                    return;
                //Execute the command, store the result
                var result = await commands.ExecuteAsync(context, argPos, map);

                //If the command failed, notify the user
                if (!result.IsSuccess && result.ErrorReason != "Unknown command.")
                    await errors.sendErrorTemp(pMsg.Channel, result.ErrorReason, Colours.errorCol);
            }
        }

        public async Task ProfanityCheck(SocketMessage pMsg)
        {
            var message = pMsg as SocketUserMessage;
            if (message == null)
                return;

            if (message.ToString().ToLower().Contains("testword"))
            {
                var user = pMsg.Author.Username;
                var userId = pMsg.Author.Id;
                var channel = pMsg.Channel.ToString();
                var fullMsg = pMsg.ToString();
                var logsChannel = BotConfig.Load().LogsId; //find the channel

                await message.DeleteAsync(); //delete message

                var warningEmbed = new EmbedBuilder() { Color = Colours.errorCol };
                warningEmbed.Title = "";
                warningEmbed.Description = user + " | Do not use that profanity, your message has been deleted.";
                var msg = await pMsg.Channel.SendMessageAsync("", false, warningEmbed);
                //Delete msg

                var logEmbed = new EmbedBuilder() { Color = Colours.errorCol };
                logEmbed.Title = "Discord Profanity Detected";
                logEmbed.Description = "Message was deleted from the channel.";
                var userField = new EmbedFieldBuilder() { Name = "User", Value = user };
                var userIdField = new EmbedFieldBuilder() { Name = "User ID", Value = userId };
                var channelField = new EmbedFieldBuilder() { Name = "Channel", Value = channel };
                var wordField = new EmbedFieldBuilder() { Name = "Word", Value = "TestWord" };
                var messageField = new EmbedFieldBuilder() { Name = "Full Message", Value = fullMsg };
                logEmbed.AddField(userField);
                logEmbed.AddField(userIdField);
                logEmbed.AddField(channelField);
                logEmbed.AddField(wordField);
                logEmbed.AddField(messageField);
                msg = await pMsg.Channel.SendMessageAsync("", false, logEmbed);
            }
        }
    }
}
