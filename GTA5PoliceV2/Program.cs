using System;
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
        public static List<ulong> modRoleID = new List<ulong>();
        public static ulong[] modRoleIDs;
        public static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();
        private DiscordSocketClient client;
        private CommandHandler handler;

        public async Task Start()
        {
            EnsureBotConfigExists(); // Ensure that the bot configuration json file has been created.

            client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Verbose,
            });

            client.Log += Logger;
            await client.LoginAsync(TokenType.Bot, BotConfig.Load().Token);
            await client.StartAsync();

            var serviceProvider = ConfigureServices();
            handler = new CommandHandler(serviceProvider);
            await handler.ConfigureAsync();

            //Block this program untill it is closed
            await Task.Delay(-1);
        }
        private static Task Logger(LogMessage lmsg)
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
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
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
            if (!Directory.Exists(Path.Combine(AppContext.BaseDirectory, "configuration")))
                Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "configuration"));

            string configLoc = Path.Combine(AppContext.BaseDirectory, "configuration/config.json");
            string connectionsLoc = Path.Combine(AppContext.BaseDirectory, "configuration/connections_config.json");
            string urlsLoc = Path.Combine(AppContext.BaseDirectory, "configuration/url_config.json");

            if (!File.Exists(configLoc))
            {
                var config = new BotConfig();

                Console.WriteLine("Please enter the following information to save into your configuration/config.json file");
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
                config.Commanders = 1;
                config.BotCommanders[0] = 211938243535568896;
                config.Filters = 4;
                config.FilteredWords[0] = "Nigga";
                config.FilteredWords[1] = "Nigger";
                config.FilteredWords[2] = "Nibba";
                config.FilteredWords[3] = "Chink";
                config.Save();
            }
            Console.WriteLine("Configuration has been loaded");

            if (!File.Exists(connectionsLoc))
            {
                var connections = new ConnectionsConfig();

                connections.ServerIp = "66.150.121.131";
                connections.NyPort = 30150;
                connections.LaPort = 30141;
                connections.NyWlPort = 30151;
                connections.LaWlPort = 30142;
                connections.Save();
            }
            Console.WriteLine("Connections configuration has been loaded");

            if (!File.Exists(urlsLoc))
            {
                var url = new UrlConfig();
                url.Website = "https://www.gta5police.com";
                url.Dashboard = "https://gta5police.com/panel/index.php";
                url.Forums = "https://gta5police.com/forums/";
                url.Support = "http://gta5police.com/forums/index.php?/support/";
                url.Donate = "http://gta5police.com/forums/index.php?/donate/";

                url.Whitelist = "https://goo.gl/TLSGdf";
                url.Police = "https://goo.gl/RYNDBA";
                url.EMS = "https://goo.gl/vNzGvr";
                url.Mechanic = "https://goo.gl/rChgek";

                url.Logo = "https://cdn.discordapp.com/attachments/336338554424918017/353934612503855115/GTA5Police_Main.png";
                url.Rules = "http://goo.gl/7app1D";
                url.HowWeBan = "https://puu.sh/yG7Nv.png";

                url.Save();
            }
            Console.WriteLine("URL configuration has been loaded");
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
