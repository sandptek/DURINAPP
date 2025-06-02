using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class AppConfig
    {
        public static AppConfig.Environment environment = AppConfig.Environment.Test;

        public static string sqlConnStr { 
            get {
                switch (environment)
                {
                    case AppConfig.Environment.Test: return "Data Source=51.81.209.163;Initial Catalog=DURIN;Persist Security Info=True;User ID=8y89022qgqmi9;Password=zm&8Jbuk@Ym8E@a%;Encrypt=False";
                    case AppConfig.Environment.Prep: return "";
					case AppConfig.Environment.Prod: return "Data Source=20.119.99.89;Initial Catalog=DURIN;Persist Security Info=True;User ID=8y89022qgqmi9;Password=zm&8Jbuk@Ym8E@a%;Encrypt=False";
					default: return "";
				}
			}
		}

        //FEDEX//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static bool fedEx_isTest = true;

        public static string fedEx_APIKEY = fedEx_isTest ? "l7c07b1e0f2b6e4f45ab81f1b429d19376" : "";
        public static string fedEx_APISEDCRET = fedEx_isTest ? "ba9860000fc0429980cd8937f7497a3c" : "";
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //APPSETTINGS////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static string appSettingsPath = @"wwwroot\appSettings.json";
        private static Entities.JSON.Settings _settings;
        public static Entities.JSON.Settings settings
        {
            get
            {
                if (_settings == null)
                {
                    if (!File.Exists(appSettingsPath))
                    {
                        File.Create(appSettingsPath);
                    }
                    else
                    {
                        Task<string> data = ReadSettingsAsync();
                        try
                        {
                            _settings = JsonConvert.DeserializeObject<Entities.JSON.Settings>(data.Result);

                            if (_settings == null)
                                _settings = new Entities.JSON.Settings();
                        }
                        catch (Exception ex)
                        {
                            _settings = new Entities.JSON.Settings();
                        }
                    }
                }
                return _settings;
            }
            set
            {
                _settings = value;
                if (!File.Exists(appSettingsPath))
                {
                    File.Create(appSettingsPath);
                }
                WriteSettingsAsync();
            }

        }

        private static async Task WriteSettingsAsync()
        {
            string output = JsonConvert.SerializeObject(_settings, Formatting.Indented);
            await File.WriteAllTextAsync(appSettingsPath, output);
        }

        private static async Task<string> ReadSettingsAsync()
        {
            string data = await File.ReadAllTextAsync(appSettingsPath);
            return data;
        }
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public enum Environment
		{
			Test = 0,
			Prep = 1,
			Prod = 2
		}
	}
}
