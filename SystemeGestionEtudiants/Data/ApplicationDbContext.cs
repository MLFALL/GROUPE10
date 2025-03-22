using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using SystemeGestionEtudiants.Models;

namespace SystemeGestionEtudiants.Data
{
    class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext():base("ecoleConnect")
        {

        }

        public DbSet<Etudiants> Etudiant { get; set; }
            public DbSet<Classe> Classes { get; set; }
            public DbSet<Cours> Cours { get; set; }
            public DbSet<Matiere> Matieres { get; set; }
            public DbSet<Professeur> Professeurs { get; set; }
            public DbSet<Note> Notes { get; set; }
            public DbSet<Utilisateur> Utilisateurs { get; set; }
            public DbSet<OTPCode> OTPCodes { get; set; }
            public DbSet<ClasseCours> ClasseCours { get; set; }
            public DbSet<CoursMatiere> CoursMatieres { get; set; }
            public DbSet<ProfesseurMatiere> ProfesseurMatieres { get; set; }
            public DbSet<ProfesseurClasse> ProfesseurClasses { get; set; }
        public object CodesA2F { get; internal set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configuration de la clé composite pour ClasseCours
            modelBuilder.Entity<ClasseCours>()
                .HasKey(cc => new { cc.ClasseId, cc.CoursId });

            // Configuration de la relation entre Classe et ClasseCours
            modelBuilder.Entity<ClasseCours>()
                .HasRequired(cc => cc.Classe)
                .WithMany(c => c.ClasseCours)
                .HasForeignKey(cc => cc.ClasseId);

            // Configuration de la relation entre Cours et ClasseCours
            modelBuilder.Entity<ClasseCours>()
                .HasRequired(cc => cc.Cours)
                .WithMany(c => c.ClasseCours)
                .HasForeignKey(cc => cc.CoursId);

            // Configuration de la clé composite pour CoursMatiere
            modelBuilder.Entity<CoursMatiere>()
                .HasKey(cm => new { cm.CoursId, cm.MatiereId });

            // Configuration de la relation entre Cours et CoursMatiere
            modelBuilder.Entity<CoursMatiere>()
                .HasRequired(cm => cm.Cour)
                .WithMany(c => c.CoursMatieres)
                .HasForeignKey(cm => cm.CoursId);

            // Configuration de la relation entre Matiere et CoursMatiere
            modelBuilder.Entity<CoursMatiere>()
                .HasRequired(cm => cm.Matiere)
                .WithMany(m => m.CoursMatieres)
                .HasForeignKey(cm => cm.MatiereId);

            // Configuration de la clé composite pour ProfesseurMatiere
            modelBuilder.Entity<ProfesseurMatiere>()
                .HasKey(pm => new { pm.ProfesseurId, pm.MatiereId });

            // Configuration de la relation entre Professeur et ProfesseurMatiere
            modelBuilder.Entity<ProfesseurMatiere>()
                .HasRequired(pm => pm.Professeur)
                .WithMany(p => p.ProfesseurMatieres)
                .HasForeignKey(pm => pm.ProfesseurId);

            // Configuration de la relation entre Matiere et ProfesseurMatiere
            modelBuilder.Entity<ProfesseurMatiere>()
                .HasRequired(pm => pm.Matiere)
                .WithMany(m => m.ProfesseurMatieres)
                .HasForeignKey(pm => pm.MatiereId);

            // Configuration de la clé composite pour ProfesseurClasse
            modelBuilder.Entity<ProfesseurClasse>()
                .HasKey(pc => new { pc.ProfesseurId, pc.ClasseId });

            // Configuration de la relation entre Professeur et ProfesseurClasse
            modelBuilder.Entity<ProfesseurClasse>()
                .HasRequired(pc => pc.Professeur)
                .WithMany(p => p.ProfesseurClasses)
                .HasForeignKey(pc => pc.ProfesseurId);

            // Configuration de la relation entre Classe et ProfesseurClasse
            modelBuilder.Entity<ProfesseurClasse>()
                .HasRequired(pc => pc.Classe)
                .WithMany(c => c.ProfesseurClasses)
                .HasForeignKey(pc => pc.ClasseId);
        }
    }

}
