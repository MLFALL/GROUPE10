using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using SystemeGestionEtudiants.Data;
using SystemeGestionEtudiants.Models;
using Font = iTextSharp.text.Font;
using System.Diagnostics;
using MaterialSkin;
using MaterialSkin.Controls;

namespace SystemeGestionEtudiants.Views
{
    public partial class MeilleursEtudiantsForm : MaterialForm
    {
        private ApplicationDbContext _context;

        public MeilleursEtudiantsForm()
        {
            InitializeComponent();
            _context = new ApplicationDbContext();

            // Configuration du MaterialSkinManager (si vous utilisez MaterialSkin pour le thème)
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

            // Configurer le DataGridView
            dataGridViewMeilleursEleves.AutoGenerateColumns = false;
            dataGridViewMeilleursEleves.Columns.Add("Matricule", "Matricule");
            dataGridViewMeilleursEleves.Columns.Add("Nom", "Nom");
            dataGridViewMeilleursEleves.Columns.Add("Prenom", "Prénom");
            dataGridViewMeilleursEleves.Columns.Add("DateNaissance", "Date de Naissance");
            dataGridViewMeilleursEleves.Columns.Add("Classe", "Classe");
            dataGridViewMeilleursEleves.Columns.Add("Moyenne", "Moyenne");

            // Ajouter les boutons pour générer les fichiers (en utilisant Button standard)
            var btnGenererPDF = new Button
            {
                Text = "Générer PDF",
                BackColor = System.Drawing.Color.DodgerBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                Cursor = Cursors.Hand,
                Size = new System.Drawing.Size(150, 40),
                Location = new System.Drawing.Point(50, 300)
            };
            btnGenererPDF.FlatAppearance.BorderSize = 0; // Supprimer la bordure
            btnGenererPDF.Click += btnGenererPDF_Click;
            this.Controls.Add(btnGenererPDF);

            var btnGenererExcel = new Button
            {
                Text = "Générer Excel",
                BackColor = System.Drawing.Color.DodgerBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                Cursor = Cursors.Hand,
                Size = new System.Drawing.Size(150, 40),
                Location = new System.Drawing.Point(220, 300)
            };
            btnGenererExcel.FlatAppearance.BorderSize = 0; // Supprimer la bordure
            btnGenererExcel.Click += btnGenererExcel_Click;
            this.Controls.Add(btnGenererExcel);

            // Charger les données dans le DataGridView
            ChargerDonnees();
        }
        //Récupération des notes
        private void ChargerDonnees()
        {
            var meilleursEleves = GetMeilleursElevesParClasse();
            dataGridViewMeilleursEleves.Rows.Clear();

            foreach (var etudiant in meilleursEleves)
            {
                var notes = _context.Notes
                    .Where(n => n.StudentId == etudiant.Id)
                    .Select(n => n.NoteValue)
                    .ToList();

                double moyenne = notes.Any() ? notes.Average() : 0; // Si pas de notes, moyenne = 0

                dataGridViewMeilleursEleves.Rows.Add(
                    etudiant.Matricule,
                    etudiant.Nom,
                    etudiant.Prenom,
                    etudiant.DateNaissance.ToShortDateString(),
                    etudiant.Classe.NomClasse,
                    moyenne.ToString("F2")
                );
            }
        }

        // Méthode pour obtenir les meilleurs élèves de chaque classe
        private List<Etudiants> GetMeilleursElevesParClasse()
        {
            var meilleursEleves = new List<Etudiants>();

            var classes = _context.Classes.ToList();

            foreach (var classe in classes)
            {
                var etudiants = _context.Etudiant
                    .Where(e => e.ClasseId == classe.Id)
                    .ToList();

                if (etudiants.Any())
                {
                    var meilleurEtudiant = etudiants
                        .OrderByDescending(e =>
                        {
                            var notes = _context.Notes
                                .Where(n => n.StudentId == e.Id)
                                .Select(n => n.NoteValue)
                                .ToList();

                            // Si l'étudiant n'a pas de notes, retourner 0
                            if (!notes.Any())
                            {
                                return 0f;
                            }

                            // Calculer la moyenne des notes
                            return notes.Average();
                        })
                        .FirstOrDefault();

                    if (meilleurEtudiant != null)
                    {
                        meilleursEleves.Add(meilleurEtudiant);
                    }
                }
            }

            return meilleursEleves;
        }

