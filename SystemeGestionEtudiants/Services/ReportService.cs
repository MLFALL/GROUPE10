using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SystemeGestionEtudiants.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace SystemeGestionEtudiants.Services
{
    class ReportService
    {
        public void GenerateReport(Etudiants etudiant)
        {
            Document document = new Document();
            PdfWriter.GetInstance(document, new FileStream("ReleveNotes.pdf", FileMode.Create));
            document.Open();

            document.Add(new Paragraph($"Relevé de notes pour {etudiant.Nom} {etudiant.Prenom}"));
            document.Add(new Paragraph($"Matricule : {etudiant.Matricule}"));
            document.Add(new Paragraph($"Classe : {etudiant.Classe.NomClasse}"));

            foreach (var note in etudiant.Notes)
            {
                document.Add(new Paragraph($"{note.Matiere.NomMatiere} : {note.NoteValue}"));
            }

            document.Close();
        }

    }
}
