using Discord;
using GTA5PoliceV2.Config;

namespace GTA5PoliceV2.Suggestions
{
    class SuggestionCheck
    {

        public static bool IsDev(IUser user)
        {
            for (int i = 0; i <= DevConfig.Load().Devs - 1; i++)
            {
                if (DevConfig.Load().Developers[i] == user.Id)
                {
                    Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Suggestions", "This isn't a suggestion, it is a dev!"));
                    return true;
                }
            }
            Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Suggestions", "This isn't a dev, it is a suggestion!"));
            return false;
        }

        public static bool IsCorrectLayout(string message)
        {
            if (message.ToLower().Contains("Suggestion Title:"))
            {
                if (message.ToLower().Contains("Suggestion Topic"))
                {
                    if (message.ToLower().Contains("Suggestion Description"))
                    {
                        Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Suggestions", "This suggestion is the correct layout!"));
                        return true;
                    }
                }
            }
            Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Suggestions", "This suggestion is not the correct layout!"));
            return false;
        }

    }
}
