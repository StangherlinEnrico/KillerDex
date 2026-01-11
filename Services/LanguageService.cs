using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Web.Script.Serialization;

namespace KillerDex.Services
{
    public class LanguageSettings
    {
        public string Language { get; set; }
    }

    public static class LanguageService
    {
        private static readonly string _settingsPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "settings.json");
        private static readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();

        public static string CurrentLanguage { get; private set; } = "en";

        public static void Initialize()
        {
            LoadSettings();
            ApplyLanguage(CurrentLanguage);
        }

        private static void LoadSettings()
        {
            if (File.Exists(_settingsPath))
            {
                try
                {
                    string json = File.ReadAllText(_settingsPath);
                    var settings = _serializer.Deserialize<LanguageSettings>(json);
                    if (settings != null && !string.IsNullOrEmpty(settings.Language))
                    {
                        CurrentLanguage = settings.Language;
                    }
                }
                catch { }
            }
        }

        private static void SaveSettings()
        {
            var settings = new LanguageSettings { Language = CurrentLanguage };
            string json = _serializer.Serialize(settings);
            File.WriteAllText(_settingsPath, json);
        }

        public static void SetLanguage(string language)
        {
            CurrentLanguage = language;
            ApplyLanguage(language);
            SaveSettings();
        }

        private static void ApplyLanguage(string language)
        {
            CultureInfo culture;
            switch (language)
            {
                case "it":
                    culture = new CultureInfo("it-IT");
                    break;
                default:
                    culture = new CultureInfo("en-US");
                    break;
            }

            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
        }

        public static bool IsItalian => CurrentLanguage == "it";
        public static bool IsEnglish => CurrentLanguage == "en";
    }
}