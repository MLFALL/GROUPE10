using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MaterialSkin;
using MaterialSkin.Controls;
using SystemeGestionEtudiants.Data;
using System.Data;
using System.Data.Entity;


namespace SystemeGestionEtudiants.Views
{
    public partial class ListeProfsForm : MaterialForm
    {
        private ApplicationDbContext _context;
        private ComboBox cbFormat;
        private Button btnDownload;

        public ListeProfsForm()
        {
            InitializeComponent();
            _context = new ApplicationDbContext();

            // Configuration du MaterialSkinManager
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(
                Primary.Blue800,    // Couleur principale
                Primary.Blue900,    // Couleur principale plus sombre
                Primary.Blue400,    // Couleur secondaire
                Accent.Blue200,     // Accent
                TextShade.WHITE);  // Couleur du texte

            InitializeControls();
            CenterControls(); // Centrer les contrôles au démarrage
            this.Resize += DownloadForm_Resize; // Gérer le redimensionnement du formulaire
        }

        private void InitializeControls()
        {
            // Initialisation de ComboBox
            cbFormat = new ComboBox();
            cbFormat.Size = new System.Drawing.Size(200, 25);
            cbFormat.Items.Add("Sélectionner le format à télécharger");
            cbFormat.Items.Add("Excel (.xlsx)");
            cbFormat.Items.Add("PDF (.pdf)");
            cbFormat.SelectedIndex = 0;

            // Initialisation du bouton de téléchargement
            btnDownload = new Button();
            btnDownload.Size = new System.Drawing.Size(100, 30);
            btnDownload.Text = "Télécharger";
            btnDownload.Click += btnDownload_Click;

            // Ajouter les contrôles au formulaire
            this.Controls.Add(cbFormat);
            this.Controls.Add(btnDownload);
        }

        private void CenterControls()
        {
            // Centrer le ComboBox horizontalement
            cbFormat.Location = new System.Drawing.Point(
                (this.ClientSize.Width - cbFormat.Width) / 2, // Centrer horizontalement
                (this.ClientSize.Height - cbFormat.Height - btnDownload.Height - 10) / 2 // Centrer verticalement avec un espace de 10 pixels
            );

            // Centrer le bouton horizontalement et le placer sous le ComboBox
            btnDownload.Location = new System.Drawing.Point(
                (this.ClientSize.Width - btnDownload.Width) / 2, // Centrer horizontalement
                cbFormat.Bottom + 10 // Placer 10 pixels sous le ComboBox
            );
        }

        private void DownloadForm_Resize(object sender, EventArgs e)
        {
            CenterControls(); // Recentrer les contrôles lorsque le formulaire est redimensionné
        }

