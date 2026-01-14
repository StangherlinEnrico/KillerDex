using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using KillerDex.Theme;

namespace KillerDex.Controls
{
    /// <summary>
    /// Custom text box with Dead by Daylight styling.
    /// Features custom scrollbar and placeholder text support.
    /// </summary>
    public class DbdTextBox : UserControl
    {
        #region Win32 API

        [DllImport("user32.dll")]
        private static extern int GetScrollInfo(IntPtr hwnd, int nBar, ref SCROLLINFO lpsi);

        [StructLayout(LayoutKind.Sequential)]
        private struct SCROLLINFO
        {
            public int cbSize;
            public int fMask;
            public int nMin;
            public int nMax;
            public int nPage;
            public int nPos;
            public int nTrackPos;
        }

        private const int SB_VERT = 1;
        private const int SIF_ALL = 0x17;

        #endregion

        #region Private Fields

        private RichTextBox _innerTextBox;
        private Panel _textContainer;
        private bool _isHovered;
        private bool _isFocused;
        private string _placeholderText = "";
        private bool _multiline = true;

        // Scrollbar
        private const int ScrollbarWidth = 10;
        private const int ScrollbarPadding = 4;
        private Rectangle _scrollbarTrack;
        private Rectangle _scrollbarThumb;
        private bool _isScrollbarHovered;
        private bool _isScrollbarDragging;
        private int _dragStartY;
        private int _dragStartScrollPos;

        #endregion

        #region Public Properties

        [Category("Appearance")]
        [Description("Text displayed when the control is empty.")]
        [DefaultValue("")]
        public string PlaceholderText
        {
            get => _placeholderText;
            set
            {
                _placeholderText = value;
                Invalidate();
            }
        }

        [Category("Behavior")]
        [Description("Indicates whether this is a multiline text box.")]
        [DefaultValue(true)]
        public bool Multiline
        {
            get => _multiline;
            set
            {
                _multiline = value;
                if (_innerTextBox != null)
                {
                    _innerTextBox.Multiline = value;
                }
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("The text associated with the control.")]
        [DefaultValue("")]
        public override string Text
        {
            get => _innerTextBox?.Text ?? "";
            set
            {
                if (_innerTextBox != null)
                {
                    _innerTextBox.Text = value;
                }
                Invalidate();
            }
        }

        [Category("Behavior")]
        [Description("Maximum number of characters allowed.")]
        [DefaultValue(32767)]
        public int MaxLength
        {
            get => _innerTextBox?.MaxLength ?? 32767;
            set { if (_innerTextBox != null) _innerTextBox.MaxLength = value; }
        }

        [Category("Behavior")]
        [Description("Indicates whether the text is read-only.")]
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get => _innerTextBox?.ReadOnly ?? false;
            set { if (_innerTextBox != null) _innerTextBox.ReadOnly = value; }
        }

        #endregion

        #region Events

        public new event EventHandler TextChanged;

        #endregion

        #region Constructor

        public DbdTextBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            Size = new Size(310, 100);

            // Container panel to clip the native scrollbar
            _textContainer = new Panel
            {
                Location = new Point(8, 8),
                BackColor = DbdColors.InputBackground
            };
            _textContainer.MouseEnter += (s, e) => { _isHovered = true; Invalidate(); };
            _textContainer.MouseLeave += Container_MouseLeave;

            // Create inner RichTextBox
            _innerTextBox = new RichTextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = DbdColors.InputBackground,
                ForeColor = DbdColors.TextPrimary,
                Font = new Font("Segoe UI", 10F),
                Multiline = true,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                DetectUrls = false,
                ShortcutsEnabled = true
            };

            _innerTextBox.TextChanged += InnerTextBox_TextChanged;
            _innerTextBox.VScroll += InnerTextBox_VScroll;
            _innerTextBox.GotFocus += InnerTextBox_GotFocus;
            _innerTextBox.LostFocus += InnerTextBox_LostFocus;
            _innerTextBox.MouseEnter += (s, e) => { _isHovered = true; Invalidate(); };
            _innerTextBox.MouseLeave += Container_MouseLeave;
            _innerTextBox.MouseWheel += InnerTextBox_MouseWheel;

            _textContainer.Controls.Add(_innerTextBox);
            Controls.Add(_textContainer);

