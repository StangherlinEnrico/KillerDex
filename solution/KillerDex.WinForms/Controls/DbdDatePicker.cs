using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using KillerDex.Theme;

namespace KillerDex.Controls
{
    /// <summary>
    /// Custom date picker with Dead by Daylight styling.
    /// Features a styled date display and a custom calendar dropdown.
    /// </summary>
    public class DbdDatePicker : UserControl
    {
        #region Private Fields

        private DateTime _value = DateTime.Today;
        private string _customFormat = "dd/MM/yyyy";
        private bool _isHovered;
        private bool _isDropdownOpen;
        private DbdCalendarDropdown _dropdown;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the selected date changes.
        /// </summary>
        public event EventHandler ValueChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the selected date value.
        /// </summary>
        [Category("Data")]
        [Description("The currently selected date.")]
        public DateTime Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    Invalidate();
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the custom date format string.
        /// </summary>
        [Category("Appearance")]
        [Description("The format string for displaying the date.")]
        [DefaultValue("dd/MM/yyyy")]
        public string CustomFormat
        {
            get => _customFormat;
            set
            {
                _customFormat = value;
                Invalidate();
            }
        }

        #endregion

        #region Constructor

        public DbdDatePicker()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.Selectable, true);

            Size = new Size(200, 32);
            Cursor = Cursors.Hand;
            TabStop = true;
        }

        #endregion

        #region Mouse Events

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovered = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovered = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            ToggleDropdown();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
            {
                ToggleDropdown();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape && _isDropdownOpen)
            {
                CloseDropdown();
                e.Handled = true;
            }
        }

        #endregion

        #region Dropdown Management

        private void ToggleDropdown()
        {
            if (_isDropdownOpen)
            {
                CloseDropdown();
            }
            else
            {
                OpenDropdown();
            }
        }

        private void OpenDropdown()
        {
            if (_isDropdownOpen) return;

            _dropdown = new DbdCalendarDropdown(_value);
            _dropdown.DateSelected += Dropdown_DateSelected;
            _dropdown.DropdownClosed += Dropdown_Closed;

            // Position below this control
            Point screenPoint = PointToScreen(new Point(0, Height));
            _dropdown.Location = screenPoint;
            _dropdown.Show();

            _isDropdownOpen = true;
            Invalidate();
        }

        private void CloseDropdown()
        {
            if (_dropdown != null)
            {
                _dropdown.Close();
                _dropdown.Dispose();
                _dropdown = null;
            }
            _isDropdownOpen = false;
            Invalidate();
        }

        private void Dropdown_DateSelected(object sender, DateTime date)
        {
            Value = date;
            CloseDropdown();
        }

        private void Dropdown_Closed(object sender, EventArgs e)
        {
            _isDropdownOpen = false;
            Invalidate();
        }

        #endregion

        #region Painting

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

            // Background
            Color bgColor = _isHovered ? DbdColors.CardHover : DbdColors.InputBackground;
            Color borderColor = _isDropdownOpen ? DbdColors.AccentRed :
                               (_isHovered ? DbdColors.BorderHover : DbdColors.Border);

            using (GraphicsPath path = CreateRoundedRectangle(rect, 4))
            {
                using (SolidBrush brush = new SolidBrush(bgColor))
                {
                    g.FillPath(brush, path);
                }

                using (Pen pen = new Pen(borderColor, _isDropdownOpen ? 2f : 1f))
                {
                    g.DrawPath(pen, path);
                }
            }

            // Date text
            string dateText = _value.ToString(_customFormat);
            using (Font font = new Font("Segoe UI", 10F))
            using (SolidBrush textBrush = new SolidBrush(DbdColors.TextPrimary))
            {
                SizeF textSize = g.MeasureString(dateText, font);
                float textY = (Height - textSize.Height) / 2;
                g.DrawString(dateText, font, textBrush, 12, textY);
            }

            // Calendar icon
            DrawCalendarIcon(g, new Rectangle(Width - 32, 0, 32, Height));

            // Focus indicator
            if (Focused)
            {
                using (Pen focusPen = new Pen(DbdColors.AccentRed, 1f))
                {
                    focusPen.DashStyle = DashStyle.Dot;
                    g.DrawRectangle(focusPen, 2, 2, Width - 5, Height - 5);
                }
            }
        }

        private void DrawCalendarIcon(Graphics g, Rectangle bounds)
        {
            int iconSize = 16;
            int x = bounds.X + (bounds.Width - iconSize) / 2;
            int y = bounds.Y + (bounds.Height - iconSize) / 2;

            Color iconColor = _isHovered ? DbdColors.AccentRed : DbdColors.TextSecondary;

            using (Pen pen = new Pen(iconColor, 1.5f))
            {
                // Calendar body
                Rectangle calRect = new Rectangle(x, y + 3, iconSize, iconSize - 3);
                g.DrawRectangle(pen, calRect);

                // Top bar
                g.DrawLine(pen, x, y + 6, x + iconSize, y + 6);

                // Calendar hooks
                g.DrawLine(pen, x + 4, y, x + 4, y + 4);
                g.DrawLine(pen, x + 12, y, x + 12, y + 4);
            }

            // Day dots
            using (SolidBrush dotBrush = new SolidBrush(iconColor))
            {
                g.FillRectangle(dotBrush, x + 4, y + 9, 2, 2);
                g.FillRectangle(dotBrush, x + 8, y + 9, 2, 2);
                g.FillRectangle(dotBrush, x + 12, y + 9, 2, 2);
                g.FillRectangle(dotBrush, x + 4, y + 13, 2, 2);
                g.FillRectangle(dotBrush, x + 8, y + 13, 2, 2);
            }
        }

        private GraphicsPath CreateRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        #endregion

        #region Cleanup

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CloseDropdown();
            }
            base.Dispose(disposing);
        }

        #endregion
    }

    #region Calendar Dropdown Form

    /// <summary>
    /// Custom calendar dropdown form with Dead by Daylight styling.
    /// </summary>
    internal class DbdCalendarDropdown : Form
    {
        private DateTime _displayMonth;
        private DateTime _selectedDate;
        private Rectangle[] _dayRects;
        private int _hoveredDay = -1;

        private const int CellSize = 32;
        private const int HeaderHeight = 40;
        private const int DayNamesHeight = 25;
        private const int CalendarPadding = 8;

        public event EventHandler<DateTime> DateSelected;
        public event EventHandler DropdownClosed;

        public DbdCalendarDropdown(DateTime initialDate)
        {
            _selectedDate = initialDate;
            _displayMonth = new DateTime(initialDate.Year, initialDate.Month, 1);
            _dayRects = new Rectangle[42]; // 6 weeks x 7 days

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            TopMost = true;
            Size = new Size(CellSize * 7 + CalendarPadding * 2, HeaderHeight + DayNamesHeight + CellSize * 6 + CalendarPadding * 2);
            BackColor = DbdColors.CardBackground;

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            CalculateDayRects();
        }

        private void CalculateDayRects()
        {
            int startX = CalendarPadding;
            int startY = CalendarPadding + HeaderHeight + DayNamesHeight;

            for (int week = 0; week < 6; week++)
            {
                for (int day = 0; day < 7; day++)
                {
                    int index = week * 7 + day;
                    _dayRects[index] = new Rectangle(
                        startX + day * CellSize,
                        startY + week * CellSize,
                        CellSize,
                        CellSize);
                }
            }
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            DropdownClosed?.Invoke(this, EventArgs.Empty);
            Close();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int newHovered = GetDayAtPoint(e.Location);
            if (newHovered != _hoveredDay)
            {
                _hoveredDay = newHovered;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hoveredDay = -1;
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            // Check navigation buttons
            Rectangle prevRect = new Rectangle(CalendarPadding, CalendarPadding, 30, HeaderHeight);
            Rectangle nextRect = new Rectangle(Width - CalendarPadding - 30, CalendarPadding, 30, HeaderHeight);

            if (prevRect.Contains(e.Location))
            {
                _displayMonth = _displayMonth.AddMonths(-1);
                Invalidate();
                return;
            }

            if (nextRect.Contains(e.Location))
            {
                _displayMonth = _displayMonth.AddMonths(1);
                Invalidate();
                return;
            }

            // Check day cells
            int dayIndex = GetDayAtPoint(e.Location);
            if (dayIndex >= 0)
            {
                DateTime firstDayOfMonth = _displayMonth;
                int startDayOfWeek = ((int)firstDayOfMonth.DayOfWeek + 6) % 7; // Monday = 0
                int dayNumber = dayIndex - startDayOfWeek + 1;
                int daysInMonth = DateTime.DaysInMonth(_displayMonth.Year, _displayMonth.Month);

                if (dayNumber >= 1 && dayNumber <= daysInMonth)
                {
                    DateTime selected = new DateTime(_displayMonth.Year, _displayMonth.Month, dayNumber);
                    DateSelected?.Invoke(this, selected);
                }
            }
        }

        private int GetDayAtPoint(Point pt)
        {
            for (int i = 0; i < _dayRects.Length; i++)
            {
                if (_dayRects[i].Contains(pt))
                    return i;
            }
            return -1;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // Border
            using (Pen borderPen = new Pen(DbdColors.AccentRed, 2f))
            {
                g.DrawRectangle(borderPen, 0, 0, Width - 1, Height - 1);
            }

            DrawHeader(g);
            DrawDayNames(g);
            DrawDays(g);
        }

        private void DrawHeader(Graphics g)
        {
            Rectangle headerRect = new Rectangle(CalendarPadding, CalendarPadding, Width - CalendarPadding * 2, HeaderHeight);

            // Month/Year text
            string monthYear = _displayMonth.ToString("MMMM yyyy", CultureInfo.CurrentCulture);
            using (Font font = new Font("Segoe UI", 12F, FontStyle.Bold))
            using (SolidBrush brush = new SolidBrush(DbdColors.TextPrimary))
            {
                SizeF textSize = g.MeasureString(monthYear, font);
                float x = headerRect.X + (headerRect.Width - textSize.Width) / 2;
                float y = headerRect.Y + (headerRect.Height - textSize.Height) / 2;
                g.DrawString(monthYear, font, brush, x, y);
            }

            // Navigation arrows
            DrawNavigationArrow(g, new Rectangle(CalendarPadding, CalendarPadding, 30, HeaderHeight), true);
            DrawNavigationArrow(g, new Rectangle(Width - CalendarPadding - 30, CalendarPadding, 30, HeaderHeight), false);
        }

        private void DrawNavigationArrow(Graphics g, Rectangle bounds, bool isLeft)
        {
            int arrowSize = 8;
            int centerX = bounds.X + bounds.Width / 2;
            int centerY = bounds.Y + bounds.Height / 2;

            Point[] arrow;
            if (isLeft)
            {
                arrow = new Point[]
                {
                    new Point(centerX + arrowSize/2, centerY - arrowSize/2),
                    new Point(centerX - arrowSize/2, centerY),
                    new Point(centerX + arrowSize/2, centerY + arrowSize/2)
                };
            }
            else
            {
                arrow = new Point[]
                {
                    new Point(centerX - arrowSize/2, centerY - arrowSize/2),
                    new Point(centerX + arrowSize/2, centerY),
                    new Point(centerX - arrowSize/2, centerY + arrowSize/2)
                };
            }

            using (Pen pen = new Pen(DbdColors.TextSecondary, 2f))
            {
                pen.LineJoin = LineJoin.Round;
                g.DrawLines(pen, arrow);
            }
        }

        private void DrawDayNames(Graphics g)
        {
            string[] dayNames = { "Lu", "Ma", "Me", "Gi", "Ve", "Sa", "Do" };
            if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "en")
            {
                dayNames = new[] { "Mo", "Tu", "We", "Th", "Fr", "Sa", "Su" };
            }

            int y = CalendarPadding + HeaderHeight;

            using (Font font = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (SolidBrush brush = new SolidBrush(DbdColors.TextSecondary))
            {
                for (int i = 0; i < 7; i++)
                {
                    Rectangle rect = new Rectangle(CalendarPadding + i * CellSize, y, CellSize, DayNamesHeight);
                    SizeF textSize = g.MeasureString(dayNames[i], font);
                    float x = rect.X + (rect.Width - textSize.Width) / 2;
                    float ty = rect.Y + (rect.Height - textSize.Height) / 2;
                    g.DrawString(dayNames[i], font, brush, x, ty);
                }
            }
        }

        private void DrawDays(Graphics g)
        {
            DateTime firstDayOfMonth = _displayMonth;
            int daysInMonth = DateTime.DaysInMonth(_displayMonth.Year, _displayMonth.Month);
            int startDayOfWeek = ((int)firstDayOfMonth.DayOfWeek + 6) % 7; // Monday = 0

            using (Font font = new Font("Segoe UI", 10F))
            {
                for (int i = 0; i < 42; i++)
                {
                    int dayNumber = i - startDayOfWeek + 1;
                    if (dayNumber < 1 || dayNumber > daysInMonth)
                        continue;

                    Rectangle rect = _dayRects[i];
                    DateTime thisDate = new DateTime(_displayMonth.Year, _displayMonth.Month, dayNumber);
                    bool isSelected = thisDate.Date == _selectedDate.Date;
                    bool isToday = thisDate.Date == DateTime.Today;
                    bool isHovered = i == _hoveredDay;

                    // Background
                    if (isSelected)
                    {
                        using (SolidBrush brush = new SolidBrush(DbdColors.AccentRed))
                        {
                            g.FillRectangle(brush, rect);
                        }
                    }
                    else if (isHovered)
                    {
                        using (SolidBrush brush = new SolidBrush(DbdColors.CardHover))
                        {
                            g.FillRectangle(brush, rect);
                        }
                    }

                    // Today indicator
                    if (isToday && !isSelected)
                    {
                        using (Pen pen = new Pen(DbdColors.AccentRed, 1f))
                        {
                            g.DrawRectangle(pen, rect.X + 2, rect.Y + 2, rect.Width - 5, rect.Height - 5);
                        }
                    }

                    // Day number
                    Color textColor = isSelected ? Color.White : DbdColors.TextPrimary;
                    using (SolidBrush brush = new SolidBrush(textColor))
                    {
                        string dayText = dayNumber.ToString();
                        SizeF textSize = g.MeasureString(dayText, font);
                        float x = rect.X + (rect.Width - textSize.Width) / 2;
                        float y = rect.Y + (rect.Height - textSize.Height) / 2;
                        g.DrawString(dayText, font, brush, x, y);
                    }
                }
            }
        }
    }

    #endregion
}
