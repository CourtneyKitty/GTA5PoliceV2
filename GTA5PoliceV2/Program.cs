﻿using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using GTA5PoliceV2.Config;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using GTA5PoliceV2.Connection;
using GTA5PoliceV2.Util;

namespace GTA5PoliceV2
{
    class Program
    {
        public static void Main(string[] args) => new Program().StartAsync().GetAwaiter().GetResult();
        private DiscordSocketClient client;
        private CommandHandler handler;

        public async Task StartAsync()
        {
            EnsureBotConfigExists(); // Ensure that the bot configuration json file has been created.

            client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Verbose,
            });

            client.Log += Logger;
            await Logger(new LogMessage(LogSeverity.Info, "GTA5Police Start Up", "Launching bot."));
            await Logger(new LogMessage(LogSeverity.Info, "GTA5Police Start Up", "Initialization started."));

            await Logger(new LogMessage(LogSeverity.Info, "GTA5Police Start Up", "Log in started."));
            await client.LoginAsync(TokenType.Bot, BotConfig.Load().Token);
            await Logger(new LogMessage(LogSeverity.Info, "GTA5Police Start Up", "Log in complete."));

            await client.StartAsync();
            await Logger(new LogMessage(LogSeverity.Info, "GTA5Police Start Up", "Bot successfully launched."));

            await Logger(new LogMessage(LogSeverity.Info, "GTA5Police Start Up", "Connecting to service provider."));
            var serviceProvider = ConfigureServices();
            await Logger(new LogMessage(LogSeverity.Info, "GTA5Police Start Up", "Command handler initialization started."));
            handler = new CommandHandler(serviceProvider);
            await Logger(new LogMessage(LogSeverity.Info, "GTA5Police Start Up", "Command handler initialization complete."));
            await Logger(new LogMessage(LogSeverity.Info, "GTA5Police Start Up", "Config initialization started."));
            await handler.ConfigureAsync();
            await Logger(new LogMessage(LogSeverity.Info, "GTA5Police Start Up", "Config initialization complete."));
            await Logger(new LogMessage(LogSeverity.Info, "GTA5Police Start Up", "Initialization complete."));
            await Logger(new LogMessage(LogSeverity.Info, "GTA5Police Start Up", "Bot ready for use."));

