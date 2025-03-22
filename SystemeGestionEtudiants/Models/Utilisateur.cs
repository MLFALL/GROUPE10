using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SystemeGestionEtudiants.Models
{
    public class Utilisateur
    {
        private int _id;
        private string _nomUtilisateur;
        private string _motDePasse;
        private string _role;
        private string _telephone;

        [Key]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string NomUtilisateur
        {
            get { return _nomUtilisateur; }
            set
            {
                /*if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Le nom d'utilisateur ne peut pas être vide.");
                if (value.Length > 50)
                    throw new ArgumentException("Le nom d'utilisateur ne peut pas dépasser 50 caractères.");*/
                _nomUtilisateur = value;
            }
        }

        public string MotDePasse
        {
            get { return _motDePasse; }
            set
            {
                /*if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Le mot de passe ne peut pas être vide.");
                if (value.Length < 8)
                    throw new ArgumentException("Le mot de passe doit contenir au moins 8 caractères.");*/
                _motDePasse = value;
            }
        }

        public string Role
        {
            get { return _role; }
            set
            {
                /*if (string.IsNullOrWhiteSpace(value) || (value != "Administrateur" && value != "DE" && value != "Agent" && value != "Professeur"))
                    throw new ArgumentException("Le rôle doit être 'Administrateur', 'DE', 'Agent' ou 'Professeur'.");*/
                _role = value;
            }
        }

        public string Telephone
        {
            get { return _telephone; }
            set
            {
                //if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, @"^\d{9}$"))
                    //throw new ArgumentException("Le numéro de téléphone doit contenir 9 chiffres.");
                _telephone = value;
            }
        }

        public ICollection<OTPCode> OTPCodes { get; set; } = new List<OTPCode>();
    }

}

