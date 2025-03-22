using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SystemeGestionEtudiants.Services
{
    class SMSService
    {
        // Remplace ces variables par ton propre SID et token Twilio
        private const string AccountSid = "AC781f48f34a29310022b2662bce90c078";  // Ton SID Twilio
        private const string AuthToken = "54b87c74d08cbdded3916be69093f7e6";    // Ton AuthToken Twilio

        public void SendOTP(string phoneNumber, string otp)
        {
            try
            {
                // Vérifie si le numéro de téléphone est valide
                if (string.IsNullOrWhiteSpace(phoneNumber) || !Regex.IsMatch(phoneNumber, @"^\+221\d{9}$"))
                {
                    throw new ArgumentException("Le numéro de téléphone doit commencer par +221 et contenir 9 chiffres.");
                }

                // Initialise le client Twilio
                TwilioClient.Init(AccountSid, AuthToken);

                // Envoi du message
                var message = MessageResource.Create(
                    body: $"Votre code OTP est : {otp}",
                    from: new PhoneNumber("+221763401076"), // Remplace par ton numéro Twilio
                    to: new PhoneNumber(phoneNumber)
                );

                // Tu peux afficher le SID du message pour vérifier qu'il a été envoyé avec succès
                MessageBox.Show($"Message envoyé avec succès, SID : {message.Sid}", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (ArgumentException ex)
            {
                // Affiche une erreur si le numéro de téléphone n'est pas valide
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur de saisie", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Twilio.Exceptions.ApiException ex)
            {
                // Affiche une erreur spécifique à l'API Twilio
                MessageBox.Show($"Erreur Twilio : {ex.Message}", "Erreur API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Affiche une erreur générique pour toute autre exception
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
