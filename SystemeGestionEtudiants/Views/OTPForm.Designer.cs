namespace SystemeGestionEtudiants.Views
{
    partial class OTPForm
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
            this.btnValidateOTP = new System.Windows.Forms.Button();
            this.txtOTP = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnValidateOTP
            // 
            this.btnValidateOTP.Location = new System.Drawing.Point(293, 134);
            this.btnValidateOTP.Name = "btnValidateOTP";
            this.btnValidateOTP.Size = new System.Drawing.Size(75, 23);
            this.btnValidateOTP.TabIndex = 0;
            this.btnValidateOTP.Text = "button1";
            this.btnValidateOTP.UseVisualStyleBackColor = true;
            this.btnValidateOTP.Click += new System.EventHandler(this.btnValidateOTP_Click);
            // 
            // txtOTP
            // 
            this.txtOTP.Location = new System.Drawing.Point(264, 101);
            this.txtOTP.Name = "txtOTP";
            this.txtOTP.Size = new System.Drawing.Size(100, 20);
            this.txtOTP.TabIndex = 1;
            // 
            // OTPForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtOTP);
            this.Controls.Add(this.btnValidateOTP);
            this.Name = "OTPForm";
            this.Text = "OTPFrom";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnValidateOTP;
        private System.Windows.Forms.TextBox txtOTP;
    }
}