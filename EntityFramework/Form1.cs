using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityFramework.Models;

namespace EntityFramework
{
    public partial class Form1 : Form
    {
        ITIContext db = new ITIContext();

        public Form1()
        {
            InitializeComponent();

            cmb_depts.DataSource = db.Departments.ToList();
            cmb_depts.DisplayMember = "Dept_Name";
            cmb_depts.ValueMember = "Dept_Id";

            cmb_supervisor.DataSource=db.Students.Select(n=> new { id=n.St_Id,name=n.St_Fname +" "+ n.St_Lname}).ToList();
            cmb_supervisor.DisplayMember = "name";
            cmb_supervisor.ValueMember = "id";
            dgv_students.DataSource = db.Students.Select(n => new { Id = n.St_Id, FirstName = n.St_Fname, LastName = n.St_Lname, Age = n.St_Age, Addrees = n.St_Address, department = n.Department.Dept_Name,Supervisor=n.St_super }).ToList();

        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            Student s = new Student()
            {
                St_Id = int.Parse(txt_id.Text),
                St_Fname = txt_fname.Text,
                St_Lname = txt_lname.Text,
                St_Age = int.Parse(txt_age.Text),
                St_Address = txt_address.Text,
                Dept_Id = (int)cmb_depts.SelectedValue,
                St_super = (int)cmb_supervisor.SelectedValue


            };

            db.Students.Add(s);
            db.SaveChanges();
            txt_id.Text = txt_fname.Text = txt_lname.Text = txt_age.Text = txt_address.Text = " ";
            cmb_depts.SelectedIndex =cmb_supervisor.SelectedIndex = 0;
            lbl_result.Text = "Added Successfully";
            dgv_students.DataSource = db.Students.Select(n => new { Id = n.St_Id, FirstName = n.St_Fname, LastName = n.St_Lname, Age = n.St_Age, Addrees = n.St_Address, department = n.Department.Dept_Name, Supervisor = n.St_super }).ToList();

        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
           int id=(int) dgv_students.SelectedRows[0].Cells[0].Value;
           Student s =  db.Students.Where(n => n.St_Id == id).SingleOrDefault();
            db.Students.Remove(s);
            db.SaveChanges();
            dgv_students.DataSource = db.Students.Select(n => new { Id = n.St_Id, FirstName = n.St_Fname, LastName = n.St_Lname, Age = n.St_Age, Addrees = n.St_Address, department = n.Department.Dept_Name, Supervisor = n.St_super }).ToList();

            txt_id.Text = txt_fname.Text = txt_lname.Text = txt_age.Text = txt_address.Text = " ";
            cmb_depts.SelectedIndex = cmb_supervisor.SelectedIndex = 0;
            lbl_result.Text = "Student Deleted";

        }

        private void dgv_students_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            int id = (int)dgv_students.SelectedRows[0].Cells[0].Value;
            Student s = db.Students.Where(n => n.St_Id == id).SingleOrDefault();
            txt_id.Text = s.St_Id.ToString();
            txt_fname.Text = s.St_Fname;
            txt_lname.Text = s.St_Lname;
            txt_age.Text = s.St_Age.ToString();
            txt_address.Text = s.St_Address;
            if(s.Department != null)
            {
                cmb_depts.Text = s.Department.Dept_Name;

            }
            else
            {
                cmb_depts.Text = "";
            }
            cmb_supervisor.Text = s.St_super.ToString();

            txt_id.Enabled = false;
            btn_add.Visible = false;
            btn_update.Visible = true;
            
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            int id =int.Parse( txt_id.Text);
            Student s=db.Students.Where(n=>n.St_Id==id).SingleOrDefault();
            s.St_Fname = txt_fname.Text;
            s.St_Lname = txt_lname.Text;
            s.St_Age = int.Parse(txt_age.Text);
            s.St_Address = txt_address.Text;
            s.Dept_Id = (int)cmb_depts.SelectedValue;
            s.St_super = (int)cmb_supervisor.SelectedValue;
            db.SaveChanges();
            dgv_students.DataSource = db.Students.Select(n => new { Id = n.St_Id, FirstName = n.St_Fname, LastName = n.St_Lname, Age = n.St_Age, Addrees = n.St_Address, department = n.Department.Dept_Name, Supervisor = n.St_super }).ToList();
            txt_id.Enabled = true;
            btn_add.Visible = true;
            btn_update.Visible = false;
            txt_id.Text = txt_fname.Text = txt_lname.Text = txt_age.Text = txt_address.Text = " ";
            cmb_depts.SelectedIndex = cmb_supervisor.SelectedIndex = 0;
            lbl_result.Text = "Student Data Updated";
        }
    }
}
