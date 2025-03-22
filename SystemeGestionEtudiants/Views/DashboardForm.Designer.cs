namespace SystemeGestionEtudiants.Views
{
    partial class DashboardForm
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
            this.lblNombreClasses = new System.Windows.Forms.Label();
            this.lblNombreEtudiants = new System.Windows.Forms.Label();
            this.lblNombreProfesseurs = new System.Windows.Forms.Label();
            this.lblPresentation = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblNombreClasses
            // 
            this.lblNombreClasses.AutoSize = true;
            this.lblNombreClasses.Font = new System.Drawing.Font("Times New Roman", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombreClasses.Location = new System.Drawing.Point(97, 71);
            this.lblNombreClasses.Name = "lblNombreClasses";
            this.lblNombreClasses.Size = new System.Drawing.Size(205, 46);
            this.lblNombreClasses.TabIndex = 0;
            this.lblNombreClasses.Text = "nbre Classe";
            // 
            // lblNombreEtudiants
            // 
            this.lblNombreEtudiants.AutoSize = true;
            this.lblNombreEtudiants.Font = new System.Drawing.Font("Times New Roman", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombreEtudiants.Location = new System.Drawing.Point(322, 71);
            this.lblNombreEtudiants.Name = "lblNombreEtudiants";
            this.lblNombreEtudiants.Size = new System.Drawing.Size(235, 46);
            this.lblNombreEtudiants.TabIndex = 1;
            this.lblNombreEtudiants.Text = "nbre Etudiant";
            // 
            // lblNombreProfesseurs
            // 
            this.lblNombreProfesseurs.AutoSize = true;
            this.lblNombreProfesseurs.Font = new System.Drawing.Font("Times New Roman", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombreProfesseurs.Location = new System.Drawing.Point(604, 71);
            this.lblNombreProfesseurs.Name = "lblNombreProfesseurs";
            this.lblNombreProfesseurs.Size = new System.Drawing.Size(84, 46);
            this.lblNombreProfesseurs.TabIndex = 2;
            this.lblNombreProfesseurs.Text = "prof";
            this.lblNombreProfesseurs.Click += new System.EventHandler(this.lblNombreProfesseurs_Click);
            // 
            // lblPresentation
            // 
            this.lblPresentation.AutoSize = true;
            this.lblPresentation.Font = new System.Drawing.Font("Times New Roman", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPresentation.Location = new System.Drawing.Point(89, 217);
            this.lblPresentation.Name = "lblPresentation";
            this.lblPresentation.Size = new System.Drawing.Size(148, 31);
            this.lblPresentation.TabIndex = 3;
            this.lblPresentation.Text = "presentation";
            this.lblPresentation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblPresentation);
            this.Controls.Add(this.lblNombreProfesseurs);
            this.Controls.Add(this.lblNombreEtudiants);
            this.Controls.Add(this.lblNombreClasses);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DashboardForm";
            this.Text = "DashboardForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNombreClasses;
        private System.Windows.Forms.Label lblNombreEtudiants;
        private System.Windows.Forms.Label lblNombreProfesseurs;
        private System.Windows.Forms.Label lblPresentation;
    }
}