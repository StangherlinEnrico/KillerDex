using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using KillerDex.Core.Enums;
using KillerDex.Core.Extensions;
using KillerDex.Core.Models;
using KillerDex.Infrastructure.Services;
using KillerDex.Resources;
using KillerDex.Controls;
using KillerDex.Theme;

namespace KillerDex
{
    public partial class AddMatch : Form
    {
        private readonly AllyService _allyService;
        private readonly MatchService _matchService;

        // Custom selector buttons
        private List<DbdSelectorButton> _generatorButtons;
        private List<DbdSelectorButton> _survivorButtons;
        private int _selectedGenerators = 0;
        private int _selectedSurvivors = 0;

        public AddMatch()
        {
            InitializeComponent();

            _allyService = new AllyService();
            _matchService = new MatchService();

            // Enable double buffering
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            ApplyLocalization();
            InitializeCustomControls();
            LoadData();
        }

        private void ApplyLocalization()
        {
            this.Text = $"{Strings.AppName} - {Strings.Match_Title}";
            formHeader.Title = Strings.Match_Title;
            formHeader.Subtitle = GetLocalizedSubtitle();
            lblDate.Text = Strings.Match_Date.TrimEnd(':');
            lblMap.Text = Strings.Match_Map.TrimEnd(':');
            lblKiller.Text = Strings.Match_Killer.TrimEnd(':');
            lblAllies.Text = Strings.Match_Allies.TrimEnd(':');
            lblAlliesHint.Text = Strings.Match_AlliesHint;
            lblFirstHook.Text = Strings.Match_FirstHook.TrimEnd(':');
            lblGenerators.Text = Strings.Match_Generators.TrimEnd(':');
            lblSurvivors.Text = Strings.Match_Survivors.TrimEnd(':');
            lblNotes.Text = Strings.Match_Notes.TrimEnd(':');
            btnSave.Text = "💾 " + Strings.Match_Save;
            btnCancel.Text = Strings.Button_Cancel;
        }

        private string GetLocalizedSubtitle()
        {
            return Strings.Match_Subtitle;
        }

        private void InitializeCustomControls()
        {
            // Create generator selector buttons (0-5)
            _generatorButtons = new List<DbdSelectorButton>();
            for (int i = 0; i <= 5; i++)
            {
                var btn = new DbdSelectorButton
                {
                    Text = i.ToString(),
                    Value = i,
                    Size = new Size(45, 40),
                    Location = new Point(i * 50, 5),
                    IsSelected = i == 0,
                    Icon = "⚡"
                };
                btn.Click += GeneratorButton_Click;
                _generatorButtons.Add(btn);
                pnlGenerators.Controls.Add(btn);
            }

            // Create survivor selector buttons (0-4)
            _survivorButtons = new List<DbdSelectorButton>();
            for (int i = 0; i <= 4; i++)
            {
                var btn = new DbdSelectorButton
                {
                    Text = i.ToString(),
                    Value = i,
                    Size = new Size(55, 40),
                    Location = new Point(i * 60, 5),
                    IsSelected = i == 0,
                    Icon = "👤",
                    UseGreenWhenSelected = true
                };
                btn.Click += SurvivorButton_Click;
                _survivorButtons.Add(btn);
                pnlSurvivors.Controls.Add(btn);
            }
        }

        private void LoadData()
        {
            // Load allies
            chkAllies.Items.Clear();
            foreach (var ally in _allyService.GetAll())
            {
                chkAllies.Items.Add(ally);
            }

            // Load maps
            cmbMap.DataSource = null;
            var maps = Enum.GetValues(typeof(MapType))
                .Cast<MapType>()
                .Select(k => new { Value = k, Display = k.GetDisplayName() })
                .OrderBy(k => k.Display)
                .ToList();
            cmbMap.DataSource = maps;
            cmbMap.DisplayMember = "Display";
            cmbMap.ValueMember = "Value";
            cmbMap.SelectedIndex = -1;

            // Load killers from enum
            cmbKiller.DataSource = null;
            var killers = Enum.GetValues(typeof(KillerType))
                .Cast<KillerType>()
                .Select(k => new { Value = k, Display = k.GetDisplayName() })
                .OrderBy(k => k.Display)
                .ToList();
            cmbKiller.DataSource = killers;
            cmbKiller.DisplayMember = "Display";
            cmbKiller.ValueMember = "Value";
            cmbKiller.SelectedIndex = -1;

            // Set current date
            dtpDate.Value = DateTime.Now;

            // Initialize first hook options
            UpdateFirstHookOptions();
        }

