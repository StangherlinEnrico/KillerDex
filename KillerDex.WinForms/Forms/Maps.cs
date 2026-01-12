using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using KillerDex.Core.Models;
using KillerDex.Infrastructure.Services;
using KillerDex.Resources;

namespace KillerDex
{
    public partial class Maps : Form
    {
        private readonly MapService _service;
        private Map _selectedMap;
        private bool _isAddMode = false;

        // Dead by Daylight color palette
        private static class DbdColors
        {
            public static readonly Color Background = Color.FromArgb(20, 20, 25);
            public static readonly Color HeaderBackground = Color.FromArgb(15, 15, 20);
            public static readonly Color CardBackground = Color.FromArgb(30, 30, 38);
            public static readonly Color CardBackgroundAlt = Color.FromArgb(25, 25, 32);
            public static readonly Color CardSelected = Color.FromArgb(140, 20, 20);
            public static readonly Color CardHover = Color.FromArgb(40, 40, 50);
            public static readonly Color AccentRed = Color.FromArgb(180, 30, 30);
            public static readonly Color AccentRedDark = Color.FromArgb(140, 20, 20);
            public static readonly Color TextPrimary = Color.FromArgb(220, 220, 220);
            public static readonly Color TextSecondary = Color.FromArgb(140, 140, 140);
            public static readonly Color TextAccent = Color.FromArgb(140, 50, 50);
            public static readonly Color Border = Color.FromArgb(70, 70, 80);
        }

        public Maps()
        {
            InitializeComponent();
            _service = new MapService();

            // Enable double buffering for smoother rendering
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            ApplyLocalization();
            LoadMapsList();
            ShowEditPanel(false);
            ShowActionButtons(false);
        }

        private void ApplyLocalization()
        {
            this.Text = $"{Strings.AppName} - {Strings.Maps_Title}";
            lblTitle.Text = Strings.Maps_Title;
            btnAdd.Text = Strings.Button_Add;
            btnEdit.Text = Strings.Button_Edit;
            btnDelete.Text = Strings.Button_Delete;
            btnCancel.Text = Strings.Button_Cancel;
            lblName.Text = Strings.Label_Name;
        }

        private void LoadMapsList()
        {
            lstMaps.Items.Clear();
            foreach (var map in _service.GetAll())
            {
                lstMaps.Items.Add(map);
            }
            UpdateMapCount();
        }

        private void UpdateMapCount()
        {
            lblCount.Text = string.Format(Strings.Maps_Count, lstMaps.Items.Count);
        }

        private void ShowEditPanel(bool show)
        {
            pnlEdit.Visible = show;
            lstMaps.Visible = !show;
            btnAdd.Visible = !show;
            pnlActions.Visible = !show;
        }

        private void ShowActionButtons(bool show)
        {
            btnEdit.Visible = show;
            btnDelete.Visible = show;
        }

        private void ClearSelection()
        {
            _selectedMap = null;
            lstMaps.ClearSelected();
            ShowActionButtons(false);
        }

        private void lstMaps_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMaps.SelectedItem is Map map)
            {
                _selectedMap = map;
                ShowActionButtons(true);
            }
            else
            {
                _selectedMap = null;
                ShowActionButtons(false);
            }
        }

        private void lstMaps_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            Map map = (Map)lstMaps.Items[e.Index];
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle bounds = e.Bounds;
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            // Background
            Color backColor;
            if (isSelected)
            {
                backColor = DbdColors.CardSelected;
            }
            else
            {
                backColor = e.Index % 2 == 0 ? DbdColors.CardBackground : DbdColors.CardBackgroundAlt;
            }

            using (SolidBrush brush = new SolidBrush(backColor))
            {
                g.FillRectangle(brush, bounds);
            }

            // Left accent bar for selected item
            if (isSelected)
            {
                Rectangle accentRect = new Rectangle(bounds.X, bounds.Y, 4, bounds.Height);
                using (SolidBrush accentBrush = new SolidBrush(DbdColors.AccentRed))
                {
                    g.FillRectangle(accentBrush, accentRect);
                }
            }

            // Map icon (location/map emoji)
            string icon = "🗺️";
            Font iconFont = new Font("Segoe UI Emoji", 16F);
            SizeF iconSize = g.MeasureString(icon, iconFont);
            float iconX = bounds.X + 20;
            float iconY = bounds.Y + (bounds.Height - iconSize.Height) / 2;

            using (SolidBrush iconBrush = new SolidBrush(isSelected ? Color.White : DbdColors.AccentRed))
            {
                g.DrawString(icon, iconFont, iconBrush, iconX, iconY);
            }

            // Map name
            string text = map.Name;
            Font textFont = new Font("Segoe UI", 12F, FontStyle.Regular);
            Color textColor = isSelected ? Color.White : DbdColors.TextPrimary;

            float textX = iconX + iconSize.Width + 15;
            float textY = bounds.Y + (bounds.Height - textFont.GetHeight()) / 2;

            using (SolidBrush textBrush = new SolidBrush(textColor))
            {
                g.DrawString(text, textFont, textBrush, textX, textY);
            }

            // Bottom separator line
            using (Pen pen = new Pen(Color.FromArgb(40, 40, 50), 1))
            {
                g.DrawLine(pen, bounds.Left + 20, bounds.Bottom - 1, bounds.Right - 20, bounds.Bottom - 1);
            }

            // Dispose fonts
            iconFont.Dispose();
            textFont.Dispose();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _isAddMode = true;
            txtName.Text = "";
            lblPanelTitle.Text = Strings.Maps_PanelNew;
            btnSave.Text = Strings.Button_Add.Replace("➕ ", "");
            ClearSelection();
            ShowEditPanel(true);
            txtName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedMap == null) return;

            _isAddMode = false;
            txtName.Text = _selectedMap.Name;
            lblPanelTitle.Text = Strings.Maps_PanelEdit;
            btnSave.Text = Strings.Button_Save;
            ShowEditPanel(true);
            txtName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedMap == null) return;

            var result = MessageBox.Show(
                string.Format(Strings.Maps_ConfirmDelete, _selectedMap.Name),
                Strings.Dialog_ConfirmDelete,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _service.Delete(_selectedMap.Id);
                LoadMapsList();
                ClearSelection();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(
                    Strings.Maps_ValidationName,
                    Strings.Dialog_Warning,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (_isAddMode)
            {
                var map = new Map
                {
                    Name = txtName.Text.Trim()
                };

                var validationResult = _service.Add(map);
                if (!validationResult.IsValid)
                {
                    MessageBox.Show(
                        validationResult.GetErrorsAsString(),
                        Strings.Dialog_Warning,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                _selectedMap.Name = txtName.Text.Trim();

                var validationResult = _service.Update(_selectedMap);
                if (!validationResult.IsValid)
                {
                    MessageBox.Show(
                        validationResult.GetErrorsAsString(),
                        Strings.Dialog_Warning,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
            }

            LoadMapsList();
            ShowEditPanel(false);
            ClearSelection();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ShowEditPanel(false);
            ClearSelection();
        }
    }
}