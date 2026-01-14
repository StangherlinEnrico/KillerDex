using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using KillerDex.Resources;
using KillerDex.Theme;

namespace KillerDex.Controls
{
    /// <summary>
    /// Custom multi-select control with Dead by Daylight styling.
    /// Features a styled dropdown list with checkboxes and search functionality.
    /// </summary>
    public class DbdMultiSelect : UserControl
    {
        #region Private Fields

        private readonly List<object> _items;
        private readonly HashSet<int> _selectedIndices;
        private string _displayMember = "";
        private object _dataSource;
        private bool _isHovered;
        private bool _isDropdownOpen;
        private DbdMultiSelectDropdown _dropdown;
        private int _maxSelection = 3;

        #endregion

        #region Events

        public event EventHandler SelectionChanged;

        #endregion

        #region Public Properties

        [Category("Data")]
        [Description("The data source for the multi-select.")]
        [DefaultValue(null)]
        public object DataSource
        {
            get => _dataSource;
            set
            {
                _dataSource = value;
                PopulateFromDataSource();
                Invalidate();
            }
        }

        [Category("Data")]
        [Description("The property to display.")]
        [DefaultValue("")]
        public string DisplayMember
        {
            get => _displayMember;
            set
            {
                _displayMember = value;
                Invalidate();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IReadOnlyList<object> SelectedItems
        {
            get => _selectedIndices.Select(i => _items[i]).ToList();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IReadOnlyList<int> SelectedIndices => _selectedIndices.ToList();

        [Category("Behavior")]
        [Description("Maximum number of items that can be selected.")]
        [DefaultValue(3)]
        public int MaxSelection
        {
            get => _maxSelection;
            set
            {
                _maxSelection = Math.Max(1, value);
                // Remove excess selections if needed
                while (_selectedIndices.Count > _maxSelection)
                {
                    _selectedIndices.Remove(_selectedIndices.Last());
                }
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Text to show when no item is selected.")]
        [DefaultValue("")]
        public string PlaceholderText { get; set; } = "";

        #endregion

        #region Constructor

        public DbdMultiSelect()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.Selectable, true);

            _items = new List<object>();
            _selectedIndices = new HashSet<int>();

            Size = new Size(310, 32);
            Cursor = Cursors.Hand;
            TabStop = true;
        }

        #endregion

        #region Public Methods

        public void ClearItems()
        {
            _items.Clear();
            _selectedIndices.Clear();
            Invalidate();
        }

        public void AddItem(object item)
        {
            _items.Add(item);
            Invalidate();
        }

        public void SetSelectedItems(IEnumerable<object> items)
        {
            _selectedIndices.Clear();
            foreach (var item in items)
            {
                int index = _items.IndexOf(item);
                if (index >= 0 && _selectedIndices.Count < _maxSelection)
                {
                    _selectedIndices.Add(index);
                }
            }
            Invalidate();
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ClearSelection()
        {
            _selectedIndices.Clear();
            Invalidate();
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Data Binding

        private void PopulateFromDataSource()
        {
            _items.Clear();
            _selectedIndices.Clear();

            if (_dataSource == null) return;

            if (_dataSource is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    _items.Add(item);
                }
            }
        }

        private object GetPropertyValue(object obj, string propertyName)
        {
            if (obj == null || string.IsNullOrEmpty(propertyName)) return obj;
            var prop = obj.GetType().GetProperty(propertyName);
            return prop?.GetValue(obj);
        }

        private string GetDisplayText(object item)
        {
            if (item == null) return "";
            if (string.IsNullOrEmpty(_displayMember)) return item.ToString();
            var value = GetPropertyValue(item, _displayMember);
            return value?.ToString() ?? "";
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

            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down)
            {
                if (!_isDropdownOpen)
                {
                    OpenDropdown();
                }
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
                CloseDropdown();
            else
                OpenDropdown();
        }

        private void OpenDropdown()
        {
            if (_isDropdownOpen || _items.Count == 0) return;

            _dropdown = new DbdMultiSelectDropdown(
                _items,
                _selectedIndices,
                _maxSelection,
                _displayMember,
                GetDisplayText);

            _dropdown.SelectionChanged += Dropdown_SelectionChanged;
            _dropdown.DropdownClosed += Dropdown_Closed;

            // Calculate dropdown height (search box + items)
            int itemHeight = 32;
            int searchBoxHeight = 36;
            int maxVisibleItems = Math.Min(_items.Count, 8);
            int dropdownHeight = searchBoxHeight + maxVisibleItems * itemHeight + 4;

            _dropdown.Size = new Size(Width, dropdownHeight);

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

        private void Dropdown_SelectionChanged(object sender, HashSet<int> selectedIndices)
        {
            _selectedIndices.Clear();
            foreach (var index in selectedIndices)
            {
                _selectedIndices.Add(index);
            }
            Invalidate();
            SelectionChanged?.Invoke(this, EventArgs.Empty);
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

            // Text
            string displayText = GetSelectionDisplayText();
            bool hasSelection = _selectedIndices.Count > 0;
            Color textColor = hasSelection ? DbdColors.TextPrimary : DbdColors.TextSecondary;

            using (Font font = new Font("Segoe UI", 10F))
            using (SolidBrush textBrush = new SolidBrush(textColor))
            {
                SizeF textSize = g.MeasureString(displayText, font);
                float textY = (Height - textSize.Height) / 2;

                // Clip text to not overlap with arrow
                Rectangle textRect = new Rectangle(12, 0, Width - 44, Height);
                g.SetClip(textRect);
                g.DrawString(displayText, font, textBrush, 12, textY);
                g.ResetClip();
            }

            // Selection count badge (if items selected)
            if (hasSelection)
            {
                DrawSelectionBadge(g, _selectedIndices.Count);
            }

            // Dropdown arrow
            DrawDropdownArrow(g, new Rectangle(Width - 32, 0, 32, Height));

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

        private string GetSelectionDisplayText()
        {
            if (_selectedIndices.Count == 0)
            {
                return PlaceholderText;
            }

            var selectedNames = _selectedIndices
                .OrderBy(i => i)
                .Select(i => GetDisplayText(_items[i]))
                .ToList();

            return string.Join(", ", selectedNames);
        }

        private void DrawSelectionBadge(Graphics g, int count)
        {
            int badgeSize = 18;
            int badgeX = Width - 55;
            int badgeY = (Height - badgeSize) / 2;

            using (SolidBrush bgBrush = new SolidBrush(DbdColors.AccentRed))
            {
                g.FillEllipse(bgBrush, badgeX, badgeY, badgeSize, badgeSize);
            }

            using (Font font = new Font("Segoe UI", 8F, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(Color.White))
            {
                string countText = count.ToString();
                SizeF textSize = g.MeasureString(countText, font);
                float textX = badgeX + (badgeSize - textSize.Width) / 2;
                float textY = badgeY + (badgeSize - textSize.Height) / 2;
                g.DrawString(countText, font, textBrush, textX, textY);
            }
        }

        private void DrawDropdownArrow(Graphics g, Rectangle bounds)
        {
            int arrowWidth = 8;
            int arrowHeight = 5;
            int centerX = bounds.X + bounds.Width / 2;
            int centerY = bounds.Y + bounds.Height / 2;

            Point[] arrow = new Point[]
            {
                new Point(centerX - arrowWidth / 2, centerY - arrowHeight / 2),
                new Point(centerX + arrowWidth / 2, centerY - arrowHeight / 2),
                new Point(centerX, centerY + arrowHeight / 2)
            };

            Color arrowColor = _isHovered ? DbdColors.AccentRed : DbdColors.TextSecondary;
            using (SolidBrush brush = new SolidBrush(arrowColor))
            {
                g.FillPolygon(brush, arrow);
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

    #region Dropdown Form

    internal class DbdMultiSelectDropdown : Form
    {
        private readonly List<object> _allItems;
        private readonly List<int> _filteredIndices;
        private readonly HashSet<int> _selectedIndices;
        private readonly int _maxSelection;
        private readonly string _displayMember;
        private readonly Func<object, string> _getDisplayText;
        private int _hoveredFilteredIndex = -1;
        private int _scrollOffset = 0;
        private const int ItemHeight = 32;
        private const int SearchBoxHeight = 36;
        private const int CheckboxSize = 16;

        private readonly TextBox _searchBox;
        private readonly Panel _itemsPanel;

        public event EventHandler<HashSet<int>> SelectionChanged;
        public event EventHandler DropdownClosed;

        public DbdMultiSelectDropdown(
            List<object> items,
            HashSet<int> selectedIndices,
            int maxSelection,
            string displayMember,
            Func<object, string> getDisplayText)
        {
            _allItems = items;
            _filteredIndices = new List<int>();
            _selectedIndices = new HashSet<int>(selectedIndices);
            _maxSelection = maxSelection;
            _displayMember = displayMember;
            _getDisplayText = getDisplayText;

            // Initialize filtered indices with all items
            for (int i = 0; i < items.Count; i++)
                _filteredIndices.Add(i);

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            TopMost = true;
            BackColor = DbdColors.CardBackground;

            // Create search box
            _searchBox = new TextBox
            {
                Dock = DockStyle.Top,
                Height = SearchBoxHeight,
                BackColor = DbdColors.InputBackground,
                ForeColor = DbdColors.TextPrimary,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10F),
                Text = ""
            };
            _searchBox.TextChanged += SearchBox_TextChanged;
            _searchBox.KeyDown += SearchBox_KeyDown;

            // Create search box container with padding
            var searchContainer = new Panel
            {
                Dock = DockStyle.Top,
                Height = SearchBoxHeight,
                BackColor = DbdColors.InputBackground,
                Padding = new Padding(10, 8, 10, 8)
            };
            searchContainer.Controls.Add(_searchBox);
            _searchBox.Dock = DockStyle.Fill;

            // Create items panel
            _itemsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = DbdColors.CardBackground
            };
            _itemsPanel.Paint += ItemsPanel_Paint;
            _itemsPanel.MouseMove += ItemsPanel_MouseMove;
            _itemsPanel.MouseLeave += ItemsPanel_MouseLeave;
            _itemsPanel.MouseClick += ItemsPanel_MouseClick;
            _itemsPanel.MouseWheel += ItemsPanel_MouseWheel;

            Controls.Add(_itemsPanel);
            Controls.Add(searchContainer);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _searchBox.Focus();
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            DropdownClosed?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            FilterItems(_searchBox.Text);
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (_hoveredFilteredIndex < _filteredIndices.Count - 1)
                    _hoveredFilteredIndex++;
                else
                    _hoveredFilteredIndex = 0;

                EnsureVisible(_hoveredFilteredIndex);
                _itemsPanel.Invalidate();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (_hoveredFilteredIndex > 0)
                    _hoveredFilteredIndex--;
                else
                    _hoveredFilteredIndex = _filteredIndices.Count - 1;

                EnsureVisible(_hoveredFilteredIndex);
                _itemsPanel.Invalidate();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
            {
                if (_hoveredFilteredIndex >= 0 && _hoveredFilteredIndex < _filteredIndices.Count)
                {
                    ToggleSelection(_filteredIndices[_hoveredFilteredIndex]);
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                DropdownClosed?.Invoke(this, EventArgs.Empty);
                Close();
                e.Handled = true;
            }
        }

        private void ToggleSelection(int actualIndex)
        {
            if (_selectedIndices.Contains(actualIndex))
            {
                _selectedIndices.Remove(actualIndex);
                SelectionChanged?.Invoke(this, _selectedIndices);
                _itemsPanel.Invalidate();
            }
            else if (_selectedIndices.Count < _maxSelection)
            {
                _selectedIndices.Add(actualIndex);
                SelectionChanged?.Invoke(this, _selectedIndices);
                _itemsPanel.Invalidate();
            }
            // If max selection reached and item not selected, do nothing (visual feedback via grayed out checkbox)
        }

        private void EnsureVisible(int filteredIndex)
        {
            int maxVisible = (_itemsPanel.Height) / ItemHeight;
            if (filteredIndex < _scrollOffset)
                _scrollOffset = filteredIndex;
            else if (filteredIndex >= _scrollOffset + maxVisible)
                _scrollOffset = filteredIndex - maxVisible + 1;
        }

        private void FilterItems(string searchText)
        {
            _filteredIndices.Clear();
            _scrollOffset = 0;
            _hoveredFilteredIndex = -1;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                for (int i = 0; i < _allItems.Count; i++)
                    _filteredIndices.Add(i);
            }
            else
            {
                string search = searchText.ToLowerInvariant();
                for (int i = 0; i < _allItems.Count; i++)
                {
                    string itemText = _getDisplayText(_allItems[i]).ToLowerInvariant();
                    if (itemText.Contains(search))
                        _filteredIndices.Add(i);
                }
            }

            // Pre-select first filtered item
            if (_filteredIndices.Count > 0)
                _hoveredFilteredIndex = 0;

            _itemsPanel.Invalidate();
        }

        private void ItemsPanel_MouseMove(object sender, MouseEventArgs e)
        {
            int newHovered = GetFilteredIndexAtPoint(e.Location);
            if (newHovered != _hoveredFilteredIndex)
            {
                _hoveredFilteredIndex = newHovered;
                _itemsPanel.Invalidate();
            }
        }

        private void ItemsPanel_MouseLeave(object sender, EventArgs e)
        {
            // Don't clear hover when leaving, keep keyboard selection
        }

        private void ItemsPanel_MouseClick(object sender, MouseEventArgs e)
        {
            int filteredIndex = GetFilteredIndexAtPoint(e.Location);
            if (filteredIndex >= 0 && filteredIndex < _filteredIndices.Count)
            {
                ToggleSelection(_filteredIndices[filteredIndex]);
            }
        }

        private void ItemsPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            int maxScroll = Math.Max(0, _filteredIndices.Count - (_itemsPanel.Height / ItemHeight));
            if (e.Delta > 0)
                _scrollOffset = Math.Max(0, _scrollOffset - 1);
            else
                _scrollOffset = Math.Min(maxScroll, _scrollOffset + 1);
            _itemsPanel.Invalidate();
        }

        private int GetFilteredIndexAtPoint(Point pt)
        {
            if (pt.X < 2 || pt.X > _itemsPanel.Width - 2 || pt.Y < 0 || pt.Y > _itemsPanel.Height)
                return -1;

            int index = pt.Y / ItemHeight + _scrollOffset;
            if (index >= 0 && index < _filteredIndices.Count)
                return index;
            return -1;
        }

        private void ItemsPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // Background
            using (SolidBrush bgBrush = new SolidBrush(DbdColors.CardBackground))
            {
                g.FillRectangle(bgBrush, _itemsPanel.ClientRectangle);
            }

            if (_filteredIndices.Count == 0)
            {
                // No results message
                using (Font font = new Font("Segoe UI", 10F, FontStyle.Italic))
                using (SolidBrush brush = new SolidBrush(DbdColors.TextSecondary))
                {
                    string noResults = Strings.Search_NoResults;
                    SizeF textSize = g.MeasureString(noResults, font);
                    float textX = (_itemsPanel.Width - textSize.Width) / 2;
                    float textY = (_itemsPanel.Height - textSize.Height) / 2;
                    g.DrawString(noResults, font, brush, textX, textY);
                }
                return;
            }

            // Items
            int itemY = 0;
            int maxVisible = _itemsPanel.Height / ItemHeight + 1;

            using (Font font = new Font("Segoe UI", 10F))
            {
                for (int i = 0; i < maxVisible && i + _scrollOffset < _filteredIndices.Count; i++)
                {
                    int filteredIndex = i + _scrollOffset;
                    int actualIndex = _filteredIndices[filteredIndex];
                    Rectangle itemRect = new Rectangle(2, itemY, _itemsPanel.Width - 4, ItemHeight);
                    DrawItem(g, itemRect, actualIndex, filteredIndex, font);
                    itemY += ItemHeight;
                }
            }

            // Scroll indicators
            int visibleCount = _itemsPanel.Height / ItemHeight;
            if (_scrollOffset > 0)
            {
                DrawScrollIndicator(g, true);
            }
            if (_scrollOffset + visibleCount < _filteredIndices.Count)
            {
                DrawScrollIndicator(g, false);
            }
        }

        private void DrawItem(Graphics g, Rectangle rect, int actualIndex, int filteredIndex, Font font)
        {
            bool isSelected = _selectedIndices.Contains(actualIndex);
            bool isHovered = filteredIndex == _hoveredFilteredIndex;
            bool canSelect = isSelected || _selectedIndices.Count < _maxSelection;

            // Background
            if (isHovered)
            {
                using (SolidBrush brush = new SolidBrush(DbdColors.CardHover))
                {
                    g.FillRectangle(brush, rect);
                }
            }

            // Checkbox
            int checkboxX = rect.X + 10;
            int checkboxY = rect.Y + (rect.Height - CheckboxSize) / 2;
            Rectangle checkboxRect = new Rectangle(checkboxX, checkboxY, CheckboxSize, CheckboxSize);

            // Checkbox background
            Color checkboxBg = isSelected ? DbdColors.AccentRed :
                              (canSelect ? DbdColors.InputBackground : Color.FromArgb(25, 25, 30));
            using (SolidBrush brush = new SolidBrush(checkboxBg))
            {
                g.FillRectangle(brush, checkboxRect);
            }

            // Checkbox border
            Color checkboxBorder = isSelected ? DbdColors.AccentRed :
                                  (canSelect ? DbdColors.Border : Color.FromArgb(50, 50, 55));
            using (Pen pen = new Pen(checkboxBorder, 1f))
            {
                g.DrawRectangle(pen, checkboxRect);
            }

            // Checkmark
            if (isSelected)
            {
                using (Pen pen = new Pen(Color.White, 2f))
                {
                    Point[] checkmark = new Point[]
                    {
                        new Point(checkboxX + 3, checkboxY + 8),
                        new Point(checkboxX + 6, checkboxY + 11),
                        new Point(checkboxX + 12, checkboxY + 5)
                    };
                    g.DrawLines(pen, checkmark);
                }
            }

            // Text
            string text = _getDisplayText(_allItems[actualIndex]);
            Color textColor = canSelect ? (isHovered ? Color.White : DbdColors.TextPrimary) : DbdColors.TextSecondary;

            using (SolidBrush brush = new SolidBrush(textColor))
            {
                SizeF textSize = g.MeasureString(text, font);
                float textY = rect.Y + (rect.Height - textSize.Height) / 2;
                g.DrawString(text, font, brush, checkboxX + CheckboxSize + 10, textY);
            }

            // Hover indicator
            if (isHovered)
            {
                using (SolidBrush brush = new SolidBrush(DbdColors.AccentRed))
                {
                    g.FillRectangle(brush, rect.Right - 5, rect.Y + 4, 3, rect.Height - 8);
                }
            }
        }

        private void DrawScrollIndicator(Graphics g, bool isTop)
        {
            int y = isTop ? 8 : _itemsPanel.Height - 12;
            int centerX = _itemsPanel.Width / 2;

            Point[] arrow;
            if (isTop)
            {
                arrow = new Point[]
                {
                    new Point(centerX - 6, y + 4),
                    new Point(centerX + 6, y + 4),
                    new Point(centerX, y)
                };
            }
            else
            {
                arrow = new Point[]
                {
                    new Point(centerX - 6, y),
                    new Point(centerX + 6, y),
                    new Point(centerX, y + 4)
                };
            }

            using (SolidBrush brush = new SolidBrush(DbdColors.TextSecondary))
            {
                g.FillPolygon(brush, arrow);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Border around entire dropdown
            using (Pen borderPen = new Pen(DbdColors.AccentRed, 2f))
            {
                e.Graphics.DrawRectangle(borderPen, 0, 0, Width - 1, Height - 1);
            }
        }
    }

    #endregion
}
