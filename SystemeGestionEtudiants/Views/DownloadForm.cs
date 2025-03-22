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

namespace SystemeGestionEtudiants.Views
{
    public partial class DownloadForm : MaterialForm
    {
        private ApplicationDbContext _context;
        private ComboBox cmbFormat;
        private Button btnnDownload;

        public DownloadForm()
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
                var worksheet = workbook.Worksheets.Add("Classes");

                // Ajouter les en-têtes
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Nom de la classe";

                // Remplir les données
                var classes = _context.Classes.ToList();
                for (int i = 0; i < classes.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = classes[i].Id;
                    worksheet.Cell(i + 2, 2).Value = classes[i].NomClasse;
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
                Paragraph title = new Paragraph("Liste des classes\n\n", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                // Créer un tableau pour afficher les données
                PdfPTable table = new PdfPTable(2); // 2 colonnes : ID et Nom de la classe
                table.WidthPercentage = 100;

                // Définir la police pour les cellules du tableau
                Font tableFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

                // Ajouter les en-têtes du tableau
                PdfPCell header1 = new PdfPCell(new Phrase("ID", tableFont));
                PdfPCell header2 = new PdfPCell(new Phrase("Nom de la classe", tableFont));
                table.AddCell(header1);
                table.AddCell(header2);

                // Remplir les données
                var classes = _context.Classes.ToList();
                foreach (var classe in classes)
                {
                    table.AddCell(new PdfPCell(new Phrase(classe.Id.ToString(), tableFont)));
                    table.AddCell(new PdfPCell(new Phrase(classe.NomClasse, tableFont)));
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

            saveFileDialog.Title = "Enregistrer la liste des classes";

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