        // Méthode pour générer un PDF des meilleurs élèves
        private void GenererPDFMeilleursEleves()
        {
            var meilleursEleves = GetMeilleursElevesParClasse();

            // Demander à l'utilisateur de choisir l'emplacement du fichier
            string defaultFileName = "Meilleurs_Eleves.pdf";
            string filter = "Fichiers PDF (*.pdf)|*.pdf";
            string filePath = ChoisirEmplacementFichier(defaultFileName, filter);

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
                doc.Add(new Paragraph("Liste des Meilleurs Élèves par Classe", titleFont));

                // Ajouter les informations des élèves
                foreach (var etudiant in meilleursEleves)
                {
                    double moyenne = _context.Notes
                        .Where(n => n.StudentId == etudiant.Id)
                        .Average(n => n.NoteValue);

                    doc.Add(new Paragraph($"Nom: {etudiant.Nom}"));
                    doc.Add(new Paragraph($"Prénom: {etudiant.Prenom}"));
                    doc.Add(new Paragraph($"Matricule: {etudiant.Matricule}"));
                    doc.Add(new Paragraph($"Date de Naissance: {etudiant.DateNaissance.ToShortDateString()}"));
                    doc.Add(new Paragraph($"Classe: {etudiant.Classe.NomClasse}"));
                    doc.Add(new Paragraph($"Moyenne: {moyenne:F2}"));
                    doc.Add(new Paragraph("----------------------------------------"));
                }

                doc.Close();
            }

            // Ouvrir le fichier après génération
            OuvrirFichier(filePath);

            MessageBox.Show($"Liste des meilleurs élèves générée avec succès : {filePath}", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Méthode pour générer un Excel des meilleurs élèves
        private void GenererExcelMeilleursEleves()
        {
            var meilleursEleves = GetMeilleursElevesParClasse();

            // Demander à l'utilisateur de choisir l'emplacement du fichier
            string defaultFileName = "Meilleurs_Eleves.xlsx";
            string filter = "Fichiers Excel (*.xlsx)|*.xlsx";
            string filePath = ChoisirEmplacementFichier(defaultFileName, filter);

            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Aucun emplacement sélectionné. Le fichier n'a pas été enregistré.", "Annulé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Créer un fichier Excel
            FileInfo file = new FileInfo(filePath);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Meilleurs Élèves");

                // Ajouter le titre
                worksheet.Cells[1, 1].Value = "Liste des Meilleurs Élèves par Classe";
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.Font.Size = 16;

                // Ajouter les en-têtes
                worksheet.Cells[3, 1].Value = "Matricule";
                worksheet.Cells[3, 2].Value = "Nom";
                worksheet.Cells[3, 3].Value = "Prénom";
                worksheet.Cells[3, 4].Value = "Date de Naissance";
                worksheet.Cells[3, 5].Value = "Classe";
                worksheet.Cells[3, 6].Value = "Moyenne";

                // Ajouter les informations des élèves
                int row = 4;
                foreach (var etudiant in meilleursEleves)
                {
                    double moyenne = _context.Notes
                        .Where(n => n.StudentId == etudiant.Id)
                        .Average(n => n.NoteValue);

                    worksheet.Cells[row, 1].Value = etudiant.Matricule;
                    worksheet.Cells[row, 2].Value = etudiant.Nom;
                    worksheet.Cells[row, 3].Value = etudiant.Prenom;
                    worksheet.Cells[row, 4].Value = etudiant.DateNaissance.ToShortDateString();
                    worksheet.Cells[row, 5].Value = etudiant.Classe.NomClasse;
                    worksheet.Cells[row, 6].Value = moyenne.ToString("F2");
                    row++;
                }

                package.Save();
            }

            // Ouvrir le fichier après génération
            OuvrirFichier(filePath);

            MessageBox.Show($"Liste des meilleurs élèves générée avec succès : {filePath}", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Méthode pour choisir l'emplacement du fichier
        private string ChoisirEmplacementFichier(string defaultFileName, string filter)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.FileName = defaultFileName;
                saveFileDialog.Filter = filter;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return saveFileDialog.FileName;
                }
            }

            return null;
        }

        // Méthode pour ouvrir un fichier
        private void OuvrirFichier(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
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

        // Événements des boutons
        private void btnGenererPDF_Click(object sender, EventArgs e)
        {
            GenererPDFMeilleursEleves();
        }

        private void btnGenererExcel_Click(object sender, EventArgs e)
        {
            GenererExcelMeilleursEleves();
        }
    }
}