namespace KillerDex
{
    partial class AddMatch
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label lblAllies;
        private System.Windows.Forms.ListBox lstAllies;
        private System.Windows.Forms.Label lblAlliesHint;
        private System.Windows.Forms.Label lblMap;
        private System.Windows.Forms.ComboBox cmbMap;
        private System.Windows.Forms.Label lblKiller;
        private System.Windows.Forms.ComboBox cmbKiller;
        private System.Windows.Forms.Label lblFirstHook;
        private System.Windows.Forms.ComboBox cmbFirstHook;
        private System.Windows.Forms.Label lblGenerators;
        private System.Windows.Forms.ComboBox cmbGenerators;
        private System.Windows.Forms.Label lblSurvivors;
        private System.Windows.Forms.ComboBox cmbSurvivors;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
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
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.lblAllies = new System.Windows.Forms.Label();
            this.lstAllies = new System.Windows.Forms.ListBox();
            this.lblAlliesHint = new System.Windows.Forms.Label();
            this.lblMap = new System.Windows.Forms.Label();
            this.cmbMap = new System.Windows.Forms.ComboBox();
            this.lblKiller = new System.Windows.Forms.Label();
            this.cmbKiller = new System.Windows.Forms.ComboBox();
            this.lblFirstHook = new System.Windows.Forms.Label();
            this.cmbFirstHook = new System.Windows.Forms.ComboBox();
            this.lblGenerators = new System.Windows.Forms.Label();
            this.cmbGenerators = new System.Windows.Forms.ComboBox();
            this.lblSurvivors = new System.Windows.Forms.Label();
            this.cmbSurvivors = new System.Windows.Forms.ComboBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(12, 15);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(33, 13);
            this.lblDate.Text = "Data:";
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(15, 31);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(150, 20);
            this.dtpDate.TabIndex = 0;
            // 
            // lblAllies
            // 
            this.lblAllies.AutoSize = true;
            this.lblAllies.Location = new System.Drawing.Point(12, 60);
            this.lblAllies.Name = "lblAllies";
            this.lblAllies.Size = new System.Drawing.Size(40, 13);
            this.lblAllies.Text = "Alleati:";
            // 
            // lstAllies
            // 
            this.lstAllies.FormattingEnabled = true;
            this.lstAllies.Location = new System.Drawing.Point(15, 76);
            this.lstAllies.Name = "lstAllies";
            this.lstAllies.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstAllies.Size = new System.Drawing.Size(150, 95);
            this.lstAllies.TabIndex = 1;
            this.lstAllies.SelectedIndexChanged += new System.EventHandler(this.lstAllies_SelectedIndexChanged);
            // 
            // lblAlliesHint
            // 
            this.lblAlliesHint.AutoSize = true;
            this.lblAlliesHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.lblAlliesHint.ForeColor = System.Drawing.Color.Gray;
            this.lblAlliesHint.Location = new System.Drawing.Point(12, 174);
            this.lblAlliesHint.Name = "lblAlliesHint";
            this.lblAlliesHint.Size = new System.Drawing.Size(113, 13);
            this.lblAlliesHint.Text = "(Max 3 selezionabili)";
            // 
            // lblMap
            // 
            this.lblMap.AutoSize = true;
            this.lblMap.Location = new System.Drawing.Point(190, 15);
            this.lblMap.Name = "lblMap";
            this.lblMap.Size = new System.Drawing.Size(42, 13);
            this.lblMap.Text = "Mappa:";
            // 
            // cmbMap
            // 
            this.cmbMap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMap.FormattingEnabled = true;
            this.cmbMap.Location = new System.Drawing.Point(193, 31);
            this.cmbMap.Name = "cmbMap";
            this.cmbMap.Size = new System.Drawing.Size(180, 21);
            this.cmbMap.TabIndex = 2;
            // 
            // lblKiller
            // 
            this.lblKiller.AutoSize = true;
            this.lblKiller.Location = new System.Drawing.Point(190, 60);
            this.lblKiller.Name = "lblKiller";
            this.lblKiller.Size = new System.Drawing.Size(33, 13);
            this.lblKiller.Text = "Killer:";
            // 
            // cmbKiller
            // 
            this.cmbKiller.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKiller.FormattingEnabled = true;
            this.cmbKiller.Location = new System.Drawing.Point(193, 76);
            this.cmbKiller.Name = "cmbKiller";
            this.cmbKiller.Size = new System.Drawing.Size(180, 21);
            this.cmbKiller.TabIndex = 3;
            // 
            // lblFirstHook
            // 
            this.lblFirstHook.AutoSize = true;
            this.lblFirstHook.Location = new System.Drawing.Point(190, 105);
            this.lblFirstHook.Name = "lblFirstHook";
            this.lblFirstHook.Size = new System.Drawing.Size(74, 13);
            this.lblFirstHook.Text = "Primo gancio:";
            // 
            // cmbFirstHook
            // 
            this.cmbFirstHook.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFirstHook.FormattingEnabled = true;
            this.cmbFirstHook.Location = new System.Drawing.Point(193, 121);
            this.cmbFirstHook.Name = "cmbFirstHook";
            this.cmbFirstHook.Size = new System.Drawing.Size(180, 21);
            this.cmbFirstHook.TabIndex = 4;
            // 
            // lblGenerators
            // 
            this.lblGenerators.AutoSize = true;
            this.lblGenerators.Location = new System.Drawing.Point(190, 150);
            this.lblGenerators.Name = "lblGenerators";
            this.lblGenerators.Size = new System.Drawing.Size(108, 13);
            this.lblGenerators.Text = "Generatori completati:";
            // 
            // cmbGenerators
            // 
            this.cmbGenerators.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGenerators.FormattingEnabled = true;
            this.cmbGenerators.Location = new System.Drawing.Point(193, 166);
            this.cmbGenerators.Name = "cmbGenerators";
            this.cmbGenerators.Size = new System.Drawing.Size(60, 21);
            this.cmbGenerators.TabIndex = 5;
            // 
            // lblSurvivors
            // 
            this.lblSurvivors.AutoSize = true;
            this.lblSurvivors.Location = new System.Drawing.Point(270, 150);
            this.lblSurvivors.Name = "lblSurvivors";
            this.lblSurvivors.Size = new System.Drawing.Size(66, 13);
            this.lblSurvivors.Text = "Sopravvissuti:";
            // 
            // cmbSurvivors
            // 
            this.cmbSurvivors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSurvivors.FormattingEnabled = true;
            this.cmbSurvivors.Location = new System.Drawing.Point(273, 166);
            this.cmbSurvivors.Name = "cmbSurvivors";
            this.cmbSurvivors.Size = new System.Drawing.Size(60, 21);
            this.cmbSurvivors.TabIndex = 6;
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Location = new System.Drawing.Point(12, 200);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(38, 13);
            this.lblNotes.Text = "Memo:";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(15, 216);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(358, 60);
            this.txtNotes.TabIndex = 7;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(15, 290);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(170, 35);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Salva Partita";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(203, 290);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(170, 35);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Annulla";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AddMatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 340);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.lblAllies);
            this.Controls.Add(this.lstAllies);
            this.Controls.Add(this.lblAlliesHint);
            this.Controls.Add(this.lblMap);
            this.Controls.Add(this.cmbMap);
            this.Controls.Add(this.lblKiller);
            this.Controls.Add(this.cmbKiller);
            this.Controls.Add(this.lblFirstHook);
            this.Controls.Add(this.cmbFirstHook);
            this.Controls.Add(this.lblGenerators);
            this.Controls.Add(this.cmbGenerators);
            this.Controls.Add(this.lblSurvivors);
            this.Controls.Add(this.cmbSurvivors);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddMatch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nuova Partita";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}