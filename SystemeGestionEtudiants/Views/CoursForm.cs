using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemeGestionEtudiants.Data;
using SystemeGestionEtudiants.Models;

namespace SystemeGestionEtudiants.Views
{
    public partial class CoursForm: MaterialForm
    {
        private ApplicationDbContext _context;
        private bool isEditingCours = false;
        private bool isEditingMatiere = false;
        public CoursForm()
        {
            InitializeComponent();
            _context = new ApplicationDbContext();

            // Configuration des contrôles
            ConfigureDataGridViews();
            ConfigureButtons1(); 
            ConfigureButtons2();
            ConfigureTextBoxes();
            LoadClasses();

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

            RefreshData();
        }
        private void ConfigureDataGridViews()
        {
            // Configuration du DataGridView pour les cours
            dataGridViewCours.ReadOnly = true;
            dataGridViewCours.AllowUserToAddRows = false;
            dataGridViewCours.AllowUserToDeleteRows = false;
            dataGridViewCours.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCours.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCours.RowHeadersVisible = false;
            dataGridViewCours.ClearSelection();

            // Configuration du DataGridView pour les matières
            dataGridViewMatieres.ReadOnly = true;
            dataGridViewMatieres.AllowUserToAddRows = false;
            dataGridViewMatieres.AllowUserToDeleteRows = false;
            dataGridViewMatieres.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewMatieres.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewMatieres.RowHeadersVisible = false;
            dataGridViewMatieres.ClearSelection();
        }
        private void ConfigureButtons1()
        {
            btnAddCours.Enabled = false;
            btnUpdateCours.Enabled = false;
            btnDeleteCours.Enabled = false;

        }
        private void ConfigureButtons2()
        {
          
            btnAddMatieres.Enabled = false;
            btnUpdateMatiere.Enabled = false;
            btnDeleteMatiere.Enabled = false;
        }


        private void ConfigureTextBoxes()
        {
            txtNomCours.TextChanged += txtNomCours_TextChanged;
            txtNomMatiere.TextChanged += txtNomMatiere_TextChanged;
        }

        private void LoadClasses()
        {
            var classes = _context.Classes.ToList();
            classes.Insert(0, new Classe { Id = 0, NomClasse = "Sélectionnez une classe" });

            cbClasse.DataSource = classes;
            cbClasse.DisplayMember = "NomClasse";
            cbClasse.ValueMember = "Id";
            cbClasse.SelectedIndex = 0;
        }

        private void LoadCours()
        {
            // Charger les colonnes Id, NomCours et Description
            var cours = _context.Cours
                .Select(c => new
                {
                    c.Id,
                    c.NomCours,
                    c.Description // Inclure la colonne Description
                })
                .ToList();

            dataGridViewCours.DataSource = cours;

            // Masquer les autres colonnes si elles existent
            foreach (DataGridViewColumn col in dataGridViewCours.Columns)
            {
                if (col.Name != "Id" && col.Name != "NomCours" && col.Name != "Description")
                {
                    col.Visible = false;
                }
            }
        }

