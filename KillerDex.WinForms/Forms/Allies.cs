using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using KillerDex.Core.Models;
using KillerDex.Infrastructure.Services;
using KillerDex.Resources;
using KillerDex.Theme;

namespace KillerDex
{
    public partial class Allies : Form
    {
        private readonly AllyService _service;
        private Ally _selectedAlly;
        private bool _isAddMode = false;

        public Allies()
        {
            InitializeComponent();
            _service = new AllyService();

            // Enable double buffering for smoother rendering
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            ApplyLocalization();
            LoadAlliesList();
            ShowEditPanel(false);
            ShowActionButtons(false);
        }

        private void ApplyLocalization()
        {
            this.Text = $"{Strings.AppName} - {Strings.Allies_Title}";
            formHeader.Title = Strings.Allies_Title;
            btnAdd.Text = Strings.Button_Add;
            btnEdit.Text = Strings.Button_Edit;
            btnDelete.Text = Strings.Button_Delete;
            btnCancel.Text = Strings.Button_Cancel;
            lblName.Text = Strings.Label_Name;
        }

        private void LoadAlliesList()
        {
            lstAllies.Items.Clear();
            foreach (var ally in _service.GetAll())
            {
                lstAllies.Items.Add(ally);
            }
            UpdateAllyCount();
        }

        private void UpdateAllyCount()
        {
            formHeader.Subtitle = string.Format(Strings.Allies_Count, lstAllies.Items.Count);
        }

        private void ShowEditPanel(bool show)
        {
            pnlEdit.Visible = show;
            lstAllies.Visible = !show;
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
            _selectedAlly = null;
            lstAllies.ClearSelected();
            ShowActionButtons(false);
        }

        private void lstAllies_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstAllies.SelectedItem is Ally ally)
            {
                _selectedAlly = ally;
                ShowActionButtons(true);
            }
            else
            {
                _selectedAlly = null;
                ShowActionButtons(false);
            }
        }

        private void lstAllies_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            Ally ally = (Ally)lstAllies.Items[e.Index];
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

            // Ally icon (survivor/person emoji)
            string icon = "👤";
            Font iconFont = new Font("Segoe UI Emoji", 16F);
            SizeF iconSize = g.MeasureString(icon, iconFont);
            float iconX = bounds.X + 20;
            float iconY = bounds.Y + (bounds.Height - iconSize.Height) / 2;

            using (SolidBrush iconBrush = new SolidBrush(isSelected ? Color.White : DbdColors.AccentRed))
            {
                g.DrawString(icon, iconFont, iconBrush, iconX, iconY);
            }

            // Ally name
            string text = ally.Name;
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
            lblPanelTitle.Text = Strings.Allies_PanelNew;
            btnSave.Text = Strings.Button_Add.Replace("➕ ", "");
            ClearSelection();
            ShowEditPanel(true);
            txtName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedAlly == null) return;

            _isAddMode = false;
            txtName.Text = _selectedAlly.Name;
            lblPanelTitle.Text = Strings.Allies_PanelEdit;
            btnSave.Text = Strings.Button_Save;
            ShowEditPanel(true);
            txtName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedAlly == null) return;

            var result = MessageBox.Show(
                string.Format(Strings.Allies_ConfirmDelete, _selectedAlly.Name),
                Strings.Dialog_ConfirmDelete,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _service.Delete(_selectedAlly.Id);
                LoadAlliesList();
                ClearSelection();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(
                    Strings.Allies_ValidationName,
                    Strings.Dialog_Warning,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (_isAddMode)
            {
                var ally = new Ally
                {
                    Name = txtName.Text.Trim()
                };

                var validationResult = _service.Add(ally);
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
                _selectedAlly.Name = txtName.Text.Trim();

                var validationResult = _service.Update(_selectedAlly);
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

            LoadAlliesList();
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