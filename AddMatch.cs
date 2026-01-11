using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using KillerDex.Models;
using KillerDex.Services;

namespace KillerDex
{
    public partial class AddMatch : Form
    {
        private AllyService _allyService;
        private MapService _mapService;
        private KillerService _killerService;
        private MatchService _matchService;

        public AddMatch()
        {
            InitializeComponent();
            _allyService = new AllyService();
            _mapService = new MapService();
            _killerService = new KillerService();
            _matchService = new MatchService();
            LoadData();
        }

        private void LoadData()
        {
            // Carica Alleati
            lstAllies.Items.Clear();
            foreach (var ally in _allyService.GetAll())
            {
                lstAllies.Items.Add(ally);
            }

            // Carica Mappe
            cmbMap.DataSource = null;
            cmbMap.DataSource = _mapService.GetAll();
            cmbMap.DisplayMember = "Name";
            cmbMap.SelectedIndex = -1;

            // Carica Killer
            cmbKiller.DataSource = null;
            cmbKiller.DataSource = _killerService.GetAll();
            cmbKiller.DisplayMember = "Alias";
            cmbKiller.SelectedIndex = -1;

            // Imposta data odierna
            dtpDate.Value = DateTime.Now;
            dtpDate.Format = DateTimePickerFormat.Custom;
            dtpDate.CustomFormat = "dd/MM/yyyy";

            // Generatori (0-5)
            cmbGenerators.Items.Clear();
            for (int i = 0; i <= 5; i++)
            {
                cmbGenerators.Items.Add(i);
            }
            cmbGenerators.SelectedIndex = 0;

            // Sopravvissuti (0-4)
            cmbSurvivors.Items.Clear();
            for (int i = 0; i <= 4; i++)
            {
                cmbSurvivors.Items.Add(i);
            }
            cmbSurvivors.SelectedIndex = 0;

            // Primo gancio default
            UpdateFirstHookOptions();
        }

        private void lstAllies_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Limita selezione a 3
            if (lstAllies.SelectedItems.Count > 3)
            {
                MessageBox.Show("Puoi selezionare al massimo 3 alleati!", "Attenzione",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Deseleziona l'ultimo
                lstAllies.SelectedIndexChanged -= lstAllies_SelectedIndexChanged;
                lstAllies.SetSelected(lstAllies.SelectedIndices[lstAllies.SelectedIndices.Count - 1], false);
                lstAllies.SelectedIndexChanged += lstAllies_SelectedIndexChanged;
            }

            UpdateFirstHookOptions();
        }

        private void UpdateFirstHookOptions()
        {
            cmbFirstHook.Items.Clear();
            cmbFirstHook.Items.Add("Me stesso");

            var selectedAllies = lstAllies.SelectedItems.Cast<Ally>().ToList();

            foreach (var ally in selectedAllies)
            {
                cmbFirstHook.Items.Add(ally.Name);
            }

            // Se meno di 3 alleati, aggiungi Filler
            int fillerCount = 3 - selectedAllies.Count;
            for (int i = 0; i < fillerCount; i++)
            {
                cmbFirstHook.Items.Add("Filler");
            }

            cmbFirstHook.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validazione
            if (cmbMap.SelectedItem == null)
            {
                MessageBox.Show("Seleziona una mappa!", "Attenzione",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbKiller.SelectedItem == null)
            {
                MessageBox.Show("Seleziona un killer!", "Attenzione",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Crea la partita
            var match = new Match
            {
                Date = dtpDate.Value,
                MapId = ((Map)cmbMap.SelectedItem).Id,
                KillerId = ((Killer)cmbKiller.SelectedItem).Id,
                FirstHook = cmbFirstHook.SelectedItem.ToString(),
                GeneratorsCompleted = (int)cmbGenerators.SelectedItem,
                Survivors = (int)cmbSurvivors.SelectedItem,
                Notes = txtNotes.Text.Trim(),
                AllyIds = lstAllies.SelectedItems.Cast<Ally>().Select(a => a.Id).ToList()
            };

            _matchService.Add(match);

            MessageBox.Show("Partita salvata!", "Successo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}