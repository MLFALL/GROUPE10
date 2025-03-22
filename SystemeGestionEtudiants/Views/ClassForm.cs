using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Linq;
using System.Windows.Forms;
using SystemeGestionEtudiants.Data;
using SystemeGestionEtudiants.Models;

namespace SystemeGestionEtudiants.Views
{
    public partial class ClassForm : MaterialForm // Hérite de MaterialForm pour appliquer le thème Material
    {
        private ApplicationDbContext _context;
        private bool isEditing = false;  // Indicateur pour savoir si une modification est en cours
        public ClassForm()
        {
            InitializeComponent();
            _context = new ApplicationDbContext();

            // Configuration des contrôles
            ConfigureDataGridView();
            ConfigureButtons();
            ConfigureTextBox();

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
            Refresh();
        }
        private void ConfigureDataGridView()
        {
            dataGridViewClasses.ReadOnly = true;
            dataGridViewClasses.AllowUserToAddRows = false;
            dataGridViewClasses.AllowUserToDeleteRows = false;
            dataGridViewClasses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewClasses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewClasses.RowHeadersVisible = false;
            dataGridViewClasses.ClearSelection();
        }

        private void ConfigureButtons()
        {
            btnAdd.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

        }

        private void ConfigureTextBox()
        {
            txtNomClasse.TextChanged += txtNomClasse_TextChanged;
        }



        private void RefreshData()
        {
            LoadClasses();  // Rafraîchir la liste des classes
            txtNomClasse.Clear();  // Vider le champ de texte
                                   // Désélectionner toutes les lignes du DataGridView
            dataGridViewClasses.ClearSelection();
            btnAdd.Enabled = false;

            // Réinitialiser l'état d'édition
            isEditing = false;
        }

        private void ClassForm_Load(object sender, EventArgs e)
        {
            // Charger les classes au démarrage du formulaire
            LoadClasses();
        }

