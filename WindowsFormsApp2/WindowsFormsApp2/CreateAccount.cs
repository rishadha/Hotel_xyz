using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp2
{
    public partial class CreateAccountForm : Form
    {
        //databse connection
        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=");

        public CreateAccountForm()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void CreateAccountForm_Load(object sender, EventArgs e)
        {
            cboGender.Items.Add("Female");
            cboGender.Items.Add("Male");
        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            //validation
            if (!this.txtEmail.Text.Contains('@') || !this.txtEmail.Text.Contains('.'))
            {
                MessageBox.Show("Please Enter A Valid Email", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtPassword.Text != txtCPassword.Text)
            {
                MessageBox.Show("Password doesn't match!", "Error");
                return;
            }

            if (string.IsNullOrEmpty(txtFName.Text) || string.IsNullOrEmpty(txtLName.Text) || string.IsNullOrEmpty(cboGender.Text) || string.IsNullOrEmpty(txtEmail.Text) 
                || string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(txtCPassword.Text))
            {
                MessageBox.Show("Please fill out all information!", "Error");
                return;
            }

            else
            {
                // open database connection
                connection.Open();

                // Check if the username already exists in the database
                MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM HotelDB.Login WHERE Username = @UserName", connection),

                // Check if the email already exists in the database
                cmd2 = new MySqlCommand("SELECT * FROM HotelDB.Login WHERE Email = @UserEmail", connection);

                // Add parameters to prevent SQL injection
                cmd1.Parameters.AddWithValue("@UserName", txtUsername.Text);
                cmd2.Parameters.AddWithValue("@UserEmail", txtEmail.Text);

                bool userExists = false, mailExists = false;

                // Check if the username exists
                using (var dr1 = cmd1.ExecuteReader())
                    if (userExists = dr1.HasRows) MessageBox.Show("Username is already available! try another one");

                // Check if the email exists
                using (var dr2 = cmd2.ExecuteReader())
                    if (mailExists = dr2.HasRows) MessageBox.Show("Email is available! try another one");

                // If neither the username nor email exists, insert a new record
                if (!(userExists || mailExists))
                {

                    string iquery = "INSERT INTO HotelDB.Login(`ID`,`FirstName`,`LastName`,`Gender`,`Birthday`,`Email`,`Username`, `Password`)" +
                        " VALUES (NULL, '" + txtFName.Text + "', '" + txtLName.Text + "', '" + cboGender.Text + "', '" + dateTimePicker1.Value.Date + 
                        "', '" + txtEmail.Text + "', '" + txtUsername.Text + "', '" + txtPassword.Text + "')";
                    MySqlCommand commandDatabase = new MySqlCommand(iquery, connection);
                    commandDatabase.CommandTimeout = 60;

                    try
                    {
                        MySqlDataReader myReader = commandDatabase.ExecuteReader();
                    }
                    catch (Exception ex)
                    {
                        // Show any error message.
                        MessageBox.Show(ex.Message);
                    }

                    MessageBox.Show("Account Successfully Created!");

                }
                // Close the database connection
                connection.Close();
            }

        }
        // back to login
        private void btnBackToLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 frm4 = new Form1();
            frm4.ShowDialog();

        }
    }
}
