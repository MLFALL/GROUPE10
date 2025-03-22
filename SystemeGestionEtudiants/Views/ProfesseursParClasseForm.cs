using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Windows.Forms;
using SystemeGestionEtudiants.Data;
using SystemeGestionEtudiants.Models;
using MaterialSkin.Controls;
using MaterialSkin;

namespace SystemeGestionEtudiants.Views
{
    public partial class ProfesseursParClasseForm : MaterialForm
    {
        private ApplicationDbContext _context;
        private List<Professeur> _tousLesProfesseurs;  // Liste complète des professeurs

        public ProfesseursParClasseForm()
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

            // Charger tous les professeurs et les classes au démarrage du formulaire
            LoadTousLesProfesseurs();
            LoadClasses();
        }

        // Charger tous les professeurs avec leurs classes et matières
        private void LoadTousLesProfesseurs()
        {
            try
            {
                // Récupérer tous les professeurs avec leurs classes et matières associées
                var professeurs = _context.Professeurs
                    .Include(p => p.ProfesseurClasses.Select(pc => pc.Classe))  // Charger les classes associées
                    .Include(p => p.ProfesseurMatieres.Select(pm => pm.Matiere))  // Charger les matières associées
                    .ToList();

                // Formater les données pour le DataGridView
                var data = professeurs.Select(p => new
                {
                    p.Id,
                    p.Nom,
                    p.Prenom,
                    p.Email,
                    p.Telephone,
                    MatieresEnseignees = string.Join(", ", p.ProfesseurMatieres.Select(pm => pm.Matiere.NomMatiere)),  // Liste des matières
                    ClassesAssociees = string.Join(", ", p.ProfesseurClasses.Select(pc => pc.Classe.NomClasse))        // Liste des classes
                }).ToList();

                // Afficher tous les professeurs dans le DataGridView au démarrage
                dataGridViewProfesseurs.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des professeurs : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Charger les classes dans le ComboBox
        private void LoadClasses()
        {
            try
            {
                // Récupérer toutes les classes
                var classes = _context.Classes.ToList();

                // Ajouter une option vide pour permettre à l'utilisateur de choisir une classe
                classes.Insert(0, new Classe { Id = 0, NomClasse = "Sélectionnez une classe" });

                // Lier les classes au ComboBox
                cbClasse.DataSource = classes;
                cbClasse.DisplayMember = "NomClasse";  // Afficher le nom de la classe
                cbClasse.ValueMember = "Id";  // Utiliser l'ID de la classe comme valeur

                // Sélectionner la première option par défaut
                cbClasse.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des classes : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Filtrer les professeurs par classe
        private void FiltrerProfesseursParClasse(int classeId)
        {
            try
            {
                if (classeId == 0)
                {
                    // Si aucune classe n'est sélectionnée, afficher tous les professeurs
                    LoadTousLesProfesseurs();
                }
                else
                {
                    // Récupérer les professeurs associés à la classe sélectionnée
                    var professeursFiltres = _context.Professeurs
                        .Include(p => p.ProfesseurClasses.Select(pc => pc.Classe))
                        .Include(p => p.ProfesseurMatieres.Select(pm => pm.Matiere))
                        .Where(p => p.ProfesseurClasses.Any(pc => pc.ClasseId == classeId))
                        .ToList();

                    // Formater les données pour le DataGridView
                    var data = professeursFiltres.Select(p => new
                    {
                        p.Id,
                        p.Nom,
                        p.Prenom,
                        p.Email,
                        p.Telephone,
                        MatieresEnseignees = string.Join(", ", p.ProfesseurMatieres.Select(pm => pm.Matiere.NomMatiere)),
                        ClassesAssociees = string.Join(", ", p.ProfesseurClasses.Select(pc => pc.Classe.NomClasse))
                    }).ToList();

                    // Afficher les professeurs filtrés dans le DataGridView
                    dataGridViewProfesseurs.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du filtrage des professeurs : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Gérer l'événement SelectedIndexChanged du ComboBox des classes
        private void cbClasse_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Vérifier si une classe est sélectionnée
            if (cbClasse.SelectedValue != null && cbClasse.SelectedValue is int classeId)
            {
                // Filtrer les professeurs par classe
                FiltrerProfesseursParClasse(classeId);
            }
        }

        private void btnVoir_Click(object sender, EventArgs e)
        {
            // Ouvrir le formulaire DownloadForm
            ListeProfsForm downloadForm = new ListeProfsForm();
            downloadForm.ShowDialog();
        }
    }
}