using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemeGestionEtudiants.Models
{
    public class Cours
    {
        private int _id;
        private string _nomCours;
        private string _description;

        [Key]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string NomCours
        {
            get { return _nomCours; }
            set
            {
                /*if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Le nom du cours ne peut pas être vide.");
                if (value.Length > 100)
                    throw new ArgumentException("Le nom du cours ne peut pas dépasser 100 caractères.");*/
                _nomCours = value;
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (value.Length > 500)
                    throw new ArgumentException("La description ne peut pas dépasser 500 caractères.");
                _description = value;
            }
        }

        public ICollection<ClasseCours> ClasseCours { get; set; } = new List<ClasseCours>();

        public ICollection<CoursMatiere> CoursMatieres { get; set; } = new List<CoursMatiere>();
    }

    public class CoursMatiere
    {
        public int CoursId { get; set; }
        public int MatiereId { get; set; }

        // Propriétés de navigation pour les relations avec d'autres entités
        public virtual Cours Cour { get; set; }
        public virtual Matiere Matiere { get; set; }
    }
}
