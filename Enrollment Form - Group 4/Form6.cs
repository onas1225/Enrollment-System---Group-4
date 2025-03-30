using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enrollment_Form___Group_4
{
    public partial class Form6 : Form
    {
        private string Connect = "Data Source=DESKTOP-CVS0J8H\\SQLEXPRESS;Initial Catalog=enrollment;Integrated Security=True;TrustServerCertificate=True";

        public Form6()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(Connect))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM enrollment", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;

                    dataGridView1.ReadOnly = true;
                    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch (SqlException sqlEx)
                {
                    MessageBox.Show("Database Error: " + sqlEx.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Go back to Admin Page?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                Form5 adminForm = new Form5();
                adminForm.Show();
            }
        }

        private void SearchStudents()
        {
            using (SqlConnection conn = new SqlConnection(Connect))
            {
                try
                {
                    conn.Open();
                    string searchText = SearchBar.Text.Trim();

                    string query = "SELECT StudentNumber, " +
                                   "FirstName, MiddleName, LastName, " +
                                   "Strand, Sex, Birthday, PlaceOfBirth, Religion, Age, " +
                                   "MotherFullName, MotherNumber, MotherOccupation, " +
                                   "FatherFullName, FatherNumber, FatherOccupation, " +
                                   "PreviousSchool, BirthCertificate, ReportCard, TwoByTwoPicture " +
                                   "FROM enrollment " +
                                   "WHERE FirstName LIKE @searchText " +
                                   "OR MiddleName LIKE @searchText " +
                                   "OR LastName LIKE @searchText " +
                                   "OR StudentNumber LIKE @searchText " +
                                   "OR Strand LIKE @searchText";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@searchText", "%" + searchText + "%");
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error searching students: " + ex.Message);
                }
            }
        }

        private void SearchBar_TextChanged(object sender, EventArgs e)
        {
            SearchStudents();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                if (e.ColumnIndex == dataGridView1.Columns["Edit"].Index)
                {
                    EditRow(e.RowIndex);
                }
                else if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index)
                {
                    DeleteRow(e.RowIndex);
                }
                
            }
        }

        private void EditRow(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow row = dataGridView1.Rows[rowIndex];

                string strand = row.Cells["Strand"].Value?.ToString() ?? "";
                string firstName = row.Cells["FirstName"].Value?.ToString() ?? "";
                string middleName = row.Cells["MiddleName"].Value?.ToString() ?? "";
                string lastName = row.Cells["LastName"].Value?.ToString() ?? "";
                string sex = row.Cells["Sex"].Value?.ToString() ?? "";

                DateTime birthday;
                if (!DateTime.TryParse(row.Cells["Birthday"].Value?.ToString(), out birthday))
                {
                    birthday = DateTime.MinValue;
                }

                string studentNumber = row.Cells["StudentNumber"].Value?.ToString() ?? "";
                string placeOfBirth = row.Cells["PlaceOfBirth"].Value?.ToString() ?? "";
                string religion = row.Cells["Religion"].Value?.ToString() ?? "";

                int age;
                if (!int.TryParse(row.Cells["Age"].Value?.ToString(), out age))
                {
                    age = 0;
                }

                string motherName = row.Cells["MotherFullName"].Value?.ToString() ?? "";
                string motherNumber = row.Cells["MotherNumber"].Value?.ToString() ?? "";
                string motherOccupation = row.Cells["MotherOccupation"].Value?.ToString() ?? "";
                string fatherName = row.Cells["FatherFullName"].Value?.ToString() ?? "";
                string fatherNumber = row.Cells["FatherNumber"].Value?.ToString() ?? "";
                string fatherOccupation = row.Cells["FatherOccupation"].Value?.ToString() ?? "";

                bool birthCert = row.Cells["BirthCertificate"].Value?.ToString() == "True";
                bool reportCard = row.Cells["ReportCard"].Value?.ToString() == "True";
                bool picture = row.Cells["TwoByTwoPicture"].Value?.ToString() == "True";

                string previousSchool = row.Cells["PreviousSchool"].Value?.ToString() ?? "";

                Form7 editForm = new Form7(
                    strand, firstName, middleName, lastName, sex, birthday,
                    studentNumber, placeOfBirth, religion, age,
                    motherName, motherNumber, motherOccupation,
                    fatherName, fatherNumber, fatherOccupation,
                    birthCert, reportCard, picture, previousSchool
                );

                editForm.ShowDialog();
            }
        }


        private void DeleteRow(int rowIndex)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this row?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string studentNumber = dataGridView1.Rows[rowIndex].Cells["StudentNumber"].Value.ToString();
                DeleteFromDatabase(studentNumber);
                dataGridView1.Rows.RemoveAt(rowIndex);
                MessageBox.Show("Row deleted successfully.");
            }
        }

        private void DeleteFromDatabase(string studentNumber)
        {
            string query = "DELETE FROM enrollment WHERE StudentNumber = @StudentNumber";
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-CVS0J8H\\SQLEXPRESS;Initial Catalog=enrollment;Integrated Security=True;TrustServerCertificate=True"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentNumber", studentNumber);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}