        private void ExportToExcel(string filePath)
        {
            try
            {
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Professeurs");

                // Ajouter les en-têtes
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Nom";
                worksheet.Cell(1, 3).Value = "Prénom";
                worksheet.Cell(1, 4).Value = "Email";
                worksheet.Cell(1, 5).Value = "Téléphone";
                worksheet.Cell(1, 6).Value = "Classe";
                worksheet.Cell(1, 7).Value = "Matière";

                // Remplir les données
                var professeurs = _context.Professeurs
                    .Include(p => p.ProfesseurClasses.Select(pc => pc.Classe))
                    .Include(p => p.ProfesseurMatieres.Select(pm => pm.Matiere))
                    .ToList();

                for (int i = 0; i < professeurs.Count; i++)
                {
                    var professeur = professeurs[i];
                    worksheet.Cell(i + 2, 1).Value = professeur.Id;
                    worksheet.Cell(i + 2, 2).Value = professeur.Nom;
                    worksheet.Cell(i + 2, 3).Value = professeur.Prenom;
                    worksheet.Cell(i + 2, 4).Value = professeur.Email;
                    worksheet.Cell(i + 2, 5).Value = professeur.Telephone;

                    // Ajouter la classe et la matière
                    var classe = professeur.ProfesseurClasses.FirstOrDefault()?.Classe?.NomClasse ?? "N/A";
                    var matiere = professeur.ProfesseurMatieres.FirstOrDefault()?.Matiere?.NomMatiere ?? "N/A";

                    worksheet.Cell(i + 2, 6).Value = classe;
                    worksheet.Cell(i + 2, 7).Value = matiere;
                }

                workbook.SaveAs(filePath);
                MessageBox.Show("Fichier Excel généré avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la génération du fichier Excel : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToPdf(string filePath)
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
                Paragraph title = new Paragraph("Liste des professeurs\n\n", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                // Créer un tableau pour afficher les données
                PdfPTable table = new PdfPTable(7); // 7 colonnes : ID, Nom, Prénom, Email, Téléphone, Classe, Matière
                table.WidthPercentage = 100;

                // Définir la police pour les cellules du tableau
                Font tableFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

                // Ajouter les en-têtes du tableau
                table.AddCell(new PdfPCell(new Phrase("ID", tableFont)));
                table.AddCell(new PdfPCell(new Phrase("Nom", tableFont)));
                table.AddCell(new PdfPCell(new Phrase("Prénom", tableFont)));
                table.AddCell(new PdfPCell(new Phrase("Email", tableFont)));
                table.AddCell(new PdfPCell(new Phrase("Téléphone", tableFont)));
                table.AddCell(new PdfPCell(new Phrase("Classe", tableFont)));
                table.AddCell(new PdfPCell(new Phrase("Matière", tableFont)));

                // Remplir les données
                var professeurs = _context.Professeurs
                    .Include(p => p.ProfesseurClasses.Select(pc => pc.Classe))
                    .Include(p => p.ProfesseurMatieres.Select(pm => pm.Matiere))
                    .ToList();

                foreach (var professeur in professeurs)
                {
                    table.AddCell(new PdfPCell(new Phrase(professeur.Id.ToString(), tableFont)));
                    table.AddCell(new PdfPCell(new Phrase(professeur.Nom, tableFont)));
                    table.AddCell(new PdfPCell(new Phrase(professeur.Prenom, tableFont)));
                    table.AddCell(new PdfPCell(new Phrase(professeur.Email, tableFont)));
                    table.AddCell(new PdfPCell(new Phrase(professeur.Telephone, tableFont)));

                    // Ajouter la classe et la matière
                    var classe = professeur.ProfesseurClasses.FirstOrDefault()?.Classe?.NomClasse ?? "N/A";
                    var matiere = professeur.ProfesseurMatieres.FirstOrDefault()?.Matiere?.NomMatiere ?? "N/A";

                    table.AddCell(new PdfPCell(new Phrase(classe, tableFont)));
                    table.AddCell(new PdfPCell(new Phrase(matiere, tableFont)));
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

        private void btnDownload_Click(object sender, EventArgs e)
        {
            // Vérifier si un format valide est sélectionné
            if (cbFormat.SelectedIndex == 0) // L'index 0 correspond à "Sélectionner le format à télécharger"
            {
                MessageBox.Show("Veuillez sélectionner un format de fichier avant de télécharger.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Arrêter l'exécution si aucun format n'est sélectionné
            }

            // Ouvrir une boîte de dialogue pour choisir l'emplacement du fichier
            var saveFileDialog = new SaveFileDialog();

            // Définir le filtre en fonction du format sélectionné
            if (cbFormat.SelectedItem.ToString() == "Excel (.xlsx)")
            {
                saveFileDialog.Filter = "Fichier Excel (*.xlsx)|*.xlsx";
            }
            else
            {
                saveFileDialog.Filter = "Fichier PDF (*.pdf)|*.pdf";
            }

            saveFileDialog.Title = "Enregistrer la liste des professeurs";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                if (cbFormat.SelectedItem.ToString() == "Excel (.xlsx)")
                {
                    ExportToExcel(filePath);
                }
                else
                {
                    ExportToPdf(filePath);
                }
            }
        }
    }
}