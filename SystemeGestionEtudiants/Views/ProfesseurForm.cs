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
using MaterialSkin.Controls;
using MaterialSkin;
using System.Text.RegularExpressions;

namespace SystemeGestionEtudiants.Views
{
    public partial class ProfesseurForm : MaterialForm
    {
        private ApplicationDbContext _context;
        private bool isEditing = false;
        private string nomMatiere;

        public ProfesseurForm()
        {
            InitializeComponent();
            // Lier l'événement Load
            this.Load += ProfesseurForm_Load;
            _context = new ApplicationDbContext();

            // Configuration des ComboBox en mode lecture seule
            cbClasse.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMatiere.DropDownStyle = ComboBoxStyle.DropDownList;

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
                TextShade.WHITE     // Couleur du texte (blanc)
            );

            Refresh();
        }
        private void ConfigureDataGridView()
        {
            dataGridViewProfesseurs.ReadOnly = true;
            dataGridViewProfesseurs.AllowUserToAddRows = false;
            dataGridViewProfesseurs.AllowUserToDeleteRows = false;
            dataGridViewProfesseurs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewProfesseurs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewProfesseurs.RowHeadersVisible = false;
            dataGridViewProfesseurs.ClearSelection();
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
        }

        private void LoadClasses()
        {
            var classes = _context.Classes.ToList();
            if (classes.Count == 0)
            {
                MessageBox.Show("Aucune classe disponible.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            classes.Insert(0, new Classe { Id = 0, NomClasse = "Sélectionnez une classe" });

            cbClasse.DataSource = classes;
            cbClasse.DisplayMember = "NomClasse";
            cbClasse.ValueMember = "Id";
            cbClasse.SelectedIndex = 0;
        }

        private void LoadMatieres(int classeId)
        {
            try
            {
                var matieres = _context.ClasseCours
                    .Where(cc => cc.ClasseId == classeId)
                    .SelectMany(cc => cc.Cours.CoursMatieres)
                    .Select(cm => cm.Matiere)
                    .Distinct()
                    .ToList();
                if (matieres.Count == 0)
                {
                    MessageBox.Show("Aucune matière disponible pour cette classe.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;  // Ne pas définir SelectedIndex si la liste est vide
                }

                matieres.Insert(0, new Matiere { Id = 0, NomMatiere = "Sélectionnez une matière" });

                cbMatiere.DataSource = matieres;
                cbMatiere.DisplayMember = "NomMatiere";
                cbMatiere.ValueMember = "Id";
                cbMatiere.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des matières : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RefreshData()
        {
            LoadProfesseurs();
            ClearFields();
            dataGridViewProfesseurs.ClearSelection();
            btnAdd.Enabled = false;
            isEditing = false;
            ConfigureButtons(); // Mettre à jour l'état des boutons
        }



        private void ProfesseurForm_Load(object sender, EventArgs e)
        {
            // Charger les données des professeurs dans le DataGridView
            LoadProfesseurs();

            // Charger les classes dans le ComboBox
            LoadClasses();

            // Charger les matières si une classe est déjà sélectionnée
            if (cbClasse.SelectedValue != null && cbClasse.SelectedValue is int classeId && classeId != 0)
            {
                LoadMatieres(classeId);
            }
        }

        private void LoadProfesseurs()
        {
            var professeurs = _context.Professeurs
                .Include(p => p.ProfesseurClasses.Select(pc => pc.Classe))
                .Include(p => p.ProfesseurMatieres.Select(pm => pm.Matiere))
                .ToList();

            var professeurAvecDetails = professeurs.Select(p => new
            {
                p.Id,
                p.Nom,
                p.Prenom,
                p.Email,
                p.Telephone,
                Classe = p.ProfesseurClasses.FirstOrDefault()?.Classe?.NomClasse,
                Matiere = p.ProfesseurMatieres.FirstOrDefault()?.Matiere?.NomMatiere,
                ClasseId = p.ProfesseurClasses.FirstOrDefault()?.ClasseId ?? 0, // Inclure ClasseId
                MatiereId = p.ProfesseurMatieres.FirstOrDefault()?.MatiereId ?? 0 // Inclure Matiere
            }).ToList();

            dataGridViewProfesseurs.DataSource = professeurAvecDetails;
            dataGridViewProfesseurs.Columns["ClasseId"].Visible = false;
            dataGridViewProfesseurs.Columns["MatiereId"].Visible = false;
        }

        private void ClearFields()
        {
            txtNom.Text = string.Empty;
            txtPrenom.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtTelephone.Text = string.Empty;
            if(cbClasse.Items.Count > 0)
                cbClasse.SelectedIndex = 0;

            if (cbMatiere.Items.Count > 0)
                cbMatiere.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;
            string email = NettoyerEmail(txtEmail.Text);
            string telephone = txtTelephone.Text;

            if (EmailOuTelephoneExiste(email, telephone))
            {
                MessageBox.Show("L'email ou le numéro de téléphone est déjà utilisé par un autre professeur.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var professeur = new Professeur
                {
                    Nom = FormatNom(txtNom.Text),
                    Prenom = FormatPrenom(txtPrenom.Text),
                    Email = email,
                    Telephone = telephone
                };

                var classeId = (int)cbClasse.SelectedValue;
                var classe = _context.Classes.Find(classeId);
                professeur.ProfesseurClasses.Add(new ProfesseurClasse
                {
                    Professeur = professeur,
                    Classe = classe
                });

                var matiereId = (int)cbMatiere.SelectedValue;
                var matiere = _context.Matieres.Find(matiereId);
                professeur.ProfesseurMatieres.Add(new ProfesseurMatiere
                {
                    Professeur = professeur,
                    Matiere = matiere
                });

                _context.Professeurs.Add(professeur);
                _context.SaveChanges();

                MessageBox.Show("Professeur ajouté avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                isEditing = false;
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
            if (dataGridViewProfesseurs.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner un professeur à modifier.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateFields()) return;
            var professeurId = (int)dataGridViewProfesseurs.CurrentRow.Cells["Id"].Value;
            string email = NettoyerEmail(txtEmail.Text);
            string telephone = txtTelephone.Text;

            if (EmailOuTelephoneExiste(email, telephone, professeurId))
            {
                MessageBox.Show("L'email ou le numéro de téléphone est déjà utilisé par un autre professeur.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var confirmation = MessageBox.Show(
                "Êtes-vous sûr de vouloir modifier ce professeur ?",
                "Confirmation de modification",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmation == DialogResult.Yes)
            {
                try
                {
                   // var professeurId = (int)dataGridViewProfesseurs.CurrentRow.Cells["Id"].Value;
                    var professeur = _context.Professeurs
                        .Include(p => p.ProfesseurClasses)
                        .Include(p => p.ProfesseurMatieres)
                        .FirstOrDefault(p => p.Id == professeurId);

                    if (professeur != null)
                    {
                        professeur.Nom = FormatNom(txtNom.Text);
                        professeur.Prenom = FormatPrenom(txtPrenom.Text);
                        professeur.Email = email;
                        professeur.Telephone = telephone;

                        var ancienneClasse = professeur.ProfesseurClasses.FirstOrDefault();
                        if (ancienneClasse != null)
                        {
                            _context.ProfesseurClasses.Remove(ancienneClasse);
                        }

                        var ancienneMatiere = professeur.ProfesseurMatieres.FirstOrDefault();
                        if (ancienneMatiere != null)
                        {
                            _context.ProfesseurMatieres.Remove(ancienneMatiere);
                        }

                        var classeId = (int)cbClasse.SelectedValue;
                        var nouvelleClasse = _context.Classes.Find(classeId);
                        professeur.ProfesseurClasses.Add(new ProfesseurClasse
                        {
                            Professeur = professeur,
                            Classe = nouvelleClasse
                        });

                        var matiereId = (int)cbMatiere.SelectedValue;
                        var nouvelleMatiere = _context.Matieres.Find(matiereId);
                        professeur.ProfesseurMatieres.Add(new ProfesseurMatiere
                        {
                            Professeur = professeur,
                            Matiere = nouvelleMatiere
                        });

                        _context.SaveChanges();
                        MessageBox.Show("Professeur modifié avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        isEditing = false;
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

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (dataGridViewProfesseurs.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner un professeur à supprimer.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmation = MessageBox.Show(
                "Êtes-vous sûr de vouloir supprimer ce professeur ?",
                "Confirmation de suppression",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmation == DialogResult.Yes)
            {
                try
                {
                    var professeurId = (int)dataGridViewProfesseurs.CurrentRow.Cells["Id"].Value;
                    var professeur = _context.Professeurs.Find(professeurId);

                    if (professeur != null)
                    {
                        _context.Professeurs.Remove(professeur);
                        _context.SaveChanges();
                        MessageBox.Show("Professeur supprimé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (string.IsNullOrWhiteSpace(txtNom.Text) || string.IsNullOrWhiteSpace(txtPrenom.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtTelephone.Text) ||
                cbClasse.SelectedIndex == 0 || cbMatiere.SelectedIndex == 0)
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ValiderEmail(txtEmail.Text))
            {
                MessageBox.Show("Adresse e-mail invalide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!ValiderNumeroSenegalais(txtTelephone.Text))
            {
                MessageBox.Show("Le numéro de téléphone doit être un numéro sénégalais valide (ex: 7XXXXXXXX ou +2217XXXXXXXX).", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private bool ValiderNumeroSenegalais(string numero)
        {
            numero = numero.Trim().Replace(" ", "");
            string pattern = @"^(?:\+221)?(70|75|76|77|78)\d{7}$";
            return Regex.IsMatch(numero, pattern);
        }
        private void dataGridViewProfesseurs_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dataGridViewProfesseurs.Rows[e.RowIndex];
                txtNom.Text = selectedRow.Cells["Nom"].Value.ToString();
                txtPrenom.Text = selectedRow.Cells["Prenom"].Value.ToString();
                txtEmail.Text = selectedRow.Cells["Email"].Value.ToString();
                txtTelephone.Text = selectedRow.Cells["Telephone"].Value.ToString();
                // Charger la classe sélectionnée
                if (selectedRow.Cells["ClasseId"].Value != null)
                {
                    cbClasse.SelectedValue = (int)selectedRow.Cells["ClasseId"].Value;
                }

                // Charger la matière sélectionnée
                /*if (!string.IsNullOrEmpty(nomMatiere))
                {
                    var matiere = _context.Matieres
                        .AsEnumerable() // Charger les données en mémoire pour éviter les problèmes de traduction SQL
                        .FirstOrDefault(m => m.NomMatiere == nomMatiere);

                    if (matiere != null)
                    {
                        cbMatiere.SelectedValue = matiere.Id;
                    }
                    else
                    {
                        cbMatiere.SelectedIndex = 0; // Sélectionner l'option par défaut si la matière n'est pas trouvée
                    }
                }
                else
                {
                    cbMatiere.SelectedIndex = 0; // Sélectionner l'option par défaut si la matière est vide
                }*/
                // Charger la classe sélectionnée
                if (selectedRow.Cells["ClasseId"].Value != null)
                {
                    cbClasse.SelectedValue = (int)selectedRow.Cells["ClasseId"].Value;
                }

                // Charger la matière sélectionnée
                if (selectedRow.Cells["MatiereId"].Value != null)
                {
                    int matiereId = (int)selectedRow.Cells["MatiereId"].Value;
                    cbMatiere.SelectedValue = matiereId;
                }
                else
                {
                    cbMatiere.SelectedIndex = 0; // Sélectionner l'option par défaut si la matière n'est pas trouvée
                }


                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
                btnAdd.Enabled = false;
                isEditing = true;
            }
        }

        private void cbClasse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbClasse.SelectedValue != null && cbClasse.SelectedValue is int classeId && classeId != 0)
            {
                LoadMatieres(classeId);
            }
            else
            {
                cbMatiere.DataSource = null;
                cbMatiere.SelectedIndex = -1; // Réinitialiser l'index pour éviter l'erreur
            }
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
        private bool EmailOuTelephoneExiste(string email, string telephone, int? professeurId = null)
        {
            return _context.Professeurs.Any(p =>
                (p.Email == email || p.Telephone == telephone) &&
                (!professeurId.HasValue || p.Id != professeurId));
        }






        private void cbMatiere_SelectedIndexChanged(object sender, EventArgs e)
        {
            ///,,
        }

        private void dataGridViewProfesseurs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewProfesseurs_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }

}
