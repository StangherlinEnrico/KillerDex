using System;
using System.Drawing;
using System.Windows.Forms;
using KillerDex.Models;
using KillerDex.Services;

namespace KillerDex
{
    public partial class Maps : Form
    {
        private MapService _service;
        private Map _selectedMap;
        private bool _isAddMode = false;

        public Maps()
        {
            InitializeComponent();
            _service = new MapService();
            LoadMapsList();
            ShowEditPanel(false);
            ShowActionButtons(false);
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
            lblCount.Text = $"{lstMaps.Items.Count} mappe";
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

            string text = map.Name;
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
            txtName.Text = "";
            lblPanelTitle.Text = "Nuova Mappa";
            btnSave.Text = "Aggiungi";
            ClearSelection();
            ShowEditPanel(true);
            txtName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedMap == null) return;

            _isAddMode = false;
            txtName.Text = _selectedMap.Name;
            lblPanelTitle.Text = "Modifica Mappa";
            btnSave.Text = "Salva";
            ShowEditPanel(true);
            txtName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedMap == null) return;

            var result = MessageBox.Show(
                $"Vuoi eliminare {_selectedMap.Name}?",
                "Conferma eliminazione",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

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
                MessageBox.Show("Inserisci il nome della mappa!", "Attenzione",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_isAddMode)
            {
                var map = new Map
                {
                    Name = txtName.Text.Trim()
                };
                _service.Add(map);
            }
            else
            {
                _selectedMap.Name = txtName.Text.Trim();
                _service.Update(_selectedMap);
            }

            LoadMapsList();
            ShowEditPanel(false);
            ClearSelection();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ShowEditPanel(false);
        }
    }
}