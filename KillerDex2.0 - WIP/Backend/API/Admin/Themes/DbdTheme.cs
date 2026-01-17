using MudBlazor;

namespace API.Admin.Themes;

/// <summary>
/// Dead by Daylight inspired theme for KillerDex Admin
/// </summary>
public static class DbdTheme
{
    // ═══════════════════════════════════════════════════════════════════════════
    // DBD COLOR PALETTE
    // ═══════════════════════════════════════════════════════════════════════════

    /// <summary>Blood red - Primary DBD color</summary>
    public const string BloodRed = "#c9252a";

    /// <summary>Deep blood red - Darker variant</summary>
    public const string BloodRedDark = "#9e1e22";

    /// <summary>Light blood red - Lighter variant for hover states</summary>
    public const string BloodRedLight = "#e53935";

    /// <summary>Ember orange - Secondary accent color</summary>
    public const string EmberOrange = "#ff6b00";

    /// <summary>Amber gold - For legendary/special items</summary>
    public const string AmberGold = "#ffb300";

    /// <summary>Entity purple - For ultra rare/special elements</summary>
    public const string EntityPurple = "#7c3aed";

    /// <summary>Mist purple - Lighter purple variant</summary>
    public const string MistPurple = "#a855f7";

    /// <summary>Fog green - Success states</summary>
    public const string FogGreen = "#22c55e";

    /// <summary>Hook rust - Warning states</summary>
    public const string HookRust = "#f59e0b";

    // Background colors (darkest to lighter)
    /// <summary>Void black - Deepest background</summary>
    public const string VoidBlack = "#0a0a0a";

    /// <summary>Shadow black - Main background</summary>
    public const string ShadowBlack = "#121212";

    /// <summary>Fog gray - Surface/cards background</summary>
    public const string FogGray = "#1a1a1a";

    /// <summary>Mist gray - Elevated surfaces</summary>
    public const string MistGray = "#242424";

    /// <summary>Ash gray - Drawer/sidebar background</summary>
    public const string AshGray = "#1e1e1e";

    // Text colors
    /// <summary>Bone white - Primary text</summary>
    public const string BoneWhite = "#f5f5f5";

    /// <summary>Pale gray - Secondary text</summary>
    public const string PaleGray = "#a3a3a3";

    /// <summary>Dim gray - Disabled text</summary>
    public const string DimGray = "#666666";

    // Rarity colors (matching DBD item rarities)
    /// <summary>Common rarity - Brown</summary>
    public const string RarityCommon = "#8b7355";

    /// <summary>Uncommon rarity - Yellow</summary>
    public const string RarityUncommon = "#fff176";

    /// <summary>Rare rarity - Green</summary>
    public const string RarityRare = "#81c784";

    /// <summary>Very Rare rarity - Purple</summary>
    public const string RarityVeryRare = "#ce93d8";

    /// <summary>Ultra Rare rarity - Pink/Magenta</summary>
    public const string RarityUltraRare = "#f06292";

    /// <summary>Event/Special rarity - Orange</summary>
    public const string RarityEvent = "#ffb74d";

