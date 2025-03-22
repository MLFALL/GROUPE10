using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemeGestionEtudiants.Views;

namespace SystemeGestionEtudiants
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run (new MeilleursEtudiantsForm());
            // Boucle pour gérer la transition entre LoginForm et MainForm
            while (true)
            {
                // Afficher la LoginForm
                using (LoginForm loginForm = new LoginForm())
                {
                    if (loginForm.ShowDialog() != DialogResult.OK)
                    {
                        break; // Quitter la boucle si l'utilisateur annule la connexion
                    }

                    // Si la connexion réussit, afficher la MainForm
                    using (MainForm mainForm = new MainForm(loginForm.UserRole))
                    {
                        Application.Run(mainForm);

                        // Si l'utilisateur se déconnecte, revenir à la LoginForm
                        if (mainForm.IsDisposed)
                        {
                            continue;
                        }
                    }
                }
            }
        }
    }
}