        private void chkAllies_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Count currently checked + the one being checked/unchecked
            int checkedCount = chkAllies.CheckedItems.Count;

            if (e.NewValue == CheckState.Checked)
            {
                checkedCount++;
                if (checkedCount > 3)
                {
                    e.NewValue = CheckState.Unchecked;
                    ShowWarning(Strings.Match_AlliesMax);
                    return;
                }
            }

            // Update first hook options after a small delay to let the check complete
            BeginInvoke(new Action(UpdateFirstHookOptions));
        }

        private void UpdateFirstHookOptions()
        {
            cmbFirstHook.Items.Clear();
            cmbFirstHook.Items.Add(Strings.Match_Myself);

            var selectedAllies = chkAllies.CheckedItems.Cast<Ally>().ToList();
            foreach (var ally in selectedAllies)
            {
                cmbFirstHook.Items.Add(ally.Name);
            }

            // Add fillers for remaining slots
            int fillerCount = 3 - selectedAllies.Count;
            for (int i = 0; i < fillerCount; i++)
            {
                cmbFirstHook.Items.Add($"{Strings.Match_Filler} {i + 1}");
            }

            if (cmbFirstHook.Items.Count > 0)
                cmbFirstHook.SelectedIndex = 0;
        }

        private void GeneratorButton_Click(object sender, EventArgs e)
        {
            var clickedBtn = sender as DbdSelectorButton;
            if (clickedBtn == null) return;

            _selectedGenerators = clickedBtn.Value;

            foreach (var btn in _generatorButtons)
            {
                btn.IsSelected = btn.Value == _selectedGenerators;
                btn.Invalidate();
            }
        }

        private void SurvivorButton_Click(object sender, EventArgs e)
        {
            var clickedBtn = sender as DbdSelectorButton;
            if (clickedBtn == null) return;

            _selectedSurvivors = clickedBtn.Value;

            foreach (var btn in _survivorButtons)
            {
                btn.IsSelected = btn.Value == _selectedSurvivors;
                btn.Invalidate();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validation
            if (cmbMap.SelectedItem == null)
            {
                ShowWarning(Strings.Match_ValidationMap);
                cmbMap.Focus();
                return;
            }

            if (cmbKiller.SelectedItem == null)
            {
                ShowWarning(Strings.Match_ValidationKiller);
                cmbKiller.Focus();
                return;
            }

            // Create match
            var match = new Match
            {
                Date = dtpDate.Value,
                Map = (MapType)cmbMap.SelectedValue,
                Killer = (KillerType)cmbKiller.SelectedValue,
                FirstHook = cmbFirstHook.SelectedItem?.ToString() ?? Strings.Match_Myself,
                GeneratorsCompleted = _selectedGenerators,
                Notes = txtNotes.Text.Trim(),
                AllyIds = chkAllies.CheckedItems.Cast<Ally>().Select(a => a.Id).ToList()
            };

            // Set survivors as a list of names based on count
            match.Survivors = new List<string>();
            for (int i = 0; i < _selectedSurvivors; i++)
            {
                match.Survivors.Add($"Survivor {i + 1}");
            }

            var result = _matchService.Add(match);

            if (!result.IsValid)
            {
                ShowWarning(result.GetErrorsAsString());
                return;
            }

            ShowSuccess(Strings.Match_Saved);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ShowWarning(string message)
        {
            MessageBox.Show(message, Strings.Dialog_Warning,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ShowSuccess(string message)
        {
            MessageBox.Show(message, Strings.Dialog_Success,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}