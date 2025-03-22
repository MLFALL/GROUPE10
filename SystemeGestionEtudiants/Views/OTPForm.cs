using System;
using System.Linq;
using System.Windows.Forms;
using SystemeGestionEtudiants.Data;

namespace SystemeGestionEtudiants.Views
{
    public partial class OTPForm : Form
    {
        private int _userId;
        private ApplicationDbContext _context;

        public OTPForm(int userId)
        {
            InitializeComponent();
            _userId = userId;
            _context = new ApplicationDbContext();
        }

        private void btnValidateOTP_Click(object sender, EventArgs e)
        {
            string otp = txtOTP.Text;

            // Valider le code OTP
            var otpCode = _context.OTPCodes
                .FirstOrDefault(o => o.UtilisateurId == _userId && o.Code == otp && o.DateExpiration > DateTime.Now);

            if (otpCode != null)
            {
                this.DialogResult = DialogResult.OK; // Connexion réussie
                this.Close();
            }
            else
            {
                MessageBox.Show("Code OTP invalide ou expiré.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}