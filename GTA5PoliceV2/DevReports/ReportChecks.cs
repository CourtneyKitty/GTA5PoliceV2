using Discord;
using GTA5PoliceV2.Config;

namespace GTA5PoliceV2.DevReports
{
    class ReportChecks
    {

        public static bool IsDev(IUser user)
        {
            for (int i = 0; i <= DevConfig.Load().Devs - 1; i++)
            {
                if (DevConfig.Load().Developers[i] == user.Id)
                {
                    Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Dev Reports", "This isn't a report, it is a dev!"));
                    return true;
                }
            }
            Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Dev Reports", "This isn't a dev, it is a report!"));
            return false;
        }

        public static bool IsCorrectLayout(string message)
        {
            if (message.ToLower().Contains("server of bug:"))
            {
                if (message.ToLower().Contains("screenshot:"))
                {
                    if (message.ToLower().Contains("describe:"))
                    {
                        Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Dev Reports", "This report is the correct layout!"));
                        return true;
                    }
                }
            }
            Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Dev Reports", "This report is not the correct layout!"));
            return false;
        }

    }
}
