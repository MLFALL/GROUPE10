using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SystemeGestionEtudiants.Data;
using SystemeGestionEtudiants.Models;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Data.Entity;

namespace SystemeGestionEtudiants.Views
{
    public partial class DownProfForm : Form
    {
        private ApplicationDbContext _context;

        public DownProfForm()
        {
            InitializeComponent();
            _context = new ApplicationDbContext();

            InitializeControls();
        }

        private void InitializeControls()
        {
            // Initialisation du ComboBox pour les classes
            cbClasse = new ComboBox();
            cbClasse.DropDownStyle = ComboBoxStyle.DropDownList;
            cbClasse.Location = new System.Drawing.Point(20, 20);
            cbClasse.Size = new System.Drawing.Size(200, 25);
            this.Controls.Add(cbClasse);

            // Initialisation du ComboBox pour les formats
            cbFormat = new ComboBox();
            cbFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFormat.Location = new System.Drawing.Point(20, 60);
            cbFormat.Size = new System.Drawing.Size(200, 25);
            cbFormat.Items.AddRange(new string[] { "Excel (.xlsx)", "PDF (.pdf)" });
            cbFormat.SelectedIndex = 0;
            this.Controls.Add(cbFormat);

            // Initialisation du bouton de téléchargement
            btnDownload = new Button();
            btnDownload.Text = "Télécharger";
            btnDownload.Location = new System.Drawing.Point(20, 100);
            btnDownload.Size = new System.Drawing.Size(100, 30);
            btnDownload.Click += btnDownload_Click;
            this.Controls.Add(btnDownload);

            // Charger les classes dans le ComboBox
            LoadClasses();
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
                MessageBox.Show($"Erreur lors du chargement des classes : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (cbClasse.SelectedValue == null || (int)cbClasse.SelectedValue == 0)
            {
                MessageBox.Show("Veuillez sélectionner une classe.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int classeId = (int)cbClasse.SelectedValue;
            string format = cbFormat.SelectedItem.ToString();

            // Ouvrir une boîte de dialogue pour choisir l'emplacement du fichier
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = format == "Excel (.xlsx)" ? "Fichier Excel (*.xlsx)|*.xlsx" : "Fichier PDF (*.pdf)|*.pdf";
            saveFileDialog.Title = "Enregistrer la liste des étudiants";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                if (format == "Excel (.xlsx)")
                {
                    ExportToExcel(filePath, classeId);
                }
                else
                {
                    ExportToPdf(filePath, classeId);
                }
            }
        }

        private void ExportToExcel(string filePath, int classeId)
        {
            try
            {
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Étudiants");

                // Ajouter les en-têtes
                worksheet.Cell(1, 1).Value = "Matricule";
                worksheet.Cell(1, 2).Value = "Nom";
                worksheet.Cell(1, 3).Value = "Prénom";
                worksheet.Cell(1, 4).Value = "Date de Naissance";
                worksheet.Cell(1, 5).Value = "Sexe";
                worksheet.Cell(1, 6).Value = "Adresse";
                worksheet.Cell(1, 7).Value = "Téléphone";
                worksheet.Cell(1, 8).Value = "Email";
                worksheet.Cell(1, 9).Value = "Classe";

                // Remplir les données
                var etudiants = _context.Etudiant
                    .Where(e => e.ClasseId == classeId)
                    .Include(e => e.Classe)
                    .ToList();

                for (int i = 0; i < etudiants.Count; i++)
                {
                    var etudiant = etudiants[i];
                    worksheet.Cell(i + 2, 1).Value = etudiant.Matricule;
                    worksheet.Cell(i + 2, 2).Value = etudiant.Nom;
                    worksheet.Cell(i + 2, 3).Value = etudiant.Prenom;
                    worksheet.Cell(i + 2, 4).Value = etudiant.DateNaissance.ToShortDateString();
                    worksheet.Cell(i + 2, 5).Value = etudiant.Sexe;
                    worksheet.Cell(i + 2, 6).Value = etudiant.Adresse;
                    worksheet.Cell(i + 2, 7).Value = etudiant.Telephone;
                    worksheet.Cell(i + 2, 8).Value = etudiant.Email;
                    worksheet.Cell(i + 2, 9).Value = etudiant.Classe?.NomClasse ?? "N/A";
                }

                workbook.SaveAs(filePath);
                MessageBox.Show("Fichier Excel généré avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la génération du fichier Excel : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToPdf(string filePath, int classeId)
        {
            try
            {
                // Créer un document PDF
                Document document = new Document();
                PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();

                // Définir la police Helvetica en gras pour le titre
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);

                // Ajouter un titre
                Paragraph title = new Paragraph("Liste des étudiants\n\n", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                // Créer un tableau pour afficher les données
                PdfPTable table = new PdfPTable(9); // 9 colonnes : Matricule, Nom, Prénom, Date de Naissance, Sexe, Adresse, Téléphone, Email, Classe
                table.WidthPercentage = 100;

                // Définir la police pour les cellules du tableau
                Font tableFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

                // Ajouter les en-têtes du tableau
                table.AddCell(new PdfPCell(new Phrase("Matricule", tableFont)));
                table.AddCell(new PdfPCell(new Phrase("Nom", tableFont)));
                table.AddCell(new PdfPCell(new Phrase("Prénom", tableFont)));
                table.AddCell(new PdfPCell(new Phrase("Date de Naissance", tableFont)));
                table.AddCell(new PdfPCell(new Phrase("Sexe", tableFont)));
                table.AddCell(new PdfPCell(new Phrase("Adresse", tableFont)));
                table.AddCell(new PdfPCell(new Phrase("Téléphone", tableFont)));
                table.AddCell(new PdfPCell(new Phrase("Email", tableFont)));
                table.AddCell(new PdfPCell(new Phrase("Classe", tableFont)));

                // Remplir les données
                var etudiants = _context.Etudiant
                    .Where(e => e.ClasseId == classeId)
                    .Include(e => e.Classe)
                    .ToList();

                foreach (var etudiant in etudiants)
                {
                    table.AddCell(new PdfPCell(new Phrase(etudiant.Matricule, tableFont)));
                    table.AddCell(new PdfPCell(new Phrase(etudiant.Nom, tableFont)));
                    table.AddCell(new PdfPCell(new Phrase(etudiant.Prenom, tableFont)));
                    table.AddCell(new PdfPCell(new Phrase(etudiant.DateNaissance.ToShortDateString(), tableFont)));
                    table.AddCell(new PdfPCell(new Phrase(etudiant.Sexe, tableFont)));
                    table.AddCell(new PdfPCell(new Phrase(etudiant.Adresse, tableFont)));
                    table.AddCell(new PdfPCell(new Phrase(etudiant.Telephone, tableFont)));
                    table.AddCell(new PdfPCell(new Phrase(etudiant.Email, tableFont)));
                    table.AddCell(new PdfPCell(new Phrase(etudiant.Classe?.NomClasse ?? "N/A", tableFont)));
                }

                document.Add(table);
                document.Close();

                MessageBox.Show("Fichier PDF généré avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la génération du fichier PDF : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DownProfForm_Load(object sender, EventArgs e)
        {

        }

        private void btnDownload_Click_1(object sender, EventArgs e)
        {
            if (cbClasse.SelectedValue == null || (int)cbClasse.SelectedValue == 0)
            {
                MessageBox.Show("Veuillez sélectionner une classe.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int classeId = (int)cbClasse.SelectedValue;
            string format = cbFormat.SelectedItem.ToString();

            // Ouvrir une boîte de dialogue pour choisir l'emplacement du fichier
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = format == "Excel (.xlsx)" ? "Fichier Excel (*.xlsx)|*.xlsx" : "Fichier PDF (*.pdf)|*.pdf";
            saveFileDialog.Title = "Enregistrer la liste des étudiants";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                if (format == "Excel (.xlsx)")
                {
                    ExportToExcel(filePath, classeId);
                }
                else
                {
                    ExportToPdf(filePath, classeId);
                }
            }
        }
    }
}