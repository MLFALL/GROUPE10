namespace SystemeGestionEtudiants.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Lamine1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClasseCours",
                c => new
                    {
                        ClasseId = c.Int(nullable: false),
                        CoursId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ClasseId, t.CoursId })
                .ForeignKey("dbo.Classes", t => t.ClasseId, cascadeDelete: true)
                .ForeignKey("dbo.Cours", t => t.CoursId, cascadeDelete: true)
                .Index(t => t.ClasseId)
                .Index(t => t.CoursId);
            
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NomClasse = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Etudiants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Matricule = c.String(),
                        Nom = c.String(),
                        Prenom = c.String(),
                        DateNaissance = c.DateTime(nullable: false),
                        Sexe = c.String(),
                        Adresse = c.String(),
                        Telephone = c.String(),
                        Email = c.String(),
                        ClasseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Classes", t => t.ClasseId, cascadeDelete: true)
                .Index(t => t.ClasseId);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        MatiereId = c.Int(nullable: false),
                        NoteValue = c.Single(nullable: false),
                        Etudiants_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Etudiants", t => t.Etudiants_Id)
                .ForeignKey("dbo.Matieres", t => t.MatiereId, cascadeDelete: true)
                .Index(t => t.MatiereId)
                .Index(t => t.Etudiants_Id);
            
            CreateTable(
                "dbo.Matieres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NomMatiere = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CoursMatieres",
                c => new
                    {
                        CoursId = c.Int(nullable: false),
                        MatiereId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CoursId, t.MatiereId })
                .ForeignKey("dbo.Cours", t => t.CoursId, cascadeDelete: true)
                .ForeignKey("dbo.Matieres", t => t.MatiereId, cascadeDelete: true)
                .Index(t => t.CoursId)
                .Index(t => t.MatiereId);
            
            CreateTable(
                "dbo.Cours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NomCours = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProfesseurMatieres",
                c => new
                    {
                        ProfesseurId = c.Int(nullable: false),
                        MatiereId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProfesseurId, t.MatiereId })
                .ForeignKey("dbo.Matieres", t => t.MatiereId, cascadeDelete: true)
                .ForeignKey("dbo.Professeurs", t => t.ProfesseurId, cascadeDelete: true)
                .Index(t => t.ProfesseurId)
                .Index(t => t.MatiereId);
            
            CreateTable(
                "dbo.Professeurs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(),
                        Prenom = c.String(),
                        Email = c.String(),
                        Telephone = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProfesseurClasses",
                c => new
                    {
                        ProfesseurId = c.Int(nullable: false),
                        ClasseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProfesseurId, t.ClasseId })
                .ForeignKey("dbo.Classes", t => t.ClasseId, cascadeDelete: true)
                .ForeignKey("dbo.Professeurs", t => t.ProfesseurId, cascadeDelete: true)
                .Index(t => t.ProfesseurId)
                .Index(t => t.ClasseId);
            
            CreateTable(
                "dbo.OTPCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UtilisateurId = c.Int(nullable: false),
                        Code = c.String(),
                        DateExpiration = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Utilisateurs", t => t.UtilisateurId, cascadeDelete: true)
                .Index(t => t.UtilisateurId);
            
            CreateTable(
                "dbo.Utilisateurs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NomUtilisateur = c.String(),
                        MotDePasse = c.String(),
                        Role = c.String(),
                        Telephone = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OTPCodes", "UtilisateurId", "dbo.Utilisateurs");
            DropForeignKey("dbo.Notes", "MatiereId", "dbo.Matieres");
            DropForeignKey("dbo.ProfesseurMatieres", "ProfesseurId", "dbo.Professeurs");
            DropForeignKey("dbo.ProfesseurClasses", "ProfesseurId", "dbo.Professeurs");
            DropForeignKey("dbo.ProfesseurClasses", "ClasseId", "dbo.Classes");
            DropForeignKey("dbo.ProfesseurMatieres", "MatiereId", "dbo.Matieres");
            DropForeignKey("dbo.CoursMatieres", "MatiereId", "dbo.Matieres");
            DropForeignKey("dbo.CoursMatieres", "CoursId", "dbo.Cours");
            DropForeignKey("dbo.ClasseCours", "CoursId", "dbo.Cours");
            DropForeignKey("dbo.Notes", "Etudiants_Id", "dbo.Etudiants");
            DropForeignKey("dbo.Etudiants", "ClasseId", "dbo.Classes");
            DropForeignKey("dbo.ClasseCours", "ClasseId", "dbo.Classes");
            DropIndex("dbo.OTPCodes", new[] { "UtilisateurId" });
            DropIndex("dbo.ProfesseurClasses", new[] { "ClasseId" });
            DropIndex("dbo.ProfesseurClasses", new[] { "ProfesseurId" });
            DropIndex("dbo.ProfesseurMatieres", new[] { "MatiereId" });
            DropIndex("dbo.ProfesseurMatieres", new[] { "ProfesseurId" });
            DropIndex("dbo.CoursMatieres", new[] { "MatiereId" });
            DropIndex("dbo.CoursMatieres", new[] { "CoursId" });
            DropIndex("dbo.Notes", new[] { "Etudiants_Id" });
            DropIndex("dbo.Notes", new[] { "MatiereId" });
            DropIndex("dbo.Etudiants", new[] { "ClasseId" });
            DropIndex("dbo.ClasseCours", new[] { "CoursId" });
            DropIndex("dbo.ClasseCours", new[] { "ClasseId" });
            DropTable("dbo.Utilisateurs");
            DropTable("dbo.OTPCodes");
            DropTable("dbo.ProfesseurClasses");
            DropTable("dbo.Professeurs");
            DropTable("dbo.ProfesseurMatieres");
            DropTable("dbo.Cours");
            DropTable("dbo.CoursMatieres");
            DropTable("dbo.Matieres");
            DropTable("dbo.Notes");
            DropTable("dbo.Etudiants");
            DropTable("dbo.Classes");
            DropTable("dbo.ClasseCours");
        }
    }
}
