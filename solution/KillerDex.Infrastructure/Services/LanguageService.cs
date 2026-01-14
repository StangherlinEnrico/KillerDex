using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using KillerDex.Infrastructure.Resources;

namespace KillerDex.Infrastructure.Services
{
    /// <summary>
    /// Settings class for JSON serialization
    /// </summary>
    public class LanguageSettings
    {
        public string Language { get; set; }
    }

    /// <summary>
    /// Service for managing application language/culture settings
    /// </summary>
    public static class LanguageService
    {
        private static readonly string _settingsPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "settings.json");

        /// <summary>
        /// Current language code (e.g., "en", "it")
        /// </summary>
        public static string CurrentLanguage { get; private set; } = "en";

        /// <summary>
        /// Initializes the language service by loading saved settings
        /// </summary>
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
                    var settings = JsonConvert.DeserializeObject<LanguageSettings>(json);
                    if (settings != null && !string.IsNullOrEmpty(settings.Language))
                    {
                        CurrentLanguage = settings.Language;
                    }
                }
                catch
                {
                    // If loading fails, use default language
                }
            }
        }

        private static void SaveSettings()
        {
            try
            {
                var settings = new LanguageSettings { Language = CurrentLanguage };
                string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(_settingsPath, json);
            }
            catch
            {
                // If saving fails, continue silently
            }
        }

        /// <summary>
        /// Sets the application language and saves the preference
        /// </summary>
        /// <param name="language">Language code (e.g., "en", "it")</param>
        public static void SetLanguage(string language)
        {
            CurrentLanguage = language;
            ApplyLanguage(language);
            SaveSettings();
        }

        private static void ApplyLanguage(string language)
        {
            CultureInfo culture;
            switch (language.ToLowerInvariant())
            {
                case "it":
                    culture = new CultureInfo("it-IT");
                    break;
                case "en":
                default:
                    culture = new CultureInfo("en-US");
                    break;
            }

            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
        }

        /// <summary>
        /// Returns true if current language is Italian
        /// </summary>
        public static bool IsItalian => CurrentLanguage == "it";

        /// <summary>
        /// Returns true if current language is English
        /// </summary>
        public static bool IsEnglish => CurrentLanguage == "en";

        /// <summary>
        /// Gets the localized message for language change notification
        /// </summary>
        public static string GetRestartRequiredMessage()
        {
            return InfrastructureMessages.Language_RestartRequired;
        }

        /// <summary>
        /// Gets the localized title for language dialog
        /// </summary>
        public static string GetLanguageDialogTitle()
        {
            return InfrastructureMessages.Language_Title;
        }
    }
}