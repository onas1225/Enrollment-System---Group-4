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

namespace Enrollment_Form___Group_4
{
    public partial class Form7: Form
    {
        public Form7(string strand, string firstName, string middleName, string lastName, string sex, DateTime birthday,
                 string studentNumber, string placeOfBirth, string religion, int age,
                 string motherName, string motherNumber, string motherOccupation,
                 string fatherName, string fatherNumber, string fatherOccupation,
                 bool birthCert, bool reportCard, bool picture, string previousSchool)
        {
            InitializeComponent();

            Strand.Text = strand;
            FirstNameTxtBox.Text = firstName;
            MiddleNameTxtBox.Text = middleName;
            LastNameTxtBox.Text = lastName;

            if (sex == "Male") Male.Checked = true;
            else Female.Checked = true;

            Birthday.Value = birthday;
            StudentNum.Text = studentNumber;
            PlaceofBirth.Text = placeOfBirth;
            Religion.Text = religion;
            Age.Text = age.ToString();

            textBox1.Text = motherName;
            textBox2.Text = motherNumber;
            textBox3.Text = motherOccupation;
            textBox4.Text = fatherName;
            textBox5.Text = fatherNumber;
            textBox6.Text = fatherOccupation;

            BirthCerti.Checked = birthCert;
            ReportCard.Checked = reportCard;
            twobytwopic.Checked = picture;

            PreviousScho.Text = previousSchool;
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form6 adminForm = new Form6();
            adminForm.Show();
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            string query = "UPDATE enrollment SET Strand = @Strand, FirstName =@FirstName, MiddleName = @MiddleName, LastName = @LastName, Sex = @Sex, Birthday = @Birthday, " +
                  "StudentNumber = @StudentNumber, PlaceOfBirth = @PlaceOfBirth, Religion = @Religion, Age = @Age, " +
                  "MotherFullName = @MotherFullName, MotherNumber = @MotherNumber, MotherOccupation = @MotherOccupation, " +
                  "FatherFullName = @FatherFullName, FatherNumber = @FatherNumber, FatherOccupation = @FatherOccupation, " +
                  "BirthCertificate = @BirthCertificate, ReportCard = @ReportCard, TwoByTwoPicture = @TwoByTwoPicture, PreviousSchool = @PreviousSchool " +
                  "WHERE StudentNumber = @StudentNumber";

            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-CVS0J8H\\SQLEXPRESS;Initial Catalog=enrollment;Integrated Security=True;TrustServerCertificate=True"))
            {
                conn.Open();
                using (SqlCommand strandcommand = new SqlCommand(query, conn))
                {
                    strandcommand.Parameters.AddWithValue("@Strand", Strand.Text);
                    strandcommand.Parameters.AddWithValue("@FirstName", FirstNameTxtBox.Text);
                    strandcommand.Parameters.AddWithValue("@MiddleName", MiddleNameTxtBox.Text);
                    strandcommand.Parameters.AddWithValue("@LastName", LastNameTxtBox.Text);
                    strandcommand.Parameters.AddWithValue("@Sex", Male.Checked ? "Male" : "Female");
                    strandcommand.Parameters.AddWithValue("@Birthday", Birthday.Value);
                    strandcommand.Parameters.AddWithValue("@StudentNumber", StudentNum.Text);
                    strandcommand.Parameters.AddWithValue("@PlaceOfBirth", PlaceofBirth.Text);
                    strandcommand.Parameters.AddWithValue("@Religion", Religion.Text);
                    strandcommand.Parameters.AddWithValue("@Age", int.Parse(Age.Text));

                    strandcommand.Parameters.AddWithValue("@MotherFullName", textBox1.Text);
                    strandcommand.Parameters.AddWithValue("@MotherNumber", textBox2.Text);
                    strandcommand.Parameters.AddWithValue("@MotherOccupation", textBox3.Text);
                    strandcommand.Parameters.AddWithValue("@FatherFullName", textBox4.Text);
                    strandcommand.Parameters.AddWithValue("@FatherNumber", textBox5.Text);
                    strandcommand.Parameters.AddWithValue("@FatherOccupation", textBox6.Text);

                    strandcommand.Parameters.AddWithValue("@BirthCertificate", BirthCerti.Checked);
                    strandcommand.Parameters.AddWithValue("@ReportCard", ReportCard.Checked);
                    strandcommand.Parameters.AddWithValue("@TwoByTwoPicture", twobytwopic.Checked);

                    strandcommand.Parameters.AddWithValue("@PreviousSchool", PreviousScho.Text);

                    strandcommand.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Student record updated successfully!");
            this.Close();
        }
    }
}
