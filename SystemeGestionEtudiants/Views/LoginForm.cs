using MaterialSkin;
using MaterialSkin.Controls;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemeGestionEtudiants.Data;

namespace SystemeGestionEtudiants.Views
{
    public partial class LoginForm: MaterialForm
    {
        public string UserRole { get; private set; } // Propriété pour stocker le rôle de l'utilisateur
        private ApplicationDbContext _context;
        public LoginForm()
        {
            InitializeComponent();
            _context = new ApplicationDbContext();
            // Configuration du MaterialSkinManager
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(
                Primary.Blue800,    // Couleur principale
                Primary.Blue900,    // Couleur principale plus sombre
                Primary.Blue400,    // Couleur secondaire
                Accent.Blue200,     // Accent
                TextShade.WHITE);  // Couleur du texte
        }

        private void login_showPass_CheckedChanged(object sender, EventArgs e)
        {
            if (login_showPass.Checked)
            {
                txtMotDePasse.PasswordChar = '\0';
            }
            else
            {
                txtMotDePasse.PasswordChar = '*';
            }
        }

        private void login_btn_Click(object sender, EventArgs e)
        {
            string nomUtilisateur = txtNomUtilisateur.Text;
            string motDePasse = txtMotDePasse.Text;

            // Valider les champs
            if (string.IsNullOrWhiteSpace(nomUtilisateur) || string.IsNullOrWhiteSpace(motDePasse))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Récupérer l'utilisateur par nom d'utilisateur
            var utilisateur = _context.Utilisateurs
                .FirstOrDefault(u => u.NomUtilisateur == nomUtilisateur);

            if (utilisateur == null)
            {
                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Valider le mot de passe avec BCrypt
            if (!BCrypt.Net.BCrypt.Verify(motDePasse, utilisateur.MotDePasse))
            {
                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Envoyer un code A2F par SMS
            string codeA2F = GenererCodeA2F();
            EnvoyerCodeParSMS(utilisateur.Telephone, codeA2F);

            // Afficher le formulaire de vérification A2F
            var a2fForm = new A2FForm(codeA2F, utilisateur);
            if (a2fForm.ShowDialog() == DialogResult.OK)
            {
                // Si la vérification A2F réussit, fermer la LoginForm avec DialogResult.OK
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
            private string GenererCodeA2F()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Code à 6 chiffres
        }

        private void EnvoyerCodeParSMS(string telephone, string code)
        {
            // Implémentez l'envoi de SMS ici (utilisez une API comme Twilio)
            MessageBox.Show($"Code A2F envoyé à {telephone}: {code}", "A2F", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Fermer_Paint(object sender, PaintEventArgs e)
        {
            //Application.Exit();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

