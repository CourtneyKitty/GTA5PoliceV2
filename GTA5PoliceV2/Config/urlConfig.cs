using Newtonsoft.Json;
using System;
using System.IO;

namespace GTA5PoliceV2.Config
{
    class UrlConfig
    {
        [JsonIgnore]
        public static readonly string appdir = AppContext.BaseDirectory;

        public string Website { get; set; }
        public string Dashboard { get; set; }
        public string Forums { get; set; }
        public string Support { get; set; }
        public string Donate { get; set; }

        public string Whitelist { get; set; }
        public string Police { get; set; }
        public string EMS { get; set; }
        public string Mechanic { get; set; }

        public string Logo { get; set; }
        public string Rules { get; set; }
        public string HowWeBan { get; set; }

        public UrlConfig()
        {
            Website = "https://www.gta5police.com";
            Dashboard = "https://gta5police.com/panel/index.php";
            Forums = "https://gta5police.com/forums/";
            Support = "http://gta5police.com/forums/index.php?/support/";
            Donate = "http://gta5police.com/forums/index.php?/donate/";

            Whitelist = "https://goo.gl/TLSGdf";
            Police = "https://goo.gl/RYNDBA";
            EMS = "https://goo.gl/vNzGvr";
            Mechanic = "https://goo.gl/rChgek";

            Logo = "https://cdn.discordapp.com/attachments/336338554424918017/353934612503855115/GTA5Police_Main.png";
            Rules = "http://goo.gl/7app1D";
            HowWeBan = "https://puu.sh/yG7Nv.png";
        }

        public void Save(string dir = "configuration/url_config.json")
        {
            string file = Path.Combine(appdir, dir);
            File.WriteAllText(file, ToJson());
        }
        public static UrlConfig Load(string dir = "configuration/url_config.json")
        {
            string file = Path.Combine(appdir, dir);
            return JsonConvert.DeserializeObject<UrlConfig>(File.ReadAllText(file));
        }
        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }  
}