    // ═══════════════════════════════════════════════════════════════════════════
    // MUDBLAZOR THEME
    // ═══════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// The main MudBlazor theme with DBD styling
    /// </summary>
    public static MudTheme Theme { get; } = new MudTheme
    {
        PaletteLight = new PaletteLight
        {
            Primary = BloodRed,
            PrimaryDarken = BloodRedDark,
            PrimaryLighten = BloodRedLight,
            Secondary = EmberOrange,
            Tertiary = EntityPurple,
            Info = MistPurple,
            Success = FogGreen,
            Warning = HookRust,
            Error = BloodRedLight,
            Dark = VoidBlack,
            AppbarBackground = FogGray,
            AppbarText = BoneWhite,
            Background = ShadowBlack,
            Surface = FogGray,
            DrawerBackground = AshGray,
            DrawerText = BoneWhite,
            TextPrimary = BoneWhite,
            TextSecondary = PaleGray,
            TextDisabled = DimGray,
            ActionDefault = PaleGray,
            ActionDisabled = DimGray,
            Divider = MistGray,
            DividerLight = "#333333",
            TableLines = MistGray,
            TableStriped = "#1f1f1f",
            TableHover = "#2a2a2a",
            LinesDefault = MistGray,
            LinesInputs = PaleGray
        },
        PaletteDark = new PaletteDark
        {
            Primary = BloodRed,
            PrimaryDarken = BloodRedDark,
            PrimaryLighten = BloodRedLight,
            Secondary = EmberOrange,
            Tertiary = EntityPurple,
            Info = MistPurple,
            Success = FogGreen,
            Warning = HookRust,
            Error = BloodRedLight,
            Dark = VoidBlack,
            AppbarBackground = FogGray,
            AppbarText = BoneWhite,
            Background = ShadowBlack,
            Surface = FogGray,
            DrawerBackground = AshGray,
            DrawerText = BoneWhite,
            TextPrimary = BoneWhite,
            TextSecondary = PaleGray,
            TextDisabled = DimGray,
            ActionDefault = PaleGray,
            ActionDisabled = DimGray,
            Divider = MistGray,
            DividerLight = "#333333",
            TableLines = MistGray,
            TableStriped = "#1f1f1f",
            TableHover = "#2a2a2a",
            LinesDefault = MistGray,
            LinesInputs = PaleGray
        },
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "8px",
            DrawerWidthLeft = "260px"
        }
    };

    // ═══════════════════════════════════════════════════════════════════════════
    // CSS CUSTOM PROPERTIES (for use in Blazor components)
    // ═══════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// CSS custom properties that can be injected into the page
    /// </summary>
    public const string CssVariables = @"
        :root {
            /* Primary colors */
            --dbd-blood-red: #c9252a;
            --dbd-blood-red-dark: #9e1e22;
            --dbd-blood-red-light: #e53935;

            /* Accent colors */
            --dbd-ember-orange: #ff6b00;
            --dbd-amber-gold: #ffb300;
            --dbd-entity-purple: #7c3aed;
            --dbd-mist-purple: #a855f7;
            --dbd-fog-green: #22c55e;
            --dbd-hook-rust: #f59e0b;

            /* Background colors */
            --dbd-void-black: #0a0a0a;
            --dbd-shadow-black: #121212;
            --dbd-fog-gray: #1a1a1a;
            --dbd-mist-gray: #242424;
            --dbd-ash-gray: #1e1e1e;

            /* Text colors */
            --dbd-bone-white: #f5f5f5;
            --dbd-pale-gray: #a3a3a3;
            --dbd-dim-gray: #666666;

            /* Rarity colors */
            --dbd-rarity-common: #8b7355;
            --dbd-rarity-uncommon: #fff176;
            --dbd-rarity-rare: #81c784;
            --dbd-rarity-very-rare: #ce93d8;
            --dbd-rarity-ultra-rare: #f06292;
            --dbd-rarity-event: #ffb74d;

            /* Gradients */
            --dbd-gradient-dark: linear-gradient(135deg, #0a0a0a 0%, #1a1a1a 100%);
            --dbd-gradient-blood: linear-gradient(135deg, #9e1e22 0%, #c9252a 100%);
            --dbd-gradient-entity: linear-gradient(135deg, #5b21b6 0%, #7c3aed 100%);
        }
    ";

    /// <summary>
    /// Gets inline style for background gradient
    /// </summary>
    public static string BackgroundGradient => $"background: linear-gradient(135deg, {VoidBlack} 0%, {FogGray} 100%);";

    /// <summary>
    /// Gets inline style for blood gradient
    /// </summary>
    public static string BloodGradient => $"background: linear-gradient(135deg, {BloodRedDark} 0%, {BloodRed} 100%);";

    /// <summary>
    /// Gets inline style for entity/purple gradient
    /// </summary>
    public static string EntityGradient => $"background: linear-gradient(135deg, #5b21b6 0%, {EntityPurple} 100%);";
}
