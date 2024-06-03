using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class AdminLoginSucess : Form
    {
        public AdminLoginSucess()
        {
            InitializeComponent();
        }

        //logout
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 frm1 = new Form1();
            frm1.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Create an instance of the Mstaff form
            Mstaff2 mstaffForm = new Mstaff2();

            // Show the Mstaff form
            mstaffForm.Show();

            // Optionally, you can hide the current form if needed
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            // Create an instance of the MPackages form
            MPackages mPackagesForm = new MPackages();

            // Show the MPackages form
            mPackagesForm.Show();

            // Optionally, you can hide the current form if you want to close it when navigating to the new form
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void AdminLoginSucess_Load(object sender, EventArgs e)
        {

        }
    }
}
