using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace SystemeGestionEtudiants.Models
{
    public class Etudiants
    {
        // Champs privés pour l'encapsulation
        private int _id;
        private string _matricule;
        private string _nom;
        private string _prenom;
        private DateTime _dateNaissance;
        private string _sexe;
        private string _adresse;
        private string _telephone;
        private string _email;
        private int _classeId;
        private Classe _classe;
        private ICollection<Note> _notes;

        // Propriétés publiques avec contrôle de saisie
        [Key]
        public int Id
        {
            get { return _id; }
             set { _id = value; } // Setter privé pour empêcher la modification externe
        }

        public string Matricule
        {
            get { return _matricule; }
            set
            {
                _matricule = value;  // Le matricule ne peut pas être modifié après génération
            }
        }

        public string Nom
        {
            get { return _nom; }
            set
            {
                /*if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Le nom ne peut pas être vide.");
                if (value.Length > 50)
                    throw new ArgumentException("Le nom ne peut pas dépasser 50 caractères.");*/
                _nom = value;
            }
        }

        public string Prenom
        {
            get { return _prenom; }
            set
            {
                /*if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Le prénom ne peut pas être vide.");
                if (value.Length > 50)
                    throw new ArgumentException("Le prénom ne peut pas dépasser 50 caractères.");*/
                _prenom = value;
            }
        }

        public DateTime DateNaissance
        {
            get { return _dateNaissance; }
            set
            {
                /*if (value > DateTime.Now)
                    throw new ArgumentException("La date de naissance ne peut pas être dans le futur.");*/
                _dateNaissance = value;
            }
        }

        public string Sexe
        {
            get { return _sexe; }
            set
            {
               /* if (string.IsNullOrWhiteSpace(value) || (value != "M" && value != "F"))
                    throw new ArgumentException("Le sexe doit être 'M' ou 'F'.");*/
                _sexe = value;
            }
        }

        public string Adresse
        {
            get { return _adresse; }
            set
            {
                /*if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("L'adresse ne peut pas être vide.");*/
                _adresse = value;
            }
        }

        public string Telephone
        {
            get { return _telephone; }
            set
            {
                /*if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, @"^\d{9}$"))
                    throw new ArgumentException("Le numéro de téléphone doit contenir 10 chiffres.");*/
                _telephone = value;
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                /*if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new ArgumentException("L'email n'est pas valide.");*/
                _email = value;
            }
        }

        public int ClasseId
        {
            get { return _classeId; }
            set
            {
                /*if (value <= 0)
                    throw new ArgumentException("L'ID de la classe doit être un nombre positif.");*/
                _classeId = value;
            }
        }

        public Classe Classe
        {
            get { return _classe; }
            set
            {
               // if (value == null)
                    //throw new ArgumentException("La classe ne peut pas être nulle.");
                _classe = value;
            }
        }

        public ICollection<Note> Notes
        {
            get { return _notes; }
            set
            {
                /*if (value == null)
                    throw new ArgumentException("La collection de notes ne peut pas être nulle.");*/
                _notes = value;
            }
        }

        // Méthode pour générer un matricule unique basé sur la date du système, y compris les secondes
        private string GenererMatricule()
        {
            // Générer le matricule sous la forme "ETU20250307123045"
            string matricule = "ETU" + DateTime.Now.ToString("yyyyMMddHHmmss");  // Format : ETU+AAAA-MM-JJ-HH-MM-SS
            return matricule;
        }
        // Constructeur pour initialiser les valeurs par défaut
        public Etudiants()
        {
            _notes = new List<Note>();
            _matricule = GenererMatricule();  // Générer automatiquement le matricule à la création
        }
    }
}
