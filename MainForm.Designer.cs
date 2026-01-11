using KillerDex;
using KillerDex.Models;
using KillerDex.Services;
using System.Reflection;

namespace KillerDex
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manageMenu;
        private System.Windows.Forms.ToolStripMenuItem languageMenu;
        private System.Windows.Forms.ToolStripMenuItem italianMenuItem;
        private System.Windows.Forms.ToolStripMenuItem englishMenuItem;
        private System.Windows.Forms.ToolStripMenuItem killersMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alliesMenuItem;
        private System.Windows.Forms.GroupBox grpStats;
        private System.Windows.Forms.Label lblTotalMatchesTitle;
        private System.Windows.Forms.Label lblTotalMatches;
        private System.Windows.Forms.Label lblWinsTitle;
        private System.Windows.Forms.Label lblWins;
        private System.Windows.Forms.Label lblLossesTitle;
        private System.Windows.Forms.Label lblLosses;
        private System.Windows.Forms.Label lblWinRateTitle;
        private System.Windows.Forms.Label lblWinRate;
        private System.Windows.Forms.GroupBox grpRecentMatches;
        private System.Windows.Forms.ListBox lstRecentMatches;
        private System.Windows.Forms.Button btnAddMatch;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.languageMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.italianMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.killersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alliesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grpStats = new System.Windows.Forms.GroupBox();
            this.lblTotalMatchesTitle = new System.Windows.Forms.Label();
            this.lblTotalMatches = new System.Windows.Forms.Label();
            this.lblWinsTitle = new System.Windows.Forms.Label();
            this.lblWins = new System.Windows.Forms.Label();
            this.lblLossesTitle = new System.Windows.Forms.Label();
            this.lblLosses = new System.Windows.Forms.Label();
            this.lblWinRateTitle = new System.Windows.Forms.Label();
            this.lblWinRate = new System.Windows.Forms.Label();
            this.grpRecentMatches = new System.Windows.Forms.GroupBox();
            this.lstRecentMatches = new System.Windows.Forms.ListBox();
            this.btnAddMatch = new System.Windows.Forms.Button();
            this.menuStrip.SuspendLayout();
            this.grpStats.SuspendLayout();
            this.grpRecentMatches.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.fileMenu,
                this.manageMenu,
                this.languageMenu
            });
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(584, 24);
            this.menuStrip.TabIndex = 0;
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.exitMenuItem
            });
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(37, 20);
            this.fileMenu.Text = "File";
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(100, 22);
            this.exitMenuItem.Text = "Esci";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // manageMenu
            // 
            this.manageMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.killersMenuItem,
                this.mapsMenuItem,
                this.alliesMenuItem
            });
            this.manageMenu.Name = "manageMenu";
            this.manageMenu.Size = new System.Drawing.Size(62, 20);
            this.manageMenu.Text = "Gestione";
            // 
            // languageMenu
            // 
            this.languageMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.italianMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            this.languageMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.italianMenuItem,
                this.englishMenuItem
            });
            this.languageMenu.Name = "languageMenu";
            this.languageMenu.Size = new System.Drawing.Size(55, 20);
            this.languageMenu.Text = "Lingua";
            // 
            // italianMenuItem
            // 
            this.italianMenuItem.Name = "italianMenuItem";
            this.italianMenuItem.Size = new System.Drawing.Size(120, 22);
            this.italianMenuItem.Text = "🇮🇹 Italiano";
            this.italianMenuItem.Click += new System.EventHandler(this.italianMenuItem_Click);
            // 
            // englishMenuItem
            // 
            this.englishMenuItem.Name = "englishMenuItem";
            this.englishMenuItem.Size = new System.Drawing.Size(120, 22);
            this.englishMenuItem.Text = "🇬🇧 English";
            this.englishMenuItem.Click += new System.EventHandler(this.englishMenuItem_Click);
            // 
            // killersMenuItem
            // 
            this.killersMenuItem.Name = "killersMenuItem";
            this.killersMenuItem.Size = new System.Drawing.Size(120, 22);
            this.killersMenuItem.Text = "Killer";
            this.killersMenuItem.Click += new System.EventHandler(this.killersMenuItem_Click);
            // 
            // mapsMenuItem
            // 
            this.mapsMenuItem.Name = "mapsMenuItem";
            this.mapsMenuItem.Size = new System.Drawing.Size(120, 22);
            this.mapsMenuItem.Text = "Mappe";
            this.mapsMenuItem.Click += new System.EventHandler(this.mapsMenuItem_Click);
            // 
            // alliesMenuItem
            // 
            this.alliesMenuItem.Name = "alliesMenuItem";
            this.alliesMenuItem.Size = new System.Drawing.Size(120, 22);
            this.alliesMenuItem.Text = "Alleati";
            this.alliesMenuItem.Click += new System.EventHandler(this.alliesMenuItem_Click);
            // 
            // grpStats
            // 
            this.grpStats.Controls.Add(this.lblTotalMatchesTitle);
            this.grpStats.Controls.Add(this.lblTotalMatches);
            this.grpStats.Controls.Add(this.lblWinsTitle);
            this.grpStats.Controls.Add(this.lblWins);
            this.grpStats.Controls.Add(this.lblLossesTitle);
            this.grpStats.Controls.Add(this.lblLosses);
            this.grpStats.Controls.Add(this.lblWinRateTitle);
            this.grpStats.Controls.Add(this.lblWinRate);
            this.grpStats.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grpStats.Location = new System.Drawing.Point(12, 35);
            this.grpStats.Name = "grpStats";
            this.grpStats.Size = new System.Drawing.Size(560, 80);
            this.grpStats.TabIndex = 1;
            this.grpStats.TabStop = false;
            this.grpStats.Text = "Statistiche Generali";
            // 
            // lblTotalMatchesTitle
            // 
            this.lblTotalMatchesTitle.Location = new System.Drawing.Point(15, 25);
            this.lblTotalMatchesTitle.Name = "lblTotalMatchesTitle";
            this.lblTotalMatchesTitle.Size = new System.Drawing.Size(120, 20);
            this.lblTotalMatchesTitle.Text = "Partite Totali:";
            // 
            // lblTotalMatches
            // 
            this.lblTotalMatches.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotalMatches.Location = new System.Drawing.Point(15, 45);
            this.lblTotalMatches.Name = "lblTotalMatches";
            this.lblTotalMatches.Size = new System.Drawing.Size(120, 25);
            this.lblTotalMatches.Text = "0";
            // 
            // lblWinsTitle
            // 
            this.lblWinsTitle.Location = new System.Drawing.Point(155, 25);
            this.lblWinsTitle.Name = "lblWinsTitle";
            this.lblWinsTitle.Size = new System.Drawing.Size(120, 20);
            this.lblWinsTitle.Text = "Vittorie:";
            // 
            // lblWins
            // 
            this.lblWins.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblWins.ForeColor = System.Drawing.Color.Green;
            this.lblWins.Location = new System.Drawing.Point(155, 45);
            this.lblWins.Name = "lblWins";
            this.lblWins.Size = new System.Drawing.Size(120, 25);
            this.lblWins.Text = "0";
            // 
            // lblLossesTitle
            // 
            this.lblLossesTitle.Location = new System.Drawing.Point(295, 25);
            this.lblLossesTitle.Name = "lblLossesTitle";
            this.lblLossesTitle.Size = new System.Drawing.Size(120, 20);
            this.lblLossesTitle.Text = "Sconfitte:";
            // 
            // lblLosses
            // 
            this.lblLosses.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblLosses.ForeColor = System.Drawing.Color.Red;
            this.lblLosses.Location = new System.Drawing.Point(295, 45);
            this.lblLosses.Name = "lblLosses";
            this.lblLosses.Size = new System.Drawing.Size(120, 25);
            this.lblLosses.Text = "0";
            // 
            // lblWinRateTitle
            // 
            this.lblWinRateTitle.Location = new System.Drawing.Point(435, 25);
            this.lblWinRateTitle.Name = "lblWinRateTitle";
            this.lblWinRateTitle.Size = new System.Drawing.Size(120, 20);
            this.lblWinRateTitle.Text = "Win Rate:";
            // 
            // lblWinRate
            // 
            this.lblWinRate.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblWinRate.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblWinRate.Location = new System.Drawing.Point(435, 45);
            this.lblWinRate.Name = "lblWinRate";
            this.lblWinRate.Size = new System.Drawing.Size(120, 25);
            this.lblWinRate.Text = "0%";
            // 
            // grpRecentMatches
            // 
            this.grpRecentMatches.Controls.Add(this.lstRecentMatches);
            this.grpRecentMatches.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grpRecentMatches.Location = new System.Drawing.Point(12, 125);
            this.grpRecentMatches.Name = "grpRecentMatches";
            this.grpRecentMatches.Size = new System.Drawing.Size(560, 170);
            this.grpRecentMatches.TabIndex = 2;
            this.grpRecentMatches.TabStop = false;
            this.grpRecentMatches.Text = "Ultime 5 Partite";
            // 
            // lstRecentMatches
            // 
            this.lstRecentMatches.FormattingEnabled = true;
            this.lstRecentMatches.ItemHeight = 15;
            this.lstRecentMatches.Location = new System.Drawing.Point(10, 22);
            this.lstRecentMatches.Name = "lstRecentMatches";
            this.lstRecentMatches.Size = new System.Drawing.Size(540, 139);
            this.lstRecentMatches.TabIndex = 0;
            // 
            // btnAddMatch
            // 
            this.btnAddMatch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddMatch.Location = new System.Drawing.Point(12, 305);
            this.btnAddMatch.Name = "btnAddMatch";
            this.btnAddMatch.Size = new System.Drawing.Size(560, 40);
            this.btnAddMatch.TabIndex = 3;
            this.btnAddMatch.Text = "+ Aggiungi Nuova Partita";
            this.btnAddMatch.UseVisualStyleBackColor = true;
            this.btnAddMatch.Click += new System.EventHandler(this.btnAddMatch_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.btnAddMatch);
            this.Controls.Add(this.grpRecentMatches);
            this.Controls.Add(this.grpStats);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KillerDex - Dead by Daylight Tracker";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.grpStats.ResumeLayout(false);
            this.grpRecentMatches.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
