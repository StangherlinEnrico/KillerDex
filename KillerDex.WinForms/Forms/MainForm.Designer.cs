namespace KillerDex
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // Menu
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manageMenu;
        private System.Windows.Forms.ToolStripMenuItem killersMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alliesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem languageMenu;
        private System.Windows.Forms.ToolStripMenuItem italianMenuItem;
        private System.Windows.Forms.ToolStripMenuItem englishMenuItem;

        // Header
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlHeaderAccent;
        private System.Windows.Forms.Label lblAppTitle;
        private System.Windows.Forms.Label lblAppSubtitle;

        // Stats cards
        private System.Windows.Forms.Panel pnlStats;
        private System.Windows.Forms.Panel pnlStatTotal;
        private System.Windows.Forms.Panel pnlStatWins;
        private System.Windows.Forms.Panel pnlStatLosses;
        private System.Windows.Forms.Panel pnlStatWinRate;

        // Recent matches
        private System.Windows.Forms.Panel pnlRecentMatches;
        private System.Windows.Forms.Label lblRecentTitle;
        private System.Windows.Forms.Panel pnlRecentAccent;
        private System.Windows.Forms.Panel pnlMatchesList;

        // Footer / Actions
        private System.Windows.Forms.Panel pnlFooter;
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
            this.killersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alliesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.italianMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlHeaderAccent = new System.Windows.Forms.Panel();
            this.lblAppTitle = new System.Windows.Forms.Label();
            this.lblAppSubtitle = new System.Windows.Forms.Label();
            this.pnlStats = new System.Windows.Forms.Panel();
            this.pnlStatTotal = new System.Windows.Forms.Panel();
            this.pnlStatWins = new System.Windows.Forms.Panel();
            this.pnlStatLosses = new System.Windows.Forms.Panel();
            this.pnlStatWinRate = new System.Windows.Forms.Panel();
            this.pnlRecentMatches = new System.Windows.Forms.Panel();
            this.lblRecentTitle = new System.Windows.Forms.Label();
            this.pnlRecentAccent = new System.Windows.Forms.Panel();
            this.pnlMatchesList = new System.Windows.Forms.Panel();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnAddMatch = new System.Windows.Forms.Button();
            this.menuStrip.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlStats.SuspendLayout();
            this.pnlRecentMatches.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(15)))));
            this.menuStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuStrip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.fileMenu,
                this.manageMenu,
                this.languageMenu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(10, 4, 0, 4);
            this.menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip.Size = new System.Drawing.Size(750, 27);
            this.menuStrip.TabIndex = 0;
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.exitMenuItem});
            this.fileMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(37, 19);
            this.fileMenu.Text = "File";
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(35)))));
            this.exitMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(100, 22);
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // manageMenu
            // 
            this.manageMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.killersMenuItem,
                this.mapsMenuItem,
                this.alliesMenuItem});
            this.manageMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.manageMenu.Name = "manageMenu";
            this.manageMenu.Size = new System.Drawing.Size(62, 19);
            this.manageMenu.Text = "Manage";
            // 
            // killersMenuItem
            // 
            this.killersMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(35)))));
            this.killersMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.killersMenuItem.Name = "killersMenuItem";
            this.killersMenuItem.Size = new System.Drawing.Size(110, 22);
            this.killersMenuItem.Text = "💀 Killers";
            this.killersMenuItem.Click += new System.EventHandler(this.killersMenuItem_Click);
            // 
            // mapsMenuItem
            // 
            this.mapsMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(35)))));
            this.mapsMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.mapsMenuItem.Name = "mapsMenuItem";
            this.mapsMenuItem.Size = new System.Drawing.Size(110, 22);
            this.mapsMenuItem.Text = "🗺️ Maps";
            this.mapsMenuItem.Click += new System.EventHandler(this.mapsMenuItem_Click);
            // 
            // alliesMenuItem
            // 
            this.alliesMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(35)))));
            this.alliesMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.alliesMenuItem.Name = "alliesMenuItem";
            this.alliesMenuItem.Size = new System.Drawing.Size(110, 22);
            this.alliesMenuItem.Text = "👤 Allies";
            this.alliesMenuItem.Click += new System.EventHandler(this.alliesMenuItem_Click);
            // 
            // languageMenu
            // 
            this.languageMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.italianMenuItem,
                this.englishMenuItem});
            this.languageMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.languageMenu.Name = "languageMenu";
            this.languageMenu.Size = new System.Drawing.Size(71, 19);
            this.languageMenu.Text = "Language";
            // 
            // italianMenuItem
            // 
            this.italianMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(35)))));
            this.italianMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.italianMenuItem.Name = "italianMenuItem";
            this.italianMenuItem.Size = new System.Drawing.Size(130, 22);
            this.italianMenuItem.Text = "🇮🇹 Italiano";
            this.italianMenuItem.Click += new System.EventHandler(this.italianMenuItem_Click);
            // 
            // englishMenuItem
            // 
            this.englishMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(35)))));
            this.englishMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.englishMenuItem.Name = "englishMenuItem";
            this.englishMenuItem.Size = new System.Drawing.Size(130, 22);
            this.englishMenuItem.Text = "🇬🇧 English";
            this.englishMenuItem.Click += new System.EventHandler(this.englishMenuItem_Click);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(20)))));
            this.pnlHeader.Controls.Add(this.pnlHeaderAccent);
            this.pnlHeader.Controls.Add(this.lblAppTitle);
            this.pnlHeader.Controls.Add(this.lblAppSubtitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 27);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(750, 90);
            this.pnlHeader.TabIndex = 1;
            // 
            // pnlHeaderAccent
            // 
            this.pnlHeaderAccent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.pnlHeaderAccent.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlHeaderAccent.Location = new System.Drawing.Point(0, 87);
            this.pnlHeaderAccent.Name = "pnlHeaderAccent";
            this.pnlHeaderAccent.Size = new System.Drawing.Size(750, 3);
            this.pnlHeaderAccent.TabIndex = 0;
            // 
            // lblAppTitle
            // 
            this.lblAppTitle.AutoSize = true;
            this.lblAppTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblAppTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lblAppTitle.Location = new System.Drawing.Point(25, 18);
            this.lblAppTitle.Name = "lblAppTitle";
            this.lblAppTitle.Size = new System.Drawing.Size(165, 45);
            this.lblAppTitle.Text = "KillerDex";
            // 
            // lblAppSubtitle
            // 
            this.lblAppSubtitle.AutoSize = true;
            this.lblAppSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblAppSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblAppSubtitle.Location = new System.Drawing.Point(29, 60);
            this.lblAppSubtitle.Name = "lblAppSubtitle";
            this.lblAppSubtitle.Size = new System.Drawing.Size(182, 19);
            this.lblAppSubtitle.Text = "Dead by Daylight Match Tracker";
            // 
            // pnlStats
            // 
            this.pnlStats.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.pnlStats.Controls.Add(this.pnlStatTotal);
            this.pnlStats.Controls.Add(this.pnlStatWins);
            this.pnlStats.Controls.Add(this.pnlStatLosses);
            this.pnlStats.Controls.Add(this.pnlStatWinRate);
            this.pnlStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStats.Location = new System.Drawing.Point(0, 117);
            this.pnlStats.Name = "pnlStats";
            this.pnlStats.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.pnlStats.Size = new System.Drawing.Size(750, 110);
            this.pnlStats.TabIndex = 2;
            // 
            // pnlStatTotal
            // 
            this.pnlStatTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(38)))));
            this.pnlStatTotal.Location = new System.Drawing.Point(20, 15);
            this.pnlStatTotal.Name = "pnlStatTotal";
            this.pnlStatTotal.Size = new System.Drawing.Size(165, 80);
            this.pnlStatTotal.TabIndex = 0;
            // 
            // pnlStatWins
            // 
            this.pnlStatWins.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(38)))));
            this.pnlStatWins.Location = new System.Drawing.Point(200, 15);
            this.pnlStatWins.Name = "pnlStatWins";
            this.pnlStatWins.Size = new System.Drawing.Size(165, 80);
            this.pnlStatWins.TabIndex = 1;
            // 
            // pnlStatLosses
            // 
            this.pnlStatLosses.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(38)))));
            this.pnlStatLosses.Location = new System.Drawing.Point(380, 15);
            this.pnlStatLosses.Name = "pnlStatLosses";
            this.pnlStatLosses.Size = new System.Drawing.Size(165, 80);
            this.pnlStatLosses.TabIndex = 2;
            // 
            // pnlStatWinRate
            // 
            this.pnlStatWinRate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(38)))));
            this.pnlStatWinRate.Location = new System.Drawing.Point(560, 15);
            this.pnlStatWinRate.Name = "pnlStatWinRate";
            this.pnlStatWinRate.Size = new System.Drawing.Size(165, 80);
            this.pnlStatWinRate.TabIndex = 3;
            // 
            // pnlRecentMatches
            // 
            this.pnlRecentMatches.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.pnlRecentMatches.Controls.Add(this.lblRecentTitle);
            this.pnlRecentMatches.Controls.Add(this.pnlRecentAccent);
            this.pnlRecentMatches.Controls.Add(this.pnlMatchesList);
            this.pnlRecentMatches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRecentMatches.Location = new System.Drawing.Point(0, 227);
            this.pnlRecentMatches.Name = "pnlRecentMatches";
            this.pnlRecentMatches.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.pnlRecentMatches.Size = new System.Drawing.Size(750, 283);
            this.pnlRecentMatches.TabIndex = 3;
            // 
            // lblRecentTitle
            // 
            this.lblRecentTitle.AutoSize = true;
            this.lblRecentTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblRecentTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.lblRecentTitle.Location = new System.Drawing.Point(20, 10);
            this.lblRecentTitle.Name = "lblRecentTitle";
            this.lblRecentTitle.Size = new System.Drawing.Size(145, 25);
            this.lblRecentTitle.Text = "Recent Matches";
            // 
            // pnlRecentAccent
            // 
            this.pnlRecentAccent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.pnlRecentAccent.Location = new System.Drawing.Point(20, 38);
            this.pnlRecentAccent.Name = "pnlRecentAccent";
            this.pnlRecentAccent.Size = new System.Drawing.Size(60, 3);
            this.pnlRecentAccent.TabIndex = 0;
            // 
            // pnlMatchesList
            // 
            this.pnlMatchesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMatchesList.AutoScroll = true;
            this.pnlMatchesList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(30)))));
            this.pnlMatchesList.Location = new System.Drawing.Point(20, 50);
            this.pnlMatchesList.Name = "pnlMatchesList";
            this.pnlMatchesList.Size = new System.Drawing.Size(710, 220);
            this.pnlMatchesList.TabIndex = 1;
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(20)))));
            this.pnlFooter.Controls.Add(this.btnAddMatch);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 510);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(750, 70);
            this.pnlFooter.TabIndex = 4;
            // 
            // btnAddMatch
            // 
            this.btnAddMatch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAddMatch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.btnAddMatch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddMatch.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.btnAddMatch.FlatAppearance.BorderSize = 1;
            this.btnAddMatch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.btnAddMatch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnAddMatch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddMatch.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnAddMatch.ForeColor = System.Drawing.Color.White;
            this.btnAddMatch.Location = new System.Drawing.Point(250, 12);
            this.btnAddMatch.Name = "btnAddMatch";
            this.btnAddMatch.Size = new System.Drawing.Size(250, 46);
            this.btnAddMatch.TabIndex = 0;
            this.btnAddMatch.Text = "➕ Add New Match";
            this.btnAddMatch.UseVisualStyleBackColor = false;
            this.btnAddMatch.Click += new System.EventHandler(this.btnAddMatch_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.ClientSize = new System.Drawing.Size(750, 580);
            this.Controls.Add(this.pnlRecentMatches);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlStats);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(766, 619);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KillerDex - Dead by Daylight Tracker";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlStats.ResumeLayout(false);
            this.pnlRecentMatches.ResumeLayout(false);
            this.pnlRecentMatches.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}