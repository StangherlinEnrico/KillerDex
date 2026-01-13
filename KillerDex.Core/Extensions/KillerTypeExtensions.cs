using KillerDex.Core.Enums;
using KillerDex.Core.Resources;

namespace KillerDex.Core.Extensions
{
    public static class KillerTypeExtensions
    {
        public static string GetDisplayName(this KillerType killer)
        {
            return KillerNames.ResourceManager.GetString(killer.ToString())
                ?? killer.ToString();
        }
    }
}
