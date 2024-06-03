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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;


using MySql.Data.MySqlClient;

namespace WindowsFormsApp2
{

    //database connection
    public partial class Form1 : Form
    {
        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=");
        MySqlCommand command;
        MySqlDataReader mdr;

        public Form1()
        {
            InitializeComponent();
        }

        private void label_Click(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin_Click(sender, e, mdr);
        }

        //login in
        private void btnLogin_Click(object sender, EventArgs e, MySqlDataReader mdr)
        {
            // validation if user didnt enter the username or password
            // It checks if the txtUsername and txtPassword fields are empty and displays an  message 
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please input Username and Password", "Error");
            }

            else
            {
                //open databse connection
                connection.Open();

                // Construct an SQL query to select records from the "Login" table
                string selectQuery = "SELECT * FROM HotelDB.Login WHERE Username = '" + txtUsername.Text + "' AND Password = '" + txtPassword.Text + "';";

                // Create a MySqlCommand object to execute the query
                command = new MySqlCommand(selectQuery, connection);

                // Add parameters to prevent SQL injection
                command.Parameters.AddWithValue("@Username", txtUsername.Text);
                command.Parameters.AddWithValue("@Password", txtPassword.Text);
                
                // Execute the query and retrieve the results
                mdr = command.ExecuteReader();


                if (mdr.Read())
                {
                    // Update the "LastLogin" field in the database
                    string MyConnection2 = "datasource=localhost;port=3306;username=root;password=";
                    string Query = "update HotelDB.Login set LastLogin='" + dateTimePicker1.Value + "' where Username='" + this.txtUsername.Text + "';";

                    // Create a MySqlCommand object to execute the update query
                    MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);

                    //login validation
                    MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                    MySqlDataReader MyReader2;
                    MyConn2.Open();
                    MyReader2 = MyCommand2.ExecuteReader();
                    while (MyReader2.Read())
                    {
                    }
                    MyConn2.Close();

                    MessageBox.Show("Login Successful!");


                    // Check if the login is for "admin" and the password is "admin123"
                    if (txtUsername.Text == "admin" && txtPassword.Text == "admin123")
                        {
                            this.Hide();
                            AdminLoginSucess frmAdmin = new AdminLoginSucess();
                            frmAdmin.ShowDialog();
                        }
                        else
                        {
                            this.Hide();
                            LoginSuccessForm frmUser = new LoginSuccessForm();
                            frmUser.ShowDialog();
                        }
                }
                else
                {

                    MessageBox.Show("Incorrect Login Information! Try again.");
                }
                // Close the database connection
                connection.Close();
            }

        }
        //create an account
        private void btnCreate_Click(object sender, EventArgs e)
        {
            this.Hide();
            CreateAccountForm frm3 = new CreateAccountForm();
            frm3.ShowDialog();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        //exit 
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
