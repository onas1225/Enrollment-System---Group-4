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
    public partial class Form2 : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-CVS0J8H\SQLEXPRESS;Initial Catalog=login;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
        public Form2()
        {
            InitializeComponent();
        }

        private void AlreadyAcc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 sForm = new Form1();
            sForm.Show();
            this.Hide();
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 loginForm = new Form1();
            loginForm.Show();
        }

        private void SignUpBtn_Click(object sender, EventArgs e)
        {
            if (EmailTxtBox.Text == "" || PassTxtBox.Text == "")
            {
                MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connect.State != ConnectionState.Open)
                {
                    try
                    {
                        connect.Open();

                        String checkUsername = "SELECT * FROM login WHERE Username = @Username";
                        using (SqlCommand checkUser = new SqlCommand(checkUsername, connect))
                        {
                            checkUser.Parameters.AddWithValue("@Username", EmailTxtBox.Text.Trim());

                            SqlDataAdapter adapter = new SqlDataAdapter(checkUser);
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            if (table.Rows.Count >= 1)
                            {
                                MessageBox.Show(EmailTxtBox.Text + " already exists", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                string insertData = "INSERT INTO login (username, pass) VALUES(@Username, @Pass)";
                                using (SqlCommand sql = new SqlCommand(insertData, connect))
                                {
                                    sql.Parameters.AddWithValue("@Username", EmailTxtBox.Text.Trim());
                                    sql.Parameters.AddWithValue("@Pass", PassTxtBox.Text.Trim());

                                    sql.ExecuteNonQuery();

                                    MessageBox.Show("Registered successfully", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    Form1 lForm = new Form1();
                                    lForm.Show();
                                    this.Hide();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error connecting Database: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
        }
        private void ShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowPass.Checked)
            {
                PassTxtBox.PasswordChar = '\0';
            }
            else
            {
                PassTxtBox.PasswordChar = '•';
            }
        }
    }
}