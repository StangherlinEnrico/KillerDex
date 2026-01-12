using System;
using System.Drawing;
using System.Windows.Forms;
using KillerDex.Models;
using KillerDex.Services;
using KillerDex.Resources;

namespace KillerDex
{
    public partial class Killers : Form
    {
        private KillerService _service;
        private Killer _selectedKiller;
        private bool _isAddMode = false;

        public Killers()
        {
            InitializeComponent();
            _service = new KillerService();
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
            btnAdd.Visible = !show;
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

            Color backColor;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                backColor = Color.FromArgb(108, 92, 231);
            }
            else
            {
                backColor = e.Index % 2 == 0
                    ? Color.FromArgb(45, 45, 55)
                    : Color.FromArgb(55, 55, 65);
            }

            e.Graphics.FillRectangle(new SolidBrush(backColor), e.Bounds);

            string text = killer.Alias;
            Font font = new Font("Segoe UI", 11F, FontStyle.Regular);
            Color textColor = Color.FromArgb(240, 240, 240);

            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;

            Rectangle textBounds = new Rectangle(e.Bounds.X + 15, e.Bounds.Y, e.Bounds.Width - 15, e.Bounds.Height);
            e.Graphics.DrawString(text, font, new SolidBrush(textColor), textBounds, sf);

            using (Pen pen = new Pen(Color.FromArgb(70, 70, 80)))
            {
                e.Graphics.DrawLine(pen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
            }
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
                MessageBoxIcon.Question);

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
                MessageBox.Show(Strings.Killers_ValidationAlias, Strings.Dialog_Warning,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_isAddMode)
            {
                var killer = new Killer
                {
                    Alias = txtAlias.Text.Trim()
                };
                _service.Add(killer);
            }
            else
            {
                _selectedKiller.Alias = txtAlias.Text.Trim();
                _service.Update(_selectedKiller);
            }

            LoadKillersList();
            ShowEditPanel(false);
            ClearSelection();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ShowEditPanel(false);
        }
    }
}