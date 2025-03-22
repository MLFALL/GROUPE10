using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemeGestionEtudiants.Models
{
    public class OTPCode
    {
        private int _id;
        private int _utilisateurId;
        private string _code;
        private DateTime _dateExpiration;

        [Key]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int UtilisateurId
        {
            get { return _utilisateurId; }
            set
            {
               /* if (value <= 0)
                    throw new ArgumentException("L'ID de l'utilisateur doit être un nombre positif.");*/
                _utilisateurId = value;
            }
        }

        public string Code
        {
            get { return _code; }
            set
            {
                /*if (string.IsNullOrWhiteSpace(value) || value.Length != 6)
                    throw new ArgumentException("Le code OTP doit contenir exactement 6 caractères.");*/
                _code = value;
            }
        }

        public DateTime DateExpiration
        {
            get { return _dateExpiration; }
            set
            {
               /* if (value < DateTime.Now)
                    throw new ArgumentException("La date d'expiration ne peut pas être dans le passé.");*/
                _dateExpiration = value;
            }
        }

        public Utilisateur Utilisateur { get; set; }
    }

}
