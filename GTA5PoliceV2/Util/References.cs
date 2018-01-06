using GTA5PoliceV2.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace GTA5PoliceV2.Util
{
    class References
    {
        public static string GetGta5policeLogo() { return UrlConfig.Load().Logo; }
        public static string GetRulesURL() { return UrlConfig.Load().Rules; }
        public static string GetHowBanURL() { return UrlConfig.Load().HowWeBan; }
        public static string GetClearcacheURL() { return UrlConfig.Load().ClearCache; }

        public static string GetWebsiteURL() { return UrlConfig.Load().Website; }
        public static string GetDashboardURL() { return UrlConfig.Load().Dashboard; }
        public static string GetForumsURL() { return UrlConfig.Load().Forums; }
        public static string GetSupportURL() { return UrlConfig.Load().Support; }
        public static string GetSuggestionsURL() { return UrlConfig.Load().Suggestions; }
        public static string GetDonateURL() { return UrlConfig.Load().Donate; }
        public static string GetVacbannedURL() { return UrlConfig.Load().Vacbanned; }

        public static string GetApplicationsURL() { return UrlConfig.Load().Applications; }
        public static string GetWhitelistURL() { return UrlConfig.Load().Whitelist; }
        public static string GetPoliceURL() { return UrlConfig.Load().Police; }
        public static string GetEmsURL() {  return UrlConfig.Load().EMS; }
        public static string GetMechanicURL() { return UrlConfig.Load().Mechanic; }
        public static string GetTaxiURL() { return UrlConfig.Load().Taxi; }
        public static string GetStreamURL() { return UrlConfig.Load().Stream; }

        public static bool IsStartUp = true;
    }
}
