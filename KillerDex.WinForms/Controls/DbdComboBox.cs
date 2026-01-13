using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using KillerDex.Resources;
using KillerDex.Theme;

namespace KillerDex.Controls
{
    /// <summary>
    /// Custom combo box with Dead by Daylight styling.
    /// Features a styled dropdown list with dark theme.
    /// </summary>
    public class DbdComboBox : UserControl
    {
        #region Private Fields

        private object _selectedItem;
        private int _selectedIndex = -1;
        private string _displayMember = "";
        private string _valueMember = "";
        private object _dataSource;
        private readonly BindingList<object> _items;
        private bool _isHovered;
        private bool _isDropdownOpen;
        private DbdComboBoxDropdown _dropdown;

        #endregion

        #region Events

        public event EventHandler SelectedIndexChanged;
        public event EventHandler SelectedValueChanged;

        #endregion

        #region Public Properties

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ObjectCollection Items { get; }

        [Category("Data")]
        [Description("The data source for the combo box.")]
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

        [Category("Data")]
        [Description("The property to use as value.")]
        [DefaultValue("")]
        public string ValueMember
        {
            get => _valueMember;
            set => _valueMember = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    _selectedIndex = FindItemIndex(value);
                    Invalidate();
                    SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
                    SelectedValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (value >= -1 && value < _items.Count)
                {
                    _selectedIndex = value;
                    _selectedItem = value >= 0 ? _items[value] : null;
                    Invalidate();
                    SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
                    SelectedValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedValue
        {
            get
            {
                if (_selectedItem == null) return null;
                if (string.IsNullOrEmpty(_valueMember)) return _selectedItem;
                return GetPropertyValue(_selectedItem, _valueMember);
            }
            set
            {
                if (string.IsNullOrEmpty(_valueMember)) return;
                for (int i = 0; i < _items.Count; i++)
                {
                    var itemValue = GetPropertyValue(_items[i], _valueMember);
                    if (Equals(itemValue, value))
                    {
                        SelectedIndex = i;
                        return;
                    }
                }
            }
        }

        [Category("Appearance")]
        [Description("Text to show when no item is selected.")]
        [DefaultValue("")]
        public string PlaceholderText { get; set; } = "";

        #endregion

        #region Constructor

        public DbdComboBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.Selectable, true);

            _items = new BindingList<object>();
            Items = new ObjectCollection(_items);

            Size = new Size(200, 32);
            Cursor = Cursors.Hand;
            TabStop = true;
        }

        #endregion

        #region Data Binding

        private void PopulateFromDataSource()
        {
            _items.Clear();
            _selectedIndex = -1;
            _selectedItem = null;

            if (_dataSource == null) return;

            if (_dataSource is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    _items.Add(item);
                }
            }
        }

