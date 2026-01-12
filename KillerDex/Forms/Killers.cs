using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using KillerDex.Core.Models;
using KillerDex.Infrastructure.Services;
using KillerDex.Resources;

namespace KillerDex
{
    public partial class Killers : Form
    {
        private readonly KillerService _service;
        private Killer _selectedKiller;
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

        public Killers()
        {
            InitializeComponent();
            _service = new KillerService();

            // Enable double buffering for smoother rendering
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            ApplyLocalization();
            LoadKillersList();
            ShowEditPanel(false);
            ShowActionButtons(false);
        }

        private void ApplyLocalization()
        {
            this.Text = $"{Strings.AppName} - {Strings.Killers_Title}";
            lblTitle.Text = Strings.Killers_Title;
            btnAdd.Text = Strings.Button_Add;
            btnEdit.Text = Strings.Button_Edit;
            btnDelete.Text = Strings.Button_Delete;
            btnCancel.Text = Strings.Button_Cancel;
            lblAlias.Text = Strings.Label_Alias;
        }

        private void LoadKillersList()
        {
            lstKillers.Items.Clear();
            foreach (var killer in _service.GetAll())
            {
                lstKillers.Items.Add(killer);
            }
            UpdateKillerCount();
        }

        private void UpdateKillerCount()
        {
            lblCount.Text = string.Format(Strings.Killers_Count, lstKillers.Items.Count);
        }

        private void ShowEditPanel(bool show)
        {
            pnlEdit.Visible = show;
            lstKillers.Visible = !show;
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
            _selectedKiller = null;
            lstKillers.ClearSelected();
            ShowActionButtons(false);
        }

        private void lstKillers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstKillers.SelectedItem is Killer killer)
            {
                _selectedKiller = killer;
                ShowActionButtons(true);
            }
            else
            {
                _selectedKiller = null;
                ShowActionButtons(false);
            }
        }

        private void lstKillers_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            Killer killer = (Killer)lstKillers.Items[e.Index];
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

            // Killer icon placeholder (skull emoji or custom icon)
            string icon = "💀";
            Font iconFont = new Font("Segoe UI Emoji", 16F);
            SizeF iconSize = g.MeasureString(icon, iconFont);
            float iconX = bounds.X + 20;
            float iconY = bounds.Y + (bounds.Height - iconSize.Height) / 2;

            using (SolidBrush iconBrush = new SolidBrush(isSelected ? Color.White : DbdColors.AccentRed))
            {
                g.DrawString(icon, iconFont, iconBrush, iconX, iconY);
            }

            // Killer name
            string text = killer.Alias;
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
            txtAlias.Text = "";
            lblPanelTitle.Text = Strings.Killers_PanelNew;
            btnSave.Text = Strings.Button_Add.Replace("➕ ", "");
            ClearSelection();
            ShowEditPanel(true);
            txtAlias.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedKiller == null) return;

            _isAddMode = false;
            txtAlias.Text = _selectedKiller.Alias;
            lblPanelTitle.Text = Strings.Killers_PanelEdit;
            btnSave.Text = Strings.Button_Save;
            ShowEditPanel(true);
            txtAlias.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedKiller == null) return;

            var result = MessageBox.Show(
                string.Format(Strings.Killers_ConfirmDelete, _selectedKiller.Alias),
                Strings.Dialog_ConfirmDelete,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _service.Delete(_selectedKiller.Id);
                LoadKillersList();
                ClearSelection();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAlias.Text))
            {
                MessageBox.Show(
                    Strings.Killers_ValidationAlias,
                    Strings.Dialog_Warning,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtAlias.Focus();
                return;
            }

            if (_isAddMode)
            {
                var killer = new Killer
                {
                    Alias = txtAlias.Text.Trim()
                };

                var validationResult = _service.Add(killer);
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
                _selectedKiller.Alias = txtAlias.Text.Trim();

                var validationResult = _service.Update(_selectedKiller);
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

            LoadKillersList();
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