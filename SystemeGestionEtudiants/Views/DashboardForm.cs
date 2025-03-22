using System;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using SystemeGestionEtudiants.Data;
using SystemeGestionEtudiants.Models;
using LiveCharts; // Pour les graphiques
using LiveCharts.WinForms;
using LiveCharts.Wpf;
using CartesianChart = LiveCharts.WinForms.CartesianChart;
using PieChart = LiveCharts.WinForms.PieChart; // Pour les graphiques

namespace SystemeGestionEtudiants.Views
{
    public partial class DashboardForm : Form
    {

        private ApplicationDbContext _context;

        public DashboardForm()
        {
            InitializeComponent();
            _context = new ApplicationDbContext();
            LoadData();


        }

        private void LoadData()
        {
            try
            {
                // Récupérer les données de la base de données
                int nombreClasses = _context.Classes.Count();
                int nombreEtudiants = _context.Etudiant.Count();
                int nombreProfesseurs = _context.Professeurs.Count();

                // Afficher les statistiques dans des labels
                lblNombreClasses.Text = nombreClasses.ToString();
                lblNombreEtudiants.Text = nombreEtudiants.ToString();
                lblNombreProfesseurs.Text = nombreProfesseurs.ToString();

                lblPresentation.Text = "Bienvenue dans le système de gestion de l'école XYZ." + Environment.NewLine +
                        "Nous avons actuellement " + nombreClasses + " classes," + Environment.NewLine +
                        nombreEtudiants + " étudiants et " + nombreProfesseurs + " professeurs.";

                // Créer des graphiques
                CreateCharts(nombreClasses, nombreEtudiants, nombreProfesseurs);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateCharts(int nombreClasses, int nombreEtudiants, int nombreProfesseurs)
        {
            // Graphique à barres pour les statistiques
            var barChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Classes",
                        Values = new ChartValues<int> { nombreClasses }
                    },
                    new ColumnSeries
                    {
                        Title = "Étudiants",
                        Values = new ChartValues<int> { nombreEtudiants }
                    },
                    new ColumnSeries
                    {
                        Title = "Professeurs",
                        Values = new ChartValues<int> { nombreProfesseurs }
                    }
                },
                AxisX = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Catégories",
                        Labels = new[] { "Classes", "Étudiants", "Professeurs" }
                    }
                },
                AxisY = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Nombre",
                        LabelFormatter = value => value.ToString("N0")
                    }
                }
            };

            // Graphique camembert pour la répartition
            var pieChart = new PieChart
            {
                Series = new SeriesCollection
                {
                    new PieSeries
                    {
                        Title = "Classes",
                        Values = new ChartValues<int> { nombreClasses },
                        DataLabels = true
                    },
                    new PieSeries
                    {
                        Title = "Étudiants",
                        Values = new ChartValues<int> { nombreEtudiants },
                        DataLabels = true
                    },
                    new PieSeries
                    {
                        Title = "Professeurs",
                        Values = new ChartValues<int> { nombreProfesseurs },
                        DataLabels = true
                    }
                }
            };

            // Ajouter les graphiques au formulaire
            barChart.Dock = DockStyle.Top;
            pieChart.Dock = DockStyle.Bottom;

            this.Controls.Add(barChart);
            this.Controls.Add(pieChart);
        }

        private void lblNombreProfesseurs_Click(object sender, EventArgs e)
        {

        }
    }
}