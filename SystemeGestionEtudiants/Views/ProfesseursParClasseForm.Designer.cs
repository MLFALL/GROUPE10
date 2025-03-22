namespace SystemeGestionEtudiants.Views
{
    partial class ProfesseursParClasseForm
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
            this.cbClasse = new System.Windows.Forms.ComboBox();
            this.dataGridViewProfesseurs = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnVoir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProfesseurs)).BeginInit();
            this.SuspendLayout();
            // 
            // cbClasse
            // 
            this.cbClasse.FormattingEnabled = true;
            this.cbClasse.Location = new System.Drawing.Point(12, 148);
            this.cbClasse.Name = "cbClasse";
            this.cbClasse.Size = new System.Drawing.Size(121, 21);
            this.cbClasse.TabIndex = 0;
            this.cbClasse.SelectedIndexChanged += new System.EventHandler(this.cbClasse_SelectedIndexChanged);
            // 
            // dataGridViewProfesseurs
            // 
            this.dataGridViewProfesseurs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProfesseurs.Location = new System.Drawing.Point(172, 119);
            this.dataGridViewProfesseurs.Name = "dataGridViewProfesseurs";
            this.dataGridViewProfesseurs.Size = new System.Drawing.Size(603, 210);
            this.dataGridViewProfesseurs.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "CLasse";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(400, 380);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(238, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Télécharger la liste de tous les profs";
            // 
            // btnVoir
            // 
            this.btnVoir.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnVoir.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVoir.Image = global::SystemeGestionEtudiants.Properties.Resources.icons8_télécharger_24;
            this.btnVoir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnVoir.Location = new System.Drawing.Point(644, 368);
            this.btnVoir.Name = "btnVoir";
            this.btnVoir.Size = new System.Drawing.Size(131, 29);
            this.btnVoir.TabIndex = 3;
            this.btnVoir.Text = "Télécharger";
            this.btnVoir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnVoir.UseVisualStyleBackColor = false;
            this.btnVoir.Click += new System.EventHandler(this.btnVoir_Click);
            // 
            // ProfesseursParClasseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnVoir);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewProfesseurs);
            this.Controls.Add(this.cbClasse);
            this.Name = "ProfesseursParClasseForm";
            this.Text = "ProfesseursParClasseForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProfesseurs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbClasse;
        private System.Windows.Forms.DataGridView dataGridViewProfesseurs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnVoir;
        private System.Windows.Forms.Label label2;
    }
}