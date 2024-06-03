using MySql.Data.MySqlClient;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp2
{
    public partial class MPackages : Form
    {
        // connecting to database
        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=");
        public MPackages()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //logout
            this.Hide();
            Form1 frm1 = new Form1();
            frm1.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Create an instance of the AdminLoginSuccessForm (going back to the admin Main Menu)
            AdminLoginSucess adminLoginForm = new AdminLoginSucess();

            // Show the AdminLoginSuccessForm
            adminLoginForm.Show();

            // Close the current form
            this.Close();
        }


        //update button
        private void button1_Click(object sender, EventArgs e)
        {
            //update the details
            if (!ValidateFormData())
            {
                MessageBox.Show("Please fill out all information!", "Error");
                return;
            }

            using (MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password="))
            {
                try
                {
                    //open database conection
                    connection.Open();

                    // Define the SQL update query
                    string updateQuery = "UPDATE HotelDB.packages SET `Name` = @Name, `CostPerPerson` = @CostPerPerson, `Description` = @Description, `Complementary`" +
                        " = @Complementary WHERE `Id` = @Id";

                    // Create a MySqlCommand object to execute the update query
                    MySqlCommand cmdUpdate = new MySqlCommand(updateQuery, connection);

                    // Add parameters to the query to prevent SQL injection
                    cmdUpdate.Parameters.AddWithValue("@Id", txtId.Text);
                    cmdUpdate.Parameters.AddWithValue("@Name", txtName.Text);
                    cmdUpdate.Parameters.AddWithValue("@CostPerPerson", txtCostPerPerson.Text);
                    cmdUpdate.Parameters.AddWithValue("@Description", txtDescription.Text);
                    cmdUpdate.Parameters.AddWithValue("@Complementary", txtComplementary.Text);

                    // Execute the update query and get the number of rows affected
                    int rowsAffected = cmdUpdate.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record updated successfully!");
                        PopulateDataGridView(); // Refresh the DataGridView with updated data
                    }
                    else
                    {
                        MessageBox.Show("No records were updated.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private bool ValidateFormData()
        {
            if(string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Please Enter A Valid Id", "Invalid Id", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return !string.IsNullOrEmpty(txtId.Text) &&
                   !string.IsNullOrEmpty(txtName.Text) &&
                   !string.IsNullOrEmpty(txtCostPerPerson.Text) &&
                   !string.IsNullOrEmpty(txtDescription.Text) &&
                   !string.IsNullOrEmpty(txtComplementary.Text);           
        }

        private void MPackages_Load(object sender, EventArgs e)
        {
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;

            PopulateDataGridView();
        }
        private void PopulateDataGridView()
        {
            // Add a method to populate the DataGridView with data
            try
            {
                connection.Open();

                // Define an SQL query to retrieve data from the database
                string query = "SELECT * FROM HotelDB.packages";

                // Create a MySqlCommand object to execute the query
                MySqlCommand command = new MySqlCommand(query, connection);

                // Create a MySqlDataAdapter to retrieve data into a DataTable
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);

                // Create a DataTable to store the retrieved data
                DataTable dataTable = new DataTable();

                // Fill the DataTable with data from the database using the adapter
                adapter.Fill(dataTable);

                // Bind the DataGridView to the DataTable, displaying the retrieved data
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        //add record to the database
        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            //validate data to database
            if (string.IsNullOrEmpty(txtId.Text) || string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtCostPerPerson.Text) || 
                string.IsNullOrEmpty(txtDescription.Text) || string.IsNullOrEmpty(txtComplementary.Text) )
            {
                MessageBox.Show("Please fill out all information!", "Error");
                return;
            }
            using (MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password="))
            {
                try
                {
                    connection.Open();

                    // Create MySqlCommand objects to check if the name and ID already exist in the database
                    MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM HotelDB.packages WHERE Name = @Name", connection);
                    MySqlCommand cmd2 = new MySqlCommand("SELECT * FROM HotelDB.packages WHERE Id = @Id", connection);

                    // Add parameters to the commands to prevent SQL injection
                    cmd1.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd2.Parameters.AddWithValue("@Id", txtId.Text);

                    // Variables to track if the name and ID already exist
                    bool userExists = false, mailExists = false;

                    using (var dr1 = cmd1.ExecuteReader())
                        if (userExists = dr1.HasRows) MessageBox.Show("Name already exists!");

                    using (var dr2 = cmd2.ExecuteReader())
                        if (mailExists = dr2.HasRows) MessageBox.Show("Id already exists!");


                    // If neither the name nor ID exists, insert a new record
                    if (!(userExists || mailExists))
                    {
                       
                        string iquery = "INSERT INTO HotelDB.packages(`Id`,`Name`,`CostPerPerson`,`Description`,`Complementary`) VALUES (@Id, @Name, " +
                            "@CostPerPerson, " +
                            "@Description, @Complementary)";

                        // Add parameters to the insert query
                        MySqlCommand commandDatabase = new MySqlCommand(iquery, connection);
                        commandDatabase.Parameters.AddWithValue("@Id", txtId.Text);
                        commandDatabase.Parameters.AddWithValue("@Name", txtName.Text);
                        commandDatabase.Parameters.AddWithValue("@CostPerPerson", txtCostPerPerson.Text);
                        commandDatabase.Parameters.AddWithValue("@Description", txtDescription.Text);
                        commandDatabase.Parameters.AddWithValue("@Complementary", txtComplementary.Text);

                        // Execute the insert query to add the new record
                        commandDatabase.ExecuteNonQuery();

                        MessageBox.Show("Record Inserted Successfully!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBackToLogin_Click(object sender, EventArgs e)
        {
            // clear the form
            txtId.Clear();
            txtName.Clear();
            txtCostPerPerson.Clear();
            txtDescription.Clear();
            txtComplementary.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //delete the data which is inserted
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Please select a record to delete.", "Error");
                return;
            }
            if (MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == 
                DialogResult.Yes)
            {
                using (MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password="))
                {
                    try
                    {
                        connection.Open();

                        string deleteQuery = "DELETE FROM HotelDB.packages WHERE Id = @Id";

                        MySqlCommand cmdDelete = new MySqlCommand(deleteQuery, connection);
                        cmdDelete.Parameters.AddWithValue("@Id", txtId.Text);

                        int rowsAffected = cmdDelete.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record deleted successfully!");
                            PopulateDataGridView(); // Refresh the DataGridView after deletion
                            ClearFormFields(); // Clear the form fields
                        }
                        else
                        {
                            MessageBox.Show("No records were deleted.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void ClearFormFields()
        {
            txtId.Clear();
            txtName.Clear();
            txtCostPerPerson.Clear();
            txtDescription.Clear();
            txtComplementary.Clear();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
        private void txtFName_TextChanged(object sender, EventArgs e)
        {
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void txtLName_TextChanged(object sender, EventArgs e)
        {

        }
        private void label6_Click(object sender, EventArgs e)
        {

        }
        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtId.Text = row.Cells[0].Value.ToString();
                txtName.Text = row.Cells[1].Value.ToString();
                txtCostPerPerson.Text = row.Cells[2].Value.ToString();
                txtDescription.Text = row.Cells[3].Value.ToString();
                txtComplementary.Text = row.Cells[4].Value.ToString();
            }
        }
    }
}
