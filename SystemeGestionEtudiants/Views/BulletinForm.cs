using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using SystemeGestionEtudiants.Data;
using SystemeGestionEtudiants.Models;
using Font = iTextSharp.text.Font;
using System.Diagnostics;


namespace SystemeGestionEtudiants.Views
{
    public partial class BulletinForm : MaterialForm
    {
        private ApplicationDbContext _context;

        public BulletinForm()
        {
            InitializeComponent();
            _context = new ApplicationDbContext();

            // Configuration du MaterialSkinManager
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            // Utiliser un bleu plus sombre
            skinManager.ColorScheme = new ColorScheme(
                Primary.Blue800,    // Couleur principale plus sombre
                Primary.Blue900,    // Couleur principale encore plus sombre
                Primary.Blue400,    // Couleur secondaire (plus claire)
                Accent.Blue200,     // Accent (bleu clair)
                TextShade.WHITE     // Couleur du texte (blanc)
            );

            // Charger les classes disponibles
            LoadClasses();

            // Rendre les ComboBox en lecture seule
            cbClasse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEtudiant.DropDownStyle = ComboBoxStyle.DropDownList;

            // Abonnement aux événements
            cbClasse.SelectedIndexChanged += cbClasse_SelectedIndexChanged;
            btnGenererBulletin.Click += btnGenererBulletin_Click;
            btnTelechargerBulletin.Click += btnTelechargerBulletin_Click;
        }