        private void LoadClasses()
        {
            var classes = _context.Classes.ToList();
            dataGridViewClasses.DataSource = classes; // Met à jour la source de données du DataGridView
            // Masquer toutes les colonnes sauf "Id" et "NomClasse"
            foreach (DataGridViewColumn col in dataGridViewClasses.Columns)
            {
                if (col.Name != "Id" && col.Name != "NomClasse")
                {
                    col.Visible = false;
                }
            }

        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            // Validation du champ txtNomClasse
            if (string.IsNullOrWhiteSpace(txtNomClasse.Text))
            {
                MessageBox.Show("Veuillez entrer un nom de classe.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Vérifier que le nom de la classe ne contient pas de caractères spéciaux et ne commence pas par un chiffre
            if (!ValiderNomClasse(txtNomClasse.Text))
            {
                return; // Si la validation échoue, on arrête le processus
            }

            // Vérifier que le nom de la classe est unique
            if (!VerifierNomClasseUnique(txtNomClasse.Text))
            {
                return; // Si le nom n'est pas unique, on arrête le processus

            }
                try
                {
                // Ajouter une nouvelle classe à la base de données
                var classe = new Classe
                {
                    NomClasse = txtNomClasse.Text
                };

                _context.Classes.Add(classe);
                _context.SaveChanges();

                // Afficher un message de succès
                MessageBox.Show("La classe a été ajoutée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Rafraîchir les données
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Vérifier si une ligne est sélectionnée dans le DataGridView
            if (dataGridViewClasses.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner une classe à modifier.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validation du champ txtNomClasse
            if (string.IsNullOrWhiteSpace(txtNomClasse.Text))
            {
                MessageBox.Show("Veuillez entrer un nom de classe.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Mettre à jour l'état d'édition
            isEditing = true;
            // Vérifier que le nom de la classe ne contient pas de caractères spéciaux et ne commence pas par un chiffre
            if (!ValiderNomClasse(txtNomClasse.Text))
            {
                return; // Si la validation échoue, on arrête le processus
            }

            // Vérifier que le nom de la classe est unique
            if (!VerifierNomClasseUnique(txtNomClasse.Text))
            {
                return; // Si le nom n'est pas unique, on arrête le processus

            }

            // Demander une confirmation avant de modifier
            var confirmation = MessageBox.Show(
                "Êtes-vous sûr de vouloir modifier cette classe ?",
                "Confirmation de modification",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmation == DialogResult.Yes)  // Si l'utilisateur confirme
            {
                try
                {
                    // Trouver la classe à mettre à jour
                    var classeId = (int)dataGridViewClasses.CurrentRow.Cells["Id"].Value;
                    var classe = _context.Classes.Find(classeId);

                    if (classe != null)
                    {
                        // Mettre à jour le nom de la classe avec le texte du champ
                        classe.NomClasse = txtNomClasse.Text;
                        _context.SaveChanges();

                        // Rafraîchir les données
                        RefreshData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Vérifier si une ligne est sélectionnée dans le DataGridView
            if (dataGridViewClasses.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner une classe à supprimer.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Demander une confirmation avant de supprimer
            var confirmation = MessageBox.Show(
                "Êtes-vous sûr de vouloir supprimer cette classe et tous ses étudiants ?",
                "Confirmation de suppression",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmation == DialogResult.Yes)  // Si l'utilisateur confirme
            {
                try
                {
                    // Récupérer l'ID de la classe sélectionnée
                    var classeId = (int)dataGridViewClasses.CurrentRow.Cells["Id"].Value;

                    // Récupérer tous les étudiants de cette classe
                    var etudiants = _context.Etudiant.Where(etudiant => etudiant.ClasseId == classeId).ToList();

                    // Supprimer tous les étudiants de la classe
                    _context.Etudiant.RemoveRange(etudiants);

                    // Supprimer la classe
                    var classe = _context.Classes.Find(classeId);
                    if (classe != null)
                    {
                        _context.Classes.Remove(classe);
                    }

                    _context.SaveChanges();

                    // Afficher un message de succès
                    MessageBox.Show("La classe et tous ses étudiants ont été supprimés avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Rafraîchir les données
                    RefreshData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridViewClasses_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewClasses_SelectionChanged(object sender, EventArgs e)
        {
            // Vérifier si une ligne est sélectionnée

        }

        private void dataGridViewClasses_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Vérifier si une ligne est sélectionnée et si l'utilisateur double-clique sur une ligne (pas l'en-tête)
            if (e.RowIndex >= 0)
            {
                // Récupérer la ligne sélectionnée
                var selectedRow = dataGridViewClasses.Rows[e.RowIndex];

                // Charger le nom de la classe dans le champ txtNomClasse
                txtNomClasse.Text = selectedRow.Cells["NomClasse"].Value.ToString();
                // activer les boutons Modifier et Supprimer 
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
                btnAdd.Enabled = false;

                // Optionnel : Stocker l'ID de la classe sélectionnée dans une variable de classe
                var classeId = (int)selectedRow.Cells["Id"].Value;
            }
          
        }
        // Méthode pour valider le nom de la classe
        private bool ValiderNomClasse(string nomClasse)
        {
            // Supprimer les espaces avant et après
            nomClasse = nomClasse.Trim();

            // Vérifier qu'il ne contient pas de caractères spéciaux
            if (System.Text.RegularExpressions.Regex.IsMatch(nomClasse, @"[^a-zA-Z0-9\s]"))
            {
                MessageBox.Show("Le nom de la classe ne doit pas contenir de caractères spéciaux.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Vérifier que le nom ne commence pas par un chiffre
            if (char.IsDigit(nomClasse[0]))
            {
                MessageBox.Show("Le nom de la classe ne doit pas commencer par un chiffre.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Convertir chaque mot en majuscule (Format Titre)
            nomClasse = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nomClasse.ToLower());

            // Retourner le nom de la classe validé
            txtNomClasse.Text = nomClasse;  // Appliquer cette modification au champ de texte

            return true;
        }

        // Méthode pour vérifier que le nom de la classe est unique
        private bool VerifierNomClasseUnique(string nomClasse)
        {
            // Vérifier dans la base de données si une classe avec ce nom existe déjà
            bool existeDeja = _context.Classes.Any(c => c.NomClasse == nomClasse);

            if (existeDeja)
            {
                MessageBox.Show("Une classe avec ce nom existe déjà.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void dataGridViewClasses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Vérifiez si l'utilisateur clique sur une ligne (pas l'en-tête)
            if (e.RowIndex >= 0)
            {
                // Récupérer la ligne sélectionnée
                var selectedRow = dataGridViewClasses.Rows[e.RowIndex];

                // Désactiver le bouton "Modifier"
                btnUpdate.Enabled = false;
                btnAdd.Enabled = false;
                // Mettre à jour l'état d'édition
                isEditing = true;

                // Activer le bouton "Supprimer"
                btnDelete.Enabled = true;

                // Optionnel : Stocker l'ID de la classe sélectionnée dans une variable de classe
                var classeId = (int)selectedRow.Cells["Id"].Value;
            }
        }

        private void txtNomClasse_TextChanged(object sender, EventArgs e)
        {
            // Activer ou désactiver le bouton "Ajouter" selon que le champ est rempli ou non
            btnAdd.Enabled = !string.IsNullOrWhiteSpace(txtNomClasse.Text) && !isEditing;
        }
        
        
        
    }
}
