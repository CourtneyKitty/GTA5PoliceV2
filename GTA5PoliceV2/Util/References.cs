using GTA5PoliceV2.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace GTA5PoliceV2.Util
{
    class References
    {
        public static string gta5policeLogo() { return UrlConfig.Load().Logo; }
        public static string rulesURL() { return UrlConfig.Load().Rules; }
        public static string howBanURL() { return UrlConfig.Load().HowWeBan; }
        public static string clearcacheURL() { return UrlConfig.Load().ClearCache; }

        public static string websiteURL() { return UrlConfig.Load().Website; }
        public static string dashboardURL() { return UrlConfig.Load().Dashboard; }
        public static string forumsURL() { return UrlConfig.Load().Forums; }
        public static string supportURL() { return UrlConfig.Load().Support; }
        public static string donateURL() { return UrlConfig.Load().Donate; }
        public static string vacbannedURL() { return UrlConfig.Load().Vacbanned; }

        public static string applicationsURL() { return UrlConfig.Load().Applications; }
        public static string whitelistURL() { return UrlConfig.Load().Whitelist; }
        public static string policeURL() { return UrlConfig.Load().Police; }
        public static string emsURL() {  return UrlConfig.Load().EMS; }
        public static string mechanicURL() { return UrlConfig.Load().Mechanic; }
        public static string taxiURL() { return UrlConfig.Load().Taxi; }

        public static bool isStartUp = true;
    }
}
