using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemeGestionEtudiants.Models
{
    public class Classe
    {
        private int _id;
        private string _nomClasse;

        [Key]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string NomClasse
        {
            get { return _nomClasse; }
            set
            {
               /* if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Le nom de la classe ne peut pas être vide.");
                if (value.Length > 50)
                    throw new ArgumentException("Le nom de la classe ne peut pas dépasser 50 caractères.");*/
                _nomClasse = value;
            }
        }
        // Relation avec les étudiants
        public ICollection<Etudiants> Etudiant { get; set; } = new List<Etudiants>();

        // Relation avec les cours via ClasseCours

        public ICollection<ClasseCours> ClasseCours { get; set; } = new List<ClasseCours>();

        // Relation avec les professeurs via ProfesseurClasse
        public ICollection<ProfesseurClasse> ProfesseurClasses { get; set; } = new List<ProfesseurClasse>();
    }

    public class ClasseCours
    {
        public int ClasseId { get; set; }  
        public int CoursId { get; set; }  

        // Propriétés de navigation pour les relations avec d'autres entités
        public virtual Classe Classe { get; set; }
        public virtual Cours Cours { get; set; }
    }

}
