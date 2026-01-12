using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace KillerDex.Controls
{
    /// <summary>
    /// Custom selector button with Dead by Daylight styling.
    /// Used for generator count and survivor count selection.
    /// </summary>
    public class DbdSelectorButton : Control
    {
        #region Private Fields

        private bool _isSelected;
        private bool _isHovered;
        private bool _isPressed;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the numeric value associated with this button.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the icon/emoji to display alongside the value.
        /// </summary>
        public string Icon { get; set; } = "";

        /// <summary>
        /// Gets or sets whether to use green color when selected (for survivors).
        /// If false, uses red color (for generators/default).
        /// </summary>
        public bool UseGreenWhenSelected { get; set; } = false;

        /// <summary>
        /// Gets or sets whether this button is currently selected.
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    Invalidate();
                }
            }
        }

        #endregion

        #region Color Palette - Dead by Daylight Theme

        /// <summary>
        /// Default button background color.
        /// </summary>
        private static readonly Color ColorDefault = Color.FromArgb(45, 45, 55);

        /// <summary>
        /// Button background color when hovered.
        /// </summary>
        private static readonly Color ColorHover = Color.FromArgb(60, 60, 70);

        /// <summary>
        /// Button background color when pressed.
        /// </summary>
        private static readonly Color ColorPressed = Color.FromArgb(35, 35, 45);

        /// <summary>
        /// Selected button background color (red theme - for generators).
        /// </summary>
        private static readonly Color ColorSelectedRed = Color.FromArgb(140, 20, 20);

        /// <summary>
        /// Selected button hover color (red theme).
        /// </summary>
        private static readonly Color ColorSelectedRedHover = Color.FromArgb(160, 30, 30);

        /// <summary>
        /// Selected button background color (green theme - for survivors).
        /// </summary>
        private static readonly Color ColorSelectedGreen = Color.FromArgb(30, 130, 60);

        /// <summary>
        /// Selected button hover color (green theme).
        /// </summary>
        private static readonly Color ColorSelectedGreenHover = Color.FromArgb(40, 150, 70);

        /// <summary>
        /// Default border color.
        /// </summary>
        private static readonly Color ColorBorder = Color.FromArgb(80, 80, 90);

        /// <summary>
        /// Border color when selected (red theme).
        /// </summary>
        private static readonly Color ColorBorderSelectedRed = Color.FromArgb(180, 30, 30);

        /// <summary>
        /// Border color when selected (green theme).
        /// </summary>
        private static readonly Color ColorBorderSelectedGreen = Color.FromArgb(50, 180, 80);

        /// <summary>
        /// Text/foreground color.
        /// </summary>
        private static readonly Color ColorText = Color.FromArgb(220, 220, 220);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the DbdSelectorButton control.
        /// </summary>
        public DbdSelectorButton()
        {
            // Enable double buffering and custom painting
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            // Set default properties
            Cursor = Cursors.Hand;
            Size = new Size(50, 40);
            Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            BackColor = Color.Transparent;
        }

        #endregion

        #region Mouse Event Handlers

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovered = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovered = false;
            _isPressed = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isPressed = true;
                Invalidate();
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _isPressed = false;
            Invalidate();
            base.OnMouseUp(e);
        }

        #endregion

        #region Painting

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

            // Determine colors based on state
            Color backColor = GetBackgroundColor();
            Color borderColor = GetBorderColor();

            // Draw rounded rectangle background
            int cornerRadius = 6;
            using (GraphicsPath path = CreateRoundedRectangle(rect, cornerRadius))
            {
                // Fill background
                using (SolidBrush brush = new SolidBrush(backColor))
                {
                    g.FillPath(brush, path);
                }

                // Draw border
                using (Pen pen = new Pen(borderColor, _isSelected ? 2f : 1f))
                {
                    g.DrawPath(pen, path);
                }
            }

            // Draw content (icon + value)
            DrawContent(g, rect);
        }

        /// <summary>
        /// Gets the appropriate background color based on current state.
        /// </summary>
        private Color GetBackgroundColor()
        {
            if (_isSelected)
            {
                if (_isPressed)
                    return UseGreenWhenSelected ? ColorSelectedGreen : ColorSelectedRed;
                if (_isHovered)
                    return UseGreenWhenSelected ? ColorSelectedGreenHover : ColorSelectedRedHover;
                return UseGreenWhenSelected ? ColorSelectedGreen : ColorSelectedRed;
            }
            else
            {
                if (_isPressed)
                    return ColorPressed;
                if (_isHovered)
                    return ColorHover;
                return ColorDefault;
            }
        }

        /// <summary>
        /// Gets the appropriate border color based on current state.
        /// </summary>
        private Color GetBorderColor()
        {
            if (_isSelected)
            {
                return UseGreenWhenSelected ? ColorBorderSelectedGreen : ColorBorderSelectedRed;
            }
            return _isHovered ? Color.FromArgb(100, 100, 110) : ColorBorder;
        }

        /// <summary>
        /// Draws the button content (icon and value).
        /// </summary>
        private void DrawContent(Graphics g, Rectangle rect)
        {
            string displayText;

            if (!string.IsNullOrEmpty(Icon))
            {
                displayText = $"{Icon} {Value}";
            }
            else
            {
                displayText = Value.ToString();
            }

            // Measure text
            SizeF textSize = g.MeasureString(displayText, Font);

            // Calculate centered position
            float x = rect.X + (rect.Width - textSize.Width) / 2;
            float y = rect.Y + (rect.Height - textSize.Height) / 2;

            // Draw text with slight shadow for depth
            using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(50, 0, 0, 0)))
            {
                g.DrawString(displayText, Font, shadowBrush, x + 1, y + 1);
            }

            using (SolidBrush textBrush = new SolidBrush(ColorText))
            {
                g.DrawString(displayText, Font, textBrush, x, y);
            }
        }

        /// <summary>
        /// Creates a rounded rectangle GraphicsPath.
        /// </summary>
        private GraphicsPath CreateRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            // Top-left arc
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);

            // Top-right arc
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);

            // Bottom-right arc
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);

            // Bottom-left arc
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);

            path.CloseFigure();

            return path;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns a string representation of the control.
        /// </summary>
        public override string ToString()
        {
            return $"DbdSelectorButton: Value={Value}, Selected={IsSelected}";
        }

        #endregion
    }
}