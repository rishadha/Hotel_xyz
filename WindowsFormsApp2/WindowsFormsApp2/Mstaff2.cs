using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace WindowsFormsApp2
{
    public partial class Mstaff2 : Form
    {
        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=");
        public Mstaff2()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected department
            string selectedDepartment = comboBox1.SelectedItem != null ? comboBox1.SelectedItem.ToString() : "";

            // Display the corresponding salary based on the selected department textBox3
            switch (selectedDepartment)
            {
                case "Maintainence":
                    txtSalary.Text = "Salary: Rs.45000";
                    break;
                case "Kitchen":
                    txtSalary.Text = "Salary: Rs.50000";
                    break;
                case "Housekeeping":
                    txtSalary.Text = "Salary: Rs.35000";
                    break;
                case "Banquets":
                    txtSalary.Text = "Salary: Rs.60000";
                    break;
            }
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            //logout from the system going back to the login page
            this.Hide();
            Form1 frm1 = new Form1();
            frm1.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Create an instance of the AdminLoginSuccessForm (going back to the admin login success form)
            AdminLoginSucess adminLoginForm = new AdminLoginSucess();

            // Show the AdminLoginSuccessForm
            adminLoginForm.Show();

            // Close the current form
            this.Close();
        }

        private void Mstaff2_Load(object sender, EventArgs e)
        {
            PopulateDataGridView();
        }

        private void PopulateDataGridView()
        {
            // Add a method to populate the DataGridView with data
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM HotelDB.mstaff";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
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

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            //inserting data to the databse through add button
            if (!this.txtEmail.Text.Contains('@') || !this.txtEmail.Text.Contains('.'))
            {
                MessageBox.Show("Please Enter A Valid Email", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtId.Text) || string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(cboGender.Text) || 
                comboBox1.SelectedItem == null || string.IsNullOrEmpty(txtAddress.Text) || string.IsNullOrEmpty(txtContact.Text) ||
                string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtSalary.Text))
            {
                MessageBox.Show("Please fill out all information!", "Error");
                return;
            }

            using (MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password="))
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM HotelDB.mstaff WHERE Name = @Name", connection);
                    MySqlCommand cmd2 = new MySqlCommand("SELECT * FROM HotelDB.mstaff WHERE Email = @Email", connection);

                    cmd1.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd2.Parameters.AddWithValue("@Email", txtEmail.Text);

                    bool userExists = false, mailExists = false;

                    using (var dr1 = cmd1.ExecuteReader())
                        if (userExists = dr1.HasRows) MessageBox.Show("Name already exists!");

                    using (var dr2 = cmd2.ExecuteReader())
                        if (mailExists = dr2.HasRows) MessageBox.Show("Email already exists!");

                    if (!(userExists || mailExists))
                    {
                        string selectedDepartment = comboBox1.SelectedItem.ToString();

                        string iquery = "INSERT INTO HotelDB.mstaff(`Id`,`Name`,`Gender`,`Department`,`Address`, `ContactNo`,`Email`,`Salary`) " +
                            "VALUES (NULL, @Name, @Gender, @Department, @Address, @ContactNo, @Email, @Salary)";
                        MySqlCommand commandDatabase = new MySqlCommand(iquery, connection);

                        commandDatabase.Parameters.AddWithValue("@Name", txtName.Text);
                        commandDatabase.Parameters.AddWithValue("@Gender", cboGender.Text);
                        commandDatabase.Parameters.AddWithValue("@Department", selectedDepartment);
                        commandDatabase.Parameters.AddWithValue("@Address", txtAddress.Text);
                        commandDatabase.Parameters.AddWithValue("@ContactNo", txtContact.Text);
                        commandDatabase.Parameters.AddWithValue("@Email", txtEmail.Text);
                        commandDatabase.Parameters.AddWithValue("@Salary", txtSalary.Text);

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

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Clear textboxes(form)
            txtId.Clear();
            txtName.Clear();
            txtAddress.Clear();
            txtContact.Clear();
            txtEmail.Clear();
            txtSalary.Clear();

            // Reset comboboxes
            cboGender.SelectedIndex = -1; // Clear the selected gender
            comboBox1.SelectedIndex = -1; // Clear the selected department
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtId.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                cboGender.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtAddress.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                txtContact.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                txtEmail.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                txtSalary.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
            }

         }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //update the details already exists
            if (!ValidateFormData())
            {
                MessageBox.Show("Please fill out all information!", "Error");
                return;
            }

            using (MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password="))
            {
                try
                {
                    connection.Open();

                    string selectedDepartment = comboBox1.SelectedItem.ToString();
                    string updateQuery = "UPDATE HotelDB.mstaff SET Name = @Name, Gender = @Gender, Department = @Department, " +
                        "Address = @Address, ContactNo = @ContactNo, Email = @Email, Salary = @Salary WHERE Id = @Id";

                    MySqlCommand cmdUpdate = new MySqlCommand(updateQuery, connection);
                    cmdUpdate.Parameters.AddWithValue("@Id", txtId.Text);
                    cmdUpdate.Parameters.AddWithValue("@Name", txtName.Text);
                    cmdUpdate.Parameters.AddWithValue("@Gender", cboGender.Text);
                    cmdUpdate.Parameters.AddWithValue("@Department", selectedDepartment);
                    cmdUpdate.Parameters.AddWithValue("@Address", txtAddress.Text);
                    cmdUpdate.Parameters.AddWithValue("@ContactNo", txtContact.Text);
                    cmdUpdate.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmdUpdate.Parameters.AddWithValue("@Salary", txtSalary.Text);

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
            if (!txtEmail.Text.Contains('@') || !txtEmail.Text.Contains('.'))
            {
                MessageBox.Show("Please Enter A Valid Email", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return !string.IsNullOrEmpty(txtId.Text) &&
                   !string.IsNullOrEmpty(txtName.Text) &&
                   !string.IsNullOrEmpty(cboGender.Text) &&
                   comboBox1.SelectedItem != null &&
                   !string.IsNullOrEmpty(txtAddress.Text) &&
                   !string.IsNullOrEmpty(txtContact.Text) &&
                   !string.IsNullOrEmpty(txtEmail.Text) &&
                   !string.IsNullOrEmpty(txtSalary.Text);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // delete the record
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Please select a record to delete.", "Error");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) 
                == DialogResult.Yes)
            {
                using (MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password="))
                {
                    try
                    {
                        connection.Open();

                        string deleteQuery = "DELETE FROM HotelDB.mstaff WHERE Id = @Id";

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
            cboGender.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
            txtAddress.Clear();
            txtContact.Clear();
            txtEmail.Clear();
            txtSalary.Clear();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //search the details through the name
            string searchName = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(searchName))
            {
                MessageBox.Show("Please enter a name to search.", "Error");
                PopulateDataGridView();
                return;
                
            }

            using (MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password="))
            {
                try
                {
                    connection.Open();

                    string searchQuery = "SELECT * FROM HotelDB.mstaff WHERE Name LIKE @Name";

                    MySqlCommand cmdSearch = new MySqlCommand(searchQuery, connection);
                    cmdSearch.Parameters.AddWithValue("@Name", "%" + searchName + "%");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmdSearch);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dataTable;
                    }
                    else
                    {
                        MessageBox.Show("No matching records found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView1.DataSource = null; // Clear the DataGridView
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
 }
