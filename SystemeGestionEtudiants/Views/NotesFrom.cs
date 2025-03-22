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
    public partial class NotesFrom: MaterialForm
    {
        private ApplicationDbContext _context;
        public NotesFrom()
        {
            InitializeComponent();
            _context = new ApplicationDbContext();

            

            // Charger les classes disponibles
            LoadClasses();
            // Désactiver le bouton "Ajouter" par défaut
            btnAdd.Enabled = false;
            dgvNotes.ReadOnly = true;
            cmbEtudiant.DropDownStyle = ComboBoxStyle.DropDownList;
            cbClasse.DropDownStyle = ComboBoxStyle.DropDownList;

            // Abonnement à l'événement SelectedIndexChanged de cbClasse
            cbClasse.SelectedIndexChanged += cbClasse_SelectedIndexChanged;
            // Abonnement à l'événement SelectedIndexChanged de cmbEtudiant
            cmbEtudiant.SelectedIndexChanged += cmbEtudiant_SelectedIndexChanged;
            // Abonnement à l'événement TextChanged de txtNote
            txtNote.TextChanged += txtNote_TextChanged;

        }
        // Charger les classes disponibles dans le ComboBox
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

        private void NotesFrom_Load(object sender, EventArgs e)
        {

        }
        /*private void LoadEtudiants(int classeId)
        {
            try
            {
                // Récupérer les étudiants associés à la classe
                var etudiants = _context.Etudiant
                    .Where(e => e.ClasseId == classeId)  // Filtrer par l'ID de la classe
                    .ToList();

                // Ajouter une option vide pour permettre à l'utilisateur de choisir un étudiant
                etudiants.Insert(0, new Etudiants { Id = 0, Nom = "Sélectionnez un étudiant" });

                // Lier les étudiants au ComboBox
                cmbEtudiant.DataSource = etudiants;
                cmbEtudiant.DisplayMember = "Nom";  // Afficher le nom de l'étudiant
                cmbEtudiant.ValueMember = "Id";  // Utiliser l'ID de l'étudiant comme valeur

                // Sélectionner la première option par défaut
                cmbEtudiant.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des étudiants : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/
        private void LoadEtudiants(int classeId)
        {
            try
            {
                // Récupérer les étudiants associés à la classe
                var etudiants = _context.Etudiant
                    .Where(e => e.ClasseId == classeId)  // Filtrer par l'ID de la classe
                    .Select(e => new
                    {
                        Id = e.Id, // ID de l'étudiant
                        NomComplet = e.Prenom + " " + e.Nom, // Nom complet (nom + prénom)
                        Matricule = e.Matricule // Matricule de l'étudiant
                    })
                    .ToList();

                // Ajouter une option vide pour permettre à l'utilisateur de choisir un étudiant
                etudiants.Insert(0, new { Id = 0, NomComplet = "Sélectionnez un étudiant", Matricule = "" });

                // Lier les étudiants au ComboBox
                cmbEtudiant.DataSource = etudiants;
                cmbEtudiant.DisplayMember = "NomComplet";  // Afficher le nom complet
                cmbEtudiant.ValueMember = "Id";  // Utiliser l'ID de l'étudiant comme valeur

                // Sélectionner la première option par défaut
                cmbEtudiant.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des étudiants : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*private void LoadCours(int classeId)
        {
            try
            {
                // Charger les cours associés à la classe via ClasseCours
                var cours = _context.ClasseCours
                    .Where(cc => cc.ClasseId == classeId)  // Filtrer par l'ID de la classe
                    .Select(cc => cc.Cours)  // Sélectionner les Cours associés à la classe
                    .Distinct()
                    .ToList();

                if (cours.Count == 0)
                {
                    MessageBox.Show("Aucun cours trouvé pour cette classe.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Ajouter une option vide pour obliger l'utilisateur à sélectionner un cours
                cours.Insert(0, new Cours { Id = 0, NomCours = "Sélectionnez un cours" });

                // Lier les cours à la ComboBox
                cmbCours.DataSource = cours;
                cmbCours.DisplayMember = "NomCours";  // Afficher le nom du cours
                cmbCours.ValueMember = "Id";  // Utiliser l'ID du cours comme valeur

                // S'assurer qu'un cours est sélectionné par défaut
                cmbCours.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des cours : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/

        /* private void btnAdd_Click(object sender, EventArgs e)
         {
             // Vérifier que l'étudiant, la matière et la note sont bien renseignés
             /* if (cmbEtudiant.SelectedValue == null || cmbMatiere.SelectedValue == null || string.IsNullOrEmpty(txtNote.Text))
              {
                  MessageBox.Show("Veuillez sélectionner un étudiant, un cours et entrer une note.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  return;
              }*/
        // Vérifier que l'étudiant est sélectionné
        /* if (cmbEtudiant.SelectedValue == null || (int)cmbEtudiant.SelectedValue == 0)
         {
             MessageBox.Show("Veuillez sélectionner un étudiant.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
             return;
         }
         try
         {
             int studentId = (int)cmbEtudiant.SelectedValue;

             // Parcourir les TextBox pour récupérer les notes
             foreach (Control control in panelMatieres.Controls)
             {
                 if (control is TextBox textBox && textBox.Name.StartsWith("txtNote_"))
                 {
                     // Récupérer l'ID de la matière à partir du nom du TextBox
                     int matiereId = int.Parse(textBox.Name.Split('_')[1]);

                     // Vérifier que la note est un nombre valide et compris entre 0 et 20
                     if (float.TryParse(textBox.Text, out float note) && note >= 0 && note <= 20)
                     {
                         var noteEntity = new Note
                         {
                             StudentId = studentId,
                             MatiereId = matiereId,
                             NoteValue = note
                         };

                         // Ajouter la note dans la base de données
                         _context.Notes.Add(noteEntity);
                     }
                     else
                     {
                         MessageBox.Show($"La note pour la matière {matiereId} doit être un nombre compris entre 0 et 20.", "Erreur de saisie", MessageBoxButtons.OK, MessageBoxIcon.Error);
                         return; // Arrêter l'exécution de la méthode en cas d'erreur
                     }
                 }
             }
             // Sauvegarder les changements dans la base de données
             _context.SaveChanges();

             MessageBox.Show("Notes ajoutées avec succès", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
         catch (Exception ex)
         {
             MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         /* try
          {
              var note = new Note
              {
                  StudentId = (int)cmbEtudiant.SelectedValue,
                  MatiereId = (int)cmbMatiere.SelectedValue,
                  NoteValue = float.Parse(txtNote.Text)
              };

              // Ajouter la note dans la base de données
              _context.Notes.Add(note);
              _context.SaveChanges();

              MessageBox.Show("Note ajoutée avec succès", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
          catch (Exception ex)
          {
              MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }*/
        //}

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Vérifier qu'un étudiant est sélectionné
            if (cmbEtudiant.SelectedValue == null || (int)cmbEtudiant.SelectedValue == 0)
            {
                MessageBox.Show("Veuillez sélectionner un étudiant.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int studentId = (int)cmbEtudiant.SelectedValue;

                // Parcourir les TextBox pour récupérer les notes
                foreach (Control control in panelMatieres.Controls)
                {
                    if (control is TextBox textBox && textBox.Name.StartsWith("txtNote_"))
                    {
                        // Récupérer l'ID de la matière à partir du nom du TextBox
                        int matiereId = int.Parse(textBox.Name.Split('_')[1]);

                        // Vérifier que la note est un nombre valide et compris entre 0 et 20
                        if (float.TryParse(textBox.Text, out float note) && note >= 0 && note <= 20)
                        {
                            // Vérifier si une note existe déjà pour cet étudiant et cette matière
                            bool noteExiste = _context.Notes
                                .Any(n => n.StudentId == studentId && n.MatiereId == matiereId);

                            if (noteExiste)
                            {
                                MessageBox.Show($"Une note existe déjà pour cet étudiant dans cette matière.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return; // Arrêter l'exécution de la méthode
                            }

                            // Créer une nouvelle note
                            var noteEntity = new Note
                            {
                                StudentId = studentId,
                                MatiereId = matiereId,
                                NoteValue = note
                            };

                            // Ajouter la note dans la base de données
                            _context.Notes.Add(noteEntity);
                        }
                        else
                        {
                            MessageBox.Show($"La note pour la matière {matiereId} doit être un nombre compris entre 0 et 20.", "Erreur de saisie", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; // Arrêter l'exécution de la méthode en cas d'erreur
                        }
                    }
                }

                // Sauvegarder les changements dans la base de données
                _context.SaveChanges();

                MessageBox.Show("Notes ajoutées avec succès", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Vider les champs après l'ajout
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbEtudiant_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* // Vérifier si un étudiant est sélectionné
             if (cmbEtudiant.SelectedValue != null && int.TryParse(cmbEtudiant.SelectedValue.ToString(), out int studentId) && studentId != 0)
             {
                 // Activer le bouton "Ajouter"
                 btnAdd.Enabled = true;

                 // Récupérer l'étudiant sélectionné
                 var etudiantSelectionne = (dynamic)cmbEtudiant.SelectedItem;

                 // Afficher le matricule dans le TextBox
                 txtMatricule.Text = etudiantSelectionne.Matricule;
             }
             else
             {
                 // Désactiver le bouton "Ajouter" et vider le TextBox
                 btnAdd.Enabled = false;
                 txtMatricule.Text = string.Empty;
             }*/
            // Vérifier si un étudiant est sélectionné
            if (cmbEtudiant.SelectedValue != null && int.TryParse(cmbEtudiant.SelectedValue.ToString(), out int studentId) && studentId != 0)
            {
                // Activer le bouton "Ajouter"
                btnAdd.Enabled = true;

                // Récupérer l'étudiant sélectionné
                var etudiantSelectionne = (dynamic)cmbEtudiant.SelectedItem;

                // Afficher le matricule dans le TextBox
                txtMatricule.Text = etudiantSelectionne.Matricule;

                // Charger les notes existantes de l'étudiant
                LoadNotesEtudiant(studentId);
            }
            else
            {
                // Désactiver le bouton "Ajouter" et vider le TextBox
                btnAdd.Enabled = false;
                txtMatricule.Text = string.Empty;

                // Vider le DataGridView
                dgvNotes.DataSource = null;
            }

        }
        private void LoadNotesEtudiant(int studentId)
        {
            try
            {
                // Récupérer les notes de l'étudiant
                var notes = _context.Notes
                    .Where(n => n.StudentId == studentId)
                    .Select(n => new
                    {
                        Matiere = n.Matiere.NomMatiere, // Nom de la matière
                        Note = n.NoteValue // Note
                    })
                    .ToList();

                // Afficher les notes dans le DataGridView
                dgvNotes.DataSource = notes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des notes : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Quand la classe change, on charge les matières disponibles pour cette classe
        private void cbClasse_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Vérifier si une classe est sélectionnée
            if (cbClasse.SelectedValue != null && cbClasse.SelectedValue is int classeId && classeId != 0)
            {
                // Charger les étudiants et les matières associés à la classe sélectionnée
                LoadEtudiants(classeId);
                LoadMatieres(classeId);
            }
            else
            {
                // Si aucune classe n'est sélectionnée, vider les ComboBox des étudiants et des matières
                cmbEtudiant.DataSource = null;
                cmbMatiere.DataSource = null;
            }
        }


        // Charger les matières de la classe sélectionnée
        private void LoadMatieres(int classeId)
        {
            try
            {
                // Récupérer les matières associées à la classe via ClasseCours et CoursMatiere
                var matieres = _context.ClasseCours
                    .Where(cc => cc.ClasseId == classeId)  // Filtrer par l'ID de la classe
                    .SelectMany(cc => cc.Cours.CoursMatieres)  // Récupérer les matières associées aux cours de la classe
                    .Select(cm => cm.Matiere)  // Sélectionner les matières
                    .Distinct()  // Éviter les doublons
                    .ToList();

                // Vider les contrôles existants
                panelMatieres.Controls.Clear();

                // Créer des champs dynamiques pour chaque matière
                int yOffset = 10; // Espacement vertical initial
                foreach (var matiere in matieres)
                {
                    // Créer un Label pour la matière
                    var label = new Label
                    {
                        Text = matiere.NomMatiere,
                        AutoSize = false, // Désactiver l'auto-size pour définir une largeur fixe
                        Width = 200, // Largeur fixe pour le Label
                        Location = new Point(10, yOffset) // Positionnement vertical
                    };

                    // Créer un TextBox pour la note
                    var textBox = new TextBox
                    {
                        Name = $"txtNote_{matiere.Id}",
                        Location = new Point(220, yOffset), // Positionnement horizontal après le Label
                        Width = 100
                    };

                    // Ajouter les contrôles au panel
                    panelMatieres.Controls.Add(label);
                    panelMatieres.Controls.Add(textBox);

                    // Augmenter l'espacement vertical pour la prochaine matière
                    yOffset += 30; // Espacement de 30 pixels entre chaque matière
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des matières : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Contrôle de saisie des notes
        private void txtNote_TextChanged(object sender, EventArgs e)
        {
            // Vérifier si la saisie est un nombre valide
            if (float.TryParse(txtNote.Text, out float note))
            {
                // Vérifier que la note est comprise entre 0 et 20
                if (note < 0 || note > 20)
                {
                    MessageBox.Show("La note doit être comprise entre 0 et 20.", "Erreur de saisie", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtNote.Text = "";  // Effacer la saisie invalide
                }
            }
            else if (!string.IsNullOrEmpty(txtNote.Text))
            {
                // Si la saisie n'est pas un nombre, afficher un message d'erreur
                MessageBox.Show("Veuillez entrer un nombre valide.", "Erreur de saisie", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNote.Text = "";  // Effacer la saisie invalide
            }
        }
        private void ClearFields()
        {
            // Vider les champs de saisie des notes
            foreach (Control control in panelMatieres.Controls)
            {
                if (control is TextBox textBox && textBox.Name.StartsWith("txtNote_"))
                {
                    textBox.Text = string.Empty;
                }
            }

            // Réinitialiser la sélection de l'étudiant
            cmbEtudiant.SelectedIndex = 0;

            // Vider le TextBox du matricule
            txtMatricule.Text = string.Empty;

            // Désactiver le bouton "Ajouter"
            btnAdd.Enabled = false;

            // Vider le DataGridView des notes existantes
            dgvNotes.DataSource = null;
        }


        private void cmbCours_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCours.SelectedValue is int coursId && coursId != 0)
            {
                // Charger les matières associées au cours sélectionné
                //  LoadMatieres(coursId);
            }
            else
            {
                // Si aucun cours n'est sélectionné, vider la ComboBox des matières
                // cmbMatiere.DataSource = null;
            }
        }

        /* private void LoadMatieres(int classeId)
         {
             try
             {
                 // Récupérer les matières associées à la classe via ClasseCours et CoursMatiere
                 var matieres = _context.ClasseCours
                     .Where(cc => cc.ClasseId == classeId)  // Filtrer par l'ID de la classe
                     .SelectMany(cc => cc.Cours.CoursMatieres)  // Récupérer les matières associées aux cours de la classe
                     .Select(cm => cm.Matiere)  // Sélectionner les matières
                     .Distinct()  // Éviter les doublons
                     .ToList();

                 // Ajouter une option vide pour permettre à l'utilisateur de choisir une matière
                 matieres.Insert(0, new Matiere { Id = 0, NomMatiere = "Sélectionnez une matière" });

                 // Lier les matières au ComboBox
                 cmbMatiere.DataSource = matieres;
                 cmbMatiere.DisplayMember = "NomMatiere";  // Afficher le nom de la matière
                 cmbMatiere.ValueMember = "Id";  // Utiliser l'ID de la matière comme valeur

                 // Sélectionner la première option par défaut
                 cmbMatiere.SelectedIndex = 0;
             }
             catch (Exception ex)
             {
                 MessageBox.Show($"Erreur lors du chargement des matières : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
             }
         }*/


    }
}