        private void LoadMatieres()
        {
            // Charger uniquement les colonnes Id et NomMatiere
            var matieres = _context.Matieres
                .Select(m => new
                {
                    m.Id,
                    m.NomMatiere
                })
                .ToList();

            dataGridViewMatieres.DataSource = matieres;

            // Masquer les autres colonnes si elles existent
            foreach (DataGridViewColumn col in dataGridViewMatieres.Columns)
            {
                if (col.Name != "Id" && col.Name != "NomMatiere")
                {
                    col.Visible = false;
                }
            }
        }
        private void RefreshData()
        {
            LoadCours();
            LoadMatieres();
            ClearFields();
            dataGridViewCours.ClearSelection();
            dataGridViewMatieres.ClearSelection();
            btnAddCours.Enabled = false;
            btnAddMatieres.Enabled = false;
            isEditingCours = false;
            isEditingMatiere = false;
        }
        private void ClearFields()
        {
            txtNomCours.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtNomMatiere.Text = string.Empty;
            cbClasse.SelectedIndex = 0;
        }
        private void btnAddCours_Click(object sender, EventArgs e)
        {
            if (!ValidateCoursFields()) return;
            // Vérifier les doublons de cours
            if (VerifierDoublonCours(txtNomCours.Text))
            {
                MessageBox.Show("Un cours avec ce nom existe déjà.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
                try
                {
                var cours = new Cours
                {
                    NomCours = txtNomCours.Text,
                    Description = txtDescription.Text
                };

                _context.Cours.Add(cours);
                _context.SaveChanges();

                // Associer le cours à la classe sélectionnée
                var classeId = (int)cbClasse.SelectedValue;
                if (classeId != 0) // Vérifier qu'une classe valide est sélectionnée
                {
                    var classeCours = new ClasseCours
                    {
                        ClasseId = classeId,
                        CoursId = cours.Id
                    };
                    _context.ClasseCours.Add(classeCours);
                    _context.SaveChanges();
                }

                MessageBox.Show("Cours ajouté avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ConfigureButtons1();
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnUpdateCours_Click(object sender, EventArgs e)
        {
            if (dataGridViewCours.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner un cours à modifier.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateCoursFields()) return;

            // Récupérer l'ID du cours sélectionné
            var coursId = (int)dataGridViewCours.CurrentRow.Cells["Id"].Value;

            // Vérifier les doublons de cours (en normalisant le nom et en excluant le cours en cours de modification)
            if (VerifierDoublonCours(txtNomCours.Text, coursId))
            {
                MessageBox.Show("Un cours avec ce nom existe déjà.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmation = MessageBox.Show(
                "Êtes-vous sûr de vouloir modifier ce cours ?",
                "Confirmation de modification",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmation == DialogResult.Yes)
            {
                try
                {
                    var cours = _context.Cours.Find(coursId);

                    if (cours != null)
                    {
                        cours.NomCours = txtNomCours.Text;
                        cours.Description = txtDescription.Text;
                        _context.SaveChanges();

                        MessageBox.Show("Cours modifié avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnDeleteCours_Click(object sender, EventArgs e)
        {
            if (dataGridViewCours.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner un cours à supprimer.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmation = MessageBox.Show(
                "Êtes-vous sûr de vouloir supprimer ce cours ?",
                "Confirmation de suppression",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmation == DialogResult.Yes)
            {
                try
                {
                    var coursId = (int)dataGridViewCours.CurrentRow.Cells["Id"].Value;
                    var cours = _context.Cours.Find(coursId);

                    if (cours != null)
                    {
                        // Supprimer les associations ClasseCours liées à ce cours
                        var classeCours = _context.ClasseCours.Where(cc => cc.CoursId == coursId).ToList();
                        _context.ClasseCours.RemoveRange(classeCours);

                        // Supprimer les associations CoursMatiere liées à ce cours
                        var coursMatieres = _context.CoursMatieres.Where(cm => cm.CoursId == coursId).ToList();
                        _context.CoursMatieres.RemoveRange(coursMatieres);

                        // Supprimer le cours
                        _context.Cours.Remove(cours);
                        _context.SaveChanges();

                        MessageBox.Show("Cours supprimé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ConfigureButtons1();
                        RefreshData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnAddMatieres_Click(object sender, EventArgs e)
        {
            if (!ValidateMatiereFields()) return;
            // Vérifier les doublons de matières
            if (VerifierDoublonMatiere(txtNomMatiere.Text))
            {
                MessageBox.Show("Une matière avec ce nom existe déjà.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var matiere = new Matiere
                {
                    NomMatiere = txtNomMatiere.Text
                };

                _context.Matieres.Add(matiere);
                _context.SaveChanges();

                MessageBox.Show("Matière ajoutée avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ConfigureButtons2();
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnUpdateMatiere_Click(object sender, EventArgs e)
        {
            if (dataGridViewMatieres.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner une matière à modifier.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateMatiereFields()) return;

            // Récupérer l'ID de la matière sélectionnée
            var matiereId = (int)dataGridViewMatieres.CurrentRow.Cells["Id"].Value;

            // Vérifier les doublons de matières (en normalisant le nom et en excluant la matière en cours de modification)
            if (VerifierDoublonMatiere(txtNomMatiere.Text, matiereId))
            {
                MessageBox.Show("Une matière avec ce nom existe déjà.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmation = MessageBox.Show(
                "Êtes-vous sûr de vouloir modifier cette matière ?",
                "Confirmation de modification",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmation == DialogResult.Yes)
            {
                try
                {
                    var matiere = _context.Matieres.Find(matiereId);

                    if (matiere != null)
                    {
                        matiere.NomMatiere = txtNomMatiere.Text;
                        _context.SaveChanges();

                        MessageBox.Show("Matière modifiée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnDeleteMatiere_Click(object sender, EventArgs e)
        {
            if (dataGridViewMatieres.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner une matière à supprimer.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmation = MessageBox.Show(
                "Êtes-vous sûr de vouloir supprimer cette matière ?",
                "Confirmation de suppression",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmation == DialogResult.Yes)
            {
                try
                {
                    var matiereId = (int)dataGridViewMatieres.CurrentRow.Cells["Id"].Value;
                    var matiere = _context.Matieres.Find(matiereId);

                    if (matiere != null)
                    {
                        // Supprimer les associations CoursMatiere liées à cette matière
                        var coursMatieres = _context.CoursMatieres.Where(cm => cm.MatiereId == matiereId).ToList();
                        _context.CoursMatieres.RemoveRange(coursMatieres);

                        // Supprimer la matière
                        _context.Matieres.Remove(matiere);
                        _context.SaveChanges();

                        MessageBox.Show("Matière supprimée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ConfigureButtons2();
                        RefreshData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private bool ValidateCoursFields()
        {
            if (string.IsNullOrWhiteSpace(txtNomCours.Text))
            {
                MessageBox.Show("Veuillez entrer un nom de cours.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
        private bool ValidateMatiereFields()
        {
            if (string.IsNullOrWhiteSpace(txtNomMatiere.Text))
            {
                MessageBox.Show("Veuillez entrer un nom de matière.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
        private void dataGridViewCours_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dataGridViewCours.Rows[e.RowIndex];
                txtNomCours.Text = selectedRow.Cells["NomCours"].Value?.ToString();
                txtDescription.Text = selectedRow.Cells["Description"].Value?.ToString();

                btnUpdateCours.Enabled = true;
                btnDeleteCours.Enabled = true;
                btnAddCours.Enabled = false;
                isEditingCours = true;
            }
        }
        private void dataGridViewMatieres_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dataGridViewMatieres.Rows[e.RowIndex];
                txtNomMatiere.Text = selectedRow.Cells["NomMatiere"].Value?.ToString();

                btnUpdateMatiere.Enabled = true;
                btnDeleteMatiere.Enabled = true;
                btnAddMatieres.Enabled = false;
                isEditingMatiere = true;
            }
        }
        private void txtNomCours_TextChanged(object sender, EventArgs e)
        {
            btnAddCours.Enabled = !string.IsNullOrWhiteSpace(txtNomCours.Text) && !isEditingCours;
        }
        private void txtNomMatiere_TextChanged(object sender, EventArgs e)
        {
            btnAddMatieres.Enabled = !string.IsNullOrWhiteSpace(txtNomMatiere.Text) && !isEditingMatiere;
        }
        private void btnAssociateMatiere_Click(object sender, EventArgs e)
        {
            // Vérifier si un cours, une matière et une classe sont sélectionnés
            if (dataGridViewCours.SelectedRows.Count == 0 || dataGridViewMatieres.SelectedRows.Count == 0 || cbClasse.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un cours, une matière et une classe.", "Sélection requise", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var coursId = (int)dataGridViewCours.CurrentRow.Cells["Id"].Value;  // Récupérer l'ID du cours sélectionné
            var matiereId = (int)dataGridViewMatieres.CurrentRow.Cells["Id"].Value;  // Récupérer l'ID de la matière sélectionnée
            var classeId = (int)cbClasse.SelectedValue;  // Récupérer l'ID de la classe sélectionnée

            // Vérifier si le cours est déjà associé à la classe via ClasseCours
            var coursClasseAssocie = _context.ClasseCours
                .FirstOrDefault(cc => cc.CoursId == coursId && cc.ClasseId == classeId);

            if (coursClasseAssocie == null)
            {
                // Si le cours n'est pas associé à la classe, l'associer
                var nouvelleAssociationClasseCours = new ClasseCours
                {
                    ClasseId = classeId,
                    CoursId = coursId
                };
                _context.ClasseCours.Add(nouvelleAssociationClasseCours);
            }

            // Vérifier si l'association matière-cours existe déjà pour cette classe
            var existingAssociationCoursMatiere = _context.CoursMatieres
                .FirstOrDefault(cm => cm.CoursId == coursId && cm.MatiereId == matiereId);

            if (existingAssociationCoursMatiere != null)
            {
                // Si l'association matière-cours existe déjà, afficher un message d'erreur
                MessageBox.Show("Cette association matière-cours existe déjà.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Créer l'association entre la matière et le cours
            var coursMatiere = new CoursMatiere
            {
                CoursId = coursId,
                MatiereId = matiereId
            };

            _context.CoursMatieres.Add(coursMatiere);  // Ajouter l'association matière-cours
            _context.SaveChanges();  // Sauvegarder les changements dans la base de données

            MessageBox.Show("La matière a été associée au cours pour la classe avec succès.", "Association réussie", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshData();
        }


        private bool VerifierDoublonCours(string nomCours)
        {
            string nomNormalise = NormaliserNom(nomCours); // Normaliser le nom du cours avant la requête
            return _context.Cours.Any(c => c.NomCours.Trim().ToLower() == nomNormalise);
        }

        private bool VerifierDoublonCours(string nomCours, int coursId)
        {
            string nomNormalise = NormaliserNom(nomCours); // Normaliser le nom du cours avant la requête
            return _context.Cours
                .Any(c => c.NomCours.Trim().ToLower() == nomNormalise && c.Id != coursId); // Exclure le cours en cours de modification
        }

        private bool VerifierDoublonMatiere(string nomMatiere)
        {
            string nomNormalise = NormaliserNom(nomMatiere); // Normaliser le nom de la matière avant la requête
            return _context.Matieres.Any(m => m.NomMatiere.Trim().ToLower() == nomNormalise);
        }

        private bool VerifierDoublonMatiere(string nomMatiere, int matiereId)
        {
            string nomNormalise = NormaliserNom(nomMatiere); // Normaliser le nom de la matière avant la requête
            return _context.Matieres
                .Any(m => m.NomMatiere.Trim().ToLower() == nomNormalise && m.Id != matiereId); // Exclure la matière en cours de modification
        }

        private string NormaliserNom(string nom)
        {
            return nom.Trim().ToLower(); // Supprimer les espaces et convertir en minuscules
        }

        private void CoursForm_Load(object sender, EventArgs e)
        {
            LoadMatieres();
            LoadCours();
        }
    }
}