        private int FindItemIndex(object item)
        {
            if (item == null) return -1;
            for (int i = 0; i < _items.Count; i++)
            {
                if (Equals(_items[i], item)) return i;
            }
            return -1;
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
            else if (e.KeyCode == Keys.Up && !_isDropdownOpen && _selectedIndex > 0)
            {
                SelectedIndex--;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Down && !_isDropdownOpen && _selectedIndex < _items.Count - 1)
            {
                SelectedIndex++;
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

            _dropdown = new DbdComboBoxDropdown(_items, _selectedIndex, _displayMember, GetDisplayText);
            _dropdown.ItemSelected += Dropdown_ItemSelected;
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

        private void Dropdown_ItemSelected(object sender, int index)
        {
            SelectedIndex = index;
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

            // Text
            string displayText = _selectedItem != null ? GetDisplayText(_selectedItem) : PlaceholderText;
            Color textColor = _selectedItem != null ? DbdColors.TextPrimary : DbdColors.TextSecondary;

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

        #region ObjectCollection

        public class ObjectCollection : IList
        {
            private readonly BindingList<object> _innerList;

            internal ObjectCollection(BindingList<object> list)
            {
                _innerList = list;
            }

            public int Count => _innerList.Count;
            public bool IsReadOnly => false;
            public bool IsFixedSize => false;
            public bool IsSynchronized => false;
            public object SyncRoot => this;

            public object this[int index]
            {
                get => _innerList[index];
                set => _innerList[index] = value;
            }

            public int Add(object value)
            {
                _innerList.Add(value);
                return _innerList.Count - 1;
            }

            public void Clear() => _innerList.Clear();
            public bool Contains(object value) => _innerList.Contains(value);
            public int IndexOf(object value) => _innerList.IndexOf(value);
            public void Insert(int index, object value) => _innerList.Insert(index, value);
            public void Remove(object value) => _innerList.Remove(value);
            public void RemoveAt(int index) => _innerList.RemoveAt(index);
            public void CopyTo(Array array, int index) => ((IList)_innerList).CopyTo(array, index);
            public IEnumerator GetEnumerator() => _innerList.GetEnumerator();
        }

        #endregion
    }

    #region Dropdown Form

    internal class DbdComboBoxDropdown : Form
    {
        private readonly BindingList<object> _allItems;
        private readonly List<int> _filteredIndices;
        private readonly int _selectedIndex;
        private readonly string _displayMember;
        private readonly Func<object, string> _getDisplayText;
        private int _hoveredFilteredIndex = -1;
        private int _scrollOffset = 0;
        private const int ItemHeight = 32;
        private const int SearchBoxHeight = 36;

        private readonly TextBox _searchBox;
        private readonly Panel _itemsPanel;

        public event EventHandler<int> ItemSelected;
        public event EventHandler DropdownClosed;

        public DbdComboBoxDropdown(BindingList<object> items, int selectedIndex,
            string displayMember, Func<object, string> getDisplayText)
        {
            _allItems = items;
            _filteredIndices = new List<int>();
            _selectedIndex = selectedIndex;
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

            // Scroll to selected item if needed
            int selectedFilteredIndex = _filteredIndices.IndexOf(selectedIndex);
            if (selectedFilteredIndex > 5)
            {
                _scrollOffset = Math.Min(selectedFilteredIndex - 3, _filteredIndices.Count - 8);
                if (_scrollOffset < 0) _scrollOffset = 0;
            }
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
            else if (e.KeyCode == Keys.Enter)
            {
                if (_hoveredFilteredIndex >= 0 && _hoveredFilteredIndex < _filteredIndices.Count)
                {
                    ItemSelected?.Invoke(this, _filteredIndices[_hoveredFilteredIndex]);
                }
                else if (_filteredIndices.Count > 0)
                {
                    ItemSelected?.Invoke(this, _filteredIndices[0]);
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
                ItemSelected?.Invoke(this, _filteredIndices[filteredIndex]);
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
            bool isSelected = actualIndex == _selectedIndex;
            bool isHovered = filteredIndex == _hoveredFilteredIndex;

            // Background
            if (isHovered)
            {
                using (SolidBrush brush = new SolidBrush(DbdColors.CardHover))
                {
                    g.FillRectangle(brush, rect);
                }
            }
            else if (isSelected)
            {
                using (SolidBrush brush = new SolidBrush(DbdColors.AccentRedDark))
                {
                    g.FillRectangle(brush, rect);
                }
            }

            // Text
            string text = _getDisplayText(_allItems[actualIndex]);
            Color textColor = (isSelected || isHovered) ? Color.White : DbdColors.TextPrimary;

            using (SolidBrush brush = new SolidBrush(textColor))
            {
                SizeF textSize = g.MeasureString(text, font);
                float textY = rect.Y + (rect.Height - textSize.Height) / 2;
                g.DrawString(text, font, brush, rect.X + 12, textY);
            }

            // Selection indicator (original selected item)
            if (isSelected)
            {
                using (SolidBrush brush = new SolidBrush(DbdColors.AccentRed))
                {
                    g.FillRectangle(brush, rect.X, rect.Y + 4, 3, rect.Height - 8);
                }
            }

            // Hover indicator
            if (isHovered && !isSelected)
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
