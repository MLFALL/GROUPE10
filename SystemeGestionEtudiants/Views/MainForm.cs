using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemeGestionEtudiants.Views
{
    public partial class MainForm : Form
    {
        private ContextMenuStrip rapportMenu;
        private string userRole;

        public MainForm(string role)
        {
            InitializeComponent();
            this.userRole = role;
            InitializeSidebar();
            InitializeContentPanel();
            InitializeRapportMenu();

            // Définir la taille de la fenêtre principale
            this.Size = new Size(1200, 600); // Taille augmentée (1200x800)
            this.Text = "Système de Gestion des Étudiants";
            this.StartPosition = FormStartPosition.CenterScreen; // Centrer la fenêtre

            // Afficher le Dashboard par défaut au démarrage
            LoadForm(new DashboardForm());
        }

        private void InitializeSidebar()
        {
            // Panel pour la sidebar
            Panel sidebarPanel = new Panel();
            sidebarPanel.Dock = DockStyle.Left;
            sidebarPanel.Width = 200;
            sidebarPanel.BackColor = Color.FromArgb(33, 150, 243); // Bleu clair

            // Boutons pour les menus (dans l'ordre spécifié)
            Button btnDashboard = CreateMenuButton("   Dashboard", Properties.Resources.dashboar_icon, BtnDashboard_Click);
            Button btnEtudiants = CreateMenuButton("   Étudiants", Properties.Resources.students_icon, BtnEtudiants_Click);
            Button btnClasses = CreateMenuButton("   Classes", Properties.Resources.classes_icon, BtnClasses_Click);
            Button btnCours = CreateMenuButton("   Cours", Properties.Resources.courses_icon, BtnCours_Click);
            Button btnNotes = CreateMenuButton("   Notes", Properties.Resources.notes_icon, BtnNotes_Click);
            Button btnProfesseurs = CreateMenuButton("   Professeurs", Properties.Resources.teachers_icon, BtnProfesseurs_Click);
            Button btnUsers = CreateMenuButton("   Utilisateurs", Properties.Resources.users_icon, BtnUsers_Click);
            Button btnRapports = CreateMenuButton("   Rapports", Properties.Resources.reports_icon, BtnRapports_Click);



            // Bouton Déconnexion en bas
            Button btnDeconnexion = CreateMenuButton("   Déconnexion", Properties.Resources.deconnexion, BtnDeconnexion_Click);
            btnDeconnexion.Dock = DockStyle.Bottom; // Placer en bas



            switch (userRole)
            {
                case "Administrateur":
                    sidebarPanel.Controls.Add(btnRapports);
                    sidebarPanel.Controls.Add(btnUsers);
                    sidebarPanel.Controls.Add(btnProfesseurs);
                    sidebarPanel.Controls.Add(btnNotes);
                    sidebarPanel.Controls.Add(btnCours);
                    sidebarPanel.Controls.Add(btnClasses);
                    sidebarPanel.Controls.Add(btnEtudiants);
                    sidebarPanel.Controls.Add(btnDashboard);
                    sidebarPanel.Controls.Add(btnDeconnexion);
                    break;

                case "Agent":
                    sidebarPanel.Controls.Add(btnNotes);
                    sidebarPanel.Controls.Add(btnEtudiants);
                    sidebarPanel.Controls.Add(btnDashboard);
                    sidebarPanel.Controls.Add(btnDeconnexion);
                    break;

                case "DE":
                    sidebarPanel.Controls.Add(btnRapports);
                    sidebarPanel.Controls.Add(btnProfesseurs);
                    sidebarPanel.Controls.Add(btnCours);
                    sidebarPanel.Controls.Add(btnClasses);
                    sidebarPanel.Controls.Add(btnDashboard);
                    sidebarPanel.Controls.Add(btnDeconnexion);
                    break;
            }

            // Ajouter le panel au formulaire
            this.Controls.Add(sidebarPanel);
        }

        private Button CreateMenuButton(string text, Image icon, EventHandler clickHandler)
        {
            Button button = new Button();
            button.Text = text;
            button.Image = icon; // Ajouter l'icône
            button.Dock = DockStyle.Top;
            button.Height = 40;
            button.FlatStyle = FlatStyle.Flat;
            button.ForeColor = Color.White;
            button.BackColor = Color.FromArgb(33, 150, 243); // Bleu clair
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            button.TextAlign = ContentAlignment.MiddleLeft; // Aligner le texte à gauche
            button.ImageAlign = ContentAlignment.MiddleLeft; // Aligner l'icône à gauche
            button.TextImageRelation = TextImageRelation.ImageBeforeText; // Icône avant le texte
            button.Click += clickHandler;

            // Effet de survol
            button.MouseEnter += (sender, e) =>
            {
                button.BackColor = Color.FromArgb(25, 118, 210); // Bleu plus foncé au survol
            };
            button.MouseLeave += (sender, e) =>
            {
                button.BackColor = Color.FromArgb(33, 150, 243); // Retour au bleu clair
            };

            return button;
        }

        /*private void InitializeContentPanel()
        {
            // Panel pour le contenu principal
            Panel contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = Color.FromArgb(240, 240, 240); // Gris clair pour le contenu

            // Ajouter le panel au formulaire
            this.Controls.Add(contentPanel);
        }*/
        private void InitializeContentPanel()
        {
            // Panel pour le contenu principal
            Panel contentPanel = new Panel();
            contentPanel.BackColor = Color.FromArgb(240, 240, 240); // Gris clair pour le contenu

            // Positionner le contentPanel à droite de la sidebar
            contentPanel.Location = new Point(200, 0); // 200 = largeur de la sidebar
            contentPanel.Size = new Size(this.ClientSize.Width - 200, this.ClientSize.Height);

            // Redimensionner dynamiquement le contentPanel lorsque la fenêtre est redimensionnée
            this.Resize += (sender, e) =>
            {
                contentPanel.Size = new Size(this.ClientSize.Width - 200, this.ClientSize.Height);
            };

            // Ajouter le panel au formulaire
            this.Controls.Add(contentPanel);
        }

        private void InitializeRapportMenu()
        {
            // Créer le menu contextuel pour les rapports
            rapportMenu = new ContextMenuStrip();
            rapportMenu.BackColor = Color.FromArgb(33, 150, 243); // Bleu clair
            rapportMenu.ForeColor = Color.White;

            // Ajouter les options au menu
            rapportMenu.Items.Add("Liste des Étudiants", null, RapportMenuItem_Click);
            rapportMenu.Items.Add("Liste des Profs", null, RapportMenuItem_Click);
            rapportMenu.Items.Add("Liste des Classes", null, RapportMenuItem_Click);
            rapportMenu.Items.Add("Bulletin", null, RapportMenuItem_Click);
            rapportMenu.Items.Add("Meilleurs Étudiants", null, RapportMenuItem_Click);

            // Style des éléments du menu
            foreach (ToolStripMenuItem item in rapportMenu.Items)
            {
                item.BackColor = Color.FromArgb(33, 150, 243); // Bleu clair
                item.ForeColor = Color.White;
                item.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            }
        }

        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            LoadForm(new DashboardForm());
        }

        private void BtnEtudiants_Click(object sender, EventArgs e)
        {
            LoadForm(new EtudiantsForm());
        }

        private void BtnClasses_Click(object sender, EventArgs e)
        {
            LoadForm(new ClassForm());
        }

        private void BtnCours_Click(object sender, EventArgs e)
        {
            LoadForm(new CoursForm());
        }

        private void BtnNotes_Click(object sender, EventArgs e)
        {
            LoadForm(new NotesFrom());
        }

        private void BtnProfesseurs_Click(object sender, EventArgs e)
        {
            LoadForm(new ProfesseurForm());
        }

        private void BtnUsers_Click(object sender, EventArgs e)
        {
            LoadForm(new UserForm());
        }

        private void BtnRapports_Click(object sender, EventArgs e)
        {
            // Afficher le menu contextuel sous le bouton Rapports
            rapportMenu.Show((Button)sender, new Point(0, ((Button)sender).Height));
        }

        private void RapportMenuItem_Click(object sender, EventArgs e)
        {
            // Récupérer l'élément sélectionné dans le menu
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem != null)
            {
                string selectedOption = menuItem.Text;

                // Charger le formulaire correspondant à l'option sélectionnée
                switch (selectedOption)
                {
                    case "Liste des Étudiants":
                        LoadForm(new EtudiantsParClasseForm());
                        break;
                    case "Liste des Profs":
                        LoadForm(new ProfesseursParClasseForm());
                        break;
                    case "Liste des Classes":
                        LoadForm(new DownloadForm());
                        break;
                    case "Bulletin":
                        LoadForm(new BulletinForm());
                        break;
                    case "Meilleurs Étudiants":
                        LoadForm(new MeilleursEtudiantsForm());
                        break;
                }
            }
        }

        private void BtnDeconnexion_Click(object sender, EventArgs e)
        {
            // Fermer la MainForm
            this.Close();

            // Ouvrir la LoginForm
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }

        /* private void LoadForm(Form form)
         {
             // Vider le panel de contenu
             foreach (Control control in this.Controls)
             {
                 if (control is Panel && control.Dock == DockStyle.Fill)
                 {
                     control.Controls.Clear();
                     form.TopLevel = false;
                     form.FormBorderStyle = FormBorderStyle.None;
                     form.Dock = DockStyle.Fill; // Adapter le formulaire à la taille du panel
                     control.Controls.Add(form);
                     form.Show();
                     break;
                 }
             }
         }*/
        private void LoadForm(Form form)
        {
            // Trouver le panel de contenu
            Panel contentPanel = this.Controls.OfType<Panel>()
                .FirstOrDefault(p => p.BackColor == Color.FromArgb(240, 240, 240)); // Identifier le contentPanel par sa couleur

            if (contentPanel != null)
            {
                // Vider le panel de contenu
                contentPanel.Controls.Clear();

                // Configurer le formulaire
                form.TopLevel = false; // Indiquer que le formulaire n'est pas une fenêtre de niveau supérieur
                form.FormBorderStyle = FormBorderStyle.None; // Supprimer les bordures du formulaire

                // Ajuster la taille du formulaire pour qu'il s'adapte au contentPanel
                form.Size = contentPanel.Size;

                // Ajouter le formulaire au contentPanel
                contentPanel.Controls.Add(form);
                // Gérer l'événement FormClosed pour revenir au Dashboard
                form.FormClosed += (s, args) => LoadForm(new DashboardForm());
                form.Show();
            }
            else
            {
                MessageBox.Show("Le panel de contenu est introuvable.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}
