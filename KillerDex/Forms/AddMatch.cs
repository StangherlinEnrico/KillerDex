using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using KillerDex.Core.Models;
using KillerDex.Infrastructure.Services;
using KillerDex.Resources;

namespace KillerDex
{
    public partial class AddMatch : Form
    {
        private readonly AllyService _allyService;
        private readonly MapService _mapService;
        private readonly KillerService _killerService;
        private readonly MatchService _matchService;

        // Custom selector buttons
        private List<DbdSelectorButton> _generatorButtons;
        private List<DbdSelectorButton> _survivorButtons;
        private int _selectedGenerators = 0;
        private int _selectedSurvivors = 0;

        // Dead by Daylight color palette
        private static class DbdColors
        {
            public static readonly Color Background = Color.FromArgb(20, 20, 25);
            public static readonly Color HeaderBackground = Color.FromArgb(15, 15, 20);
            public static readonly Color CardBackground = Color.FromArgb(30, 30, 38);
            public static readonly Color CardBackgroundAlt = Color.FromArgb(25, 25, 32);
            public static readonly Color InputBackground = Color.FromArgb(35, 35, 40);
            public static readonly Color AccentRed = Color.FromArgb(180, 30, 30);
            public static readonly Color AccentRedDark = Color.FromArgb(140, 20, 20);
            public static readonly Color AccentGreen = Color.FromArgb(40, 160, 80);
            public static readonly Color AccentYellow = Color.FromArgb(200, 160, 40);
            public static readonly Color TextPrimary = Color.FromArgb(220, 220, 220);
            public static readonly Color TextSecondary = Color.FromArgb(140, 140, 140);
            public static readonly Color TextAccent = Color.FromArgb(140, 50, 50);
            public static readonly Color Border = Color.FromArgb(70, 70, 80);
            public static readonly Color ButtonDefault = Color.FromArgb(45, 45, 55);
            public static readonly Color ButtonHover = Color.FromArgb(55, 55, 65);
            public static readonly Color ButtonSelected = Color.FromArgb(140, 20, 20);
        }

        public AddMatch()
        {
            InitializeComponent();

            _allyService = new AllyService();
            _mapService = new MapService();
            _killerService = new KillerService();
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
            lblTitle.Text = Strings.Match_Title;
            lblSubtitle.Text = GetLocalizedSubtitle();
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
            // Could add to resources, for now hardcoded based on language
            return LanguageService.IsItalian
                ? "Registra la tua partita Dead by Daylight"
                : "Record your Dead by Daylight match";
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
            var maps = _mapService.GetAll();
            cmbMap.DataSource = maps;
            cmbMap.DisplayMember = "Name";
            cmbMap.SelectedIndex = maps.Count > 0 ? -1 : -1;

            // Load killers
            cmbKiller.DataSource = null;
            var killers = _killerService.GetAll();
            cmbKiller.DataSource = killers;
            cmbKiller.DisplayMember = "Alias";
            cmbKiller.SelectedIndex = killers.Count > 0 ? -1 : -1;

            // Set current date
            dtpDate.Value = DateTime.Now;
            dtpDate.Format = DateTimePickerFormat.Custom;
            dtpDate.CustomFormat = "dd/MM/yyyy";

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
                MapId = ((Map)cmbMap.SelectedItem).Id,
                KillerId = ((Killer)cmbKiller.SelectedItem).Id,
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

    /// <summary>
    /// Custom selector button with Dead by Daylight styling
    /// </summary>
    public class DbdSelectorButton : Control
    {
        private bool _isSelected;
        private bool _isHovered;

        public int Value { get; set; }
        public string Icon { get; set; } = "";
        public bool UseGreenWhenSelected { get; set; } = false;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                Invalidate();
            }
        }

        // Colors
        private static readonly Color ColorDefault = Color.FromArgb(45, 45, 55);
        private static readonly Color ColorHover = Color.FromArgb(60, 60, 70);
        private static readonly Color ColorSelectedRed = Color.FromArgb(140, 20, 20);
        private static readonly Color ColorSelectedGreen = Color.FromArgb(30, 130, 60);
        private static readonly Color ColorBorder = Color.FromArgb(80, 80, 90);
        private static readonly Color ColorBorderSelected = Color.FromArgb(180, 30, 30);
        private static readonly Color ColorBorderSelectedGreen = Color.FromArgb(50, 180, 80);
        private static readonly Color ColorText = Color.FromArgb(220, 220, 220);

        public DbdSelectorButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            Cursor = Cursors.Hand;
            Size = new Size(50, 40);
        }

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

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

            // Determine colors
            Color backColor;
            Color borderColor;

            if (_isSelected)
            {
                backColor = UseGreenWhenSelected ? ColorSelectedGreen : ColorSelectedRed;
                borderColor = UseGreenWhenSelected ? ColorBorderSelectedGreen : ColorBorderSelected;
            }
            else if (_isHovered)
            {
                backColor = ColorHover;
                borderColor = ColorBorder;
            }
            else
            {
                backColor = ColorDefault;
                borderColor = ColorBorder;
            }

            // Draw background with rounded corners
            using (GraphicsPath path = CreateRoundedRectangle(rect, 6))
            {
                using (SolidBrush brush = new SolidBrush(backColor))
                {
                    g.FillPath(brush, path);
                }

                using (Pen pen = new Pen(borderColor, 1))
                {
                    g.DrawPath(pen, path);
                }
            }

            // Draw icon and text
            string displayText = string.IsNullOrEmpty(Icon) ? Text : $"{Icon} {Text}";

            using (Font font = new Font("Segoe UI", 10F, _isSelected ? FontStyle.Bold : FontStyle.Regular))
            using (SolidBrush textBrush = new SolidBrush(ColorText))
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                g.DrawString(displayText, font, textBrush, rect, sf);
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
    }
}