using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;




namespace WindowsFormsApp2
{
    public partial class MReservation : Form
    {
        private bool ValidateFormData()
        {
            if (!IsNumeric(txtMobileNo.Text))
            {
                MessageBox.Show("Please Enter a Valid Numeric Mobile Number", "Invalid Mobile Number", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return !string.IsNullOrEmpty(txtId.Text) &&
                   !string.IsNullOrEmpty(txtFirstName.Text) &&
                   !string.IsNullOrEmpty(txtAddress.Text) &&
                   !string.IsNullOrEmpty(txtReservatioDate.Text) &&
                   !string.IsNullOrEmpty(txtNoOfPax.Text) &&
                   !string.IsNullOrEmpty(txtCost.Text);
        }

        private bool IsNumeric(string input)
        {
            return int.TryParse(input, out _);
        }

        private MySqlConnection connection; // Declare the MySqlConnection variable at the class level
        public MReservation()
        {
            InitializeComponent();
            // Initialize the MySqlConnection in the constructor
            connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=");

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            LoginSuccessForm login = new LoginSuccessForm();
            login.Show();

            
            this.Close();


            // Create an instance of the LoginSucess form
            // LoginSuccess loginSuccessForm = new LoginSuccess();

            // Show the LoginSucess form
            //loginSuccessForm.Show();

            // Close the current form
        }

        private void txtNoOfPax_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtNoOfPax.Text, out int pax))
            {
                int cost = 0;

                if (pax <= 5)
                {
                    cost = 3000 * pax;
                }
                else if (pax <= 20)
                {
                    cost = 2500 * pax;
                }
                else if (pax <= 50)
                {
                    cost = 1500 * pax;
                }
                else
                {
                    cost = 1000 * pax;
                }

                txtCost.Text = cost.ToString();
            }
            else
            {
                // Handle the case where the input is not a valid integer
                // You can display a message to the user or take appropriate action.
                txtCost.Text = ""; // Clear the cost TextBox or display an error message.
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 frm1 = new Form1();
            frm1.ShowDialog();
        }

