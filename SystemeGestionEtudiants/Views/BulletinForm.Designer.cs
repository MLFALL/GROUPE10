namespace SystemeGestionEtudiants.Views
{
    partial class BulletinForm
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
            this.cmbEtudiant = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGenererBulletin = new System.Windows.Forms.Button();
            this.btnTelechargerBulletin = new System.Windows.Forms.Button();
            this.panelBulletin = new System.Windows.Forms.Panel();
            this.btnExcel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbClasse
            // 
            this.cbClasse.FormattingEnabled = true;
            this.cbClasse.Location = new System.Drawing.Point(17, 184);
            this.cbClasse.Margin = new System.Windows.Forms.Padding(5);
            this.cbClasse.Name = "cbClasse";
            this.cbClasse.Size = new System.Drawing.Size(212, 24);
            this.cbClasse.TabIndex = 8;
            this.cbClasse.SelectedIndexChanged += new System.EventHandler(this.cbClasse_SelectedIndexChanged);
            // 
            // cmbEtudiant
            // 
            this.cmbEtudiant.FormattingEnabled = true;
            this.cmbEtudiant.Location = new System.Drawing.Point(293, 184);
            this.cmbEtudiant.Margin = new System.Windows.Forms.Padding(5);
            this.cmbEtudiant.Name = "cmbEtudiant";
            this.cmbEtudiant.Size = new System.Drawing.Size(212, 24);
            this.cmbEtudiant.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 157);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "Classe";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(307, 157);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "Etudiants";
            // 
            // btnGenererBulletin
            // 
            this.btnGenererBulletin.BackColor = System.Drawing.Color.Lime;
            this.btnGenererBulletin.Location = new System.Drawing.Point(211, 248);
            this.btnGenererBulletin.Margin = new System.Windows.Forms.Padding(4);
            this.btnGenererBulletin.Name = "btnGenererBulletin";
            this.btnGenererBulletin.Size = new System.Drawing.Size(100, 28);
            this.btnGenererBulletin.TabIndex = 12;
            this.btnGenererBulletin.Text = "Générer Bulletin";
            this.btnGenererBulletin.UseVisualStyleBackColor = false;
            this.btnGenererBulletin.Click += new System.EventHandler(this.btnGenererBulletin_Click);
            // 
            // btnTelechargerBulletin
            // 
            this.btnTelechargerBulletin.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnTelechargerBulletin.Location = new System.Drawing.Point(116, 318);
            this.btnTelechargerBulletin.Margin = new System.Windows.Forms.Padding(4);
            this.btnTelechargerBulletin.Name = "btnTelechargerBulletin";
            this.btnTelechargerBulletin.Size = new System.Drawing.Size(122, 62);
            this.btnTelechargerBulletin.TabIndex = 13;
            this.btnTelechargerBulletin.Text = "Téléchargement en PDF";
            this.btnTelechargerBulletin.UseVisualStyleBackColor = false;
            this.btnTelechargerBulletin.Click += new System.EventHandler(this.btnTelechargerBulletin_Click);
            // 
            // panelBulletin
            // 
            this.panelBulletin.Location = new System.Drawing.Point(513, 87);
            this.panelBulletin.Name = "panelBulletin";
            this.panelBulletin.Size = new System.Drawing.Size(542, 455);
            this.panelBulletin.TabIndex = 14;
            // 
            // btnExcel
            // 
            this.btnExcel.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnExcel.Location = new System.Drawing.Point(293, 318);
            this.btnExcel.Margin = new System.Windows.Forms.Padding(4);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(131, 62);
            this.btnExcel.TabIndex = 15;
            this.btnExcel.Text = "Téléchargement en Excel";
            this.btnExcel.UseVisualStyleBackColor = false;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // BulletinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.panelBulletin);
            this.Controls.Add(this.btnTelechargerBulletin);
            this.Controls.Add(this.btnGenererBulletin);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbEtudiant);
            this.Controls.Add(this.cbClasse);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BulletinForm";
            this.Text = "BulletinForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbClasse;
        private System.Windows.Forms.ComboBox cmbEtudiant;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGenererBulletin;
        private System.Windows.Forms.Button btnTelechargerBulletin;
        private System.Windows.Forms.Panel panelBulletin;
        private System.Windows.Forms.Button btnExcel;
    }
}