            UpdateLayout();
        }

        #endregion

        #region Event Handlers

        private void Container_MouseLeave(object sender, EventArgs e)
        {
            Point mousePos = PointToClient(MousePosition);
            if (!ClientRectangle.Contains(mousePos))
            {
                _isHovered = false;
                Invalidate();
            }
        }

        private void InnerTextBox_TextChanged(object sender, EventArgs e)
        {
            TextChanged?.Invoke(this, e);
            UpdateScrollbar();
            Invalidate();
        }

        private void InnerTextBox_VScroll(object sender, EventArgs e)
        {
            UpdateScrollbar();
            Invalidate();
        }

        private void InnerTextBox_MouseWheel(object sender, MouseEventArgs e)
        {
            // Let the default handling occur, then update scrollbar
            BeginInvoke(new Action(() =>
            {
                UpdateScrollbar();
                Invalidate();
            }));
        }

        private void InnerTextBox_GotFocus(object sender, EventArgs e)
        {
            _isFocused = true;
            Invalidate();
        }

        private void InnerTextBox_LostFocus(object sender, EventArgs e)
        {
            _isFocused = false;
            Invalidate();
        }

        #endregion

        #region Overrides

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovered = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Point mousePos = PointToClient(MousePosition);
            if (!ClientRectangle.Contains(mousePos))
            {
                _isHovered = false;
                _isScrollbarHovered = false;
                Invalidate();
            }
            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_isScrollbarDragging)
            {
                // Calculate new scroll position based on drag
                int deltaY = e.Y - _dragStartY;
                int trackHeight = _scrollbarTrack.Height - _scrollbarThumb.Height;

                if (trackHeight > 0)
                {
                    float scrollRatio = (float)deltaY / trackHeight;
                    int totalLines = GetTotalScrollRange();
                    int newPos = _dragStartScrollPos + (int)(scrollRatio * totalLines);

                    ScrollToLine(Math.Max(0, Math.Min(newPos, totalLines)));
                }
            }
            else
            {
                bool wasHovered = _isScrollbarHovered;
                _isScrollbarHovered = _scrollbarTrack.Contains(e.Location);

                if (wasHovered != _isScrollbarHovered)
                    Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                if (_scrollbarThumb.Contains(e.Location))
                {
                    _isScrollbarDragging = true;
                    _dragStartY = e.Y;
                    _dragStartScrollPos = GetCurrentScrollPosition();
                    Capture = true;
                }
                else if (_scrollbarTrack.Contains(e.Location))
                {
                    // Click on track - page up/down
                    if (e.Y < _scrollbarThumb.Y)
                        PageUp();
                    else if (e.Y > _scrollbarThumb.Bottom)
                        PageDown();
                }
                else
                {
                    _innerTextBox?.Focus();
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_isScrollbarDragging)
            {
                _isScrollbarDragging = false;
                Capture = false;
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            // Forward to inner textbox if not already handled
            if (_innerTextBox != null && _innerTextBox.Focused == false)
            {
                int lines = -e.Delta / 40;
                ScrollByLines(lines);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateLayout();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

            // Determine colors based on state
            Color bgColor = _isHovered ? DbdColors.CardHover : DbdColors.InputBackground;
            Color borderColor = _isFocused ? DbdColors.AccentRed :
                               (_isHovered ? DbdColors.BorderHover : DbdColors.Border);
            float borderWidth = _isFocused ? 2f : 1f;

            // Draw background
            using (GraphicsPath path = CreateRoundedRectangle(rect, 4))
            {
                using (SolidBrush brush = new SolidBrush(bgColor))
                {
                    g.FillPath(brush, path);
                }

                using (Pen pen = new Pen(borderColor, borderWidth))
                {
                    g.DrawPath(pen, path);
                }
            }

            // Update container and textbox background
            if (_textContainer != null)
                _textContainer.BackColor = bgColor;
            if (_innerTextBox != null)
                _innerTextBox.BackColor = bgColor;

            // Draw scrollbar if needed
            if (_multiline && NeedsScrollbar())
            {
                DrawScrollbar(g);
            }

            // Draw placeholder if empty and not focused
            if (string.IsNullOrEmpty(_innerTextBox?.Text) && !_isFocused && !string.IsNullOrEmpty(_placeholderText))
            {
                using (Font font = new Font("Segoe UI", 10F, FontStyle.Italic))
                using (SolidBrush brush = new SolidBrush(DbdColors.TextSecondary))
                {
                    g.DrawString(_placeholderText, font, brush, 10, 10);
                }
            }
        }

        #endregion

        #region Scrollbar

        private void DrawScrollbar(Graphics g)
        {
            // Draw track
            Color trackColor = Color.FromArgb(25, 25, 30);
            using (SolidBrush brush = new SolidBrush(trackColor))
            {
                g.FillRectangle(brush, _scrollbarTrack);
            }

            // Draw thumb
            Color thumbColor = _isScrollbarDragging ? DbdColors.AccentRed :
                              (_isScrollbarHovered ? DbdColors.BorderHover : DbdColors.Border);

            using (GraphicsPath thumbPath = CreateRoundedRectangle(_scrollbarThumb, 3))
            using (SolidBrush brush = new SolidBrush(thumbColor))
            {
                g.FillPath(brush, thumbPath);
            }
        }

        private void UpdateScrollbar()
        {
            if (_innerTextBox == null || !_multiline) return;

            int scrollbarX = Width - ScrollbarWidth - ScrollbarPadding - 2;
            int scrollbarY = ScrollbarPadding + 4;
            int scrollbarHeight = Height - (ScrollbarPadding * 2) - 8;

            _scrollbarTrack = new Rectangle(scrollbarX, scrollbarY, ScrollbarWidth, scrollbarHeight);

            // Calculate thumb size and position
            int totalRange = GetTotalScrollRange();
            int visibleLines = GetVisibleLines();
            int currentPos = GetCurrentScrollPosition();

            if (totalRange <= 0 || visibleLines >= totalRange)
            {
                // No scrolling needed - thumb fills track
                _scrollbarThumb = _scrollbarTrack;
            }
            else
            {
                // Calculate thumb height (minimum 20px)
                float thumbRatio = (float)visibleLines / (totalRange + visibleLines);
                int thumbHeight = Math.Max(20, (int)(scrollbarHeight * thumbRatio));

                // Calculate thumb position
                float posRatio = (float)currentPos / totalRange;
                int maxThumbY = scrollbarHeight - thumbHeight;
                int thumbY = scrollbarY + (int)(posRatio * maxThumbY);

                _scrollbarThumb = new Rectangle(scrollbarX, thumbY, ScrollbarWidth, thumbHeight);
            }
        }

        private bool NeedsScrollbar()
        {
            if (_innerTextBox == null) return false;
            return GetTotalScrollRange() > 0;
        }

        private int GetTotalScrollRange()
        {
            if (_innerTextBox == null || _innerTextBox.Handle == IntPtr.Zero) return 0;

            try
            {
                SCROLLINFO si = new SCROLLINFO();
                si.cbSize = Marshal.SizeOf(si);
                si.fMask = SIF_ALL;

                if (GetScrollInfo(_innerTextBox.Handle, SB_VERT, ref si) != 0)
                {
                    return Math.Max(0, si.nMax - si.nPage + 1);
                }
            }
            catch { }

            return 0;
        }

        private int GetCurrentScrollPosition()
        {
            if (_innerTextBox == null || _innerTextBox.Handle == IntPtr.Zero) return 0;

            try
            {
                SCROLLINFO si = new SCROLLINFO();
                si.cbSize = Marshal.SizeOf(si);
                si.fMask = SIF_ALL;

                if (GetScrollInfo(_innerTextBox.Handle, SB_VERT, ref si) != 0)
                {
                    return si.nPos;
                }
            }
            catch { }

            return 0;
        }

        private int GetVisibleLines()
        {
            if (_innerTextBox == null || _innerTextBox.Handle == IntPtr.Zero) return 0;

            try
            {
                SCROLLINFO si = new SCROLLINFO();
                si.cbSize = Marshal.SizeOf(si);
                si.fMask = SIF_ALL;

                if (GetScrollInfo(_innerTextBox.Handle, SB_VERT, ref si) != 0)
                {
                    return (int)si.nPage;
                }
            }
            catch { }

            return 0;
        }

        private void ScrollToLine(int line)
        {
            if (_innerTextBox == null) return;

            int charIndex = _innerTextBox.GetFirstCharIndexFromLine(Math.Max(0, line));
            if (charIndex >= 0)
            {
                _innerTextBox.Select(charIndex, 0);
                _innerTextBox.ScrollToCaret();
            }
            UpdateScrollbar();
            Invalidate();
        }

        private void ScrollByLines(int lines)
        {
            if (_innerTextBox == null) return;

            int currentLine = _innerTextBox.GetLineFromCharIndex(_innerTextBox.GetFirstCharIndexOfCurrentLine());
            ScrollToLine(currentLine + lines);
        }

        private void PageUp()
        {
            ScrollByLines(-GetVisibleLines());
        }

        private void PageDown()
        {
            ScrollByLines(GetVisibleLines());
        }

        #endregion

        #region Private Methods

        private void UpdateLayout()
        {
            if (_textContainer == null || _innerTextBox == null) return;

            int padding = 8;
            int scrollbarSpace = _multiline ? ScrollbarWidth + ScrollbarPadding + 4 : 0;

            _textContainer.Location = new Point(padding, padding);
            _textContainer.Size = new Size(Width - padding * 2 - scrollbarSpace, Height - padding * 2);

            // Make textbox wider than container to hide native scrollbar
            int nativeScrollbarWidth = SystemInformation.VerticalScrollBarWidth;
            _innerTextBox.Location = new Point(0, 0);
            _innerTextBox.Size = new Size(_textContainer.Width + nativeScrollbarWidth + 5, _textContainer.Height);

            UpdateScrollbar();
        }

        private GraphicsPath CreateRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            if (rect.Width < radius * 2 || rect.Height < radius * 2)
            {
                path.AddRectangle(rect);
                return path;
            }

            int diameter = radius * 2;

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        #endregion

        #region Public Methods

        public void Clear()
        {
            _innerTextBox?.Clear();
        }

        public void SelectAll()
        {
            _innerTextBox?.SelectAll();
        }

        public new bool Focus()
        {
            return _innerTextBox?.Focus() ?? false;
        }

        #endregion
    }
}
