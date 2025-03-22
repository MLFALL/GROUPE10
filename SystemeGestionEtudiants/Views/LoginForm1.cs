using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Linq;
using System.Windows.Forms;
using SystemeGestionEtudiants.Data;
using SystemeGestionEtudiants.Models;
using BCrypt.Net;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SystemeGestionEtudiants.Views
{
    public partial class LoginForm1 : MaterialForm
    {
        private ApplicationDbContext _context;

        public LoginForm1()
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
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

                // Vérifier le mot de passe haché
                if (!BCrypt.Net.BCrypt.Verify(motDePasse, utilisateur.MotDePasse))
                {
                    MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Générer un code OTP
                string codeOTP = GenerateOTP();

                // Enregistrer le code OTP dans la base de données
                SaveOTP(utilisateur.Id, codeOTP);

                // Envoyer le code OTP par SMS
                SendOTP(utilisateur.Telephone, codeOTP);

                
            }
            catch (Exception ex)
            {
                // Gestion des erreurs
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Code à 6 chiffres
        }

        private void SaveOTP(int userId, string codeOTP)
        {
            try
            {
                var otpCode = new OTPCode
                {
                    UtilisateurId = userId,
                    Code = codeOTP,
                    DateExpiration = DateTime.Now.AddMinutes(5) // Expire dans 5 minutes
                };

                _context.OTPCodes.Add(otpCode);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'enregistrement du code OTP : " + ex.Message);
            }
        }

        private void SendOTP(string telephone, string codeOTP)
        {
            try
            {
                // Vérifier et formater le numéro de téléphone
                string tel = FormatPhoneNumber(telephone);

                // Utiliser des valeurs codées en dur pour tester
                string accountSid = "ACadd361a45d09c4c2bbfdb7f47a544bf5"; // Remplacez par votre Account SID
                string authToken = "369b84f97007dc72dbfb56f3aeca5fa4";   // Remplacez par votre Auth Token

                // Envoyer le SMS
                var message = MessageResource.Create(
                    body: $"Votre code OTP est : {codeOTP}",
                    from: new PhoneNumber("+221763401076"), // Votre numéro Twilio
                    to: new PhoneNumber(tel) // Numéro de l'utilisateur
                );

                MessageBox.Show("Code OTP envoyé avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Afficher un message d'erreur clair
                MessageBox.Show($"Erreur lors de l'envoi du SMS : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string FormatPhoneNumber(string telephone)
        {
            // Supprimer les espaces et les caractères non numériques
            telephone = new string(telephone.Where(char.IsDigit).ToArray());

            // Ajouter l'indicatif pays si nécessaire
            if (!telephone.StartsWith("+"))
            {
                telephone = "+221" + telephone; // Assurez-vous que c'est l'indicatif correct pour le Sénégal
            }

            // Vérifier la longueur du numéro
            if (telephone.Length < 9)
            {
                throw new Exception("Le numéro de téléphone est trop court.");
            }

            return telephone;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Fermer le formulaire de connexion
            this.Close();
        }
    }
}