using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Text.Json;

namespace API.Localization;

public class JsonStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<JsonStringLocalizerFactory> _logger;

    public JsonStringLocalizerFactory(IWebHostEnvironment env, ILogger<JsonStringLocalizerFactory> logger)
    {
        _env = env;
        _logger = logger;
    }

    public IStringLocalizer Create(Type resourceSource) => new JsonStringLocalizer(_env, _logger);
    public IStringLocalizer Create(string baseName, string location) => new JsonStringLocalizer(_env, _logger);
}

public class JsonStringLocalizer : IStringLocalizer
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger _logger;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    private static readonly Dictionary<string, Dictionary<string, string>> _cache = new();
    private static readonly object _lock = new();

    public JsonStringLocalizer(IWebHostEnvironment env, ILogger logger)
    {
        _env = env;
        _logger = logger;
    }

    public LocalizedString this[string name] => GetString(name);

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var value = GetString(name);
            return new LocalizedString(name, string.Format(value.Value, arguments), value.ResourceNotFound);
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        var translations = LoadTranslations(culture);
        return translations.Select(kv => new LocalizedString(kv.Key, kv.Value));
    }

    private LocalizedString GetString(string key)
    {
        var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        var translations = LoadTranslations(culture);

        if (translations.TryGetValue(key, out var value))
        {
            return new LocalizedString(key, value);
        }

        // Fallback to English
        if (culture != "en")
        {
            var enTranslations = LoadTranslations("en");
            if (enTranslations.TryGetValue(key, out var enValue))
            {
                return new LocalizedString(key, enValue);
            }
        }

        return new LocalizedString(key, key, resourceNotFound: true);
    }

    private Dictionary<string, string> LoadTranslations(string culture)
    {
        var cacheKey = culture;

        lock (_lock)
        {
            if (_cache.TryGetValue(cacheKey, out var cached))
            {
                return cached;
            }

            var filePath = Path.Combine(_env.ContentRootPath, "Localization", $"{culture}.json");

            if (!File.Exists(filePath))
            {
                _logger.LogWarning("Localization file not found: {FilePath}", filePath);
                filePath = Path.Combine(_env.ContentRootPath, "Localization", "en.json");
            }

            if (!File.Exists(filePath))
            {
                _logger.LogError("Default localization file not found: {FilePath}", filePath);
                return new Dictionary<string, string>();
            }

            try
            {
                var json = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json, _jsonOptions)
                    ?? new Dictionary<string, JsonElement>();

                var flattened = FlattenJson(data);
                _cache[cacheKey] = flattened;
                return flattened;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading localization file: {FilePath}", filePath);
                return new Dictionary<string, string>();
            }
        }
    }

    private static Dictionary<string, string> FlattenJson(Dictionary<string, JsonElement> data, string prefix = "")
    {
        var result = new Dictionary<string, string>();

        foreach (var (key, value) in data)
        {
            var fullKey = string.IsNullOrEmpty(prefix) ? key : $"{prefix}_{key}";

            if (value.ValueKind == JsonValueKind.Object)
            {
                var nested = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(value.GetRawText())
                    ?? new Dictionary<string, JsonElement>();
                foreach (var (nestedKey, nestedValue) in FlattenJson(nested, fullKey))
                {
                    result[nestedKey] = nestedValue;
                }
            }
            else if (value.ValueKind == JsonValueKind.String)
            {
                result[fullKey] = value.GetString() ?? string.Empty;
            }
        }

        return result;
    }
}

public class JsonStringLocalizer<T> : IStringLocalizer<T>
{
    private readonly IStringLocalizer _localizer;

    public JsonStringLocalizer(IStringLocalizerFactory factory)
    {
        _localizer = factory.Create(typeof(T));
    }

    public LocalizedString this[string name] => _localizer[name];
    public LocalizedString this[string name, params object[] arguments] => _localizer[name, arguments];
    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => _localizer.GetAllStrings(includeParentCultures);
}
