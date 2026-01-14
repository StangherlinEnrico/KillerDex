using KillerDex.Core.Enums;
using KillerDex.Core.Resources;

namespace KillerDex.Core.Extensions
{
    public static class MapTypeExtensions
    {
        public static string GetDisplayName(this MapType map)
        {
            return MapNames.ResourceManager.GetString(map.ToString())
                ?? map.ToString();
        }
    }
}
