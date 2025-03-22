using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SystemeGestionEtudiants.Data;
using SystemeGestionEtudiants.Models;
using System.Data.Entity;
using MaterialSkin;
using MaterialSkin.Controls;

namespace SystemeGestionEtudiants.Views
{
    public partial class EtudiantsParClasseForm : MaterialForm
    {
        private ApplicationDbContext _context;

        public EtudiantsParClasseForm()
        {
            InitializeComponent();
            // Configuration du MaterialSkinManager
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(
                Primary.Blue800,    // Couleur principale
                Primary.Blue900,    // Couleur principale plus sombre
                Primary.Blue400,    // Couleur secondaire
                Accent.Blue200,     // Accent
                TextShade.WHITE);  // Couleur du texte_context = new ApplicationDbContext();
            _context = new ApplicationDbContext();
            LoadEtudiantsParClasse();
        }

        private void LoadEtudiantsParClasse()
        {
            try
            {
                // Charger les classes
                var classes = _context.Classes.ToList();

                // Ajouter une option vide pour permettre à l'utilisateur de choisir une classe
                classes.Insert(0, new Classe { Id = 0, NomClasse = "Sélectionnez une classe" });

                // Remplir le ComboBox avec les classes
                cbClasse.DataSource = classes;
                cbClasse.DisplayMember = "NomClasse";
                cbClasse.ValueMember = "Id";
                cbClasse.SelectedIndex = 0;  // Sélectionner par défaut la première option
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors du chargement des classes : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbClasse_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lorsque la classe sélectionnée change, afficher les étudiants correspondants
            AfficherEtudiantsParClasse();
        }

        private void AfficherEtudiantsParClasse()
        {
            try
            {
                // Vérifier si SelectedValue est valide et peut être converti en entier
                int classeId;
                if (cbClasse.SelectedValue == null || !int.TryParse(cbClasse.SelectedValue.ToString(), out classeId))
                {
                    // Si SelectedValue est invalide, afficher tous les étudiants
                    classeId = 0;
                }

                List<Etudiants> etudiants;

                if (classeId == 0)
                {
                    // Si aucune classe n'est sélectionnée, afficher tous les étudiants
                    etudiants = _context.Etudiant.Include(e => e.Classe).ToList();
                }
                else
                {
                    // Afficher les étudiants d'une classe spécifique
                    etudiants = _context.Etudiant.Where(e => e.ClasseId == classeId).Include(e => e.Classe).ToList();
                }

                // Appliquer le filtre de recherche si un texte est saisi
                string searchTerm = txtRecherche.Text.ToLower(); // Recherche insensible à la casse
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    etudiants = etudiants.Where(e =>
                        e.Nom.ToLower().Contains(searchTerm) || 
                        e.Prenom.ToLower().Contains(searchTerm) || 
                        e.Matricule.ToLower().Contains(searchTerm)).ToList();
                }
                // Ajouter le nom de la classe à chaque étudiant pour affichage dans le DataGridView
                var result = etudiants.Select(e => new
                {
                    e.Matricule,
                    e.Nom,
                    e.Prenom,
                    e.DateNaissance,
                    e.Sexe,
                    e.Adresse,
                    e.Telephone,
                    e.Email,
                    ClasseNom = e.Classe.NomClasse // Ajouter ici le nom de la classe
                }).ToList();
                // Afficher les étudiants dans le DataGridView
                dataGridViewEtudiants.DataSource = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors du chargement des étudiants : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnRechercher_Click(object sender, EventArgs e)
        {
            // Lorsque le bouton Rechercher est cliqué, appliquer la recherche
            AfficherEtudiantsParClasse();
        }

        private void cbClasse_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lorsque la classe sélectionnée change, afficher les étudiants correspondants et appliquer la recherche
            AfficherEtudiantsParClasse();
        }

        private void txtRecherche_TextChanged(object sender, EventArgs e)
        {
            // Lorsque la classe sélectionnée change, afficher les étudiants correspondants et appliquer la recherche
            AfficherEtudiantsParClasse();
        }

        private void btnCharger_Click(object sender, EventArgs e)
        {
            // Ouvrir le formulaire DownloadForm
            DownProfForm downloadForm = new DownProfForm();
            downloadForm.ShowDialog(); // Utiliser ShowDialog() pour ouvrir en mode modal
        }
    }
}
