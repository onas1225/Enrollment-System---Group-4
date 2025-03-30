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
    public partial class Form3 : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-CVS0J8H\SQLEXPRESS;Initial Catalog=enrollment;Integrated Security=True;TrustServerCertificate=True");
        public Form3()
        {
            InitializeComponent();
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form4 HomeForm = new Form4();
            HomeForm.Show();
        }

        private void Male_CheckedChanged(object sender, EventArgs e)
        {
            if (Male.Checked)
            {
                Female.Checked = false;
            }
        }

        private void Female_CheckedChanged(object sender, EventArgs e)
        {
            if (Female.Checked)
            {
                Male.Checked = false;
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            {
                if (Strand.SelectedIndex == -1 || FirstNameTxtBox.Text == "" || MiddleNameTxtBox.Text == "" ||
                LastNameTxtBox.Text == "" || StudentNum.Text == "" || PlaceofBirth.Text == "" ||
                Religion.Text == "" || Age.Text == "" || textBox1.Text == "" || textBox2.Text == "" ||
                textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" ||
                PreviousScho.Text == "" || (!Male.Checked && !Female.Checked) ||
                !BirthCerti.Checked || !ReportCard.Checked || !twobytwopic.Checked)
                {
                    MessageBox.Show("Please fill all blank fields and check all required checkboxes", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string selectedSex = Male.Checked ? "Male" : "Female";
                string selectedBirthday = Birthday.Value.ToString("yyyy-MM-dd");

                try
                {
                    connect.Open();
                    string checkStudent = "SELECT * FROM enrollment WHERE StudentNumber = @StudentNumber";

                    using (SqlCommand checkUser = new SqlCommand(checkStudent, connect))
                    {
                        checkUser.Parameters.AddWithValue("@StudentNumber", StudentNum.Text.Trim());

                        SqlDataAdapter adapter = new SqlDataAdapter(checkUser);
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        if (table.Rows.Count >= 1)
                        {
                            MessageBox.Show("Student Number " + StudentNum.Text + " already exists", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    string insertData = "INSERT INTO enrollment (Strand, FirstName, MiddleName, LastName, Sex, Birthday, StudentNumber, PlaceofBirth, Religion, Age, PreviousSchool, " +
                        "MotherFullName, MotherNumber, MotherOccupation, FatherFullName, FatherNumber, FatherOccupation, BirthCertificate, ReportCard, TwoByTwoPicture) " +
                        "VALUES(@Strand, @FirstName, @MiddleName, @LastName, @Sex, @Birthday, @StudentNumber, @PlaceofBirth, @Religion, @Age, @PreviousSchool, " +
                        "@MotherFullName, @MotherNumber, @MotherOccupation, @FatherFullName, @FatherNumber, @FatherOccupation, @BirthCertificate, @ReportCard, @TwoByTwoPicture)";

                    using (SqlCommand Insert = new SqlCommand(insertData, connect))
                    {
                        Insert.Parameters.AddWithValue("@Strand", Strand.SelectedItem.ToString());
                        Insert.Parameters.AddWithValue("@FirstName", FirstNameTxtBox.Text.Trim());
                        Insert.Parameters.AddWithValue("@MiddleName", MiddleNameTxtBox.Text.Trim());
                        Insert.Parameters.AddWithValue("@LastName", LastNameTxtBox.Text.Trim());
                        Insert.Parameters.AddWithValue("@Sex", selectedSex);
                        Insert.Parameters.AddWithValue("@Birthday", selectedBirthday);
                        Insert.Parameters.AddWithValue("@StudentNumber", StudentNum.Text.Trim());
                        Insert.Parameters.AddWithValue("@PlaceofBirth", PlaceofBirth.Text.Trim());
                        Insert.Parameters.AddWithValue("@Religion", Religion.Text.Trim());
                        Insert.Parameters.AddWithValue("@Age", Age.Text.Trim());
                        Insert.Parameters.AddWithValue("@PreviousSchool", PreviousScho.Text.Trim());
                        Insert.Parameters.AddWithValue("@MotherFullName", textBox1.Text.Trim());
                        Insert.Parameters.AddWithValue("@MotherNumber", textBox2.Text.Trim());
                        Insert.Parameters.AddWithValue("@MotherOccupation", textBox3.Text.Trim());
                        Insert.Parameters.AddWithValue("@FatherFullName", textBox4.Text.Trim());
                        Insert.Parameters.AddWithValue("@FatherNumber", textBox5.Text.Trim());
                        Insert.Parameters.AddWithValue("@FatherOccupation", textBox6.Text.Trim());
                        Insert.Parameters.AddWithValue("@BirthCertificate", BirthCerti.Checked ? 1 : 0);
                        Insert.Parameters.AddWithValue("@ReportCard", ReportCard.Checked ? 1 : 0);
                        Insert.Parameters.AddWithValue("@TwoByTwoPicture", twobytwopic.Checked ? 1 : 0);

                        Insert.ExecuteNonQuery();
                        MessageBox.Show("Registered successfully", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Form4 hForm = new Form4();
                        hForm.Show();
                        this.Hide();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error connecting to Database: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }
        }
    }
}