        private void LoadClasses()
        {
            try
            {
                var classes = _context.Classes.ToList();
                classes.Insert(0, new Classe { Id = 0, NomClasse = "Sélectionnez une classe" });
                cbClasse.DataSource = classes;
                cbClasse.DisplayMember = "NomClasse";
                cbClasse.ValueMember = "Id";
                cbClasse.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

        private void LoadEtudiants(int classeId)
        {
            try
            {
                var etudiants = _context.Etudiant
                    .Where(e => e.ClasseId == classeId)
                    .Select(e => new { Id = e.Id, NomComplet = e.Nom + " " + e.Prenom })
                    .ToList();

                etudiants.Insert(0, new { Id = 0, NomComplet = "Sélectionnez un étudiant" });
                cmbEtudiant.DataSource = etudiants;
                cmbEtudiant.DisplayMember = "NomComplet";
                cmbEtudiant.ValueMember = "Id";
                cmbEtudiant.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void GenererBulletin(int studentId)
        {
            var etudiant = _context.Etudiant.Find(studentId);
            var notes = _context.Notes
                .Where(n => n.StudentId == studentId)
                .Select(n => new NoteBulletin { Matiere = n.Matiere.NomMatiere, Note = n.NoteValue })
                .ToList();

            // Calculer la moyenne, le rang, etc.
            double moyenne = notes.Any() ? notes.Average(n => n.Note) : 0;
            string appreciation = GetAppreciation(moyenne);

            // Afficher le bulletin dans le Panel
            AfficherBulletin(etudiant, notes, moyenne, appreciation);
        }

        private void AfficherBulletin(Etudiants etudiant, List<NoteBulletin> notes, double moyenne, string appreciation)
        {
            panelBulletin.Controls.Clear();

            // En-tête
            var lblTitre = new Label
            {
                Text = "Bulletin Scolaire",
                Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold), // Utiliser System.Drawing.Font
                Location = new System.Drawing.Point(10, 10),
                AutoSize = true
            };
            panelBulletin.Controls.Add(lblTitre);

            // Informations de l'étudiant
            var lblInfos = new Label
            {
                Text = $"Nom: {etudiant.Nom}\nPrénom: {etudiant.Prenom}\nMatricule: {etudiant.Matricule}\nClasse: {etudiant.Classe.NomClasse}",
                Location = new Point(10, 50),
                AutoSize = true
            };
            panelBulletin.Controls.Add(lblInfos);

            // Notes
            int yOffset = 100;
            foreach (var note in notes)
            {
                var lblNote = new Label
                {
                    Text = $"{note.Matiere}: {note.Note}",
                    Location = new Point(10, yOffset),
                    AutoSize = true
                };
                panelBulletin.Controls.Add(lblNote);
                yOffset += 20;
            }

            // Moyenne et appréciation
            var lblMoyenne = new Label
            {
                Text = $"Moyenne: {moyenne:F2}\nAppréciation: {appreciation}",
                Location = new Point(10, yOffset + 20),
                AutoSize = true
            };
            panelBulletin.Controls.Add(lblMoyenne);
        }
        private string GetAppreciation(double moyenne)
        {
            if (moyenne >= 16) return "Excellent";
            if (moyenne >= 14) return "Très bien";
            if (moyenne >= 12) return "Bien";
            if (moyenne >= 10) return "Assez bien";
            return "Insuffisant";
        }

        private void OuvrirFichier(string filePath)
        {
            try
            {
                // Vérifier si le fichier existe
                if (File.Exists(filePath))
                {
                    // Ouvrir le fichier avec l'application par défaut
                    Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                }
                else
                {
                    MessageBox.Show("Le fichier n'a pas été trouvé.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture du fichier : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void GenererExcel(int studentId)
        {
            var etudiant = _context.Etudiant.Find(studentId);
            var notes = _context.Notes
                .Where(n => n.StudentId == studentId)
                .Select(n => new NoteBulletin { Matiere = n.Matiere.NomMatiere, Note = n.NoteValue })
                .ToList();

            // Calculer la moyenne
            double moyenne = notes.Any() ? notes.Average(n => n.Note) : 0;
            string appreciation = GetAppreciation(moyenne);

            // Demander à l'utilisateur de choisir l'emplacement du fichier
            string defaultFileName = $"Bulletin_{etudiant.Nom}_{etudiant.Prenom}.xlsx";
            string filter = "Fichiers Excel (*.xlsx)|*.xlsx";
            string filePath = ChoisirEmplacementFichier(defaultFileName, filter);

            // Vérifier si l'utilisateur a choisi un emplacement
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Aucun emplacement sélectionné. Le fichier n'a pas été enregistré.", "Annulé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Créer un fichier Excel
            FileInfo file = new FileInfo(filePath);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Bulletin");

                // Ajouter le titre
                worksheet.Cells[1, 1].Value = "Bulletin Scolaire";
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.Font.Size = 16;

                // Ajouter les informations de l'étudiant
                worksheet.Cells[3, 1].Value = "Nom";
                worksheet.Cells[3, 2].Value = etudiant.Nom;
                worksheet.Cells[4, 1].Value = "Prénom";
                worksheet.Cells[4, 2].Value = etudiant.Prenom;
                worksheet.Cells[5, 1].Value = "Matricule";
                worksheet.Cells[5, 2].Value = etudiant.Matricule;
                worksheet.Cells[6, 1].Value = "Classe";
                worksheet.Cells[6, 2].Value = etudiant.Classe.NomClasse;

                // Ajouter les notes
                worksheet.Cells[8, 1].Value = "Matière";
                worksheet.Cells[8, 2].Value = "Note";
                int row = 9;
                foreach (var note in notes)
                {
                    worksheet.Cells[row, 1].Value = note.Matiere;
                    worksheet.Cells[row, 2].Value = note.Note;
                    row++;
                }

                // Ajouter la moyenne et l'appréciation
                worksheet.Cells[row + 1, 1].Value = "Moyenne";
                worksheet.Cells[row + 1, 2].Value = moyenne.ToString("F2");
                worksheet.Cells[row + 2, 1].Value = "Appréciation";
                worksheet.Cells[row + 2, 2].Value = appreciation;

                // Ajouter un message en bas de page
                worksheet.Cells[row + 4, 1].Value = "Le nom de l'institut, ou le bulletin ne sera délivré qu'une seule fois.";

                package.Save();
            }

            // Ouvrir le fichier après génération
            OuvrirFichier(filePath);

            MessageBox.Show($"Bulletin généré avec succès : {filePath}", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnGenererBulletin_Click(object sender, EventArgs e)
        {
            if (cmbEtudiant.SelectedValue == null || (int)cmbEtudiant.SelectedValue == 0)
            {
                MessageBox.Show("Veuillez sélectionner un étudiant.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int studentId = (int)cmbEtudiant.SelectedValue;
            GenererBulletin(studentId);
        }
        /*private void GenererPDF(int studentId)
        {
            var etudiant = _context.Etudiant.Find(studentId);
            var notes = _context.Notes
                .Where(n => n.StudentId == studentId)
                .Select(n => new NoteBulletin { Matiere = n.Matiere.NomMatiere, Note = n.NoteValue })
                .ToList();

            // Calculer la moyenne
            double moyenne = notes.Any() ? notes.Average(n => n.Note) : 0;
            string appreciation = GetAppreciation(moyenne);

            // Créer un fichier PDF
            string filePath = $"Bulletin_{etudiant.Nom}_{etudiant.Prenom}.pdf";
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // Ajouter le titre
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                doc.Add(new Paragraph("Bulletin Scolaire", titleFont));

                // Ajouter les informations de l'étudiant
                doc.Add(new Paragraph($"Nom: {etudiant.Nom}"));
                doc.Add(new Paragraph($"Prénom: {etudiant.Prenom}"));
                doc.Add(new Paragraph($"Matricule: {etudiant.Matricule}"));
                doc.Add(new Paragraph($"Classe: {etudiant.Classe.NomClasse}"));

                // Ajouter les notes
                PdfPTable table = new PdfPTable(2);
                table.AddCell("Matière");
                table.AddCell("Note");
                foreach (var note in notes)
                {
                    table.AddCell(note.Matiere);
                    table.AddCell(note.Note.ToString("F2"));
                }
                doc.Add(table);

                // Ajouter la moyenne et l'appréciation
                doc.Add(new Paragraph($"Moyenne: {moyenne:F2}"));
                doc.Add(new Paragraph($"Appréciation: {appreciation}"));

                // Ajouter un message en bas de page
                doc.Add(new Paragraph("Le nom de l'institut, ou le bulletin ne sera délivré qu'une seule fois."));

                doc.Close();
            }

            MessageBox.Show($"Bulletin généré avec succès : {filePath}", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }*/
        private string ChoisirEmplacementFichier(string defaultFileName, string filter)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                // Définir le nom de fichier par défaut
                saveFileDialog.FileName = defaultFileName;

                // Définir le filtre pour les types de fichiers
                saveFileDialog.Filter = filter;

                // Afficher la boîte de dialogue
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Retourner le chemin complet du fichier choisi
                    return saveFileDialog.FileName;
                }
            }

            // Retourner null si l'utilisateur annule
            return null;
        }
        private void GenererPDF(int studentId)
        {
            var etudiant = _context.Etudiant.Find(studentId);
            var notes = _context.Notes
                .Where(n => n.StudentId == studentId)
                .Select(n => new NoteBulletin { Matiere = n.Matiere.NomMatiere, Note = n.NoteValue })
                .ToList();

            // Calculer la moyenne
            double moyenne = notes.Any() ? notes.Average(n => n.Note) : 0;
            string appreciation = GetAppreciation(moyenne);

            // Demander à l'utilisateur de choisir l'emplacement du fichier
            string defaultFileName = $"Bulletin_{etudiant.Nom}_{etudiant.Prenom}.pdf";
            string filter = "Fichiers PDF (*.pdf)|*.pdf";
            string filePath = ChoisirEmplacementFichier(defaultFileName, filter);

            // Vérifier si l'utilisateur a choisi un emplacement
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Aucun emplacement sélectionné. Le fichier n'a pas été enregistré.", "Annulé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Créer un fichier PDF
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // Ajouter le titre
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                doc.Add(new Paragraph("Bulletin Scolaire", titleFont));

                // Ajouter les informations de l'étudiant
                doc.Add(new Paragraph($"Nom: {etudiant.Nom}"));
                doc.Add(new Paragraph($"Prénom: {etudiant.Prenom}"));
                doc.Add(new Paragraph($"Matricule: {etudiant.Matricule}"));
                doc.Add(new Paragraph($"Classe: {etudiant.Classe.NomClasse}"));

                // Ajouter les notes
                PdfPTable table = new PdfPTable(2);
                table.AddCell("Matière");
                table.AddCell("Note");
                foreach (var note in notes)
                {
                    table.AddCell(note.Matiere);
                    table.AddCell(note.Note.ToString("F2"));
                }
                doc.Add(table);

                // Ajouter la moyenne et l'appréciation
                doc.Add(new Paragraph($"Moyenne: {moyenne:F2}"));
                doc.Add(new Paragraph($"Appréciation: {appreciation}"));

                // Ajouter un message en bas de page
                doc.Add(new Paragraph("Le nom de l'institut, ou le bulletin ne sera délivré qu'une seule fois."));

                doc.Close();
            }

            // Ouvrir le fichier après génération
            OuvrirFichier(filePath);

            MessageBox.Show($"Bulletin généré avec succès : {filePath}", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnTelechargerBulletin_Click(object sender, EventArgs e)
        {
            if (cmbEtudiant.SelectedValue == null || (int)cmbEtudiant.SelectedValue == 0)
            {
                MessageBox.Show("Veuillez sélectionner un étudiant.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int studentId = (int)cmbEtudiant.SelectedValue;
            GenererPDF(studentId);
        }

        private void cbClasse_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Vérifier si une classe est sélectionnée
            if (cbClasse.SelectedValue != null && int.TryParse(cbClasse.SelectedValue.ToString(), out int classeId) && classeId != 0)
            {
                LoadEtudiants(classeId); // Charger les étudiants de la classe sélectionnée
            }
            else
            {
                // Si aucune classe n'est sélectionnée, vider le ComboBox des étudiants
                cmbEtudiant.DataSource = null;
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (cmbEtudiant.SelectedValue == null || (int)cmbEtudiant.SelectedValue == 0)
            {
                MessageBox.Show("Veuillez sélectionner un étudiant.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int studentId = (int)cmbEtudiant.SelectedValue;
            GenererExcel(studentId);
        }
    }
    public class NoteBulletin
    {
        public string Matiere { get; set; }
        public float Note { get; set; }
    }
}
