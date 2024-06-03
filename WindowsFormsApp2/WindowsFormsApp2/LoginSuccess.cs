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
    public partial class LoginSuccessForm : Form
    {
        public LoginSuccessForm()
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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btmReservations_Click(object sender, EventArgs e)
        {
            // Create an instance of the MReservation form
            MReservation mReservationForm = new MReservation();

            // Show the MReservation form
            mReservationForm.Show();

            // Optionally, hide the current form
            this.Hide();
        }

        private void btnViewPackages_Click(object sender, EventArgs e)
        {
            Viewpackages viewPackagesForm = new Viewpackages();

            // Show the ViewPackages form
            viewPackagesForm.Show();

            // Optionally, you can hide the current form if you want to close it when navigating to the new form
            this.Hide();
        }

        private void LoginSuccessForm_Load(object sender, EventArgs e)
        {

        }
    }
}
