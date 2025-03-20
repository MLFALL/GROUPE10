using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemeGestionEtudiants.Models
{
     public class Note
    {
        private int _id;
        private int _studentId;
        private int _matiereId;
        private float _noteValue;

        [Key]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int StudentId
        {
            get { return _studentId; }
            set
            {
                /*if (value <= 0)
                    throw new ArgumentException("L'ID de l'étudiant doit être un nombre positif.");*/
                _studentId = value;
            }
        }

        public int MatiereId
        {
            get { return _matiereId; }
            set
            {
               /* if (value <= 0)
                    throw new ArgumentException("L'ID de la matière doit être un nombre positif.");*/
                _matiereId = value;
            }
        }

        public float NoteValue
        {
            get { return _noteValue; }
            set
            {
                /*if (value < 0 || value > 20)
                    throw new ArgumentException("La note doit être comprise entre 0 et 20.");*/
                _noteValue = value;
            }
        }

        public Etudiants Etudiants { get; set; }
        public Matiere Matiere { get; set; }
    }

}

