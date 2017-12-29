using GTA5PoliceV2.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace GTA5PoliceV2.Util
{
    class References
    {
        public static string gta5policeLogo = UrlConfig.Load().Logo;
        public static string rulesURL = UrlConfig.Load().Rules;
        public static string howBanURL = UrlConfig.Load().HowWeBan;

        public static string websiteURL = UrlConfig.Load().Website;
        public static string dashboardURL = UrlConfig.Load().Dashboard;
        public static string forumsURL = UrlConfig.Load().Forums;
        public static string supportURL = UrlConfig.Load().Support;
        public static string donateURL = UrlConfig.Load().Donate;

        public static string applicationsURL = UrlConfig.Load().Applications;
        public static string whitelistURL = UrlConfig.Load().Whitelist;
        public static string policeURL = UrlConfig.Load().Police;
        public static string emsURL = UrlConfig.Load().EMS;
        public static string mechanicURL = UrlConfig.Load().Mechanic;
        public static string taxiURL = UrlConfig.Load().Taxi;
    }
}
