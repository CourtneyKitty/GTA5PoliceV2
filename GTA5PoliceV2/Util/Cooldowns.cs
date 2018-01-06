using Discord;
using Discord.WebSocket;
using GTA5PoliceV2.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GTA5PoliceV2.Util
{
    class Cooldowns
    {
        private static TimeSpan startupTime;
        private static TimeSpan CommandCooldown;

        private static int MessageTimerCooldown = 0;

        private static double GetCommandCooldown() { return BotConfig.Load().CommandCooldown; }
        private static TimeSpan statusLast, rulesLast, linksLast, applyLast, clearcacheLast, uptimeLast;

        public static async Task ResetCommandCooldownAsync()
        {
            statusLast = DateTime.Now.TimeOfDay.Subtract(new TimeSpan(0, 0, (int)BotConfig.Load().CommandCooldown));
            rulesLast = DateTime.Now.TimeOfDay.Subtract(new TimeSpan(0, 0, (int)BotConfig.Load().CommandCooldown));
            linksLast = DateTime.Now.TimeOfDay.Subtract(new TimeSpan(0, 0, (int)BotConfig.Load().CommandCooldown));
            applyLast = DateTime.Now.TimeOfDay.Subtract(new TimeSpan(0, 0, (int)BotConfig.Load().CommandCooldown));
            clearcacheLast = DateTime.Now.TimeOfDay.Subtract(new TimeSpan(0, 0, (int)BotConfig.Load().CommandCooldown));
            uptimeLast = DateTime.Now.TimeOfDay.Subtract(new TimeSpan(0, 0, (int)BotConfig.Load().CommandCooldown));
            await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police", "Command cooldowns reset."));
        }

        public async Task AddCooldownMessageAsync(SocketMessage pMsg)
        {
            var message = pMsg as SocketUserMessage;
            if (message == null)
                return;

            if (pMsg.Channel.Id.Equals(BotConfig.Load().TimerChannelId)) AddMessageToCooldown();
        }

        public static void SetStartupTime(TimeSpan time) { startupTime = time; }
        public static TimeSpan GetStartupTime() { return startupTime; }

        public static void AddMessageToCooldown() { MessageTimerCooldown++; }
        public static void ResetMessageTimerCooldown() { MessageTimerCooldown = 0; }
        public static int GetMessageTimerCooldown() { return MessageTimerCooldown; }
    }

}
