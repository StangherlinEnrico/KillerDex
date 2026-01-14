using KillerDex.Controls;

namespace KillerDex
{
    partial class AddMatch
    {
        private System.ComponentModel.IContainer components = null;

        // Header
        private DbdFormHeader formHeader;

        // Main content
        private System.Windows.Forms.Panel pnlContent;

        // Left column
        private System.Windows.Forms.Panel pnlLeftColumn;
        private System.Windows.Forms.Label lblDate;
        private DbdDatePicker dtpDate;
        private System.Windows.Forms.Label lblMap;
        private DbdComboBox cmbMap;
        private System.Windows.Forms.Label lblKiller;
        private DbdComboBox cmbKiller;
        private System.Windows.Forms.Label lblAllies;
        private System.Windows.Forms.Label lblAlliesHint;
        private DbdMultiSelect msAllies;

        // Right column
        private System.Windows.Forms.Panel pnlRightColumn;
        private System.Windows.Forms.Label lblFirstHook;
        private DbdComboBox cmbFirstHook;
        private System.Windows.Forms.Label lblGenerators;
        private System.Windows.Forms.Panel pnlGenerators;
        private System.Windows.Forms.Label lblSurvivors;
        private DbdMultiSelect msSurvivors;
        private System.Windows.Forms.Label lblNotes;
        private DbdTextBox txtNotes;

