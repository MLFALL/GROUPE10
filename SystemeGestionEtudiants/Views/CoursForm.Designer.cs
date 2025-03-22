namespace SystemeGestionEtudiants.Views
{
    partial class CoursForm
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
            this.dataGridViewCours = new System.Windows.Forms.DataGridView();
            this.dataGridViewMatieres = new System.Windows.Forms.DataGridView();
            this.cbClasse = new System.Windows.Forms.ComboBox();
            this.btnAddCours = new System.Windows.Forms.Button();
            this.btnAddMatieres = new System.Windows.Forms.Button();
            this.txtNomCours = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNomMatiere = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAssociateMatiere = new System.Windows.Forms.Button();
            this.btnUpdateCours = new System.Windows.Forms.Button();
            this.btnDeleteCours = new System.Windows.Forms.Button();
            this.btnDeleteMatiere = new System.Windows.Forms.Button();
            this.btnUpdateMatiere = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCours)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMatieres)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewCours
            // 
            this.dataGridViewCours.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCours.Location = new System.Drawing.Point(176, 95);
            this.dataGridViewCours.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewCours.Name = "dataGridViewCours";
            this.dataGridViewCours.Size = new System.Drawing.Size(351, 185);
            this.dataGridViewCours.TabIndex = 0;
            this.dataGridViewCours.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCours_CellDoubleClick);
            // 
            // dataGridViewMatieres
            // 
            this.dataGridViewMatieres.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewMatieres.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMatieres.Location = new System.Drawing.Point(718, 95);
            this.dataGridViewMatieres.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewMatieres.Name = "dataGridViewMatieres";
            this.dataGridViewMatieres.Size = new System.Drawing.Size(320, 185);
            this.dataGridViewMatieres.TabIndex = 1;
            this.dataGridViewMatieres.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMatieres_CellDoubleClick);
            // 
            // cbClasse
            // 
            this.cbClasse.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cbClasse.FormattingEnabled = true;
            this.cbClasse.Location = new System.Drawing.Point(414, 408);
            this.cbClasse.Margin = new System.Windows.Forms.Padding(4);
            this.cbClasse.Name = "cbClasse";
            this.cbClasse.Size = new System.Drawing.Size(160, 24);
            this.cbClasse.TabIndex = 2;
            // 
            // btnAddCours
            // 
            this.btnAddCours.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnAddCours.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnAddCours.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddCours.Location = new System.Drawing.Point(102, 287);
            this.btnAddCours.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddCours.Name = "btnAddCours";
            this.btnAddCours.Size = new System.Drawing.Size(81, 48);
            this.btnAddCours.TabIndex = 3;
            this.btnAddCours.Text = "Ajouter Cours";
            this.btnAddCours.UseVisualStyleBackColor = false;
            this.btnAddCours.Click += new System.EventHandler(this.btnAddCours_Click);
            // 
            // btnAddMatieres
            // 
            this.btnAddMatieres.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnAddMatieres.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnAddMatieres.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddMatieres.Location = new System.Drawing.Point(639, 288);
            this.btnAddMatieres.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddMatieres.Name = "btnAddMatieres";
            this.btnAddMatieres.Size = new System.Drawing.Size(88, 48);
            this.btnAddMatieres.TabIndex = 4;
            this.btnAddMatieres.Text = "Ajouter Matieres";
            this.btnAddMatieres.UseVisualStyleBackColor = false;
            this.btnAddMatieres.Click += new System.EventHandler(this.btnAddMatieres_Click);
            // 
            // txtNomCours
            // 
            this.txtNomCours.Location = new System.Drawing.Point(22, 149);
            this.txtNomCours.Margin = new System.Windows.Forms.Padding(4);
            this.txtNomCours.Name = "txtNomCours";
            this.txtNomCours.Size = new System.Drawing.Size(132, 23);
            this.txtNomCours.TabIndex = 5;
            this.txtNomCours.TextChanged += new System.EventHandler(this.txtNomCours_TextChanged);
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(22, 229);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(4);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(132, 23);
            this.txtDescription.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(37, 117);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 22);
            this.label1.TabIndex = 7;
            this.label1.Text = "Nom Cours ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(39, 197);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 22);
            this.label2.TabIndex = 8;
            this.label2.Text = "Description";
            // 
            // txtNomMatiere
            // 
            this.txtNomMatiere.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNomMatiere.Location = new System.Drawing.Point(580, 149);
            this.txtNomMatiere.Margin = new System.Windows.Forms.Padding(4);
            this.txtNomMatiere.Name = "txtNomMatiere";
            this.txtNomMatiere.Size = new System.Drawing.Size(132, 23);
            this.txtNomMatiere.TabIndex = 9;
            this.txtNomMatiere.TextChanged += new System.EventHandler(this.txtNomMatiere_TextChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(588, 117);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 22);
            this.label3.TabIndex = 10;
            this.label3.Text = "Nom Matiere ";
            // 
            // btnAssociateMatiere
            // 
            this.btnAssociateMatiere.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAssociateMatiere.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnAssociateMatiere.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAssociateMatiere.Location = new System.Drawing.Point(639, 406);
            this.btnAssociateMatiere.Margin = new System.Windows.Forms.Padding(4);
            this.btnAssociateMatiere.Name = "btnAssociateMatiere";
            this.btnAssociateMatiere.Size = new System.Drawing.Size(88, 26);
            this.btnAssociateMatiere.TabIndex = 11;
            this.btnAssociateMatiere.Text = "Associer";
            this.btnAssociateMatiere.UseVisualStyleBackColor = false;
            this.btnAssociateMatiere.Click += new System.EventHandler(this.btnAssociateMatiere_Click);
            // 
            // btnUpdateCours
            // 
            this.btnUpdateCours.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnUpdateCours.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnUpdateCours.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateCours.Location = new System.Drawing.Point(222, 288);
            this.btnUpdateCours.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpdateCours.Name = "btnUpdateCours";
            this.btnUpdateCours.Size = new System.Drawing.Size(86, 47);
            this.btnUpdateCours.TabIndex = 12;
            this.btnUpdateCours.Text = "Modifier Cours";
            this.btnUpdateCours.UseVisualStyleBackColor = false;
            this.btnUpdateCours.Click += new System.EventHandler(this.btnUpdateCours_Click);
            // 
            // btnDeleteCours
            // 
            this.btnDeleteCours.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnDeleteCours.BackColor = System.Drawing.Color.Red;
            this.btnDeleteCours.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteCours.Location = new System.Drawing.Point(347, 288);
            this.btnDeleteCours.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeleteCours.Name = "btnDeleteCours";
            this.btnDeleteCours.Size = new System.Drawing.Size(98, 47);
            this.btnDeleteCours.TabIndex = 13;
            this.btnDeleteCours.Text = "Supprimer Cours";
            this.btnDeleteCours.UseVisualStyleBackColor = false;
            this.btnDeleteCours.Click += new System.EventHandler(this.btnDeleteCours_Click);
            // 
            // btnDeleteMatiere
            // 
            this.btnDeleteMatiere.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnDeleteMatiere.BackColor = System.Drawing.Color.Red;
            this.btnDeleteMatiere.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteMatiere.Location = new System.Drawing.Point(905, 288);
            this.btnDeleteMatiere.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeleteMatiere.Name = "btnDeleteMatiere";
            this.btnDeleteMatiere.Size = new System.Drawing.Size(117, 48);
            this.btnDeleteMatiere.TabIndex = 14;
            this.btnDeleteMatiere.Text = "Supprimer Matiere";
            this.btnDeleteMatiere.UseVisualStyleBackColor = false;
            this.btnDeleteMatiere.Click += new System.EventHandler(this.btnDeleteMatiere_Click);
            // 
            // btnUpdateMatiere
            // 
            this.btnUpdateMatiere.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnUpdateMatiere.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnUpdateMatiere.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateMatiere.Location = new System.Drawing.Point(766, 287);
            this.btnUpdateMatiere.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpdateMatiere.Name = "btnUpdateMatiere";
            this.btnUpdateMatiere.Size = new System.Drawing.Size(100, 48);
            this.btnUpdateMatiere.TabIndex = 15;
            this.btnUpdateMatiere.Text = "Modifier Matieres";
            this.btnUpdateMatiere.UseVisualStyleBackColor = false;
            this.btnUpdateMatiere.Click += new System.EventHandler(this.btnUpdateMatiere_Click);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(415, 373);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(154, 22);
            this.label4.TabIndex = 16;
            this.label4.Text = "Classe à Associer";
            // 
            // CoursForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnUpdateMatiere);
            this.Controls.Add(this.btnDeleteMatiere);
            this.Controls.Add(this.btnDeleteCours);
            this.Controls.Add(this.btnUpdateCours);
            this.Controls.Add(this.btnAssociateMatiere);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtNomMatiere);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtNomCours);
            this.Controls.Add(this.btnAddMatieres);
            this.Controls.Add(this.btnAddCours);
            this.Controls.Add(this.cbClasse);
            this.Controls.Add(this.dataGridViewMatieres);
            this.Controls.Add(this.dataGridViewCours);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CoursForm";
            this.Text = "CoursForm";
            this.Load += new System.EventHandler(this.CoursForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCours)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMatieres)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewCours;
        private System.Windows.Forms.DataGridView dataGridViewMatieres;
        private System.Windows.Forms.ComboBox cbClasse;
        private System.Windows.Forms.Button btnAddCours;
        private System.Windows.Forms.Button btnAddMatieres;
        private System.Windows.Forms.TextBox txtNomCours;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNomMatiere;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAssociateMatiere;
        private System.Windows.Forms.Button btnUpdateCours;
        private System.Windows.Forms.Button btnDeleteCours;
        private System.Windows.Forms.Button btnDeleteMatiere;
        private System.Windows.Forms.Button btnUpdateMatiere;
        private System.Windows.Forms.Label label4;
    }
}