        private void btnBackToLogin_Click(object sender, EventArgs e)
        {
            // Clear textboxes
            txtId.Clear();
            txtFirstName.Clear();
            txtAddress.Clear();
            txtMobileNo.Clear();
            txtReservatioDate.Clear(); // Clear the selected date
            txtNoOfPax.Clear();
            txtCost.Clear();


        }
        // add record to database
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text) || string.IsNullOrEmpty(txtFirstName.Text) || string.IsNullOrEmpty(txtAddress.Text) || 
                string.IsNullOrEmpty(txtMobileNo.Text) || string.IsNullOrEmpty(txtReservatioDate.Text) || string.IsNullOrEmpty(txtNoOfPax.Text))
            {
                MessageBox.Show("Please fill out all required information!", "Error");
                return;
            }

            using (MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password="))
            {
                try
                {
                    connection.Open();

                    // Check if the provided ID already exists in the database
                    MySqlCommand idCheckCommand = new MySqlCommand("SELECT COUNT(*) FROM HotelDB.MReservations WHERE Id = @Id", connection);
                    idCheckCommand.Parameters.AddWithValue("@Id", txtId.Text);

                    int idCount = Convert.ToInt32(idCheckCommand.ExecuteScalar());

                    if (idCount > 0)
                    {
                        MessageBox.Show("ID already exists in the database!", "Error");
                        return; // Exit the method if the ID is not unique
                    }


                    MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM HotelDB.MReservations WHERE FirstName = @FirstName", connection);
                    MySqlCommand cmd2 = new MySqlCommand("SELECT * FROM HotelDB.MReservations WHERE MobileNo = @MobileNo", connection);

                    cmd1.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                    cmd2.Parameters.AddWithValue("@MobileNo", txtMobileNo.Text);

                    bool userExists = false, mailExists = false;

                    using (var dr1 = cmd1.ExecuteReader())
                        if (userExists = dr1.HasRows) MessageBox.Show("FirstName already exists!");

                    using (var dr2 = cmd2.ExecuteReader())
                        if (mailExists = dr2.HasRows) MessageBox.Show("MobileNo already exists!");

                    if (!(userExists || mailExists))
                    {
                        string iquery = "INSERT INTO HotelDB.MReservations(`Id`,`FirstName`,`Address`,`MobileNo`, `ReservationDate`,`NoOfPax`,`TotalCost`) VALUES " +
                            "(@Id, @FirstName, @Address, @MobileNo, @ReservationDate, @NoOfPax, @TotalCost)";
                        MySqlCommand commandDatabase = new MySqlCommand(iquery, connection);

                        commandDatabase.Parameters.AddWithValue("@Id", txtId.Text);
                        commandDatabase.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                        commandDatabase.Parameters.AddWithValue("@Address", txtAddress.Text);
                        commandDatabase.Parameters.AddWithValue("@MobileNo", txtMobileNo.Text);
                        commandDatabase.Parameters.AddWithValue("@ReservationDate", txtReservatioDate.Text);
                        commandDatabase.Parameters.AddWithValue("@NoOfPax", txtNoOfPax.Text);
                        commandDatabase.Parameters.AddWithValue("@TotalCost", txtCost.Text);

                        commandDatabase.ExecuteNonQuery();

                        MessageBox.Show("Record Inserted Successfully!");
                        
                        PopulateDataGridView();
                    }
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
        }

        private void MReservation_Load(object sender, EventArgs e)
        {

            // Get the next available ID from the database and set it in the txtId TextBox
            int nextId = GetNextIdFromDatabase();
            string formattedId = "C" + nextId.ToString("D3");
            txtId.Text = formattedId;
            PopulateDataGridView();

            
        }


        private int GetNextIdFromDatabase()
        {
            int nextId = 0;

            using (MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password="))
            {
                try
                {
                    connection.Open();

                    // Query to find the maximum ID in the MReservations table
                    string query = "SELECT MAX(Id) FROM HotelDB.MReservations";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            string maxId = result.ToString();

                            // Extract the numeric part of the ID and increment it
                            if (int.TryParse(maxId.Substring(1), out int maxIdNumeric))
                            {
                                nextId = maxIdNumeric + 1;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return nextId;
        }

        private void PopulateDataGridView()
        {

            // Add a method to populate the DataGridView with data

           


        }

        private void ReservationaDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {



        }

        private void ReservationaDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = ReservationaDataGridView1.Rows[e.RowIndex];

                txtId.Text = row.Cells[0].Value.ToString();
                txtFirstName.Text = row.Cells[1].Value.ToString();
                txtAddress.Text = row.Cells[2].Value.ToString();
                txtMobileNo.Text = row.Cells[3].Value.ToString();
                txtReservatioDate.Text = row.Cells[4].Value.ToString();
                txtNoOfPax.Text = row.Cells[5].Value.ToString();
                txtCost.Text = row.Cells[6].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
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

                    
                    string updateQuery = "UPDATE HotelDB.MReservations SET  FirstName =@FirstName, Address =@Address, MobileNo =@MobileNo," +
                        " ReservationDate =@ReservationDate, NoOfPax =@NoOfPax, TotalCost =@TotalCost WHERE Id =@Id";

                    MySqlCommand cmdUpdate = new MySqlCommand(updateQuery, connection);
                    cmdUpdate.Parameters.AddWithValue("@Id", txtId.Text);
                    cmdUpdate.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                    cmdUpdate.Parameters.AddWithValue("@Address", txtAddress.Text);
                    cmdUpdate.Parameters.AddWithValue("@MobileNo", txtMobileNo.Text);
                    cmdUpdate.Parameters.AddWithValue("@ReservationDate", txtReservatioDate.Text);
                    cmdUpdate.Parameters.AddWithValue("@NoOfPax", txtNoOfPax.Text);
                    cmdUpdate.Parameters.AddWithValue("@TotalCost", txtCost.Text);
                

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

        private void btnDelete_Click(object sender, EventArgs e)
        {
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

                        string deleteQuery = "DELETE FROM HotelDB.MReservations WHERE Id = @Id";

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
            txtFirstName.Clear();
            txtAddress.Clear();
            txtMobileNo.Clear();
            txtReservatioDate.Clear(); // Clear the selected date
            txtNoOfPax.Clear();
            txtCost.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            string searchName = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchName))
            {
                MessageBox.Show("Please enter a Id to search.", "Error");
                PopulateDataGridView();
                return;

            }

            using (MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password="))
            {
                try
                {
                    connection.Open();

                    string searchQuery = "SELECT * FROM HotelDB.MReservations WHERE Id LIKE @Id";

                    MySqlCommand cmdSearch = new MySqlCommand(searchQuery, connection);
                    cmdSearch.Parameters.AddWithValue("@Id", "%" + searchName + "%");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmdSearch);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        ReservationaDataGridView1.DataSource = dataTable;
                    }
                    else
                    {
                        MessageBox.Show("No matching records found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ReservationaDataGridView1.DataSource = null; // Clear the DataGridView
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }
    }
}
