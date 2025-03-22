using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemeGestionEtudiants.Models
{
    public class Matiere
    {
       
        
            private int _id;
            private string _nomMatiere;

        [Key]
        public int Id
            {
                get { return _id; }
                set { _id = value; }
            }

            public string NomMatiere
            {
                get { return _nomMatiere; }
                set
                {
                    /*if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentException("Le nom de la matière ne peut pas être vide.");
                    if (value.Length > 100)
                        throw new ArgumentException("Le nom de la matière ne peut pas dépasser 100 caractères.");*/
                    _nomMatiere = value;
                }
            }

            public ICollection<CoursMatiere> CoursMatieres { get; set; } = new List<CoursMatiere>();
            public ICollection<ProfesseurMatiere> ProfesseurMatieres { get; set; } = new List<ProfesseurMatiere>();

    }

    public class ProfesseurMatiere
    {
        public int ProfesseurId { get; set; }
        public int MatiereId { get; set; }

        // Propriétés de navigation pour les relations avec d'autres entités
        public virtual Professeur Professeur { get; set; }
        public virtual Matiere Matiere { get; set; }
    }
}
