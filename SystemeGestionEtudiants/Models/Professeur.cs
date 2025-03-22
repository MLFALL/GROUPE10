using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SystemeGestionEtudiants.Models
{
    public class Professeur
    {
        private int _id;
        private string _nom;
        private string _prenom;
        private string _email;
        private string _telephone;

        [Key]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
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
               /* if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Le prénom ne peut pas être vide.");
                if (value.Length > 50)
                    throw new ArgumentException("Le prénom ne peut pas dépasser 50 caractères.");*/
                _prenom = value;
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

        public string Telephone
        {
            get { return _telephone; }
            set
            {
               // if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, @"^\d{9}$"))
                 //   throw new ArgumentException("Le numéro de téléphone doit contenir 10 chiffres.");
                _telephone = value;
            }
        }

        public ICollection<ProfesseurMatiere> ProfesseurMatieres { get; set; } = new List<ProfesseurMatiere>();
        public ICollection<ProfesseurClasse> ProfesseurClasses { get; set; } = new List<ProfesseurClasse>();
    }

    public class ProfesseurClasse
    {
        public int ProfesseurId { get; set; }
        public int ClasseId { get; set; }

        // Propriétés de navigation pour les relations avec d'autres entités
        public virtual Professeur Professeur { get; set; }
        public virtual Classe Classe { get; set; }
    }
}
