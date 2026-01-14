using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using KillerDex.Core.Extensions;
using KillerDex.Core.Models;
using KillerDex.Infrastructure.Services;
using KillerDex.Resources;
using KillerDex.Theme;

namespace KillerDex
{
    public partial class MainForm : Form
    {
        private readonly MatchService _matchService;
        private readonly AllyService _allyService;

        public MainForm()
        {
            InitializeComponent();

            _matchService = new MatchService();
            _allyService = new AllyService();

            // Enable double buffering
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            ApplyLocalization();
            InitializeStatCards();
            LoadDashboard();
        }

        private void ApplyLocalization()
        {
            this.Text = $"{Strings.AppName} - Dead by Daylight Tracker";
            formHeader.Title = Strings.AppName;
            formHeader.Subtitle = GetLocalizedSubtitle();

            // Menu
            fileMenu.Text = Strings.Menu_File;
            exitMenuItem.Text = Strings.Menu_Exit;
            manageMenu.Text = Strings.Menu_Manage;
            alliesMenuItem.Text = $"👤 {Strings.Menu_Allies}";
            languageMenu.Text = Strings.Menu_Language;
            italianMenuItem.Text = Strings.Menu_Italian;
            englishMenuItem.Text = Strings.Menu_English;

            // Content
            btnAddMatch.Text = Strings.Home_AddMatch;
        }

        private string GetLocalizedSubtitle()
        {
            return LanguageService.IsItalian
                ? "Traccia le tue partite Dead by Daylight"
                : "Dead by Daylight Match Tracker";
        }

        private void InitializeStatCards()
        {
            // Create stat cards - first row
            CreateStatCard(pnlStatTotal, "🎮", Strings.Home_TotalMatches.TrimEnd(':'), "0", DbdColors.AccentBlue);
            CreateStatCard(pnlStatWins, "✓", Strings.Home_Wins.TrimEnd(':'), "0", DbdColors.WinColor);
            CreateStatCard(pnlStatLosses, "✗", Strings.Home_Losses.TrimEnd(':'), "0", DbdColors.LossColor);
            CreateStatCard(pnlStatWinRate, "📊", Strings.Home_WinRate.TrimEnd(':'), "0%", DbdColors.AccentYellow);

            // Create stat cards - second row
            CreateStatCard(pnlStatBestAlly, "👤", Strings.Home_BestAlly.TrimEnd(':'), Strings.Home_NotAvailable, DbdColors.AccentPurple);
            CreateStatCard(pnlStatMostFacedKiller, "💀", Strings.Home_MostFacedKiller.TrimEnd(':'), Strings.Home_NotAvailable, DbdColors.AccentRed);
        }

        private void CreateStatCard(Panel panel, string icon, string title, string value, Color accentColor)
        {
            panel.Controls.Clear();
            panel.Tag = accentColor;

            // Paint event for rounded corners and accent
            panel.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Draw rounded rectangle background
                using (GraphicsPath path = CreateRoundedRectangle(new Rectangle(0, 0, panel.Width - 1, panel.Height - 1), 8))
                using (SolidBrush brush = new SolidBrush(DbdColors.CardBackground))
                {
                    g.FillPath(brush, path);
                }

                // Draw left accent bar
                using (SolidBrush accentBrush = new SolidBrush((Color)panel.Tag))
                {
                    g.FillRectangle(accentBrush, 0, 10, 4, panel.Height - 20);
                }
            };

            // Icon label
            Label lblIcon = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI Emoji", 14F),
                ForeColor = accentColor,
                AutoSize = true,
                Location = new Point(10, 14),
                BackColor = Color.Transparent
            };

            // Title label - positioned after measuring icon width
            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 9F),
                ForeColor = DbdColors.TextSecondary,
                AutoSize = true,
                Location = new Point(32, 15),
                BackColor = Color.Transparent
            };

            // Value label
            Label lblValue = new Label
            {
                Name = "lblValue",
                Text = value,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = DbdColors.TextPrimary,
                AutoSize = true,
                Location = new Point(15, 40),
                BackColor = Color.Transparent
            };

            // Add in reverse order so title is on top of icon if they overlap
            panel.Controls.Add(lblValue);
            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblIcon);
        }

        private void UpdateStatCard(Panel panel, string value)
        {
            var lblValue = panel.Controls.Find("lblValue", false).FirstOrDefault() as Label;
            if (lblValue != null)
            {
                lblValue.Text = value;
            }
        }

        private void LoadDashboard()
        {
            // Load statistics using the centralized dashboard stats
            var stats = _matchService.GetDashboardStats(_allyService);

            UpdateStatCard(pnlStatTotal, stats.TotalMatches.ToString());
            UpdateStatCard(pnlStatWins, stats.Wins.ToString());
            UpdateStatCard(pnlStatLosses, stats.Losses.ToString());
            UpdateStatCard(pnlStatWinRate, $"{stats.WinRate:F1}%");

            // Best Ally - show name with win rate
            string bestAllyValue = Strings.Home_NotAvailable;
            if (stats.BestAlly != null)
            {
                bestAllyValue = $"{stats.BestAlly.Name} ({stats.BestAlly.WinRate:F0}%)";
            }
            UpdateStatCard(pnlStatBestAlly, bestAllyValue);

            // Most Faced Killer - show killer name with count
            string mostFacedKillerValue = Strings.Home_NotAvailable;
            if (stats.MostFacedKiller != null)
            {
                string killerName = stats.MostFacedKiller.Killer.GetDisplayName();
                mostFacedKillerValue = $"{killerName} (x{stats.MostFacedKiller.TimesFaced})";
            }
            UpdateStatCard(pnlStatMostFacedKiller, mostFacedKillerValue);
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

        private void btnAddMatch_Click(object sender, EventArgs e)
        {
            using (AddMatch form = new AddMatch())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadDashboard();
                }
            }
        }

        private void alliesMenuItem_Click(object sender, EventArgs e)
        {
            using (Allies form = new Allies())
            {
                form.ShowDialog();
            }
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void italianMenuItem_Click(object sender, EventArgs e)
        {
            LanguageService.SetLanguage("it");
            MessageBox.Show(
                Strings.Language_RestartRequired,
                Strings.Language_Title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void englishMenuItem_Click(object sender, EventArgs e)
        {
            LanguageService.SetLanguage("en");
            MessageBox.Show(
                Strings.Language_RestartRequired,
                Strings.Language_Title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}