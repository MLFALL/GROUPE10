namespace SystemeGestionEtudiants.Views
{
    partial class NotesFrom
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbEtudiant = new System.Windows.Forms.ComboBox();
            this.cmbMatiere = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.a = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.cbClasse = new System.Windows.Forms.ComboBox();
            this.cmbCours = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panelMatieres = new System.Windows.Forms.Panel();
            this.txtMatricule = new System.Windows.Forms.TextBox();
            this.dgvNotes = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotes)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbEtudiant
            // 
            this.cmbEtudiant.FormattingEnabled = true;
            this.cmbEtudiant.Location = new System.Drawing.Point(236, 192);
            this.cmbEtudiant.Margin = new System.Windows.Forms.Padding(4);
            this.cmbEtudiant.Name = "cmbEtudiant";
            this.cmbEtudiant.Size = new System.Drawing.Size(160, 24);
            this.cmbEtudiant.TabIndex = 0;
            this.cmbEtudiant.SelectedIndexChanged += new System.EventHandler(this.cmbEtudiant_SelectedIndexChanged);
            // 
            // cmbMatiere
            // 
            this.cmbMatiere.FormattingEnabled = true;
            this.cmbMatiere.Location = new System.Drawing.Point(-1, 498);
            this.cmbMatiere.Margin = new System.Windows.Forms.Padding(4);
            this.cmbMatiere.Name = "cmbMatiere";
            this.cmbMatiere.Size = new System.Drawing.Size(160, 24);
            this.cmbMatiere.TabIndex = 1;
            this.cmbMatiere.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(255, 169);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Etudiant";
            // 
            // txtNote
            // 
            this.txtNote.Location = new System.Drawing.Point(27, 467);
            this.txtNote.Margin = new System.Windows.Forms.Padding(4);
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(132, 23);
            this.txtNote.TabIndex = 4;
            this.txtNote.Visible = false;
            this.txtNote.TextChanged += new System.EventHandler(this.txtNote_TextChanged);
            // 
            // a
            // 
            this.a.AutoSize = true;
            this.a.Location = new System.Drawing.Point(671, 75);
            this.a.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.a.Name = "a";
            this.a.Size = new System.Drawing.Size(62, 17);
            this.a.TabIndex = 5;
            this.a.Text = "Matieres";
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnAdd.Image = global::SystemeGestionEtudiants.Properties.Resources.icons8_ajouter_24;
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdd.Location = new System.Drawing.Point(298, 387);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(85, 28);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Ajouter";
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // cbClasse
            // 
            this.cbClasse.FormattingEnabled = true;
            this.cbClasse.Location = new System.Drawing.Point(44, 192);
            this.cbClasse.Margin = new System.Windows.Forms.Padding(4);
            this.cbClasse.Name = "cbClasse";
            this.cbClasse.Size = new System.Drawing.Size(160, 24);
            this.cbClasse.TabIndex = 7;
            this.cbClasse.SelectedIndexChanged += new System.EventHandler(this.cbClasse_SelectedIndexChanged);
            // 
            // cmbCours
            // 
            this.cmbCours.FormattingEnabled = true;
            this.cmbCours.Location = new System.Drawing.Point(-1, 532);
            this.cmbCours.Margin = new System.Windows.Forms.Padding(4);
            this.cmbCours.Name = "cmbCours";
            this.cmbCours.Size = new System.Drawing.Size(160, 24);
            this.cmbCours.TabIndex = 8;
            this.cmbCours.Visible = false;
            this.cmbCours.SelectedIndexChanged += new System.EventHandler(this.cmbCours_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 169);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "Classe";
            // 
            // panelMatieres
            // 
            this.panelMatieres.Location = new System.Drawing.Point(440, 119);
            this.panelMatieres.Margin = new System.Windows.Forms.Padding(4);
            this.panelMatieres.Name = "panelMatieres";
            this.panelMatieres.Size = new System.Drawing.Size(564, 420);
            this.panelMatieres.TabIndex = 10;
            // 
            // txtMatricule
            // 
            this.txtMatricule.Enabled = false;
            this.txtMatricule.Location = new System.Drawing.Point(146, 128);
            this.txtMatricule.Name = "txtMatricule";
            this.txtMatricule.Size = new System.Drawing.Size(143, 23);
            this.txtMatricule.TabIndex = 11;
            // 
            // dgvNotes
            // 
            this.dgvNotes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNotes.Location = new System.Drawing.Point(45, 268);
            this.dgvNotes.Name = "dgvNotes";
            this.dgvNotes.Size = new System.Drawing.Size(240, 150);
            this.dgvNotes.TabIndex = 12;
            // 
            // NotesFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.dgvNotes);
            this.Controls.Add(this.txtMatricule);
            this.Controls.Add(this.panelMatieres);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbCours);
            this.Controls.Add(this.cbClasse);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.a);
            this.Controls.Add(this.txtNote);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbMatiere);
            this.Controls.Add(this.cmbEtudiant);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "NotesFrom";
            this.Text = "NotesFrom";
            this.Load += new System.EventHandler(this.NotesFrom_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbEtudiant;
        private System.Windows.Forms.ComboBox cmbMatiere;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Label a;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ComboBox cbClasse;
        private System.Windows.Forms.ComboBox cmbCours;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panelMatieres;
        private System.Windows.Forms.TextBox txtMatricule;
        private System.Windows.Forms.DataGridView dgvNotes;
    }
}