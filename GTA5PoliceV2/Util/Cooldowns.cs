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
        private static TimeSpan statusLast, rulesLast, linksLast, applyLast, clearcacheLast, uptimeLast;

        public static double GetCommandCooldown() { return BotConfig.Load().CommandCooldown; }

        public static async Task ResetCommandCooldownAsync()
        {
            statusLast = DateTime.Now.TimeOfDay.Subtract(new TimeSpan(0, 0, (int)BotConfig.Load().CommandCooldown));
            rulesLast = DateTime.Now.TimeOfDay.Subtract(new TimeSpan(0, 0, (int)BotConfig.Load().CommandCooldown));
            linksLast = DateTime.Now.TimeOfDay.Subtract(new TimeSpan(0, 0, (int)BotConfig.Load().CommandCooldown));
            applyLast = DateTime.Now.TimeOfDay.Subtract(new TimeSpan(0, 0, (int)BotConfig.Load().CommandCooldown));
            clearcacheLast = DateTime.Now.TimeOfDay.Subtract(new TimeSpan(0, 0, (int)BotConfig.Load().CommandCooldown));
            uptimeLast = DateTime.Now.TimeOfDay.Subtract(new TimeSpan(0, 0, (int)BotConfig.Load().CommandCooldown));
            await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Cooldowns", "Command cooldowns reset."));
        }

        public static async Task AddCooldownMessageAsync(SocketMessage pMsg)
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

        public static void SetStatusLast(TimeSpan last) { statusLast = last; }
        public static TimeSpan GetStatusLast() { return statusLast; }
        public static void SetRulesLast(TimeSpan last) { rulesLast = last; }
        public static TimeSpan GetRulesLast() { return rulesLast; }
        public static void SetLinksLast(TimeSpan last) { linksLast = last; }
        public static TimeSpan GetLinksLast() { return linksLast; }
        public static void SetApplyLast(TimeSpan last) { applyLast = last; }
        public static TimeSpan GetApplyLast() { return applyLast; }
        public static void SetClearcacheLast(TimeSpan last) { clearcacheLast = last; }
        public static TimeSpan GetClearcacheLast() { return clearcacheLast; }
        public static void SetUptimeLast(TimeSpan last) { uptimeLast = last; }
        public static TimeSpan GetUptimeLast() { return uptimeLast; }
    }

}
