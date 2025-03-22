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
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using MaterialSkin;
using MaterialSkin.Controls;


namespace SystemeGestionEtudiants.Views
{
    public partial class EtudiantsForm: MaterialForm
    {
        private ApplicationDbContext _context;
        private bool isEditing = false;
        public EtudiantsForm()
        {
            InitializeComponent();
            _context = new ApplicationDbContext();
            // Configuration des ComboBox en mode lecture seule
            cmbSexe.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbClasse.DropDownStyle = ComboBoxStyle.DropDownList;

            // Configuration des contrôles
            ConfigureDataGridView();
            ConfigureButtons();
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
                TextShade.WHITE);   // Couleur du texte (blanc)
        }
        private void ConfigureDataGridView()
        {
            dataGridViewEtudiants.ReadOnly = true;
            dataGridViewEtudiants.AllowUserToAddRows = false;
            dataGridViewEtudiants.AllowUserToDeleteRows = false;
            dataGridViewEtudiants.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewEtudiants.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewEtudiants.RowHeadersVisible = false;
            dataGridViewEtudiants.ClearSelection();
        }

        private void ConfigureButtons()
        {
            btnAdd.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void ConfigureTextBoxes()
        {
            txtNom.TextChanged += txtNom_TextChanged;
            txtPrenom.TextChanged += txtPrenom_TextChanged;
            txtEmail.TextChanged += txtEmail_TextChanged;
            txtTelephone.TextChanged += txtTelephone_TextChanged;
            txtAdresse.TextChanged += txtAdresse_TextChanged;
        }
        private void LoadClasses()
        {
            var classes = _context.Classes.ToList();
            classes.Insert(0, new Classe { Id = 0, NomClasse = "Sélectionnez une classe" });

            cmbClasse.DataSource = classes;
            cmbClasse.DisplayMember = "NomClasse";
            cmbClasse.ValueMember = "Id";
            cmbClasse.SelectedIndex = 0;
        }
        private void RefreshData()
        {
            LoadEtudiants();
            ClearFields();
            dataGridViewEtudiants.ClearSelection();
            btnAdd.Enabled = false;
            isEditing = false;
        }
        private void LoadEtudiants()
        {
            var etudiants = _context.Etudiant
                .Include(e => e.Classe)
                .Select(e => new
                {
                    e.Id,
                    e.Matricule,
                    e.Nom,
                    e.Prenom,
                    e.DateNaissance,
                    e.Sexe,
                    e.Adresse,
                    e.Telephone,
                    e.Email,
                    ClasseNom = e.Classe.NomClasse,
                    e.ClasseId
                })
                .ToList();

            dataGridViewEtudiants.DataSource = etudiants;
            dataGridViewEtudiants.Columns["ClasseId"].Visible = false;
        }
        private void ClearFields()
        {
            txtMatricule.Text = string.Empty;
            txtNom.Text = string.Empty;
            txtPrenom.Text = string.Empty;
            txtAdresse.Text = string.Empty;
            txtTelephone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            cmbSexe.SelectedIndex = 0;
            cmbClasse.SelectedIndex = 0;
            dtpDateNaissance.Value = DateTime.Now.AddYears(-18);
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            try
            {
                string matricule = "ETU" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var etudiant = new Etudiants
                {
                    Matricule = matricule,
                    Nom = FormatNom(txtNom.Text),
                    Prenom = FormatPrenom(txtPrenom.Text),
                    DateNaissance = dtpDateNaissance.Value,
                    Sexe = cmbSexe.SelectedItem.ToString(),
                    Adresse = txtAdresse.Text,
                    Telephone = txtTelephone.Text,
                    Email = NettoyerEmail(txtEmail.Text),
                    ClasseId = (int)cmbClasse.SelectedValue
                };

                _context.Etudiant.Add(etudiant);
                _context.SaveChanges();

                MessageBox.Show("Étudiant ajouté avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ConfigureButtons();
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewEtudiants.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner un étudiant à modifier.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateFields()) return;

            var confirmation = MessageBox.Show(
                "Êtes-vous sûr de vouloir modifier cet étudiant ?",
                "Confirmation de modification",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmation == DialogResult.Yes)
            {
                try
                {
                    var etudiantId = (int)dataGridViewEtudiants.CurrentRow.Cells["Id"].Value;
                    var etudiant = _context.Etudiant.Find(etudiantId);

                    if (etudiant != null)
                    {
                        etudiant.Nom = FormatNom(txtNom.Text);
                        etudiant.Prenom = FormatPrenom(txtPrenom.Text);
                        etudiant.DateNaissance = dtpDateNaissance.Value;
                        etudiant.Sexe = cmbSexe.SelectedItem.ToString();
                        etudiant.Adresse = txtAdresse.Text;
                        etudiant.Telephone = txtTelephone.Text;
                        etudiant.Email = NettoyerEmail(txtEmail.Text);
                        etudiant.ClasseId = (int)cmbClasse.SelectedValue;

                        _context.SaveChanges();
                        MessageBox.Show("L'étudiant a été modifié avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ConfigureButtons();
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
            if (dataGridViewEtudiants.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner un étudiant à supprimer.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmation = MessageBox.Show(
                "Êtes-vous sûr de vouloir supprimer cet étudiant ?",
                "Confirmation de suppression",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmation == DialogResult.Yes)
            {
                try
                {
                    var etudiantId = (int)dataGridViewEtudiants.CurrentRow.Cells["Id"].Value;
                    var etudiant = _context.Etudiant.Find(etudiantId);

                    if (etudiant != null)
                    {
                        _context.Etudiant.Remove(etudiant);
                        _context.SaveChanges();
                        MessageBox.Show("L'étudiant a été supprimé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ConfigureButtons();
                        RefreshData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text) || string.IsNullOrWhiteSpace(txtPrenom.Text) || cmbSexe.SelectedIndex == 0 || cmbClasse.SelectedIndex == 0)
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ValiderNomPrenom(txtNom.Text))
            {
                MessageBox.Show("Le nom ne doit contenir que des lettres, des espaces et des tirets entre les mots.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!ValiderNomPrenom(txtPrenom.Text))
            {
                MessageBox.Show("Le prénom ne doit contenir que des lettres, des espaces et des tirets entre les mots.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!ValiderEmail(txtEmail.Text))
            {
                MessageBox.Show("Adresse e-mail invalide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!ValiderAdresse(txtAdresse.Text))
            {
                MessageBox.Show("L'adresse ne doit pas commencer par un caractère spécial.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!ValiderNumeroSenegalais(txtTelephone.Text))
            {
                MessageBox.Show("Le numéro de téléphone doit être un numéro sénégalais valide (ex: 7XXXXXXXX ou +2217XXXXXXXX).", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (dtpDateNaissance.Value > DateTime.Now.AddYears(-18))
            {
                MessageBox.Show("L'étudiant doit avoir au moins 18 ans.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
        private string FormatNom(string nom)
        {
            return nom.Trim().ToUpper();
        }

        private string FormatPrenom(string prenom)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(prenom.Trim().ToLower());
        }

        private bool ValiderNomPrenom(string texte)
        {
            texte = texte.Trim();
            string pattern = @"^[a-zA-Z]+(?:[\s-][a-zA-Z]+)*$";
            return texte.Length >= 2 && Regex.IsMatch(texte, pattern);
        }

        private bool ValiderEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        private string NettoyerEmail(string email)
        {
            return email.Trim().ToLower();
        }

        private bool ValiderAdresse(string adresse)
        {
            adresse = adresse.Trim();
            return char.IsLetter(adresse[0]);
        }
        private bool ValiderNumeroSenegalais(string numero)
        {
            numero = numero.Trim().Replace(" ", "");
            string pattern = @"^(?:\+221)?(70|75|76|77|78)\d{7}$";
            return Regex.IsMatch(numero, pattern);
        }
        private void dataGridViewEtudiants_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dataGridViewEtudiants.Rows[e.RowIndex];
                txtMatricule.Text = selectedRow.Cells["Matricule"].Value.ToString();
                txtNom.Text = selectedRow.Cells["Nom"].Value.ToString();
                txtPrenom.Text = selectedRow.Cells["Prenom"].Value.ToString();
                dtpDateNaissance.Value = (DateTime)selectedRow.Cells["DateNaissance"].Value;
                cmbSexe.SelectedItem = selectedRow.Cells["Sexe"].Value.ToString();
                txtAdresse.Text = selectedRow.Cells["Adresse"].Value.ToString();
                txtTelephone.Text = selectedRow.Cells["Telephone"].Value.ToString();
                txtEmail.Text = selectedRow.Cells["Email"].Value.ToString();
                cmbClasse.SelectedValue = (int)selectedRow.Cells["ClasseId"].Value;

                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
                btnAdd.Enabled = false;
                isEditing = true;
            }
        }






        private void EtudiantsForm_Load(object sender, EventArgs e)
        {
            // Initialisation du ComboBox avec les valeurs possibles (Masculin/Féminin)
            cmbSexe.Items.Clear();
            cmbSexe.Items.Add("M"); // M pour Masculin
            cmbSexe.Items.Add("F"); // F pour Féminin

            // Ajouter une option vide pour obliger l'utilisateur à faire un choix
            cmbSexe.Items.Insert(0, "Sélectionnez le sexe");
            cmbSexe.SelectedIndex = 0; // La première option est vide (ou explicative)

            // Charger les classes depuis la base de données dans cmbClasse
            LoadClasses();

            Refresh();

            // Désactiver le bouton Ajouter par défaut

            //btnAdd.Enabled = false;
        }
        private void cmbClasse_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Vérifier les conditions et activer/désactiver le bouton Ajouter
            
        }

        private void dtpDateNaissance_ValueChanged(object sender, EventArgs e)
        {
            // Vérifier les conditions et activer/désactiver le bouton Ajouter
            
        }




        public override void Refresh()
        {
            // Charger les étudiants avec le nom de la classe
            var etudiants = _context.Etudiant
                .Include(e => e.Classe)  // Inclure la classe associée à l'étudiant
                .Select(e => new
                {
                    e.Id,
                    e.Matricule,
                    e.Nom,
                    e.Prenom,
                    e.DateNaissance,
                    e.Sexe,
                    e.Adresse,
                    e.Telephone,
                    e.Email,
                    ClasseNom = e.Classe.NomClasse,  // Récupérer le nom de la classe
                    e.ClasseId  // Vous pouvez garder l'ID de la classe pour l'utiliser dans le code, mais vous ne l'affichez pas
                })
                .ToList();

            // Définir la source de données du DataGridView
            dataGridViewEtudiants.DataSource = etudiants;

            // Si vous ne voulez pas afficher la colonne 'ClasseId', vous pouvez la masquer
            dataGridViewEtudiants.Columns["ClasseId"].Visible = false; // Masquer la colonne 'ClasseId'
        }



        private void cmbSexe_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void dataGridViewEtudiants_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //
        }



        private void txtMatricule_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNom_TextChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = !string.IsNullOrWhiteSpace(txtNom.Text) && !isEditing;
        }

        private void txtPrenom_TextChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = !string.IsNullOrWhiteSpace(txtPrenom.Text) && !isEditing;
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = !string.IsNullOrWhiteSpace(txtEmail.Text) && !isEditing;
        }

        private void txtTelephone_TextChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = !string.IsNullOrWhiteSpace(txtTelephone.Text) && !isEditing;
        }

        private void txtAdresse_TextChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = !string.IsNullOrWhiteSpace(txtAdresse.Text) && !isEditing;
        }
    }
}
