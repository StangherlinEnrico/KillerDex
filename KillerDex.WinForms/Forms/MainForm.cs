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

        public MainForm()
        {
            InitializeComponent();

            _matchService = new MatchService();

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
            lblRecentTitle.Text = Strings.Home_RecentMatches;
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
            // Create stat cards
            CreateStatCard(pnlStatTotal, "🎮", Strings.Home_TotalMatches.TrimEnd(':'), "0", DbdColors.AccentBlue);
            CreateStatCard(pnlStatWins, "✓", Strings.Home_Wins.TrimEnd(':'), "0", DbdColors.WinColor);
            CreateStatCard(pnlStatLosses, "✗", Strings.Home_Losses.TrimEnd(':'), "0", DbdColors.LossColor);
            CreateStatCard(pnlStatWinRate, "📊", Strings.Home_WinRate.TrimEnd(':'), "0%", DbdColors.AccentYellow);
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
                Font = new Font("Segoe UI Emoji", 16F),
                ForeColor = accentColor,
                AutoSize = true,
                Location = new Point(15, 12),
                BackColor = Color.Transparent
            };

            // Title label
            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 9F),
                ForeColor = DbdColors.TextSecondary,
                AutoSize = true,
                Location = new Point(45, 15),
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

            panel.Controls.Add(lblIcon);
            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblValue);
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
            // Load statistics
            int total = _matchService.GetTotalCount();
            int wins = _matchService.GetWinsCount();
            int losses = _matchService.GetLossesCount();
            double winRate = total > 0 ? (double)wins / total * 100 : 0;

            UpdateStatCard(pnlStatTotal, total.ToString());
            UpdateStatCard(pnlStatWins, wins.ToString());
            UpdateStatCard(pnlStatLosses, losses.ToString());
            UpdateStatCard(pnlStatWinRate, $"{winRate:F1}%");

            // Load recent matches
            LoadRecentMatches();
        }

        private void LoadRecentMatches()
        {
            pnlMatchesList.Controls.Clear();
            var recentMatches = _matchService.GetRecent(5);

            if (recentMatches.Count == 0)
            {
                // Show empty state
                Label lblEmpty = new Label
                {
                    Text = Strings.Home_NoMatches,
                    Font = new Font("Segoe UI", 12F),
                    ForeColor = DbdColors.TextSecondary,
                    AutoSize = false,
                    Size = new Size(pnlMatchesList.Width, 50),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, 80)
                };
                pnlMatchesList.Controls.Add(lblEmpty);
                return;
            }

            int yPos = 5;
            foreach (var match in recentMatches)
            {
                var matchCard = CreateMatchCard(match, yPos);
                pnlMatchesList.Controls.Add(matchCard);
                yPos += 45;
            }
        }

        private Panel CreateMatchCard(Match match, int yPosition)
        {
            string killerName = match.Killer.GetDisplayName();
            string mapName = match.Map.GetDisplayName();
            bool isWin = match.IsWin;
            int survivorCount = match.SurvivorsCount;

            Panel card = new Panel
            {
                Size = new Size(pnlMatchesList.Width - 25, 40),
                Location = new Point(5, yPosition),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };

            // Paint event for custom drawing
            card.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(0, 0, card.Width - 1, card.Height - 1);

                // Background
                using (GraphicsPath path = CreateRoundedRectangle(rect, 6))
                using (SolidBrush brush = new SolidBrush(DbdColors.CardBackground))
                {
                    g.FillPath(brush, path);
                }

                // Left accent bar (win/loss color)
                Color accentColor = isWin ? DbdColors.WinColor : DbdColors.LossColor;
                using (SolidBrush accentBrush = new SolidBrush(accentColor))
                {
                    g.FillRectangle(accentBrush, 0, 5, 4, card.Height - 10);
                }

                // Date
                using (Font dateFont = new Font("Segoe UI", 9F))
                using (SolidBrush dateBrush = new SolidBrush(DbdColors.TextSecondary))
                {
                    g.DrawString(match.Date.ToString("dd/MM/yyyy"), dateFont, dateBrush, 15, 12);
                }

                // Killer with icon
                using (Font killerFont = new Font("Segoe UI", 10F, FontStyle.Bold))
                using (SolidBrush killerBrush = new SolidBrush(DbdColors.TextPrimary))
                {
                    g.DrawString($"💀 {killerName}", killerFont, killerBrush, 110, 10);
                }

                // Map with icon
                using (Font mapFont = new Font("Segoe UI", 9F))
                using (SolidBrush mapBrush = new SolidBrush(DbdColors.TextSecondary))
                {
                    g.DrawString($"🗺️ {mapName}", mapFont, mapBrush, 300, 12);
                }

                // Result
                string resultText = isWin
                    ? $"✓ {survivorCount} {Strings.Home_Survivors}"
                    : $"✗ {Strings.Home_Defeat}";
                Color resultColor = isWin ? DbdColors.WinColor : DbdColors.LossColor;

                using (Font resultFont = new Font("Segoe UI", 9F, FontStyle.Bold))
                using (SolidBrush resultBrush = new SolidBrush(resultColor))
                {
                    SizeF resultSize = g.MeasureString(resultText, resultFont);
                    g.DrawString(resultText, resultFont, resultBrush, card.Width - resultSize.Width - 15, 12);
                }
            };

            // Hover effects
            card.MouseEnter += (s, e) =>
            {
                card.BackColor = DbdColors.CardBackgroundAlt;
                card.Invalidate();
            };

            card.MouseLeave += (s, e) =>
            {
                card.BackColor = Color.Transparent;
                card.Invalidate();
            };

            return card;
        }

        private string GetLocalizedUnknownMap()
        {
            return LanguageService.IsItalian ? "Sconosciuta" : "Unknown";
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