        // Footer
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.formHeader = new DbdFormHeader();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlLeftColumn = new System.Windows.Forms.Panel();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpDate = new DbdDatePicker();
            this.lblMap = new System.Windows.Forms.Label();
            this.cmbMap = new DbdComboBox();
            this.lblKiller = new System.Windows.Forms.Label();
            this.cmbKiller = new DbdComboBox();
            this.lblAllies = new System.Windows.Forms.Label();
            this.lblAlliesHint = new System.Windows.Forms.Label();
            this.msAllies = new DbdMultiSelect();
            this.pnlRightColumn = new System.Windows.Forms.Panel();
            this.lblFirstHook = new System.Windows.Forms.Label();
            this.cmbFirstHook = new DbdComboBox();
            this.lblGenerators = new System.Windows.Forms.Label();
            this.pnlGenerators = new System.Windows.Forms.Panel();
            this.lblSurvivors = new System.Windows.Forms.Label();
            this.msSurvivors = new DbdMultiSelect();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new DbdTextBox();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlContent.SuspendLayout();
            this.pnlLeftColumn.SuspendLayout();
            this.pnlRightColumn.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            //
            // formHeader
            //
            this.formHeader.Location = new System.Drawing.Point(0, 0);
            this.formHeader.Name = "formHeader";
            this.formHeader.TabIndex = 0;
            this.formHeader.Title = "New Match";
            this.formHeader.TitleFontSize = 20F;
            this.formHeader.Subtitle = "Record your Dead by Daylight match";
            //
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.pnlContent.Controls.Add(this.pnlLeftColumn);
            this.pnlContent.Controls.Add(this.pnlRightColumn);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 80);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(20, 20, 20, 10);
            this.pnlContent.Size = new System.Drawing.Size(700, 390);
            this.pnlContent.TabIndex = 1;
            // 
            // pnlLeftColumn
            // 
            this.pnlLeftColumn.Controls.Add(this.lblDate);
            this.pnlLeftColumn.Controls.Add(this.dtpDate);
            this.pnlLeftColumn.Controls.Add(this.lblMap);
            this.pnlLeftColumn.Controls.Add(this.cmbMap);
            this.pnlLeftColumn.Controls.Add(this.lblKiller);
            this.pnlLeftColumn.Controls.Add(this.cmbKiller);
            this.pnlLeftColumn.Controls.Add(this.lblAllies);
            this.pnlLeftColumn.Controls.Add(this.lblAlliesHint);
            this.pnlLeftColumn.Controls.Add(this.msAllies);
            this.pnlLeftColumn.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeftColumn.Location = new System.Drawing.Point(20, 20);
            this.pnlLeftColumn.Name = "pnlLeftColumn";
            this.pnlLeftColumn.Size = new System.Drawing.Size(320, 360);
            this.pnlLeftColumn.TabIndex = 0;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.lblDate.Location = new System.Drawing.Point(0, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(40, 19);
            this.lblDate.Text = "Date";
            // 
            // dtpDate
            //
            this.dtpDate.CustomFormat = "dd/MM/yyyy";
            this.dtpDate.Location = new System.Drawing.Point(0, 22);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(310, 32);
            this.dtpDate.TabIndex = 0;
            // 
            // lblMap
            // 
            this.lblMap.AutoSize = true;
            this.lblMap.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.lblMap.Location = new System.Drawing.Point(0, 60);
            this.lblMap.Name = "lblMap";
            this.lblMap.Size = new System.Drawing.Size(37, 19);
            this.lblMap.Text = "Map";
            //
            // cmbMap
            //
            this.cmbMap.Location = new System.Drawing.Point(0, 82);
            this.cmbMap.Name = "cmbMap";
            this.cmbMap.Size = new System.Drawing.Size(310, 32);
            this.cmbMap.TabIndex = 1;
            // 
            // lblKiller
            // 
            this.lblKiller.AutoSize = true;
            this.lblKiller.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblKiller.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.lblKiller.Location = new System.Drawing.Point(0, 120);
            this.lblKiller.Name = "lblKiller";
            this.lblKiller.Size = new System.Drawing.Size(43, 19);
            this.lblKiller.Text = "Killer";
            //
            // cmbKiller
            //
            this.cmbKiller.Location = new System.Drawing.Point(0, 142);
            this.cmbKiller.Name = "cmbKiller";
            this.cmbKiller.Size = new System.Drawing.Size(310, 32);
            this.cmbKiller.TabIndex = 2;
            // 
            // lblAllies
            // 
            this.lblAllies.AutoSize = true;
            this.lblAllies.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblAllies.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.lblAllies.Location = new System.Drawing.Point(0, 185);
            this.lblAllies.Name = "lblAllies";
            this.lblAllies.Size = new System.Drawing.Size(44, 19);
            this.lblAllies.Text = "Allies";
            // 
            // lblAlliesHint
            // 
            this.lblAlliesHint.AutoSize = true;
            this.lblAlliesHint.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblAlliesHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.lblAlliesHint.Location = new System.Drawing.Point(50, 188);
            this.lblAlliesHint.Name = "lblAlliesHint";
            this.lblAlliesHint.Size = new System.Drawing.Size(100, 13);
            this.lblAlliesHint.Text = "(Max 3 selectable)";
            //
            // msAllies
            //
            this.msAllies.Location = new System.Drawing.Point(0, 207);
            this.msAllies.Name = "msAllies";
            this.msAllies.Size = new System.Drawing.Size(310, 32);
            this.msAllies.MaxSelection = 3;
            this.msAllies.TabIndex = 3;
            this.msAllies.SelectionChanged += new System.EventHandler(this.msAllies_SelectionChanged);
            // 
            // pnlRightColumn
            // 
            this.pnlRightColumn.Controls.Add(this.lblFirstHook);
            this.pnlRightColumn.Controls.Add(this.cmbFirstHook);
            this.pnlRightColumn.Controls.Add(this.lblGenerators);
            this.pnlRightColumn.Controls.Add(this.pnlGenerators);
            this.pnlRightColumn.Controls.Add(this.lblSurvivors);
            this.pnlRightColumn.Controls.Add(this.msSurvivors);
            this.pnlRightColumn.Controls.Add(this.lblNotes);
            this.pnlRightColumn.Controls.Add(this.txtNotes);
            this.pnlRightColumn.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRightColumn.Location = new System.Drawing.Point(360, 20);
            this.pnlRightColumn.Name = "pnlRightColumn";
            this.pnlRightColumn.Size = new System.Drawing.Size(320, 360);
            this.pnlRightColumn.TabIndex = 1;
            // 
            // lblFirstHook
            // 
            this.lblFirstHook.AutoSize = true;
            this.lblFirstHook.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblFirstHook.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.lblFirstHook.Location = new System.Drawing.Point(0, 0);
            this.lblFirstHook.Name = "lblFirstHook";
            this.lblFirstHook.Size = new System.Drawing.Size(76, 19);
            this.lblFirstHook.Text = "First Hook";
            //
            // cmbFirstHook
            //
            this.cmbFirstHook.Location = new System.Drawing.Point(0, 22);
            this.cmbFirstHook.Name = "cmbFirstHook";
            this.cmbFirstHook.Size = new System.Drawing.Size(310, 32);
            this.cmbFirstHook.TabIndex = 4;
            // 
            // lblGenerators
            // 
            this.lblGenerators.AutoSize = true;
            this.lblGenerators.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblGenerators.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.lblGenerators.Location = new System.Drawing.Point(0, 65);
            this.lblGenerators.Name = "lblGenerators";
            this.lblGenerators.Size = new System.Drawing.Size(153, 19);
            this.lblGenerators.Text = "Generators Completed";
            // 
            // pnlGenerators
            // 
            this.pnlGenerators.Location = new System.Drawing.Point(0, 87);
            this.pnlGenerators.Name = "pnlGenerators";
            this.pnlGenerators.Size = new System.Drawing.Size(310, 50);
            this.pnlGenerators.TabIndex = 5;
            // 
            // lblSurvivors
            // 
            this.lblSurvivors.AutoSize = true;
            this.lblSurvivors.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSurvivors.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.lblSurvivors.Location = new System.Drawing.Point(0, 145);
            this.lblSurvivors.Name = "lblSurvivors";
            this.lblSurvivors.Size = new System.Drawing.Size(126, 19);
            this.lblSurvivors.Text = "Survivors Escaped";
            //
            // msSurvivors
            //
            this.msSurvivors.Location = new System.Drawing.Point(0, 167);
            this.msSurvivors.Name = "msSurvivors";
            this.msSurvivors.Size = new System.Drawing.Size(310, 32);
            this.msSurvivors.MaxSelection = 4;
            this.msSurvivors.TabIndex = 6;
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblNotes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.lblNotes.Location = new System.Drawing.Point(0, 225);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(47, 19);
            this.lblNotes.Text = "Notes";
            //
            // txtNotes
            //
            this.txtNotes.Location = new System.Drawing.Point(0, 247);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(310, 100);
            this.txtNotes.TabIndex = 7;
            this.txtNotes.Multiline = true;
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(20)))));
            this.pnlFooter.Controls.Add(this.btnSave);
            this.pnlFooter.Controls.Add(this.btnCancel);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 470);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(700, 70);
            this.pnlFooter.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.btnSave.FlatAppearance.BorderSize = 1;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(200, 15);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(150, 42);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "💾 Save Match";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(55)))));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(80)))));
            this.btnCancel.FlatAppearance.BorderSize = 1;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(45)))));
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(65)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(360, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(140, 42);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AddMatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.ClientSize = new System.Drawing.Size(700, 540);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.formHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddMatch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "KillerDex - New Match";
            this.pnlContent.ResumeLayout(false);
            this.pnlLeftColumn.ResumeLayout(false);
            this.pnlLeftColumn.PerformLayout();
            this.pnlRightColumn.ResumeLayout(false);
            this.pnlRightColumn.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}