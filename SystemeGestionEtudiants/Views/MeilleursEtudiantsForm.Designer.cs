namespace SystemeGestionEtudiants.Views
{
    partial class MeilleursEtudiantsForm
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
            this.dataGridViewMeilleursEleves = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMeilleursEleves)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewMeilleursEleves
            // 
            this.dataGridViewMeilleursEleves.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMeilleursEleves.Location = new System.Drawing.Point(51, 82);
            this.dataGridViewMeilleursEleves.Name = "dataGridViewMeilleursEleves";
            this.dataGridViewMeilleursEleves.ReadOnly = true;
            this.dataGridViewMeilleursEleves.Size = new System.Drawing.Size(634, 199);
            this.dataGridViewMeilleursEleves.TabIndex = 0;
            // 
            // MeilleursEtudiantsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridViewMeilleursEleves);
            this.Name = "MeilleursEtudiantsForm";
            this.Text = "MeilleursEtudiantsForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMeilleursEleves)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewMeilleursEleves;
    }
}