            //Block this program until it is closed
            await Task.Delay(-1);
        }
        public static Task Logger(LogMessage lmsg)
        {
            var cc = Console.ForegroundColor;
            switch (lmsg.Severity)
            {
                case LogSeverity.Critical:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogSeverity.Verbose:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }
            Console.WriteLine($"{DateTime.Now} [{lmsg.Severity,8}] {lmsg.Source}: {lmsg.Message}");
            Console.ForegroundColor = cc;
            return Task.CompletedTask;
        }

        public static void EnsureBotConfigExists()
        {
            Logger(new LogMessage(LogSeverity.Info, "GTA5Police Start Up", "Searching for existing configurations."));
            if (!Directory.Exists(Path.Combine(AppContext.BaseDirectory, "configuration")))
                Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "configuration"));

            string configLoc = Path.Combine(AppContext.BaseDirectory, "configuration/config.json");
            string connectionsLoc = Path.Combine(AppContext.BaseDirectory, "configuration/connections_config.json");
            string urlsLoc = Path.Combine(AppContext.BaseDirectory, "configuration/url_config.json");
            string devConfigLoc = Path.Combine(AppContext.BaseDirectory, "configuration/dev_config.json");
            string ranksConfigLoc = Path.Combine(AppContext.BaseDirectory, "configuration/ranks_config.json");
            string autoBansLoc = Path.Combine(AppContext.BaseDirectory, "configuration/auto_bans.json");

            if (!File.Exists(configLoc))
            {
                var config = new BotConfig();

                Logger(new LogMessage(LogSeverity.Debug, "GTA5Police Configuration", "Enter the following info for the configuration."));
                Console.Write("Bot Prefix: ");
                config.Token = Console.ReadLine();
                Console.Write("Bot Token: ");
                config.Token = Console.ReadLine();
                config.ServerId = 0;
                config.LogsId = 0;
                config.TimerChannelId = 0;
                config.StatusTimerInterval = 1;
                config.MessageTimerInterval = 30;
                config.MessageTimerCooldown = 5;
                config.CommandCooldown = 120.0d;
                config.Commanders = 1;
                config.BotCommanders[0] = 211938243535568896;
                config.Filters = 0;
                config.PoliceAdd = false;
                config.EmsAdd = false;
                config.Save();
                Logger(new LogMessage(LogSeverity.Debug, "GTA5Police Configuration", "Main configuration generated"));
            }
            Logger(new LogMessage(LogSeverity.Debug, "GTA5Police Configuration", "Main configuration has been loaded"));

            if (!File.Exists(connectionsLoc))
            {
                var connections = new ConnectionsConfig();

                connections.ServerIp = "66.150.121.131";
                connections.NyPort = 30150;
                connections.LaPort = 30141;
                connections.NyWlPort = 30151;
                connections.LaWlPort = 30142;
                connections.Save();
                Logger(new LogMessage(LogSeverity.Debug, "GTA5Police Configuration", "Connections configuration generated"));
            }
            Logger(new LogMessage(LogSeverity.Debug, "GTA5Police Configuration", "Connections configuration has been loaded"));

            if (!File.Exists(urlsLoc))
            {
                var url = new UrlConfig();
                url.Website = "https://www.gta5police.com";
                url.Dashboard = "https://gta5police.com/panel/index.php";
                url.Forums = "https://gta5police.com/forums/";
                url.Support = "http://gta5police.com/forums/index.php?/support/";
                url.Suggestions = "https://gta5police.com/forums/index.php?/forum/5-suggestions/";
                url.Donate = "http://gta5police.com/forums/index.php?/donate/";
                url.Vacbanned = "http://www.vacbanned.com";

                url.Applications = "https://goo.gl/DpTEyH";
                url.Whitelist = "https://goo.gl/TLSGdf";
                url.Police = "https://goo.gl/RYNDBA";
                url.EMS = "https://goo.gl/vNzGvr";
                url.Mechanic = "https://goo.gl/rChgek";
                url.Taxi = "https://goo.gl/DbThWg";
                url.Stream = "https://goo.gl/EPZpNR";

                url.Logo = "https://cdn.discordapp.com/attachments/336338554424918017/353934612503855115/GTA5Police_Main.png";
                url.Rules = "http://goo.gl/7app1D";
                url.HowWeBan = "https://puu.sh/yG7Nv.png";
                url.ClearCache = "https://gta5police.com/forums/index.php?/topic/921-how-to-clear-fivem-cache/";

                url.Save();
                Logger(new LogMessage(LogSeverity.Debug, "GTA5Police Configuration", "URL configuration generated"));
            }
            Logger(new LogMessage(LogSeverity.Debug, "GTA5Police Configuration", "URL configuration has been loaded"));

            if (!File.Exists(devConfigLoc))
            {
                var config = new DevConfig();

                config.DevReports = 394177874657148940;
                config.Devs = 0;
                config.Suggestions = 366955141771034625;
                config.Save();
                Logger(new LogMessage(LogSeverity.Debug, "GTA5Police Configuration", "Dev config generated"));
            }
            Logger(new LogMessage(LogSeverity.Debug, "GTA5Police Configuration", "Developer configuration has been loaded"));

            if (!File.Exists(ranksConfigLoc))
            {
                var config = new RanksConfig();
                config.EMSHighRanks = 4;
                config.PDHighRanks = 5;
                config.Save();
                Logger(new LogMessage(LogSeverity.Debug, "GTA5Police Configuration", "Ranks configuration generated"));
            }
            Logger(new LogMessage(LogSeverity.Debug, "GTA5Police Configuration", "Ranks configuration has been loaded"));

            if (!File.Exists(autoBansLoc))
            {
                var config = new AutoBans();
                
                config.Bans = 0;
                config.Save();
                Logger(new LogMessage(LogSeverity.Debug, "GTA5Police Configuration", "Auto bans generated"));
            }
            Logger(new LogMessage(LogSeverity.Debug, "GTA5Police Configuration", "Auto bans has been loaded"));
        }
        public IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection()
                .AddSingleton(client)
                 .AddSingleton(new CommandService(new CommandServiceConfig { CaseSensitiveCommands = false }));
            var provider = new DefaultServiceProviderFactory().CreateServiceProvider(services);

            return provider;
        }
    }
}
