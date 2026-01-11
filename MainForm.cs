using KillerDex;
using KillerDex.Models;
using KillerDex.Services;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace KillerDex
{
    public partial class MainForm : Form
    {
        private MatchService _matchService;
        private KillerService _killerService;
        private MapService _mapService;

        public MainForm()
        {
            InitializeComponent();
            _matchService = new MatchService();
            _killerService = new KillerService();
            _mapService = new MapService();
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            // Statistiche
            int total = _matchService.GetTotalCount();
            int wins = _matchService.GetWinsCount();
            int losses = _matchService.GetLossesCount();
            double winRate = total > 0 ? (double)wins / total * 100 : 0;

            lblTotalMatches.Text = total.ToString();
            lblWins.Text = wins.ToString();
            lblLosses.Text = losses.ToString();
            lblWinRate.Text = $"{winRate:F1}%";

            // Ultime 5 partite
            lstRecentMatches.Items.Clear();
            var recentMatches = _matchService.GetRecent(5);

            foreach (var match in recentMatches)
            {
                var killer = _killerService.GetById(match.KillerId);
                var map = _mapService.GetById(match.MapId);

                string killerName = killer?.Alias ?? "Sconosciuto";
                string mapName = map?.Name ?? "Sconosciuta";
                string result = match.Survivors > 0 ? $"✓ {match.Survivors} sopravvissuti" : "✗ Sconfitta";

                string line = $"{match.Date:dd/MM/yyyy} | {killerName} | {mapName} | {result}";
                lstRecentMatches.Items.Add(line);
            }

            if (lstRecentMatches.Items.Count == 0)
            {
                lstRecentMatches.Items.Add("Nessuna partita registrata");
            }
        }

        private void btnAddMatch_Click(object sender, EventArgs e)
        {
            AddMatch form = new AddMatch();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadDashboard();
            }
        }

        private void killersMenuItem_Click(object sender, EventArgs e)
        {
            Killers form = new Killers();
            form.ShowDialog();
        }

        private void mapsMenuItem_Click(object sender, EventArgs e)
        {
            Maps form = new Maps();
            form.ShowDialog();
        }

        private void alliesMenuItem_Click(object sender, EventArgs e)
        {
            Allies form = new Allies();
            form.ShowDialog();
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void italianMenuItem_Click(object sender, EventArgs e)
        {
            LanguageService.SetLanguage("it");
            MessageBox.Show("Riavvia l'applicazione per applicare la lingua.",
                "Lingua", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void englishMenuItem_Click(object sender, EventArgs e)
        {
            LanguageService.SetLanguage("en");
            MessageBox.Show("Restart the application to apply the language.",
                "Language", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
