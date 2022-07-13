using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EF_DataBaseFirst
{
    public partial class Form : System.Windows.Forms.Form
    {
        Customer model = new Customer();
        public Form()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            LoadData();
            this.ActiveControl = txtFirstName;
        }

        private void clear()
        {
            txtFirstName.Text = txtLastName.Text = txtCity.Text = txtAddress.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            model.CustomerID = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void LoadData() 
        {
            using (EFDB_FirstEntities db = new EFDB_FirstEntities())
            {
                dgvCustomer.DataSource = db.Customers.ToList<Customer>();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            model.FirstName = txtFirstName.Text.Trim();
            model.LastName = txtFirstName.Text.Trim();
            model.City = txtCity.Text.Trim();
            model.Address = txtAddress.Text.Trim();


            using (EFDB_FirstEntities db = new EFDB_FirstEntities())
            {
                if (model.CustomerID == 0)//insert
                    db.Customers.Add(model);
                else
                    db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }
            clear();
            MessageBox.Show("Subbmitted Successfully");
            LoadData();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure To Delete This Record", "Message", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (EFDB_FirstEntities db = new EFDB_FirstEntities())
                {
                    var entry = db.Entry(model);
                    if (entry.State == EntityState.Detached)
                    {
                        db.Customers.Attach(model);
                        db.Customers.Remove(model);
                        db.SaveChanges();
                        clear();
                        MessageBox.Show("Deleted Successfully");
                    }
                }
            }
        }

        private void dgvCustomer_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.CurrentRow.Index != -1)
            {
                model.CustomerID = Convert.ToInt32(dgvCustomer.CurrentRow.Cells["CustomerID"].Value);
                using (EFDB_FirstEntities db = new EFDB_FirstEntities())
                {
                    model = db.Customers.Where(x => x.CustomerID == model.CustomerID).FirstOrDefault();
                    txtAddress.Text = model.Address;
                    txtCity.Text = model.City;
                    txtFirstName.Text = model.FirstName;
                    txtLastName.Text = model.LastName;
                  
                }

                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }
    }
}
