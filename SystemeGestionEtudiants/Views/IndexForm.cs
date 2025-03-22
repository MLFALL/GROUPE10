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
    public partial class IndexForm: Form
    {
        public IndexForm()
        {
            InitializeComponent();
            CustomizeDesign();
        }
        private void CustomizeDesign()
        {
            panelEtudiantSubMenu.Visible = false;
            panelSubClasseMenu.Visible = false;
            panelSubCoMaMenu.Visible = false;
            panelSubProfsMenu.Visible = false;
            panelSubRapportsMenu.Visible = false;
            panelSubUsersMenu.Visible = false;
        }
        private void HideSubMenu()
        {
            if(panelEtudiantSubMenu.Visible = true)
                panelEtudiantSubMenu.Visible = false;
            if(panelSubClasseMenu.Visible = true)
                panelSubClasseMenu.Visible = false;
            if(panelSubCoMaMenu.Visible = true)
                panelSubCoMaMenu.Visible = false;
            if(panelSubProfsMenu.Visible = true)
                panelSubProfsMenu.Visible = false;
            if(panelSubRapportsMenu.Visible = true)
                panelSubRapportsMenu.Visible = false;
            if(panelSubUsersMenu.Visible = true)
                panelSubUsersMenu.Visible = false;
        }
        private void ShowSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                HideSubMenu();
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
        }
        private Form activeForm = null;
        private void OpenChildForm(Form ChildForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = ChildForm;
            ChildForm.TopLevel = false;
            ChildForm.FormBorderStyle = FormBorderStyle.None;
            ChildForm.Dock = DockStyle.Fill;
            panelChildForm.Controls.Add(ChildForm);
            panelChildForm.Tag = ChildForm;
            ChildForm.BringToFront();
            ChildForm.Show();
        }

        //Debut Etudiant

        private void btnEtudiant_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panelEtudiantSubMenu);
        }

        private void btnSubListEtudiant_Click(object sender, EventArgs e)
        {
            OpenChildForm(new EtudiantsParClasseForm());
            HideSubMenu();
        }

        private void btnSubCRUDEtud_Click(object sender, EventArgs e)
        {
            OpenChildForm(new EtudiantsForm());
            HideSubMenu();
        }

        private void btnSubDetailsEtudiants_Click(object sender, EventArgs e)
        {
            HideSubMenu();
        }
        //Fin Etudiants

        //Debut Professeurs
        private void btnPlayist_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panelSubProfsMenu);
        }

        private void btnSubListProfs_Click(object sender, EventArgs e)
        {
            HideSubMenu();
        }

        private void btnSubDetailsProf_Click(object sender, EventArgs e)
        {
            HideSubMenu();
        }
        //Fin Profs

        //denut Users
        private void btnUsers_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panelSubUsersMenu);
        }

        private void btnSubListUsers_Click(object sender, EventArgs e)
        {
            HideSubMenu();
        }

        private void btnSubCRUDUser_Click(object sender, EventArgs e)
        {
            OpenChildForm(new UserForm());
            HideSubMenu();
        }
        //Fin User


        //Debut Classe
        private void btnClasse_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panelSubClasseMenu);
        }

        private void btnsubClasse_Click(object sender, EventArgs e)
        {
            HideSubMenu();
        }

        private void btbSubCRUDClasse_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ClassForm());
            HideSubMenu();
        }
        //Fin Classe

        //Debut CO+MA
        private void btnAssos_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panelSubCoMaMenu);
        }

        private void btnSubList_Click(object sender, EventArgs e)
        {
            HideSubMenu();
        }

        private void btnSubCRUD_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CoursForm());
            HideSubMenu();
        }
        //Fin CO+MA

        //Debut Rapports
        private void btnRapports_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panelSubRapportsMenu);
        }

        private void btnSubBulletin_Click(object sender, EventArgs e)
        {
            HideSubMenu();
        }

        private void btnSubListClasses_Click(object sender, EventArgs e)
        {
            HideSubMenu();
        }
        //Fin Rapports
    }
}
