using System;
using System.Drawing;
using System.Windows.Forms;
using KillerDex.Models;
using KillerDex.Services;

namespace KillerDex
{
    public partial class Allies : Form
    {
        private AllyService _service;
        private Ally _selectedAlly;
        private bool _isAddMode = false;

        public Allies()
        {
            InitializeComponent();
            _service = new AllyService();
            LoadAlliesList();
            ShowEditPanel(false);
            ShowActionButtons(false);
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
            lblCount.Text = $"{lstAllies.Items.Count} alleati";
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

            string text = ally.Name;
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
            lblPanelTitle.Text = "Nuovo Alleato";
            btnSave.Text = "Aggiungi";
            ClearSelection();
            ShowEditPanel(true);
            txtName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedAlly == null) return;

            _isAddMode = false;
            txtName.Text = _selectedAlly.Name;
            lblPanelTitle.Text = "Modifica Alleato";
            btnSave.Text = "Salva";
            ShowEditPanel(true);
            txtName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedAlly == null) return;

            var result = MessageBox.Show(
                $"Vuoi eliminare {_selectedAlly.Name}?",
                "Conferma eliminazione",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

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
                MessageBox.Show("Inserisci il nome dell'alleato!", "Attenzione",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_isAddMode)
            {
                var ally = new Ally
                {
                    Name = txtName.Text.Trim()
                };
                _service.Add(ally);
            }
            else
            {
                _selectedAlly.Name = txtName.Text.Trim();
                _service.Update(_selectedAlly);
            }

            LoadAlliesList();
            ShowEditPanel(false);
            ClearSelection();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ShowEditPanel(false);
        }
    }
}