using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Windows.Forms;
using SystemeGestionEtudiants.Models;

namespace SystemeGestionEtudiants.Views
{
    public partial class A2FForm : MaterialForm
    {
        private string _codeA2F;
        private Utilisateur _utilisateur;

        public A2FForm(string codeA2F, Utilisateur utilisateur)
        {
            InitializeComponent();
            _codeA2F = codeA2F;
            _utilisateur = utilisateur;

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

        

        private void OuvrirInterfaceUtilisateur(Utilisateur utilisateur)
        {

            
                    var adminForm = new MainForm(utilisateur.Role);
                    adminForm.Show();
     
        }

        private void btnValider_Click(object sender, EventArgs e)
        {
            if (txtCodeA2F.Text == _codeA2F)
            {
                // Rediriger en fonction du rôle
                OuvrirInterfaceUtilisateur(_utilisateur);
                this.Close();

            }
            else
            {
                MessageBox.Show("Code A2F incorrect.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtCodeA2F_TextChanged(object sender, EventArgs e)
        {

        }
    }
}