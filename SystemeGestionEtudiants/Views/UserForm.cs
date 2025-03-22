using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemeGestionEtudiants.Data;
using SystemeGestionEtudiants.Models;

namespace SystemeGestionEtudiants.Views
{
    public partial class UserForm: MaterialForm
    {
        private ApplicationDbContext _context;
        private bool isEditing = false;

        public UserForm()
        {
            InitializeComponent();
            _context = new ApplicationDbContext();

            // Ajouter errorProvider
            errorProvider = new ErrorProvider();

            // Configuration des contrôles
            ConfigureDataGridView();
            ConfigureButtons();
            ConfigureTextBoxes();

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
        }
        private void ConfigureDataGridView()
        {
            dataGridViewUsers.ReadOnly = true;
            dataGridViewUsers.AllowUserToAddRows = false;
            dataGridViewUsers.AllowUserToDeleteRows = false;
            dataGridViewUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUsers.RowHeadersVisible = false;
            dataGridViewUsers.ClearSelection();
        }
        private void ConfigureButtons()
        {
            btnAdd.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }
        private void ConfigureTextBoxes()
        {
            txtNomUtilisateur.TextChanged += txtNomUtilisateur_TextChanged;
            txtMotDePasse.TextChanged += txtMotDePasse_TextChanged;
            txtTelephone.TextChanged += txtTelephone_TextChanged;
        }
        private void LoadUsers()
        {
            var users = _context.Utilisateurs
                .Select(u => new
                {
                    u.Id,
                    u.NomUtilisateur, // Correction ici
                    u.Role,
                    u.Telephone
                })
                .ToList();

            dataGridViewUsers.DataSource = users;
        }
        private void ClearFields()
        {
            txtNomUtilisateur.Text = string.Empty;
            txtMotDePasse.Text = string.Empty;
            txtTelephone.Text = string.Empty;
            cmbRole.SelectedIndex = 0;
        }
        private void RefreshData()
        {
            LoadUsers();
            ClearFields();
            dataGridViewUsers.ClearSelection();
            btnAdd.Enabled = false;
            isEditing = false;
        }
        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtNomUtilisateur.Text) ||
                string.IsNullOrWhiteSpace(txtMotDePasse.Text) ||
                cmbRole.SelectedIndex == 0)
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ValiderNomUtilisateur(txtNomUtilisateur.Text))
            {
                MessageBox.Show("Le nom d'utilisateur ne doit contenir que des lettres et des chiffres.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!ValiderTelephone(txtTelephone.Text))
            {
                MessageBox.Show("Le numéro de téléphone doit être valide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
        private bool ValiderNomUtilisateur(string nomUtilisateur)
        {
            string pattern = @"^[a-zA-Z0-9]+$";
            return Regex.IsMatch(nomUtilisateur, pattern);
        }
        private bool ValiderMotDePasse(string motDePasse)
        {
            return motDePasse.Length >= 6;
        }
        private bool ValiderTelephone(string telephone)
        {
            string pattern = @"^(?:\+221)?(70|75|76|77|78)\d{7}$";
            return Regex.IsMatch(telephone, pattern);

        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Valider les champs obligatoires
            if (!ValidateFields()) return;

            try
            {
                // Hasher le mot de passe avant de l'enregistrer
                string motDePasseHash = BCrypt.Net.BCrypt.HashPassword(txtMotDePasse.Text);

                // Créer un nouvel utilisateur
                var utilisateur = new Utilisateur
                {
                    NomUtilisateur = txtNomUtilisateur.Text,
                    MotDePasse = motDePasseHash, // Utiliser le mot de passe hashé
                    Role = cmbRole.SelectedItem.ToString(),
                    Telephone = txtTelephone.Text
                };

                // Ajouter l'utilisateur à la base de données
                _context.Utilisateurs.Add(utilisateur);
                _context.SaveChanges();

                // Afficher un message de succès
                MessageBox.Show("Utilisateur ajouté avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Rafraîchir les données
                RefreshData();
            }
            catch (Exception ex)
            {
                // Afficher un message d'erreur en cas d'exception
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Vérifier si une ligne est sélectionnée dans le DataGridView
            if (dataGridViewUsers.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner un utilisateur à modifier.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Valider les champs obligatoires
            if (!ValidateFields()) return;

            // Demander une confirmation à l'utilisateur
            var confirmation = MessageBox.Show(
                "Êtes-vous sûr de vouloir modifier cet utilisateur ?",
                "Confirmation de modification",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmation == DialogResult.Yes)
            {
                try
                {
                    // Récupérer l'ID de l'utilisateur sélectionné
                    var userId = (int)dataGridViewUsers.CurrentRow.Cells["Id"].Value;

                    // Récupérer l'utilisateur à partir de la base de données
                    var utilisateur = _context.Utilisateurs.Find(userId);

                    if (utilisateur != null)
                    {
                        // Mettre à jour les informations de l'utilisateur
                        utilisateur.NomUtilisateur = txtNomUtilisateur.Text;

                        // Hasher le nouveau mot de passe si le champ n'est pas vide
                        if (!string.IsNullOrWhiteSpace(txtMotDePasse.Text))
                        {
                            utilisateur.MotDePasse = BCrypt.Net.BCrypt.HashPassword(txtMotDePasse.Text);
                        }

                        utilisateur.Role = cmbRole.SelectedItem.ToString();
                        utilisateur.Telephone = txtTelephone.Text;

                        // Enregistrer les modifications dans la base de données
                        _context.SaveChanges();

                        // Afficher un message de succès
                        MessageBox.Show("Utilisateur modifié avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Rafraîchir les données
                        RefreshData();
                    }
                }
                catch (Exception ex)
                {
                    // Afficher un message d'erreur en cas d'exception
                    MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.CurrentRow == null)
            {
                MessageBox.Show("Veuillez sélectionner un utilisateur à supprimer.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmation = MessageBox.Show(
                "Êtes-vous sûr de vouloir supprimer cet utilisateur ?",
                "Confirmation de suppression",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmation == DialogResult.Yes)
            {
                try
                {
                    var userId = (int)dataGridViewUsers.CurrentRow.Cells["Id"].Value;
                    var utilisateur = _context.Utilisateurs.Find(userId);

                    if (utilisateur != null)
                    {
                        _context.Utilisateurs.Remove(utilisateur);
                        _context.SaveChanges();
                        MessageBox.Show("Utilisateur supprimé avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void dataGridViewUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dataGridViewUsers.Rows[e.RowIndex];
                txtNomUtilisateur.Text = selectedRow.Cells["NomUtilisateur"].Value.ToString();
                txtMotDePasse.Text = string.Empty; // Ne pas afficher le mot de passe hashé
                cmbRole.SelectedItem = selectedRow.Cells["Role"].Value.ToString();
                txtTelephone.Text = selectedRow.Cells["Telephone"].Value.ToString();

                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
                btnAdd.Enabled = false;
                isEditing = true;
            }
        }
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
        private void UserForm_Load(object sender, EventArgs e)
        {
            // Charger les rôles disponibles
            cmbRole.Items.Clear();
            cmbRole.Items.Add("Sélectionnez un rôle");
            cmbRole.Items.Add("Administrateur");
            cmbRole.Items.Add("DE");
            cmbRole.Items.Add("Agent");
            cmbRole.SelectedIndex = 0;

            LoadUsers();
        }
        private void txtNomUtilisateur_TextChanged(object sender, EventArgs e)
        {
            // Valider le nom d'utilisateur
            if (!ValiderNomUtilisateur(txtNomUtilisateur.Text))
            {
                errorProvider.SetError(txtNomUtilisateur, "Le nom d'utilisateur ne doit contenir que des lettres et des chiffres.");
                btnAdd.Enabled = false;
            }
            else
            {
                errorProvider.SetError(txtNomUtilisateur, ""); // Effacer l'erreur
            }

            // Activer le bouton Ajouter si tous les champs sont valides
            btnAdd.Enabled = !string.IsNullOrWhiteSpace(txtNomUtilisateur.Text) &&
                             !string.IsNullOrWhiteSpace(txtMotDePasse.Text) &&
                             !string.IsNullOrWhiteSpace(txtTelephone.Text) &&
                             cmbRole.SelectedIndex > 0 &&
                             !isEditing;
        }
        private void txtMotDePasse_TextChanged(object sender, EventArgs e)
        {
            // Valider le mot de passe
            if (!ValiderMotDePasse(txtMotDePasse.Text))
            {
                errorProvider.SetError(txtMotDePasse, "Le mot de passe doit contenir au moins 6 caractères.");
                btnAdd.Enabled = false;
            }
            else
            {
                errorProvider.SetError(txtMotDePasse, ""); // Effacer l'erreur
            }

            // Activer le bouton Ajouter si tous les champs sont valides
            btnAdd.Enabled = !string.IsNullOrWhiteSpace(txtNomUtilisateur.Text) &&
                             !string.IsNullOrWhiteSpace(txtMotDePasse.Text) &&
                             !string.IsNullOrWhiteSpace(txtTelephone.Text) &&
                             cmbRole.SelectedIndex > 0 &&
                             !isEditing;
        }
        private void txtTelephone_TextChanged(object sender, EventArgs e)
        {
            // Valider le numéro de téléphone
            if (!ValiderTelephone(txtTelephone.Text))
            {
                errorProvider.SetError(txtTelephone, "Le numéro de téléphone doit être valide (ex: 7XXXXXXXX ou +2217XXXXXXXX).");
                btnAdd.Enabled = false;
            }
            else
            {
                errorProvider.SetError(txtTelephone, ""); // Effacer l'erreur
            }

            // Activer le bouton Ajouter si tous les champs sont valides
            btnAdd.Enabled = !string.IsNullOrWhiteSpace(txtNomUtilisateur.Text) &&
                             !string.IsNullOrWhiteSpace(txtMotDePasse.Text) &&
                             !string.IsNullOrWhiteSpace(txtTelephone.Text) &&
                             cmbRole.SelectedIndex > 0 &&
                             !isEditing;
        }

        // Initialiser Twilio
       /* string accountSid = "ACadd361a45d09c4c2bbfdb7f47a544bf5";
        string authToken = "369b84f97007dc72dbfb56f3aeca5fa4";*/
        




        private void dataGridViewUsers_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Vérifier que l'utilisateur a bien double-cliqué sur une ligne valide
        }
    }
}
