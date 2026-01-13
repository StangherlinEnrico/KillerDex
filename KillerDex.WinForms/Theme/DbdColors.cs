using System.Drawing;

namespace KillerDex.Theme
{
    /// <summary>
    /// Dead by Daylight color palette for consistent theming across the application.
    /// </summary>
    public static class DbdColors
    {
        // Background colors
        public static readonly Color Background = Color.FromArgb(20, 20, 25);
        public static readonly Color HeaderBackground = Color.FromArgb(15, 15, 20);
        public static readonly Color CardBackground = Color.FromArgb(30, 30, 38);
        public static readonly Color CardBackgroundAlt = Color.FromArgb(25, 25, 32);
        public static readonly Color InputBackground = Color.FromArgb(35, 35, 40);

        // Selection and interaction colors
        public static readonly Color CardSelected = Color.FromArgb(140, 20, 20);
        public static readonly Color CardHover = Color.FromArgb(40, 40, 50);

        // Accent colors
        public static readonly Color AccentRed = Color.FromArgb(180, 30, 30);
        public static readonly Color AccentRedDark = Color.FromArgb(140, 20, 20);
        public static readonly Color AccentGreen = Color.FromArgb(40, 160, 80);
        public static readonly Color AccentBlue = Color.FromArgb(60, 140, 200);
        public static readonly Color AccentYellow = Color.FromArgb(200, 160, 40);

        // Text colors
        public static readonly Color TextPrimary = Color.FromArgb(220, 220, 220);
        public static readonly Color TextSecondary = Color.FromArgb(140, 140, 140);
        public static readonly Color TextAccent = Color.FromArgb(140, 50, 50);

        // State colors
        public static readonly Color WinColor = Color.FromArgb(50, 180, 100);
        public static readonly Color LossColor = Color.FromArgb(180, 50, 50);

        // Border colors
        public static readonly Color Border = Color.FromArgb(70, 70, 80);
        public static readonly Color BorderLight = Color.FromArgb(80, 80, 90);
        public static readonly Color BorderHover = Color.FromArgb(100, 100, 110);

        // Button colors
        public static readonly Color ButtonDefault = Color.FromArgb(45, 45, 55);
        public static readonly Color ButtonHover = Color.FromArgb(60, 60, 70);
        public static readonly Color ButtonPressed = Color.FromArgb(35, 35, 45);
        public static readonly Color ButtonSelected = Color.FromArgb(140, 20, 20);
        public static readonly Color ButtonSelectedHover = Color.FromArgb(160, 30, 30);

        // Green variant for buttons (survivors)
        public static readonly Color ButtonSelectedGreen = Color.FromArgb(30, 130, 60);
        public static readonly Color ButtonSelectedGreenHover = Color.FromArgb(40, 150, 70);
        public static readonly Color BorderSelectedGreen = Color.FromArgb(50, 180, 80);
    }
}
